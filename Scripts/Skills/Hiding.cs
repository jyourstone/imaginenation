using System;
using Server.Mobiles;
using Server.Network;
using Solaris.CliLocHandler;

namespace Server.SkillHandlers
{
    public class Hiding
    {
        private static bool m_CombatOverride;

        public static bool CombatOverride
        {
            get { return m_CombatOverride; }
            set { m_CombatOverride = value; }
        }

        public static void Initialize()
        {
            SkillInfo.Table[21].Callback = OnUse;
        }

        public static TimeSpan OnUse(Mobile m)
        {
            if (m.BeginAction(typeof(IAction)))
            {
                new InternalTimer(m).Start();
                m.RevealingAction();
            }
            else
                m.SendAsciiMessage("You must wait to perform another action.");

            return TimeSpan.Zero;
        }

        private class InternalTimer : Timer, IAction
        {
            private readonly Mobile m_From;

            public InternalTimer(Mobile from)
                : base(TimeSpan.FromSeconds(2.0))
            {
                m_From = from;

                if (from is PlayerMobile)
                    ((PlayerMobile)from).ResetPlayerAction(this);
            }

            protected override void OnTick()
            {
                if (m_From.Alive)
                {
                    double bonus = 0.0;

                    int range = Math.Min((int)((100 - m_From.Skills[SkillName.Hiding].Value) / 2) + 8, 18);	//Cap of 18 not OSI-exact, intentional difference

                    bool badCombat = (m_From.Combatant != null && m_From.InRange(m_From.Combatant.Location, range) && m_From.Combatant.InLOS(m_From));
                    bool ok = (!badCombat && m_From.CheckSkill(SkillName.Hiding, 0.0 - bonus, 100.0 - bonus));

                    if (ok)
                    {
                        foreach (Mobile check in m_From.GetMobilesInRange(range))
                        {
                            if (check.InLOS(m_From) && check.Combatant == m_From)
                            {
                                badCombat = true;
                                ok = false;
                                break;
                            }
                        }

                        ok = (!badCombat && m_From.CheckSkill(SkillName.Hiding, 0.0 - bonus, 100.0 - bonus));
                    }

                    if (badCombat)
                    {
                        m_From.RevealingAction();
                        m_From.SendAsciiMessage("You can't seem to hide here.");
                    }
                    else
                    {
                        if (ok)
                        {
                            m_From.Hidden = true;
                            m_From.Warmode = false;

                            Stealth.SetAllowedStealthSteps(m_From);
                            m_From.LocalOverheadMessage(MessageType.Regular, 906, true, CliLoc.LocToString(501240)); // You have hidden yourself well.
                        }
                        else
                        {
                            m_From.RevealingAction();
                            m_From.SendLocalizedMessage(501241); // You can't seem to hide here.
                        }
                    }
                }


                if (m_From is PlayerMobile)
                    ((PlayerMobile)m_From).EndPlayerAction();
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.SendAsciiMessage("You can't seem to hide here");

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }
    }
}