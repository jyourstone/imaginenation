using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBArchitect : SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( "Interior Decorator", typeof( InteriorDecorator ), 10001, 20, 0xFC1, 0 ) );
			    Add( new GenericBuyInfo( "House Placement Tool", typeof( HousePlacementTool ), 627, 20, 0x14F6, 0 ));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( InteriorDecorator ), 5000 );
				Add( typeof( HousePlacementTool ), 301 );
			}
		}
	}
}