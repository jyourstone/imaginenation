using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a scorpion thrall corpse" )]
	public class ScorpionThrall : BaseCreature
	{
		[Constructable]
		public ScorpionThrall() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Scorpion Thrall";
			Body = 28;
			BaseSoundID = 0x388;

			SetStr( 100, 150 );
			SetDex( 100, 150 );
			SetInt( 36, 60 );

			SetHits( 100, 150 );
			SetMana( 0 );
			SetStam( 100, 150 );

			SetDamage( 5, 15 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetSkill( SkillName.Poisoning, 90.0, 99.0 );
			SetSkill( SkillName.MagicResist, 25.0, 40.0 );
			SetSkill( SkillName.Tactics, 55.0, 70.0 );
			SetSkill( SkillName.Wrestling, 70.0, 85.0 );
			SetSkill( SkillName.Parry, 55.0, 65.0 );

			Fame = Utility.RandomMinMax( 800, 1500 );

			VirtualArmor = 13;
			//PackGold(50, 100);
		}

		public ScorpionThrall( Serial serial ) : base( serial )
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

		public override void GenerateLoot()
		{
			PackItem( new SpidersSilk( Utility.RandomMinMax( 3, 10 ) ) );
			AddLoot( LootPack.Meager );
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