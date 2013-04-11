using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBRanger : SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new AnimalBuyInfo(1, typeof(Cat), 138, Utility.RandomMinMax(15, 25), 201, 0));
                Add(new AnimalBuyInfo(1, typeof(Dog), 181, Utility.RandomMinMax(15, 25), 217, 0));
                Add(new AnimalBuyInfo(1, typeof(PackLlama), 491, Utility.RandomMinMax(15, 25), 292, 0));
                Add(new AnimalBuyInfo(1, typeof(PackHorse), 606, Utility.RandomMinMax(15, 25), 291, 0));
                Add(new GenericBuyInfo(typeof(Bandage), 5, Utility.RandomMinMax(15, 25), 0xE21, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
		}
	}
}