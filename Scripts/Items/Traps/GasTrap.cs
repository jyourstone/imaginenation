using System;
using Server.Network;

namespace Server.Items
{
	public enum GasTrapType
	{
		NorthWall,
		WestWall,
		Floor
	}

	public class GasTrap : BaseTrap
	{
		private Poison m_Poison;

		[CommandProperty( AccessLevel.GameMaster )]
		public Poison Poison
		{
			get{ return m_Poison; }
			set{ m_Poison = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public GasTrapType Type
		{
			get
			{
				switch ( ItemID )
				{
					case 0x113C: return GasTrapType.NorthWall;
					case 0x1147: return GasTrapType.WestWall;
					case 0x11A8: return GasTrapType.Floor;
				}

				return GasTrapType.WestWall;
			}
			set
			{
				ItemID = GetBaseID( value );
			}
		}

		public static int GetBaseID( GasTrapType type )
		{
			switch ( type )
			{
				case GasTrapType.NorthWall: return 0x113C;
				case GasTrapType.WestWall: return 0x1147;
				case GasTrapType.Floor: return 0x11A8;
			}

			return 0;
		}

		[Constructable]
		public GasTrap() : this( GasTrapType.Floor )
		{
            m_AnimHue = 0;
		}

		[Constructable]
		public GasTrap( GasTrapType type ) : this( type, Poison.Lesser )
		{
            m_AnimHue = 0;
		}

		[Constructable]
		public GasTrap(  Poison poison ) : this( GasTrapType.Floor, Poison.Lesser )
		{
            m_AnimHue = 0;
		}

		[Constructable]
		public GasTrap( GasTrapType type, Poison poison ) : base( GetBaseID( type ) )
		{
            m_AnimHue = 0;
			m_Poison = poison;
		}

		public override bool PassivelyTriggered{ get{ return false; } }
		public override TimeSpan PassiveTriggerDelay{ get{ return TimeSpan.Zero; } }
		public override int PassiveTriggerRange{ get{ return 0; } }
		public override TimeSpan ResetDelay{ get{ return TimeSpan.Zero; } }

        private int m_AnimHue;

        [CommandProperty(AccessLevel.Counselor)]
        public virtual int AnimHue
        {
            get { return m_AnimHue; }
            set { m_AnimHue = value; }
        }

		public override void OnTrigger( Mobile from )
		{
			if ( m_Poison == null || !from.Player || !from.Alive || from.AccessLevel > AccessLevel.Player )
				return;
            //if (AnimHue > 0) 
            //    AnimHue--;

			Effects.SendLocationEffect( Location, Map, GetBaseID( Type ) - 2, 16, 3, m_AnimHue, 0 );
			Effects.PlaySound( Location, Map, 0x231 );

			from.ApplyPoison( from, m_Poison );

			from.LocalOverheadMessage( MessageType.Regular, 0x22, 500855 ); // You are enveloped by a noxious gas cloud!
		}

		public GasTrap( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version

            writer.Write(m_AnimHue);
			Poison.Serialize( m_Poison, writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
                case 1:
			    {
                    m_AnimHue = reader.ReadInt();
			        goto case 0;
			    }
				case 0:
				{
					m_Poison = Poison.Deserialize( reader );
					break;
				}
			}
		}
	}
}