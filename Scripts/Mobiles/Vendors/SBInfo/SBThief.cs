using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBThief : SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new GenericBuyInfo(typeof(Backpack), 15, Utility.RandomMinMax(15, 25), 0x9B2, 0));
                Add(new GenericBuyInfo(typeof(Pouch), 6, Utility.RandomMinMax(15, 25), 0xE79, 0));
                Add(new GenericBuyInfo(typeof(Torch), 8, Utility.RandomMinMax(15, 25), 0xF6B, 0));
                Add(new GenericBuyInfo(typeof(Lantern), 2, Utility.RandomMinMax(15, 25), 0xA25, 0));
				//Add( new GenericBuyInfo( typeof( OilFlask ), 8, 20, 0x####, 0 ) );
                Add(new GenericBuyInfo(typeof(Lockpick), 12, Utility.RandomMinMax(15, 25), 0x14FC, 0));
                Add(new GenericBuyInfo(typeof(WoodenBox), 14, Utility.RandomMinMax(15, 25), 0x9AA, 0));
                Add(new GenericBuyInfo(typeof(Key), 2, Utility.RandomMinMax(15, 25), 0x100E, 0));
                Add(new GenericBuyInfo(typeof(HairDye), 37, Utility.RandomMinMax(15, 25), 0xEFF, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Backpack ), 7 );
				Add( typeof( Pouch ), 3 );
				Add( typeof( Torch ), 3 );
				Add( typeof( Lantern ), 1 );
				//Add( typeof( OilFlask ), 4 );
				Add( typeof( Lockpick ), 6 );
				Add( typeof( WoodenBox ), 7 );
				Add( typeof( HairDye ), 19 );
			}
		}
	}
}