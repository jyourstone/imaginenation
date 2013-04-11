using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles 
{ 
	public class SBLeatherWorker: SBInfo 
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : List<GenericBuyInfo> 
		{ 
			public InternalBuyInfo() 
			{
                Add(new GenericBuyInfo(typeof(Hides), 4, Utility.RandomMinMax(55, 60), 0x1078, 0));
                Add(new GenericBuyInfo(typeof(ThighBoots), 56, Utility.RandomMinMax(5, 15), 0x1711, 0)); 
			} 
		} 

		public class InternalSellInfo : GenericSellInfo 
		{ 
			public InternalSellInfo() 
			{ 
				Add( typeof( Hides ), 2 );
                Add(typeof(Leather), 2);
				Add( typeof( ThighBoots ), 28 ); 
			} 
		} 
	} 
} 
