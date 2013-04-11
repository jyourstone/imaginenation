namespace Server.Items
{
	[Flipable( 0xF43, 0xF44 )]
	public class Hatchet : BaseAxe
	{
		////public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Disarm; } }

		public override int AosStrengthReq{ get{ return 20; } }
		public override int AosMinDamage{ get{ return 13; } }
		public override int AosMaxDamage{ get{ return 15; } }
		public override int AosSpeed{ get{ return 41; } }

		public override int OldStrengthReq{ get{ return 15; } }
		public override int OldMinDamage{ get{ return 15; } }
		public override int OldMaxDamage{ get{ return 28; } } //Loki edit: Was 12 - 26
		public override int OldSpeed{ get{ return 342; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 80; } }

		[Constructable]
		public Hatchet() : base( 0xF43 )
		{
			Weight = 4.0;
			//Name = "hatchet";
		}

		public Hatchet( Serial serial ) : base( serial )
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
                MinDamage = 15;
                MaxDamage = 28;
            }
		}

        public override void OnDoubleClick(Mobile from)
        {
			base.OnDoubleClick(from);

            if (HarvestSystem == null)
                return;

            if (IsChildOf(from.Backpack) || Parent == from)
            {
                HarvestSystem.BeginHarvesting(from, this);
            }
            else
            {
                from.SendAsciiMessage("That must be in your pack for you to use it.");
            }
        }
	}
}