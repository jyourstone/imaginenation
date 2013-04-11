using System;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public interface ILockpickable : IPoint2D
	{
		int LockLevel{ get; set; }
		bool Locked{ get; set; }
		Mobile Picker{ get; set; }
		int MaxLockLevel{ get; set; }
		int RequiredSkill{ get; set; }

		void LockPick( Mobile from );
	}



	[Flipable( 0x14fc, 0x14fb )]
	public class Lockpick : Item
	{
		[Constructable]
		public Lockpick() : this( 1 )
		{
		}

		[Constructable]
		public Lockpick( int amount ) : base( 0x14FC )
		{
			Stackable = true;
			Amount = amount;
		}

		public Lockpick( Serial serial ) : base( serial )
		{
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version == 0 && Weight == 0.1 )
				Weight = -1;
		}

		public override void OnDoubleClick( Mobile from )
		{
            if (from.BeginAction(typeof(IAction)))
            {
                from.SendLocalizedMessage(502068); // What do you want to pick?
                from.Target = new InternalTarget(this);
            }
            else
                from.SendAsciiMessage("You must wait to perform another action.");

		}

		private class InternalTarget : Target, IAction
		{
			private readonly Lockpick m_Item;

			public InternalTarget( Lockpick item ) : base( 1, false, TargetFlags.None )
			{
				m_Item = item;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
                bool releaselock = true;

                if (!m_Item.Deleted)
                {
                    if (targeted is ILockpickable)
                    {
                        Item item = (Item)targeted;
                        from.Direction = from.GetDirectionTo(item);

                        if (((ILockpickable)targeted).Locked)
                        {
                            from.PlaySound(0x241);
                            releaselock = false;

                            new InternalTimer(from, (ILockpickable)targeted, m_Item).Start();
                        }
                        else
                        {
                            // The door is not locked
                            from.SendLocalizedMessage(502069); // This does not appear to be locked
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(501666); // You can't unlock that!
                    }
                }

                if (releaselock && from is PlayerMobile)
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
				private readonly ILockpickable m_Item;
				private readonly Lockpick m_Lockpick;
			
				public InternalTimer( Mobile from, ILockpickable item, Lockpick lockpick ) : base( TimeSpan.FromSeconds( 3.5 ) )
				{
					m_From = from;
					m_Item = item;
					m_Lockpick = lockpick;
					Priority = TimerPriority.TwoFiftyMS;

                    if (from is PlayerMobile)
                        ((PlayerMobile)from).ResetPlayerAction(this);
				}

				protected void BrokeLockPickTest()
				{
					// When failed, a 25% chance to break the lockpick
					if ( Utility.Random( 4 ) == 0 )
					{
						Item item = (Item)m_Item;

						// You broke the lockpick.
						item.SendLocalizedMessageTo( m_From, 502074 );

						m_From.PlaySound( 0x3A4 );
						m_Lockpick.Consume();
					}
				}
				
				protected override void OnTick()
				{
					Item item = (Item)m_Item;

                    if (!m_From.InRange(item.GetWorldLocation(), 1))
                    {
                        if (m_From is PlayerMobile)
                            ((PlayerMobile)m_From).EndPlayerAction();
                        return;
                    }

					if ( m_Item.LockLevel == 0 || m_Item.LockLevel == -255 )
					{
						// LockLevel of 0 means that the door can't be picklocked
						// LockLevel of -255 means it's magic locked
						item.SendLocalizedMessageTo( m_From, 502073 ); // This lock cannot be picked by normal means
                        if (m_From is PlayerMobile)
                            ((PlayerMobile)m_From).EndPlayerAction();
						return;
					}

					if ( m_From.Skills[SkillName.Lockpicking].Value < m_Item.RequiredSkill )
					{
						// Do some training to gain skills
						m_From.CheckSkill( SkillName.Lockpicking, 0, m_Item.LockLevel );

						// The LockLevel is higher thant the LockPicking of the player
						item.SendLocalizedMessageTo( m_From, 502072 ); // You don't see how that lock can be manipulated.
                        if (m_From is PlayerMobile)
                            ((PlayerMobile)m_From).EndPlayerAction();
						return;
					}

					if ( m_From.CheckTargetSkill( SkillName.Lockpicking, m_Item, m_Item.LockLevel, m_Item.MaxLockLevel ) )
					{
						// Success! Pick the lock!
						item.SendLocalizedMessageTo( m_From, 502076 ); // The lock quickly yields to your skill.
						m_From.PlaySound( 0x4A );
						m_Item.LockPick( m_From );
					}
					else
					{
						// The player failed to pick the lock
						BrokeLockPickTest();
						item.SendLocalizedMessageTo( m_From, 502075 ); // You are unable to pick the lock.
					}

                    if (m_From is PlayerMobile)
                        ((PlayerMobile)m_From).EndPlayerAction();
				}

                #region IAction Members

                public void AbortAction(Mobile from)
                {
                    from.SendAsciiMessage("You are unable to pick the lock.");

                    if (from is PlayerMobile)
                        ((PlayerMobile)from).EndPlayerAction();

                    Stop();
                }

                #endregion
			}
		}
	}
}