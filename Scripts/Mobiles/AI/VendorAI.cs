using System;

namespace Server.Mobiles
{
	public class VendorAI : BaseAI
	{
        private Timer m_InteractTimer;

		public VendorAI(BaseCreature m) : base (m)
		{
		}

		public override bool DoActionWander()
		{
			m_Mobile.DebugSay( "I'm fine" );

            if (m_Mobile.Region is Regions.GuardedRegion)
                ((Regions.GuardedRegion) m_Mobile.Region).CallGuards(m_Mobile.Location);

		    if ( m_Mobile.Combatant != null )
			{
				if ( m_Mobile.Debug )
					m_Mobile.DebugSay( "{0} is attacking me", m_Mobile.Combatant.Name );

				m_Mobile.Say( Utility.RandomList( 1005305, 501603 ) );

				Action = ActionType.Combat;
			}
			else
			{
				if ( m_Mobile.FocusMob != null )
				{
					if ( m_Mobile.Debug )
						m_Mobile.DebugSay( "{0} has talked to me", m_Mobile.FocusMob.Name );

					Action = ActionType.Interact;
				}
				else
				{
					m_Mobile.Warmode = false;

					base.DoActionWander();
				}
			}

			return true;
		}

        public override bool DoActionCombat()
        {
            var combatant = m_Mobile.Combatant;

            if (combatant == null || combatant.Deleted || combatant.Map != m_Mobile.Map || !combatant.Alive || combatant.IsDeadBondedPet)
            {
                m_Mobile.DebugSay("My combatant is gone, so my guard is up");

                Action = ActionType.Guard;

                return true;
            }

            if (!m_Mobile.InRange(combatant, m_Mobile.RangePerception))
            {
                // They are somewhat far away, can we find something else?

                if (AcquireFocusMob(m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true))
                {
                    m_Mobile.Combatant = m_Mobile.FocusMob;
                    m_Mobile.FocusMob = null;
                }
                else if (!m_Mobile.InRange(combatant, m_Mobile.RangePerception * 3))
                {
                    m_Mobile.Combatant = null;
                }

                combatant = m_Mobile.Combatant;

                if (combatant == null)
                {
                    m_Mobile.DebugSay("My combatant has fled, so I am on guard");
                    Action = ActionType.Guard;

                    return true;
                }
            }

            if (MoveTo(combatant, true, m_Mobile.RangeFight))
            {
                m_Mobile.Direction = m_Mobile.GetDirectionTo(combatant);
            }
            else if (AcquireFocusMob(m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true))
            {
                if (m_Mobile.Debug)
                    m_Mobile.DebugSay("My move is blocked, so I am going to attack {0}", m_Mobile.FocusMob.Name);

                m_Mobile.Combatant = m_Mobile.FocusMob;
                Action = ActionType.Combat;

                return true;
            }
            else if (m_Mobile.GetDistanceToSqrt(combatant) > m_Mobile.RangePerception + 1)
            {
                if (m_Mobile.Debug)
                    m_Mobile.DebugSay("I cannot find {0}, so my guard is up", combatant.Name);

                Action = ActionType.Guard;

                return true;
            }
            else
            {
                if (m_Mobile.Debug)
                    m_Mobile.DebugSay("I should be closer to {0}", combatant.Name);
            }

            return true;
        }

		public override bool DoActionInteract()
		{
			var customer = m_Mobile.FocusMob;

			if ( m_Mobile.Combatant != null )
			{
				if ( m_Mobile.Debug )
					m_Mobile.DebugSay( "{0} is attacking me", m_Mobile.Combatant.Name );

				m_Mobile.Say( Utility.RandomList( 1005305, 501603 ) );

				Action = ActionType.Flee;
				
				return true;
			}

			if ( customer == null || customer.Deleted || customer.Map != m_Mobile.Map )
			{
				m_Mobile.DebugSay( "My customer have disapeared" );
				m_Mobile.FocusMob = null;

				Action = ActionType.Wander;
			}
			else
			{
				if ( customer.InRange( m_Mobile, 4 ) )
				{
					if ( m_Mobile.Debug )
						m_Mobile.DebugSay( "I am with {0}", customer.Name );

					m_Mobile.Direction = m_Mobile.GetDirectionTo( customer );
				}
				else
				{
					if ( m_Mobile.Debug )
						m_Mobile.DebugSay( "{0} is gone", customer.Name );

					m_Mobile.FocusMob = null;

					Action = ActionType.Wander;	
				}
			}

			return true;
		}

		public override bool DoActionGuard()
		{
			m_Mobile.FocusMob = m_Mobile.Combatant;
			return base.DoActionGuard();
		}

		public override bool HandlesOnSpeech( Mobile from )
		{
			if( from.InRange( m_Mobile, m_Mobile.RangePerception ) )
				return true;


			if( from.AccessLevel >= AccessLevel.GameMaster )
				return true;


			if (from.Alive && m_Mobile.Controlled && m_Mobile.Commandable && (from == m_Mobile.ControlMaster || m_Mobile.IsPetFriend(from)))
				return true;

			return (from.Alive && from.InRange(m_Mobile.Location, m_Mobile.RangePerception) && m_Mobile.IsHumanInTown());
		}

		// Temporary 
		public override void OnSpeech( SpeechEventArgs e )
		{
			base.OnSpeech( e );
 
			var from = e.Mobile;

			if ( m_Mobile is BaseVendor && from.InRange( m_Mobile, m_Mobile.RangePerception ) && !e.Handled )
			{
				if ( WasNamed( e.Speech ) )
				{
					e.Handled = true;

					if ( e.HasKeyword( 0x177 ) ) // *sell*
						((BaseVendor)m_Mobile).VendorSell( from );
					else if ( e.HasKeyword( 0x171 ) ) // *buy*
						((BaseVendor)m_Mobile).VendorBuy( from );

                    if (m_InteractTimer != null)
                        m_InteractTimer.Stop();

                    m_InteractTimer = new InteractTimer(from, (BaseVendor) m_Mobile);
                    m_InteractTimer.Start();

                    m_Mobile.FocusMob = from;
				}
				else if (e.HasKeyword(0x171) && !NamedInRange(from,e.Speech))//Buy
				{
					e.Handled = true;
					((BaseVendor)m_Mobile).VendorBuy(from);

                    if (m_InteractTimer != null)
                        m_InteractTimer.Stop();

                    m_InteractTimer = new InteractTimer(from, (BaseVendor)m_Mobile);
                    m_InteractTimer.Start();

                    m_Mobile.FocusMob = from;
				}
				else if (e.HasKeyword(0x177) && !NamedInRange(from, e.Speech))//Sell
				{
					e.Handled = true;
					((BaseVendor)m_Mobile).VendorSell(from);

                    if (m_InteractTimer != null)
                        m_InteractTimer.Stop();

                    m_InteractTimer = new InteractTimer(from, (BaseVendor)m_Mobile);
                    m_InteractTimer.Start();

                    m_Mobile.FocusMob = from;
				}
			}
		}

        private class InteractTimer : Timer
        {
            private readonly Mobile m_From;
            private readonly BaseVendor m_Vendor;

            public InteractTimer(Mobile from, BaseVendor vendor)
                : base(TimeSpan.FromSeconds(60.0))
            {
                m_From = from;
                m_Vendor = vendor;
            }

            protected override void OnTick()
            {
                if (m_From != null && m_Vendor != null)
                {
                    if (m_Vendor.FocusMob == m_From)
                    {
                        if (Utility.RandomDouble() >= 0.5)
                            m_Vendor.Say(true, string.Format("Nice speaking to you {0}.", m_From.Name));
                        else
                            m_Vendor.Say(true,
                                         string.Format(
                                             "Well it was nice speaking to you {0} but i must go about my business.",
                                             m_From.Name));

                        m_Vendor.FocusMob = null;
                    }
                }
            }
        }
	}
}