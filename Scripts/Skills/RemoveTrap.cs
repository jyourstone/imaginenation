using System;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.SkillHandlers
{
	public class RemoveTrap
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.RemoveTrap].Callback = OnUse;
		}

		public static TimeSpan OnUse( Mobile m )
		{
            bool releaseLock = false;
            if (m.BeginAction(typeof(IAction)))
            {
                releaseLock = true;
                if (m.Skills[SkillName.Lockpicking].Value < 50)
                    m.SendLocalizedMessage(502366); // You do not know enough about locks.  Become better at picking locks.
                else if (m.Skills[SkillName.DetectHidden].Value < 50)
                    m.SendLocalizedMessage(502367); // You are not perceptive enough.  Become better at detect hidden.
                else
                {
                    m.Target = new InternalTarget();

                    m.SendLocalizedMessage(502368); // Wich trap will you attempt to disarm?
                }
            }
            else
                m.SendAsciiMessage("You must wait to perform another action.");

            if (releaseLock && m is PlayerMobile)
                ((PlayerMobile)m).EndPlayerAction();

            return TimeSpan.Zero;
		}

		private class InternalTarget : Target, IAction
		{
			public InternalTarget() :  base ( 2, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
                bool releaseLock = true;
				if ( targeted is Mobile )
				{
					from.SendLocalizedMessage( 502816 ); // You feel that such an action would be inappropriate
				}
				else if ( targeted is TrapableContainer )
				{
					TrapableContainer targ = (TrapableContainer)targeted;

					from.Direction = from.GetDirectionTo( targ );

                    if (targ.TrapType == TrapType.None)
                    {
                        from.SendLocalizedMessage(502373); // That doesn't appear to be trapped
                    }

                    else
                    {
                        from.PlaySound(0x241);
                        from.SendAsciiMessage("You begin removing the trap");
                        releaseLock = false;
                        new InternalTimer(from, targ).Start();
                    }
				}
                    //Taran: Not using factions
                /*else if ( targeted is BaseFactionTrap )
                {
                    BaseFactionTrap trap = (BaseFactionTrap) targeted;
                    Faction faction = Faction.Find( from );

                    FactionTrapRemovalKit kit = ( from.Backpack == null ? null : from.Backpack.FindItemByType( typeof( FactionTrapRemovalKit ) ) as FactionTrapRemovalKit );

                    bool isOwner = ( trap.Placer == from || ( trap.Faction != null && trap.Faction.IsCommander( from ) ) );

                    if ( faction == null )
                    {
                        from.SendLocalizedMessage( 1010538 ); // You may not disarm faction traps unless you are in an opposing faction
                    }
                    else if ( faction == trap.Faction && trap.Faction != null && !isOwner )
                    {
                        from.SendLocalizedMessage( 1010537 ); // You may not disarm traps set by your own faction!
                    }
                    else if ( !isOwner && kit == null )
                    {
                        from.SendLocalizedMessage( 1042530 ); // You must have a trap removal kit at the base level of your pack to disarm a faction trap.
                    }
                    else
                    {
                        if ( (Core.ML && isOwner) || (from.CheckTargetSkill( SkillName.RemoveTrap, trap, 80.0, 100.0 ) && from.CheckTargetSkill( SkillName.Tinkering, trap, 80.0, 100.0 )) )
                        {
                            from.PrivateOverheadMessage( MessageType.Regular, trap.MessageHue, trap.DisarmMessage, from.NetState );

                            if ( !isOwner )
                            {
                                int silver = faction.AwardSilver( from, trap.SilverFromDisarm );

                                if ( silver > 0 )
                                    from.SendLocalizedMessage( 1008113, true, silver.ToString( "N0" ) ); // You have been granted faction silver for removing the enemy trap :
                            }

                            trap.Delete();
                        }
                        else
                        {
                            from.SendLocalizedMessage( 502372 ); // You fail to disarm the trap... but you don't set it off
                        }

                        if ( !isOwner && kit != null )
                            kit.ConsumeCharge( from );
                    }
                }*/
                else
				{
					from.SendLocalizedMessage( 502373 ); // That doesn't appear to be trapped
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
            private readonly TrapableContainer m_Targeted;

            public InternalTimer(Mobile from, TrapableContainer targeted)
                : base(TimeSpan.FromSeconds(5.0))
            {
                m_From = from;
                m_Targeted = targeted;

                if (from is PlayerMobile)
                    ((PlayerMobile)from).ResetPlayerAction(this);
            }

            protected override void OnTick()
            {
                if (m_From.CheckTargetSkill(SkillName.RemoveTrap, m_Targeted, m_Targeted.TrapPower, (m_Targeted.TrapPower + 30)))
                {
                    m_Targeted.TrapPower = 0;
                    m_Targeted.TrapLevel = 0;
                    m_Targeted.TrapType = TrapType.None;
                    m_From.SendLocalizedMessage(502377); // You successfully render the trap harmless
                }
                else
                {
                    m_From.SendLocalizedMessage(502372); // You fail to disarm the trap... but you don't set it off
                }

                if (m_From is PlayerMobile)
                    ((PlayerMobile)m_From).EndPlayerAction();
            }

            #region IAction Members

            public void AbortAction(Mobile from)
            {
                from.SendLocalizedMessage(502372);

                if (from is PlayerMobile)
                    ((PlayerMobile)from).EndPlayerAction();

                Stop();
            }

            #endregion
        }
	}
}