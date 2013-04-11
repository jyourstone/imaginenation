using System;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.SkillHandlers
{
	public class Begging
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.Begging].Callback = OnUse;
		}

		public static TimeSpan OnUse( Mobile m )
		{
            if (m.BeginAction(typeof(IAction)))
            {
                m.RevealingAction();
                m.Target = new InternalTarget();
                m.RevealingAction();
                m.SendLocalizedMessage(500397); // To whom do you wish to grovel?
            }
            else
                m.SendAsciiMessage("You must wait to perform another action.");

            return TimeSpan.Zero;
        }

		private class InternalTarget : Target, IAction
		{
			private bool m_SetSkillTime = true;

			public InternalTarget() :  base ( 12, false, TargetFlags.None )
			{
			}

			protected override void OnTargetFinish( Mobile from )
			{
				if ( m_SetSkillTime )
					from.NextSkillTime = DateTime.Now;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				from.RevealingAction();
                bool releaseLock = true;

				int number = -1;

				if ( targeted is Mobile )
				{
					Mobile targ = (Mobile)targeted;

					if ( targ.Player ) // We can't beg from players
					{
						number = 500398; // Perhaps just asking would work better.
					}
					else if ( !targ.Body.IsHuman ) // Make sure the NPC is human
					{
						number = 500399; // There is little chance of getting money from that!
					}
					else if ( !from.InRange( targ, 2 ) )
					{
						if ( !targ.Female )
							number = 500401; // You are too far away to beg from him.
						else
							number = 500402; // You are too far away to beg from her.
					}
					else if ( from.Mounted ) // If we're on a mount, who would give us money?
					{
						number = 500404; // They seem unwilling to give you any money.
					}
					else
					{
                        releaseLock = false;
						// Face eachother
						from.Direction = from.GetDirectionTo( targ );
						targ.Direction = targ.GetDirectionTo( from );

						from.Animate( 32, 5, 1, true, false, 0 ); // Bow

						new InternalTimer( from, targ ).Start();

						m_SetSkillTime = false;
					}
				}
				else // Not a Mobile
				{
					number = 500399; // There is little chance of getting money from that!
				}

				if ( number != -1 )
					from.SendLocalizedMessage( number );

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

			private class InternalTimer : Timer, IAction
			{
				private readonly Mobile m_From;
			    private readonly Mobile m_Target;

			    public InternalTimer( Mobile from, Mobile target ) : base( TimeSpan.FromSeconds( 3.0 ) )
				{
					m_From = from;
					m_Target = target;
					Priority = TimerPriority.TwoFiftyMS;

                    if (from is PlayerMobile)
                        ((PlayerMobile)from).ResetPlayerAction(this);
				}

				protected override void OnTick()
				{
					Container theirPack = m_Target.Backpack;

					double badKarmaChance = 0.5 - ((double)m_From.Karma / 8570);

					if ( theirPack == null )
					{
						m_From.SendLocalizedMessage( 500404 ); // They seem unwilling to give you any money.
					}
					else if ( m_From.Karma < 0 && badKarmaChance > Utility.RandomDouble() )
					{
                        m_Target.PublicOverheadMessage(MessageType.Regular, m_Target.SpeechHue, 500406); // Thou dost not look trustworthy... no gold for thee today!
                        m_From.CheckTargetSkill(SkillName.Begging, m_Target, 0.0, 100.0); //Taran: Gain begging anyway
					}
					else if ( m_From.CheckTargetSkill( SkillName.Begging, m_Target, 0.0, 100.0 ) )
					{
						int toConsume = theirPack.GetAmount( typeof( Gold ) ) / 10;
						int max = 10 + (m_From.Fame / 2500);

						if ( max > 14 )
							max = 14;
						else if ( max < 10 )
							max = 10;

						if ( toConsume > max )
							toConsume = max;

						if ( toConsume > 0 )
						{
							int consumed = theirPack.ConsumeUpTo( typeof( Gold ), toConsume );

							if ( consumed > 0 )
							{
                                m_Target.PublicOverheadMessage(MessageType.Regular, m_Target.SpeechHue, 500405); // I feel sorry for thee...

								Gold gold = new Gold( consumed );

								m_From.AddToBackpack( gold );
								m_From.PlaySound( gold.GetDropSound() );

								if ( m_From.Karma > -3000 )
								{
									int toLose = m_From.Karma + 3000;

									if ( toLose > 40 )
										toLose = 40;

									Titles.AwardKarma( m_From, -toLose, true );
								}
							}
							else
							{
                                m_Target.PublicOverheadMessage(MessageType.Regular, m_Target.SpeechHue, 500407); // I have not enough money to give thee any!
                            }
						}
						else
						{
                            m_Target.PublicOverheadMessage(MessageType.Regular, m_Target.SpeechHue, 500407); // I have not enough money to give thee any!
                        }
					}
					else
					{
						m_From.SendLocalizedMessage( 500404 ); // They seem unwilling to give you any money.
					}

					//m_From.NextSkillTime = DateTime.Now + TimeSpan.FromSeconds( 10.0 );

                    if (m_From is PlayerMobile)
                        ((PlayerMobile)m_From).EndPlayerAction();
                }

                #region IAction Members

                public void AbortAction(Mobile from)
                {
                    from.SendAsciiMessage("You stop begging");

                    if (from is PlayerMobile)
                        ((PlayerMobile)from).EndPlayerAction();

                    Stop();
                }

                #endregion
			}
		}
	}
}