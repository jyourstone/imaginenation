//Ber 2006

using System;
using Server.Network;

namespace Server.Items
{
    public class UnsettlingPortraitW : AddonComponent
	{
		//private InternalTimer m_Timer;
		public static TimeSpan AnimDelay = TimeSpan.FromSeconds( 1.0 ); //the delay between animation is 1 seconds
		public DateTime m_NextAnim;

		[Constructable]
		public UnsettlingPortraitW() : base( 10855 )
		{
			Name = "Unsettling Portrait";
			Movable = true;
		}

		public UnsettlingPortraitW( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile m )
		{
			if ( m.InRange( this, 3 ) ) 
			{
				switch ( ItemID ) 
				{ 
					//do swap or animation here 
					case 10855: //1
						ItemID=10856;
						m.PrivateOverheadMessage( MessageType.Regular, 1153, false, "What was that?", m.NetState ); 
						break;
					case 10856: //2
						ItemID=10855; 
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
					case 10855: //1
						ItemID=10856;
						new InternalTimer( this, m ).Start(); 
						break;
					case 10856: //2
						ItemID=10855; 
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
			private readonly UnsettlingPortraitW m_UnsettlingPortraitW;
			private readonly Mobile m_From;
	
			public InternalTimer( UnsettlingPortraitW unsettlingportraitw, Mobile from ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_UnsettlingPortraitW = unsettlingportraitw;
				m_From = from;
			}
// added
			protected override void OnTick() 
			{
				m_Count--;
	
				if ( m_Count == ( 2 ) )
				{
					m_UnsettlingPortraitW.ItemID=10855;
				}
				if ( m_Count == ( 1 ) )
				{
					m_UnsettlingPortraitW.ItemID=10856; 
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
			
	public class UnsettlingPortraitWAddon : BaseAddon
	{
	    public override BaseAddonDeed Deed{ get{ return new UnsettlingPortraitWDeed(); } }
		
		[Constructable]
		public UnsettlingPortraitWAddon()
		{
		  Name = "Unsettling Portrait West";
			Weight = 2.0;
			
			AddComponent( new UnsettlingPortraitW(), 0, 0, 0 );
		}

		public UnsettlingPortraitWAddon( Serial serial ) : base( serial )
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

	public class UnsettlingPortraitWDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new UnsettlingPortraitWAddon(); } }

		[Constructable]
		public UnsettlingPortraitWDeed()
		{
		    Name = "Unsettling Portrait West Deed";
		}

		public UnsettlingPortraitWDeed( Serial serial ) : base( serial )
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