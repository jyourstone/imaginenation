using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles 
{ 
	public class SBHairStylist : SBInfo 
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : List<GenericBuyInfo> 
		{ 
			public InternalBuyInfo() 
			{   //HAIR
                Add(new GenericBuyInfo("Short Hair", typeof(ShortHair), 12, Utility.RandomMinMax(15, 25), 0x203B, 0));
                Add(new GenericBuyInfo("LongHair", typeof(LongHair), 12, Utility.RandomMinMax(15, 25), 0x203C, 0));
                Add(new GenericBuyInfo("PonyTail", typeof(PonyTail), 12, Utility.RandomMinMax(15, 25), 0x203D, 0));
                Add(new GenericBuyInfo("Mohawk", typeof(Mohawk), 12, Utility.RandomMinMax(15, 25), 0x2044, 0));
                Add(new GenericBuyInfo("PageboyHair", typeof(PageboyHair), 12, Utility.RandomMinMax(15, 25), 0x2045, 0));
                Add(new GenericBuyInfo("BunsHair", typeof(BunsHair), 12, Utility.RandomMinMax(15, 25), 0x2046, 0));
                Add(new GenericBuyInfo("Afro", typeof(Afro), 12, Utility.RandomMinMax(15, 25), 0x2047, 0));
                Add(new GenericBuyInfo("ReceedingHair", typeof(ReceedingHair), 12, Utility.RandomMinMax(15, 25), 0x2048, 0));
                Add(new GenericBuyInfo("TwoPigTails", typeof(TwoPigTails), 12, Utility.RandomMinMax(15, 25), 0x2049, 0));
                Add(new GenericBuyInfo("KrisnaHair", typeof(KrisnaHair), 12, Utility.RandomMinMax(15, 25), 0x204A, 0));
				//BEARD
                Add(new GenericBuyInfo("ShortBeard", typeof(ShortBeard), 8, Utility.RandomMinMax(15, 25), 0x203F, 0));
                Add(new GenericBuyInfo("LongBeard", typeof(LongBeard), 8, Utility.RandomMinMax(15, 25), 0x203E, 0));
                Add(new GenericBuyInfo("MediumShortBeard", typeof(MediumShortBeard), 8, Utility.RandomMinMax(15, 25), 0x204B, 0));
                Add(new GenericBuyInfo("MediumLongBeard", typeof(MediumLongBeard), 8, Utility.RandomMinMax(15, 25), 0x204C, 0));
                Add(new GenericBuyInfo("Vandyke", typeof(Vandyke), 8, Utility.RandomMinMax(15, 25), 0x204D, 0));
                Add(new GenericBuyInfo("Goatee", typeof(Goatee), 8, Utility.RandomMinMax(15, 25), 0x2040, 0));
                Add(new GenericBuyInfo("Mustache", typeof(Mustache), 8, Utility.RandomMinMax(15, 25), 0x2041, 0));
			} 
		} 

		public class InternalSellInfo : GenericSellInfo 
		{
            public InternalSellInfo()
            {
                Add(typeof(Scissors), 1);
            }
		} 
	} 
}