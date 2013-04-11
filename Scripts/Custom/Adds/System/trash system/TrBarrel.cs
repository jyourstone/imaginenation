using System;
using System.Collections.Generic;
using Server.Multis;
using Xanthos.ShrinkSystem;

namespace Server.Items
{
	public class MiniRewardCan : Item
	{	
        private Item m_TrashedItem;
	    private int trashedReward;

	    public Item trashedItem
	    {
	        get { return m_TrashedItem; }
	    }

		[Constructable]
		public MiniRewardCan() : base( 0xE7A )
		{
			Name = "reward trash can";
		    LootType = LootType.Blessed;
			Movable = true;
			Hue = 2997;	
		}
		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if (dropped.LootType == LootType.Blessed || dropped.LootType == LootType.Newbied || dropped is Gold || dropped is TrTokens || dropped is BoardingVoucher || dropped is ChaosCoin || dropped is OrderCoin || dropped is GauntletToken || dropped is StarWarToken || dropped is BankCheck || dropped is ImagineNickel || dropped is ShrinkItem || dropped is Key || dropped is Container && TotalItems >= 1)
			{
				from.SendMessage( 38,"You can not trash that for a reward!");
				return false;
			}

            int prize = GetSellPriceFor( dropped );
            int totalPrize = prize;

            while (prize > 65000)
            {
                from.AddToBackpack(new TrTokens(65000));
                prize -= 65000;
            }

			from.AddToBackpack( new TrTokens( prize ) );
			from.SendMessage( 53, "You got {0} silver coins for your trash!", totalPrize);
			dropped.Internalize();
            if (m_TrashedItem != null)
                m_TrashedItem.Delete();
            m_TrashedItem = dropped;
            trashedReward = totalPrize;
			return true;
		}

		public int GetSellPriceFor( Item item )
		{
			int price = 1;


            if (item is Arrow || item is GoldOre || item is AmethystOre || item is VeriteOre || item is RoseOre || item is SilverOre || item is FishingBait || item is BarrelLid || item is SheafOfHay || item is WheatSheaf || item is MandrakeRoot || item is Garlic || item is Ginseng || item is Nightshade || item is SulfurousAsh || item is SpidersSilk || item is NightSightPotion || item is LesserCurePotion || item is RefreshPotion || item is AgilityPotion || item is LesserHealPotion || item is PoisonPotion || item is LesserPoisonPotion || item is HealPotion || item is CurePotion || item is LesserExplosionPotion || item is GreaterAgilityPotion || item is Muffins || item is Board || item is Sandals || item is BreadLoaf || item is Lettuce || item is Cabbage || item is Hides || item is BlankMap || item is Bandana || item is SkullCap || item is BlankScroll || item is SextantParts)
            {
                price = 2;
            }
            else if (item is WeakenScroll || item is ReactiveArmorScroll || item is NightSightScroll || item is MagicArrowScroll || item is ClumsyScroll || item is FeeblemindScroll || item is CreateFoodScroll || item is HealScroll || item is Torch || item is OakLog || item is OakBoard || item is BloodRockOre || item is HavocOre || item is IceOre || item is ValoriteOre || item is Vase || item is BallotBoxDeed || item is DartBoardSouthDeed || item is DartBoardEastDeed || item is DartBoard || item is BarrelStaves || item is BlackPearl || item is Bloodmoss || item is Bottle || item is MortarPestle || item is Cookies || item is BeverageBottle || item is Candle || item is Hammer || item is ChickenLeg || item is ApplePie || item is Watermelon || item is HoneydewMelon || item is Cantaloupe || item is CityMap || item is LocalMap || item is WorldMap || item is PresetMap || item is Bag || item is Pouch || item is Bolt || item is Leather || item is TinkerTools || item is IronIngot)
            {
                price = 3;
            }
            else if (item is TricorneHat || item is ChainCoif || item is MahoganyLog || item is MahoganyBoard || item is OldCopperIngot || item is ShadowIronIngot || item is SilverIngot || item is BarrelHoops || item is Dyes || item is DyeTub || item is CheesePizza || item is Dough || item is Shoes || item is LambLeg || item is MapmakersPen || item is ScribesPen || item is BodySash || item is WideBrimHat)
            {
                price = 4;
            }
            else if (item is CunningScroll || item is CureScroll || item is HarmScroll || item is MagicTrapScroll || item is MagicUnTrapScroll || item is ProtectionScroll || item is StrengthScroll || item is AgilityScroll || item is FeatheredHat || item is ButcherKnife || item is SkinningKnife || item is Cleaver || item is Clay || item is Sand || item is AshLog || item is AshBoard || item is Granite || item is FireOre || item is AquaOre || item is VeriteIngot || item is RoseIngot || item is InvisibilityPotion || item is Cake || item is PeachCobbler || item is Pitcher || item is SmallCrate || item is Boots || item is Pumpkin || item is DrawKnife || item is Froe || item is Inshave || item is SmoothingPlane || item is Scorp)
            {
                price = 5;
            }
            else if (item is Shovel || item is Club || item is MytherilOre || item is CedarLog || item is CedarBoard || item is GoldIngot || item is IceIngot || item is HavocIngot || item is Bansuriflute || item is Kavalflute || item is ShortMusicStand || item is HalfApron || item is Jug || item is Sextant || item is DovetailSaw || item is Shirt || item is ShortPants || item is Lockpick || item is Quiche || item is Ribs || item is Scissors || item is MediumCrate || item is WoodenThrone || item is Stool || item is FootStool || item is BambooChair || item is WoodenBench || item is JointingPlane || item is MouldingPlane || item is WizardsHat )
            {
                price = 6;
            }
            else if (item is BlessScroll || item is FireballScroll || item is MagicLockScroll || item is PoisonScroll || item is TelekinisisScroll || item is TeleportScroll || item is UnlockScroll || item is WallOfStoneScroll || item is Backpack || item is GnarledStaff || item is WillowLog || item is WillowBoard || item is OldCopperGranite || item is ShadowIronGranite || item is SandRockOre || item is BlackDiamondOre || item is FloppyHat || item is StrawHat || item is Tongs || item is WoodenBox || item is LargeCrate || item is Nightstand || item is ThighBoots || item is FishingPole || item is BlueBook || item is RedBook || item is BrownBook || item is TanBook || item is Saw)
            {
                price = 7;
            }
            else if (item is GreaterPoisonPotion || item is AmethystIngot || item is QuarterStaff || item is Pitchfork || item is Maul || item is Dagger || item is Bascinet || item is NorseHelm || item is CloseHelm || item is Helmet || item is YewBoard || item is YewLog || item is OceanicOre || item is BlackRockOre || item is ValoriteIngot || item is BloodRockIngot || item is WoodenChair || item is CookedBird || item is RecallRune || item is LongPants || item is FancyShirt || item is Kilt || item is Doublet)
            {
                price = 8;
            }
            else if (item is RecallScroll || item is ArchCureScroll || item is ArchProtectionScroll || item is CurseScroll || item is FireFieldScroll || item is GreaterHealScroll || item is LightningScroll || item is ManaDrainScroll || item is TallStrawHat || item is SmithHammer || item is ShepherdsCrook || item is Pickaxe || item is BlackStaff || item is PlateHelm || item is LeatherSkirt || item is LeatherBustierArms || item is SilverGranite || item is VeriteGranite || item is RoseGranite || item is DaemonSteelOre || item is ReactiveOre || item is AquaIngot || item is FireIngot || item is LargeVase || item is StatueEast || item is StatueNorth || item is StatueWest || item is DarkYarn || item is LightYarn || item is LightYarnUnraveled || item is Spellbook || item is SpoolOfThread)
            {
                price = 9;
            }
            else if (item is SilverRing || item is WarFork || item is LunarBone || item is Cutlass || item is WarHammer || item is ShortSpear || item is SapphireOre || item is AdamantiumOre || item is MytherilIngot || item is EmptyVial || item is AniRedRibbedFlask || item is AniSmallBlueFlask || item is CurvedFlask || item is BlueCurvedFlask || item is GreenCurvedFlask || item is LtBlueCurvedFlask || item is RedCurvedFlask || item is EmptyCurvedFlaskE || item is EmptyCurvedFlaskW || item is SmallBlueFlask || item is SmallFlask || item is SmallRedFlask || item is SmallYellowFlask || item is SeaGrass || item is Easle || item is fiddle || item is Bubenclacker || item is Lullabylute || item is Suspenselute || item is Seductionlute || item is TallMusicStand || item is Taikodrum || item is WarDrum || item is LapHarp || item is Lute || item is Drums || item is Tambourine || item is Harp || item is LargeTable || item is YewWoodTable || item is WoodenChairCushion || item is Blowpipe || item is SilverNecklace || item is SilverBeadNecklace || item is SilverBracelet || item is SilverEarrings || item is PlainDress || item is Skirt || item is FullApron)
            {
                price = 10;
            }
            else if (item is BladeSpiritsScroll || item is DispelFieldScroll || item is IncognitoScroll || item is MagicReflectScroll || item is MindBlastScroll || item is ParalyzeScroll || item is PoisonFieldScroll || item is SummonCreatureScroll || item is Cap || item is ThinLongsword || item is HammerPick || item is BattleAxe || item is SandRockIngot || item is GoldGranite || item is StatuePegasus || item is Clock || item is Bonnet)
            {
                price = 11;
            }
            else if (item is Mace || item is WarAxe || item is LeatherShorts || item is BlackDiamondIngot || item is FullVialsWRack || item is LargeFlask || item is EmptyRibbedFlask || item is MediumFlask || item is TambourineTassel || item is Ocarina || item is FancyWoodenChairCushion || item is CheeseWheel || item is FancyDress || item is Tunic || item is Wool)
            {
                price = 12;
            }
            else if (item is DispelScroll || item is ExplosionScroll || item is InvisibilityScroll || item is MarkScroll || item is EnergyBoltScroll || item is MassCurseScroll || item is ParalyzeFieldScroll || item is RevealScroll || item is GoldRing || item is Spear || item is WarMace || item is ExecutionersAxe || item is HeartwoodBoard || item is HeartwoodLog || item is MystWoodLog || item is MystWoodBoard || item is FrostwoodBoard || item is FrostwoodLog || item is BloodwoodBoard || item is BloodwoodLog || item is BlackRockIngot || item is ValoriteGranite || item is Necklace || item is GoldNecklace || item is GoldBeadNecklace || item is Beads || item is GoldBracelet || item is GoldEarrings)
            {
                price = 13;
            }
            else if (item is Cloak || item is ElvenBow || item is Kryss || item is Katana || item is TwoHandedAxe || item is LargeBattleAxe ||item is WoodenShield || item is OceanicIngot || item is LargeVioletFlask || item is AniLargeVioletFlask || item is LargeYellowFlask || item is RedRibbedFlask || item is VioletRibbedFlask || item is LargeEmptyFlask || item is Dressform || item is Surcoat || item is GreaterStrengthPotion || item is TotalRefreshPotion || item is ExplosionPotion)
            {
                price = 14;
            }
            else if (item is ChainLightningScroll || item is EnergyFieldScroll || item is FlamestrikeScroll || item is GateTravelScroll || item is ManaVampireScroll || item is MassDispelScroll || item is MeteorSwarmScroll || item is PolymorphScroll || item is EarthquakeScroll || item is EnergyVortexScroll || item is ResurrectionScroll || item is SummonAirElementalScroll || item is SummonDaemonScroll || item is SummonEarthElementalScroll || item is SummonFireElementalScroll || item is SummonWaterElementalScroll || item is JesterHat || item is Broadsword || item is Bow || item is DaemonSteelIngot || item is Hourglass || item is SmallFishies || item is FishBones || item is Keg || item is WoodenChest || item is Traditionaltambourine || item is RitualTambourine)
            {
                price = 15;
            }
            else if (item is Robe || item is Scimitar || item is FemaleLeatherChest || item is ReactiveIngot)
            {
                price = 16;
            }
            else if (item is Armoire || item is FancyArmoire || item is EmptyBookcase)
            {
                price = 17;
            }
            else if (item is MediumStoneTableEastDeed || item is Axe || item is MediumStoneTableSouthDeed)
            {
                price = 18;
            }
            else if (item is HairDye || item is Halberd)
            {
                price = 19;
            }
            else if (item is GreaterCurePotion || item is AdamantiumIngot || item is SapphireIngot || item is SpinningHourglass || item is HourglassAni || item is SunkenShip || item is YellowTang || item is Coral || item is Shrimp || item is QueenAngelfish || item is Pufferfish || item is FlameAngelfish || item is Jellyfish || item is PowderblueTang || item is FantailGoldfish || item is Crab || item is CopperbandedButterflyfish || item is BrineShrimp || item is BlueFish || item is BaronessButterflyfish || item is Butterflyfish || item is AlbinoAngelfish || item is Angelfish || item is AnteniAngelfish || item is Anthias || item is TrashBarrel || item is Madroneharp || item is Walnutharp)
            {
                price = 20;
            }
            else if (item is ShipwreckedItem || item is Crossbow)
            {
                price = 21;
            }
            else if (item is FemaleStuddedChest)
            {
                price = 23;
            }
            else if (item is Throne || item is DoubleAxe || item is StuddedBustierArms || item is Buckler || item is JesterSuit)
            {
                price = 24;
            }
            else if (item is Amber || item is VikingSword || item is Longsword || item is HeavyCrossbow || item is RingmailGloves || item is SeveredAnchor || item is Pearl || item is Citrine || item is BambooShark)
            {
                price = 25;
            }
            else if (item is GreaterHealPotion || item is DeadlyPoisonPotion || item is GreaterExplosionPotion)
            {
                price = 26;
            }
            else if (item is LargeStoneTableEastDeed || item is LargeStoneTableSouthDeed)
            {
                price = 27;
            }
            else if (item is Trumpet || item is Bardiche)
            {
                price = 28;
            }
            else if (item is ArcheryButteDeed)
            {
                price = 30;
            }
            else if (item is WoodenKiteShield || item is BronzeShield)
            {
                price = 32;
            }
            else if (item is Ruby)
            {
                price = 37;
            }
            else if (item is TrainingDummySouthDeed || item is TrainingDummyEastDeed)
            {
                price = 38;
            }
            else if (item is RingmailArms || item is GlassSkull || item is SmallForgeDeed || item is Flax || item is Cotton)
            {
                price = 40;
            }
            else if (item is SpinningwheelEastDeed || item is RingmailLegs || item is SpinningwheelSouthDeed)
            {
                price = 42;
            }
            else if (item is FullBookcase || item is PickpocketDipEastDeed || item is PickpocketDipSouthDeed)
            {
                price = 45;
            }
            else if (item is Tourmaline || item is LoomSouthDeed || item is LoomEastDeed)
            {
                price = 47;
            }
            else if (item is Amethyst || item is PlateGorget || item is MysticFishingNet || item is WaterTroughEastDeed || item is WaterTroughSouthDeed || item is Sapphire || item is BoltOfCloth || item is Emerald)
            {
                price = 50;
            }
            else if (item is LargeForgeEastDeed || item is LargeForgeSouthDeed)
            {
                price = 52;
            }
            else if (item is RoastPig)
            {
                price = 53;
            }
            else if (item is MetalShield || item is RingmailChest)
            {
                price = 59;
            }
            else if (item is Pandrum || item is MetalKiteShield)
            {
                price = 60;
            }
            else if (item is StarSapphire)
            {
                price = 62;
            }
            else if (item is BulletinBoard || item is PlayerBBSouthDeed || item is PlayerBBEastDeed)
            {
                price = 65;
            }
            else if (item is ChainChest)
		    {
		        price = 68;
		    }
            else if (item is PlateGloves || item is ChainLegs )
            {
                price = 70;
            }
            else if (item is FlourMillEastDeed || item is PentagramDeed || item is AbbatoirDeed || item is FlourMillSouthDeed || item is SmallBedEastDeed || item is SmallBedSouthDeed)
            {
                price = 75;
            }
            else if (item is AnvilEastDeed || item is AnvilSouthDeed)
            {
                price = 78;
            }
            else if (item is PlateArms || item is LunarGloves || item is ElvenLeatherGorget || item is ElvenLeatherGloves || item is DragonScalemailGloves || item is DragonScalemailGorget || item is UndineGloves)
            {
                price = 90;
            }
            else if (item is LunarArms || item is UndineArms || item is DragonScalemailCap || item is DragonScalemailArms || item is ElvenLeatherCap || item is ElvenLeatherArms || item is LunarHelm | item is UndineHelm)
            {
                price = 92;
            }
            else if (item is LunarLegs || item is UndineLegs || item is DragonScalemailSkirt || item is DragonScalemailLegs || item is ElvenLeatherLegs)
            {
                price = 95;
            }
            else if (item is Diamond || item is LunarChest || item is DragonScalemailFemaleChest || item is DragonScalemailChest || item is UndineChest || item is ElvenLeatherChest)
            {
                price = 100;
            }
            else if (item is GrayBrickFireplaceSouthDeed || item is PlateLegs || item is GrayBrickFireplaceEastDeed || item is SandstoneFireplaceSouthDeed || item is SandstoneFireplaceEastDeed || item is StoneFireplaceSouthDeed || item is StoneFireplaceEastDeed || item is StoneOvenEastDeed || item is StoneOvenSouthDeed)
            {
                price = 105;
            }
            else if (item is LargeBedEastDeed || item is FemalePlateChest || item is HeaterShield || item is LargeBedSouthDeed)
            {
                price = 110;
            }
            else if (item is PerformersHarpAddonDeed || item is PlateChest)
            {
                price = 120;
            }
            else if (item is PianoAddonDeed)
            {
                price = 150;
            }
            else if (item is Mirror || item is WeaversSpool)
            {
                price = 200;
            }
            else if (item is HousePlacementTool)
            {
                price = 300;
            }
            else if (item is ContractOfEmployment)
            {
                price = 600;
            }
            else if (item is SmallBoatDeed || item is SmallDragonBoatDeed )
            {
                price = 750;
            }
            else if (item is MediumDragonBoatDeed || item is MediumBoatDeed)
            {
                price = 825;
            }
            else if (item is LargeBoatDeed || item is LargeDragonBoatDeed)
            {
                price = 900;
            }
            else if (item is MasonryBook || item is GlassblowingBook || item is SandMiningBook || item is StoneMiningBook || item is InteriorDecorator)
            {
                price = 4500;
            }
            else if (item is GuildDeed)
            {
                price = 6200;
            }
            else if (item is GuildContainerDeed)
            {
                price = 20000;
            }
            if (item.Amount >= 2)
            {
                price = price * item.Amount;
            }
            if (item is BaseWeapon) //add a way to increase value for custom weps
            {
                BaseWeapon weapon = (BaseWeapon)item;

                //price += 49 * (int)weapon.DurabilityLevel;     // lower number = less reward coins
                price += 65 * (int)weapon.DamageLevel;         // gives roughly 98% of what a vendor would give
                price += 35 * (int)weapon.AccuracyLevel;

                if (weapon.Quality == WeaponQuality.Low)
                    price = (int)(price * 0.60);
                else if (weapon.Quality == WeaponQuality.Exceptional)
                    price = (int)(price * 1.15);
                else if (weapon.Hue != 0)
                    price = (int) (price*1.20);

                if (price < 2)
                    price = 2;
            }
            if (item is BaseArmor) //add a way to add for resource type, ie: sapphire
            {
                BaseArmor armor = (BaseArmor)item;

                price += 30 * (int)armor.Durability;           // lower number = less reward coins
                price += 52 * (int)armor.ProtectionLevel;      // lower number = less reward coins

                if (armor.Quality == ArmorQuality.Low)
                    price = (int)(price * 0.60);
                else if (armor.Quality == ArmorQuality.Exceptional)
                    price = (int)(price * 1.15);
                else if (armor.Resource == CraftResource.OldCopper)
                    price = (int)(price * 1.05);
                else if (armor.Resource == CraftResource.ShadowIron)
                    price = (int)(price * 1.08);
                else if (armor.Resource == CraftResource.Silver)
                    price = (int) (price * 1.11);
                else if (armor.Resource == CraftResource.Verite)
                    price = (int)(price * 1.14);
                else if (armor.Resource == CraftResource.Rose)
                    price = (int)(price * 1.17);
                else if (armor.Resource == CraftResource.Gold)
                    price = (int)(price * 1.20);
                else if (armor.Resource == CraftResource.Ice)
                    price = (int)(price * 1.23);
                else if (armor.Resource == CraftResource.Havoc)
                    price = (int)(price * 1.23);
                else if (armor.Resource == CraftResource.Valorite)
                    price = (int)(price * 1.26);
                else if (armor.Resource == CraftResource.BloodRock)
                    price = (int)(price * 1.29);
                else if (armor.Resource == CraftResource.Aqua)
                    price = (int)(price * 1.32);
                else if (armor.Resource == CraftResource.Fire)
                    price = (int)(price * 1.33);
                else if (armor.Resource == CraftResource.Mytheril)
                    price = (int)(price * 1.35);
                else if (armor.Resource == CraftResource.SandRock)
                    price = (int)(price * 1.38);
                else if (armor.Resource == CraftResource.BlackDiamond)
                    price = (int)(price * 1.42);
                else if (armor.Resource == CraftResource.BlackRock)
                    price = (int)(price * 1.45);
                else if (armor.Resource == CraftResource.Oceanic)
                    price = (int)(price * 1.50);
                else if (armor.Resource == CraftResource.DaemonSteel)
                    price = (int)(price * 1.60);
                else if (armor.Resource == CraftResource.Reactive)
                    price = (int)(price * 1.60);
                else if (armor.Resource == CraftResource.Sapphire)
                    price = (int)(price * 1.75);
                else if (armor.Resource == CraftResource.Adamantium)
                    price = (int)(price * 1.75);

                if (price < 2)
                    price = 2;
            }
			return price;
		}

		public MiniRewardCan( Serial serial ) : base( serial )
		{
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Backpack == null)
                return;

            if (m_TrashedItem != null && !m_TrashedItem.Deleted)
            {
                List<TrTokens> silverPiles = new List<TrTokens>();
                int totalSilver = 0;
                
                for (int i = 0; i < from.Backpack.Items.Count; i++ )
                {
                    if (from.Backpack.Items[i] is TrTokens)
                    {
                        silverPiles.Add(from.Backpack.Items[i] as TrTokens);
                        totalSilver += from.Backpack.Items[i].Amount;
                    }
                }

                if (totalSilver >= trashedReward)
                {
                    int leftToPay = trashedReward;

                    while (leftToPay > 0)
                    {
                        for (int i = 0; i < silverPiles.Count; i++)
                        {
                            TrTokens silver = silverPiles[i];

                            if (leftToPay > silver.Amount)
                            {
                                leftToPay -= silver.Amount;
                                silver.Delete();
                            }
                            else
                            {
                                try
                                {
                                    silver.Consume(leftToPay);
                                    leftToPay = 0;
                                    if (from.AddToBackpack(m_TrashedItem))
                                        from.SendAsciiMessage("You put the trashed item in your backpack");

                                    from.SendAsciiMessage("You get your trashed item back for {0} silver", trashedReward);
                                    m_TrashedItem = null;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(String.Format("Trashcan error:" + e));
                                }
                            }
                        }
                    }
                }
                else
                {
                    from.SendAsciiMessage("You need more silver to get your item back");
                    return;
                }
            }
            else
                from.SendAsciiMessage("Cannot find a trashed item");
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