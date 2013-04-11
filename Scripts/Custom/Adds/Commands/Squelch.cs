using System;
using Server.Mobiles;

namespace Server.Commands.Generic
{
    public class SquelchCommand : BaseCommand
    {
        public SquelchCommand()
        {
            AccessLevel = AccessLevel.Counselor;
            Supports = CommandSupport.AllMobiles;
            Commands = new[] { "Squelch" };
            ObjectTypes = ObjectTypes.Mobiles;
            Usage = "Squelch <minutes>";
            Description = "Squelches the selected player for x minutes.";
        }

        public static void Initialize()
        {
            TargetCommands.Register(new SquelchCommand());
        }

        public override void Execute(CommandEventArgs e, object obj)
        {
            Mobile from = e.Mobile;
            PlayerMobile target = obj as PlayerMobile;

            if (target != null)
            {
                if (e.Length != 1)
                {
                    e.Mobile.SendAsciiMessage("Format:");
                    e.Mobile.SendAsciiMessage("Squelch int minutes");
                    return;
                }

                if (target.CurrentSquelchTimer != null)
                {
                    target.CurrentSquelchTimer.Stop();
                    target.CurrentSquelchTimer = null;
                }

                target.Squelched = true;
                int index = e.GetInt32(0);
                from.SendAsciiMessage("You squelched {0} for {1} minute{2}", target.Name, index, index == 1 ? "" : "s");
                target.SendAsciiMessage("You have been squelched for {0} minute{1}", index, index == 1 ? "" : "s");
                new SquelchDelayTimer(target, TimeSpan.FromMinutes(index)).Start();
            }
            else
                from.SendAsciiMessage("This only works on players!");
        }
        
        private class SquelchDelayTimer : Timer
        {
            private readonly PlayerMobile m_PM;

            public SquelchDelayTimer(PlayerMobile pm, TimeSpan duration) : base(duration)
            {
                pm.CurrentSquelchTimer = this;
                m_PM = pm;
            }

            protected override void OnTick()
            {
                if (!m_PM.Deleted && m_PM != null)
                {
                    m_PM.SendAsciiMessage("You are no longer squelched.");
                    m_PM.Squelched = false;

                    Stop();
                    m_PM.CurrentSquelchTimer = null;
                }
            }
        }
    }

    public class UnSquelchCommand : BaseCommand
    {
        public UnSquelchCommand()
        {
            AccessLevel = AccessLevel.GameMaster;
            Supports = CommandSupport.AllMobiles;
            Commands = new[] { "UnSquelch" };
            ObjectTypes = ObjectTypes.Mobiles;
            Usage = "UnSquelch";
            Description = "Unsquelches the selected player.";
        }

        public static void Initialize()
        {
            TargetCommands.Register(new UnSquelchCommand());
        }

        public override void Execute(CommandEventArgs e, object obj)
        {
            Mobile from = e.Mobile;
            PlayerMobile target = obj as PlayerMobile;

            if (target != null)
            {
                if (target.CurrentSquelchTimer != null)
                {
                    target.CurrentSquelchTimer.Stop();
                    target.CurrentSquelchTimer = null;
                }

                target.SendAsciiMessage("You are no longer squelched.");
                target.Squelched = false;
            }
            else
                from.SendAsciiMessage("This only works on players!");
        }
    }
}