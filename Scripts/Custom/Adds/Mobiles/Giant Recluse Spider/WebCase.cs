using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class WebCase : BaseAddon
	{
		private SpawnTimer m_Timer;

		//private Mobile m_Mobile;
		//private Mobile m_Target;

		[Constructable]
		public WebCase() //: base( 4314 )
		{
			Movable = false;
			Name = "a mass of webbing";

AddonComponent ac = null;
			ac = new AddonComponent( 4284 );
			AddComponent( ac, -1, 3, 0 );
			ac = new AddonComponent( 4313 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 4313 );
			AddComponent( ac, -1, -2, 0 );
			ac = new AddonComponent( 4313 );
			AddComponent( ac, -2, 0, 0 );
			ac = new AddonComponent( 4282 );
			AddComponent( ac, 1, 1, 0 );
			ac = new AddonComponent( 4281 );
			AddComponent( ac, 2, 0, 0 );
			ac = new AddonComponent( 4283 );
			AddComponent( ac, 0, 2, 0 );
			ac = new AddonComponent( 4280 );
			AddComponent( ac, 3, -1, 0 );
			ac = new AddonComponent( 4314 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 4317 );
			AddComponent( ac, 2, 2, 51 );
			ac = new AddonComponent( 4313 );
			AddComponent( ac, 0, 1, 30 );
			ac = new AddonComponent( 4314 );
			AddComponent( ac, 2, 2, 50 );


		
			m_Timer = new SpawnTimer( this );
			m_Timer.Start();
			
		}

		public void OnCarve( Mobile from, Item item )
		{
			Effects.PlaySound( GetWorldLocation(), Map, 0x48F );
			Effects.SendLocationEffect( GetWorldLocation(), Map, 0x3728, 10, 10, 0, 0 );

			if ( 0.3 > Utility.RandomDouble() )
			{
				if ( ItemID == 0xF7E )
					from.SendMessage( "You destroy the web." );
				else
					from.SendMessage( "You destroy the web." );

				Gold gold = new Gold( 150, 200 );

				gold.MoveToWorld( GetWorldLocation(), Map );

				Delete();

				m_Timer.Stop();
				
			}
			else
			{
				if ( ItemID == 0xF7E )
					from.SendMessage( "You damage the web." );
				else
					from.SendMessage( "You damage the web." );
			}
		}

		public WebCase( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Timer = new SpawnTimer( this );
			m_Timer.Start();
		}

		private class SpawnTimer : Timer
		{
			private Item m_Item;

			public SpawnTimer( Item item ) : base( TimeSpan.FromSeconds( Utility.RandomMinMax( 12, 20 ) ) )
			{
				Priority = TimerPriority.FiftyMS;

				m_Item = item;
			}

			protected override void OnTick()
			{
				if ( m_Item.Deleted )
					return;

				Mobile spawn;
				Mobile spawn2;
				Mobile spawn3;

				switch ( Utility.Random( 1 ) )
				{
					default:
					case 0: spawn = new Hatchling(); spawn2 = new Hatchling(); spawn3 = new Hatchling(); break;
				}

				spawn.MoveToWorld( m_Item.Location, m_Item.Map );
				spawn2.MoveToWorld( m_Item.Location, m_Item.Map );
				spawn3.MoveToWorld( m_Item.Location, m_Item.Map );

				m_Item.Delete();
			}
		}
	}
}