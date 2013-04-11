using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class DKRemains : Item, ICarvable
	{
		private SpawnTimer m_Timer;

		public override string DefaultName
		{
			get { return "remains"; }
		}

		[Constructable]
		public DKRemains() : base( 3790 )
		{
			Movable = false;

			m_Timer = new SpawnTimer( this );
			m_Timer.Start();

			Effects.PlaySound( GetWorldLocation(), Map, 0x1FB );
			Effects.SendLocationEffect( GetWorldLocation(), Map, 0x3789, 10, 10, 0, 0 );
		}

		public void Carve( Mobile from, Item item )
		{
			Effects.PlaySound( GetWorldLocation(), Map, 0x48F );
			Effects.SendLocationEffect( GetWorldLocation(), Map, 0x3728, 10, 10, 0, 0 );

			if ( 0.3 > Utility.RandomDouble() )
			{
				from.SendMessage( "You destroy the remains." );

				Gold gold = new Gold( 25, 50 );

				gold.MoveToWorld( GetWorldLocation(), Map );

				Delete();

				m_Timer.Stop();
			}
			else
			{
				from.SendMessage( "You damage the remains." );
			}
		}

		public DKRemains( Serial serial ) : base( serial )
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

				switch ( Utility.Random( 8 ) )
				{
					default:
					case 0: spawn = new PatchworkSkeleton(); break;
					case 1: spawn = new Zombie(); break;
					case 2: spawn = new Wraith(); break;
					case 3: spawn = new SkeletalMage(); break;
					case 4: spawn = new Mummy(); break;
					case 5: spawn = new BoneKnight(); break;
					case 6: spawn = new SkeletalKnight(); break;
					case 7: spawn = new Lich(); break;
				}

				spawn.MoveToWorld( m_Item.Location, m_Item.Map );

				//Effects.PlaySound( p, map, 0x1FB );
				//Effects.SendLocationParticles( EffectItem.Create( p, map, EffectItem.DefaultDuration ), 0x3789, 1, 40, 0x3F, 3, 9907, 0 );

				m_Item.Delete();
			}
		}
	}
}