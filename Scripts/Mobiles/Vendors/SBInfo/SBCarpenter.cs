using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Multis;

namespace Server.Mobiles
{
	public class SBCarpenter: SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new GenericBuyInfo(typeof(Nails), 3, Utility.RandomMinMax(15, 25), 0x102E, 0));
                Add(new GenericBuyInfo(typeof(Axle), 2, Utility.RandomMinMax(15, 25), 0x105B, 0));
                Add(new GenericBuyInfo(typeof(Board), 3, Utility.RandomMinMax(15, 25), 0x1BD7, 0));
                Add(new GenericBuyInfo(typeof(DrawKnife), 10, Utility.RandomMinMax(15, 25), 0x10E4, 0));
                Add(new GenericBuyInfo(typeof(Froe), 10, Utility.RandomMinMax(15, 25), 0x10E5, 0));
                Add(new GenericBuyInfo(typeof(Scorp), 10, Utility.RandomMinMax(15, 25), 0x10E7, 0));
                Add(new GenericBuyInfo(typeof(Inshave), 10, Utility.RandomMinMax(15, 25), 0x10E6, 0));
                Add(new GenericBuyInfo(typeof(DovetailSaw), 12, Utility.RandomMinMax(15, 25), 0x1028, 0));
                Add(new GenericBuyInfo(typeof(Saw), 15, Utility.RandomMinMax(15, 25), 0x1034, 0));
                Add(new GenericBuyInfo(typeof(Hammer), 17, Utility.RandomMinMax(15, 25), 0x102A, 0));
                Add(new GenericBuyInfo(typeof(MouldingPlane), 11, Utility.RandomMinMax(15, 25), 0x102C, 0));
                Add(new GenericBuyInfo(typeof(SmoothingPlane), 10, Utility.RandomMinMax(15, 25), 0x1032, 0));
                Add(new GenericBuyInfo(typeof(JointingPlane), 11, Utility.RandomMinMax(15, 25), 0x1030, 0));
                Add(new GenericBuyInfo(typeof(Drums), 21, Utility.RandomMinMax(15, 25), 0xE9C, 0));
                Add(new GenericBuyInfo(typeof(Tambourine), Utility.RandomMinMax(15, 25), 20, 0xE9D, 0));
                Add(new GenericBuyInfo(typeof(LapHarp), 21, Utility.RandomMinMax(15, 25), 0xEB2, 0));
                Add(new GenericBuyInfo(typeof(Lute), 21, Utility.RandomMinMax(15, 25), 0xEB3, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( WoodenBox ), 7 );
				Add( typeof( SmallCrate ), 5 );
				Add( typeof( MediumCrate ), 6 );
				Add( typeof( LargeCrate ), 7 );
				Add( typeof( WoodenChest ), 15 );
              
				Add( typeof( LargeTable ), 10 );
				Add( typeof( Nightstand ), 7 );
				Add( typeof( YewWoodTable ), 10 );

				Add( typeof( Throne ), 24 );
				Add( typeof( WoodenThrone ), 6 );
				Add( typeof( Stool ), 6 );
				Add( typeof( FootStool ), 6 );

				Add( typeof( FancyWoodenChairCushion ), 12 );
				Add( typeof( WoodenChairCushion ), 10 );
				Add( typeof( WoodenChair ), 8 );
				Add( typeof( BambooChair ), 6 );
				Add( typeof( WoodenBench ), 6 );

				Add( typeof( Saw ), 9 );
				Add( typeof( Scorp ), 6 );
				Add( typeof( SmoothingPlane ), 6 );
				Add( typeof( DrawKnife ), 6 );
				Add( typeof( Froe ), 6 );
				Add( typeof( Hammer ), 3 );
				Add( typeof( Inshave ), 6 );
				Add( typeof( JointingPlane ), 6 );
				Add( typeof( MouldingPlane ), 6 );
				Add( typeof( DovetailSaw ), 7 );
				Add( typeof( Board ), 2 );
				Add( typeof( Axle ), 1 );

				Add( typeof( Club ), 3 );
                Add(typeof(FishingPole), 3);
                Add(typeof(ShepherdsCrook), 3);
                Add(typeof(GnarledStaff), 3);
                Add(typeof(QuarterStaff), 3);
                Add(typeof(WoodenShield), 4);

				Add( typeof( Lute ), 10 );
				Add( typeof( LapHarp ), 10 );
			    Add(typeof(Harp), 20);
				Add( typeof( Tambourine ), 10 );
				Add( typeof( Drums ), 10 );
                Add(typeof(ShortMusicStand), 6);
                Add(typeof(TallMusicStand), 10);
                Add(typeof(Madroneharp), 20);
                Add(typeof(Walnutharp), 20);
                Add(typeof(Pandrum), 60);
                Add(typeof(Taikodrum), 10);
                Add(typeof(WarDrum), 10);
                Add(typeof(Lullabylute), 10);
                Add(typeof(Suspenselute), 10);
                Add(typeof(Seductionlute), 10);
                Add(typeof(TambourineTassel), 12);
                Add(typeof(RitualTambourine), 15);
                Add(typeof(Traditionaltambourine), 15);
                Add(typeof(Bubenclacker), 10);
                Add(typeof(Bansuriflute), 6);
                Add(typeof(Kavalflute), 6);
                Add(typeof(fiddle), 10);
                Add(typeof(Trumpet), 28);
                Add(typeof(Ocarina), 12);
                Add(typeof(PianoAddonDeed), 150);
                Add(typeof(PerformersHarpAddonDeed), 120);

                Add(typeof(Armoire), 17);
                Add(typeof(FancyArmoire), 17);
                Add(typeof(EmptyBookcase), 17);
                Add(typeof(FullBookcase), 45);

                Add(typeof(BarrelStaves), 3);
                Add(typeof(BarrelLid), 2);
                Add(typeof(Keg), 15);
                Add(typeof(BlankScroll), 1);
                Add(typeof(TrashBarrel), 20);
                Add(typeof(Easle), 10);
                Add(typeof(BulletinBoard), 65);
                Add(typeof(PlayerBBEastDeed), 65);
                Add(typeof(PlayerBBSouthDeed), 65);

                Add(typeof(Dressform), 14);
                Add(typeof(DyeTub), 2);
                Add(typeof(SpinningwheelEastDeed), 42);
                Add(typeof(SpinningwheelSouthDeed), 42);
                Add(typeof(LoomEastDeed), 47);
                Add(typeof(LoomSouthDeed), 47);

                Add(typeof(SmallForgeDeed), 40);
                Add(typeof(LargeForgeEastDeed), 52);
                Add(typeof(LargeForgeSouthDeed), 52);
                Add(typeof(AnvilEastDeed), 78);
                Add(typeof(AnvilSouthDeed), 78);

                Add(typeof(WaterTroughEastDeed), 50);
                Add(typeof(WaterTroughSouthDeed), 50);
                Add(typeof(GrayBrickFireplaceEastDeed), 105);
                Add(typeof(GrayBrickFireplaceSouthDeed), 105);
                Add(typeof(SandstoneFireplaceEastDeed), 105);
                Add(typeof(SandstoneFireplaceSouthDeed), 105);
                Add(typeof(StoneFireplaceEastDeed), 105);
                Add(typeof(StoneFireplaceSouthDeed), 105);
                Add(typeof(StoneOvenEastAddon), 105);
                Add(typeof(StoneOvenSouthDeed), 105);
                Add(typeof(FlourMillEastDeed), 75);
                Add(typeof(FlourMillSouthDeed), 75);

                Add(typeof(SmallBedEastDeed), 75);
                Add(typeof(SmallBedSouthDeed), 75);
                Add(typeof(LargeBedEastDeed), 110);
                Add(typeof(LargeBedSouthDeed), 110);
                Add(typeof(DartBoardEastDeed), 3);
                Add(typeof(DartBoardSouthDeed), 3);
                Add(typeof(BallotBoxDeed), 3);
                Add(typeof(PentagramDeed), 75);
                Add(typeof(AbbatoirDeed), 75);
                Add(typeof(TrainingDummyEastDeed), 38);
                Add(typeof(TrainingDummySouthDeed), 38);
                Add(typeof(PickpocketDipEastDeed), 45);
                Add(typeof(PickpocketDipSouthDeed), 45);
                Add(typeof(ArcheryButteDeed), 30);

                Add(typeof(SmallBoatDeed), 750);
                Add(typeof(SmallDragonBoatDeed), 775);
                Add(typeof(MediumBoatDeed), 825);
                Add(typeof(MediumDragonBoatDeed), 850);
                Add(typeof(LargeBoatDeed), 900);
                Add(typeof(LargeDragonBoatDeed), 950);

				Add( typeof( Log ), 1 );
			}
		}
	}
}
