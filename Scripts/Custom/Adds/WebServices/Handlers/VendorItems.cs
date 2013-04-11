using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Caching;

using Server.Items;
using Server.Mobiles;
using Server.Regions;
using Server.Util;

namespace Server.WebServices
{
    class VendorItems : Handler
    {
        private static List<MyVendorItem> m_VendorItems = new List<MyVendorItem>();

        private static TimeSpan m_WorldSaveInterval = TimeSpan.FromMinutes(30.0);
        private static DateTime m_LastVendorUpdate;
        private static DateTime m_NextVendorUpdate;

        private static Cache m_ItemCache = HttpRuntime.Cache;

        public static void Initialize()
        {
            EventSink.ServerStarted += new ServerStartedEventHandler(LoadPlayerVendors);
            EventSink.WorldSave += new WorldSaveEventHandler(UpdatePlayerVendors);
        }

        public static void LoadPlayerVendors()
        {
            UpdatePlayerVendors(null);
        }

        public static void UpdatePlayerVendors(WorldSaveEventArgs e)
        {
            m_VendorItems.Clear();
            int count = 0;
            foreach (Mobile mob in World.Mobiles.Values)
            {
                if (mob is PlayerVendor)
                {
                    PlayerVendor vendor = mob as PlayerVendor;
                    if (vendor.Owner != null)
                    {
                        processContainer(vendor, vendor.Backpack);
                        count++;
                    }
                }
            }
            m_LastVendorUpdate = DateTime.Now;
            m_NextVendorUpdate = m_LastVendorUpdate + m_WorldSaveInterval;
            Console.WriteLine("Caching " + count + " Player Vendors");
        }

        private static void processContainer(PlayerVendor vendor, Container container)
        {
            foreach (Item i in container.Items)
            {
                VendorItem vi = vendor.GetVendorItem(i);
                if (vi != null)
                {
                    if (vi.IsForSale)
                    {
                        MyVendorItem mvi = new MyVendorItem();
                        mvi.Name = vi.Item is BaseContainer ? "container" : StringUtils.GetString(vi.Item.Name, Sphere.ComputeName(vi.Item));
                        mvi.Description = vi.Description;
                        mvi.Price = vi.Price;
                        if (vi.Item is CommodityDeed)
                        {
                            Item commodity = (vi.Item as CommodityDeed).Commodity;
                            if (commodity == null) // skip empty deeds
                                continue;
                            mvi.Amount = commodity.Amount;
                            mvi.Name += " - " + Sphere.ComputeName(commodity);
                        }
                        else
                        {
                            mvi.Amount = vi.Item.Amount;
                        }
                        if (mvi.Amount != 0)
                        {
                            mvi.PricePer = mvi.Price / mvi.Amount;
                        }
                        else
                        {
                            mvi.PricePer = mvi.Price;
                        }
                        mvi.VendorName = vendor.Name;
                        mvi.OwnerName = vendor.Owner.Name;
                        mvi.Location = StringUtils.GetString(vendor.Region.Name, BaseRegion.GetRuneNameFor(vendor.Region)) + " " + vendor.Location.X + "," + vendor.Location.Y;
                        m_VendorItems.Add(mvi);
                    }
                    else if (vi.Item is Container)
                    {
                        processContainer(vendor, vi.Item as Container);
                    }
                }
            }
        }

        public VendorItems()
        {
            Path = "vendorItems";
        }

        Func<MyVendorItem, bool> Contains(string q)
        {
            return x => x.VendorName.ToLower().Contains(q)
                        || x.OwnerName.ToLower().Contains(q)
                        || x.Description.ToLower().Contains(q)
                        || x.Name.ToLower().Contains(q);
        }

        Func<MyVendorItem, bool> NotContains(string q)
        {
            return x => !x.VendorName.ToLower().Contains(q)
                        && !x.OwnerName.ToLower().Contains(q)
                        && !x.Description.ToLower().Contains(q)
                        && !x.Name.ToLower().Contains(q);
        }

        public override void Handle(HttpListenerRequest request, HttpListenerResponse response)
        {
            TimeSpan expires = m_NextVendorUpdate - DateTime.Now;
            response.ContentType = "application/json";

            SetCacheHeaders(response.Headers, expires);
            Stream outputStream = PrepareOutputStream(request, response);
            QueryParams queryParams = new QueryParams(request);

            int start = queryParams.Start;
            int end = start + queryParams.Limit;
            List<MyVendorItem> subList;
            string query = queryParams.Query;
            if (query != null && query.Trim().Length != 0)
            {
                subList = m_ItemCache.Get(queryParams.Query) as List<MyVendorItem>;
                if (subList == null)
                {
                    string[] qps = query.Split(' ');
                    IEnumerable<MyVendorItem> results = null;
                    Func<MyVendorItem, bool> currentFunc = null;
                    //FIXME make this more dynamic/cleaner
                    for(int i = 0, j = qps.Length; i < j; i++)
                    {
                        string q = qps[i].Trim();
                        if(q.Length < 3) // ignore queries less than 3 characters
                        {
                            continue;
                        }
                        char first = q[0];
                        if (first == '-')
                        {
                            currentFunc = NotContains(q.Substring(1));
                        }
                        else if (first == '+')
                        {
                            currentFunc = Contains(q.Substring(1));
                        }
                        else
                        {
                            currentFunc = Contains(q);
                        }
                        results = results == null ? m_VendorItems.Where(currentFunc) : results.Where(currentFunc);
                    }
                    subList = results.ToList();
                    m_ItemCache.Add(query, subList, null, m_NextVendorUpdate, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                }
            }
            else
            {
                subList = m_VendorItems;
            }
            string sort = queryParams.Sort;
            QueryParams.SortDirection dir = queryParams.Direction;
            if (StringUtils.HasText(sort))
            {
                subList = dir.Equals(QueryParams.SortDirection.Ascending) ? subList.OrderBy(p => typeof(MyVendorItem).GetProperty(sort).GetValue(p, null)).ToList() : subList.OrderByDescending(p => typeof(MyVendorItem).GetProperty(sort).GetValue(p, null)).ToList();
            }
            int totalCount = subList.Count;
            if (start > subList.Count || subList.Count == 0)
            {
                sendResponse(outputStream, subList, queryParams.Callback, totalCount);
                return;
            }
            if (end > subList.Count)
            {
                end = subList.Count;
            }
            subList = subList.GetRange(start, end - start);
            sendResponse(outputStream, subList, queryParams.Callback, totalCount);

            response.StatusCode = 200;
            response.StatusDescription = "OK";
            response.Close();
        }

        public override void Shutdown()
        {
            if (m_VendorItems != null)
            {
                m_VendorItems.Clear();
            }
        }

        public override bool CanHandle(HttpListenerRequest request)
        {
            if (request.HttpMethod != "GET")
            {
                return false;
            }
            return request.RawUrl.StartsWith("/" + Path);
        }

        private void sendResponse(Stream outputStream, List<MyVendorItem> subList, string callback, int totalCount)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("success", true);
            result.Add("count", totalCount);
            result.Add("data", subList);
            base.SendResponse(outputStream, result, callback);
        }

        class MyVendorItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int Price { get; set; }
            public int Amount { get; set; }
            public string VendorName { get; set; }
            public string OwnerName { get; set; }
            public string Location { get; set; }
            public double PricePer { get; set; }
        }

        class QueryParams
        {

            public enum SortDirection { Ascending, Decending };
            static char[] matches = new char[] { '+', '-' };

            public int Start { get; set; }
            public int Limit { get; set; }
            public string Query { get; set; }
            public string Sort { get; set; }
            public SortDirection Direction { get; set; }
            public string Callback { get; set; }

            public QueryParams(HttpListenerRequest request)
            {
                int start, limit;
                if(!int.TryParse(request.QueryString.Get("start"), out start))
                {
                    start = 0;
                }
                Start = start;
                if (!int.TryParse(request.QueryString.Get("limit"), out limit) || limit > 30)
                {
                    limit = 30;
                }
                Limit = limit;
                String query = request.QueryString.Get("q");
                Query = StringUtils.HasText(query) ? query.ToLower() : null;
                Sort = StringUtils.GetString(request.QueryString.Get("sort"), "VendorName");
                String dir = request.QueryString.Get("dir");
                Direction = StringUtils.HasText(dir) ? (dir.ToLower().Equals("asc") ? SortDirection.Ascending : SortDirection.Decending) : SortDirection.Ascending;
                Callback = StringUtils.GetString(request.QueryString.Get("callback"), null);
            }

        }
    }
}
