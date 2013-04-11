using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a dark elf corpse" )]
	public class DarkElfGrunt : BaseCreature
	{
		[Constructable]
		public DarkElfGrunt() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Title = "the Dark Elf";
			Hue = 0x0597;

			Item temp;

			temp = new BoneChest();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );
			temp = new BoneGloves();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );
			temp = new BoneHelm();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );
			temp = new ThighBoots();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );
			temp = new StuddedLegs();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );
			temp = new WoodenShield();
			AddItem( temp );
			temp = new WarAxe();
			AddItem( temp );
			temp = new FancyShirt();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );

			Female = Utility.RandomBool();
			if( Female )
			{
				Body = 0x191;
				Name = NameList.RandomName( "pixie" );
				AddItem( new LongHair( 0 ) );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "golem controller" );
				AddItem( new KrisnaHair( 0 ) );
				AddItem( new Goatee( 0 ) );
			}

			SetStr( 50, 70 );
			SetDex( 61, 80 );
			SetInt( 50, 60 );

			SetHits( 150, 200 );

			SetDamage( 10, 20 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 30, 40 );
			SetResistance( ResistanceType.Fire, 25, 35 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 5, 15 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.Archery, 60.0, 80.0 );
			SetSkill( SkillName.Fencing, 60.0, 80.0 );
			SetSkill( SkillName.Macing, 60.0, 80.0 );
			SetSkill( SkillName.Swords, 60.0, 80.0 );
			SetSkill( SkillName.MagicResist, 15.0, 38.0 );
			SetSkill( SkillName.Tactics, 60.0, 90.0 );
			SetSkill( SkillName.Wrestling, 25.0, 40.0 );

			Fame = Utility.RandomMinMax( 1000, 2000 );
			Karma = Utility.RandomMinMax( -3500, -2500 );

			VirtualArmor = 15;
		}

		public DarkElfGrunt( Serial serial ) : base( serial )
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

	[CorpseName( "a dark elf corpse" )]
	public class DarkElfFighter : BaseCreature
	{
		[Constructable]
		public DarkElfFighter() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Title = "the Dark Elf";
			Hue = 0x0597;

			Body = 0x190;
			Name = NameList.RandomName( "golem controller" );
			AddItem( new LongHair( 0 ) );
			AddItem( new Goatee( 0 ) );
			AddItem( new Sandals() );
			Item temp;
			temp = new BoneChest();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );
			temp = new BoneGloves();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );
			temp = new BoneHelm();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );
			temp = new ThighBoots();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );
			temp = new StuddedLegs();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );
			temp = new WoodenShield();
			AddItem( temp );
			temp = new FancyShirt();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );

			SetStr( 50, 70 );
			SetDex( 61, 80 );
			SetInt( 50, 60 );
			SetHits( 70, 90 );
			SetStam( 81, 95 );

			SetDamage( 10, 20 );
			SetSkill( SkillName.Archery, 60.0, 80.0 );
			SetSkill( SkillName.Fencing, 60.0, 80.0 );
			SetSkill( SkillName.MagicResist, 15.0, 38.0 );
			SetSkill( SkillName.Swords, 60.0, 80.0 );
			SetSkill( SkillName.Macing, 60.0, 80.0 );
			SetSkill( SkillName.Tactics, 60.0, 90.0 );
			SetSkill( SkillName.Wrestling, 25.0, 40.0 );

			Fame = Utility.RandomMinMax( 2500, 4000 );
			Karma = Utility.RandomMinMax( -4000, -3000 );

			VirtualArmor = 15;
			switch( Utility.Random( 2 ) )
			{
				case 0:
					AddItem( new Broadsword() );
					break;
				case 1:
					AddItem( new WarMace() );
					break;
			}
		}

		public DarkElfFighter( Serial serial ) : base( serial )
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
			AddLoot( LootPack.Average );
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

	[CorpseName( "a dark elf corpse" )]
	public class DarkElfWarrior : BaseCreature
	{
		[Constructable]
		public DarkElfWarrior() : base( AIType.AI_Archer, FightMode.Closest, 10, 3, 0.2, 0.4 )
		{
			Title = "the Dark Elf";
			Hue = 0x0597;
			Body = 0x190;
			Name = NameList.RandomName( "golem controller" );
			AddItem( new Goatee( 0 ) );
			AddItem( new LongHair( 0 ) );
			Item temp;
			temp = new BoneChest();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );
			temp = new BoneGloves();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );
			temp = new BoneHelm();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );
			temp = new ThighBoots();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );
			temp = new ChainLegs();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );
			temp = new Cloak();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );
			temp = new FancyShirt();
			temp.Hue = 0x0455;
			temp.Movable = false;
			AddItem( temp );

			SetStr( 70, 90 );
			SetDex( 81, 95 );
			SetInt( 70, 100 );
			SetHits( 80, 110 );
			SetStam( 81, 95 );
			SetDamage( 15, 30 );

			SetSkill( SkillName.Archery, 75.0, 100.0 );
			SetSkill( SkillName.Tactics, 75.0, 100.0 );
			SetSkill( SkillName.MagicResist, 15.0, 38.0 );
			SetSkill( SkillName.Macing, 75.0, 100.0 );
			SetSkill( SkillName.Parry, 40.0, 50.0 );
			SetSkill( SkillName.Swords, 75.0, 100.0 );
			SetSkill( SkillName.Wrestling, 25.0, 40.0 );
			VirtualArmor = 19;
			Fame = Utility.RandomMinMax( 3000, 4000 );
			Karma = Utility.RandomMinMax( -5000, -3000 );
			switch( Utility.Random( 2 ) )
			{
				case 0:
					AddItem( new TwoHandedAxe() );
					break;
				case 1:
					AddItem( new VikingSword() );
					AddItem( new MetalShield() );
					break;
			}
		}

		public DarkElfWarrior( Serial serial ) : base( serial )
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
}