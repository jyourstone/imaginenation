using System;
using System.Text;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.SkillHandlers
{
	public class ForensicEvaluation
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.Forensics].Callback = OnUse;
		}

		public static TimeSpan OnUse( Mobile m )
		{
            if (m.BeginAction(typeof(IAction)))
            {
                m.Target = new ForensicTarget();
                m.RevealingAction();
                m.SendLocalizedMessage(500906); // What would you like to evaluate?
            }
            else
                m.SendAsciiMessage("You must wait to perform another action.");

            return TimeSpan.FromSeconds(0.0);
        }

		public class ForensicTarget : Target, IAction
		{
			public ForensicTarget() : base( 10, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object target )
			{
                bool releaseLock = true;

				if ( target is Mobile )
				{
                    from.SendAsciiMessage("You cannot target mobiles");
                    /*
					if ( from.CheckTargetSkill( SkillName.Forensics, target, 40.0, 100.0 ) )
					{
						if ( target is PlayerMobile && ((PlayerMobile)target).NpcGuild == NpcGuild.ThievesGuild )
							from.SendLocalizedMessage( 501004 );//That individual is a thief!
						else
							from.SendLocalizedMessage( 501003 );//You notice nothing unusual.
					}
					else
					{
						from.SendLocalizedMessage( 501001 );//You cannot determain anything useful.
					}
                    */
				}
				else if ( target is Corpse )
				{
                    releaseLock = false;
                    new InternalTimer(from, (Corpse)target).Start();
				}
				else if ( target is ILockpickable )
				{
					ILockpickable p = (ILockpickable)target;
					if ( p.Picker != null )
						from.SendLocalizedMessage( 1042749, p.Picker.Name );//This lock was opened by ~1_PICKER_NAME~
					else
						from.SendLocalizedMessage( 501003 );//You notice nothing unusual.
				}

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
            private readonly Corpse m_Corpse;

            public InternalTimer(Mobile from, Corpse corpse)
                : base(TimeSpan.FromSeconds(1.0))
            {
                m_From = from;
                m_Corpse = corpse;

                if (from is PlayerMobile)
                    ((PlayerMobile)from).ResetPlayerAction(this);
            }

            protected override void OnTick()
            {
                if (m_From.CheckTargetSkill(SkillName.Forensics, m_Corpse, 0.0, 100.0))
                {
                    if (m_Corpse.m_Forensicist != null)
                        m_Corpse.LabelTo(m_From, 1042750, m_Corpse.m_Forensicist); // The forensicist  ~1_NAME~ has already discovered that:
                    else
                        m_Corpse.m_Forensicist = m_From.Name;

                    if (((Body)m_Corpse.Amount).IsHuman)
                        m_Corpse.LabelTo(m_From, 1042751, (m_Corpse.Killer == null ? "no one" : m_Corpse.Killer.Name));//This person was killed by ~1_KILLER_NAME~

                    if (m_Corpse.Looters.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < m_Corpse.Looters.Count; i++)
                        {
                            if (i > 0)
                                sb.Append(", ");
                            sb.Append((m_Corpse.Looters[i]).Name);
                        }

                        m_Corpse.LabelTo(m_From, 1042752, sb.ToString());//This body has been distrubed by ~1_PLAYER_NAMES~
                    }
                    else
                        m_Corpse.LabelTo(m_From, 501002);//The corpse has not be desecrated.
                }
                else
                    m_From.SendLocalizedMessage(501001);//You cannot determain anything useful.
                
                if (m_From is PlayerMobile)
                    ((PlayerMobile)m_From).EndPlayerAction();
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.SendLocalizedMessage(501001);

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }

	}
}
