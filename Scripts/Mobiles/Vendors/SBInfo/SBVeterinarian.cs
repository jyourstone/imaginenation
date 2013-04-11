using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBVeterinarian : SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new GenericBuyInfo(typeof(Bandage), 6, Utility.RandomMinMax(15, 25), 0xE21, 0));
                Add(new AnimalBuyInfo(1, typeof(PackHorse), 841, Utility.RandomMinMax(5, 15), 291, 0));
                Add(new AnimalBuyInfo(1, typeof(PackLlama), 815, Utility.RandomMinMax(5, 15), 292, 0));
                Add(new AnimalBuyInfo(1, typeof(Dog), 158, Utility.RandomMinMax(5, 15), 217, 0));
                Add(new AnimalBuyInfo(1, typeof(Cat), 131, Utility.RandomMinMax(5, 15), 201, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Bandage ), 1 );
			}
		}
	}
}