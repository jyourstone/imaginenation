using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBRaresVendor : SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo
		{
			get { return m_SellInfo; }
		}

		public override List<GenericBuyInfo>  BuyInfo
		{
			get { return m_BuyInfo; }
		}

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new GenericBuyInfo(typeof(RareDyeTub), 250000, Utility.RandomMinMax(75, 200), 0xFAB, 0));
                Add(new GenericBuyInfo(typeof(Gravestone), 4500, Utility.RandomMinMax(75, 200), 0x1167, 0));
                Add(new GenericBuyInfo(typeof(FireworksWand), 2400, Utility.RandomMinMax(75, 200), 0xDF2, 0));
                Add(new GenericBuyInfo(typeof(GuildContainerDeed), 50000, Utility.RandomMinMax(75, 200), 5360, 0));
			    Add(new GenericBuyInfo(typeof(OrderShield), 200000, Utility.RandomMinMax(75, 200), 7108, 0));
                Add(new GenericBuyInfo(typeof(ChaosShield), 200000, Utility.RandomMinMax(75, 200), 7107, 0));
                Add(new GenericBuyInfo(typeof(NeutralShield), 200000, Utility.RandomMinMax(75, 200), 7026, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
		}
	}
}