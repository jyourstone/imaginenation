namespace Server.Mobiles
{
	[CorpseName( "a zostrich corpse" )]
	public class Zostrich : BaseMount
	{
		[Constructable]
		public Zostrich() : this( "Zostrich" )
		{
		}

		[Constructable]
		public Zostrich( string name ) : base( name, 0xDA, 0x3EA4, AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0x275;

			SetStr( 94, 170 );
			SetDex( 96, 115 );
			SetInt( 6, 10 );

			SetHits( 71, 110 );
			SetMana( 0 );

			SetDamage( 11, 17 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Fire, 10, 15 );
			SetResistance( ResistanceType.Poison, 20, 25 );
			SetResistance( ResistanceType.Energy, 20, 25 );

			SetSkill( SkillName.MagicResist, 75.1, 80.0 );
			SetSkill( SkillName.Tactics, 79.3, 94.0 );
			SetSkill( SkillName.Wrestling, 79.3, 94.0 );

			Fame = 1500;
			Karma = -1000;

			Tamable = true;
			//ControlSlots = 1;
			MinTameSkill = 77.1;
		}

		public Zostrich( Serial serial ) : base( serial )
		{
		}

		/*public override bool AlwaysAttackable
		{
			get { return true; }
		}

		public override bool AlwaysMurderer
		{
			get { return true; }
		}*/

		public override int Meat
		{
			get { return 3; }
		}

		public override FoodType FavoriteFood
		{
			get { return FoodType.Meat | FoodType.Fish | FoodType.Eggs | FoodType.FruitsAndVegies; }
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