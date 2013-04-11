//Ber 2006

using System;
using Server.Network;

namespace Server.Items
{
    public class CreepyPortrait : AddonComponent
	{
		//private InternalTimer m_Timer;
		public static TimeSpan AnimDelay = TimeSpan.FromSeconds( 1.0 ); //the delay between animation is 1 seconds
		public DateTime m_NextAnim;

		[Constructable]
		public CreepyPortrait() : base( 10857 )
		{
			Name = "Creepy Portrait";
			Movable = true;
		}

		public CreepyPortrait( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile m )
		{
			if ( m.InRange( this, 3 ) ) 
			{
				switch ( ItemID ) 
				{ 
					//do swap or animation here 
					case 10857: //1
					 m.PrivateOverheadMessage( MessageType.Regular, 0x3b2, false, "Hello?", m.NetState );
						ItemID=10858; 
						break;
					case 10858: //2
					m.PrivateOverheadMessage( MessageType.Regular, 0x3b2, false, "What was that?", m.NetState );
						ItemID=10859; 
						break;
					case 10859: //3
					m.PrivateOverheadMessage( MessageType.Regular, 0x3b2, false, "I must be seeing things?", m.NetState );	
						ItemID=10860; 
						break;
					case 10860: //4
						ItemID=10857; 
//						new InternalTimer( this, m ).Start();
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
					case 10857: //1
						ItemID=10858; 
						new InternalTimer( this, m ).Start();
						break;
					case 10858: //2
						ItemID=10859; 
						break;
					case 10859: //3
						ItemID=10860; 
						break;
					case 10860: //4
						ItemID=10857; 
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
			private readonly CreepyPortrait m_CreepyPortrait;
			private readonly Mobile m_From;
	
			public InternalTimer( CreepyPortrait creepyportrait, Mobile from ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_CreepyPortrait = creepyportrait;
				m_From = from;
			}
			
			// added
					protected override void OnTick() 
			{
				m_Count--;
	
				if ( m_Count == ( 4 ) )
				{
					m_CreepyPortrait.ItemID=10857;
				}
				if ( m_Count == ( 3 ) )
				{
					m_CreepyPortrait.ItemID=10858;
				}
				if ( m_Count == ( 2 ) )
				{
					m_CreepyPortrait.ItemID=10859;
				}
				if ( m_Count == ( 1 ) )
				{
					m_CreepyPortrait.ItemID=10860; 
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
			
	public class CreepyPortraitAddon : BaseAddon
	{
	    public override BaseAddonDeed Deed{ get{ return new CreepyPortraitDeed(); } }
		
		[Constructable]
		public CreepyPortraitAddon()
		{
		  Name = "Creepy Portrait";
			Weight = 2.0;
			
			AddComponent( new CreepyPortrait(), 0, 0, 0 );
		}

		public CreepyPortraitAddon( Serial serial ) : base( serial )
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

	public class CreepyPortraitDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new CreepyPortraitAddon(); } }

		[Constructable]
		public CreepyPortraitDeed()
		{
		    Name = "Creepy Portrait";
		}

		public CreepyPortraitDeed( Serial serial ) : base( serial )
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