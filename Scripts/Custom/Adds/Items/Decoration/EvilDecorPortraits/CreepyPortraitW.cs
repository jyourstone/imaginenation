//Ber 2006

using System;
using Server.Network;

namespace Server.Items
{
    public class CreepyPortraitW : AddonComponent
	{
		//private InternalTimer m_Timer;
		public static TimeSpan AnimDelay = TimeSpan.FromSeconds( 1.0 ); //the delay between animation is 1 seconds
		public DateTime m_NextAnim;

		[Constructable]
		public CreepyPortraitW() : base( 10861 )
		{
			Name = "Creepy Portrait";
			Movable = true;
		}

		public CreepyPortraitW( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile m )
		{
			if ( m.InRange( this, 3 ) ) 
			{
				switch ( ItemID ) 
				{ 
					//do swap or animation here 
					case 10861: //1
						ItemID=10862;
						m.PrivateOverheadMessage( MessageType.Regular, 1153, false, "What was that?  It's watching me.", m.NetState ); 
						break;
					case 10862: //2
						ItemID=10863; 
						break;
					case 10863: //3
						ItemID=10864; 
						break;
					case 10864: //4
						ItemID=10861; 
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
					case 10861: //1
						ItemID=10862;
						new InternalTimer( this, m ).Start(); 
						break;
					case 10862: //2
						ItemID=10863; 
						break;
					case 10863: //3
						ItemID=10864; 
						break;
					case 10864: //4
						ItemID=10861; 
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
			private readonly CreepyPortraitW m_CreepyPortraitW;
			private readonly Mobile m_From;
	
			public InternalTimer( CreepyPortraitW creepyportraitw, Mobile from ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_CreepyPortraitW = creepyportraitw;
				m_From = from;
			}
						// added
			protected override void OnTick() 
			{
				m_Count--;
	
				if ( m_Count == ( 4 ) )
				{
					m_CreepyPortraitW.ItemID=10861;
				}
				if ( m_Count == ( 3 ) )
				{
					m_CreepyPortraitW.ItemID=10862;
				}
				if ( m_Count == ( 2 ) )
				{
					m_CreepyPortraitW.ItemID=10863;
				}
				if ( m_Count == ( 1 ) )
				{
					m_CreepyPortraitW.ItemID=10864; 
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
			
	public class CreepyPortraitWAddon : BaseAddon
	{
	    public override BaseAddonDeed Deed{ get{ return new CreepyPortraitWDeed(); } }
		
		[Constructable]
		public CreepyPortraitWAddon()
		{
		  Name = "Creepy Portrait West";
			Weight = 2.0;
			
			AddComponent( new CreepyPortraitW(), 0, 0, 0 );
		}

		public CreepyPortraitWAddon( Serial serial ) : base( serial )
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

	public class CreepyPortraitWDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new CreepyPortraitWAddon(); } }

		[Constructable]
		public CreepyPortraitWDeed()
		{
		    Name = "Creepy Portrait West Deed";
		}

		public CreepyPortraitWDeed( Serial serial ) : base( serial )
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