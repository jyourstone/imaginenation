using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a vampire corpse" )]
	public class Vampire : BaseCreature
	{
		[Constructable]
		public Vampire() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Hue = 0x497;
			Body = 0x190;
			Name = "Vampire";

			SetStr( 450 );
			SetDex( 200, 300 );
			SetInt( 150, 200 );
			SetHits( 200, 300 );
			SetStam( 200, 300 );
			SetMana( 150, 200 );
			SetDamage( 10, 20 );

			SetSkill( SkillName.Parry, 75.0, 80.0 );
			SetSkill( SkillName.Poisoning, 90.0, 100.0 );
			SetSkill( SkillName.MagicResist, 75.0, 88.0 );
			SetSkill( SkillName.Swords, 75.0, 85.0 );
			SetSkill( SkillName.Tactics, 80.0, 90.0 );
			SetSkill( SkillName.Wrestling, 67.0, 80.0 );
			SetSkill( SkillName.Magery, 65.0, 80.0 );
            SetSkill(SkillName.EvalInt, 75.0, 85.0);

			Fame = 4000;
			Karma = -4000;
			VirtualArmor = 20;

		    Item temp = new PlateGloves {Movable = false, Hue = 0x0590, Name = "Vampire Plate Gauntlets"};
		    AddItem( temp );
			temp = new PlateArms {Hue = 0x0845, Movable = false, Name = "Vampire Plate Arms"};
		    AddItem( temp );
			temp = new PlateGorget {Movable = false, Hue = 0x0590, Name = "Vampire Plate Gorget"};
		    AddItem( temp );
			temp = new PlateLegs {Movable = false, Hue = 0x0590, Name = "Vampire Plate Legs"};
		    AddItem( temp );
			temp = new PlateChest {Movable = false, Hue = 0x0590, Name = "Vampire Plate Chest"};
		    AddItem( temp );
			temp = new Cloak( 0x1 ) {Movable = false};
		    AddItem( temp );

		    HairItemID = 8252;
		    HairHue = Utility.RandomHairHue();

			BaseSword twep;
			switch( Utility.Random( 8 ) )
			{
				case 0:
					twep = new Broadsword();
					break;
				case 1:
					twep = new Cutlass();
					break;
				case 2:
					twep = new Scimitar();
					break;
				case 3:
					twep = new Katana();
					break;
				case 4:
					twep = new Kryss();
					break;
				case 5:
					twep = new Longsword();
					break;
				case 6:
					twep = new ThinLongsword();
					break;
				default:
					twep = new VikingSword();
					break;
			}
			switch( Utility.Random( 3 ) )
			{
				case 0:
					twep.DamageLevel = WeaponDamageLevel.Ruin;
					break;
				case 1:
					twep.DamageLevel = WeaponDamageLevel.Might;
					break;
				case 2:
					twep.DamageLevel = WeaponDamageLevel.Force;
					break;
			}

			AddItem( twep );
		}

		public Vampire( Serial serial ) : base( serial )
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
            AddItem(new Spellbook(ulong.MaxValue) {LootType = LootType.Regular});
            AddLoot(LootPack.FilthyRich);
			PackGem( 4 );
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