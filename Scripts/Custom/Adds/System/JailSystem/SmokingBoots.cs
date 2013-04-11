using System;

namespace Server.Items
{
	public class SmokingBoots : Item
	{
		private static readonly int defaultShoes = 5899;

		public SmokingBoots() : base( defaultShoes )
		{
			Name = "Smoking boots";
		}

		public SmokingBoots( Mobile m ) : base( FindFootGear( m ) )
		{
			Name = m.Name + "'s smoking boots";
			MoveToWorld( m.Location, m.Map );
			new SmokingTimer( this );
		}

		public SmokingBoots( Serial s ) : base( s )
		{
		}

		public new TimeSpan DecayTime
		{
			get { return TimeSpan.FromSeconds( 5 ); }
		}

		public new bool Movable
		{
			get { return false; }
		}

		public new bool Decays
		{
			get { return true; }
		}

		public static int FindFootGear( Mobile m )
		{
			Item shoes = m.FindItemOnLayer( Layer.Shoes );
			if( shoes != null )
				return shoes.ItemID;

			return defaultShoes;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); //version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt(); //version
		}

		private class SmokingTimer : Timer
		{
			private readonly SmokingBoots footwear;
			private int tI = 1;

			public SmokingTimer( SmokingBoots i ) : base( TimeSpan.FromSeconds( 1 ), TimeSpan.FromSeconds( 5 ) )
			{
				footwear = i;
				Start();
			}

			protected override void OnTick()
			{
				if( tI == 1 )
				{
					Point3D p = new Point3D( footwear.Location.X, footwear.Location.Y, footwear.Location.Z + 2 );
					Effects.SendLocationEffect( p, footwear.Map, 0x3735, 30 );
					Effects.PlaySound( p, footwear.Map, 0x5C );
					tI++;
				}
				else
				{
					Effects.SendLocationEffect( footwear.Location, footwear.Map, 0x36BD, 10 );
					Effects.PlaySound( footwear.Location, footwear.Map, 0x307 );
					Stop();
					footwear.Delete();
				}
			}
		}
	}
}