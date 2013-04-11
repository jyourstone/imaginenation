using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBAlchemist : SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new GenericBuyInfo(typeof(RefreshPotion), 15, Utility.RandomMinMax(5, 15), 0xF0B, 0));
                Add(new GenericBuyInfo(typeof(AgilityPotion), 15, Utility.RandomMinMax(5, 15), 0xF08, 0));
                Add(new GenericBuyInfo(typeof(NightSightPotion), 15, Utility.RandomMinMax(5, 15), 0xF06, 0));
                Add(new GenericBuyInfo(typeof(LesserHealPotion), 15, Utility.RandomMinMax(5, 15), 0xF0C, 0));
                Add(new GenericBuyInfo(typeof(StrengthPotion), 15, Utility.RandomMinMax(5, 15), 0xF09, 0));
                Add(new GenericBuyInfo(typeof(LesserPoisonPotion), 15, Utility.RandomMinMax(5, 15), 0xF0A, 0));
                Add(new GenericBuyInfo(typeof(LesserCurePotion), 15, Utility.RandomMinMax(5, 15), 0xF07, 0));
                Add(new GenericBuyInfo(typeof(LesserExplosionPotion), 21, Utility.RandomMinMax(5, 15), 0xF0D, 0));
                Add(new GenericBuyInfo(typeof(MortarPestle), 8, Utility.RandomMinMax(5, 15), 0xE9B, 0));

                Add(new GenericBuyInfo(typeof(BlackPearl), 4, Utility.RandomMinMax(75, 200), 0xF7A, 0));
                Add(new GenericBuyInfo(typeof(Bloodmoss), 4, Utility.RandomMinMax(75, 200), 0xF7B, 0));
                Add(new GenericBuyInfo(typeof(Garlic), 4, Utility.RandomMinMax(75, 200), 0xF84, 0));
                Add(new GenericBuyInfo(typeof(Ginseng), 4, Utility.RandomMinMax(75, 200), 0xF85, 0));
                Add(new GenericBuyInfo(typeof(MandrakeRoot), 4, Utility.RandomMinMax(75, 200), 0xF86, 0));
                Add(new GenericBuyInfo(typeof(Nightshade), 4, Utility.RandomMinMax(75, 200), 0xF88, 0));
                Add(new GenericBuyInfo(typeof(SpidersSilk), 4, Utility.RandomMinMax(75, 200), 0xF8D, 0));
                Add(new GenericBuyInfo(typeof(SulfurousAsh), 4, Utility.RandomMinMax(75, 200), 0xF8C, 0));

                Add(new GenericBuyInfo(typeof(Bottle), 5, Utility.RandomMinMax(75, 200), 0xF0E, 0));
                Add(new GenericBuyInfo(typeof(HeatingStand), 2, Utility.RandomMinMax(25, 30), 0x1849, 0));

                Add(new GenericBuyInfo("1041060", typeof(HairDye), 37, Utility.RandomMinMax(5, 10), 0xEFF, 0));

                Add(new GenericBuyInfo(typeof(EyesOfNewt), 15, Utility.RandomMinMax(75, 200), 0x0F87, 0));
                Add(new GenericBuyInfo(typeof(BatWing), 15, Utility.RandomMinMax(75, 200), 0xF78, 0));


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
				Add( typeof( MortarPestle ), 3 );
				Add( typeof( HairDye ), 19 );

                //1 reg
			    Add(typeof (RefreshPotion), 2);
			    Add(typeof (LesserCurePotion), 2);
			    Add(typeof (AgilityPotion), 2);
			    Add(typeof (NightSightPotion), 2);
			    Add(typeof (LesserHealPotion), 2);
			    Add(typeof (LesserPoisonPotion), 2);

                //2 regs
			    Add(typeof (StrengthPotion), 7);
			    Add(typeof (PoisonPotion), 7);
			    Add(typeof (ShrinkPotion), 7);

                //3 Regs
                Add(typeof(HealPotion), 10);
                Add(typeof(CurePotion), 10);
                Add(typeof(LesserExplosionPotion), 10);
                Add(typeof(GreaterAgilityPotion), 10);
			    Add(typeof (GreaterAgilityPotion), 10);
			    Add(typeof (InvisibilityPotion), 50);

                //4 Regs
                Add(typeof(GreaterPoisonPotion), 13);
			    Add(typeof (ManaPotion), 13);

                //5 regs
                Add(typeof(GreaterStrengthPotion), 16);
                Add(typeof(ExplosionPotion), 16);
                Add(typeof(TotalRefreshPotion), 16);

                //6 regs
                Add(typeof(GreaterCurePotion), 19);

                //7, 8, 10 regs
                Add(typeof(GreaterExplosionPotion), 31);
                Add(typeof(GreaterHealPotion), 22);
			    Add(typeof (TotalManaPotion), 25);
                Add(typeof(DeadlyPoisonPotion), 25);
              

			}
		}
	}
}
