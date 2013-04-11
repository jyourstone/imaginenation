using System.Collections.Generic;
using Server.Network;

namespace Server.Commands
{
    public class Cycle
    {
        private static readonly Dictionary<Serial, List<Serial>> visited = new Dictionary<Serial, List<Serial>>();

        public static void Initialize()
        {
            CommandSystem.Register("Cycle", AccessLevel.Counselor, Cycle_OnCommand);
            CommandSystem.Register("ClearCycle", AccessLevel.Counselor, ClearCycle_OnCommand);
        }

        [Usage("Cycle")]
        [Description("Goes through the online players one at a time. Use .clearcycle to start from the beginning (will happen automatically when you have gone through the list).")]
        private static void Cycle_OnCommand(CommandEventArgs e)
        {
            List<Mobile> mobs = new List<Mobile>();
            List<NetState> states = NetState.Instances;
            for (int i = 0; i < states.Count; ++i)
            {
                Mobile m = states[i].Mobile;

                if (m == null)
                    continue;

                if (m.AccessLevel > AccessLevel.Player)
                    continue;

                mobs.Add(states[i].Mobile);
            }

            if (mobs.Count < 1)
            {
                e.Mobile.SendAsciiMessage("There is noone to go to");
                return;
            }

            if (!visited.ContainsKey(e.Mobile.Serial))
                visited.Add(e.Mobile.Serial, null);

            if (visited[e.Mobile.Serial] == null)
                visited[e.Mobile.Serial] = new List<Serial>();

            int count = 0;
            for (int i = 0; i < mobs.Count; ++i)
            {
                if (visited[e.Mobile.Serial].Contains(mobs[i].Serial))
                {
                    count++;
                    continue;
                }

                e.Mobile.Map = mobs[i].Map;
                e.Mobile.Location = mobs[i].Location;
                e.Mobile.SendAsciiMessage(1944, "Going to: " + mobs[i].Name);

                visited[e.Mobile.Serial].Add(mobs[i].Serial);

                break;
            }

            if (count >= mobs.Count)
            {
                e.Mobile.SendAsciiMessage(1171, "You went through all online players.");
                visited[e.Mobile.Serial].Clear();
            }
        }

        [Usage("ClearCycle")]
        [Description("Clears the current cycle.")]
        private static void ClearCycle_OnCommand(CommandEventArgs e)
        {
            if (visited.ContainsKey(e.Mobile.Serial) && visited[e.Mobile.Serial] != null)
                visited[e.Mobile.Serial].Clear();

            e.Mobile.SendAsciiMessage("You cleared the cycle list");
        }
    }
}