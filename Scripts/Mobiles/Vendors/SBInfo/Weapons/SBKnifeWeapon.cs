using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBKnifeWeapon: SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new GenericBuyInfo(typeof(ButcherKnife), 14, Utility.RandomMinMax(15, 25), 0x13F6, 0));
                Add(new GenericBuyInfo(typeof(Dagger), 21, Utility.RandomMinMax(15, 25), 0xF52, 0));
                Add(new GenericBuyInfo(typeof(Cleaver), 15, Utility.RandomMinMax(15, 25), 0xEC3, 0));
                Add(new GenericBuyInfo(typeof(SkinningKnife), 14, Utility.RandomMinMax(15, 25), 0xEC4, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( ButcherKnife ), 7 );
				Add( typeof( Cleaver ), 7 );
				Add( typeof( Dagger ), 12 );
				Add( typeof( SkinningKnife ), 7 );
			}
		}
	}
}
