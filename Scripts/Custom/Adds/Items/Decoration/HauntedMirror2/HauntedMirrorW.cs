//Ber 2006

using System;
using Server.Network;

namespace Server.Items
{
    public class HauntedMirrorW : AddonComponent
	{
		//private InternalTimer m_Timer;
		public static TimeSpan AnimDelay = TimeSpan.FromSeconds( 5.0 ); //the delay between animation is 5 seconds
		public DateTime m_NextAnim;

		[Constructable]
		public HauntedMirrorW() : base( 10877 )
		{
			Name = "Haunted Mirror";
			Movable = true;
		}

		public HauntedMirrorW( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile m )
		{
			if ( m.InRange( this, 3 ) ) 
			{
				switch ( ItemID ) 
				{ 
					//do swap or animation here 
					case 10877: //blank
						ItemID=10878;
						m.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "The mirror went blank", m.NetState ); 
						break;
					case 10878: //green
						ItemID=10877; 
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
					case 10878: //green 
						ItemID=10877;
						new InternalTimer( this, m ).Start();
						break;
					case 10877: //blank
						ItemID=10878;
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
			private int m_Count = 3;
			private readonly HauntedMirrorW m_HauntedMirrorW;
			private readonly Mobile m_From;
	
			public InternalTimer( HauntedMirrorW hauntedmirrorw, Mobile from ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_HauntedMirrorW = hauntedmirrorw;
				m_From = from;
			}
// added
			protected override void OnTick() 
			{
				m_Count--;
	
				if ( m_Count == ( 2 ) )
				{
					m_HauntedMirrorW.ItemID=10877;
				}
				if ( m_Count == ( 1 ) )
				{
					m_HauntedMirrorW.ItemID=10878; 
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
			
	public class HauntedMirrorWAddon : BaseAddon
	{
	    public override BaseAddonDeed Deed{ get{ return new HauntedMirrorWDeed(); } }
		
		[Constructable]
		public HauntedMirrorWAddon()
		{
		    Name = "Haunted Mirror - West";
			Weight = 2.0;
			
			AddComponent( new HauntedMirrorW(), 0, 0, 0 );
		}

		public HauntedMirrorWAddon( Serial serial ) : base( serial )
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

	public class HauntedMirrorWDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new HauntedMirrorWAddon(); } }

		[Constructable]
		public HauntedMirrorWDeed()
		{
		    Name = "Haunted Mirror Deed - West";
		}

		public HauntedMirrorWDeed( Serial serial ) : base( serial )
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