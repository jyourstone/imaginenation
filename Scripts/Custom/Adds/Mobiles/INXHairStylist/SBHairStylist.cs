/***************************************************************************
 *                                SBBarber.cs
 *                            -------------------
 *   last edited          : August 24, 2007
 *   web site             : www.in-x.org
 *   author               : Makaveli
 *
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   Created by the "Imagine Nation: Xtreme" dev team for "IN:X" and the RunUo
 *   community. If you miss the old school Sphere 0.51 gameplay, and want to
 *   try it on the best and most stable emulator, visit us at www.in-x.org.
 *      
 *   Imagine Nation: Xtreme
 *   A full sphere 0.51 replica.
 * 
 ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using Server.Mobiles;
using INX = Server.INXHairStylist;

namespace Server.INXHairStylist
{
    public class SBINXHairStylist : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBINXHairStylist(): this(null, null)
        {
        }

        public SBINXHairStylist(ArrayList buyList, ArrayList sellList)
        {
            if (buyList != null)
                m_BuyInfo = new InternalBuyInfo(buyList);
            else
                m_BuyInfo = new InternalBuyInfo();               

            if (sellList != null)              
                m_SellInfo = new InternalSellInfo(sellList);
            else
                m_SellInfo = new InternalSellInfo();
        }

        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public override List<GenericBuyInfo> BuyInfo 
        { 
            get { return m_BuyInfo; }
        }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                //HAIR
                Add(new GenericBuyInfo("Short Hair", typeof(INX.ShortHair), 12, 20, 0x203B, 0));
                Add(new GenericBuyInfo("Long Hair", typeof(INX.LongHair), 12, 20, 0x203C, 0));
                Add(new GenericBuyInfo("Pony Tail", typeof(INX.PonyTail), 12, 20, 0x203D, 0));
                Add(new GenericBuyInfo("Mohawk", typeof(INX.Mohawk), 12, 20, 0x2044, 0));
                Add(new GenericBuyInfo("Pageboy Hair", typeof(INX.PageboyHair), 12, 20, 0x2045, 0));
                Add(new GenericBuyInfo("Buns Hair", typeof(INX.BunsHair), 12, 20, 0x2046, 0));
                Add(new GenericBuyInfo("Afro", typeof(INX.Afro), 12, 20, 0x2047, 0));
                Add(new GenericBuyInfo("Receeding Hair", typeof(INX.ReceedingHair), 12, 20, 0x2048, 0));
                Add(new GenericBuyInfo("Two Pig Tails", typeof(INX.TwoPigTails), 12, 20, 0x2049, 0));
                Add(new GenericBuyInfo("Krisna Hair", typeof(INX.KrisnaHair), 12, 20, 0x204A, 0));
                
                //BEARD
                Add(new GenericBuyInfo("Short Beard", typeof(INX.ShortBeard), 8, 20, 0x203F, 0));
                Add(new GenericBuyInfo("Long Beard", typeof(INX.LongBeard), 8, 20, 0x203E, 0));
                Add(new GenericBuyInfo("Medium Short Beard", typeof(INX.MediumShortBeard), 8, 20, 0x204B, 0));
                Add(new GenericBuyInfo("Medium Long Beard", typeof(INX.MediumLongBeard), 8, 20, 0x204C, 0));
                Add(new GenericBuyInfo("Vandyke", typeof(INX.Vandyke), 8, 20, 0x204D, 0));
                Add(new GenericBuyInfo("Goatee", typeof(INX.Goatee), 8, 20, 0x2040, 0));
                Add(new GenericBuyInfo("Mustache", typeof(INX.Mustache), 8, 20, 0x2041, 0));
            }

            public InternalBuyInfo(ArrayList buyList)
            {
                Add(new GenericBuyInfo("Short Hair", typeof(ShortHair), 12, 20, 0x203B, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }

            public InternalSellInfo(ArrayList sellList)
            {
            }
        }
    }
}