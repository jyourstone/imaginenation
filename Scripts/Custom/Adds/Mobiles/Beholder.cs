using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a beholder corpse" )]
	public class Beholder : BaseCreature
	{
		[Constructable]
		public Beholder() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Beholder";
			Body = 22;
			Hue = 0x0492;
			BaseSoundID = 377;

			SetStr( 1500 );
			SetDex( 200 );
			SetInt( 800 );
			SetHits( 1500 );
			SetStam( 200 );
			SetMana( 800 );

			SetDamage( 30, 50 );

			SetSkill( SkillName.Magery, 200.0 );
			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Tactics, 150.0 );
			SetSkill( SkillName.Wrestling, 200.0 );

			Fame = 19000;
			Karma = -9000;

			VirtualArmor = 50;

            Spellbook book = new Spellbook();
            book.Content = ulong.MaxValue;
            book.LootType = LootType.Regular;
            AddItem(book);


		}

		public Beholder( Serial serial ) : base( serial )
		{
		}

		public override int TreasureMapLevel
		{
			get { return 4; }
		}

		public override int Meat
		{
			get { return 8; }
		}

		public override void GenerateLoot()
		{
			//AddLoot(LootPack.HighItems_Always, 1);
            PackGold(2500);
			AddLoot( LootPack.UltraRich);
            AddLoot(LootPack.MedScrolls, 2);
            AddLoot(LootPack.HighScrolls, 2);
            if (Utility.RandomDouble() <= 0.2)
                AddLoot(LootPack.RandomWand, 1);
            if (Utility.RandomDouble() < 0.05)
                PackItem(Engines.Plants.Seed.RandomBonsaiSeed());

            if (Utility.RandomDouble() <= 0.10)
                AddItem(new RandomAccWeap(4));
            if (Utility.RandomDouble() <= 0.10)
            {
                BaseArmor armor = Loot.RandomArmorOrShield();
                armor.ProtectionLevel = (ArmorProtectionLevel)Utility.RandomMinMax(3, 4);
                AddItem(armor);
            }
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}