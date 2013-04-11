using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles 
{ 
	public class SBFisherman : SBInfo 
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : List<GenericBuyInfo> 
		{ 
			public InternalBuyInfo() 
			{
                Add(new GenericBuyInfo(typeof(RawFishSteak), 3, Utility.RandomMinMax(15, 25), 0x97A, 0));
				//TODO: Add( new GenericBuyInfo( typeof( SmallFish ), 3, 20, 0xDD6, 0 ) );
				//TODO: Add( new GenericBuyInfo( typeof( SmallFish ), 3, 20, 0xDD7, 0 ) );
                Add(new GenericBuyInfo(typeof(Fish), 6, Utility.RandomMinMax(60, 70), 0x9CC, 0));
                Add(new GenericBuyInfo(typeof(Fish), 6, Utility.RandomMinMax(60, 70), 0x9CD, 0));
                Add(new GenericBuyInfo(typeof(Fish), 6, Utility.RandomMinMax(60, 70), 0x9CE, 0));
                Add(new GenericBuyInfo(typeof(Fish), 6, Utility.RandomMinMax(60, 70), 0x9CF, 0));
                Add(new GenericBuyInfo(typeof(FishingPole), 15, Utility.RandomMinMax(15, 25), 0xDC0, 0));

                #region Mondain's Legacy
                Add(new GenericBuyInfo(typeof(AquariumFishingNet), 250, 20, 0xDC8, 0));
                Add(new GenericBuyInfo(typeof(AquariumFood), 62, 20, 0xEFC, 0));
                Add(new GenericBuyInfo(typeof(FishBowl), 6312, 20, 0x241C, 0x482));
                Add(new GenericBuyInfo(typeof(VacationWafer), 67, 20, 0x971, 0));
                Add(new GenericBuyInfo(typeof(AquariumNorthDeed), 150000, 1, 0x14F0, 0));
                Add(new GenericBuyInfo(typeof(AquariumEastDeed), 150000, 1, 0x14F0, 0));
                #endregion
			} 
		} 

		public class InternalSellInfo : GenericSellInfo 
		{ 
			public InternalSellInfo() 
			{ 
				Add( typeof( RawFishSteak ), 1 );
				Add( typeof( Fish ), 1 );
				//TODO: Add( typeof( SmallFish ), 1 );
				Add( typeof( FishingPole ), 7 );
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
			} 
		} 
	} 
}