namespace Server.Items
{
	[Flipable( 0x13b4, 0x13b3 )]
	public class Club : BaseBashing
	{
		////public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ShadowStrike; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Dismount; } }

		public override int AosStrengthReq{ get{ return 40; } }
		public override int AosMinDamage{ get{ return 11; } }
		public override int AosMaxDamage{ get{ return 13; } }
		public override int AosSpeed{ get{ return 44; } }

		public override int OldStrengthReq{ get{ return 10; } }
		public override int OldMinDamage{ get{ return 12; } }
		public override int OldMaxDamage{ get{ return 19; } }
		public override int OldSpeed{ get{ return 302; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 40; } }

		[Constructable]
		public Club() : base( 0x13B4 )
		{
			Weight = 3.0;
			//Name = "club";
		}

		public Club( Serial serial ) : base( serial )
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

            if (version == 0)
            {
                MinDamage = 12;
                MaxDamage = 19;
            }
		}
	}
}