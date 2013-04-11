//Ber 2006

using System;
using Server.Network;

namespace Server.Items
{
    public class DisturbingPortraitW : AddonComponent
	{
		//private InternalTimer m_Timer;
		public static TimeSpan AnimDelay = TimeSpan.FromSeconds( 1.0 ); //the delay between animation is 1 seconds
		public DateTime m_NextAnim;

		[Constructable]
		public DisturbingPortraitW() : base( 10849 )
		{
			Name = "Disturbing Portrait";
			Movable = true;
		}

		public DisturbingPortraitW( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile m )
		{
			if ( m.InRange( this, 3 ) ) 
			{
				switch ( ItemID ) 
				{ 
					//do swap or animation here 
					case 10849: //1
						ItemID=10850;
						m.PrivateOverheadMessage( MessageType.Regular, 1153, false, "What was that?", m.NetState ); 
						break;
					case 10850: //2
						ItemID=10851; 
						break;
					case 10851: //3
						ItemID=10852; 
						break;
					case 10852: //4
						ItemID=10849; 
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
					case 10849: //1
						ItemID=10850;
						new InternalTimer( this, m ).Start();  
						break;
					case 10850: //2
						ItemID=10851; 
						break;
					case 10851: //3
						ItemID=10852; 
						break;
					case 10852: //4
						ItemID=10849; 
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
			private int m_Count = 2;
			private readonly DisturbingPortraitW m_DisturbingPortraitW;
			private readonly Mobile m_From;
	
			public InternalTimer( DisturbingPortraitW disturbingportraitw, Mobile from ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_DisturbingPortraitW = disturbingportraitw;
				m_From = from;
			}
// added
			protected override void OnTick() 
			{
				m_Count--;
	
				if ( m_Count == ( 4 ) )
				{
					m_DisturbingPortraitW.ItemID=10849;
				}
				if ( m_Count == ( 3 ) )
				{
					m_DisturbingPortraitW.ItemID=10850;
				}
				if ( m_Count == ( 2 ) )
				{
					m_DisturbingPortraitW.ItemID=10851;
				}
				if ( m_Count == ( 1 ) )
				{
					m_DisturbingPortraitW.ItemID=10852; 
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
			
	public class DisturbingPortraitWAddon : BaseAddon
	{
	    public override BaseAddonDeed Deed{ get{ return new DisturbingPortraitWDeed(); } }
		
		[Constructable]
		public DisturbingPortraitWAddon()
		{
		  Name = "Disturbing Portrait West";
			Weight = 2.0;
			
			AddComponent( new DisturbingPortraitW(), 0, 0, 0 );
		}

		public DisturbingPortraitWAddon( Serial serial ) : base( serial )
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

	public class DisturbingPortraitWDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new DisturbingPortraitWAddon(); } }

		[Constructable]
		public DisturbingPortraitWDeed()
		{
		    Name = "Disturbing Portrait West Deed";
		}

		public DisturbingPortraitWDeed( Serial serial ) : base( serial )
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