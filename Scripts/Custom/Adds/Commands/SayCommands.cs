using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Commands
{
    #region

    

    #endregion

    public class CustomCmdHandlers
    {
        private static bool SeperateCommands = false;
        private static int SpeechHueOverride = 0x3B2; // Set to -1 for random hue, Set to a hue # to use that hue #

        public static void Initialize()
        {
            CommandHandlers.Register("Say", AccessLevel.Counselor, Say_OnCommand);
            if (SeperateCommands) CommandHandlers.Register("ItemSay", AccessLevel.Counselor, ItemSay_OnCommand);
        }

        [Usage("Say <text>")]
        [Description("Forces Targeted Object to say <text>.")]
        public static void Say_OnCommand(CommandEventArgs e)
        {
            string toSay = e.ArgString.Trim();

            if (toSay.Length > 0)
                e.Mobile.Target = new SayTarget(toSay, (SeperateCommands ? 1 : 0));
            else
                e.Mobile.SendMessage("Format: Say \"<text>\"");
        }

        [Usage("ItemSay <text>")]
        [Description("Forces Targeted Item to Say <text>.")]
        public static void ItemSay_OnCommand(CommandEventArgs e)
        {
            string toSay = e.ArgString.Trim();

            if (toSay.Length > 0)
                e.Mobile.Target = new SayTarget(toSay, 2);
            else
                e.Mobile.SendMessage("Format: Say \"<text>\"");
        }

        #region Nested type: SayTarget

        private class SayTarget : Target
        {
            private readonly string m_toSay;
            private readonly int m_type;

            public SayTarget(string say, int type) : base(-1, false, TargetFlags.None)
            {
                m_toSay = say;
                m_type = type;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile && m_type < 2)
                {
                    Mobile targ = (Mobile) targeted;

                    if (from != targ && from.AccessLevel > targ.AccessLevel)
                    {
                        CommandLogging.WriteLine(from, "{0} {1} forcing speech on {2}.", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(targ));
                        if (m_toSay.StartsWith(CommandSystem.Prefix))
                        {
                            targ.SendMessage("{0} makes you invoke {1}.", from.Name, m_toSay);
                            CommandSystem.Handle(targ, String.Format("{0}{1}", CommandSystem.Prefix, m_toSay.Substring(1)));
                        }
                        else
                        {
                            if (targ.Player)
                            {
                                PlayerMobile pm = (PlayerMobile) targ;
                                DoSpeech(pm, m_toSay);
                            }
                            else
                                targ.Say(m_toSay);
                        }
                    }
                    else if (from == targ)
                        from.SendMessage("You don't need this command to make yourself say something!");
                    else if (from.AccessLevel <= targ.AccessLevel)
                        from.SendMessage("You cannot do that.");
                }
                else if (targeted is Item && m_type != 1)
                {
                    Item targ = (Item) targeted;
                    object root = targ.RootParent;

                    if (root == null)
                        targ.PublicOverheadMessage(MessageType.Regular, (SpeechHueOverride < 0 ? Utility.RandomDyedHue() : SpeechHueOverride), false, m_toSay);
                    else if (root is Mobile)
                    {
                        if (from != (root) && from.AccessLevel > ((Mobile) root).AccessLevel)
                        {
                            CommandLogging.WriteLine(from, "{0} {1} forcing speech on {2}.", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format((root)));
                            if (m_toSay.StartsWith(CommandSystem.Prefix))
                            {
                                ((Mobile) root).SendMessage("{0} makes you invoke {1}.", from.Name, m_toSay);
                                CommandSystem.Handle(((Mobile) root), String.Format("{0}{1}", CommandSystem.Prefix, m_toSay.Substring(1)));
                            }
                            else
                                ((Mobile) root).Say(m_toSay);
                        }
                        else if (from == (root))
                            from.SendMessage("You don't need this command to make yourself say something!");
                        else if (from.AccessLevel <= ((Mobile) root).AccessLevel)
                            from.SendMessage("You cannot do that.");
                    }
                    else if (root is Item)
                        targ.PublicOverheadMessage(MessageType.Regular, (SpeechHueOverride < 0 ? Utility.RandomDyedHue() : SpeechHueOverride), false, m_toSay);
                }
                else if (targeted is StaticTarget && m_type != 1)
                {
                    StaticOverhead s = new StaticOverhead();
                    s.MoveToWorld(((StaticTarget) targeted).Location, from.Map);
                    s.PublicOverheadMessage(MessageType.Regular, (SpeechHueOverride < 0 ? Utility.RandomDyedHue() : SpeechHueOverride), false, m_toSay);
                }
                else
                    from.SendMessage("Invaild Target Type");
            }

            private static void DoSpeech(PlayerMobile targ, string toSay)
            {
                IPooledEnumerable eable = targ.Map.GetClientsInRange(targ.Location, 15);

                if (targ.UseUnicodeSpeech)
                {
                    foreach (NetState state in eable)
                        if (state != null && (!state.Mobile.HasFilter || targ.InLOS(state.Mobile)))
                            state.Send(new UnicodeMessage(targ.Serial, targ.Body, MessageType.Regular, targ.SpeechHue, 3, "ENU", targ.Name, toSay));
                }
                else
                {
                    foreach (NetState state in eable)
                        if (state != null && (!state.Mobile.HasFilter || targ.InLOS(state.Mobile)))
                            state.Send(new AsciiMessage(targ.Serial, targ.Body, MessageType.Regular, targ.SpeechHue, 3, targ.Name, toSay));
                }

                eable.Free();
            }
        }

        #endregion
    }

    public class StaticOverhead : Item
    {
        [Constructable]
        public StaticOverhead() : base(1)
        {
            Movable = false;
            Light = LightType.Empty;
            new InternalTimer(this).Start();
        }

        public StaticOverhead(Serial serial) : base(serial)
        {
            new InternalTimer(this).Start();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            new InternalTimer(this).Start();
        }

        #region Nested type: InternalTimer

        private class InternalTimer : Timer
        {
            private readonly Item m_StaticOverhead;

            public InternalTimer(Item staticoverhead) : base(TimeSpan.FromSeconds(15.0))
            {
                Priority = TimerPriority.OneSecond;
                m_StaticOverhead = staticoverhead;
            }

            protected override void OnTick()
            {
                m_StaticOverhead.Delete();
            }
        }

        #endregion
    }
}