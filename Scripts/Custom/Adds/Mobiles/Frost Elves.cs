using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a frost elf corpse" )]
	public class FrostElfShaman : BaseCreature
	{
		[Constructable]
		public FrostElfShaman() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Title = "the Frost Elf";
			Hue = 1328;

			AddItem( new Robe() );
			AddItem( new Spellbook() );
			AddItem( new Sandals() );

			Female = Utility.RandomBool();
			if( Female )
			{
				Body = 0x191;
				Name = NameList.RandomName( "pixie" );
				AddItem( new LongHair( Serial.NewItem ) );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "golem controller" );
				AddItem( new KrisnaHair( Serial.NewItem ) );
			}

			SetStr( 126, 150 );
			SetDex( 96, 120 );
			SetInt( 151, 175 );

			SetHits( 150, 200 );

			SetDamage( 3, 9 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 30, 40 );
			SetResistance( ResistanceType.Fire, 25, 35 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 5, 15 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.EvalInt, 95.1, 100.0 );
			SetSkill( SkillName.Magery, 95.1, 100.0 );
			SetSkill( SkillName.Meditation, 95.1, 100.0 );
			SetSkill( SkillName.MagicResist, 102.5, 125.0 );
			SetSkill( SkillName.Tactics, 65.0, 87.5 );
			SetSkill( SkillName.Wrestling, 65.0, 87.5 );

			Fame = Utility.RandomMinMax( 2500, 1500 );
			Karma = Utility.RandomMinMax( -5000, 2000 );

			VirtualArmor = 16;

            if (Utility.RandomDouble() <= 0.7)
            {
                Spellbook book = new Spellbook();
                book.Content = ulong.MaxValue;
                book.LootType = LootType.Regular;
                AddItem(book);
            }
		}

		public FrostElfShaman( Serial serial ) : base( serial )
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
            Item fullbookItem;
            fullbookItem = new Spellbook();
            (fullbookItem as Spellbook).Content = ulong.MaxValue;
            (fullbookItem as Spellbook).LootType = LootType.Regular;
            Container bookbag = new Bag();
            bookbag.DropItem(fullbookItem);
            PackItem(bookbag);
            
			Container bag = new Bag();
            bag.DropItem( new BlackPearl( 15 ) );
			bag.DropItem( new Bloodmoss( 15 ) );
			bag.DropItem( new Garlic( 15 ) );
			bag.DropItem( new Ginseng( 15 ) );
			bag.DropItem( new MandrakeRoot( 15 ) );
			bag.DropItem( new Nightshade( 15 ) );
			bag.DropItem( new SulfurousAsh( 15 ) );
			bag.DropItem( new SpidersSilk( 15 ) );
            PackItem(bag);

			AddLoot( LootPack.Meager );
			AddLoot( LootPack.MedScrolls, 2 );
			AddLoot( LootPack.Potions, 1 );
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

	[CorpseName( "a frost elf corpse" )]
	public class FrostElfWarrior : BaseCreature
	{
		[Constructable]
		public FrostElfWarrior() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Title = "the Frost Elf";
			Hue = 1328;

			Body = 0x190;
			Name = NameList.RandomName( "golem controller" );
			AddItem( new KrisnaHair( Serial.NewItem ) );
			AddItem( new Sandals() );
			AddItem( new Tunic() );

			SetStr( 86, 100 );
			SetDex( 81, 95 );
			SetInt( 61, 75 );

			SetDamage( 8, 15 );

			SetSkill( SkillName.Fencing, 66.0, 97.5 );
			SetSkill( SkillName.MagicResist, 25.0, 47.5 );
			SetSkill( SkillName.Swords, 66.0, 97.5 );
			SetSkill( SkillName.Tactics, 66.0, 97.5 );

			Fame = Utility.Random( 2500, 1500 );
			Karma = Utility.Random( -4000, 1000 );

			switch( Utility.Random( 3 ) )
			{
				case 0:
					AddItem( new Spear() );
					break;
				case 1:
					AddItem( new Halberd() );
					break;
				case 2:
					AddItem( new Bardiche() );
					break;
			}
		}

		public FrostElfWarrior( Serial serial ) : base( serial )
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
			AddLoot( LootPack.Meager );
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

	[CorpseName( "a frost elf corpse" )]
	public class FrostElfArcher : BaseCreature
	{
		[Constructable]
		public FrostElfArcher() : base( AIType.AI_Archer, FightMode.Closest, 10, 3, 0.2, 0.4 )
		{
			Title = "the Frost Elf";
			Hue = 1328;
			Body = 0x190;
			Name = NameList.RandomName( "golem controller" );

			AddItem( new KrisnaHair( Serial.NewItem ) );
			AddItem( new Sandals() );
			AddItem( new Tunic() );
			AddItem( new Arrow( 20 ) );

			Bow heldweapon = new Bow();
			heldweapon.Skill = SkillName.Archery;
			heldweapon.Speed = 150;
			heldweapon.Movable = false;
			AddItem( heldweapon );

			SetStr( 86, 100 );
			SetDex( 81, 95 );
			SetInt( 61, 75 );

			SetDamage( 10, 15 );

			SetSkill( SkillName.Archery, 75.0, 100.0 );
			SetSkill( SkillName.Tactics, 75.0, 100.0 );
			SetSkill( SkillName.MagicResist, 25.0, 47.5 );

			Fame = Utility.Random( 2500, 1500 );
			Karma = Utility.Random( -4000, 1000 );
		}

		public FrostElfArcher( Serial serial ) : base( serial )
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
			AddLoot( LootPack.Meager );
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

	[CorpseName( "a frost elf corpse" )]
	public class FrostElfChieftan : BaseCreature
	{
		[Constructable]
		public FrostElfChieftan() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Title = "the Frost Elf";
			Hue = 1328;
			Body = 0x190;
			AddItem( new Sandals() );

			Female = Utility.RandomBool();
			if( Female )
			{
				Body = 0x191;
				Name = NameList.RandomName( "pixie" );
				AddItem( new FemaleStuddedChest() );
				AddItem( new LongHair( Serial.NewItem ) );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new StuddedChest() );
				AddItem( new KrisnaHair( Serial.NewItem ) );
			}

			AddItem( new VikingSword() );
			AddItem( new HeaterShield() );

			VirtualArmor = 20;

			SetStr( 86, 100 );
			SetHits( 150, 200 );
			SetDex( 81, 95 );
			SetInt( 61, 75 );

			SetDamage( 15, 30 );

			SetSkill( SkillName.MagicResist, 60.0, 75.0 );
			SetSkill( SkillName.Swords, 100.0 );
			SetSkill( SkillName.Tactics, 90.0, 97.5 );
			SetSkill( SkillName.Parry, 75.0, 100.0 );

			Fame = Utility.Random( 2500, 1500 );
			Karma = Utility.Random( -4000, 1000 );
		}

		public FrostElfChieftan( Serial serial ) : base( serial )
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
			//AddLoot(LootPack.MediumItems_Always, 1);
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
}