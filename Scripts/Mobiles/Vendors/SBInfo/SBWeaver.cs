using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles 
{ 
	public class SBWeaver: SBInfo 
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : List<GenericBuyInfo> 
		{ 
			public InternalBuyInfo() 
			{
                Add(new GenericBuyInfo(typeof(Dyes), 8, Utility.RandomMinMax(15, 25), 0xFA9, 0));
                Add(new GenericBuyInfo(typeof(DyeTub), 8, Utility.RandomMinMax(15, 25), 0xFAB, 0));

                Add(new GenericBuyInfo(typeof(Cloth), 2, Utility.RandomMinMax(15, 25), 0x175D, 0));

                Add(new GenericBuyInfo(typeof(BoltOfCloth), 100, Utility.RandomMinMax(15, 25), 0xf95, 0));



                Add(new GenericBuyInfo(typeof(LightYarn), 18, Utility.RandomMinMax(15, 25), 0xE1E, 0));


                Add(new GenericBuyInfo(typeof(Scissors), 11, Utility.RandomMinMax(15, 25), 0xF9F, 0));

			} 
		} 

		public class InternalSellInfo : GenericSellInfo 
		{ 
			public InternalSellInfo() 
			{ 
				Add( typeof( Scissors ), 6 ); 
				Add( typeof( Dyes ), 4 ); 
				Add( typeof( DyeTub ), 4 ); 
				Add( typeof( UncutCloth ), 1 );
				Add( typeof( BoltOfCloth ), 50 ); 
				Add( typeof( LightYarnUnraveled ), 9 );
				Add( typeof( LightYarn ), 9 );
				Add( typeof( DarkYarn ), 9 );
			} 
		} 
	} 
}