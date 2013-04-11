using System; 
using System.Collections;
using System.Collections.Generic;
using Server.Items; 

namespace Server.Mobiles 
{ 
	public class SBFishMonger : SBInfo 
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBFishMonger() 
		{ 
		} 

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : List<GenericBuyInfo> 
		{ 
			public InternalBuyInfo() 
			{ 
				Add( new GenericBuyInfo( typeof( RawFishSteak ), 2, 10, 0x97A, 0 ) );
				Add( new GenericBuyInfo( typeof( FishtankKit ), 5000, 20, 0x102C, 444 ) );
			} 
		} 

		public class InternalSellInfo : GenericSellInfo 
		{ 
			public InternalSellInfo() 
			{ 
				Add( typeof( RawFishSteak ), 1 );
				Add( typeof( Fish ), 1 );
				Add( typeof( FishtankKit ), 2500 );
			    Add(typeof (FishingBait), 5);
                Add(typeof(FishingPole), 6);

                Add(typeof(AlbinoAngelfish), 20);
                Add(typeof(Angelfish), 20);
                Add(typeof(AnteniAngelfish), 20);
                Add(typeof(Anthias), 20);
                Add(typeof(BambooShark), 25);
                Add(typeof(BaronessButterflyfish), 22);
                Add(typeof(BlueFish), 20);
                Add(typeof(BrineShrimp), 20);
                Add(typeof(Butterflyfish), 20);
                Add(typeof(CopperbandedButterflyfish), 20);
                Add(typeof(Crab), 22);
                Add(typeof(FantailGoldfish), 20);
                Add(typeof(FlameAngelfish), 20);
                Add(typeof(Jellyfish), 20);
                Add(typeof(PowderblueTang), 20);
                Add(typeof(Pufferfish), 20);
                Add(typeof(QueenAngelfish), 20);
                Add(typeof(Shrimp), 20);
                Add(typeof(SmallFishies), 15);
                Add(typeof(YellowTang), 20);
                Add(typeof(Coral), 20);
                Add(typeof(FishBones), 15);
                Add(typeof(Pearl), 25);
                Add(typeof(SeaGrass), 10);
                Add(typeof(SeveredAnchor), 25);
                Add(typeof(SunkenShip), 20);

                Add(typeof(AlbinoAngelfishTankAddonDeed), 100);
                Add(typeof(AngelfishTankAddonDeed), 100);
                Add(typeof(AnteniAngelfishTankAddonDeed), 100);
                Add(typeof(AnthiasTankAddonDeed), 100);
                Add(typeof(BambooSharkTankAddonDeed), 105);
                Add(typeof(BaronessButterflyfishTankAddonDeed), 100);
                Add(typeof(BlueFishTankAddonDeed), 100);
                Add(typeof(BrineShrimpTankAddonDeed), 100);
                Add(typeof(ButterflyfishTankAddonDeed), 100);
                Add(typeof(CopperbandedButterflyfishTankAddonDeed), 100);
                Add(typeof(CrabTankAddonDeed), 100);
                Add(typeof(FantailGoldfishTankAddonDeed), 100);
                Add(typeof(FlameAngelfishTankAddonDeed), 100);
                Add(typeof(JellyfishTankAddonDeed), 100);
                Add(typeof(PowderblueTangTankAddonDeed), 100);
                Add(typeof(PufferfishTankAddonDeed), 100);
                Add(typeof(QueenAngelfishTankAddonDeed), 100);
                Add(typeof(ShrimpTankAddonDeed), 100);
                Add(typeof(SmallFishiesTankAddonDeed), 90);
                Add(typeof(YellowTangTankAddonDeed), 100);
                Add(typeof(CoralTankAddonDeed), 100);
                Add(typeof(BonesTankAddonDeed), 95);
                Add(typeof(PearlTankAddonDeed), 100);
                Add(typeof(SeaGrassTankAddonDeed), 75);
                Add(typeof(SeveredAnchorTankAddonDeed), 110);
                Add(typeof(SunkenShipTankAddonDeed), 20);
                Add(typeof(EmptyTankAddonDeed), 50);
			} 
		} 
	} 
}