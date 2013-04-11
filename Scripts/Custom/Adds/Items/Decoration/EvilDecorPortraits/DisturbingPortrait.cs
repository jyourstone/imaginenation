//Ber 2006

using System;
using Server.Network;

namespace Server.Items
{
    public class DisturbingPortrait : AddonComponent
	{
		//private InternalTimer m_Timer;
		public static TimeSpan AnimDelay = TimeSpan.FromSeconds( 1.0 ); //the delay between animation is 1 seconds
		public DateTime m_NextAnim;

		[Constructable]
		public DisturbingPortrait() : base( 10845 )
		{
			Name = "Disturbing Portrait";
			Movable = true;
		}

		public DisturbingPortrait( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile m )
		{
			if ( m.InRange( this, 3 ) ) 
			{
				switch ( ItemID ) 
				{ 
					//do swap or animation here 
					case 10845: //1
						ItemID=10846;
						m.PrivateOverheadMessage( MessageType.Regular, 0x3b2, false, "What was that?", m.NetState ); 
						break;
					case 10846: //2
						ItemID=10847; 
						break;
					case 10847: //3
						ItemID=10848; 
						break;
					case 10848: //4
						ItemID=10845; 
						break;
					default: break; 
				}
			}
			else
			{
				m.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that
			}
		}

		public override bool HandlesOnMovement{ get{ return true; } }

		public override void OnMovement( Mobile m, Point3D oldLocation ) 
		{ 
			if ( DateTime.Now >= m_NextAnim && m.InRange( this, 4 ) ) // check if it's time to animate & mobile in range & in los.
			{
				m_NextAnim = DateTime.Now + AnimDelay; // set next animation time

				switch ( ItemID ) 
				{ 
					//do swap or animation here 
					case 10845: //1
						ItemID=10846;
						 new InternalTimer( this, m ).Start();
						break;
					case 10846: //2
						ItemID=10847; 
						break;
					case 10847: //3
						ItemID=10848; 
						break;
					case 10848: //4
						ItemID=10845; 
						break;
					default: break; 
				}
			}
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( 0 );
		}
        
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
		}

		public class InternalTimer : Timer
		{
			private int m_Count = 5;
			private readonly DisturbingPortrait m_DisturbingPortrait;
			private readonly Mobile m_From;
	
			public InternalTimer( DisturbingPortrait disturbingportrait, Mobile from ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_DisturbingPortrait = disturbingportrait;
				m_From = from;
			}
// added
			protected override void OnTick() 
			{
				m_Count--;
	
				if ( m_Count == ( 4 ) )
				{
					m_DisturbingPortrait.ItemID=10845;
				}
				if ( m_Count == ( 3 ) )
				{
					m_DisturbingPortrait.ItemID=10846;
				}
				if ( m_Count == ( 2 ) )
				{
					m_DisturbingPortrait.ItemID=10847;
				}
				if ( m_Count == ( 1 ) )
				{
					m_DisturbingPortrait.ItemID=10848; 
				}
				if ( m_Count == 0 )
				{
					Stop();
				}

				if ( m_From.NetState == null )
				{
					Stop();
				}
			}
//end add
		}
	}
			
	public class DisturbingPortraitAddon : BaseAddon
	{
	    public override BaseAddonDeed Deed{ get{ return new DisturbingPortraitDeed(); } }
		
		[Constructable]
		public DisturbingPortraitAddon()
		{
		  Name = "Disturbing Portrait";
			Weight = 2.0;
			
			AddComponent( new DisturbingPortrait(), 0, 0, 0 );
		}

		public DisturbingPortraitAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
        }		
	}

	public class DisturbingPortraitDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new DisturbingPortraitAddon(); } }

		[Constructable]
		public DisturbingPortraitDeed()
		{
		    Name = "Disturbing Portrait";
		}

		public DisturbingPortraitDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // version
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
    }
	
}