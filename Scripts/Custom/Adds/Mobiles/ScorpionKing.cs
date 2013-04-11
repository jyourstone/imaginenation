using Server.Items;
namespace Server.Mobiles
{
	[CorpseName( "a scorpion king corpse" )]
	public class ScorpionKing : BaseCreature
	{
		[Constructable]
		public ScorpionKing() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Scorpion King";
			Body = 0x0030;
			BaseSoundID = 0x388;
			Hue = 0x0485;

			SetStr( 300, 350 );
			SetDex( 80, 95 );
			SetInt( 300, 350 );

			SetHits( 300, 350 );
			SetMana( 300, 350 );
			SetStam( 80, 95 );

			SetDamage( 10, 18 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetSkill( SkillName.EvalInt, 90.0, 95.0 );
			SetSkill( SkillName.Magery, 90.0, 100.0 );
			SetSkill( SkillName.Poisoning, 99.0, 100.0 );
			SetSkill( SkillName.MagicResist, 50.0, 65.0 );
			SetSkill( SkillName.Tactics, 80.0, 95.0 );
			SetSkill( SkillName.Wrestling, 70.0, 85.0 );
			SetSkill( SkillName.Parry, 80.0, 90.0 );

			Fame = Utility.RandomMinMax( 3000, 5000 );

			VirtualArmor = 31;
			PackGold(300, 500);
            if (Utility.RandomDouble() <= 0.1)
                AddItem(new RandomAccWeap(Utility.RandomMinMax(2, 3)));
		}

		public ScorpionKing( Serial serial ) : base( serial )
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
			get { return Poison.Regular; }
		}

		public override int Meat
		{
			get { return 8; }
		}

		public override void GenerateLoot()
		{
			//AddLoot(LootPack.HighItems_Always, 1);
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Gems );
			AddLoot( LootPack.Gems );
			AddLoot( LootPack.Gems );
			AddLoot( LootPack.Gems );
			AddLoot( LootPack.HighScrolls );
			AddLoot( LootPack.HighScrolls );
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