using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a vampire count corpse" )]
	public class VampireCount : BaseCreature
	{
		[Constructable]
		public VampireCount() : base( AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Vampire Count";
			Body = 0x0190;
			Hue = 0x497;

			SetStr( 50 );
			SetDex( 50 );
			SetInt( 50 );

			SetHits( 50 );
			SetStam( 50 );

			SetDamage( 10, 20 );

			SetSkill( SkillName.Poisoning, 90.0, 100.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.MagicResist, 65.0, 88.0 );
			SetSkill( SkillName.Parry, 75.0, 98.0 );
			SetSkill( SkillName.Wrestling, 67.0, 90.0 );
			SetSkill( SkillName.DetectHidden, 90.0, 100.0 );
			SetSkill( SkillName.Swords, 150.0 );
            SetSkill(SkillName.EvalInt, 95.0, 100.0);

			Fame = 0;

			VirtualArmor = 10;

			//Here we add his loot. 
			//PackGold(1000, 1500);
			//PackItem(new VampireHeart());
			//PackItem(new VampireBone(10));

            HairItemID = 8252;
            HairHue = Utility.RandomHairHue();

			PlateGloves glo = new PlateGloves();
			glo.Hue = 0x1;
			PackItem( glo );
			Item temp = new Doublet( 0x1 );
			temp.Movable = false;
			AddItem( temp );
			temp = new ThighBoots( 0x1 );
			temp.Movable = false;
			AddItem( temp );
			temp = new FancyShirt( 0x0496 );
			temp.Movable = false;
			AddItem( temp );
			temp = new ShortPants( 0x1 );
			temp.Movable = false;
			AddItem( temp );
			temp = null;

            Spellbook book = new Spellbook();
            book.Content = ulong.MaxValue;
            book.LootType = LootType.Regular;
            AddItem(book);
		}

		public VampireCount( Serial serial ) : base( serial )
		{
		}

		public override bool AlwaysMurderer
		{
			get { return true; }
		}

		public override bool BardImmune
		{
			get { return true; }
		}

		public override bool ShowFameTitle
		{
			get { return false; }
		}

		public override bool ClickTitle
		{
			get { return false; }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.HighScrolls );
			AddLoot( LootPack.HighScrolls );
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