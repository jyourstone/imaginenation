using System;
using Server.Accounting;
using Server.Factions;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Misc
{
	public class CharacterCreation
	{

		public static void Initialize()
		{
			// Register our event handler
			EventSink.CharacterCreated += EventSink_CharacterCreated;
		}

		private static void AddBackpack( Mobile m )
		{
			Container pack = m.Backpack;

			if ( pack == null )
			{
				pack = new Backpack();
				pack.Movable = false;

				m.AddItem( pack );
			}

			PackItem( new Gold( 3000 ) ); // Starting gold can be customized here
			PackItem( new MiniRewardCan() );
			PackItem( new Dagger() );
			PackItem( new Spellbook() );
            PackItem( new KeyRing());
            PackItem( new PlayerGuide());
            PackItem( new Scissors());
		
		}

		private static Item MakeBlessed( Item item )
		{
			if ( !Core.AOS )
				item.LootType = LootType.Blessed;

			return item;
		}

		private static void PlaceItemIn( Container parent, int x, int y, Item item )
		{
			parent.AddItem( item );
			item.Location = new Point3D( x, y, 0 );
		}

		private static Item MakePotionKeg( PotionEffect type, int hue )
		{
			PotionKeg keg = new PotionKeg();

			keg.Held = 100;
			keg.Type = type;
			keg.Hue = hue;

			return MakeBlessed( keg );
		}

		private static void FillBankAOS( Mobile m )
		{
			BankBox bank = m.BankBox;

			// The new AOS bankboxes don't have powerscrolls, they are automatically 'applied':

			for ( int i = 0; i < PowerScroll.Skills.Count; ++i )
				m.Skills[PowerScroll.Skills[ i ]].Cap = 120.0;

			m.StatCap = 250;


			Container cont;


			// Begin box of money
			cont = new WoodenBox();
			cont.ItemID = 0xE7D;
			cont.Hue = 0x489;

			PlaceItemIn( cont, 16, 51, new BankCheck( 500000 ) );
			PlaceItemIn( cont, 28, 51, new BankCheck( 250000 ) );
			PlaceItemIn( cont, 40, 51, new BankCheck( 100000 ) );
			PlaceItemIn( cont, 52, 51, new BankCheck( 100000 ) );
			PlaceItemIn( cont, 64, 51, new BankCheck(  50000 ) );

			PlaceItemIn( cont, 16, 115, new Silver( 9000 ) );
			PlaceItemIn( cont, 34, 115, new Gold( 60000 ) );

			PlaceItemIn( bank, 18, 169, cont );
			// End box of money


			// Begin bag of potion kegs
			cont = new Backpack();
			cont.Name = "Various Potion Kegs";

			PlaceItemIn( cont,  45, 149, MakePotionKeg( PotionEffect.CureGreater, 0x2D ) );
			PlaceItemIn( cont,  69, 149, MakePotionKeg( PotionEffect.HealGreater, 0x499 ) );
			PlaceItemIn( cont,  93, 149, MakePotionKeg( PotionEffect.PoisonDeadly, 0x46 ) );
			PlaceItemIn( cont, 117, 149, MakePotionKeg( PotionEffect.RefreshTotal, 0x22 ) );
			PlaceItemIn( cont, 141, 149, MakePotionKeg( PotionEffect.ExplosionGreater, 0x74 ) );

			PlaceItemIn( cont, 93, 82, new Bottle( 1000 ) );

			PlaceItemIn( bank, 53, 169, cont );
			// End bag of potion kegs


			// Begin bag of tools
			cont = new Bag();
			cont.Name = "Tool Bag";

			PlaceItemIn( cont, 30,  35, new TinkerTools( 1000 ) );
			PlaceItemIn( cont, 60,  35, new HousePlacementTool() );
			PlaceItemIn( cont, 90,  35, new DovetailSaw( 1000 ) );
			PlaceItemIn( cont, 30,  68, new Scissors() );
			PlaceItemIn( cont, 45,  68, new MortarPestle( 1000 ) );
			PlaceItemIn( cont, 75,  68, new ScribesPen( 1000 ) );
			PlaceItemIn( cont, 90,  68, new SmithHammer( 1000 ) );
			PlaceItemIn( cont, 30, 118, new TwoHandedAxe() );
			PlaceItemIn( cont, 60, 118, new FletcherTools( 1000 ) );
			PlaceItemIn( cont, 90, 118, new SewingKit( 1000 ) );

			PlaceItemIn( cont, 36, 51, new RunicHammer( CraftResource.DullCopper, 1000 ) );
			PlaceItemIn( cont, 42, 51, new RunicHammer( CraftResource.ShadowIron, 1000 ) );
			PlaceItemIn( cont, 48, 51, new RunicHammer( CraftResource.Copper, 1000 ) );
			PlaceItemIn( cont, 54, 51, new RunicHammer( CraftResource.Bronze, 1000 ) );
			PlaceItemIn( cont, 61, 51, new RunicHammer( CraftResource.Gold, 1000 ) );
			PlaceItemIn( cont, 67, 51, new RunicHammer( CraftResource.Agapite, 1000 ) );
			PlaceItemIn( cont, 73, 51, new RunicHammer( CraftResource.Verite, 1000 ) );
			PlaceItemIn( cont, 79, 51, new RunicHammer( CraftResource.Valorite, 1000 ) );

			PlaceItemIn( cont, 36, 55, new RunicSewingKit( CraftResource.SpinedLeather, 1000 ) );
			PlaceItemIn( cont, 42, 55, new RunicSewingKit( CraftResource.HornedLeather, 1000 ) );
			PlaceItemIn( cont, 48, 55, new RunicSewingKit( CraftResource.BarbedLeather, 1000 ) );

			PlaceItemIn( bank, 118, 169, cont );
			// End bag of tools


			// Begin bag of archery ammo
			cont = new Bag();
			cont.Name = "Bag Of Archery Ammo";

			PlaceItemIn( cont, 48, 76, new Arrow( 5000 ) );
			PlaceItemIn( cont, 72, 76, new Bolt( 5000 ) );

			PlaceItemIn( bank, 118, 124, cont );
			// End bag of archery ammo


			// Begin bag of treasure maps
			cont = new Bag();
			cont.Name = "Bag Of Treasure Maps";

			PlaceItemIn( cont, 30, 35, new TreasureMap( 1, Map.Trammel ) );
			PlaceItemIn( cont, 45, 35, new TreasureMap( 2, Map.Trammel ) );
			PlaceItemIn( cont, 60, 35, new TreasureMap( 3, Map.Trammel ) );
			PlaceItemIn( cont, 75, 35, new TreasureMap( 4, Map.Trammel ) );
			PlaceItemIn( cont, 90, 35, new TreasureMap( 5, Map.Trammel ) );
			PlaceItemIn( cont, 90, 35, new TreasureMap( 6, Map.Trammel ) );

			PlaceItemIn( cont, 30, 50, new TreasureMap( 1, Map.Trammel ) );
			PlaceItemIn( cont, 45, 50, new TreasureMap( 2, Map.Trammel ) );
			PlaceItemIn( cont, 60, 50, new TreasureMap( 3, Map.Trammel ) );
			PlaceItemIn( cont, 75, 50, new TreasureMap( 4, Map.Trammel ) );
			PlaceItemIn( cont, 90, 50, new TreasureMap( 5, Map.Trammel ) );
			PlaceItemIn( cont, 90, 50, new TreasureMap( 6, Map.Trammel ) );

			PlaceItemIn( cont, 55, 100, new Lockpick( 30 ) );
			PlaceItemIn( cont, 60, 100, new Pickaxe() );

			PlaceItemIn( bank, 98, 124, cont );
			// End bag of treasure maps


			// Begin bag of raw materials
			cont = new Bag();
			cont.Hue = 0x835;
			cont.Name = "Raw Materials Bag";

			//PlaceItemIn( cont, 92, 60, new BarbedLeather( 5000 ) );
			//PlaceItemIn( cont, 92, 68, new HornedLeather( 5000 ) );
			//PlaceItemIn( cont, 92, 76, new SpinedLeather( 5000 ) );
			PlaceItemIn( cont, 92, 84, new Leather( 5000 ) );

			PlaceItemIn( cont, 30, 118, new Cloth( 5000 ) );
			PlaceItemIn( cont, 30,  84, new Board( 5000 ) );
			PlaceItemIn( cont, 57,  80, new BlankScroll( 500 ) );

			PlaceItemIn( cont, 30,  35, new DullCopperIngot( 5000 ) );
			PlaceItemIn( cont, 37,  35, new ShadowIronIngot( 5000 ) );
			PlaceItemIn( cont, 44,  35, new CopperIngot( 5000 ) );
			PlaceItemIn( cont, 51,  35, new BronzeIngot( 5000 ) );
			PlaceItemIn( cont, 58,  35, new GoldIngot( 5000 ) );
			PlaceItemIn( cont, 65,  35, new AgapiteIngot( 5000 ) );
			PlaceItemIn( cont, 72,  35, new VeriteIngot( 5000 ) );
			PlaceItemIn( cont, 79,  35, new ValoriteIngot( 5000 ) );
			PlaceItemIn( cont, 86,  35, new IronIngot( 5000 ) );

			PlaceItemIn( cont, 30,  59, new RedScales( 5000 ) );
			PlaceItemIn( cont, 36,  59, new YellowScales( 5000 ) );
			PlaceItemIn( cont, 42,  59, new BlackScales( 5000 ) );
			PlaceItemIn( cont, 48,  59, new GreenScales( 5000 ) );
			PlaceItemIn( cont, 54,  59, new WhiteScales( 5000 ) );
			PlaceItemIn( cont, 60,  59, new BlueScales( 5000 ) );

			PlaceItemIn( bank, 98, 169, cont );
			// End bag of raw materials


			// Begin bag of spell casting stuff
			cont = new Backpack();
			cont.Hue = 0x480;
			cont.Name = "Spell Casting Stuff";

//			PlaceItemIn( cont, 45, 105, new Spellbook( UInt64.MaxValue ) );
			PlaceItemIn( cont, 65, 105, new NecromancerSpellbook( (UInt64)0xFFFF ) );
			PlaceItemIn( cont, 85, 105, new BookOfChivalry( (UInt64)0x3FF ) );
			PlaceItemIn( cont, 105, 105, new BookOfBushido() );	//Default ctor = full
			PlaceItemIn( cont, 125, 105, new BookOfNinjitsu() ); //Default ctor = full

			Runebook runebook = new Runebook( 10 );
			runebook.CurCharges = runebook.MaxCharges;
			PlaceItemIn( cont, 145, 105, runebook );

			Item toHue = new BagOfReagents( 150 );
			toHue.Hue = 0x2D;
			PlaceItemIn( cont, 45, 150, toHue );

			toHue = new BagOfNecroReagents( 150 );
			toHue.Hue = 0x488;
			PlaceItemIn( cont, 65, 150, toHue );

			PlaceItemIn( cont, 140, 150, new BagOfAllReagents( 500 ) );

			for ( int i = 0; i < 9; ++i )
				PlaceItemIn( cont, 45 + (i * 10), 75, new RecallRune() );

			PlaceItemIn( cont, 141, 74, new FireHorn() );

			PlaceItemIn( bank, 78, 169, cont );
			// End bag of spell casting stuff


			// Begin bag of ethereals
			cont = new Backpack();
			cont.Hue = 0x490;
			cont.Name = "Bag Of Ethy's!";

			PlaceItemIn( cont, 45, 66, new EtherealHorse() );
			PlaceItemIn( cont, 69, 82, new EtherealOstard() );
			PlaceItemIn( cont, 93, 99, new EtherealLlama() );
			PlaceItemIn( cont, 117, 115, new EtherealKirin() );
			PlaceItemIn( cont, 45, 132, new EtherealUnicorn() );
			PlaceItemIn( cont, 69, 66, new EtherealRidgeback() );
			PlaceItemIn( cont, 93, 82, new EtherealSwampDragon() );
			PlaceItemIn( cont, 117, 99, new EtherealBeetle() );

			PlaceItemIn( bank, 38, 124, cont );
			// End bag of ethereals


			// Begin first bag of artifacts
			cont = new Backpack();
			cont.Hue = 0x48F;
			cont.Name = "Bag of Artifacts";

			PlaceItemIn( cont, 45, 66, new TitansHammer() );
			PlaceItemIn( cont, 69, 82, new InquisitorsResolution() );
			PlaceItemIn( cont, 93, 99, new BladeOfTheRighteous() );
			PlaceItemIn( cont, 117, 115, new ZyronicClaw() );

			PlaceItemIn( bank, 58, 124, cont );
			// End first bag of artifacts


			// Begin second bag of artifacts
			cont = new Backpack();
			cont.Hue = 0x48F;
			cont.Name = "Bag of Artifacts";

			PlaceItemIn( cont, 45, 66, new GauntletsOfNobility() );
			PlaceItemIn( cont, 69, 82, new MidnightBracers() );
			PlaceItemIn( cont, 93, 99, new VoiceOfTheFallenKing() );
			PlaceItemIn( cont, 117, 115, new OrnateCrownOfTheHarrower() );
			PlaceItemIn( cont, 45, 132, new HelmOfInsight() );
			PlaceItemIn( cont, 69, 66, new HolyKnightsBreastplate() );
			PlaceItemIn( cont, 93, 82, new ArmorOfFortune() );
			PlaceItemIn( cont, 117, 99, new TunicOfFire() );
			PlaceItemIn( cont, 45, 115, new LeggingsOfBane() );
			PlaceItemIn( cont, 69, 132, new ArcaneShield() );
			PlaceItemIn( cont, 93, 66, new Aegis() );
			PlaceItemIn( cont, 117, 82, new RingOfTheVile() );
			PlaceItemIn( cont, 45, 99, new BraceletOfHealth() );
			PlaceItemIn( cont, 69, 115, new RingOfTheElements() );
			PlaceItemIn( cont, 93, 132, new OrnamentOfTheMagician() );
			PlaceItemIn( cont, 117, 66, new DivineCountenance() );
			PlaceItemIn( cont, 45, 82, new JackalsCollar() );
			PlaceItemIn( cont, 69, 99, new HuntersHeaddress() );
			PlaceItemIn( cont, 93, 115, new HatOfTheMagi() );
			PlaceItemIn( cont, 117, 132, new ShadowDancerLeggings() );
			PlaceItemIn( cont, 45, 66, new SpiritOfTheTotem() );
			PlaceItemIn( cont, 69, 82, new BladeOfInsanity() );
			PlaceItemIn( cont, 93, 99, new AxeOfTheHeavens() );
			PlaceItemIn( cont, 117, 115, new TheBeserkersMaul() );
			PlaceItemIn( cont, 45, 132, new Frostbringer() );
			PlaceItemIn( cont, 69, 66, new BreathOfTheDead() );
			PlaceItemIn( cont, 93, 82, new TheDragonSlayer() );
			PlaceItemIn( cont, 117, 99, new BoneCrusher() );
			PlaceItemIn( cont, 45, 115, new StaffOfTheMagi() );
			PlaceItemIn( cont, 69, 132, new SerpentsFang() );
			PlaceItemIn( cont, 93, 66, new LegacyOfTheDreadLord() );
			PlaceItemIn( cont, 117, 82, new TheTaskmaster() );
			PlaceItemIn( cont, 45, 99, new TheDryadBow() );

			PlaceItemIn( bank, 78, 124, cont );
			// End second bag of artifacts

			// Begin bag of minor artifacts
			cont = new Backpack();
			cont.Hue = 0x48F;
			cont.Name = "Bag of Minor Artifacts";


			PlaceItemIn( cont, 45, 66, new LunaLance() );
			PlaceItemIn( cont, 69, 82, new VioletCourage() );
			PlaceItemIn( cont, 93, 99, new CavortingClub() );
			PlaceItemIn( cont, 117, 115, new CaptainQuacklebushsCutlass() );
			PlaceItemIn( cont, 45, 132, new NightsKiss() );
			PlaceItemIn( cont, 69, 66, new ShipModelOfTheHMSCape() );
			PlaceItemIn( cont, 93, 82, new AdmiralsHeartyRum() );
			PlaceItemIn( cont, 117, 99, new CandelabraOfSouls() );
			PlaceItemIn( cont, 45, 115, new IolosLute() );
			PlaceItemIn( cont, 69, 132, new GwennosHarp() );
			PlaceItemIn( cont, 93, 66, new ArcticDeathDealer() );
			PlaceItemIn( cont, 117, 82, new EnchantedTitanLegBone() );
			PlaceItemIn( cont, 45, 99, new NoxRangersHeavyCrossbow() );
			PlaceItemIn( cont, 69, 115, new BlazeOfDeath() );
			PlaceItemIn( cont, 93, 132, new DreadPirateHat() );
			PlaceItemIn( cont, 117, 66, new BurglarsBandana() );
			PlaceItemIn( cont, 45, 82, new GoldBricks() );
			PlaceItemIn( cont, 69, 99, new AlchemistsBauble() );
			PlaceItemIn( cont, 93, 115, new PhillipsWoodenSteed() );
			PlaceItemIn( cont, 117, 132, new PolarBearMask() );
			PlaceItemIn( cont, 45, 66, new BowOfTheJukaKing() );
			PlaceItemIn( cont, 69, 82, new GlovesOfThePugilist() );
			PlaceItemIn( cont, 93, 99, new OrcishVisage() );
			PlaceItemIn( cont, 117, 115, new StaffOfPower() );
			PlaceItemIn( cont, 45, 132, new ShieldOfInvulnerability() );
			PlaceItemIn( cont, 69, 66, new HeartOfTheLion() );
			PlaceItemIn( cont, 93, 82, new ColdBlood() );
			PlaceItemIn( cont, 117, 99, new GhostShipAnchor() );
			PlaceItemIn( cont, 45, 115, new SeahorseStatuette() );
			PlaceItemIn( cont, 69, 132, new WrathOfTheDryad() );
			PlaceItemIn( cont, 93, 66, new PixieSwatter() );

			for( int i = 0; i < 10; i++ )
                PlaceItemIn(cont, 117, 128, new MessageInABottle(Utility.RandomBool() ? Map.Trammel : Map.Felucca, 4));

			PlaceItemIn( bank, 18, 124, cont );

            if (Core.SE)
            {
				cont = new Bag();
				cont.Hue = 0x501;
				cont.Name = "Tokuno Minor Artifacts";

				PlaceItemIn( cont, 42, 70, new Exiler() );
				PlaceItemIn( cont, 38, 53, new HanzosBow() );
				PlaceItemIn( cont, 45, 40, new TheDestroyer() );
				PlaceItemIn( cont, 92, 80, new DragonNunchaku() );
				PlaceItemIn( cont, 42, 56, new PeasantsBokuto() );
				PlaceItemIn( cont, 44, 71, new TomeOfEnlightenment() );
                PlaceItemIn(cont, 35, 35, new ChestOfHeirlooms());
                PlaceItemIn(cont, 29, 0, new HonorableSwords());
				PlaceItemIn( cont, 49, 85, new AncientUrn() );
				PlaceItemIn( cont, 51, 58, new FluteOfRenewal() );
				PlaceItemIn( cont, 70, 51, new PigmentsOfTokuno() );
				PlaceItemIn( cont, 40, 79, new AncientSamuraiDo() );
				PlaceItemIn( cont, 51, 61, new LegsOfStability() );
				PlaceItemIn( cont, 88, 78, new GlovesOfTheSun() );
				PlaceItemIn( cont, 55, 62, new AncientFarmersKasa() );
				PlaceItemIn( cont, 55, 83, new ArmsOfTacticalExcellence() );
				PlaceItemIn( cont, 50, 85, new DaimyosHelm() );
				PlaceItemIn( cont, 52, 78, new BlackLotusHood() );
				PlaceItemIn( cont, 52, 79, new DemonForks() );
				PlaceItemIn( cont, 33, 49, new PilferedDancerFans() );

				PlaceItemIn( bank, 58, 124, cont );
			}

			if( Core.SE )	//This bag came only after SE.
			{
				cont = new Bag();
				cont.Name = "Bag of Bows";

				PlaceItemIn( cont, 31, 84, new Bow() );
				PlaceItemIn( cont, 78, 74, new CompositeBow() );
				PlaceItemIn( cont, 53, 71, new Crossbow() );
				PlaceItemIn( cont, 56, 39, new HeavyCrossbow() );
				PlaceItemIn( cont, 82, 72, new RepeatingCrossbow() );
				PlaceItemIn( cont, 49, 45, new Yumi() );

				for( int i = 0; i < cont.Items.Count; i++ )
				{
					BaseRanged bow = cont.Items[i] as BaseRanged;

					if( bow != null )
					{
						bow.Attributes.WeaponSpeed = 35;
						bow.Attributes.WeaponDamage = 35;
					}
				}

				PlaceItemIn( bank, 108, 135, cont );
			}
		}

		private static void FillBankbox( Mobile m )
		{
			if ( Core.AOS )
			{
				FillBankAOS( m );
				return;
			}

			BankBox bank = m.BankBox;

			bank.DropItem( new BankCheck( 1000000 ) );

			// Full spellbook
			Spellbook book = new Spellbook();

			book.Content = ulong.MaxValue;

			bank.DropItem( book );

			Bag bag = new Bag();

			for ( int i = 0; i < 5; ++i )
				bag.DropItem( new Moonstone( MoonstoneType.Felucca ) );

			// Felucca moonstones
			bank.DropItem( bag );

			bag = new Bag();

			for ( int i = 0; i < 5; ++i )
				bag.DropItem( new Moonstone( MoonstoneType.Trammel ) );

			// Trammel moonstones
			bank.DropItem( bag );

			// Treasure maps
			bank.DropItem( new TreasureMap( 1, Map.Trammel ) );
			bank.DropItem( new TreasureMap( 2, Map.Trammel ) );
			bank.DropItem( new TreasureMap( 3, Map.Trammel ) );
			bank.DropItem( new TreasureMap( 4, Map.Trammel ) );
			bank.DropItem( new TreasureMap( 5, Map.Trammel ) );

			// Bag containing 50 of each reagent
			bank.DropItem( new BagOfReagents( 50 ) );

			// Craft tools
			bank.DropItem( MakeBlessed( new Scissors() ) );
			bank.DropItem( MakeBlessed( new SewingKit( 1000 ) ) );
			bank.DropItem( MakeBlessed( new SmithHammer( 1000 ) ) );
			bank.DropItem( MakeBlessed( new FletcherTools( 1000 ) ) );
			bank.DropItem( MakeBlessed( new DovetailSaw( 1000 ) ) );
			bank.DropItem( MakeBlessed( new MortarPestle( 1000 ) ) );
			bank.DropItem( MakeBlessed( new ScribesPen( 1000 ) ) );
			bank.DropItem( MakeBlessed( new TinkerTools( 1000 ) ) );

			// A few dye tubs
			bank.DropItem( new Dyes() );
			bank.DropItem( new DyeTub() );
			bank.DropItem( new DyeTub() );
			bank.DropItem( new BlackDyeTub() );

			DyeTub darkRedTub = new DyeTub();

			darkRedTub.DyedHue = 0x485;
			darkRedTub.Redyable = false;

			bank.DropItem( darkRedTub );

			// Some food
			bank.DropItem( MakeBlessed( new Apple( 1000 ) ) );

			// Resources
			bank.DropItem( MakeBlessed( new Feather( 1000 ) ) );
			bank.DropItem( MakeBlessed( new BoltOfCloth( 1000 ) ) );
			bank.DropItem( MakeBlessed( new BlankScroll( 1000 ) ) );
			bank.DropItem( MakeBlessed( new Hides( 1000 ) ) );
			bank.DropItem( MakeBlessed( new Bandage( 1000 ) ) );
			bank.DropItem( MakeBlessed( new Bottle( 1000 ) ) );
			bank.DropItem( MakeBlessed( new Log( 1000 ) ) );

			bank.DropItem( MakeBlessed( new IronIngot( 5000 ) ) );
			bank.DropItem( MakeBlessed( new DullCopperIngot( 5000 ) ) );
			bank.DropItem( MakeBlessed( new ShadowIronIngot( 5000 ) ) );
			bank.DropItem( MakeBlessed( new CopperIngot( 5000 ) ) );
			bank.DropItem( MakeBlessed( new BronzeIngot( 5000 ) ) );
			bank.DropItem( MakeBlessed( new GoldIngot( 5000 ) ) );
			bank.DropItem( MakeBlessed( new AgapiteIngot( 5000 ) ) );
			bank.DropItem( MakeBlessed( new VeriteIngot( 5000 ) ) );
			bank.DropItem( MakeBlessed( new ValoriteIngot( 5000 ) ) );

			// Reagents
			bank.DropItem( MakeBlessed( new BlackPearl( 1000 ) ) );
			bank.DropItem( MakeBlessed( new Bloodmoss( 1000 ) ) );
			bank.DropItem( MakeBlessed( new Garlic( 1000 ) ) );
			bank.DropItem( MakeBlessed( new Ginseng( 1000 ) ) );
			bank.DropItem( MakeBlessed( new MandrakeRoot( 1000 ) ) );
			bank.DropItem( MakeBlessed( new Nightshade( 1000 ) ) );
			bank.DropItem( MakeBlessed( new SulfurousAsh( 1000 ) ) );
			bank.DropItem( MakeBlessed( new SpidersSilk( 1000 ) ) );

			// Some extra starting gold
			bank.DropItem( MakeBlessed( new Gold( 9000 ) ) );

			// 5 blank recall runes
			for ( int i = 0; i < 5; ++i )
				bank.DropItem( MakeBlessed( new RecallRune() ) );

			AddPowerScrolls( bank );
		}

		private static void AddPowerScrolls( BankBox bank )
		{
			Bag bag = new Bag();

			for ( int i = 0; i < PowerScroll.Skills.Count; ++i )
				bag.DropItem( new PowerScroll( PowerScroll.Skills[i], 120.0 ) );

			bag.DropItem( new StatCapScroll( 250 ) );

			bank.DropItem( bag );
		}

		private static void AddShirt( Mobile m, int shirtHue )
		{
			int hue = Utility.ClipDyedHue( shirtHue & 0x3FFF );

			if ( m.Race == Race.Elf )
			{
				EquipItem( new ElvenShirt( hue ), true );
			}
			else
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0: EquipItem( new Shirt( hue ), true ); break;
					case 1: EquipItem( new FancyShirt( hue ), true ); break;
					case 2: EquipItem( new Doublet( hue ), true ); break;
				}
			}
		}

		private static void AddPants( Mobile m, int pantsHue )
		{
			int hue = Utility.ClipDyedHue( pantsHue & 0x3FFF );

			if ( m.Race == Race.Elf )
			{
				EquipItem( new ElvenPants( hue ), true );
			}
			else
			{
				if ( m.Female )
				{
					switch ( Utility.Random( 2 ) )
					{
						case 0: EquipItem( new Skirt( hue ), true ); break;
						case 1: EquipItem( new Kilt( hue ), true ); break;
					}
				}
				else
				{
					switch ( Utility.Random( 2 ) )
					{
						case 0: EquipItem( new LongPants( hue ), true ); break;
						case 1: EquipItem( new ShortPants( hue ), true ); break;
					}
				}
			}
		}

		private static void AddShoes( Mobile m )
		{
			if( m.Race == Race.Elf )
				EquipItem( new ElvenBoots(), true );
			else
				EquipItem( new Shoes( Utility.RandomYellowHue() ), true );
		}

		private static Mobile CreateMobile( Account a )
		{
			if ( a.Count >= a.Limit )
				return null;

			for ( int i = 0; i < a.Length; ++i )
			{
				if ( a[i] == null )
					return (a[i] = new PlayerMobile());
			}

			return null;
		}

		private static void EventSink_CharacterCreated( CharacterCreatedEventArgs args )
		{
			if( !VerifyProfession( args.Profession ) )
				args.Profession = 0;

            NetState state = args.State;

            if (state == null)
                return;

			Mobile newChar = CreateMobile( args.Account as Account );

			if( newChar == null )
			{
                Console.WriteLine("Login: {0}: Character creation failed, account full", state);
                return;
			}

			args.Mobile = newChar;
			m_Mobile = newChar;

			newChar.Player = true;
			newChar.AccessLevel = args.Account.AccessLevel;
			newChar.Female = args.Female;
			//newChar.Body = newChar.Female ? 0x191 : 0x190;

			if( Core.Expansion >= args.Race.RequiredExpansion )
				newChar.Race = args.Race; //Sets body
			else
				newChar.Race = Race.DefaultRace;

			//newChar.Hue = Utility.ClipSkinHue( args.Hue & 0x3FFF ) | 0x8000;
			newChar.Hue = newChar.Race.ClipSkinHue( args.Hue & 0x3FFF ) | 0x8000;

			newChar.Hunger = 20;

			bool young = false;

			PlayerMobile pm = null;
			if( newChar is PlayerMobile )
			{
				pm = (PlayerMobile)newChar;

				pm.Profession = args.Profession;

				if( pm.AccessLevel == AccessLevel.Player && ((Account)pm.Account).Young )
					young = pm.Young = true;

                #region Nasir - Set Original Values For RaceReset

			    pm.OriginalHairHue = args.HairHue;
			    pm.OriginalHairItemID = args.HairID;
			    pm.OriginalHue = args.Hue;

                #endregion
            }

			SetName( newChar, args.Name );

            //We use player backpacks on INX

            Container pB = new PlayerBackpack(newChar);
            pB.Movable = false;
            newChar.AddItem(pB);


			AddBackpack( newChar );

            //SetStats(newChar, state, args.Str, args.Dex, args.Int);
            //SetSkills( newChar, args.Skills, args.Profession );

            // Max stats
            newChar.Str = 95; newChar.Int = 95; newChar.Dex = 95;
            newChar.Hits = 95; newChar.Mana = 95; newChar.Stam = 95;
			
            Race race = newChar.Race;

			if( race.ValidateHair( newChar, args.HairID ) )
			{
				newChar.HairItemID = args.HairID;
				newChar.HairHue = race.ClipHairHue( args.HairHue & 0x3FFF );
			}

			if( race.ValidateFacialHair( newChar, args.BeardID ) )
			{
				newChar.FacialHairItemID = args.BeardID;
				newChar.FacialHairHue = race.ClipHairHue( args.BeardHue & 0x3FFF );
			}

			if( args.Profession <= 3 )
			{
				AddShirt( newChar, args.ShirtHue );
				AddPants( newChar, args.PantsHue );
				AddShoes( newChar );
			}

			if( TestCenter.Enabled )
				FillBankbox( newChar );

			if( young)
			{
				NewPlayerTicket ticket = new NewPlayerTicket();
				ticket.Owner = newChar;
				newChar.BankBox.DropItem( ticket );
			}

			//IN:X Settings
			newChar.StatCap = 300;
			newChar.SkillsCap = 54000; //Iza - changed from 52000 to 54000 for proper all skills cap.

			//CityInfo city = GetStartLocation( args, false );
            CityInfo city = new CityInfo("Britain", "Bank", 1424, 1697, 10, Map.Felucca);

			newChar.MoveToWorld( city.Location, city.Map );

			//if ((newChar.Account).AccessLevel <= AccessLevel.GameMaster)
			//{
			//    (newChar).AccessLevel = AccessLevel.GameMaster;
			//    (newChar.Account).AccessLevel = AccessLevel.GameMaster;
			//}

			//Unicode//ASCII speech
			Account acct = newChar.Account as Account;
			if( pm != null && acct != null && acct.HardwareInfo != null )
			{
				string lang = acct.HardwareInfo.Language.ToUpper();

				//Ugliness...list.Contains()?
				if( lang.Equals( "ENG" ) || lang.Equals( "ENA" ) || lang.Equals( "ENC" ) || lang.Equals( "ENZ" ) || lang.Equals( "ENT" ) || lang.Equals( "ENU" ) || lang.Equals( "ENI" ) || lang.Equals( "ENW" ) || lang.Equals( "ENI" ) || lang.Equals( "ENS" ) || lang.Equals( "ENJ" ) || lang.Equals( "ENB" ) || lang.Equals( "ENL" ) || lang.Equals( "ENP" ) )
					pm.UseUnicodeSpeech = false;
				else
					pm.UseUnicodeSpeech = true;
			}
			else if( pm != null )
				pm.UseUnicodeSpeech = true;

            Console.WriteLine("Login: {0}: New character being created (account={1})", state, args.Account.Username);
            Console.WriteLine(" - Character: {0} (serial={1})", newChar.Name, newChar.Serial);
			Console.WriteLine( " - Started: {0} {1} in {2}", city.City, city.Location, city.Map );

//			BetaChar( newChar );
//			new WelcomeTimer( newChar ).Start();
		}

/*		public static void BetaChar( Mobile m )
		{
			m.SendAsciiMessage( "More beta items have been placed in your bank." );

			m.Str = 100;
			m.Int = 100;
			m.Dex = 100;
			m.Hits = 100;
			m.Mana = 100;
			m.Stam = 100;

			for( int i = 0; i < 51; ++i )
				m.Skills[i].Base = 100;

			Bag bag = new Bag();

			CraftResource resource = (CraftResource)Utility.Random( 2, 20 );
			CraftResource resource2 = (CraftResource)Utility.Random( 2, 20 );
			CraftResource resource3 = (CraftResource)Utility.Random( 2, 20 );

			foreach( BaseArmor i in SupplySystem.DefaultArmorList( m ) )
			{
				i.Resource = resource2;
				bag.DropItem( i );
			}
			foreach( BaseArmor i in SupplySystem.DefaultArmorList( m ) )
			{
				i.Resource = resource3;
				bag.DropItem( i );
			}
			m.BankBox.DropItem( bag );

			foreach( BaseArmor i in SupplySystem.DefaultArmorList( m ) )
			{
				i.Resource = resource;
				m.AddToBackpack( i );
			}

			for( int i = 0; i < 15; ++i )
			{
				BaseWeapon weap = BaseWeapon.CreateRandomWeapon();

				weap.DamageLevel = (WeaponDamageLevel)Utility.Random( 1, 5 );
				weap.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random( 1, 5 );

				if( i == 0 )
					m.AddToBackpack( weap );
				else
					m.BankBox.DropItem( weap );
			}

			m.AddToBackpack( new Gold( 30000 ) );
			m.BankBox.DropItem( new Gold( Utility.Random( 10000, 60000 ) ) );

			m.AddToBackpack( new BagOfReagents( 200 ) );
			m.AddToBackpack( new BagOfScrolls( 50 ) );

			List<Item> toRemove = new List<Item>();
			foreach( Item i in m.Items )
				if( i is Spellbook )
					toRemove.Add( i );

			foreach( Item i in m.Backpack.Items )
				if( i is Spellbook )
					toRemove.Add( i );

			foreach( Item i in toRemove )
				i.Delete();

			Spellbook book = new Spellbook();
			if( book.BookCount == 64 )
				book.Content = ulong.MaxValue;
			else
				book.Content = ( 1ul << book.BookCount ) - 1;

			m.AddToBackpack( book );
		}*/

		public static bool VerifyProfession( int profession )
		{
			if ( profession < 0 )
				return false;
			else if ( profession < 4 )
				return true;
			else if ( Core.AOS && profession < 6 )
				return true;
			else if ( Core.SE && profession < 8 )
				return true;
			else
				return false;
		}

		private class BadStartMessage : Timer
		{
		    readonly Mobile m_Mobile;
		    readonly int m_Message;
			public BadStartMessage( Mobile m, int message ) : base( TimeSpan.FromSeconds ( 3.5 ) )
			{
				m_Mobile = m;
				m_Message = message;
				Start();
			}

			protected override void OnTick()
			{
				m_Mobile.SendLocalizedMessage( m_Message );
			}
		}

        private static readonly CityInfo m_NewHavenInfo = new CityInfo("New Haven", "The Bountiful Harvest Inn", 3503, 2574, 14, Map.Trammel);

		private static CityInfo GetStartLocation( CharacterCreatedEventArgs args, bool isYoung )
		{
            if (Core.ML)
            {
                //if( args.State != null && args.State.NewHaven )
                return m_NewHavenInfo;	//We don't get the client Version until AFTER Character creation

                //return args.City;  TODO: Uncomment when the old quest system is actually phased out
            }

			bool useHaven = isYoung;

            ClientFlags flags = args.State == null ? ClientFlags.None : args.State.Flags;
            Mobile m = args.Mobile;

			switch ( args.Profession )
			{
				case 4: //Necro
				{
                    if ((flags & ClientFlags.Malas) != 0)
                    {
						return new CityInfo( "Umbra", "Mardoth's Tower", 2114, 1301, -50, Map.Malas );
					}
					else
					{
						useHaven = true; 

						new BadStartMessage( m, 1062205 );
						/*
						 * Unfortunately you are playing on a *NON-Age-Of-Shadows* game 
						 * installation and cannot be transported to Malas.  
						 * You will not be able to take your new player quest in Malas 
						 * without an AOS client.  You are now being taken to the city of 
						 * Haven on the Trammel facet.
						 * */
					}

					break;
				}
				case 5:	//Paladin
				{
                    return m_NewHavenInfo;
                }
				case 6:	//Samurai
				{
                    if ((flags & ClientFlags.Tokuno) != 0)
                    {
						return new CityInfo( "Samurai DE", "Haoti's Grounds", 368, 780, -1, Map.Malas );
					}
					else
					{
						useHaven = true;

						new BadStartMessage( m, 1063487 );
						/*
						 * Unfortunately you are playing on a *NON-Samurai-Empire* game 
						 * installation and cannot be transported to Tokuno. 
						 * You will not be able to take your new player quest in Tokuno 
						 * without an SE client. You are now being taken to the city of 
						 * Haven on the Trammel facet.
						 * */
					}

					break;
				}
				case 7:	//Ninja
				{
                    if ((flags & ClientFlags.Tokuno) != 0)
                    {
						return new CityInfo( "Ninja DE", "Enimo's Residence", 414,	823, -1, Map.Malas );
					}
					else
					{
						useHaven = true;

						new BadStartMessage( m, 1063487 );
						/*
						 * Unfortunately you are playing on a *NON-Samurai-Empire* game 
						 * installation and cannot be transported to Tokuno. 
						 * You will not be able to take your new player quest in Tokuno 
						 * without an SE client. You are now being taken to the city of 
						 * Haven on the Trammel facet.
						 * */
					}

					break;
				}
			}

			if( useHaven )
                return m_NewHavenInfo;
            else
				return args.City;
		}

        private static void FixStats(ref int str, ref int dex, ref int intel, int max)
        {
            int vMax = max - 30;

            int vStr = str - 10;
            int vDex = dex - 10;
            int vInt = intel - 10;

            if (vStr < 0)
                vStr = 0;

            if (vDex < 0)
                vDex = 0;

            if (vInt < 0)
                vInt = 0;

            int total = vStr + vDex + vInt;

            if (total == 0 || total == vMax)
                return;

            double scalar = vMax / (double)total;

            vStr = (int)(vStr * scalar);
            vDex = (int)(vDex * scalar);
            vInt = (int)(vInt * scalar);

            FixStat(ref vStr, (vStr + vDex + vInt) - vMax, vMax);
            FixStat(ref vDex, (vStr + vDex + vInt) - vMax, vMax);
            FixStat(ref vInt, (vStr + vDex + vInt) - vMax, vMax);

            str = vStr + 10;
            dex = vDex + 10;
            intel = vInt + 10;
        }

        private static void FixStat(ref int stat, int diff, int max)
        {
            stat += diff;

            if (stat < 0)
                stat = 0;
            else if (stat > max)
                stat = max;
        }

        private static void SetStats(Mobile m, NetState state, int str, int dex, int intel)
        {
            int max = state.NewCharacterCreation ? 90 : 80;

            FixStats(ref str, ref dex, ref intel, max);

            if (str < 10 || str > 60 || dex < 10 || dex > 60 || intel < 10 || intel > 60 || (str + dex + intel) != max)
            {
                str = 10;
                dex = 10;
                intel = 10;
            }

            m.InitStats(str, dex, intel);
        }

		private static void SetName( Mobile m, string name )
		{
			name = name.Trim();

			if ( !NameVerification.Validate( name, 2, 16, true, false, true, 1, NameVerification.SpaceDashPeriodQuote ) )
				name = "Generic Player";

			m.Name = name;
		}

		private static bool ValidSkills( SkillNameValue[] skills )
		{
			int total = 0;

			for ( int i = 0; i < skills.Length; ++i )
			{
				if ( skills[i].Value < 0 || skills[i].Value > 50 )
					return false;

				total += skills[i].Value;

				for ( int j = i + 1; j < skills.Length; ++j )
				{
					if ( skills[j].Value > 0 && skills[j].Name == skills[i].Name )
						return false;
				}
			}

            return (total == 100 || total == 120);
        }

		private static Mobile m_Mobile;

		private static void SetSkills( Mobile m, SkillNameValue[] skills, int prof )
		{
			switch ( prof )
			{
				case 1: // Warrior
				{
					skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Anatomy, 30 ),
							new SkillNameValue( SkillName.Healing, 45 ),
							new SkillNameValue( SkillName.Swords, 35 ),
							new SkillNameValue( SkillName.Tactics, 50 )
						};

					break;
				}
				case 2: // Magician
				{
					skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.EvalInt, 30 ),
							new SkillNameValue( SkillName.Wrestling, 30 ),
							new SkillNameValue( SkillName.Magery, 50 ),
							new SkillNameValue( SkillName.Meditation, 50 )
						};

					break;
				}
				case 3: // Blacksmith
				{
					skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Mining, 30 ),
							new SkillNameValue( SkillName.ArmsLore, 30 ),
							new SkillNameValue( SkillName.Blacksmith, 50 ),
							new SkillNameValue( SkillName.Tinkering, 50 )
						};

					break;
				}
				case 4: // Necromancer
				{
					skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Necromancy, 50 ),
							new SkillNameValue( SkillName.Focus, 30 ),
							new SkillNameValue( SkillName.SpiritSpeak, 30 ),
							new SkillNameValue( SkillName.Swords, 30 ),
							new SkillNameValue( SkillName.Tactics, 20 )
						};

					break;
				}
				case 5: // Paladin
				{
					skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Chivalry, 51 ),
							new SkillNameValue( SkillName.Swords, 49 ),
							new SkillNameValue( SkillName.Focus, 30 ),
							new SkillNameValue( SkillName.Tactics, 30 )
						};

					break;
				}
				case 6:	//Samurai
				{
					skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Bushido, 50 ),
							new SkillNameValue( SkillName.Swords, 50 ),
							new SkillNameValue( SkillName.Anatomy, 30 ),
							new SkillNameValue( SkillName.Healing, 30 )
					};
					break;
				}
				case 7:	//Ninja
				{
					skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Ninjitsu, 50 ),
							new SkillNameValue( SkillName.Hiding, 50 ),
							new SkillNameValue( SkillName.Fencing, 30 ),
							new SkillNameValue( SkillName.Stealth, 30 )
						};
					break;
				}
				default:
				{
					if ( !ValidSkills( skills ) )
						return;

					break;
				}
			}

			bool addSkillItems = true;
			bool elf = (m.Race == Race.Elf);

			switch ( prof )
			{
				case 1: // Warrior
				{
					if ( elf )
						EquipItem( new LeafChest() );
					else
						EquipItem( new LeatherChest() );
					break;
				}
				case 4: // Necromancer
				{
					Container regs = new BagOfNecroReagents( 50 );

					if ( !Core.AOS )
					{
						foreach ( Item item in regs.Items )
							item.LootType = LootType.Blessed;
					}

					PackItem( regs );

					regs.LootType = LootType.Regular;

					
					EquipItem( new BoneHelm() );

					if ( elf )
					{
						EquipItem( new ElvenMachete() );
						EquipItem( NecroHue( new LeafChest() ) );
						EquipItem( NecroHue( new LeafArms() ) );
						EquipItem( NecroHue( new LeafGloves() ) );
						EquipItem( NecroHue( new LeafGorget() ) );
						EquipItem( NecroHue( new LeafGorget() ) );
						EquipItem( NecroHue( new ElvenPants() ) );	//TODO: Verify the pants
						EquipItem( new ElvenBoots() );
					}
					else
					{
						EquipItem( new BoneHarvester() );
						EquipItem( NecroHue( new LeatherChest() ) );
						EquipItem( NecroHue( new LeatherArms() ) );
						EquipItem( NecroHue( new LeatherGloves() ) );
						EquipItem( NecroHue( new LeatherGorget() ) );
						EquipItem( NecroHue( new LeatherLegs() ) );
						EquipItem( NecroHue( new Skirt() ) );
						EquipItem( new Sandals( 0x8FD ) );
					}

					Spellbook book = new NecromancerSpellbook( (ulong)0x8981 ); // animate dead, evil omen, pain spike, summon familiar, wraith form

					PackItem( book );

					book.LootType = LootType.Blessed;

					addSkillItems = false;

					break;
				}
				case 5: // Paladin
				{
					if ( elf )
					{
						EquipItem( new ElvenMachete() );
						EquipItem( new WingedHelm() );
						EquipItem( new LeafGorget() );
						EquipItem( new LeafArms() );
						EquipItem( new LeafChest() );
						EquipItem( new LeafLegs() );
						EquipItem( new ElvenBoots() );	//Verify hue
					}
					else
					{
						EquipItem( new Broadsword() );
						EquipItem( new Helmet() );
						EquipItem( new PlateGorget() );
						EquipItem( new RingmailArms() );
						EquipItem( new RingmailChest() );
						EquipItem( new RingmailLegs() );
						EquipItem( new ThighBoots( 0x748 ) );
						EquipItem( new Cloak( 0xCF ) );
						EquipItem( new BodySash( 0xCF ) );
					}

					Spellbook book = new BookOfChivalry( (ulong)0x3FF );

					PackItem( book );

					book.LootType = LootType.Blessed;

					break;
				}
					
				case 6: // Samurai
				{
					addSkillItems = false;
					EquipItem( new HakamaShita( 0x2C3 ) );
					EquipItem( new Hakama( 0x2C3 ) );
					EquipItem( new SamuraiTabi( 0x2C3 ) );
					EquipItem( new TattsukeHakama( 0x22D ) );
					EquipItem( new Bokuto() );

					if ( elf )
						EquipItem( new RavenHelm() );
					else
						EquipItem( new LeatherJingasa() );

					PackItem( new Scissors() );
					PackItem( new Bandage( 50 ) );

					Spellbook book = new BookOfBushido();
					PackItem( book );

					break;
				}
				case 7: // Ninja
				{
					addSkillItems = false;
					EquipItem( new Kasa() );
					
					int[] hues = new int[] { 0x1A8, 0xEC, 0x99, 0x90, 0xB5, 0x336, 0x89	};
					//TODO: Verify that's ALL the hues for that above.

					EquipItem( new TattsukeHakama( hues[Utility.Random(hues.Length)] ) );
					
					EquipItem( new HakamaShita( 0x2C3 ) );
					EquipItem( new NinjaTabi( 0x2C3 ) );

					if ( elf )
						EquipItem( new AssassinSpike() );
					else
						EquipItem( new Tekagi() );

					PackItem( new SmokeBomb() );

					Spellbook book = new BookOfNinjitsu();
					PackItem( book );

					break;
				}
			}

			for ( int i = 0; i < skills.Length; ++i )
			{
				SkillNameValue snv = skills[i];

				if ( snv.Value > 0 && ( snv.Name != SkillName.Stealth || prof == 7 ) && snv.Name != SkillName.RemoveTrap && snv.Name != SkillName.Spellweaving )
				{
					Skill skill = m.Skills[snv.Name];

					if ( skill != null )
					{
						skill.BaseFixedPoint = snv.Value * 10;

						if ( addSkillItems )
							AddSkillItems( snv.Name, m );
					}
				}
			}
		}

		private static void EquipItem( Item item )
		{
			EquipItem( item, false, m_Mobile );
		}

        private static void EquipItem(Item item, Mobile m)
        {
            EquipItem(item, false, m);
        }

        private static void EquipItem(Item item, bool mustEquip)
        {
            EquipItem(item, mustEquip, m_Mobile);
        }

	    private static void EquipItem( Item item, bool mustEquip, Mobile m )
		{
			if ( !Core.AOS )
				item.LootType = LootType.Blessed;

			if ( m != null && m.EquipItem( item ) )
				return;

	        Container pack = null;

            if (m != null)
			    pack = m.Backpack;
 
			if ( !mustEquip && pack != null )
				pack.DropItem( item );
			else
				item.Delete();
		}

        private static void PackItem(Item item)
        {
            PackItem(item, m_Mobile);
        }

		private static void PackItem( Item item, Mobile m )
		{
			if ( !Core.AOS )
				item.LootType = LootType.Blessed;

            if (item is Gold || item is BaseReagent || item is BoltOfCloth)
                item.LootType = LootType.Regular;

		    Container pack = null;

            if (m != null)
                pack = m.Backpack;

		    if (pack != null)
                    pack.DropItem(item);
                else
                    item.Delete();
		}

        private static void PackInstrument()
        {
            PackInstrument(m_Mobile);
        }

	    private static void PackInstrument(Mobile m)
		{
			switch ( Utility.Random( 6 ) )
			{
				case 0: PackItem( new Drums(), m ); break;
				case 1: PackItem( new Harp(), m ); break;
				case 2: PackItem( new LapHarp(), m ); break;
				case 3: PackItem( new Lute(), m ); break;
				case 4: PackItem( new Tambourine(), m ); break;
				case 5: PackItem( new TambourineTassel(), m ); break;
			}
		}

        private static void PackScroll( int circle )
        {
            PackScroll(circle, m_Mobile);
        }

		private static void PackScroll( int circle, Mobile m )
		{
            switch (Utility.Random(8) * (circle + 1))
            {
				case  0: PackItem( new ClumsyScroll(), m ); break;
				case  1: PackItem( new CreateFoodScroll(), m ); break;
				case  2: PackItem( new FeeblemindScroll(), m ); break;
				case  3: PackItem( new HealScroll(), m ); break;
				case  4: PackItem( new MagicArrowScroll(), m ); break;
				case  5: PackItem( new NightSightScroll(), m ); break;
				case  6: PackItem( new ReactiveArmorScroll(), m ); break;
				case  7: PackItem( new WeakenScroll(), m ); break;
				case  8: PackItem( new AgilityScroll(), m ); break;
				case  9: PackItem( new CunningScroll(), m ); break;
				case 10: PackItem( new CureScroll(), m ); break;
				case 11: PackItem( new HarmScroll(), m ); break;
				case 12: PackItem( new MagicTrapScroll(), m ); break;
				case 13: PackItem( new MagicUnTrapScroll(), m ); break;
				case 14: PackItem( new ProtectionScroll(), m ); break;
				case 15: PackItem( new StrengthScroll(), m ); break;
				case 16: PackItem( new BlessScroll(), m ); break;
				case 17: PackItem( new FireballScroll(), m ); break;
				case 18: PackItem( new MagicLockScroll(), m ); break;
				case 19: PackItem( new PoisonScroll(), m ); break;
				case 20: PackItem( new TelekinisisScroll(), m ); break;
				case 21: PackItem( new TeleportScroll(), m ); break;
				case 22: PackItem( new UnlockScroll(), m ); break;
				case 23: PackItem( new WallOfStoneScroll(), m ); break;
			}
		}

		private static Item NecroHue( Item item )
		{
			item.Hue = 0x2C3;

			return item;
		}

		public static void	AddSkillItems( SkillName skill, Mobile m )
		{
			bool elf = (m.Race == Race.Elf);
		    Mobile from = m;

			switch ( skill )
			{
				case SkillName.Alchemy:
				{
                    PackItem(new Bottle(4), from);
                    PackItem(new MortarPestle(), from);

					int hue = Utility.RandomPinkHue();

					if ( elf )
					{
						if ( m.Female )
                            EquipItem(new FemaleElvenRobe(hue), from);
						else
                            EquipItem(new MaleElvenRobe(hue), from);
					}
					else
					{
                        EquipItem(new Robe(Utility.RandomPinkHue()), from);
					}
					break;
				}
				case SkillName.Anatomy:
				{
					PackItem( new Bandage( 3 ), from );

					int hue = Utility.RandomYellowHue();

					if ( elf )
					{
						if ( m.Female )
                            EquipItem(new FemaleElvenRobe(hue), from);
						else
                            EquipItem(new MaleElvenRobe(hue), from);
					}
					else
					{
                        EquipItem(new Robe(Utility.RandomPinkHue()), from);
					}
					break;
				}
				case SkillName.AnimalLore:
				{
					

					int hue = Utility.RandomBlueHue();

					if ( elf )
					{
                        EquipItem(new WildStaff(), from);

						if ( m.Female )
                            EquipItem(new FemaleElvenRobe(hue), from);
						else
                            EquipItem(new MaleElvenRobe(hue), from);
					}
					else
					{
                        EquipItem(new ShepherdsCrook(), from);
                        EquipItem(new Robe(hue), from);
					}
					break;
				}
				case SkillName.Archery:
				{
                    PackItem(new Arrow(25), from);

					if ( elf )
                        EquipItem(new ElvenCompositeLongbow(), from);
					else
                        EquipItem(new Bow(), from);
					
					break;
				}
				case SkillName.ArmsLore:
				{
					if ( elf )
					{
						switch ( Utility.Random( 3 ) )
						{
                            case 0: EquipItem(new Leafblade(), from); break;
                            case 1: EquipItem(new RuneBlade(), from); break;
                            case 2: EquipItem(new DiamondMace(), from); break;
						}
					}
					else
					{
						switch ( Utility.Random( 3 ) )
						{
                            case 0: EquipItem(new Kryss(), from); break;
                            case 1: EquipItem(new Katana(), from); break;
                            case 2: EquipItem(new Club(), from); break;
						}
					}

					break;
				}
				case SkillName.Begging:
				{
					if ( elf )
                        EquipItem(new WildStaff(), from);
					else
                        EquipItem(new GnarledStaff(), from);
					break;
				}
				case SkillName.Blacksmith:
				{
                    PackItem(new Tongs(), from);
                    PackItem(new Pickaxe(), from);
                    PackItem(new Pickaxe(), from);
					PackItem( new IronIngot( 50 ), from );
                    EquipItem(new HalfApron(Utility.RandomYellowHue()), from);
					break;
				}
				case SkillName.Bushido:
				{
                    EquipItem(new Hakama(), from);
                    EquipItem(new Kasa(), from);
                    EquipItem(new BookOfBushido(), from);
					break;
				}
				case SkillName.Fletching:
				{
                    PackItem(new Board(14), from);
                    PackItem(new Feather(5), from);
                    PackItem(new Shaft(5), from);
					break;
				}
				case SkillName.Camping:
				{
                    PackItem(new Bedroll(), from);
                    PackItem(new Kindling(5), from);
					break;
				}
				case SkillName.Carpentry:
				{
                    PackItem(new Board(10), from);
                    PackItem(new Saw(), from);
                    EquipItem(new HalfApron(Utility.RandomYellowHue()), from);
					break;
				}
				case SkillName.Cartography:
				{
                    PackItem(new BlankMap(), from);
                    PackItem(new BlankMap(), from);
                    PackItem(new BlankMap(), from);
                    PackItem(new BlankMap(), from);
                    PackItem(new Sextant(), from);
					break;
				}
				case SkillName.Cooking:
				{
                    PackItem(new Kindling(2), from);
                    PackItem(new RawLambLeg(), from);
                    PackItem(new RawChickenLeg(), from);
                    PackItem(new RawFishSteak(), from);
                    PackItem(new SackFlour(), from);
                    PackItem(new Pitcher(BeverageType.Water), from);
					break;
				}
				case SkillName.Chivalry:
				{
					if( Core.ML )
                        PackItem(new BookOfChivalry((ulong)0x3FF), from);

					break;
				}
				case SkillName.DetectHidden:
				{
                    EquipItem(new Cloak(0x455), from);
					break;
				}
				case SkillName.Discordance:
				{
					PackInstrument(from);
					break;
				}
				case SkillName.Fencing:
				{
					if ( elf )
                        EquipItem(new Leafblade(), from);
					else
                        EquipItem(new Kryss(), from);

					break;
				}
				case SkillName.Fishing:
				{
                    EquipItem(new FishingPole(), from);

					int hue = Utility.RandomYellowHue();

					if ( elf )
					{
						Item i = new Circlet();
						i.Hue = hue;
                        EquipItem(i, from);
					}
					else
					{
                        EquipItem(new FloppyHat(Utility.RandomYellowHue()), from);
					}

					break;
				}
				case SkillName.Healing:
				{
                    PackItem(new Bandage(50), from);
                    PackItem(new Scissors(), from);
					break;
				}
				case SkillName.Herding:
				{
					if ( elf )
                        EquipItem(new WildStaff(), from);
					else
                        EquipItem(new ShepherdsCrook(), from);

					break;
				}
				case SkillName.Hiding:
				{
                    EquipItem(new Cloak(0x455), from);
					break;
				}
				case SkillName.Inscribe:
				{
                    PackItem(new BlankScroll(2), from);
                    PackItem(new BlueBook(), from);
					break;
				}
				case SkillName.ItemID:
				{
					if ( elf )
                        EquipItem(new WildStaff(), from);
					else
                        EquipItem(new GnarledStaff(), from);
					break;
				}
				case SkillName.Lockpicking:
				{
                    PackItem(new Lockpick(20), from);
					break;
				}
				case SkillName.Lumberjacking:
				{
                    EquipItem(new Hatchet(), from);
					break;
				}
				case SkillName.Macing:
				{
					if ( elf )
                        EquipItem(new DiamondMace(), from);
					else
                        EquipItem(new Club(), from);

					break;
				}
				case SkillName.Magery:
				{
					BagOfReagents regs = new BagOfReagents( 30 );

					if ( !Core.AOS )
					{
						foreach ( Item item in regs.Items )
							item.LootType = LootType.Regular;
					}

                    PackItem(regs, from);

					regs.LootType = LootType.Regular;

                    PackScroll(0, from);
                    PackScroll(1, from);
                    PackScroll(2, from);

					Spellbook book = new Spellbook( (ulong)0x382A8C38 );

                    EquipItem(book, from);

					book.LootType = LootType.Blessed;

					if ( elf )
					{
                        EquipItem(new Circlet(), from);

						if( m.Female )
                            EquipItem(new FemaleElvenRobe(Utility.RandomBlueHue()), from);
						else
                            EquipItem(new MaleElvenRobe(Utility.RandomBlueHue()), from);
					}
					else
					{
                        EquipItem(new WizardsHat(), from);
                        EquipItem(new Robe(Utility.RandomBlueHue()), from);
					}

					break;
				}
				case SkillName.Mining:
				{
                    PackItem(new Pickaxe(), from);
					break;
				}
				case SkillName.Musicianship:
				{
					PackInstrument(from);
					break;
				}
				case SkillName.Necromancy:
				{
					if( Core.ML )
					{
						Container regs = new BagOfNecroReagents( 50 );

                        PackItem(regs, from);

						regs.LootType = LootType.Regular;
					}

					break;
				}
				case SkillName.Ninjitsu:
				{
                    EquipItem(new Hakama(0x2C3), from);	//Only ninjas get the hued one.
                    EquipItem(new Kasa(), from);
                    EquipItem(new BookOfNinjitsu(), from);
					break;
				}
				case SkillName.Parry:
				{
                    EquipItem(new WoodenShield(), from);
					break;
				}
				case SkillName.Peacemaking:
				{
					PackInstrument(from);
					break;
				}
				case SkillName.Poisoning:
				{
                    PackItem(new LesserPoisonPotion(), from);
                    PackItem(new LesserPoisonPotion(), from);
					break;
				}
				case SkillName.Provocation:
				{
					PackInstrument(from);
					break;
				}
				case SkillName.Snooping:
				{
                    PackItem(new Lockpick(20), from);
					break;
				}
				case SkillName.SpiritSpeak:
				{
                    EquipItem(new Cloak(0x455), from);
					break;
				}
				case SkillName.Stealing:
				{
                    PackItem(new Lockpick(20), from);
					break;
				}
				case SkillName.Swords:
				{
					if ( elf )
                        EquipItem(new RuneBlade(), from);
					else
                        EquipItem(new Longsword(), from);

					break;
				}
				case SkillName.Tactics:
				{
					if ( elf )
                        EquipItem(new RuneBlade(), from);
					else
                        EquipItem(new Katana(), from);

					break;
				}
				case SkillName.Tailoring:
				{
                    PackItem(new BoltOfCloth(), from);
                    PackItem(new SewingKit(), from);
                    PackItem(new Scissors(), from);
					break;
				}
				case SkillName.Tracking:
				{
					if ( m_Mobile != null )
					{
						Item shoes = m_Mobile.FindItemOnLayer( Layer.Shoes );

						if ( shoes != null )
							shoes.Delete();
					}

					int hue = Utility.RandomYellowHue();

					if ( elf )
                        EquipItem(new ElvenBoots(hue), from);
					else
                        EquipItem(new Boots(hue), from);

                    EquipItem(new SkinningKnife(), from);
					break;
				}
				case SkillName.Veterinary:
				{
                    PackItem(new Bandage(5), from);
                    PackItem(new Scissors(), from);
					break;
				}
				case SkillName.Wrestling:
				{
					if ( elf )
                        EquipItem(new LeafGloves(), from);
					else
                        EquipItem(new LeatherGloves(), from);

					break;
				}
                default:
			        break;
			}
		}
	}
}