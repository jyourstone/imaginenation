using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Multis;

namespace Server.Mobiles
{
	public class SBBanker : SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( "Contract Of Employment", typeof( ContractOfEmployment ), 1252, 20, 0x14F0, 0 ) );

				if ( BaseHouse.NewVendorSystem )
					Add( new GenericBuyInfo( "Vendor Rental Contract", typeof( VendorRentalContract ), 1252, 20, 0x14F0, 0x672 ) );

				Add( new GenericBuyInfo( "Commodity Deed", typeof( CommodityDeed ), 5, 20, 0x14F0, 0x47 ) );
                Add( new GenericBuyInfo( "Waystone", typeof(Waystone), 5000, 20, 7955, 2473));

			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
            public InternalSellInfo()
            {
                Add(typeof(ContractOfEmployment), 600);
                Add(typeof(CommodityDeed), 1);
                Add(typeof(VendorRentalContract), 600);
            }
		}
	}
}