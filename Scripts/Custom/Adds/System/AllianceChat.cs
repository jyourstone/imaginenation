/*using System;
using Server.Guilds;
using Server.Network;
using Server.Targeting;

namespace Server.Commands
{
    public class AllianceChat
    {
        private static int usercount;

        public static void Initialize()
        {
            CommandSystem.Register("A", AccessLevel.Player, A_OnCommand);
            CommandSystem.Register("ListenToAlliance", AccessLevel.GameMaster, ListenToAlliance_OnCommand);
        }

        public static void ListenToAlliance_OnCommand(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, ListenToAlliance_OnTarget);
            e.Mobile.SendMessage("Target a chaos or order player.");
        }

        public static void ListenToAlliance_OnTarget(Mobile from, object obj)
        {
            if (obj is Mobile)
            {
                var g = ((Mobile)obj).Guild as Guild;


                if (g == null)
                    from.SendMessage("They are not in a guild.");
                else if (g.Type == GuildType.Regular)
                    from.SendMessage("They are not in a chaos or order guild");
                else if (g.AllianceListeners.Contains(from))
                {
                    g.RemoveAllianceListener(from);
                    from.SendMessage("You are no longer listening to " + g.Name);
                }
                else
                {
                    g.AddAllianceListener(from);

                    from.SendMessage("You are now listening to " + g.Name);
                }
            }
        }

        [Usage("A [<text>|list]")]
        [Description("Broadcasts a message to all online alliance members.")]
        private static void A_OnCommand(CommandEventArgs e)
        {
            switch (e.ArgString.ToLower())
            {
                case "list":
                    List(e.Mobile);
                    break;
                default:
                    Msg(e);
                    break;
            }
        }

        private static void List(Mobile g)
        {
            usercount = 0;
            var GuildC = g.Guild as Guild;
            if (GuildC == null)
                g.SendMessage("You are not in a guild!");
            else if (GuildC.Type == GuildType.Regular)
                g.SendMessage("You are not in a chaos or order guild");
            else
            {
                foreach (var state in NetState.Instances)
                {
                    var m = state.Mobile;
                    if (m != null && m.Guild != null && !m.Guild.Disbanded && m.Guild.Type == GuildC.Type )
                    {
                        usercount++;
                    }
                }
                if (usercount == 1)
                {
                    g.SendMessage("There is 1 member of your alliance online.");
                }
                else
                {
                    g.SendMessage("There are {0} members of your alliance online.", usercount);
                }
                g.SendMessage("Online list:");
                foreach (var state in NetState.Instances)
                {
                    var m = state.Mobile;
                    if (m != null && m.Guild != null && !m.Guild.Disbanded && m.Guild.Type == GuildC.Type)
                    {
                        g.SendMessage(m.Name);
                    }
                }
            }
        }

        private static void Msg(CommandEventArgs e)
        {
            var from = e.Mobile;

            if (from.Squelched)
            {
                from.SendAsciiMessage("You can not say anything, you have been squelched.");
                return;
            }

            Guild GuildC = from.Guild as Guild;

            if (GuildC == null)
                from.SendMessage("You are not in a guild!");
            else if (GuildC.Type == GuildType.Regular)
                from.SendMessage("You are not in a chaos or order guild");
            else
            {
                string guild = from.Guild.Abbreviation == "none" ? from.Guild.Name : from.Guild.Abbreviation;
                EventSink.InvokeAllianceChat(new AllianceChatEventArgs(from, GuildC, e.ArgString));
                foreach (NetState state in NetState.Instances)
                {
                    var m = state.Mobile;
                    if (m != null && m.Guild != null && !m.Guild.Disbanded && m.Guild.Type == GuildC.Type)
                    {
                        m.SendMessage(from.SpeechHue, String.Format("Alliance [{0}] [{1}]: {2}", guild, from.Name, e.ArgString));
                    }
                }

                for (var i = 0; i < GuildC.AllianceListeners.Count; ++i)
                {
                    ((Mobile)GuildC.AllianceListeners[i]).SendMessage("{0}[{1}]: {2}", guild, from.Name, e.ArgString);
                }

                //Staff can see alliancechat
                Packet p = null;
                foreach (NetState ns in from.GetClientsInRange(14))
                {
                    var mob = ns.Mobile;

                    if (mob != null && mob.AccessLevel >= AccessLevel.GameMaster && mob.AccessLevel > from.AccessLevel)
                    {
                        if (p == null)
                            p = Packet.Acquire(new UnicodeMessage(from.Serial, from.Body, MessageType.Regular, from.SpeechHue, 3, from.Language, from.Name, String.Format("[AllianceChat]: {0}", e.ArgString)));

                        ns.Send(p);
                    }
                }

                Packet.Release(p);
            }
        }
    }
}*/