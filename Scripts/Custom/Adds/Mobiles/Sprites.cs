using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a sprite corpse" )]
	public class Sprite : BaseCreature
	{
		[Constructable]
		public Sprite() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Sprite";
			Hue = 0x045e;
			Body = 0x191;
			Female = true;
			AddItem( new PonyTail( 0x0597 ) );

			SetStr( 86, 90 );
			SetDex( 91, 100 );
			SetInt( 71, 85 );

			SetHits( 86, 90 );
			SetStam( 91, 100 );
			SetMana( 0 );

			SetDamage( 6, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetSkill( SkillName.Fencing, 65.0, 88.0 );
			SetSkill( SkillName.MagicResist, 60.0, 80.0 );
			SetSkill( SkillName.Tactics, 75.0, 85.0 );
			SetSkill( SkillName.Wrestling, 35.0, 58.0 );

			Fame = Utility.RandomMinMax( 1200, 3200 );

			VirtualArmor = 5;

			Item temp;
			temp = new ThighBoots();
			temp.Hue = 0x0599;
			temp.Movable = false;
			AddItem( temp );
			temp = new FemaleLeatherChest();
			temp.Hue = 0x0599;
			temp.Movable = false;
			AddItem( temp );
			temp = new Cloak();
			temp.Hue = 0x0599;
			temp.Movable = false;
			AddItem( temp );
			temp = new Kryss();
			temp.Hue = 0x0599;
			temp.Movable = false;
			AddItem( temp );
			temp = new LeatherLegs();
			temp.Hue = 0x0599;
			temp.Movable = false;
			AddItem( temp );
		}

		public Sprite( Serial serial ) : base( serial )
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

	[CorpseName( "a sprite mage corpse" )]
	public class SpriteMage : BaseCreature
	{
		[Constructable]
		public SpriteMage() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Sprite Mage";
			Hue = 0x045e;
			Body = 0x191;
			Female = true;
			AddItem( new PonyTail( 0x0597 ) );

			SetStr( 86, 90 );
			SetDex( 91, 100 );
			SetInt( 71, 85 );

			SetHits( 86, 90 );
			SetStam( 91, 100 );
			SetMana( 100 );

			SetDamage( 6, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetSkill( SkillName.Magery, 65.0, 85.0 );
			SetSkill( SkillName.MagicResist, 60.0, 80.0 );

			SetSkill( SkillName.Wrestling, 58.0, 78.0 );

			Fame = Utility.RandomMinMax( 1200, 3200 );
			Karma = Utility.RandomMinMax( -1500, -6000 );

			VirtualArmor = 5;

			Item temp;
			temp = new Sandals();
			temp.Hue = 0x0599;
			temp.Movable = false;
			AddItem( temp );

			temp = new Robe();
			temp.Hue = 0x0599;
			temp.Movable = false;
			AddItem( temp );

            if (Utility.RandomDouble() <= 0.7)
            {
                Spellbook book = new Spellbook();
                book.Content = ulong.MaxValue;
                book.LootType = LootType.Regular;
                AddItem(book);
            }
		}

		public SpriteMage( Serial serial ) : base( serial )
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

	[CorpseName( "a sprite sorceress corpse" )]
	public class SpriteSorceress : BaseCreature
	{
		[Constructable]
		public SpriteSorceress() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Sprite Sorceress";
			Hue = 0x045e;
			Body = 0x191;
			Female = true;
			AddItem( new LongHair( 0x0597 ) );

			SetStr( 86, 90 );
			SetDex( 91, 100 );
			SetInt( 71, 85 );

			SetHits( 86, 95 );
			SetStam( 91, 100 );
			SetMana( 100 );

			SetDamage( 6, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetSkill( SkillName.Magery, 75.0, 95.0 );
			SetSkill( SkillName.MagicResist, 70.0, 85.0 );

			SetSkill( SkillName.Wrestling, 68.0, 88.0 );

			Fame = Utility.RandomMinMax( 1700, 3700 );
			Karma = Utility.RandomMinMax( -2000, -6500 );

			VirtualArmor = 5;

			Item temp;
			temp = new Sandals();
			temp.Hue = 0x0599;
			temp.Movable = false;
			AddItem( temp );
			temp = new WizardsHat();
			temp.Hue = 0x0599;
			temp.Movable = false;
			AddItem( temp );
			temp = new Robe();
			temp.Hue = 0x0595;
			temp.Movable = false;
			AddItem( temp );
		}

		public SpriteSorceress( Serial serial ) : base( serial )
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

	[CorpseName( "a sprite archer corpse" )]
	public class SpriteArcher : BaseCreature
	{
		[Constructable]
		public SpriteArcher() : base( AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Sprite Archer";
			Hue = 0x045e;
			Body = 0x191;
			Female = true;
			AddItem( new LongHair( 0x0597 ) );
			AddItem( new Arrow( Utility.Random( 20, 40 ) ) );

			SetStr( 86, 90 );
			SetDex( 91, 100 );
			SetInt( 71, 85 );

			SetHits( 86, 90 );
			SetStam( 91, 100 );
			SetMana( 0 );

			SetDamage( 6, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetSkill( SkillName.Archery, 65.0, 88.0 );
			SetSkill( SkillName.MagicResist, 50.0, 60.0 );
			SetSkill( SkillName.Tactics, 75.0, 85.0 );
			SetSkill( SkillName.Wrestling, 35.0, 58.0 );

			Fame = Utility.RandomMinMax( 1700, 3700 );
			Karma = Utility.RandomMinMax( -1500, -6000 );

			VirtualArmor = 5;

			Item temp;
			temp = new ThighBoots();
			temp.Hue = 0x0599;
			temp.Movable = false;
			AddItem( temp );
			temp = new LeatherChest();
			temp.Hue = 0x0595;
			temp.Movable = false;
			AddItem( temp );
			temp = new LeatherGloves();
			temp.Hue = 0x0595;
			temp.Movable = false;
			AddItem( temp );
			temp = new LeatherLegs();
			temp.Hue = 0x0595;
			temp.Movable = false;
			AddItem( temp );
			temp = new Cloak();
			temp.Hue = 0x0599;
			temp.Movable = false;
			AddItem( temp );
			temp = new Bow();

			AddItem( temp );
		}

		public SpriteArcher( Serial serial ) : base( serial )
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