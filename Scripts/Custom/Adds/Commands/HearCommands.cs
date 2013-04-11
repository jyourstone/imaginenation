using System;
using System.Collections.Generic;

namespace Server.Commands
{
    public class HearCommands
    {
        private static readonly Dictionary<Mobile, bool> m_HearCommands = new Dictionary<Mobile, bool>();

        public static void DisplayCommand(Mobile from, string command)
        {
            List<Mobile> remove = null;
            List<Mobile> mobs = new List<Mobile>();

            foreach (Mobile mob in m_HearCommands.Keys)
                mobs.Add(mob);

            for (int i = 0; i < mobs.Count; i++)
            {
                if (mobs[i].NetState == null)
                {
                    if (remove == null)
                        remove = new List<Mobile>();
                    remove.Add(mobs[i]);
                }
                else
                {
                    bool hearplayers;
                    m_HearCommands.TryGetValue(mobs[i], out hearplayers);

                    //Don't hear your own commands
                    if (from == mobs[i])
                        continue;

                    //Don't display player commands unless enabled.
                    if (!hearplayers && from.AccessLevel == AccessLevel.Player)
                        continue;

                    //Don't display staff commands with higher or equal privs
                    if (from.AccessLevel >= mobs[i].AccessLevel)
                        continue;

                    mobs[i].SendMessage(435, command);
                }
            }

            if (remove != null)
                for (int i = 0; i < remove.Count; i++)
                    m_HearCommands.Remove(remove[i]);
        }

        public static object Format(object o)
        {
            if (o is Item)
            {
                Item item = (Item)o;

                return String.Format("0x{0:X} ({1})", item.Serial.Value, item.GetType().Name);
            }

            return o;
        }

        public static void Initialize()
        {
            CommandSystem.Register("HearCommands", AccessLevel.Owner, HearCommands_OnCommand);
        }

        [Usage("HearCommands <noplayers>")]
        [Description("See what commands everyone writes (except g, tele and opendoor). Use <noplayers> to block player commands.")]
        private static void HearCommands_OnCommand(CommandEventArgs args)
        {
            if (m_HearCommands.ContainsKey(args.Mobile))
            {
                m_HearCommands.Remove(args.Mobile);
                args.Mobile.SendMessage("Hear Commands disabled.");
            }
            else
            {
                if (args.Length == 1)
                {
                    if (args.GetString(0).ToLower() == "noplayers")
                    {
                        m_HearCommands.Add(args.Mobile, false);
                        args.Mobile.SendMessage("Hear Commands enabled without player commands, type .hearcommands again to disable it.");
                    }
                    else
                    {
                        args.Mobile.SendMessage("Format:");
                        args.Mobile.SendMessage("HearCommands OR HearCommands noplayers");
                    }
                }
                else
                {
                    m_HearCommands.Add(args.Mobile, true);
                    args.Mobile.SendMessage("Hear Commands enabled with playercommands, type .hearcommands again to disable it.");
                    args.Mobile.SendMessage("To enable Hear Commands without player commands, type '.hearcommands noplayers'");
                }
            }
        }
    }
}