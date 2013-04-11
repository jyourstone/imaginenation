namespace Server.Mobiles
{
	public class MustangSummon : BaseMount
	{
		[Constructable]
		public MustangSummon() : this( "Mustang" )
		{
		}

		[Constructable]
		public MustangSummon( string name ) : base( name, 228, 16033, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0xA8;

			Hue = 1501;

			SetStr( 22, 98 );
			SetDex( 56, 75 );
			SetInt( 6, 10 );

			SetHits( 28, 45 );
			SetMana( 0 );

			SetDamage( 3, 4 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );

			SetSkill( SkillName.MagicResist, 25.1, 30.0 );
			SetSkill( SkillName.Tactics, 29.3, 44.0 );
			SetSkill( SkillName.Wrestling, 29.3, 44.0 );

			Fame = 300;
			Karma = 300;

			Tamable = false;
			ControlSlots = 0;
			MinTameSkill = 29.1;
		}

		public MustangSummon( Serial serial ) : base( serial )
		{
		}

		public override int Meat
		{
			get { return 3; }
		}

		public override FoodType FavoriteFood
		{
			get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; }
		}

		public override PackInstinct PackInstinct
		{
			get { return PackInstinct.Ostard; }
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