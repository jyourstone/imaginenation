using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBSEBowyer: SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new GenericBuyInfo(typeof(Yumi), 53, Utility.RandomMinMax(15, 25), 0x27A5, 0));
                Add(new GenericBuyInfo(typeof(Fukiya), 20, Utility.RandomMinMax(15, 25), 0x27AA, 0));
                Add(new GenericBuyInfo(typeof(Nunchaku), 35, Utility.RandomMinMax(15, 25), 0x27AE, 0));
                Add(new GenericBuyInfo(typeof(FukiyaDarts), 3, Utility.RandomMinMax(15, 25), 0x2806, 0));
                Add(new GenericBuyInfo(typeof(Bokuto), 21, Utility.RandomMinMax(15, 25), 0x27A8, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Yumi ), 26 );
				Add( typeof( Fukiya ), 10 );
				Add( typeof( Nunchaku ), 17 );
				Add( typeof( FukiyaDarts ), 1 );
				Add( typeof( Bokuto ), 10 );
			}
		}
	}
}