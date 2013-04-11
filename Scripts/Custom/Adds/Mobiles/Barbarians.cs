using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a barbarian corpse" )]
	public class BarbarianChieftain : BaseCreature
	{
		[Constructable]
		public BarbarianChieftain() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			int hairHue = Utility.RandomHairHue();
			Name = "Barbarian Chieftain";
			Hue = Utility.RandomSkinHue();
			Body = 0x190;

			AddItem( new LongHair( hairHue ) );
			AddItem( new LongBeard( hairHue ) );

			SetStr( 200, 250 );
			SetDex( 88, 98 );
			SetInt( 20, 30 );

			SetHits( 150, 200 );
			SetStam( 81, 95 );

			SetDamage( 25, 28 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetSkill( SkillName.Swords, 100.0 );
			SetSkill( SkillName.MagicResist, 20.0, 35.0 );
			SetSkill( SkillName.Tactics, 90.0, 100.0 );
			SetSkill( SkillName.Wrestling, 90.0, 100.0 );
			SetSkill( SkillName.Parry, 85.0, 100.0 );
			Fame = Utility.RandomMinMax( 4000, 5500 );
			Karma = Utility.RandomMinMax( -4500, -6500 );

			VirtualArmor = 20;
			Item temp;
			temp = new ThighBoots();
			temp.Hue = 0x01bb;
			temp.Movable = false;
			AddItem( temp );
			temp = new LeatherChest();
			temp.Movable = false;
			AddItem( temp );
			temp = new Kilt();
			temp.Hue = 0x01bb;
			temp.Movable = false;
			AddItem( temp );
			LargeBattleAxe a = new LargeBattleAxe();
			a.DamageLevel = WeaponDamageLevel.Ruin;
			AddItem( a );
		}

		public BarbarianChieftain( Serial serial ) : base( serial )
		{
		}

		public override bool ClickTitle
		{
			get { return false; }
		}

		public override bool ShowFameTitle
		{
			get { return false; }
		}

		public override bool AlwaysMurderer
		{
			get { return true; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
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

	[CorpseName( "a barbarian corpse" )]
	public class BarbarianShaman : BaseCreature
	{
		[Constructable]
		public BarbarianShaman() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			int hairHue = Utility.RandomHairHue();
			Hue = Utility.RandomSkinHue();

			Body = 0x190;
			Name = "Barbarian Shaman";
			AddItem( new LongHair( hairHue ) );
			AddItem( new ShortBeard( hairHue ) );
			Item temp;
			temp = new ThighBoots();
			temp.Hue = 0x01bb;
			temp.Movable = false;
			AddItem( temp );
			temp = new Kilt();
			temp.Hue = 0x01bb;
			temp.Movable = false;
			AddItem( temp );
			temp = new BodySash();
			temp.Hue = 0x01bb;
			temp.Movable = false;
			AddItem( temp );
			temp = new GnarledStaff();
			temp.Movable = false;
			AddItem( temp );

			SetStr( 100, 150 );
			SetDex( 81, 95 );
			SetInt( 20, 30 );
			SetHits( 150, 200 );
			SetStam( 81, 95 );
			SetMana( 100 );

			SetDamage( 20, 25 );
			SetSkill( SkillName.Magery, 60.0, 75.0 );
			SetSkill( SkillName.Parry, 75.0, 100.0 );
			SetSkill( SkillName.MagicResist, 40.0, 55.0 );
			SetSkill( SkillName.Macing, 100.0 );
			SetSkill( SkillName.Tactics, 80.0, 95.0 );
			SetSkill( SkillName.Wrestling, 80.0, 95.0 );

			Fame = Utility.RandomMinMax( 3500, 5000 );
			Karma = Utility.RandomMinMax( -4000, -6000 );

			VirtualArmor = 20;

            if (Utility.RandomDouble() <= 0.7)
            {
                Spellbook book = new Spellbook();
                book.Content = ulong.MaxValue;
                book.LootType = LootType.Regular;
                AddItem(book);
            }
		}

		public BarbarianShaman( Serial serial ) : base( serial )
		{
		}

		public override bool ClickTitle
		{
			get { return false; }
		}

		public override bool ShowFameTitle
		{
			get { return false; }
		}

		public override bool AlwaysMurderer
		{
			get { return true; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Gems, 2 );
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

	[CorpseName( "a barbarian corpse" )]
	public class BarbarianWarrior : BaseCreature
	{
		[Constructable]
		public BarbarianWarrior() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 3, 0.2, 0.4 )
		{
			Hue = Utility.RandomSkinHue();
			Female = Utility.RandomBool();

			Item temp;
			int hairHue = Utility.RandomHairHue();

			if( Female )
			{
				Body = 0x0191;
				Name = "Barbarian Warrioress";
				SetStr( 90, 110 );
				SetHits( 125, 150 );
				SetSkill( SkillName.Parry, 65.0, 90.0 );

				AddItem( new PonyTail( hairHue ) );
				AddItem( new BattleAxe() );
				AddItem( new FemaleLeatherChest() );
				AddItem( new LeatherLegs() );
			}
			else
			{
				Body = 0x0190;
				Name = "barbarian warrior";
				SetStr( 150, 200 );
				SetHits( 150, 200 );
				SetSkill( SkillName.Parry, 75.0, 100.0 );

				AddItem( new LongHair( hairHue ) );
				AddItem( new ShortBeard( hairHue ) );
				AddItem( new DoubleAxe() );
				AddItem( new LeatherChest() );

				temp = new Kilt();
				temp.Hue = 0x01bb;
				temp.Movable = false;
				AddItem( temp );
			}

			temp = new ThighBoots();
			temp.Hue = 0x01bb;
			temp.Movable = false;
			AddItem( temp );

			SetDex( 81, 95 );
			SetInt( 20, 30 );

			SetStam( 81, 95 );
			SetDamage( 20, 25 );

			SetSkill( SkillName.Tactics, 80.0, 95.0 );
			SetSkill( SkillName.MagicResist, 20.0, 35.0 );

			SetSkill( SkillName.Swords, 100.0 );
			SetSkill( SkillName.Wrestling, 80.0, 95.0 );
			VirtualArmor = 20;
			Fame = Utility.RandomMinMax( 3500, 5000 );
			Karma = Utility.RandomMinMax( -4000, -6000 );
		}

		public BarbarianWarrior( Serial serial ) : base( serial )
		{
		}

		public override bool ClickTitle
		{
			get { return false; }
		}

		public override bool ShowFameTitle
		{
			get { return false; }
		}

		public override bool AlwaysMurderer
		{
			get { return true; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
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