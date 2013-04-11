using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBSEWeapons: SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new GenericBuyInfo(typeof(NoDachi), 82, Utility.RandomMinMax(15, 25), 0x27A2, 0));
                Add(new GenericBuyInfo(typeof(Tessen), 83, Utility.RandomMinMax(15, 25), 0x27A3, 0));
                Add(new GenericBuyInfo(typeof(Wakizashi), 38, Utility.RandomMinMax(15, 25), 0x27A4, 0));
                Add(new GenericBuyInfo(typeof(Tetsubo), 43, Utility.RandomMinMax(15, 25), 0x27A6, 0));
                Add(new GenericBuyInfo(typeof(Lajatang), 108, Utility.RandomMinMax(15, 25), 0x27A7, 0));
                Add(new GenericBuyInfo(typeof(Daisho), 66, Utility.RandomMinMax(15, 25), 0x27A9, 0));
                Add(new GenericBuyInfo(typeof(Tekagi), 55, Utility.RandomMinMax(15, 25), 0x27AB, 0));
                Add(new GenericBuyInfo(typeof(Shuriken), 18, Utility.RandomMinMax(15, 25), 0x27AC, 0));
                Add(new GenericBuyInfo(typeof(Kama), 61, Utility.RandomMinMax(15, 25), 0x27AD, 0));
                Add(new GenericBuyInfo(typeof(Sai), 56, Utility.RandomMinMax(15, 25), 0x27AF, 0));		

			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( NoDachi ), 41 );
				Add( typeof( Tessen ), 41 );
				Add( typeof( Wakizashi ), 19 );
				Add( typeof( Tetsubo ), 21 );
				Add( typeof( Lajatang ), 54 );
				Add( typeof( Daisho ), 33 );
				Add( typeof( Tekagi ), 22 );
				Add( typeof( Shuriken), 9 );
				Add( typeof( Kama ), 30 );
				Add( typeof( Sai ), 28 );
			}
		}
	}
}