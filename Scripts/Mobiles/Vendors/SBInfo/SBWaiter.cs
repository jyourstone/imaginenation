using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBWaiter : SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new BeverageBuyInfo(typeof(BeverageBottle), BeverageType.Ale, 7, Utility.RandomMinMax(15, 25), 0x99F, 0));
                Add(new BeverageBuyInfo(typeof(BeverageBottle), BeverageType.Wine, 7, Utility.RandomMinMax(15, 25), 0x9C7, 0));
                Add(new BeverageBuyInfo(typeof(BeverageBottle), BeverageType.Liquor, 7, Utility.RandomMinMax(15, 25), 0x99B, 0));
                Add(new BeverageBuyInfo(typeof(Jug), BeverageType.Cider, 13, Utility.RandomMinMax(15, 25), 0x9C8, 0));
                Add(new BeverageBuyInfo(typeof(Pitcher), BeverageType.Milk, 7, Utility.RandomMinMax(15, 25), 0x9F0, 0));
                Add(new BeverageBuyInfo(typeof(Pitcher), BeverageType.Ale, 11, Utility.RandomMinMax(15, 25), 0x1F95, 0));
                Add(new BeverageBuyInfo(typeof(Pitcher), BeverageType.Cider, 11, Utility.RandomMinMax(15, 25), 0x1F97, 0));
                Add(new BeverageBuyInfo(typeof(Pitcher), BeverageType.Liquor, 11, Utility.RandomMinMax(15, 25), 0x1F99, 0));
                Add(new BeverageBuyInfo(typeof(Pitcher), BeverageType.Wine, 11, Utility.RandomMinMax(15, 25), 0x1F9B, 0));
                Add(new BeverageBuyInfo(typeof(Pitcher), BeverageType.Water, 11, Utility.RandomMinMax(15, 25), 0x1F9D, 0));

                Add(new GenericBuyInfo(typeof(BreadLoaf), 6, Utility.RandomMinMax(15, 15), 0x103B, 0));
                Add(new GenericBuyInfo(typeof(CheeseWheel), 21, Utility.RandomMinMax(5, 15), 0x97E, 0));
                Add(new GenericBuyInfo(typeof(CookedBird), 17, Utility.RandomMinMax(15, 25), 0x9B7, 0));
                Add(new GenericBuyInfo(typeof(LambLeg), 8, Utility.RandomMinMax(15, 25), 0x160A, 0));

                Add(new GenericBuyInfo(typeof(WoodenBowlOfCarrots), 3, Utility.RandomMinMax(15, 25), 0x15F9, 0));
                Add(new GenericBuyInfo(typeof(WoodenBowlOfCorn), 3, Utility.RandomMinMax(15, 25), 0x15FA, 0));
                Add(new GenericBuyInfo(typeof(WoodenBowlOfLettuce), 3, Utility.RandomMinMax(15, 25), 0x15FB, 0));
                Add(new GenericBuyInfo(typeof(WoodenBowlOfPeas), 3, Utility.RandomMinMax(15, 25), 0x15FC, 0));
                Add(new GenericBuyInfo(typeof(EmptyPewterBowl), 2, Utility.RandomMinMax(15, 25), 0x15FD, 0));
                Add(new GenericBuyInfo(typeof(PewterBowlOfCorn), 3, Utility.RandomMinMax(15, 25), 0x15FE, 0));
                Add(new GenericBuyInfo(typeof(PewterBowlOfLettuce), 3, Utility.RandomMinMax(15, 25), 0x15FF, 0));
                Add(new GenericBuyInfo(typeof(PewterBowlOfPeas), 3, Utility.RandomMinMax(15, 25), 0x1600, 0));
                Add(new GenericBuyInfo(typeof(PewterBowlOfPotatos), 3, Utility.RandomMinMax(15, 25), 0x1601, 0));
                Add(new GenericBuyInfo(typeof(WoodenBowlOfStew), 3, Utility.RandomMinMax(15, 25), 0x1604, 0));
                Add(new GenericBuyInfo(typeof(WoodenBowlOfTomatoSoup), 3, Utility.RandomMinMax(15, 25), 0x1606, 0));

                Add(new GenericBuyInfo(typeof(ApplePie), 7, Utility.RandomMinMax(15, 25), 0x1041, 0)); //OSI just has Pie, not Apple/Fruit/Meat

			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
		}
	}
}