using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBMage : SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

	    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new GenericBuyInfo(typeof(Spellbook), 18, Utility.RandomMinMax(5, 15), 0xEFA, 0));
				
				if ( Core.AOS )
                    Add(new GenericBuyInfo(typeof(NecromancerSpellbook), 115, Utility.RandomMinMax(5, 15), 0x2253, 0));

                Add(new GenericBuyInfo(typeof(ScribesPen), 8, Utility.RandomMinMax(15, 20), 0xFBF, 0));

                Add(new GenericBuyInfo(typeof(BlankScroll), 6, Utility.RandomMinMax(75, 200), 0x0E34, 0));

                Add(new GenericBuyInfo("Wizard's hat", typeof(WizardsHat), 11, Utility.RandomMinMax(5, 15), 0x1718, 0));
			//	Add( new GenericBuyInfo( "1041072", typeof( MagicWizardsHat ), 11, 10, 0x1718, Utility.RandomDyedHue() ) );

				Add( new GenericBuyInfo( typeof( RecallRune ), 15, Utility.Random(10,30), 0x1F14, 0 ) );

                Add(new GenericBuyInfo(typeof(RefreshPotion), 15, Utility.RandomMinMax(5, 15), 0xF0B, 0));
                Add(new GenericBuyInfo(typeof(AgilityPotion), 15, Utility.RandomMinMax(5, 15), 0xF08, 0));
                Add(new GenericBuyInfo(typeof(NightSightPotion), 15, Utility.RandomMinMax(5, 15), 0xF06, 0));
                Add(new GenericBuyInfo(typeof(LesserHealPotion), 15, Utility.RandomMinMax(5, 15), 0xF0C, 0));
                Add(new GenericBuyInfo(typeof(StrengthPotion), 15, Utility.RandomMinMax(5, 15), 0xF09, 0));
                Add(new GenericBuyInfo(typeof(LesserPoisonPotion), 15, Utility.RandomMinMax(5, 15), 0xF0A, 0));
                Add(new GenericBuyInfo(typeof(LesserCurePotion), 15, Utility.RandomMinMax(5, 15), 0xF07, 0));
                Add(new GenericBuyInfo(typeof(LesserExplosionPotion), 21, Utility.RandomMinMax(5, 15), 0xF0D, 0));

                Add(new GenericBuyInfo(typeof(BlackPearl), 4, Utility.RandomMinMax(75, 200), 0xF7A, 0));
                Add(new GenericBuyInfo(typeof(Bloodmoss), 4, Utility.RandomMinMax(75, 200), 0xF7B, 0));
                Add(new GenericBuyInfo(typeof(Garlic), 4, Utility.RandomMinMax(75, 200), 0xF84, 0));
                Add(new GenericBuyInfo(typeof(Ginseng), 4, Utility.RandomMinMax(75, 200), 0xF85, 0));
                Add(new GenericBuyInfo(typeof(MandrakeRoot), 4, Utility.RandomMinMax(75, 200), 0xF86, 0));
                Add(new GenericBuyInfo(typeof(Nightshade), 4, Utility.RandomMinMax(75, 200), 0xF88, 0));
                Add(new GenericBuyInfo(typeof(SpidersSilk), 4, Utility.RandomMinMax(75, 200), 0xF8D, 0));
                Add(new GenericBuyInfo(typeof(SulfurousAsh), 4, Utility.RandomMinMax(75, 200), 0xF8C, 0));

                Add(new GenericBuyInfo(typeof(EyesOfNewt), 15, Utility.RandomMinMax(75, 200), 0x0F87, 0));
                Add(new GenericBuyInfo(typeof(BatWing), 15, Utility.RandomMinMax(75, 200), 0xF78, 0));

				if ( Core.AOS )
				{
                    Add(new GenericBuyInfo(typeof(DaemonBlood), 8, Utility.RandomMinMax(75, 200), 0xF7D, 0));
                    Add(new GenericBuyInfo(typeof(PigIron), 7, Utility.RandomMinMax(75, 200), 0xF8A, 0));
                    Add(new GenericBuyInfo(typeof(NoxCrystal), 8, Utility.RandomMinMax(75, 200), 0xF8E, 0));
                    Add(new GenericBuyInfo(typeof(GraveDust), 7, Utility.RandomMinMax(75, 200), 0xF8F, 0));
				}

				Type[] types = Loot.RegularScrollTypes;

				int circles = 3;

				for ( int i = 0; i < circles*8 && i < types.Length; ++i )
				{
					int itemID = 0x1F2E + i;

					if ( i == 6 )
						itemID = 0x1F2D;
					else if ( i > 6 )
						--itemID;

                    if (types[i] == typeof(ReactiveArmorScroll) || types[i] == typeof(UnlockScroll) || types[i] == typeof(MagicLockScroll)) //Don't sell disabled spell scrolls
                        continue;

					Add( new GenericBuyInfo( types[i], 12 + ((i / 8) * 10), 20, itemID, 0 ) );
				}
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( WizardsHat ), 6 );
				Add( typeof( BlackPearl ), 2 ); 
				Add( typeof( Bloodmoss ), 2 ); 
				Add( typeof( MandrakeRoot ), 2 ); 
				Add( typeof( Garlic ), 2 ); 
				Add( typeof( Ginseng ), 2 ); 
				Add( typeof( Nightshade ), 2 ); 
				Add( typeof( SpidersSilk ), 2 ); 
				Add( typeof( SulfurousAsh ), 2 ); 
				Add( typeof( RecallRune ), 8 );
				Add( typeof( Spellbook ), 10 );

                if (Core.AOS)
                {
                    Add(typeof(BatWing), 1);
                    Add(typeof(DaemonBlood), 3);
                    Add(typeof(PigIron), 2);
                    Add(typeof(NoxCrystal), 3);
                    Add(typeof(GraveDust), 1);
                }

				Type[] types = Loot.RegularScrollTypes;

                //Circle 1 / 9 sec
				for ( int i = 0; i < 8; ++i )
					Add( types[i], 3 );

                //Circle 2 / 13 sec
                for (int i = 8; i < 16; ++i)
                    Add(types[i], 5);

                //Circle 3 / 16 sec
                for (int i = 16; i < 24; ++i)
                    Add(types[i], 7);

                //Circle 4 / 20 sec
                for (int i = 24; i < 32; ++i)
                    Add(types[i], 9);

                //Circle 5 / 24 sec
                for (int i = 32; i < 40; ++i)
                    Add(types[i], 11);

                //Circle 6 / 32 sec
                for (int i = 40; i < 48; ++i)
                    Add(types[i], 13);

                //Circle 7-8
                for (int i = 56; i < types.Length; ++i)
                    Add(types[i], 15);

                if (Core.SE)
                {
                    Add(typeof(ExorcismScroll), 1);
                    Add(typeof(AnimateDeadScroll), 26);
                    Add(typeof(BloodOathScroll), 26);
                    Add(typeof(CorpseSkinScroll), 26);
                    Add(typeof(CurseWeaponScroll), 26);
                    Add(typeof(EvilOmenScroll), 26);
                    Add(typeof(PainSpikeScroll), 26);
                    Add(typeof(SummonFamiliarScroll), 26);
                    Add(typeof(HorrificBeastScroll), 27);
                    Add(typeof(MindRotScroll), 39);
                    Add(typeof(PoisonStrikeScroll), 39);
                    Add(typeof(WraithFormScroll), 51);
                    Add(typeof(LichFormScroll), 64);
                    Add(typeof(StrangleScroll), 64);
                    Add(typeof(WitherScroll), 64);
                    Add(typeof(VampiricEmbraceScroll), 101);
                    Add(typeof(VengefulSpiritScroll), 114);
                }
			}
		}
	}
}