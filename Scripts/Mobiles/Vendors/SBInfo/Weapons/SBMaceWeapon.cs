using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBMaceWeapon: SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new GenericBuyInfo(typeof(HammerPick), 26, Utility.RandomMinMax(15, 25), 0x143D, 0));
                Add(new GenericBuyInfo(typeof(Club), 16, Utility.RandomMinMax(15, 25), 0x13B4, 0));
                Add(new GenericBuyInfo(typeof(TrainingWand), 12, Utility.RandomMinMax(10, 19), 0xDF5, 0));
                Add(new GenericBuyInfo(typeof(Mace), 28, Utility.RandomMinMax(15, 25), 0xF5C, 0));
                Add(new GenericBuyInfo(typeof(Maul), 21, Utility.RandomMinMax(15, 25), 0x143B, 0));
                Add(new GenericBuyInfo(typeof(WarHammer), 25, Utility.RandomMinMax(15, 25), 0x1439, 0));
                Add(new GenericBuyInfo(typeof(WarMace), 31, Utility.RandomMinMax(15, 25), 0x1407, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Club ), 8 );
                Add(typeof(TrainingWand), 6);
				Add( typeof( HammerPick ), 13 );
				Add( typeof( Mace ), 14 );
				Add( typeof( Maul ), 10 );
				Add( typeof( WarHammer ), 12 );
				Add( typeof( WarMace ), 15 );
			}
		}
	}
}
