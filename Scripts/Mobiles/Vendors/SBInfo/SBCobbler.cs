using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles 
{ 
	public class SBCobbler : SBInfo 
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : List<GenericBuyInfo> 
		{ 
			public InternalBuyInfo() 
			{
                Add(new GenericBuyInfo(typeof(ThighBoots), 15, Utility.RandomMinMax(15, 25), 0x1711, 0));
                Add(new GenericBuyInfo(typeof(Shoes), 8, Utility.RandomMinMax(15, 25), 0x170f, 0));
                Add(new GenericBuyInfo(typeof(Boots), 10, Utility.RandomMinMax(15, 25), 0x170b, 0));
                Add(new GenericBuyInfo(typeof(Sandals), 5, Utility.RandomMinMax(15, 25), 0x170d, 0)); 
 
			} 
		} 

		public class InternalSellInfo : GenericSellInfo 
		{ 
			public InternalSellInfo() 
			{ 
				Add( typeof( Shoes ), 4 ); 
				Add( typeof( Boots ), 5 ); 
				Add( typeof( ThighBoots ), 7 ); 
				Add( typeof( Sandals ), 2 ); 
			} 
		} 
	} 
}