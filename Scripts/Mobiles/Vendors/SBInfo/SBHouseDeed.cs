using System.Collections;
using System.Collections.Generic;
using Server.Multis.Deeds;

namespace Server.Mobiles
{
	public class SBHouseDeed: SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new GenericBuyInfo("deed to a stone-and-plaster house", typeof(StonePlasterHouseDeed), 43800, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a field stone house", typeof(FieldStoneHouseDeed), 43800, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a small brick house", typeof(SmallBrickHouseDeed), 43800, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a wooden house", typeof(WoodHouseDeed), 43800, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a wood-and-plaster house", typeof(WoodPlasterHouseDeed), 43800, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a thatched-roof cottage", typeof(ThatchedRoofCottageDeed), 43800, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a brick house", typeof(BrickHouseDeed), 144500, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a two-story wood-and-plaster house", typeof(TwoStoryWoodPlasterHouseDeed), 192400, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a tower", typeof(TowerDeed), 433200, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a small stone keep", typeof(KeepDeed), 665200, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a castle", typeof(CastleDeed), 1022800, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a large house with patio", typeof(LargePatioDeed), 152800, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a marble house with patio", typeof(LargeMarbleDeed), 192000, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a small stone tower", typeof(SmallTowerDeed), 88500, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a two story log cabin", typeof(LogCabinDeed), 97800, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a sandstone house with patio", typeof(SandstonePatioDeed), 90900, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a two story villa", typeof(VillaDeed), 136500, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a small stone workshop", typeof(StoneWorkshopDeed), 60600, Utility.RandomMinMax(15, 25), 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a small marble workshop", typeof(MarbleWorkshopDeed), 63000, Utility.RandomMinMax(15, 25), 0x14F0, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{/*
			public InternalSellInfo()
			{
                //Taran: This is disabled because the price in the vendor sell menu displays wrong
                //Taran: Buy back value is 93% of original price = same value as customizing a house and then demolishing it
                Add(typeof(StonePlasterHouseDeed), 40734);
                Add(typeof(FieldStoneHouseDeed), 40734);
                Add(typeof(SmallBrickHouseDeed), 40734);
                Add(typeof(WoodHouseDeed), 40734);
                Add(typeof(WoodPlasterHouseDeed), 40734);
                Add(typeof(ThatchedRoofCottageDeed), 40734);
                Add(typeof(BrickHouseDeed), 134385);
                Add(typeof(TwoStoryWoodPlasterHouseDeed), 178932);
                Add(typeof(TowerDeed), 402876);
                Add(typeof(KeepDeed), 618636);
                Add(typeof(CastleDeed), 951204);
                Add(typeof(LargePatioDeed), 142104);
                Add(typeof(LargeMarbleDeed), 178560);
                Add(typeof(SmallTowerDeed), 82305);
                Add(typeof(LogCabinDeed), 90954);
                Add(typeof(SandstonePatioDeed), 84537);
                Add(typeof(VillaDeed), 126945);
                Add(typeof(StoneWorkshopDeed), 56358);
                Add(typeof(MarbleWorkshopDeed), 58590);
            }*/
        }
	}
}
