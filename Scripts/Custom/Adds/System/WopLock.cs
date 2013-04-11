using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server
{
    public class WopLock
    {
        private const int TIME_SPAN = 4;  // In seconds
        private const int ALLOWED_WOPS = 8;

        public List<DateTime> WopList = new List<DateTime>();

        // Check if player can use wop
        public bool CanWop (PlayerMobile from, bool IsWop)
        {
            if (from.WopLock == null)
                from.WopLock = new WopLock();

            if(from.WopLock.WopList.Count == 0)
            {
                from.WopLock.UsedWop(from);
                return true;
            }

            TimeSpan TimePassed = (DateTime.Now.Subtract(from.WopLock.WopList[0]));

            if ((TimePassed.Subtract(TimeSpan.FromSeconds(TIME_SPAN)).TotalSeconds < 0 && (from.WopLock.WopList.Count >= ALLOWED_WOPS)))
            {
                if (IsWop)
                    from.SendAsciiMessage("You can't use .wop that often in such a short time!");
                else
                    from.SendAsciiMessage("Your speech was filtered due to spamming.");
                return false;
            }
            from.WopLock.UsedWop(from);
            return true;
        }

        // Player used wop
        public void UsedWop(PlayerMobile from)
        {
            WopList.Add(DateTime.Now);
            if (WopList.Count > ALLOWED_WOPS)
                WopList.RemoveAt(0);
        }
    }
}
