using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBSpearForkWeapon: SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new GenericBuyInfo(typeof(Pitchfork), 19, Utility.RandomMinMax(15, 25), 0xE87, 0));
                Add(new GenericBuyInfo(typeof(ShortSpear), 23, Utility.RandomMinMax(15, 25), 0x1403, 0));
                Add(new GenericBuyInfo(typeof(Spear), 31, Utility.RandomMinMax(15, 25), 0xF62, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Spear ), 15 );
				Add( typeof( Pitchfork ), 9 );
				Add( typeof( ShortSpear ), 11 );
			}
		}
	}
}
