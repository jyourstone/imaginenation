using Server.Items;
namespace Server.Mobiles
{
	[CorpseName( "a scorpion queen corpse" )]
	public class ScorpionQueen : BaseCreature
	{
		[Constructable]
		public ScorpionQueen() : base( AIType.AI_SphereMage, FightMode.Strongest, 10, 7, 0.2, 0.4 )
		{
			Name = "Scorpion Queen";
			Body = 48;
			BaseSoundID = 0x388;
			Hue = 0x0498;

			SetStr( 450, 500 );
			SetDex( 80, 95 );
			SetInt( 600, 650 );

			SetHits( 450, 500 );
			SetMana( 600, 650 );
			SetStam( 80, 95 );

			SetDamage( 10, 20 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetSkill( SkillName.EvalInt, 90.0, 95.0 );
			SetSkill( SkillName.DetectHidden, 90.0, 100.0 );
			SetSkill( SkillName.Magery, 90.0, 100.0 );
			SetSkill( SkillName.Poisoning, 99.0, 100.0 );
			SetSkill( SkillName.MagicResist, 50.0, 65.0 );
			SetSkill( SkillName.Tactics, 80.0, 95.0 );
			SetSkill( SkillName.Wrestling, 70.0, 85.0 );
			SetSkill( SkillName.Parry, 80.0, 90.0 );

			Fame = Utility.RandomMinMax( 800, 1500 );

			VirtualArmor = 29;
		}

		public ScorpionQueen( Serial serial ) : base( serial )
		{
		}

		public override FoodType FavoriteFood
		{
			get { return FoodType.Meat; }
		}

		public override PackInstinct PackInstinct
		{
			get { return PackInstinct.Arachnid; }
		}

		public override Poison PoisonImmune
		{
			get { return Poison.Regular; }
		}

		public override Poison HitPoison
		{
			get { return Poison.Greater; }
		}

		public override int Meat
		{
			get { return 8; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Gems );
			AddLoot( LootPack.Gems );
			AddLoot( LootPack.Gems );
            AddLoot(LootPack.Gems);
			AddLoot( LootPack.HighScrolls );
			AddLoot( LootPack.HighScrolls );
            AddLoot(LootPack.MedScrolls);
            PackGold(1000);
            if (Utility.RandomDouble() <= 0.1)
                AddItem(new RandomAccWeap(Utility.RandomMinMax(3, 4)));
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