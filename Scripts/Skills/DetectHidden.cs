using System;
using System.Collections.Generic;
using Server.Factions;
using Server.Mobiles;

namespace Server.SkillHandlers
{
    public class DetectHidden
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.DetectHidden].Callback = OnUse;
        }

        public static TimeSpan OnUse(Mobile user)
        {
            if (user.BeginAction(typeof(IAction)))
            {
                user.RevealingAction();
                new InternalTimer(user).Start();
            }
            else
                user.SendAsciiMessage("You must wait to perform another action.");

            return TimeSpan.Zero;
        }

        private class InternalTimer : Timer, IAction
        {
            private readonly Mobile m_From;

            public InternalTimer(Mobile from) : base(TimeSpan.FromSeconds(1.0))
            {
                m_From = from;

                if (from is PlayerMobile)
                    ((PlayerMobile)from).ResetPlayerAction(this);
            }

            protected override void OnTick()
            {
                List<Mobile> mobilesFound = new List<Mobile>();

                Point3D p = m_From.Location;

                double srcSkill = m_From.Skills[SkillName.DetectHidden].Base;
                int range = (int) (srcSkill/3.3);

                if (!m_From.CheckSkill(SkillName.DetectHidden, 0.0, 100.0))
                    range /= 2;

                if (range > 0)
                {
                    IPooledEnumerable inRange = m_From.Map.GetMobilesInRange(p, range);

                    foreach (Mobile trg in inRange)
                    {
                        if (trg.Hidden)
                        {
                            if (m_From.AccessLevel >= trg.AccessLevel)
                            {
                                if (trg is ShadowKnight && (trg.X != p.X || trg.Y != p.Y))
                                    continue;

                                trg.RevealingAction();
                                if (trg is PlayerMobile && (trg as PlayerMobile).HiddenWithSpell)
                                {
                                    (trg as PlayerMobile).HiddenWithSpell = false;
                                    trg.RevealingAction();
                                }
                                trg.SendAsciiMessage("You have been revealed!");

                                mobilesFound.Add(trg);
                            }
                        }
                    }

                    inRange.Free();

                    if (Faction.Find(m_From) != null)
                    {
                        IPooledEnumerable itemsInRange = m_From.Map.GetItemsInRange(p, range);

                        foreach (Item item in itemsInRange)
                        {
                            if (item is BaseFactionTrap)
                            {
                                BaseFactionTrap trap = (BaseFactionTrap) item;

                                if (m_From.CheckTargetSkill(SkillName.DetectHidden, trap, 80.0, 100.0))
                                {
                                    m_From.SendLocalizedMessage(1042712, true, " " + (trap.Faction == null ? "" : trap.Faction.Definition.FriendlyName)); // You reveal a trap placed by a faction:

                                    trap.Visible = true;
                                    trap.BeginConceal();
                                }
                            }
                        }

                        itemsInRange.Free();
                    }

                    if (mobilesFound.Count <= 0)
                        m_From.SendAsciiMessage("You can see nothing hidden there.");
                    else
                    {
                        foreach (Mobile s in mobilesFound)
                            m_From.SendAsciiMessage(string.Format("You find {0}.", s.Name));
                    }
                }
                else
                    m_From.SendAsciiMessage("You can see nothing hidden there.");

                if (m_From is PlayerMobile)
                    ((PlayerMobile)m_From).EndPlayerAction();
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.SendAsciiMessage("You see nothing hidden here");

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }
    }
}
