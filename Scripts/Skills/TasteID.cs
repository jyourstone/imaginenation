using System;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.SkillHandlers
{
	public class TasteID
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.TasteID].Callback = OnUse;
		}

        public static TimeSpan OnUse(Mobile m)
        {
            if (m.BeginAction((typeof (IAction))))
            {
                m.Target = new InternalTarget();
                m.SendLocalizedMessage(502807); // What would you like to taste?
            }
            else
                m.SendAsciiMessage("You must wait to perform another action.");

            return TimeSpan.Zero;
        }

	    [PlayerVendorTarget]
        private class InternalTarget : Target, IAction
        {
            public InternalTarget() : base(2, false, TargetFlags.None)
            {
                AllowNonlocal = true;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                bool releaseLock = true;

                if (targeted is Food) //Only skill check
                {
                    releaseLock = false;
                    new InternalTimer(from, (Food) targeted).Start();
                }
                else if (targeted is Mobile)
                    from.SendAsciiMessage("You feel that such an action would be inappropriate.");
                else if (targeted is BasePotion)
                {
                    BasePotion potion = (BasePotion)targeted;
                    from.SendAsciiMessage("You already know what kind of potion that is.");
                    potion.SendLocalizedMessageTo(from, potion.LabelNumber);
                }
                else if (targeted is PotionKeg)
                {
                    PotionKeg keg = (PotionKeg)targeted;

                    if (keg.Held <= 0)
                        from.SendAsciiMessage("There is nothing in the keg to taste!");
                    else
                    {
                        from.SendAsciiMessage("You are already familiar with this keg's contents.");
                        keg.SendLocalizedMessageTo(from, keg.LabelNumber);
                    }
                }
                else
                    from.SendAsciiMessage("That's not something you can taste.");


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
            private readonly Food m_Food;

            public InternalTimer(Mobile from, Food food) : base(TimeSpan.FromSeconds(1.0))
            {
                m_From = from;
                m_Food = food;

                if (from is PlayerMobile)
                    ((PlayerMobile)from).ResetPlayerAction(this);
            }

            protected override void OnTick()
            {
                if (m_From.CheckTargetSkill(SkillName.TasteID, m_Food, 0, 100))
                {
                    if (m_Food.Poison != null)
                        m_From.SendAsciiMessage("It appears to have poison smeared on it.");
                    else
                        m_From.SendAsciiMessage("You detect nothing unusual about this substance.");
                }
                else
                    m_From.SendAsciiMessage("You cannot discern anything about this substance.");

                if (m_From is PlayerMobile)
                    ((PlayerMobile)m_From).EndPlayerAction();
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.SendAsciiMessage("You cannot discern anything about this substance.");

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }
	}
}