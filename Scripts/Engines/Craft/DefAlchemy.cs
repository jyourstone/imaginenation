using System;
using Server.Items;
using Server.Network;

namespace Server.Engines.Craft
{
	public class DefAlchemy : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Alchemy;	}
		}

		public override int GumpTitleNumber
		{
			get { return 1044001; } // <CENTER>ALCHEMY MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefAlchemy();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.0; // 0%
		}

        private DefAlchemy()
            : base(1,1, 2.0)// base( 1, 1, 3.1 )
		{
		}

        public static int GetDelay(CraftItem toCraft)
        {
            if (toCraft != null)
                return toCraft.Resources.GetAt(0).Amount+1;
            else
                return 2;
        }

	    public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if( tool == null || tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.

			return 0;
		}

		public override void PlayCraftEffect( Mobile from)
		{
			from.PlaySound( 0x242 );
		}

        public void PlayAlchEffect(Mobile from, Type resType, int count)
        {
            string reagentName = string.Empty;

            from.PlaySound(0x242);

            if (resType != null)
            {
                if (resType == typeof(Bloodmoss))
                    reagentName = "Blood Moss";
                else if (resType == typeof(BlackPearl))
                    reagentName = "Black Pearl";
                else if (resType == typeof(Garlic))
                    reagentName = "Garlic";
                else if (resType == typeof(Ginseng))
                    reagentName = "Ginseng";
                else if (resType == typeof(MandrakeRoot))
                    reagentName = "Mandrake Root";
                else if (resType == typeof(Nightshade))
                    reagentName = "Nightshade";
                else if (resType == typeof(SulfurousAsh))
                    reagentName = "Sulfurous Ash";
                else if (resType == typeof(SpidersSilk))
                    reagentName = "Spiders' Silk";
                else if (resType == typeof(EyesOfNewt))
                    reagentName = "Eyes of Newt";
                else if (resType == typeof(BatWing))
                    reagentName = "Bat Wings";
                else if (resType == typeof(WyrmsHeart))
                    reagentName = "Wyrms' Heart";
                else
                    reagentName = "Reagents";
            }

            if (count != 1)
            {
                //For everyone else
                //from.NonlocalOverheadMessage(MessageType.Emote, 0x22, true,
                //                             string.Format("*You see {0} add some more {1} and continue grinding*",
                //                                           from.Name, reagentName));

                //For you
                from.LocalOverheadMessage(MessageType.Emote, 0x22, true, string.Format("*You add more {0} and continue grinding*", reagentName));
            }
            else
            {
                //For you
                from.LocalOverheadMessage(MessageType.Emote, 0x22, true,
                                          string.Format("*You start grinding some {0} in the mortar*", reagentName));

                //For everyone else
                //from.LocalOverheadMessage(MessageType.Emote, 0x22, true,
                //                             string.Format("*You see {0} start grinding some {1} in the mortar*",
                //                                           from.Name, reagentName));
            }
        }

		private static readonly Type typeofPotion = typeof( BasePotion );

		public static bool IsPotion( Type type )
		{
			return typeofPotion.IsAssignableFrom( type );
		}

        public override int PlayEndingEffect(Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item)
        {
            if (toolBroken)
                from.SendLocalizedMessage(1044038); // You have worn out your tool

            if (failed)
            {
                if (item == null || IsPotion(item.ItemType))
                {
                    from.AddToBackpack(new Bottle(), false);
                    return 500287; // You fail to create a useful potion.
                }

                return 1044043; // You failed to create the item, and some of your materials are lost.
            }

            from.PlaySound(0x240);// Sound of a filling bottle
            return 1044154; // You create the item.
        }

	    public override void InitCraftList()
		{
			int index = -1;

			// Refresh Potion
			index = AddCraft( typeof( RefreshPotion ), 1044530, 1044538, -25, 25.0, typeof( BlackPearl ), 1044353, 1, 1044361 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
			index = AddCraft( typeof( TotalRefreshPotion ), 1044530, 1044539, 25.0, 75.0, typeof( BlackPearl ), 1044353, 5, 1044361 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );

			// Agility Potion
			index = AddCraft( typeof( AgilityPotion ), 1044531, 1044540, 15.0, 65.0, typeof( Bloodmoss ), 1044354, 1, 1044362 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
			index = AddCraft( typeof( GreaterAgilityPotion ), 1044531, 1044541, 35.0, 85.0, typeof( Bloodmoss ), 1044354, 3, 1044362 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );

			// Nightsight Potion
			index = AddCraft( typeof( NightSightPotion ), 1044532, 1044542, -25.0, 25.0, typeof( SpidersSilk ), 1044360, 1, 1044368 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );

			// Heal Potion
			index = AddCraft( typeof( LesserHealPotion ), 1044533, 1044543, -25.0, 25.0, typeof( Ginseng ), 1044356, 1, 1044364 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
			index = AddCraft( typeof( HealPotion ), 1044533, 1044544, 15.0, 65.0, typeof( Ginseng ), 1044356, 3, 1044364 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
			index = AddCraft( typeof( GreaterHealPotion ), 1044533, 1044545, 55.0, 105.0, typeof( Ginseng ), 1044356, 7, 1044364 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );

			// Strength Potion
			index = AddCraft( typeof( StrengthPotion ), 1044534, 1044546, 25.0, 75.0, typeof( MandrakeRoot ), 1044357, 2, 1044365 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
			index = AddCraft( typeof( GreaterStrengthPotion ), 1044534, 1044547, 45.0, 95.0, typeof( MandrakeRoot ), 1044357, 5, 1044365 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );

			// Poison Potion
			index = AddCraft( typeof( LesserPoisonPotion ), 1044535, 1044548, -5.0, 45.0, typeof( Nightshade ), 1044358, 1, 1044366 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
			index = AddCraft( typeof( PoisonPotion ), 1044535, 1044549, 15.0, 65.0, typeof( Nightshade ), 1044358, 2, 1044366 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
			index = AddCraft( typeof( GreaterPoisonPotion ), 1044535, 1044550, 55.0, 105.0, typeof( Nightshade ), 1044358, 4, 1044366 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
			index = AddCraft( typeof( DeadlyPoisonPotion ), 1044535, 1044551, 90.0, 110.0, typeof( Nightshade ), 1044358, 8, 1044366 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );

			// Cure Potion
			index = AddCraft( typeof( LesserCurePotion ), 1044536, 1044552, -10.0, 40.0, typeof( Garlic ), 1044355, 1, 1044363 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
			index = AddCraft( typeof( CurePotion ), 1044536, 1044553, 25.0, 75.0, typeof( Garlic ), 1044355, 3, 1044363 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
			index = AddCraft( typeof( GreaterCurePotion ), 1044536, 1044554, 65.0, 115.0, typeof( Garlic ), 1044355, 6, 1044363 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );

			// Explosion Potion
			index = AddCraft( typeof( LesserExplosionPotion ), 1044537, 1044555, 5.0, 55.0, typeof( SulfurousAsh ), 1044359, 3, 1044367 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
			index = AddCraft( typeof( ExplosionPotion ), 1044537, 1044556, 35.0, 85.0, typeof( SulfurousAsh ), 1044359, 5, 1044367 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
			index = AddCraft( typeof( GreaterExplosionPotion ), 1044537, 1044557, 65.0, 115.0, typeof( SulfurousAsh ), 1044359, 10, 1044367 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );

            //Mana Potion
            index = AddCraft(typeof(ManaPotion), "Misc Potions", "Mana Potion", 60.0, 101.0, typeof(EyesOfNewt), 1023975, 4, "You do not have enough eyes of newts to make that potion.");
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            //Total Mana Potion
            index = AddCraft(typeof(TotalManaPotion), "Misc Potions", "Total Mana Potion", 90.0, 102.0, typeof(EyesOfNewt), 1023975, 8, "You do not have enough eyes of newts to make that potion.");
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            //Shrink pot
            index = AddCraft(typeof(ShrinkPotion), "Misc Potions", "Shrink Potion", 55.0, 90.0, typeof(BatWing), 1023960, 2, "You do not have enough bat wings to make that potion.");
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            //invis pot
	        index = AddCraft(typeof (InvisibilityPotion), "Misc Potions", "Invisibility Potion", 95.0, 105.0, typeof (WyrmsHeart), "Wyrms Heart", 3, "You do not have enough wyrms heart to make that potion.");
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);

			if( Core.SE )
			{
				index = AddCraft( typeof( SmokeBomb ), 1044537, 1030248, 90.0, 120.0, typeof( Eggs ), 1044477, 1, 1044253 );
				AddRes( index, typeof ( Ginseng ), 1044356, 3, 1044364 );
				SetNeededExpansion( index, Expansion.SE );

                // Conflagration Potions
                index = AddCraft(typeof(ConflagrationPotion), 1044109, 1072096, 55.0, 105.0, typeof(GraveDust), 1023983, 5, 1044253);
                AddRes(index, typeof(Bottle), 1044529, 1, 500315);
                SetNeededExpansion(index, Expansion.SE);
                index = AddCraft(typeof(GreaterConflagrationPotion), 1044109, 1072099, 65.0, 115.0, typeof(GraveDust), 1023983, 10, 1044253);
                AddRes(index, typeof(Bottle), 1044529, 1, 500315);
                SetNeededExpansion(index, Expansion.SE);
                // Confusion Blast Potions
                index = AddCraft(typeof(ConfusionBlastPotion), 1044109, 1072106, 55.0, 105.0, typeof(PigIron), 1023978, 5, 1044253);
                AddRes(index, typeof(Bottle), 1044529, 1, 500315);
                SetNeededExpansion(index, Expansion.SE);
                index = AddCraft(typeof(GreaterConfusionBlastPotion), 1044109, 1072109, 65.0, 115.0, typeof(PigIron), 1023978, 10, 1044253);
                AddRes(index, typeof(Bottle), 1044529, 1, 500315);
                SetNeededExpansion(index, Expansion.SE);
			}

            MarkOption = false;
		}
	}
}