using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a balron corpse" )]
	public class GoldBalron : BaseCreature
	{
		[Constructable]
		public GoldBalron() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "balron" );
			Body = 40;
			BaseSoundID = 357;
			Hue = 0x0494;
			SetStr( 1000, 1250 );
			SetDex( 255, 300 );
			SetInt( 151, 250 );

			SetHits( 1100, 1250 );
			SetStam( 255, 300 );
			SetMana( 400 );

			SetDamage( 60 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetSkill( SkillName.Parry, 120.0 );
			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.Meditation, 70.0 );
			SetSkill( SkillName.MagicResist, 150.0 );
			SetSkill( SkillName.Tactics, 150.0 );
			SetSkill( SkillName.Wrestling, 150.0 );

			Fame = 24000;
			Karma = -24000;

			VirtualArmor = 90;

			PackItem( new Longsword() );
			//PackItem(new DaemonsBlood(8));

                Spellbook book = new Spellbook();
                book.Content = ulong.MaxValue;
                book.LootType = LootType.Regular;
                AddItem(book);
		}

		public GoldBalron( Serial serial ) : base( serial )
		{
		}

		public override bool CanRummageCorpses
		{
			get { return false; }
		}

		public override Poison PoisonImmune
		{
			get { return Poison.Deadly; }
		}

		public override int TreasureMapLevel
		{
			get { return 5; }
		}

		public override int Meat
		{
			get { return 9; }
		}

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.HighScrolls, 3);
            AddLoot(LootPack.MedScrolls, 1);
            PackGold(1200, 1800);
            if (Utility.RandomDouble() <= 0.5)
                AddLoot(LootPack.RandomWand, 1);
            if (Utility.RandomDouble() <= 0.1)
                AddItem(new RandomAccWeap(Utility.RandomMinMax(3, 5)));
            if (Utility.RandomDouble() <= 0.4)
            {
                BaseArmor armor = Loot.RandomArmorOrShield();
                armor.ProtectionLevel = (ArmorProtectionLevel)Utility.RandomMinMax(3, 5);
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