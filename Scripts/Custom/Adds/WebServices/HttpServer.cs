using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace Server.WebServices
{
    class HttpServer
    {
        private static HttpServer Server;
        private readonly int Port = 2595;

        protected HttpListener Listener;
        protected bool IsStarted = false;

        private List<Handler> Handlers;      

        public static void Initialize()
        {
            EventSink.ServerStarted += new ServerStartedEventHandler(ServerStarted);
            EventSink.Shutdown += new ShutdownEventHandler(Shutdown);
        }

        private static void ServerStarted()
        {
//            Server = new HttpServer();
//            Server.AddHandler(new VendorItems());
//            Server.AddHandler(new OnlinePlayers());
//            Server.Start();
        }

        private static void Shutdown(ShutdownEventArgs args)
        {
            if (Server != null)
            {
                Server.Stop();
            }         
        }

        public HttpServer()
        {
            Handlers = new List<Handler>();
        }

        private void AddHandler(Handler handler)
        {
            Handlers.Add(handler);
        }

        public void Start()
        {
            this.Listener = new HttpListener();

            foreach (Handler handler in Handlers)
            {
                this.Listener.Prefixes.Add("http://*:" + Port + "/" + handler.Path + "/");
            }

            this.IsStarted = true;
            this.Listener.Start();

            try
            {
                this.Listener.BeginGetContext(new AsyncCallback(WebRequestCallback), this.Listener);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in HttpServer: " + e.Message);
            }
        }

        public void Stop()
        {
            if (Handlers != null)
            {
                foreach (Handler handler in Handlers)
                {
                    handler.Shutdown();
                }
                Handlers.Clear();
            }
            if (this.Listener != null)
            {
                this.Listener.Close();
                this.Listener = null;
                this.IsStarted = false;
            }
        }

        protected void WebRequestCallback(IAsyncResult result)
        {
            try
            {
                HttpListenerContext context = this.Listener.EndGetContext(result);
                this.Listener.BeginGetContext(new AsyncCallback(WebRequestCallback), this.Listener);
                this.ProcessRequest(context);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in HttpServer: " + e.Message);
            }
        }

        protected virtual void ProcessRequest(HttpListenerContext Context)
        {
            HttpListenerRequest request = Context.Request;
            HttpListenerResponse response = Context.Response;
            Handler requestHandler = null;
            foreach (Handler handler in Handlers)
            {
                if (handler.CanHandle(request))
                {
                    requestHandler = handler;
                    break;
                }
            }
            if (requestHandler != null)
            {
                requestHandler.Handle(request, response);
            }
            else
            {
                response.StatusCode = 404;
                response.Close();
            }
        }
    }
}
