using System;
using Server.Misc;
using Server.Mobiles;

namespace Server.SkillHandlers
{
	public class Meditation
	{
		public static void Initialize()
		{
			SkillInfo.Table[46].Callback = OnUse;
		}

        public static TimeSpan OnUse(Mobile m)
        {
            bool releaseLock = true;

            if (m.BeginAction(typeof(IAction)))
            {
                int range = Math.Min((int)((100 - m.Skills[SkillName.Meditation].Value) / 2) + 8, 18);
                bool badCombat = (m.Combatant != null && m.InRange(m.Combatant.Location, range) && m.Combatant.InLOS(m));

                foreach (Mobile check in m.GetMobilesInRange(range))
                {
                    if (check.InLOS(m) && check.Combatant == m)
                    {
                        badCombat = true;
                        break;
                    }
                }

                if (m.Mana >= m.ManaMax)
                    m.SendLocalizedMessage(501846); // You are at peace.

                else if (badCombat || m.Warmode || (m is PlayerMobile && (((PlayerMobile)m).LastAttackTime + TimeSpan.FromSeconds(5.0)) >= DateTime.Now))
                    m.SendAsciiMessage("You are preoccupied with thoughts of battle.");

                else
                {
                    new InternalTimer(m).Start();

                    m.RevealingAction();
                    m.SendAsciiMessage("You begin meditating...");
                    //m.SendLocalizedMessage(501851); // You enter a meditative trance.
                    releaseLock = false;

                    if (m.Player)
                        m.PlaySound(0xF9);
                }
            }

            else
            {
                m.SendAsciiMessage("You must wait to perform another action.");
                releaseLock = false;    
            }

            if (m is PlayerMobile && releaseLock)
                ((PlayerMobile)m).EndPlayerAction();

            return TimeSpan.Zero;
        }

        private class InternalTimer : Timer, IAction
        {
            private readonly Mobile m_From;
            private double m_Boosted;
            private Point3D m_Location;
            private const int m_Delay = 5;
            private int m_Count = 10;
            private int m_BoostCount;
            private bool m_FirstUse = true;

            public InternalTimer(Mobile from) : base(TimeSpan.FromSeconds(m_Delay), TimeSpan.FromSeconds(0.5))
            {
                m_From = from;
                m_Location = from.Location;

                if (from is PlayerMobile)
                    ((PlayerMobile)from).ResetPlayerAction(this);
            }

            protected override void OnTick()
            {
                m_Count++;
                m_BoostCount++;
                bool releaseLock = false;
                double chance = (m_From.Skills[SkillName.Meditation].Base - 5.0) / 100;
                
                if (m_From.Alive)
                {
                    if (m_From.Mana >= m_From.ManaMax)
                    {
                        m_From.SendLocalizedMessage(501846); // You are at peace.
                        releaseLock = true;
                    }

                    else if (m_From.Combatant != null || m_From.Warmode || (m_From is PlayerMobile && (((PlayerMobile)m_From).LastAttackTime + TimeSpan.FromSeconds(5.0)) >= DateTime.Now))
                    {
                        m_From.SendAsciiMessage("You are preoccupied with thoughts of battle.");
                        releaseLock = true;
                    }

                    else
                    {
                        if (m_From.Location != m_Location)
                        {
                            m_BoostCount = 0;
                            m_Location = m_From.Location;
                            chance -= 0.1;
                        }

                        if (m_Count / 2 >= m_Delay)
                        {
                            m_From.CheckSkill(SkillName.Meditation, 0.0, 100.0);
                            m_Count = 0;

                            int range = Math.Min((int)((100 - m_From.Skills[SkillName.Meditation].Value) / 2) + 10, 18);

                            if (chance < 0.05)
                                chance = 0.05;

                            bool badCombat = (m_From.Combatant != null && m_From.InRange(m_From.Combatant.Location, range) && m_From.Combatant.InLOS(m_From));
                            bool ok = (!badCombat && chance > Utility.RandomDouble());

                            if (ok)
                            {
                                foreach (Mobile m in m_From.GetMobilesInRange(range))
                                {
                                    if (m.InLOS(m_From) && m.Combatant == m_From)
                                    {
                                        badCombat = true;
                                        break;
                                    }

                                    if (m is BaseCreature && NotorietyHandlers.IsGuardCandidate(m))
                                    {
                                        m_BoostCount = 0;
                                        break;
                                    }
                                }
                            }

                            if (badCombat)
                            {
                                m_From.SendAsciiMessage("You are preoccupied with thoughts of battle.");
                                releaseLock = true;
                            }

                            if (!ok)
                            {
                                m_From.SendLocalizedMessage(501850); // You cannot focus your concentration.
                                releaseLock = true;
                            }

                            if (ok && m_FirstUse)
                            {
                                m_From.SendLocalizedMessage(501851); // You enter a meditative trance.
                                m_FirstUse = false;

                                if (m_From.Player)
                                    m_From.PlaySound(0xF9);
                            }
                        }

                        if (!releaseLock)
                        {
                            m_From.Warmode = false;
                            m_From.PlaySound(0xF9);

                            int toBoost;
                            m_Boosted += (chance*Utility.RandomMinMax(1, 5))/6;

                            if (m_BoostCount < 10)
                                m_Boosted /= 1.3;

                            if (m_Boosted >= 1)
                            {
                                toBoost = (int) Math.Floor(m_Boosted);
                                m_Boosted -= Math.Floor(m_Boosted);

                                if (m_From.Mana + toBoost > m_From.ManaMax)
                                    m_From.Mana = m_From.ManaMax;
                                else
                                    m_From.Mana += toBoost;
                            }
                        }
                    }

                    if (releaseLock)
                    {
                        if (m_From is PlayerMobile)
                            ((PlayerMobile)m_From).EndPlayerAction();

                        Stop();
                    }
                }
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.SendLocalizedMessage(501850); // You cannot focus your concentration.

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }
	}
}