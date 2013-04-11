using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Solaris.CliLocHandler;

namespace Server.SkillHandlers
{
    public class Stealth
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Stealth].Callback = OnUse;
        }

        public static int[,] ArmorTable { get { return m_ArmorTable; } }
        private static readonly int[,] m_ArmorTable = new int[,]
			{
							//	Gorget	Gloves	Helmet	Arms	Legs	Chest	Shield
				/* Cloth	*/	{ 0,	0,		0,		0,		0,		0,		0 },
				/* Leather	*/	{ 0,	0,		0,		0,		0,		0,		0 },
				/* Studded	*/	{ 2,	2,		0,		4,		6,		10,		0 },
				/* Bone		*/ 	{ 0,	5,		10,		10,		15,		25,		0 },
				/* Spined	*/	{ 0,	0,		0,		0,		0,		0,		0 },
				/* Horned	*/	{ 0,	0,		0,		0,		0,		0,		0 },
				/* Barbed	*/	{ 0,	0,		0,		0,		0,		0,		0 },
				/* Ring		*/	{ 0,	5,		0,		10,		15,		25,		0 },
				/* Chain	*/	{ 0,	0,		10,		0,		15,		25,		0 },
				/* Plate	*/	{ 5,	5,		10,		10,		15,		25,		0 },
				/* Dragon	*/	{ 0,	5,		10,		10,		15,		25,		0 }
			};

        public static int GetArmorRating(Mobile m)
        {
            if (!Core.AOS)
                return (int)m.ArmorRating;

            int ar = 0;

            for (int i = 0; i < m.Items.Count; i++)
            {
                BaseArmor armor = m.Items[i] as BaseArmor;

                if (armor != null && armor.ArmorAttributes.MageArmor == 0)
                    ar += m_ArmorTable[(int)armor.MaterialType, (int)armor.BodyPosition];
            }

            return ar;
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

        public static bool HidingCheckPassed(Mobile from)
        {
            return (from.Skills.Hiding.Base / 100) > Utility.RandomDouble();
        }

        public static void SetAllowedStealthSteps(Mobile from)
        {
            int skillOffset = (int)(from.Skills[SkillName.Stealth].Value / 10.0);
            int steps = Utility.RandomMinMax(skillOffset - 2, skillOffset + 4);

            if (!from.Mounted && from.ArmorRating == 0) //Bonus steps when not mounted and no armor
                steps = (int)(steps* 1.50);
            
            if (steps < 1)
                steps = 1;

            from.AllowedStealthSteps = steps;
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
                    bool ok = (!badCombat && m_From.CheckSkill(SkillName.Stealth, -20.0 - bonus, 80.0 - bonus));

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

                        ok = !badCombat;
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

                            if (HidingCheckPassed(m_From))
                            {
                                SetAllowedStealthSteps(m_From);
                                PlayerMobile pm = m_From as PlayerMobile; // IsStealthing should be moved to Server.Mobiles
                                if (pm != null) pm.IsStealthing = true;
                            }
                            else
                                m_From.SendAsciiMessage("Your skill in hiding is not enough to keep you hidden while moving.");

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