using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

using Server.Mobiles;
using Server.Network;
using Server.Util;

namespace Server.WebServices
{
    class OnlinePlayers : Handler
    {
        public OnlinePlayers()
        {
            Path = "onlinePlayers";
        }

        public override bool CanHandle(HttpListenerRequest request)
        {
            if (request.HttpMethod != "GET")
            {
                return false;
            }
            return request.RawUrl.StartsWith("/" + Path);
        }

        public override void Handle(HttpListenerRequest request, HttpListenerResponse response)
        {
            String startStr = StringUtils.GetString(request.QueryString.Get("start"), "0");
            String limitStr = StringUtils.GetString(request.QueryString.Get("limit"), "25");
            String callback = request.QueryString.Get("callback");

            int start = 0, limit = 25;
            int.TryParse(startStr, out start);
            int.TryParse(limitStr, out limit);
            if (limit == 0 || limit > 25)
            {
                limit = 25;
            }
            TimeSpan expires = TimeSpan.FromMinutes(5.0);
            response.ContentType = "application/json";

            SetCacheHeaders(response.Headers, expires);
            Stream outputStream = PrepareOutputStream(request, response);

            List<Mobile> list = new List<Mobile>();
            List<NetState> states = NetState.Instances;
            List<Player> players = new List<Player>();
            for (int i = 0; i < states.Count; ++i)
            {
                Mobile m = states[i].Mobile;

                if (m != null && (m.AccessLevel == AccessLevel.Player || !m.Hidden))
                    list.Add(m);
            }

            int end = start + limit, totalCount = list.Count;
            if (start > list.Count || list.Count == 0)
            {
                sendResponse(outputStream, players, callback, totalCount);
                return;
            }
            if (end > list.Count)
            {
                end = list.Count;
            }
            list = list.GetRange(start, end - start);
            foreach (Mobile m in list)
            {
                PlayerMobile pm = m as PlayerMobile;
                Player p = new Player();
                p.Name = pm.Name;
                p.Kills = pm.Kills;
                p.Location = pm.Region != null ? pm.Region.Name + pm.Location.ToString() : pm.Location.ToString();
                p.GameTime = pm.GameTime.Days + (pm.GameTime.Days != 1 ? " days" : " day");
                players.Add(p);
            }
            sendResponse(outputStream, players, callback, totalCount);

            response.StatusCode = 200;
            response.StatusDescription = "OK";
            response.Close();
        }

        private void sendResponse(Stream outputStream, List<Player> subList, string callback, int totalCount)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("success", true);
            result.Add("count", totalCount);
            result.Add("data", subList);
            base.SendResponse(outputStream, result, callback);
        }

        class Player
        {
            public string Name { get; set; }
            public int Kills { get; set; }
            public string Location { get; set; }
            public string GameTime { get; set; }
        }
    }
}
