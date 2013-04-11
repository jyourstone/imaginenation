using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an altageas corpse" )]
	public class Altageas : BaseCreature //BaseCreature
	{
        [Constructable]
        public Altageas()
            : base(AIType.AI_SphereMage, FightMode.Closest, 10, 5, 0.2, 0.4)
        {
            Name = "Altageas";
            Body = 0x03ca;
            Hue = 0x1;

            SetStr(600, 700);
            SetDex(350, 450);
            SetInt(300, 400);

            SetHits(610, 720);
            SetStam(350, 450);
            SetMana(400);
            SetDamage(38, 51);

            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Tactics, 300.0, 400.0);
            SetSkill(SkillName.MagicResist, 100.0, 150.0);
            SetSkill(SkillName.Parry, 200.0, 250.0);
            SetSkill(SkillName.Wrestling, 100.0);
            SetSkill(SkillName.EvalInt, 120.0);

            Fame = Utility.RandomMinMax(2500, 4000);
            Karma = Utility.RandomMinMax(-4000, -6000);

            VirtualArmor = 80;

            PackGold(650, 1150);
            PlateGloves glo = new PlateGloves {Hue = 0x1};
            PackItem(glo);

            Spellbook book = new Spellbook {Content = ulong.MaxValue, LootType = LootType.Regular};
            AddItem(book);
        }

	    public Altageas( Serial serial ) : base( serial )
		{
		}

		public override bool AlwaysMurderer
		{
			get { return true; }
		}

		public override bool BardImmune
		{
			get { return true; }
		}

		public override bool ShowFameTitle
		{
			get { return false; }
		}

		public override bool ClickTitle
		{
			get { return false; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
			AddLoot( LootPack.HighScrolls );
            AddLoot(LootPack.MedScrolls);
            AddLoot(LootPack.Gems, 4);
            PackGold(1200, 1600);
            if (Utility.RandomDouble() <= 0.3)
                AddLoot(LootPack.RandomWand, 1);
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