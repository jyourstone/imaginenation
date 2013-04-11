using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles 
{ 
	public class SBBard: SBInfo 
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : List<GenericBuyInfo> 
		{ 
			public InternalBuyInfo() 
			{
                Add(new GenericBuyInfo(typeof(Drums), 21, Utility.RandomMinMax(5, 15), 0x0E9C, 0));
                Add(new GenericBuyInfo(typeof(Tambourine), 21, Utility.RandomMinMax(5, 15), 0x0E9E, 0));
                Add(new GenericBuyInfo(typeof(LapHarp), 21, Utility.RandomMinMax(5, 15), 0x0EB2, 0));
                Add(new GenericBuyInfo(typeof(Lute), 21, Utility.RandomMinMax(5, 15), 0x0EB3, 0)); 

			} 
		} 

		public class InternalSellInfo : GenericSellInfo 
		{ 
			public InternalSellInfo() 
			{
                Add(typeof(Lute), 10);
                Add(typeof(LapHarp), 10);
                Add(typeof(Harp), 20);
                Add(typeof(Tambourine), 10);
                Add(typeof(Drums), 10);
                Add(typeof(ShortMusicStand), 6);
                Add(typeof(TallMusicStand), 10);
                Add(typeof(Madroneharp), 20);
                Add(typeof(Walnutharp), 20);
                Add(typeof(Pandrum), 60);
                Add(typeof(Taikodrum), 10);
                Add(typeof(WarDrum), 10);
                Add(typeof(Lullabylute), 10);
                Add(typeof(Suspenselute), 10);
                Add(typeof(Seductionlute), 10);
                Add(typeof(TambourineTassel), 12);
                Add(typeof(RitualTambourine), 15);
                Add(typeof(Traditionaltambourine), 15);
                Add(typeof(Bubenclacker), 10);
                Add(typeof(Bansuriflute), 6);
                Add(typeof(Kavalflute), 6);
                Add(typeof(fiddle), 10);
                Add(typeof(Trumpet), 28);
                Add(typeof(Ocarina), 12);
                Add(typeof(PianoAddonDeed), 150);
                Add(typeof(PerformersHarpAddonDeed), 120); 
			} 
		} 
	} 
}