using System;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;
using Solaris.CliLocHandler;

namespace Server.SkillHandlers
{
    public class EvalInt
    {
        public static void Initialize()
        {
            SkillInfo.Table[16].Callback = OnUse;
        }

        public static TimeSpan OnUse(Mobile m)
        {
            if (m.BeginAction(typeof(IAction)))
            {
                m.RevealingAction();
                m.Target = new InternalTarget();
                m.SendLocalizedMessage(500906); // What do you wish to evaluate?
            }
            else
                m.SendAsciiMessage("You must wait to perform another action.");

            return TimeSpan.Zero;
        }

        private class InternalTarget : Target, IAction
        {
            public InternalTarget()
                : base(8, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                bool releaseLock = true;

                SpellHelper.Turn(from, targeted);

                if (targeted is Mobile)
                {
                    releaseLock = false;
                    new InternalTimer(from, (Mobile) targeted).Start();
                }
                else if (targeted is Item)
                    from.SendAsciiMessage(CliLoc.LocToString(500908)); // It looks smarter than a rock, but dumber than a piece of wood.

                if (releaseLock && from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
            }

            #region TargetFailed

            protected override void OnCantSeeTarget(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnCantSeeTarget(from, targeted);
            }

            protected override void OnTargetDeleted(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetDeleted(from, targeted);
            }

            protected override void OnTargetUntargetable(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetUntargetable(from, targeted);
            }

            protected override void OnNonlocalTarget(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnNonlocalTarget(from, targeted);
            }

            protected override void OnTargetInSecureTrade(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetInSecureTrade(from, targeted);
            }

            protected override void OnTargetNotAccessible(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetNotAccessible(from, targeted);
            }

            protected override void OnTargetOutOfLOS(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetOutOfLOS(from, targeted);
            }

            protected override void OnTargetOutOfRange(Mobile from, object targeted)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetOutOfRange(from, targeted);
            }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();
                else
                    from.EndAction(typeof(IAction));

                base.OnTargetCancel(from, cancelType);
            }

            #endregion

            #region IAction Members

            public void AbortAction(Mobile from)
            {
            }

            #endregion
        }

        private class InternalTimer : Timer, IAction
        {
            private readonly Mobile m_From;
            private readonly Mobile m_Targeted;

            public InternalTimer(Mobile from, Mobile targeted)
                : base(TimeSpan.FromSeconds(1.0))
            {
                m_From = from;
                m_Targeted = targeted;

                if (from is PlayerMobile)
                    ((PlayerMobile)from).ResetPlayerAction(this);
            }

            protected override void OnTick()
            {
                Mobile targ = m_Targeted;

                int i = targ.Int;
                string intMsg;

                if (i < 11)
                    intMsg = "slightly less intelligent than a rock";
                else if (i < 21 && i > 10)
                    intMsg = "fairly stupid";
                else if (i < 31 && i > 20)
                    intMsg = "not the brightest";
                else if (i < 41 && i > 30)
                    intMsg = "about average";
                else if (i < 51 && i > 40)
                    intMsg = "moderately intelligent";
                else if (i < 61 && i > 50)
                    intMsg = "very intelligent";
                else if (i < 71 && i > 60)
                    intMsg = "extraordinarily intelligent";
                else if (i < 81 && i > 70)
                    intMsg = "like a formidable intellect, well beyond the ordinary";
                else if (i < 91 && i > 80)
                    intMsg = "like a definite genius";
                else
                    intMsg = "superhumanly intelligent in a manner you cannot comprehend";

                if (m_From.CheckTargetSkill(SkillName.EvalInt, targ, 0.0, 120.0))
                    m_From.SendAsciiMessage("{0} looks {1}.", targ.Name, intMsg);
                else
                {
                    string type;

                    if (m_Targeted.Body.IsHuman)
                    {
                        if (m_Targeted.Body.IsMale)
                            type = "his";
                        else
                            type = "hers";
                    }
                    else
                        type = "its";

                    m_From.SendAsciiMessage("You cannot judge {0} mental abilities.", type);
                }

                if (m_From is PlayerMobile)
                    ((PlayerMobile)m_From).EndPlayerAction();
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.SendAsciiMessage("You cannot seem to judge the creature correctly");

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }
    }
}