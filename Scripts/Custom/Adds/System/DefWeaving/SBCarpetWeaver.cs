using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBCarpetWeaver: SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( Dyes ), 8, 20, 0xFA9, 0 ) ); 
				Add( new GenericBuyInfo( typeof( Wool ), 24, 20, 0xDF8, 0 ) );
				Add( new GenericBuyInfo( typeof( Flax ), 97, 20, 0x1A9C, 0 ) );
				Add( new GenericBuyInfo( typeof( WeaversSpool), 10000, 10, 0x1420, 0 ) );
				
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Flax ), 42 );
				Add( typeof( Wool ), 12 );
			}
		}
	}
}
