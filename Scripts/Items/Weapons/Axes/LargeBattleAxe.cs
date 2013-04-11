namespace Server.Items
{
	[Flipable( 0x13FB, 0x13FA )]
	public class LargeBattleAxe : BaseAxe
	{
		////public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.WhirlwindAttack; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.BleedAttack; } }

		public override int AosStrengthReq{ get{ return 80; } }
		public override int AosMinDamage{ get{ return 16; } }
		public override int AosMaxDamage{ get{ return 17; } }
		public override int AosSpeed{ get{ return 29; } }

		public override int OldStrengthReq{ get{ return 40; } }
		public override int OldMinDamage{ get{ return 22; } }
		public override int OldMaxDamage{ get{ return 40; } } //Loki edit: Was 17 - 38
		public override int OldSpeed{ get{ return 455; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 70; } }

		[Constructable]
		public LargeBattleAxe() : base( 0x13FB )
		{
			Weight = 6.0;
			//Name = "large battle axe";
		}

		public LargeBattleAxe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 2 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version < 2)
            {
                MinDamage = 22;
                MaxDamage = 40;
            }
		}
	}
}