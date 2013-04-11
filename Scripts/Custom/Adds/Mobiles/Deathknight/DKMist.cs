using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class DKMist : Item
	{
		private SpawnTimer m_Timer;

		public override string DefaultName
		{
			get { return ""; }
		}

		[Constructable]
		public DKMist() : base( 0x3789 )
		{
			Movable = false;
			Hue = 0x3F;

			m_Timer = new SpawnTimer( this );
			m_Timer.Start();
		}

		public DKMist( Serial serial ) : base( serial )
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
				m_Item.Delete();
			}
		}
	}
}