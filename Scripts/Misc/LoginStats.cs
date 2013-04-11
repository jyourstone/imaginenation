using Server.Network;
using Server.Gumps;
using System.Collections.Generic;
using Server.Mobiles;
using System;

namespace Server.Misc
{
	public class LoginStats
	{
		public static void Initialize()
		{
			// Register our event handler
			EventSink.Login += EventSink_Login;
		}

		private static void EventSink_Login( LoginEventArgs args )
		{
			int userCount = NetState.Instances.Count;
			int itemCount = World.Items.Count;
			int mobileCount = World.Mobiles.Count;
            int count = 0;

            Mobile owner = args.Mobile;

            List<NetState> states = NetState.Instances;

            for (int i = 0; i < states.Count; ++i)
            {
                Mobile m = states[i].Mobile;

                if (m != null && (m == owner || !m.Hidden || owner.AccessLevel >= m.AccessLevel || (m is PlayerMobile && ((PlayerMobile)m).VisibilityList.Contains(owner))))
                {
                    ++count;
                }
            }

            owner.SendMessage("Welcome {0}! There {1} currently {2} user{3} online and {4} item{5} in the world.",
                args.Mobile.Name,
                count == 1 ? "is" : "are",
                count, count == 1 ? "" : "s",
                itemCount, itemCount == 1 ? "" : "s");
        }
	}
}