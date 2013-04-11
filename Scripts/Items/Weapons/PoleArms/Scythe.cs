using Server.Engines.Harvest;

namespace Server.Items
{
	[Flipable( 0x26BA, 0x26C4 )]
	public class Scythe : BasePoleArm
	{
		////public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.BleedAttack; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }

		public override int AosStrengthReq{ get{ return 45; } }
		public override int AosMinDamage{ get{ return 15; } }
		public override int AosMaxDamage{ get{ return 18; } }
		public override int AosSpeed{ get{ return 32; } }

		public override int OldStrengthReq{ get{ return 45; } }
		public override int OldMinDamage{ get{ return 25; } }
		public override int OldMaxDamage{ get{ return 39; } }
		public override int OldSpeed{ get{ return 475; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 100; } }

		public override HarvestSystem HarvestSystem{ get{ return null; } }

		[Constructable]
		public Scythe() : base( 0x26BA )
		{
			Weight = 5.0;
			//Name = "scythe";
		}

		public Scythe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version == 0)
            {
                MinDamage = 25;
                MaxDamage = 39;
            }
		}
	}
}