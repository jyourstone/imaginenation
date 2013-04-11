using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBGlassblower : SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new GenericBuyInfo(typeof(Bottle), 5, Utility.RandomMinMax(75, 200), 0xF0E, 0));

                Add(new GenericBuyInfo(typeof(HeatingStand), 2, Utility.RandomMinMax(75, 200), 0x1849, 0));

                Add(new GenericBuyInfo("Crafting Glass With Glassblowing", typeof(GlassblowingBook), 10637, Utility.RandomMinMax(25, 35), 0xFF4, 0));
                Add(new GenericBuyInfo("Finding Glass-Quality Sand", typeof(SandMiningBook), 10637, Utility.RandomMinMax(25, 35), 0xFF4, 0));
                Add(new GenericBuyInfo("1044608", typeof(Blowpipe), 21, Utility.RandomMinMax(75, 200), 0xE8A, 0x3B9));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( BlackPearl ), 3 ); 
				Add( typeof( Bloodmoss ), 3 ); 
				Add( typeof( MandrakeRoot ), 2 ); 
				Add( typeof( Garlic ), 2 ); 
				Add( typeof( Ginseng ), 2 ); 
				Add( typeof( Nightshade ), 2 ); 
				Add( typeof( SpidersSilk ), 2 ); 
				Add( typeof( SulfurousAsh ), 2 ); 
				Add( typeof( Bottle ), 3 );
				Add( typeof( MortarPestle ), 4 );

				Add( typeof( NightSightPotion ), 7 );
				Add( typeof( AgilityPotion ), 7 );
				Add( typeof( StrengthPotion ), 7 );
				Add( typeof( RefreshPotion ), 7 );
				Add( typeof( LesserCurePotion ), 7 );
				Add( typeof( LesserHealPotion ), 7 );
				Add( typeof( LesserPoisonPotion ), 7 );
				Add( typeof( LesserExplosionPotion ), 10 );

				Add( typeof( GlassblowingBook ), 5000 );
				Add( typeof( SandMiningBook ), 5000 );
				Add( typeof( Blowpipe ), 10 );

                Add(typeof(SmallBlueFlask), 10);
                Add(typeof(SmallFlask), 10);
                Add(typeof(SmallRedFlask), 10);
                Add(typeof(SmallYellowFlask), 10);
                Add(typeof(MediumFlask), 12);
                Add(typeof(CurvedFlask), 10);
                Add(typeof(BlueCurvedFlask), 10);
                Add(typeof(RedCurvedFlask), 10);
                Add(typeof(GreenCurvedFlask), 10);
                Add(typeof(LtBlueCurvedFlask), 10);
                Add(typeof(EmptyCurvedFlaskE), 10);
                Add(typeof(EmptyCurvedFlaskW), 10);
                Add(typeof(EmptyRibbedFlask), 12);
                Add(typeof(RedRibbedFlask), 14);
                Add(typeof(VioletRibbedFlask), 14);
                Add(typeof(LargeEmptyFlask), 14);
                Add(typeof(LargeFlask), 12);
                Add(typeof(LargeVioletFlask), 14);
                Add(typeof(LargeYellowFlask), 14);
                Add(typeof(AniLargeVioletFlask), 14);
                Add(typeof(AniRedRibbedFlask), 10);
                Add(typeof(AniSmallBlueFlask), 10);
                Add(typeof(EmptyVial), 10);
                Add(typeof(Hourglass), 15);
                Add(typeof(HourglassAni), 20);
                Add(typeof(SpinningHourglass), 20);
                Add(typeof(FullVialsWRack), 12);
                Add(typeof(Mirror), 200);
                Add(typeof(GlassSkull), 40);
			}
		}
	}
}