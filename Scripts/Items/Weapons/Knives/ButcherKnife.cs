namespace Server.Items
{
	[Flipable( 0x13F6, 0x13F7 )]
	public class ButcherKnife : BaseKnife
	{
		////public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.InfectiousStrike; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Disarm; } }

		public override int AosStrengthReq{ get{ return 5; } }
		public override int AosMinDamage{ get{ return 9; } }
		public override int AosMaxDamage{ get{ return 11; } }
		public override int AosSpeed{ get{ return 49; } }

		public override int OldStrengthReq{ get{ return 5; } }
		public override int OldMinDamage{ get{ return 2; } }
		public override int OldMaxDamage{ get{ return 5; } }
		public override int OldSpeed{ get{ return 200; } }

		public override int InitMinHits{ get{ return 51; } }
		public override int InitMaxHits{ get{ return 60; } }

		[Constructable]
		public ButcherKnife() : base( 0x13F6 )
		{
			Weight = 1.0;
			//Name = "butcher knife";
		}

		public ButcherKnife( Serial serial ) : base( serial )
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