using System.Collections;
using System.Collections.Generic;
using Server.Multis;

namespace Server.Mobiles
{
	public class SBShipwright : SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new GenericBuyInfo("1041205", typeof(SmallBoatDeed), 10177, Utility.RandomMinMax(15, 25), 0x14F2, 0));
                Add(new GenericBuyInfo("1041206", typeof(SmallDragonBoatDeed), 10177, Utility.RandomMinMax(15, 25), 0x14F2, 0));
                Add(new GenericBuyInfo("1041207", typeof(MediumBoatDeed), 11552, Utility.RandomMinMax(15, 25), 0x14F2, 0));
                Add(new GenericBuyInfo("1041208", typeof(MediumDragonBoatDeed), 11552, Utility.RandomMinMax(15, 25), 0x14F2, 0));
                Add(new GenericBuyInfo("1041209", typeof(LargeBoatDeed), 12927, Utility.RandomMinMax(15, 25), 0x14F2, 0));
                Add(new GenericBuyInfo("1041210", typeof(LargeDragonBoatDeed), 12927, Utility.RandomMinMax(15, 25), 0x14F2, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
            public InternalSellInfo()
            {
                Add(typeof(SmallBoatDeed), 750);
                Add(typeof(SmallDragonBoatDeed), 775);
                Add(typeof(MediumBoatDeed), 825);
                Add(typeof(MediumDragonBoatDeed), 850);
                Add(typeof(LargeBoatDeed), 900);
                Add(typeof(LargeDragonBoatDeed), 950);
            }
		}
	}
}