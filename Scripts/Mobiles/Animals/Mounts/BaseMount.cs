using System;
using System.Collections;
using Carding.Mobiles;
using Server.Multis;
using Server.Network;

namespace Server.Mobiles
{
	public abstract class BaseMount : BaseCreature, IMount
	{
		public Mobile m_Rider;
		private Item m_InternalItem;
		private DateTime m_NextMountAbility;

		public virtual TimeSpan MountAbilityDelay { get { return TimeSpan.Zero; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextMountAbility
		{
			get { return m_NextMountAbility; }
			set { m_NextMountAbility = value; }
		}

	    [CommandProperty(AccessLevel.GameMaster)]
	    public bool DonationMount { get; set; }

        protected Item InternalItem { get { return m_InternalItem; } }

		public virtual bool AllowMaleRider{ get{ return true; } }
		public virtual bool AllowFemaleRider{ get{ return true; } }

		public BaseMount( string name, int bodyID, int itemID, AIType aiType, FightMode fightMode, int rangePerception, int rangeFight, double activeSpeed, double passiveSpeed ) : base ( aiType, fightMode, rangePerception, rangeFight, activeSpeed, passiveSpeed )
		{
			Name = name;
			Body = bodyID;

			m_InternalItem = new MountItem( this, itemID );
		}

		public BaseMount( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 2 ); // version

            //version 2
            writer.Write(DonationMount);

			writer.Write( m_NextMountAbility );

			writer.Write( m_Rider );
			writer.Write( m_InternalItem );
		}

		[Hue, CommandProperty( AccessLevel.GameMaster )]
		public override int Hue
		{
			get
			{
				return base.Hue;
			}
			set
			{
				base.Hue = value;

				if ( m_InternalItem != null )
					m_InternalItem.Hue = value;
			}
		}

		public override bool OnBeforeDeath()
		{
			Rider = null;

			return base.OnBeforeDeath();
		}

		public override void OnAfterDelete()
		{
			if ( m_InternalItem != null )
				m_InternalItem.Delete();

			m_InternalItem = null;

			base.OnAfterDelete();
		}

		public override void OnDelete()
		{
			Rider = null;

			base.OnDelete();
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			var version = reader.ReadInt();

			switch ( version )
			{
                case 2:
			        {
			            DonationMount = reader.ReadBool();
			            goto case 1;
			        }
				case 1:
				{
					m_NextMountAbility = reader.ReadDateTime();
					goto case 0;
				}
				case 0:
				{
					m_Rider = reader.ReadMobile();
					m_InternalItem = reader.ReadItem();

					if ( m_InternalItem == null )
						Delete();

					break;
				}
			}
		}

		public virtual void OnDisallowedRider( Mobile m )
		{
			m.SendMessage( "You may not ride this creature." );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsDeadPet )
				return;

			if ( from.IsBodyMod && !from.Body.IsHuman )
			{
				if ( Core.AOS ) // You cannot ride a mount in your current form.
					PrivateOverheadMessage( MessageType.Regular, 0x3B2, 1062061, from.NetState );
				else
					from.SendLocalizedMessage( 1061628 ); // You can't do that while polymorphed.

				return;
			}

			if ( !CheckMountAllowed( from, true ) )
				return;


			if ( from.Female ? !AllowFemaleRider : !AllowMaleRider )
			{
				OnDisallowedRider( from );
				return;
			}

			if ( !DesignContext.Check( from ) )
				return;

			if ( from.HasTrade )
			{
				from.SendLocalizedMessage( 1042317, "", 0x41 ); // You may not ride at this time
				return;
			}

			if ( from.InRange( this, 5 ) && from.InLOS(this) )
			{
			    var canAccess = (from.AccessLevel >= AccessLevel.GameMaster) ||
			                    (Controlled && (ControlMaster == from || (Friends != null && Friends.Contains(from)))) ||
			                    (Summoned && SummonMaster == from);

                if (canAccess)
                {
                    if (from.Mounted)
                        from.Mount.Rider = null;

                    //if ( this.Poisoned )
                    //	PrivateOverheadMessage( MessageType.Regular, 0x3B2, 1049692, from.NetState ); // This mount is too ill to ride.
                    //else
                    if (this is Horse || this is Mustang)
                        Effects.PlaySound(this, Map, 168);
                    else if (this is Zostrich)
                        Effects.PlaySound(this, Map, 629);
                    else if (this is Orn || this is Oclock)
                        Effects.PlaySound(this, Map, 624);
                    else if (this is Llama)
                        Effects.PlaySound(this, Map, 184);

                    Rider = from;
                }
                /*else if ( !Controlled && !Summoned )
                {
                    // That mount does not look broken! You would have to tame it to ride it.
                    //PrivateOverheadMessage( MessageType.Regular, 0x3B2, 501263, from.NetState );
                }*/
                else
                {
                    // This isn't your mount; it refuses to let you ride.
                    from.SendAsciiMessage("You dont own that mount.");
                    //PrivateOverheadMessage( MessageType.Regular, 0x3B2, 501264, from.NetState );
                }
			}
			else
			{
				from.SendLocalizedMessage( 500206 ); // That is too far away to ride.
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int ItemID
		{
			get
			{
			    if ( m_InternalItem != null )
					return m_InternalItem.ItemID;
			    return 0;
			}
		    set
			{
				if ( m_InternalItem != null )
					m_InternalItem.ItemID = value;
			}
		}

		public static void Dismount( Mobile m )
		{
			var mount = m.Mount;

            if (mount != null)
                mount.Rider = null;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Rider
		{
			get
			{
				return m_Rider;
			}
			set
			{
				if ( m_Rider != value )
				{
					if ( value == null )
					{
						var loc = m_Rider.Location;
						var map = m_Rider.Map;

						if ( map == null || map == Map.Internal )
						{
							loc = m_Rider.LogoutLocation;
							map = m_Rider.LogoutMap;
						}
                        /*
                        if (this is Horse || this is Mustang)
                            Effects.PlaySound(m_Rider, m_Rider.Map, 168);
                        else if (this is Zostrich)
                            Effects.PlaySound(m_Rider, m_Rider.Map, 629);
                        else if (this is Orn || this is Oclock)
                            Effects.PlaySound(m_Rider, m_Rider.Map, 624);
                        else if (this is Llama)
                            Effects.PlaySound(m_Rider, m_Rider.Map, 184);
                        */
						Direction = m_Rider.Direction;
						Location = loc;
						Map = map;

						if ( m_InternalItem != null )
							m_InternalItem.Internalize();
					}
					else
					{
						if ( m_Rider != null )
							Dismount( m_Rider );

						Dismount( value );

						if ( m_InternalItem != null )
							value.AddItem( m_InternalItem );

						value.Direction = Direction;

						Internalize();
					}

					m_Rider = value;
				}
			}
		}

		private class BlockEntry
		{
			public BlockMountType m_Type;
			public DateTime m_Expiration;

			public bool IsExpired{ get{ return ( DateTime.Now >= m_Expiration ); } }

			public BlockEntry( BlockMountType type, DateTime expiration )
			{
				m_Type = type;
				m_Expiration = expiration;
			}
		}

		private static readonly Hashtable m_Table = new Hashtable();

		public static void SetMountPrevention( Mobile mob, BlockMountType type, TimeSpan duration )
		{
			if ( mob == null )
				return;

			var expiration = DateTime.Now + duration;

			var entry = m_Table[mob] as BlockEntry;

			if ( entry != null )
			{
				entry.m_Type = type;
				entry.m_Expiration = expiration;
			}
			else
			{
				m_Table[mob] = entry = new BlockEntry( type, expiration );
			}
		}

		public static void ClearMountPrevention( Mobile mob )
		{
			if ( mob != null )
				m_Table.Remove( mob );
		}

		public static BlockMountType GetMountPrevention( Mobile mob )
		{
			if ( mob == null )
				return BlockMountType.None;

			var entry = m_Table[mob] as BlockEntry;

			if ( entry == null )
				return BlockMountType.None;

			if ( entry.IsExpired )
			{
				m_Table.Remove( mob );
				return BlockMountType.None;
			}

			return entry.m_Type;
		}

		public static bool CheckMountAllowed( Mobile mob, bool message )
		{
			var type = GetMountPrevention( mob );

			if ( type == BlockMountType.None )
				return true;

			if ( message )
			{
				switch ( type )
				{
					case BlockMountType.Dazed:
					{
						mob.SendLocalizedMessage( 1040024 ); // You are still too dazed from being knocked off your mount to ride!
						break;
					}
					case BlockMountType.BolaRecovery:
					{
						mob.SendLocalizedMessage( 1062910 ); // You cannot mount while recovering from a bola throw.
						break;
					}
					case BlockMountType.DismountRecovery:
					{
						mob.SendLocalizedMessage( 1070859 ); // You cannot mount while recovering from a dismount special maneuver.
						break;
					}
				}
			}

			return false;
		}

		public virtual void OnRiderDamaged( int amount, Mobile from, bool willKill )
		{
			if( m_Rider == null )
				return;

			var attacker = from;
			if( attacker == null )
				attacker = m_Rider.FindMostRecentDamager( true );

			if( !(attacker == this || attacker == m_Rider || willKill || DateTime.Now < m_NextMountAbility) )
			{
				if( DoMountAbility( amount, from ) )
					m_NextMountAbility = DateTime.Now + MountAbilityDelay;

			}
		}

		public virtual bool DoMountAbility( int damage, Mobile attacker )
		{
			return false;
		}
	}

	public class MountItem : Item, IMountItem
	{
		private BaseMount m_Mount;

        public override double DefaultWeight { get { return 0; } }

		public MountItem( BaseMount mount, int itemID ) : base( itemID )
		{
			Layer = Layer.Mount;
			Movable = false;

			m_Mount = mount;
		}

		public MountItem( Serial serial ) : base( serial )
		{
		}

		public override void OnAfterDelete()
		{
			if ( m_Mount != null )
				m_Mount.Delete();

			m_Mount = null;

			base.OnAfterDelete();
		}

		public override DeathMoveResult OnParentDeath(Mobile parent)
		{
			if ( m_Mount != null )
				m_Mount.Rider = null;

			return DeathMoveResult.RemainEquiped;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version

			writer.Write( m_Mount );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			var version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Mount = reader.ReadMobile() as BaseMount;

					if ( m_Mount == null )
						Delete();

					break;
				}
			}
		}

		public IMount Mount
		{
			get
			{
				return m_Mount;
			}
		}
	}

	public enum BlockMountType
	{
		None = -1,
		Dazed,
		BolaRecovery,
		DismountRecovery
	}
}