using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBToolMaster: SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new CustomGenericBuyInfo(typeof(ClayCraftTool), 1000, 50, 0xFB7, 0));
                Add(new CustomGenericBuyInfo(typeof(ClayShovel), 500, 50, 0xF39, 0));
                Add(new CustomGenericBuyInfo(typeof(ClayMiningBook), 10000, 20, 0xFF4, 0));
			}
		}
        
		public class InternalSellInfo : GenericSellInfo
		{
		}
	}
}
