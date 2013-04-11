using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class SBTailor : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo(typeof(SewingKit), 6, Utility.RandomMinMax(15, 25), 0xF9D, 0));
                Add(new GenericBuyInfo(typeof(Scissors), 11, Utility.RandomMinMax(15, 25), 0xF9F, 0));
                Add(new GenericBuyInfo(typeof(DyeTub), 12, Utility.RandomMinMax(15, 25), 0xFAB, 0));
                Add(new GenericBuyInfo(typeof(Dyes), 10, Utility.RandomMinMax(15, 25), 0xFA9, 0));

                Add(new GenericBuyInfo(typeof(Shirt), 24, Utility.RandomMinMax(15, 25), 0x1517, 0));
                Add(new GenericBuyInfo(typeof(ShortPants), 24, Utility.RandomMinMax(15, 25), 0x152E, 0));
                Add(new GenericBuyInfo(typeof(FancyShirt), 32, Utility.RandomMinMax(15, 25), 0x1EFD, 0));
                Add(new GenericBuyInfo(typeof(LongPants), 32, Utility.RandomMinMax(15, 25), 0x1539, 0));
                Add(new GenericBuyInfo(typeof(FancyDress), 48, Utility.RandomMinMax(15, 25), 0x1EFF, 0));
                Add(new GenericBuyInfo(typeof(PlainDress), 40, Utility.RandomMinMax(15, 25), 0x1F01, 0));
                Add(new GenericBuyInfo(typeof(Kilt), 32, 20, 0x1537, 0));
                Add(new GenericBuyInfo(typeof(HalfApron), 24, Utility.RandomMinMax(15, 25), 0x153b, 0));
                Add(new GenericBuyInfo(typeof(Robe), 64, Utility.RandomMinMax(15, 25), 0x1F03, 0));
                Add(new GenericBuyInfo(typeof(Cloak), 56, Utility.RandomMinMax(15, 25), 0x1515, 0));
                Add(new GenericBuyInfo(typeof(Doublet), 32, Utility.RandomMinMax(15, 25), 0x1F7B, 0));
                Add(new GenericBuyInfo(typeof(Tunic), 48, Utility.RandomMinMax(15, 25), 0x1FA1, 0));
                Add(new GenericBuyInfo(typeof(JesterSuit), 96, Utility.RandomMinMax(15, 25), 0x1F9F, 0));
                Add(new GenericBuyInfo(typeof(Skirt), 40, Utility.RandomMinMax(15, 25), 0x1516, 0));
                Add(new GenericBuyInfo(typeof(BodySash), 16, Utility.RandomMinMax(15, 25), 0x1541, 0));
                Add(new GenericBuyInfo(typeof(Surcoat), 56, Utility.RandomMinMax(15, 25), 0x1FFD, 0));
                Add(new GenericBuyInfo(typeof(FullApron), 40, Utility.RandomMinMax(15, 25), 0x153D, 0));

                Add(new GenericBuyInfo(typeof(JesterHat), 30, Utility.RandomMinMax(15, 25), 0x171C, 0));
                Add(new GenericBuyInfo(typeof(FloppyHat), 44, Utility.RandomMinMax(15, 25), 0x1713, 0));
                Add(new GenericBuyInfo(typeof(WideBrimHat), 48, Utility.RandomMinMax(15, 25), 0x1714, 0));
                Add(new GenericBuyInfo(typeof(Cap), 22, Utility.RandomMinMax(15, 25), 0x1715, 0));
                Add(new GenericBuyInfo(typeof(TallStrawHat), 52, Utility.RandomMinMax(15, 25), 0x1716, 0));
                Add(new GenericBuyInfo(typeof(StrawHat), 40, Utility.RandomMinMax(15, 25), 0x1717, 0));
                Add(new GenericBuyInfo(typeof(WizardsHat), 60, Utility.RandomMinMax(15, 25), 0x1718, 0));
                Add(new GenericBuyInfo(typeof(FeatheredHat), 48, Utility.RandomMinMax(15, 25), 0x171A, 0));
                Add(new GenericBuyInfo(typeof(TricorneHat), 48, Utility.RandomMinMax(15, 25), 0x171B, 0));
                Add(new GenericBuyInfo(typeof(Bandana), 8, Utility.RandomMinMax(15, 25), 0x1540, 0));
                Add(new GenericBuyInfo(typeof(SkullCap), 8, Utility.RandomMinMax(15, 25), 0x1544, 0));
                Add(new GenericBuyInfo(typeof(Bonnet), 44, Utility.RandomMinMax(15, 25), 0x1719, 0));

                Add(new GenericBuyInfo(typeof(BoltOfCloth), 100, Utility.RandomMinMax(15, 25), 0xf95, 0));

                Add(new GenericBuyInfo(typeof(Cloth), 4, Utility.RandomMinMax(15, 25), 0x1767, 0));

                Add(new GenericBuyInfo(typeof(Cotton), 97, Utility.RandomMinMax(15, 25), 0xDF9, 0));
                Add(new GenericBuyInfo(typeof(Wool), 48, Utility.RandomMinMax(15, 25), 0xDF8, 0));
                Add(new GenericBuyInfo(typeof(Flax), 97, Utility.RandomMinMax(15, 25), 0x1A9C, 0));
                Add(new GenericBuyInfo(typeof(SpoolOfThread), 18, Utility.RandomMinMax(15, 25), 0xFA0, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(Scissors), 6);
                Add(typeof(SewingKit), 2);
                Add(typeof(Dyes), 4);
                Add(typeof(DyeTub), 5);

                Add(typeof(BoltOfCloth), 50);

                Add(typeof(FancyShirt), 16);
                Add(typeof(Shirt), 12);

                Add(typeof(Surcoat), 28);
               
                Add(typeof(ShortPants), 12);
                Add(typeof(LongPants), 16);

                Add(typeof(Cloak), 28);
                Add(typeof(FancyDress), 24);
                Add(typeof(Robe), 32);
                Add(typeof(PlainDress), 20);

                Add(typeof(Skirt), 20);
                Add(typeof(Kilt), 16);

                Add(typeof(Doublet), 16);
                Add(typeof(Tunic), 24);
                Add(typeof(JesterSuit), 62);

                Add(typeof(BodySash), 8);
                Add(typeof(FullApron), 20);
                Add(typeof(HalfApron), 12);

                Add(typeof(JesterHat), 29);
                Add(typeof(FloppyHat), 21);
                Add(typeof(WideBrimHat), 23);
                Add(typeof(Cap), 21);
                Add(typeof(SkullCap), 3);
                Add(typeof(Bandana), 3);
                Add(typeof(TallStrawHat), 25);
                Add(typeof(StrawHat), 19);
                Add(typeof(WizardsHat), 29);
                Add(typeof(Bonnet), 21);
                Add(typeof(FeatheredHat), 23);
                Add(typeof(TricorneHat), 23);

                Add(typeof(SpoolOfThread), 9);

                Add(typeof(Flax), 42);
                Add(typeof(Cotton), 42);
                Add(typeof(Wool), 20);
                Add(typeof(Cloth), 1);
            }
        }
    }
}
