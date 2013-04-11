using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace Server.WebServices
{
    public abstract class Handler
    {
        public static JavaScriptSerializer m_JavascriptSerializer = new JavaScriptSerializer();
        public static System.Text.ASCIIEncoding m_Encoding = new System.Text.ASCIIEncoding();

        public string Path { get; set; }

        /*
         * Return true if this handler can handle the request */
        public abstract bool CanHandle(HttpListenerRequest request);

        /*
         * Handle the request. Expected to close the response */
        public abstract void Handle(HttpListenerRequest request, HttpListenerResponse response);

        /*
         * Perform any cleanup, reclaim memory. Called on server shutdown. 
           Children should override this if needed. */
        public virtual void Shutdown() { }

        /*
         * Added cache headers for the expiration time */
        protected void SetCacheHeaders(WebHeaderCollection headers, TimeSpan expires)
        {
            DateTime now = DateTime.Now;
            DateTime nextUpdate = now + expires;
            headers["Cache-Control"] = "public, max-age=" + expires.TotalSeconds;
            headers["Expires"] = nextUpdate.ToLongDateString() + " " + nextUpdate.ToLongTimeString();
            headers["Last-Modified"] = now.ToLongDateString() + " " + now.ToLongTimeString();
        }

        /*
         * Prepare Output Stream, using gzip if supported */
        protected Stream PrepareOutputStream(HttpListenerRequest request, HttpListenerResponse response)
        {
            string accept = request.Headers.Get("Accept-Encoding");
            Stream outputStream = response.OutputStream;
            if (accept != null && accept.Contains("gzip"))
            {
                response.AddHeader("Content-Encoding", "gzip");
                outputStream = new GZipStream(response.OutputStream, CompressionMode.Compress);
            }
            return outputStream;
        }

        internal void SendResponse(Stream outputStream, Object obj, string callback)
        {
            StringBuilder sb = new StringBuilder();
            m_JavascriptSerializer.Serialize(obj, sb);
            if (callback != null && callback.Trim().Length > 0)
            {
                sb.Insert(0, "(");
                sb.Insert(0, callback);
                sb.Append(");");
            }
            outputStream.Write(m_Encoding.GetBytes(sb.ToString()), 0, sb.Length);
            outputStream.Close();
        }
    }
}
