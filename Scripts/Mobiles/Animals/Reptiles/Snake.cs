namespace Server.Mobiles
{
	[CorpseName( "a snake corpse" )]
	public class Snake : BaseCreature
	{
		[Constructable]
		public Snake() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			//Name = "Snake";
            switch (Utility.Random(6))
            {
                case 0: Name = "Boa"; break;
                case 1: Name = "Burrowing asp"; break;
                case 2: Name = "Viper"; break;
                case 3: Name = "Coral snake"; break;
                case 4: Name = "Pipe snake"; break;
                case 5: Name = "Python"; break;
            }

			Body = 52;
			Hue = Utility.RandomSnakeHue();
			BaseSoundID = 0xDB;

			SetStr( 22, 34 );
			SetDex( 16, 25 );
			SetInt( 6, 10 );

			SetHits( 15, 19 );
			SetMana( 0 );

			SetDamage( 1, 4 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Poison, 20, 30 );

			SetSkill( SkillName.Poisoning, 50.1, 70.0 );
			SetSkill( SkillName.MagicResist, 15.1, 20.0 );
			SetSkill( SkillName.Tactics, 19.3, 34.0 );
			SetSkill( SkillName.Wrestling, 19.3, 34.0 );

			Fame = 300;

			VirtualArmor = 16;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 59.1;
		}

		public override Poison PoisonImmune{ get{ return Poison.Lesser; } }
		public override Poison HitPoison{ get{ return Poison.Lesser; } }

		public override bool DeathAdderCharmable{ get{ return true; } }

		public override int Meat{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Eggs; } }

		public Snake(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}