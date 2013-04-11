using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a witch corpse" )]
	public class Witch : BaseCreature
	{
		[Constructable]
		public Witch() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "female" );
			Title = "the Witch";
			Body = 0x0191;
			Hue = 0x0599;
			Female = true;
			SetStr( 70, 90 );
			SetDex( 81, 95 );
			SetInt( 80, 100 );

			SetHits( 70, 90 );
			SetStam( 81, 95 );
			SetMana( 100 );
			SetDamage( 15, 20 );

			AddItem( new LongHair( Utility.RandomHairHue() ) );

			Item temp;
			temp = new FancyShirt( 0 );
			temp.Movable = false;
			AddItem( temp );

			temp = new Skirt( 1 );
			temp.Movable = false;
			AddItem( temp );

			temp = new WizardsHat( 1 );
			temp.Movable = false;
			AddItem( temp );

			temp = new ThighBoots( 1 );
			temp.Movable = false;
			AddItem( temp );

			temp = new GnarledStaff();
			AddItem( temp );
			//PackGold(50, 80);
			AddItem( new EyesOfNewt( Utility.RandomMinMax( 1, 5 ) ) );
			AddItem( new BatWing( Utility.RandomMinMax( 1, 5 ) ) );
			PackPotion();
			PackPotion();

			SetSkill( SkillName.MagicResist, 80.0, 90.0 );
			SetSkill( SkillName.Magery, 65.0, 85.0 );
			SetSkill( SkillName.Macing, 55.0, 75.0 );
			SetSkill( SkillName.Tactics, 70.0, 90.0 );
			SetSkill( SkillName.Wrestling, 40.0, 55.0 );

			Fame = Utility.RandomMinMax( 2000, 4000 );
			Karma = Utility.RandomMinMax( -3000, -5000 );

			VirtualArmor = 16;

			PackItem( new Sandals() );

            if (Utility.RandomDouble() <= 0.7)
            {
                Spellbook book = new Spellbook();
                book.Content = ulong.MaxValue;
                book.LootType = LootType.Regular;
                AddItem(book);
            }
		}

		public Witch( Serial serial ) : base( serial )
		{
		}

		public override bool CanRummageCorpses
		{
			get { return true; }
		}

		public override bool AlwaysMurderer
		{
			get { return true; }
		}

		public override int Meat
		{
			get { return 1; }
		}

		public override int TreasureMapLevel
		{
			get { return 1; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.MedScrolls );
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

	[CorpseName( "a witch corpse" )]
	public class HighWitch : BaseCreature
	{
		[Constructable]
		public HighWitch() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "female" );
			Title = "the High Witch";
			Body = 0x0191;
			Hue = 0x0599;
			Female = true;
			SetStr( 90, 110 );
			SetDex( 81, 95 );
			SetInt( 80, 100 );

			SetHits( 90, 110 );
			SetStam( 81, 95 );
			SetMana( 100 );
			SetDamage( 15, 20 );

			AddItem( new LongHair( Utility.RandomHairHue() ) );

			Item temp;
			temp = new PlainDress( 1 );
			temp.Movable = false;
			AddItem( temp );

			temp = new WizardsHat( 1 );
			temp.Movable = false;
			AddItem( temp );

			temp = new ThighBoots( 1 );
			temp.Movable = false;
			AddItem( temp );

			temp = new BlackStaff();
			AddItem( temp );
			//PackGold(100, 200);
			AddItem( new EyesOfNewt( Utility.RandomMinMax( 1, 5 ) ) );
			AddItem( new BatWing( 2 ) );
			PackPotion();
			PackPotion();

			SetSkill( SkillName.MagicResist, 90.0, 95.0 );
			SetSkill( SkillName.Magery, 75.0, 95.0 );
			SetSkill( SkillName.Macing, 75.0, 95.0 );
			SetSkill( SkillName.Tactics, 80.0, 90.0 );
			SetSkill( SkillName.Wrestling, 40.0, 55.0 );

			Fame = -2000;
			Karma = -6000;

			VirtualArmor = 16;

			PackItem( new Sandals() );
		}

		public HighWitch( Serial serial ) : base( serial )
		{
		}

		public override bool CanRummageCorpses
		{
			get { return true; }
		}

		public override bool AlwaysMurderer
		{
			get { return true; }
		}

		public override int Meat
		{
			get { return 1; }
		}

		public override int TreasureMapLevel
		{
			get { return Core.AOS ? 1 : 0; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.MedScrolls );
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

	[CorpseName( "a witch corpse" )]
	public class BloodStoneWitch : BaseCreature
	{
		[Constructable]
		public BloodStoneWitch() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "female" );
			Title = "the Blood Stone Witch";
			Body = 0x0191;
			Hue = 0x0493;
			Female = true;
			SetStr( 100, 150 );
			SetDex( 100, 150 );
			SetInt( 200 );

			SetHits( 100, 150 );
			SetStam( 100, 150 );
			SetMana( 200 );
			SetDamage( 30, 40 );

			AddItem( new LongHair( 1 ) );

			Item temp;
			temp = new FancyShirt( 0 );
			temp.Movable = false;
			AddItem( temp );

			temp = new Skirt( 1 );
			temp.Movable = false;
			AddItem( temp );

			temp = new WizardsHat( 1 );
			temp.Movable = false;
			AddItem( temp );

			temp = new ThighBoots( 1 );
			temp.Movable = false;
			AddItem( temp );

			temp = new GnarledStaff();
			AddItem( temp );
			//PackGold(100, 200);

			SetSkill( SkillName.MagicResist, 80.0, 90.0 );
			SetSkill( SkillName.Magery, 100.0, 150.0 );
			SetSkill( SkillName.Macing, 55.0, 75.0 );
			SetSkill( SkillName.Tactics, 70.0, 90.0 );
			SetSkill( SkillName.Wrestling, 40.0, 55.0 );

			Fame = Utility.RandomMinMax( 2000, 4000 );
			Karma = Utility.RandomMinMax( -3000, -5000 );

			VirtualArmor = 45;

			PackItem( new Sandals() );
		}

		public BloodStoneWitch( Serial serial ) : base( serial )
		{
		}

		public override bool CanRummageCorpses
		{
			get { return true; }
		}

		public override bool AlwaysMurderer
		{
			get { return true; }
		}

		public override int Meat
		{
			get { return 1; }
		}

		public override int TreasureMapLevel
		{
			get { return Core.AOS ? 1 : 0; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.MedScrolls );
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

	[CorpseName( "a witch corpse" )]
	public class BloodStoneHighWitch : BaseCreature
	{
		[Constructable]
		public BloodStoneHighWitch() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "female" );
			Title = "the Blood Stone High Witch";
			Body = 0x0191;
			Hue = 0x0493;
			Female = true;
			SetStr( 200, 250 );
			SetDex( 100, 200 );
			SetInt( 300 );

			SetHits( 200, 250 );
			SetStam( 100, 200 );
			SetMana( 300 );
			SetDamage( 40, 50 );

			AddItem( new LongHair( Utility.RandomHairHue() ) );

			Item temp;
			temp = new PlainDress( 1 );
			temp.Movable = false;
			AddItem( temp );

			temp = new WizardsHat( 1 );
			temp.Movable = false;
			AddItem( temp );

			temp = new ThighBoots( 1 );
			temp.Movable = false;
			AddItem( temp );

			temp = new BlackStaff();
			AddItem( temp );
			//PackGold(200, 300);
			AddItem( new EyesOfNewt( Utility.RandomMinMax( 1, 5 ) ) );
			AddItem( new BatWing( 2 ) );
			PackPotion();
			PackPotion();

			SetSkill( SkillName.MagicResist, 90.0, 95.0 );
			SetSkill( SkillName.Magery, 200.0, 250.0 );
			SetSkill( SkillName.Macing, 75.0, 95.0 );
			SetSkill( SkillName.Tactics, 80.0, 90.0 );
			SetSkill( SkillName.Wrestling, 40.0, 55.0 );

			Fame = -2000;
			Karma = -6000;

			VirtualArmor = 55;

			PackItem( new Sandals() );
		}

		public BloodStoneHighWitch( Serial serial ) : base( serial )
		{
		}

		public override bool CanRummageCorpses
		{
			get { return true; }
		}

		public override bool AlwaysMurderer
		{
			get { return true; }
		}

		public override int Meat
		{
			get { return 1; }
		}

		public override int TreasureMapLevel
		{
			get { return Core.AOS ? 1 : 0; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.MedScrolls );
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