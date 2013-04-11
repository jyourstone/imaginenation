using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an imp corpse" )]
	public class Imp : BaseCreature
	{
		[Constructable]
		public Imp() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Imp";
			Body = 74;
			BaseSoundID = 422;

			SetStr( 91, 115 );
			SetDex( 61, 80 );
			SetInt( 86, 98 );

			SetHits( 55, 70 );

			SetDamage( 10, 14 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Fire, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 25, 35 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.EvalInt, 20.1, 40.0 );
			SetSkill( SkillName.Magery, 85.1, 90.0 );
			SetSkill( SkillName.MagicResist, 39.1, 59.0 );
			SetSkill( SkillName.Tactics, 42.1, 50.0 );
			SetSkill( SkillName.Wrestling, 40.1, 44.0 );

			Fame = 2500;

			VirtualArmor = 30;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 83.1;

            if (Core.ML && Utility.RandomDouble() < 0.09)
                PackItem(Engines.Plants.Seed.RandomPeculiarSeed(2));

            if (Utility.RandomDouble() <= 0.2)
            {
                Spellbook book = new Spellbook();
                book.Content = ulong.MaxValue;
                book.LootType = LootType.Regular;
                AddItem(book);
            }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.MedScrolls, 2 );
            PackGold (50, 150);
            PackItem(new BatWing(2));
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 6; } }
        public override HideType HideType { get { return HideType.Regular; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Daemon; } }

		public Imp( Serial serial ) : base( serial )
		{
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