using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBRangedWeapon: SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo(typeof(Crossbow), 55, Utility.RandomMinMax(15, 25), 0xF50, 0));
                Add(new GenericBuyInfo(typeof(HeavyCrossbow), 55, Utility.RandomMinMax(15, 25), 0x13FD, 0));
                if (Core.AOS)
                {
                    Add(new GenericBuyInfo(typeof(RepeatingCrossbow), 46, Utility.RandomMinMax(15, 25), 0x26C3, 0));
                    Add(new GenericBuyInfo(typeof(CompositeBow), 45, Utility.RandomMinMax(15, 25), 0x26C2, 0));
                }
                Add(new GenericBuyInfo(typeof(Bolt), 8, Utility.Random(50, 100), 0x1BFB, 0));
                Add(new GenericBuyInfo(typeof(Bow), 40, Utility.RandomMinMax(15, 25), 0x13B2, 0));
                Add(new GenericBuyInfo(typeof(Arrow), 7, Utility.Random(50, 100), 0xF3F, 0));
                Add(new GenericBuyInfo(typeof(Feather), 3, Utility.Random(50, 150), 0x1BD1, 0));
                Add(new GenericBuyInfo(typeof(Shaft), 2, Utility.Random(50, 200), 0x1BD4, 0));
            }
        }

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Bolt ), 3 );
				Add( typeof( Arrow ), 2 );
				Add( typeof( Shaft ), 1 );
				Add( typeof( Feather ), 1 );			

				Add( typeof( HeavyCrossbow ), 27 );
				Add( typeof( Bow ), 17 );
				Add( typeof( Crossbow ), 25 ); 

				if( Core.AOS )
				{
					Add( typeof( CompositeBow ), 23 );
					Add( typeof( RepeatingCrossbow ), 22 );
				}
			}
		}
	}
}
