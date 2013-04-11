using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a vampire lord corpse" )]
	public class VampireLord : BaseCreature //BaseCreature
	{
		[Constructable]
		//AIType, Fight Mode, Range Perception, Fighting Range, Active Speed, Passive Speed
		public VampireLord() : base( AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Vampire Lord";
			Body = 0x0190;
			Hue = 0x497;

			SetStr( 750 );
			SetDex( 400, 500 );
			SetInt( 200, 250 );

			SetHits( 350, 400 );
			SetStam( 250, 300 );
			SetMana( 250, 300 );

			SetDamage( 20, 40 );

			SetSkill( SkillName.Poisoning, 90.0, 100.0 );
			SetSkill( SkillName.Tactics, 90.0, 98.0 );
			SetSkill( SkillName.MagicResist, 75.0, 88.0 );
			SetSkill( SkillName.Parry, 85.0, 98.0 );
			SetSkill( SkillName.Wrestling, 67.0, 90.0 );
			SetSkill( SkillName.DetectHidden, 90.0, 100.0 );
			SetSkill( SkillName.Swords, 90.0, 95.0 );
			SetSkill( SkillName.Magery, 95.0, 100.0 );
            SetSkill(SkillName.EvalInt, 95.0, 100.0);

			Fame = 3000;
			Karma = -10000;

			VirtualArmor = 40;

		    Item temp = new PlateGloves {Movable = false, Hue = 0x0492, Name = "Vampire Plate Gauntlets"};
		    AddItem( temp );
			temp = new PlateArms {Hue = 0x0492, Movable = false, Name = "Vampire Plate Arms"};
		    AddItem( temp );
			temp = new PlateGorget {Movable = false, Hue = 0x0492, Name = "Vampire Plate Gorget"};
		    AddItem( temp );
			temp = new PlateLegs {Movable = false, Hue = 0x0492, Name = "Vampire Plate Legs"};
		    AddItem( temp );
			temp = new PlateChest {Movable = false, Hue = 0x0492, Name = "Vampire Plate Chest"};
		    AddItem( temp );
			temp = new PlateHelm {Movable = false, Hue = 0x0492, Name = "Vampire Plate Helm"};
		    AddItem( temp );
			temp = new BodySash {Movable = false, Hue = 0x1, Name = "Vampire Sash"};
		    AddItem( temp );
			temp = new Cloak {Movable = false, Hue = 0x1};
		    AddItem( temp );

            BaseArmor tarm;
            if (Utility.RandomDouble() < 0.3)
            {
                switch (Utility.Random(6))
                {
                    case 0:
                        tarm = new PlateGloves {Hue = 0x492, Name = "Vampire Plate Gauntlets"};
                        break;
                    case 1:
                        tarm = new PlateArms {Hue = 0x492, Name = "Vampire Plate Arms"};
                        break;
                    case 2:
                        tarm = new PlateChest {Hue = 0x492, Name = "Vampire Plate Chest"};
                        break;
                    case 3:
                        tarm = new PlateHelm {Hue = 0x492, Name = "Vampire Plate Helm"};
                        break;
                    case 4:
                        tarm = new PlateLegs {Hue = 0x492, Name = "Vampire Plate Legs"};
                        break;
                    default:
                        tarm = new PlateGorget {Hue = 0x492, Name = "Vampire Plate Gorget"};
                        break;
                }

                tarm.ProtectionLevel = (ArmorProtectionLevel) Utility.RandomMinMax(0, 5);
                AddItem(tarm);
            }

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
                    twep.DamageLevel = WeaponDamageLevel.Might;
					break;
				case 1:
                    twep.DamageLevel = WeaponDamageLevel.Force;
					break;
				case 2:
					twep.DamageLevel = WeaponDamageLevel.Power;
					break;
			}

			AddItem( twep );
		}

		public VampireLord( Serial serial ) : base( serial )
		{
		}

		public override int TreasureMapLevel
		{
			get { return 5; }
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
            AddItem(new Spellbook(ulong.MaxValue) { LootType = LootType.Regular });
			AddItem( new BlankScroll( 3 ) );
			AddLoot( LootPack.FilthyRich );
            AddLoot(LootPack.MedScrolls, 2);
            PackGold(6000, 1000);
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