namespace Server.Mobiles
{
	[CorpseName( "a scorpion prince corpse" )]
	public class ScorpionPrince : BaseCreature
	{
		[Constructable]
		public ScorpionPrince() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Scorpion Prince";
			Body = 48;
			BaseSoundID = 397;
			Hue = 0x0387;
			SetStr( 150, 175 );
			SetDex( 76, 95 );
			SetInt( 16, 30 );

			SetHits( 150, 175 );
			SetMana( 0 );
			SetStam( 76, 95 );

			SetDamage( 6, 15 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetSkill( SkillName.Poisoning, 90.0, 100.0 );
			SetSkill( SkillName.MagicResist, 40.0, 45.0 );
			SetSkill( SkillName.Tactics, 70.0, 85.0 );
			SetSkill( SkillName.Wrestling, 60.0, 75.0 );
			SetSkill( SkillName.Parry, 70.0, 80.0 );

			Fame = Utility.RandomMinMax( 1000, 2000 );

			VirtualArmor = 22;
			//PackGold(200, 400);
		}

		public ScorpionPrince( Serial serial ) : base( serial )
		{
		}

		public override int Meat
		{
			get { return 8; }
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

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Gems );
			AddLoot( LootPack.Gems );
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