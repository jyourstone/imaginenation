using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class RecluseSpiderEggSac : Item
	{
		private SpawnTimer m_Timer;

		public override string DefaultName
		{
			get { return "egg sac"; }
		}

		[Constructable]
		public RecluseSpiderEggSac() : base( 4313 )
		{
			Movable = false;


			m_Timer = new SpawnTimer( this );
			m_Timer.Start();
		}

		public void OnDoubleClick( Mobile from, Item item )
		{
			Effects.PlaySound( GetWorldLocation(), Map, 0x25B );
			Effects.SendLocationEffect( GetWorldLocation(), Map, 0x3728, 10, 10, 0, 0 );

			if ( 0.3 > Utility.RandomDouble() )
			{

				from.SendMessage( "You destroy the egg sac." );

				Gold gold = new Gold( 25, 100 );

				gold.MoveToWorld( GetWorldLocation(), Map );

				Delete();

				m_Timer.Stop();
			}
			else
			{
					from.SendMessage( "You damage the egg sac." );
			}
		}

		public RecluseSpiderEggSac( Serial serial ) : base( serial )
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

			public SpawnTimer( Item item ) : base( TimeSpan.FromSeconds( Utility.RandomMinMax( 5, 10 ) ) )
			{
				Priority = TimerPriority.FiftyMS;

				m_Item = item;
			}

			protected override void OnTick()
			{
				if ( m_Item.Deleted )
					return;

				Mobile spawn;

				spawn = new Hatchling();

				spawn.MoveToWorld( m_Item.Location, m_Item.Map );

				m_Item.Delete();
			}
		}
	}
}