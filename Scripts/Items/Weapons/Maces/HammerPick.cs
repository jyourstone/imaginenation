namespace Server.Items
{
	[Flipable( 0x143D, 0x143C )]
	public class HammerPick : BaseBashing
	{
		////public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.MortalStrike; } }

		public override int AosStrengthReq{ get{ return 45; } }
		public override int AosMinDamage{ get{ return 15; } }
		public override int AosMaxDamage{ get{ return 17; } }
		public override int AosSpeed{ get{ return 28; } }

		public override int OldStrengthReq{ get{ return 35; } }
		//public override int OldMinDamage{ get{ return 20; } }
		//public override int OldMaxDamage{ get{ return 45; } }
        public override int OldMinDamage { get { return 25; } }
        public override int OldMaxDamage { get { return 40; } }
		public override int OldSpeed{ get{ return 604; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 70; } }

		[Constructable]
		public HammerPick() : base( 0x143D )
		{
			Weight = 9.0;
			Layer = Layer.OneHanded;
			//Name = "hammer pick";
		}

		public HammerPick( Serial serial ) : base( serial )
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
                MaxDamage = 40;
            }
		}
	}
}