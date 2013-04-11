using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a dread lord corpse" )]
	public class DreadLord : BaseCreature
	{
		[Constructable]
		public DreadLord() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.1, 0.1 )
		{
			SpeechHue = 0;

			Hue = Utility.RandomSkinHue();
			Body = 0x190;
			Name = "Dread Lord";

			SetStr( 200, 250 );
			SetDex( 100, 150 );
			SetInt( 71, 85 );
			SetHits( 200, 250 );
			SetDex( 100, 150 );
			SetInt( 71, 85 );
			SetDamage( 10, 20 );

			SetSkill( SkillName.Parry, 75.0, 98.0 );
			SetSkill( SkillName.Poisoning, 90.0, 100.0 );
			SetSkill( SkillName.MagicResist, 65.0, 88.0 );
			SetSkill( SkillName.Swords, 80.0, 95.0 );
			SetSkill( SkillName.Tactics, 80.0, 98.0 );
			SetSkill( SkillName.Wrestling, 67.0, 90.0 );
			SetSkill( SkillName.DetectHidden, 90.0, 100.0 );

			Fame = 6000;
			Karma = -10000;
			VirtualArmor = 30;

			Item temp;
			temp = new PlateGloves();
			temp.Movable = false;
			AddItem( temp );
			temp = new PlateHelm();
			temp.Movable = false;
			AddItem( temp );
			temp = new PlateArms();
			temp.Movable = false;
			AddItem( temp );
			temp = new PlateGorget();
			temp.Movable = false;
			AddItem( temp );
			temp = new PlateLegs();
			temp.Movable = false;
			AddItem( temp );
			temp = new PlateChest();
			temp.Movable = false;
			AddItem( temp );
			temp = new VikingSword();
			temp.Movable = false;
			AddItem( temp );
			temp = new Cloak();
			temp.Hue = 0x0020;
			temp.Movable = true;
			AddItem( temp );
			temp = new BodySash();
			temp.Hue = 0x0020;
			temp.Movable = true;
			AddItem( temp );
			temp = new HalfApron();
			temp.Hue = 0x0020;
			temp.Movable = false;
			AddItem( temp );

			AddItem( new KrisnaHair( Utility.RandomHairHue() ) );

            Spellbook book = new Spellbook();
            book.Content = ulong.MaxValue;
            book.LootType = LootType.Regular;
            AddItem(book);
		}

		public DreadLord( Serial serial ) : base( serial )
		{
		}

		public override bool ClickTitle
		{
			get { return false; }
		}

		public override bool AlwaysMurderer
		{
			get { return true; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich);
            AddLoot(LootPack.Gems, 4);
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

	[CorpseName( "a dread lord captain corpse" )]
	public class DreadLordCaptain : BaseCreature
	{
		[Constructable]
		public DreadLordCaptain() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.1, 0.1 )
		{
			SpeechHue = Utility.RandomDyedHue();

			Hue = Utility.RandomSkinHue();

			Body = 0x190;
			Name = "Dread Lord Captain";

			SetStr( 400 );
			SetDex( 200, 250 );
			SetInt( 200, 250 );
			SetHits( 350, 400 );
			SetStam( 200, 250 );
			SetMana( 200, 250 );
			SetDamage( 10, 20 );

			SetSkill( SkillName.Parry, 85.0, 98.0 );
			SetSkill( SkillName.Wrestling, 67.0, 90.0 );
			SetSkill( SkillName.Magery, 90.0, 100.0 );
			SetSkill( SkillName.Poisoning, 90.0, 100.0 );
			SetSkill( SkillName.MagicResist, 65.0, 88.0 );
			SetSkill( SkillName.Swords, 90.0, 95.0 );
			SetSkill( SkillName.Tactics, 90.0, 98.0 );

			Fame = 7500;
			Karma = -10000;

			VirtualArmor = 40;

			Item temp;
			temp = new PlateGloves();
			temp.Movable = false;
			AddItem( temp );
			temp = new PlateHelm();
			temp.Movable = false;
			AddItem( temp );
			temp = new PlateArms();
			temp.Movable = false;
			AddItem( temp );
			temp = new PlateGorget();
			temp.Movable = false;
			AddItem( temp );
			temp = new PlateLegs();
			temp.Movable = false;
			AddItem( temp );
			temp = new PlateChest();
			temp.Movable = false;
			AddItem( temp );
			temp = new VikingSword();
			temp.Movable = false;
			AddItem( temp );
			temp = new Cloak();
			temp.Hue = 0x0493;
			temp.Movable = true;
			AddItem( temp );
			temp = new BodySash();
			temp.Hue = 0x0493;
			temp.Movable = true;
			AddItem( temp );
			temp = new HalfApron();
			temp.Hue = 0x1;
			temp.Movable = false;
			AddItem( temp );

			Item hair = new Item( Utility.RandomList( 0x203B, 0x2049, 0x2048, 0x204A ) );
			hair.Hue = Utility.RandomNondyedHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );
			//PackGold(800, 1200);
			PackGem( 4 );
		}

		public DreadLordCaptain( Serial serial ) : base( serial )
		{
		}

		public override bool AlwaysMurderer
		{
			get { return true; }
		}

		public override void GenerateLoot()
		{
			//AddLoot(LootPack.HighItems_Always, 1);
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.HighScrolls, 2 );
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