using Server.Network;

namespace Server.Items
{
	[Flipable( 0x108F, 0x1091 )]
	public class AdvancedSwitch : Item
	{
		private bool m_HasLinkedDoor;
		private bool m_HasMessage;
		private bool m_HasOnItemId;
		private BaseDoor m_LinkedDoor;
		private string m_Message;
		private int m_OffItemId;
		private int m_OnItemId;

		private bool m_SayOverhead;

		[Constructable]
		public AdvancedSwitch() : this( "left", false )
		{
		}

		[Constructable]
		public AdvancedSwitch( string direction, bool onoff )
		{
			switch( direction.ToLower() )
			{
				default:
				case "left":
				{
					ItemID = 0x108F;
					if( onoff )
					{
						m_OffItemId = 0x108F;
						m_OnItemId = 0x1090;
						m_HasOnItemId = true;
					}
					else
					{
						m_OnItemId = -1;
						m_HasOnItemId = false;
					}
					break;
				}
				case "right":
				{
					ItemID = 0x1091;
					if (onoff)
					{
						m_OffItemId = 0x1091;
						m_OnItemId = 0x1092;
						m_HasOnItemId = true;
					}
					else
					{
						m_OnItemId = -1;
						m_HasOnItemId = false;
					}
					break;
				}
			}

			Movable = false;

			m_HasOnItemId = false;
			m_LinkedDoor = null;
			m_Message = null;
			m_HasMessage = false;
			m_HasLinkedDoor = false;
			m_SayOverhead = false;
		}

		public AdvancedSwitch( Serial serial ) : base( serial )
		{
		}

		[CommandProperty( AccessLevel.Counselor )]
		public bool SayOverhead
		{
			get { return m_SayOverhead; }
			set { m_SayOverhead = value; }
		}

		[CommandProperty( AccessLevel.Counselor )]
		public int OnItemId
		{
			get { return m_OnItemId; }
			set
			{
				m_OnItemId = value;
				m_HasOnItemId = ( m_OnItemId != -1 );
			}
		}

		[CommandProperty( AccessLevel.Counselor )]
		public int OffItemId
		{
			get { return m_OffItemId; }
			set { m_OffItemId = value; }
		}

		[CommandProperty( AccessLevel.Counselor )]
		public BaseDoor LinkedDoor
		{
			get { return m_LinkedDoor; }
			set
			{
				m_LinkedDoor = value;
				m_HasLinkedDoor = ( m_LinkedDoor != null );
			}
		}

		[CommandProperty( AccessLevel.Counselor )]
		public string Message
		{
			get { return m_Message; }
			set
			{
				m_Message = value;
				m_HasMessage = ( !string.IsNullOrEmpty( m_Message ) );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // version

			writer.Write( m_HasMessage );

			if( m_HasMessage )
				writer.Write( m_Message );

			writer.Write( m_HasLinkedDoor );

			if( m_HasLinkedDoor )
				writer.Write( m_LinkedDoor );

			writer.Write( m_HasOnItemId );

			if( m_HasOnItemId )
				writer.Write( m_OnItemId );

			writer.Write( m_OffItemId );

			writer.Write( m_SayOverhead );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			switch( version )
			{
				case 0:
				{
					m_HasMessage = reader.ReadBool();

					if( m_HasMessage )
						m_Message = reader.ReadString();

					m_HasLinkedDoor = reader.ReadBool();

					if( m_HasLinkedDoor )
						m_LinkedDoor = (BaseDoor)reader.ReadItem();

					m_HasOnItemId = reader.ReadBool();

					if( m_HasOnItemId )
						m_OnItemId = reader.ReadInt();

					m_OffItemId = reader.ReadInt();

					m_SayOverhead = reader.ReadBool();

					break;
				}
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( m_HasLinkedDoor )
			{
				if( m_LinkedDoor == null || m_LinkedDoor.Deleted )
				{
					m_HasLinkedDoor = false;
					m_LinkedDoor = null;
					return;
				}

				if( from.InRange( GetWorldLocation(), 5 ) && from.InLOS(this) )
				{
					if( m_HasMessage )
						if( m_SayOverhead )
							PublicOverheadMessage( MessageType.Regular, 0, false, m_Message );
						else
							from.PrivateOverheadMessage( MessageType.Regular, 0, false, m_Message, from.NetState );

					if( m_HasOnItemId )
						ItemID = ( ItemID == m_OnItemId ) ? m_OffItemId : m_OnItemId;

					m_LinkedDoor.Open = !m_LinkedDoor.Open;
				}
				else
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
			}
		}
	}
}