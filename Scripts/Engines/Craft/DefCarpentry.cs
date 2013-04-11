//Rev 187

using System;
using Server.Items;
using Server.Multis;

namespace Server.Engines.Craft
{
	public class DefCarpentry : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Carpentry;	}
		}

		public override int GumpTitleNumber
		{
			get { return 1044004; } // <CENTER>CARPENTRY MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefCarpentry();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.5; // 50%
		}

        public override bool RetainsColorFrom(CraftItem item, Type type)
        {
            //Taran: Only color furnitures and containers
            if (item.ItemType.IsDefined(typeof(FurnitureAttribute), false) || item.ItemType == typeof(BaseContainer))
                return true;

            return false;
        }

		private DefCarpentry() : base( 2, 5, 1.0 )// base( 1, 1, 3.0 )
		{
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if( tool == null || tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.

			return 0;
		}

		public override void PlayCraftEffect( Mobile from )
		{
			// no animation 
			if ( from.Body.Type == BodyType.Human ) // Vulcan: Added mounted animation
            {
                if (!from.Mounted)
                    from.Animate(9, 5, 1, true, false, 0);
                else
                    from.Animate(26, 5, 1, true, false, 0);
            }
			from.PlaySound( 0x23D );
		}

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item )
		{
			if ( toolBroken )
				from.SendLocalizedMessage( 1044038 ); // You have worn out your tool

			if ( failed )
			{
				if ( lostMaterial )
					return 1044043; // You failed to create the item, and some of your materials are lost.
				else
					return 1044157; // You failed to create the item, but no materials were lost.
			}
			else
			{
				if ( quality == 0 )
					return 502785; // You were barely able to make this item.  It's quality is below average.
                else if ( MarkOption && quality == 2 && from.Skills[SkillName.Carpentry].Base >= 100.0)
					return 1044156; // You create an exceptional quality item and affix your maker's mark.
				else if ( quality == 2 )
					return 1044155; // You create an exceptional quality item.
				else				
					return 1044154; // You create the item.
			}
		}

		public override void InitCraftList()
		{
			int index = -1;

            #region Other Items
            //index = AddCraft(typeof(Board), 1044294, 1027127, 0.0, 0.0, typeof(Log), 1044466, 1, 1044465);
            //SetUseAllRes(index, true);

            AddCraft(typeof(TrashBarrel), 1044294, string.Format("trash barrel"), 42.6, 77.6, typeof(Log), 1044041, 50, 1044351);
            index = AddCraft(typeof(BlankScroll), 1044294, string.Format("blank scroll"), 70.0, 100.0, typeof(Log), 1044041, 1, 1044351);
            AddSkill(index, SkillName.Inscribe, 45.0, 50.0);
            AddRes(index, typeof(SpidersSilk), string.Format("spider's silk"), 1, 1044351);
			AddCraft( typeof( BarrelStaves ),				1044294, 1027857,	00.0,  25.0,	typeof( Log ), 1044041,  5, 1044351 );
			AddCraft( typeof( BarrelLid ),					1044294, 1027608,	11.0,  36.0,	typeof( Log ), 1044041,  4, 1044351 );
			AddCraft( typeof( ShortMusicStand ),			1044294, 1044313,	78.9, 103.9,	typeof( Log ), 1044041, 15, 1044351 );
			AddCraft( typeof( TallMusicStand ),				1044294, 1044315,	81.5, 106.5,	typeof( Log ), 1044041, 20, 1044351 );
			AddCraft( typeof( Easle ),						1044294, 1044317,	86.8, 111.8,	typeof( Log ), 1044041, 20, 1044351 );
		    index = AddCraft(typeof (PlayerBBSouthDeed), 1044294, string.Format("bulletin board (south)"), 90.0, 112.5, typeof (Log), 1044041, 35, 1044351);
            AddRes(index, typeof(BlankScroll), string.Format("blank scroll"), 100, 1044378);
            index = AddCraft(typeof(PlayerBBEastDeed), 1044294, string.Format("bulletin board (east)"), 90.0, 112.5, typeof(Log), 1044041, 35, 1044351);
            AddRes(index, typeof(BlankScroll), string.Format("blank scroll"), 100, 1044378);
            index = AddCraft(typeof(DogHouse), "Add-Ons", "dog house", 95.5, 112.5, typeof(Log), 1044041, 55, 1044351);
            AddSkill(index, SkillName.Tinkering, 80.0, 85.0);
            AddRes(index, typeof(Nails), "nails", 5, "you do not have enough nails to make that");
			if( Core.SE )
			{
				index = AddCraft( typeof( RedHangingLantern ), 1044294, 1029412, 65.0, 90.0, typeof( Log ), 1044041, 5, 1044351 );
				AddRes( index, typeof( BlankScroll ), 1044377, 10, 1044378 );
				SetNeededExpansion( index, Expansion.SE );

				index = AddCraft( typeof( WhiteHangingLantern ), 1044294, 1029416, 65.0, 90.0, typeof( Log ), 1044041, 5, 1044351 );
				AddRes( index, typeof( BlankScroll ), 1044377, 10, 1044378 );
				SetNeededExpansion( index, Expansion.SE );

				index = AddCraft( typeof( ShojiScreen ), 1044294, 1029423, 80.0, 105.0, typeof( Log ), 1044041, 75, 1044351 );
				AddSkill( index, SkillName.Tailoring, 50.0, 55.0 );
				AddRes( index, typeof( Cloth ), 1044286, 60, 1044287 );
				SetNeededExpansion( index, Expansion.SE );

				index = AddCraft( typeof( BambooScreen ), 1044294, 1029428, 80.0, 105.0, typeof( Log ), 1044041, 75, 1044351 );
				AddSkill( index, SkillName.Tailoring, 50.0, 55.0 );
				AddRes( index, typeof( Cloth ), 1044286, 60, 1044287 );
				SetNeededExpansion( index, Expansion.SE );
			}

            if (Core.AOS)	//Duplicate Entries to preserve ordering depending on era 
            {
                index = AddCraft(typeof(FishingPole), 1044294, 1023519, 68.4, 93.4, typeof(Log), 1044041, 5, 1044351); //This is in the categor of Other during AoS
                AddSkill(index, SkillName.Tailoring, 40.0, 45.0);
                AddRes(index, typeof(Cloth), 1044286, 5, 1044287);
            }

            if (Core.ML)
            {
                index = AddCraft(typeof(RunedSwitch), 1044294, 1072896, 70.0, 120.0, typeof(Log), 1044041, 2, 1044351);
                AddRes(index, typeof(EnchantedSwitch), 1072893, 1, 1053098);
                AddRes(index, typeof(RunedPrism), 1073465, 1, 1053098);
                AddRes(index, typeof(JeweledFiligree), 1072894, 1, 1053098);
                SetNeededExpansion(index, Expansion.ML);
            }
            #endregion

            #region Furniture
            AddCraft( typeof( FootStool ),					1044291, 1022910,	11.0,  36.0,	            typeof( Log ), 1044041,  9, 1044351 );
			AddCraft( typeof( Stool ),						1044291, 1022602,	11.0,  36.0,	            typeof( Log ), 1044041,  9, 1044351 );
			AddCraft( typeof( BambooChair ),				1044291, 1044300,	21.0,  46.0,	            typeof( Log ), 1044041, 13, 1044351 );
			AddCraft( typeof( WoodenChair ),				1044291, 1044301,	21.0,  46.0,	            typeof( Log ), 1044041, 13, 1044351 );
			AddCraft( typeof( FancyWoodenChairCushion ),	1044291, 1044302,	42.1,  67.1,	            typeof( Log ), 1044041, 15, 1044351 );
			AddCraft( typeof( WoodenChairCushion ),			1044291, 1044303,	42.1,  67.1,	            typeof( Log ), 1044041, 13, 1044351 );
			AddCraft( typeof( WoodenBench ),				1044291, 1022860,	52.6,  77.6,	            typeof( Log ), 1044041, 17, 1044351 );
			AddCraft( typeof( WoodenThrone ),				1044291, 1044304,	52.6,  77.6,            	typeof( Log ), 1044041, 17, 1044351 );
			AddCraft( typeof( Throne ),						1044291, 1044305,	73.6,  98.6,            	typeof( Log ), 1044041, 23, 1044351 );
			AddCraft( typeof( Nightstand ),					1044291, 1044306,	42.1,  67.1,            	typeof( Log ), 1044041, 17, 1044351 );
            AddCraft( typeof( WritingTable ),               1044291, 1022890,   63.1,  88.1,                typeof( Log ), 1044041, 17, 1044351 );
			AddCraft( typeof( YewWoodTable ),				1044291, 1044308,	63.1,  88.1,            	typeof( Log ), 1044041, 23, 1044351 );
			AddCraft( typeof( LargeTable ),					1044291, 1044307,	73.6,  98.6,            	typeof( Log ), 1044041, 27, 1044351 );
            AddCraft( typeof( CornerDisplay ),              1044291, "corner display case",   95.0, 110.0,  typeof( Log ), 1044041, 65, 1044351 );
            AddCraft( typeof( BarTable ),                   1044291, "bar table",   89.2, 114.2,            typeof( Log ), 1044041, 30, 1044351 );
            AddCraft( typeof( BarDoor ),                    1044291, "bar door",    89.2, 114.2,            typeof( Log ), 1044041, 20, 1044351 );
            AddCraft( typeof( Table ), 1044291, "table", 73.6, 98.6, typeof(Log), 1044041, 14, 1044351);
            AddCraft( typeof( Table2 ), 1044291, "table 2", 73.6, 98.6, typeof(Log), 1044041, 14, 1044351);
            AddCraft( typeof( LongTable ), 1044291, "long table", 63.1, 88.1, typeof(Log), 1044041, 14, 1044351);
            AddCraft( typeof( ColoredTable ), 1044291, "colored table", 84.2, 109.2, typeof(Log), 1044041, 17, 1044351);
            AddCraft( typeof( ColoredTable2 ), 1044291, "colored table 2", 84.2, 109.2, typeof(Log), 1044041, 17, 1044351);

            index = AddCraft(typeof(DeskEastAddonDeed), 1044291, "desk east", 90.0, 115.0, typeof(Log), 1044041, 110, 1044351);
            AddSkill(index, SkillName.Tinkering, 95.0, 100.0);
            AddRes(index, typeof(Nails), "nails", 10, "you do not have enough nails to make that");

            index = AddCraft(typeof(DeskSouthAddonDeed), 1044291, "desk west", 90.0, 115.0, typeof(Log), 1044041, 110, 1044351);
            AddSkill(index, SkillName.Tinkering, 95.0, 100.0);
            AddRes(index, typeof(Nails), "nails", 10, "you do not have enough nails to make that");

            AddCraft(typeof(Counter), 1044291, "counter", 73.6, 98.6, typeof(Log), 1044041, 23, 1044351);

			if( Core.SE )
			{
				index = AddCraft( typeof( ElegantLowTable ),	1044291, 1030265,	80.0, 105.0,	typeof( Log ), 1044041, 35, 1044351 );
				SetNeededExpansion( index, Expansion.SE );

				index = AddCraft( typeof( PlainLowTable ),		1044291, 1030266,	80.0, 105.0,	typeof( Log ), 1044041, 35, 1044351 );
				SetNeededExpansion( index, Expansion.SE );
            }
            #endregion

            #region Containers
            AddCraft( typeof( WoodenBox ),					1044292, 1023709,	21.0,  46.0,	typeof( Log ), 1044041, 10, 1044351 );
			AddCraft( typeof( SmallCrate ),					1044292, 1044309,	10.0,  35.0,	typeof( Log ), 1044041, 8 , 1044351 );
			AddCraft( typeof( MediumCrate ),				1044292, 1044310,	31.0,  56.0,	typeof( Log ), 1044041, 15, 1044351 );
			AddCraft( typeof( LargeCrate ),					1044292, 1044311,	47.3,  72.3,	typeof( Log ), 1044041, 18, 1044351 );
			AddCraft( typeof( WoodenChest ),				1044292, 1023650,	73.6,  98.6,	typeof( Log ), 1044041, 20, 1044351 );
			AddCraft( typeof( EmptyBookcase ),				1044292, 1022718,	31.5,  56.5,	typeof( Log ), 1044041, 25, 1044351 );
			AddCraft( typeof( FancyArmoire ),				1044292, 1044312,	84.2, 109.2,	typeof( Log ), 1044041, 35, 1044351 );
			AddCraft( typeof( Armoire ),					1044292, 1022643,	84.2, 109.2,	typeof( Log ), 1044041, 35, 1044351 );

            index = AddCraft(typeof(OakNightStand), 1044292, "oak nightstand", 90.0, 107.0, typeof(Log), 1044041, 30, 1044351);
            AddSkill(index, SkillName.Tinkering, 50.0, 60.0);
            AddRes(index, typeof(Nails), "nails", 2, "you do not have enough nails to make that");

            index = AddCraft(typeof(EngravedArmoire),       1044292, "engraved armoire", 90.0, 110.5, typeof(Log), 1044041, 50, 1044351);
            AddSkill(index, SkillName.Tinkering, 95.0, 100.0);
            AddRes(index, typeof(Nails), "nails", 5, "you do not have enough nails to make that");

            index = AddCraft(typeof(SmallBookShelf), 1044292, "small full bookcase", 90.0, 102.0, typeof(Log), 1044041, 12, 1044351);
            AddSkill(index, SkillName.Inscribe, 50.0, 50.0);
            AddRes(index, typeof(BlueBook), "blue book", 3, "You do not have sufficient books to make that.");
            AddRes(index, typeof(RedBook), "red book", 3, "You do not have sufficient books to make that.");
            AddRes(index, typeof(BrownBook), "brown book", 3, "You do not have sufficient books to make that.");
            AddRes(index, typeof(TanBook), "tan book", 3, "You do not have sufficient books to make that.");

            index = AddCraft(typeof(FullBookcase),          1044292, "full bookcase", 90.0, 102.5, typeof(Log), 1044041, 12, 1044351);
            AddSkill(index, SkillName.Inscribe, 55.0, 60.0);
            AddRes(index, typeof(EmptyBookcase), "empty bookcase", 1, "You need an empty bookcase to make that.");
            AddRes(index, typeof(BlueBook), "blue book", 6, "You do not have sufficient books to make that.");
            AddRes(index, typeof(RedBook), "red book", 6, "You do not have sufficient books to make that.");
            AddRes(index, typeof(BrownBook), "brown book", 6, "You do not have sufficient books to make that.");
            AddRes(index, typeof(TanBook), "tan book", 6, "You do not have sufficient books to make that.");

			if( Core.SE )
			{
				index = AddCraft( typeof( PlainWoodenChest ),	1044292, 1030251, 90.0, 115.0,	typeof( Log ), 1044041, 30, 1044351 );
				SetNeededExpansion( index, Expansion.SE );

				index = AddCraft( typeof( OrnateWoodenChest ),	1044292, 1030253, 90.0, 115.0,	typeof( Log ), 1044041, 30, 1044351 );
				SetNeededExpansion( index, Expansion.SE );

				index = AddCraft( typeof( GildedWoodenChest ),	1044292, 1030255, 90.0, 115.0,	typeof( Log ), 1044041, 30, 1044351 );
				SetNeededExpansion( index, Expansion.SE );

				index = AddCraft( typeof( WoodenFootLocker ),	1044292, 1030257, 90.0, 115.0,	typeof( Log ), 1044041, 30, 1044351 );
				SetNeededExpansion( index, Expansion.SE );

				index = AddCraft( typeof( FinishedWoodenChest ),1044292, 1030259, 90.0, 115.0,	typeof( Log ), 1044041, 30, 1044351 );
				SetNeededExpansion( index, Expansion.SE );

				index = AddCraft( typeof( TallCabinet ),	1044292, 1030261, 90.0, 115.0,	typeof( Log ), 1044041, 35, 1044351 );
				SetNeededExpansion( index, Expansion.SE );

				index = AddCraft( typeof( ShortCabinet ),	1044292, 1030263, 90.0, 115.0,	typeof( Log ), 1044041, 35, 1044351 );
				SetNeededExpansion( index, Expansion.SE );

				index = AddCraft( typeof( RedArmoire ),	1044292, 1030328, 90.0, 115.0,	typeof( Log ), 1044041, 40, 1044351 );
				SetNeededExpansion( index, Expansion.SE );
				
				index = AddCraft( typeof( ElegantArmoire ),	1044292, 1030330, 90.0, 115.0,	typeof( Log ), 1044041, 40, 1044351 );
				SetNeededExpansion( index, Expansion.SE );

                index = AddCraft(typeof(MapleArmoire), 1044292, 1030332, 90.0, 115.0, typeof(Log), 1044041, 40, 1044351);
                SetNeededExpansion(index, Expansion.SE);

                index = AddCraft(typeof(CherryArmoire), 1044292, 1030334, 90.0, 115.0, typeof(Log), 1044041, 40, 1044351);
                SetNeededExpansion(index, Expansion.SE);
			}

			index = AddCraft( typeof( Keg ), 1044292, 1023711, 57.8, 82.8, typeof( BarrelStaves ), 1044288, 3, 1044253 );
			AddRes( index, typeof( BarrelHoops ), 1044289, 1, 1044253 );
			AddRes( index, typeof( BarrelLid ), 1044251, 1, 1044253 );
            #endregion

            #region Staves and Shields
            AddCraft(typeof(Club), 1044295, 1025043, 32.9, 70.9, typeof(Log), 1044041, 3, 1044351);
			AddCraft( typeof( ShepherdsCrook ), 1044295, 1023713, 78.9, 103.9, typeof( Log ), 1044041, 7, 1044351 );
			AddCraft( typeof( QuarterStaff ), 1044295, 1023721, 73.6, 98.6, typeof( Log ), 1044041, 6, 1044351 );
			AddCraft( typeof( GnarledStaff ), 1044295, 1025112, 78.9, 103.9, typeof( Log ), 1044041, 7, 1044351 );
			AddCraft( typeof( WoodenShield ), 1044295, 1027034, 52.6, 77.6, typeof( Log ), 1044041, 9, 1044351 );

            index = AddCraft(typeof(GoblinClooba), 1044295, "Goblin Clooba", 76.0, 100.5, typeof(OakLog), "Oak Log", 10, 1044037);
            AddRes(index, typeof(OldCopperIngot), "Old Copper Ingots", 10, 1044037);
            AddRes(index, typeof(Emerald), "Emeralds", 4, "You lack emeralds to create this");
            AddRes(index, typeof(Log), 1044041, 20, 1044351);
            AddSkill(index, SkillName.Tactics, 80.0, 100.0);

            index = AddCraft(typeof(LeafShield), 1044295, "leaf shield", 75.5, 102.5, typeof(Log), 1044041, 22, 1044351);
            AddRes(index, typeof(OakBoard), "Oak Board", 10, "You lack the oak boards to make this");
            AddRes(index, typeof(AshBoard), "Ash Board", 10, "You lack the ash boards to make this");
            AddSkill(index, SkillName.Parry, 100.0, 100.0);

            /*if (!Core.AOS)	//Duplicate Entries to preserve ordering depending on era 
            {
                index = AddCraft(typeof(FishingPole), 1044295, 1023519, 68.4, 93.4, typeof(Log), 1044041, 5, 1044351); //This is in the categor of Other during AoS
                AddSkill(index, SkillName.Tailoring, 40.0, 45.0);
                AddRes(index, typeof(Cloth), 1044286, 5, 1044287);
            }
            */
			index = AddCraft( typeof( FishingPole ), Core.AOS ? 1044294 : 1044295, 1023519, 68.4, 93.4, typeof( Log ), 1044041, 3, 1044351 ); //This is in the categor of Other during AoS
			AddSkill( index, SkillName.Tailoring, 40.0, 45.0 );
			AddRes( index, typeof( Cloth ), 1044286, 5, 1044287 );
             
			if( Core.SE )
			{
				index = AddCraft( typeof( Bokuto ), 1044295, 1030227, 70.0, 95.0, typeof( Log ), 1044041, 6, 1044351 );
				SetNeededExpansion( index, Expansion.SE );

				index = AddCraft( typeof( Fukiya ), 1044295, 1030229, 60.0, 85.0, typeof( Log ), 1044041, 6, 1044351 );
				SetNeededExpansion( index, Expansion.SE );

				index = AddCraft( typeof( Tetsubo ), 1044295, 1030225, 85.0, 110.0, typeof( Log ), 1044041, 8, 1044351 );
				AddSkill( index, SkillName.Tinkering, 40.0, 45.0 );
				AddRes( index, typeof( IronIngot ), 1044036, 5, 1044037 );
				SetNeededExpansion( index, Expansion.SE );
            }
            #endregion

            #region Instruments
            index = AddCraft( typeof( LapHarp ), 1044293, 1023762, 63.1, 88.1, typeof( Log ), 1044041, 20, 1044351 );
			AddSkill( index, SkillName.Musicianship, 45.0, 50.0 );
			AddRes( index, typeof( Cloth ), 1044286, 10, 1044287 );

			index = AddCraft( typeof( Harp ), 1044293, 1023761, 78.9, 103.9, typeof( Log ), 1044041, 35, 1044351 );
			AddSkill( index, SkillName.Musicianship, 45.0, 50.0 );
			AddRes( index, typeof( Cloth ), 1044286, 15, 1044287 );

            index = AddCraft(typeof(Madroneharp), 1044293, "madrone harp", 88.9, 105.9, typeof(Log), 1044041, 35, 1044351);
            AddSkill(index, SkillName.Musicianship, 75.0, 90.0);
            AddRes(index, typeof(Cloth), 1044286, 15, 1044287);

            index = AddCraft(typeof(MapleHarp), 1044293, "maple harp", 88.9, 105.9, typeof(Log), 1044041, 35, 1044351);
            AddSkill(index, SkillName.Musicianship, 75.0, 90.0);
            AddRes(index, typeof(Cloth), 1044286, 15, 1044287);

            index = AddCraft(typeof(Walnutharp), 1044293, "walnut harp", 88.9, 105.9, typeof(Log), 1044041, 35, 1044351);
            AddSkill(index, SkillName.Musicianship, 75.0, 90.0);
            AddRes(index, typeof(Cloth), 1044286, 15, 1044287);
			
			index = AddCraft( typeof( Drums ), 1044293, 1023740, 57.8, 82.8, typeof( Log ), 1044041, 20, 1044351 );
			AddSkill( index, SkillName.Musicianship, 45.0, 50.0 );
			AddRes( index, typeof( Cloth ), 1044286, 10, 1044287 );

            index = AddCraft(typeof(Pandrum), 1044293, "pan drum", 90.0, 102.8, typeof(Log), 1044041, 10, 1044351);
            AddSkill(index, SkillName.Musicianship, 85.0, 100.0);
            AddRes(index, typeof(IronIngot), 1044036, 75, 1044037);
            AddRes(index, typeof( Leather ), 1044462, 10, 1044463 );

            index = AddCraft(typeof(Taikodrum), 1044293, "taiko drum", 65.0, 90.0, typeof(Log), 1044041, 20, 1044351);
            AddSkill(index, SkillName.Musicianship, 75.0, 80.0);
            AddRes(index, typeof(Cloth), 1044286, 10, 1044287);

            index = AddCraft(typeof(WarDrum), 1044293, "war drum", 65.0, 90.0, typeof(Log), 1044041, 20, 1044351);
            AddSkill(index, SkillName.Musicianship, 75.0, 80.0);
            AddRes(index, typeof(Cloth), 1044286, 10, 1044287);
			
			index = AddCraft( typeof( Lute ), 1044293, 1023763, 68.4, 93.4, typeof( Log ), 1044041, 25, 1044351 );
			AddSkill( index, SkillName.Musicianship, 45.0, 50.0 );
			AddRes( index, typeof( Cloth ), 1044286, 10, 1044287 );

            index = AddCraft(typeof(Lullabylute), 1044293, "lullaby lute", 78.4, 93.4, typeof(Log), 1044041, 25, 1044351);
            AddSkill(index, SkillName.Musicianship, 75.0, 90.0);
            AddRes(index, typeof(Cloth), 1044286, 10, 1044287);

            index = AddCraft(typeof(Seductionlute), 1044293, "seduction lute", 78.4, 93.4, typeof(Log), 1044041, 25, 1044351);
            AddSkill(index, SkillName.Musicianship, 75.0, 90.0);
            AddRes(index, typeof(Cloth), 1044286, 10, 1044287);

            index = AddCraft(typeof(Suspenselute), 1044293, "suspense lute", 78.4, 93.4, typeof(Log), 1044041, 25, 1044351);
            AddSkill(index, SkillName.Musicianship, 75.0, 90.0);
            AddRes(index, typeof(Cloth), 1044286, 10, 1044287);
			
			index = AddCraft( typeof( Tambourine ), 1044293, 1023741, 57.8, 82.8, typeof( Log ), 1044041, 15, 1044351 );
			AddSkill( index, SkillName.Musicianship, 45.0, 50.0 );
			AddRes( index, typeof( Cloth ), 1044286, 10, 1044287 );

			index = AddCraft( typeof( TambourineTassel ), 1044293, 1044320, 57.8, 82.8, typeof( Log ), 1044041, 15, 1044351 );
			AddSkill( index, SkillName.Musicianship, 45.0, 50.0 );
			AddRes( index, typeof( Cloth ), 1044286, 15, 1044287 );

            index = AddCraft(typeof(RitualTambourine), 1044293, "ritual tambourine", 60.5, 90.5, typeof(Log), 1044041, 15, 1044351);
            AddSkill(index, SkillName.Musicianship, 65.5, 75.5);
            AddRes(index, typeof(Cloth), 1044286, 10, 1044287);

            index = AddCraft(typeof(Traditionaltambourine), 1044293, "traditional tambourine", 60.5, 90.5, typeof(Log), 1044041, 15, 1044351);
            AddSkill(index, SkillName.Musicianship, 65.5, 75.5);
            AddRes(index, typeof(Cloth), 1044286, 10, 1044287);

            index = AddCraft(typeof(Bubenclacker), 1044293, "buben clacker", 60.5, 90.5, typeof(Log), 1044041, 15, 1044351);
            AddSkill(index, SkillName.Musicianship, 65.5, 75.5);
            AddRes(index, typeof(Cloth), 1044286, 10, 1044287);

            index = AddCraft(typeof(Bansuriflute), 1044293, "bansuri flute", 65.5, 92.5, typeof(Log), 1044041, 12, 1044351);
            AddSkill(index, SkillName.Musicianship, 75.5, 85.5);

            index = AddCraft(typeof(Kavalflute), 1044293, "kaval flute", 65.5, 92.5, typeof(Log), 1044041, 12, 1044351);
            AddSkill(index, SkillName.Musicianship, 75.5, 85.5);

            index = AddCraft(typeof(fiddle), 1044293, "fiddle", 80.5, 102.5, typeof(Log), 1044041, 20, 1044351);
            AddSkill(index, SkillName.Musicianship, 85.5, 95.5);

            index = AddCraft(typeof(Trumpet), 1044293, "trumpet", 94.5, 110.5, typeof(Log), 1044041, 5, 1044351);
            AddSkill(index, SkillName.Musicianship, 85.5, 95.5);
            AddRes(index, typeof(IronIngot), 1044036, 50, 1044037);

            index = AddCraft(typeof(Ocarina), 1044293, "ocarina", 97.5, 105.5, typeof(Log), 1044041, 20, 1044351);
            AddSkill(index, SkillName.Musicianship, 90.0, 95.5);
            AddRes(index, typeof(IronIngot), 1044036, 5, 1044037);

            index = AddCraft(typeof(PianoAddonDeed), 1044293, "piano", 87.8, 105.0, typeof(Log), 1044041, 200, 1044351);
            AddSkill(index, SkillName.Musicianship, 95.0, 100.0);
            AddRes(index, typeof(Cloth), 1044286, 50, 1044287);
            AddRes(index, typeof(IronIngot), 1044036, 25, 1044037);

            index = AddCraft(typeof(PerformersHarpAddonDeed), 1044293, "performer's harp", 88.0, 105.0, typeof(Log), 1044041, 40, 1044351);
            AddSkill(index, SkillName.Musicianship, 95.0, 100.0);
            AddRes(index, typeof(IronIngot), 1044036, 75, 1044037);
            AddRes(index, typeof(Granite), "Granite", 12, "You do not have sufficient granite to make that.");

			if( Core.SE )
			{
				index = AddCraft( typeof( BambooFlute ), 1044293, 1030247, 80.0, 105.0, typeof( Log ), 1044041, 15, 1044351 );
				AddSkill( index, SkillName.Musicianship, 45.0, 50.0 );
				SetNeededExpansion( index, Expansion.SE );
            }
            #endregion

            #region Addons
            index = AddCraft( typeof( SmallBedSouthDeed ), "Add-Ons", 1044321, 94.7, 113.1, typeof( Log ), 1044041, 100, 1044351 );
			AddSkill( index, SkillName.Tailoring, 75.0, 80.0 );
			AddRes( index, typeof( Cloth ), 1044286, 100, 1044287 );
            index = AddCraft(typeof(SmallBedEastDeed), "Add-Ons", 1044322, 94.7, 113.1, typeof(Log), 1044041, 100, 1044351);
			AddSkill( index, SkillName.Tailoring, 75.0, 80.0 );
			AddRes( index, typeof( Cloth ), 1044286, 100, 1044287 );
            index = AddCraft(typeof(LargeBedSouthDeed), "Add-Ons", 1044323, 94.7, 113.1, typeof(Log), 1044041, 150, 1044351);
			AddSkill( index, SkillName.Tailoring, 75.0, 80.0 );
			AddRes( index, typeof( Cloth ), 1044286, 150, 1044287 );
            index = AddCraft(typeof(LargeBedEastDeed), "Add-Ons", 1044324, 94.7, 113.1, typeof(Log), 1044041, 150, 1044351);
			AddSkill( index, SkillName.Tailoring, 75.0, 80.0 );
			AddRes( index, typeof( Cloth ), 1044286, 150, 1044287 );
            AddCraft(typeof(DartBoardSouthDeed), "Add-Ons", 1044325, 15.7, 40.7, typeof(Log), 1044041, 5, 1044351);
            AddCraft(typeof(DartBoardEastDeed), "Add-Ons", 1044326, 15.7, 40.7, typeof(Log), 1044041, 5, 1044351);
            AddCraft(typeof(BallotBoxDeed), "Add-Ons", 1044327, 47.3, 72.3, typeof(Log), 1044041, 5, 1044351);
            index = AddCraft(typeof(PentagramDeed), "Add-Ons", 1044328, 95.0, 101.0, typeof(Log), 1044041, 100, 1044351);
			AddSkill( index, SkillName.Magery, 75.0, 80.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 40, 1044037 );
            index = AddCraft(typeof(AbbatoirDeed), "Add-Ons", 1044329, 95.0, 101.0, typeof(Log), 1044041, 100, 1044351);
			AddSkill( index, SkillName.Magery, 50.0, 55.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 40, 1044037 );
            index = AddCraft(typeof(TrainingDummyEastDeed), "Add-Ons", 1044335, 68.4, 93.4, typeof(Log), 1044041, 55, 1044351);
            AddSkill(index, SkillName.Tailoring, 50.0, 55.0);
            AddRes(index, typeof(Cloth), 1044286, 60, 1044287);
            index = AddCraft(typeof(TrainingDummySouthDeed), "Add-Ons", 1044336, 68.4, 93.4, typeof(Log), 1044041, 55, 1044351);
            AddSkill(index, SkillName.Tailoring, 50.0, 55.0);
            AddRes(index, typeof(Cloth), 1044286, 60, 1044287);
            index = AddCraft(typeof(PickpocketDipEastDeed), "Add-Ons", 1044337, 73.6, 98.6, typeof(Log), 1044041, 65, 1044351);
            AddSkill(index, SkillName.Tailoring, 50.0, 55.0);
            AddRes(index, typeof(Cloth), 1044286, 60, 1044287);
            index = AddCraft(typeof(PickpocketDipSouthDeed), "Add-Ons", 1044338, 73.6, 98.6, typeof(Log), 1044041, 65, 1044351);
            AddSkill(index, SkillName.Tailoring, 50.0, 55.0);
            AddRes(index, typeof(Cloth), 1044286, 60, 1044287);
            index = AddCraft(typeof(ArcheryButteDeed), "Add-Ons", "archery butte", 68.4, 93.4, typeof(Log), 1044041, 50, 1044351);
            AddSkill(index, SkillName.Archery, 50.0, 55.0);
            AddRes(index, typeof(SheafOfHay), "sheaf of hay", 4, "you do not have enough hay to make that");

			if ( Core.AOS )
			{
				AddCraft( typeof( PlayerBBEast ), 1044290, 1062420, 85.0, 110.0, typeof( Log ), 1044041, 50, 1044351 );
				AddCraft( typeof( PlayerBBSouth ), 1044290, 1062421, 85.0, 110.0, typeof( Log ), 1044041, 50, 1044351 );
            }
            #endregion

            // Blacksmithy
			index = AddCraft( typeof( SmallForgeDeed ), 1044296, 1044330, 73.6, 98.6, typeof( Log ), 1044041, 5, 1044351 );
			AddSkill( index, SkillName.Blacksmith, 75.0, 80.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 75, 1044037 );
			index = AddCraft( typeof( LargeForgeEastDeed ), 1044296, 1044331, 78.9, 103.9, typeof( Log ), 1044041, 5, 1044351 );
			AddSkill( index, SkillName.Blacksmith, 80.0, 85.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 100, 1044037 );
			index = AddCraft( typeof( LargeForgeSouthDeed ), 1044296, 1044332, 78.9, 103.9, typeof( Log ), 1044041, 5, 1044351 );
			AddSkill( index, SkillName.Blacksmith, 80.0, 85.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 100, 1044037 );
			index = AddCraft( typeof( AnvilEastDeed ), 1044296, 1044333, 73.6, 98.6, typeof( Log ), 1044041, 5, 1044351 );
			AddSkill( index, SkillName.Blacksmith, 75.0, 80.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 150, 1044037 );
			index = AddCraft( typeof( AnvilSouthDeed ), 1044296, 1044334, 73.6, 98.6, typeof( Log ), 1044041, 5, 1044351 );
			AddSkill( index, SkillName.Blacksmith, 75.0, 80.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 150, 1044037 );

			// Tailoring
			index = AddCraft( typeof( Dressform ), "Tailoring", 1044339, 63.1, 88.1, typeof( Log ), 1044041, 25, 1044351 );
			AddSkill( index, SkillName.Tailoring, 65.0, 70.0 );
			AddRes( index, typeof( Cloth ), 1044286, 10, 1044287 );
            index = AddCraft(typeof(DyeTub), "Tailoring", "dye tub", 30.1, 50.1, typeof(Log), 1044041, 5, 1044351);
			index = AddCraft( typeof( SpinningwheelEastDeed ), "Tailoring", 1044341, 73.6, 98.6, typeof( Log ), 1044041, 75, 1044351 );
			AddSkill( index, SkillName.Tailoring, 65.0, 70.0 );
			AddRes( index, typeof( Cloth ), 1044286, 25, 1044287 );
			index = AddCraft( typeof( SpinningwheelSouthDeed ), "Tailoring", 1044342, 73.6, 98.6, typeof( Log ), 1044041, 75, 1044351 );
			AddSkill( index, SkillName.Tailoring, 65.0, 70.0 );
			AddRes( index, typeof( Cloth ), 1044286, 25, 1044287 );
			index = AddCraft( typeof( LoomEastDeed ), "Tailoring", 1044343, 84.2, 109.2, typeof( Log ), 1044041, 85, 1044351 );
			AddSkill( index, SkillName.Tailoring, 65.0, 70.0 );
			AddRes( index, typeof( Cloth ), 1044286, 25, 1044287 );
			index = AddCraft( typeof( LoomSouthDeed ), "Tailoring", 1044344, 84.2, 109.2, typeof( Log ), 1044041, 85, 1044351 );
			AddSkill( index, SkillName.Tailoring, 65.0, 70.0 );
			AddRes( index, typeof( Cloth ), 1044286, 25, 1044287 );
            
			// Cooking
            index = AddCraft(typeof(GrayBrickFireplaceEastDeed), 1044299, "gray brick fireplace (east)", 68.9, 95.4, typeof(Log), 1044041, 85, 1044351);
            AddSkill(index, SkillName.Tinkering, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 125, 1044037);
            index = AddCraft(typeof(GrayBrickFireplaceSouthDeed), 1044299, "gray brick fireplace (south)", 68.9, 95.4, typeof(Log), 1044041, 85, 1044351);
            AddSkill(index, SkillName.Tinkering, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 125, 1044037);
            index = AddCraft(typeof(SandstoneFireplaceEastDeed), 1044299, "sandstone fireplace (east)", 68.9, 95.4, typeof(Log), 1044041, 85, 1044351);
            AddSkill(index, SkillName.Tinkering, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 125, 1044037);
            index = AddCraft(typeof(SandstoneFireplaceSouthDeed), 1044299, "sandstone fireplace (south)", 68.9, 95.4, typeof(Log), 1044041, 85, 1044351);
            AddSkill(index, SkillName.Tinkering, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 125, 1044037);
            index = AddCraft(typeof(StoneFireplaceEastDeed), 1044299, "stone fireplace (east)", 68.4, 94.4, typeof(Log), 1044041, 85, 1044351);
            AddSkill(index, SkillName.Tinkering, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 125, 1044037);
            index = AddCraft(typeof(StoneFireplaceSouthDeed), 1044299, "stone fireplace (south)", 68.4, 94.4, typeof(Log), 1044041, 85, 1044351);
            AddSkill(index, SkillName.Tinkering, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 125, 1044037);
            index = AddCraft(typeof(StoneOvenEastDeed), 1044299, 1044345, 68.4, 93.4, typeof(Log), 1044041, 85, 1044351);
            AddSkill(index, SkillName.Tinkering, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 125, 1044037); 
            index = AddCraft(typeof(StoneOvenSouthDeed), 1044299, 1044346, 68.4, 93.4, typeof(Log), 1044041, 85, 1044351);
            AddSkill(index, SkillName.Tinkering, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 125, 1044037);
			index = AddCraft( typeof( FlourMillEastDeed ), 1044299, 1044347, 94.7, 102.7, typeof( Log ), 1044041, 100, 1044351 );
			AddSkill( index, SkillName.Tinkering, 50.0, 55.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 50, 1044037 );
			index = AddCraft( typeof( FlourMillSouthDeed ), 1044299, 1044348, 94.7, 102.7, typeof( Log ), 1044041, 100, 1044351 );
			AddSkill( index, SkillName.Tinkering, 50.0, 55.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 50, 1044037 );
			index = AddCraft( typeof( WaterTroughEastDeed ), 1044299, 1044349, 94.7, 102.7, typeof( Log ), 1044041, 100, 1044351 );
			index = AddCraft( typeof( WaterTroughSouthDeed ), 1044299, 1044350, 94.7, 102.7, typeof( Log ), 1044041, 100, 1044351 );

            // Boats 
            index = AddCraft(typeof(SmallBoatDeed), "Boats", "small boat", 81.6, 105.0, typeof(Log), 1044041, 425, 1044351);
            AddSkill(index, SkillName.Lumberjacking, 75.0, 80.0);
            AddSkill(index, SkillName.Tailoring, 75.0, 75.0);
            AddRes(index, typeof(Cloth), 1044286, 100, 1044287);

            index = AddCraft(typeof(SmallDragonBoatDeed), "Boats", "small dragon boat", 83.0, 105.0, typeof(Log), 1044041, 450, 1044351);
            AddSkill(index, SkillName.Lumberjacking, 75.0, 80.0);
            AddSkill(index, SkillName.Tailoring, 75.0, 75.0);
            AddRes(index, typeof(Cloth), 1044286, 100, 1044287);

            index = AddCraft(typeof(MediumBoatDeed), "Boats", "medium boat", 84.6, 105.0, typeof(Log), 1044041, 500, 1044351);
            AddSkill(index, SkillName.Lumberjacking, 80.0, 80.0);
            AddSkill(index, SkillName.Tailoring, 80.0, 80.0);
            AddRes(index, typeof(Cloth), 1044286, 100, 1044287);

            index = AddCraft(typeof(MediumDragonBoatDeed), "Boats", "medium dragon boat", 86.4, 105.0, typeof(Log), 1044041, 525, 1044351);
            AddSkill(index, SkillName.Lumberjacking, 80.0, 80.0);
            AddSkill(index, SkillName.Tailoring, 80.0, 80.0);
            AddRes(index, typeof(Cloth), 1044286, 100, 1044287);

            index = AddCraft(typeof(LargeBoatDeed), "Boats", "large boat", 90.2, 105.0, typeof(Log), 1044041, 575, 1044351);
            AddSkill(index, SkillName.Lumberjacking, 85.0, 85.0);
            AddSkill(index, SkillName.Tailoring, 85.0, 85.0);
            AddRes(index, typeof(Cloth), 1044286, 100, 1044287);

            index = AddCraft(typeof(LargeDragonBoatDeed), "Boats", "large dragon boat", 93.5, 105.0, typeof(Log), 1044041, 600, 1044351);
            AddSkill(index, SkillName.Lumberjacking, 85.0, 85.0);
            AddSkill(index, SkillName.Tailoring, 85.0, 85.0);
            AddRes(index, typeof(Cloth), 1044286, 100, 1044287);

			MarkOption = true;
			Repair = Core.AOS;

            // Add every material you want the player to be able to choose from
            // This will override the overridable material	TODO: Verify the required skill amount

            SetSubRes(typeof(Log), "Wood");
            
            AddSubRes(typeof(Log), "Wood", 00.0, 1072652);
            AddSubRes(typeof(OakLog), "Oak", 65.0, 1072652);
            AddSubRes(typeof(MahoganyLog), "Mahogany", 70.0, 1072652);
            AddSubRes(typeof(AshLog), "Ash", 80.0, 1072652);
            AddSubRes(typeof(CedarLog), "Cedar", 85.0, 1072652);
            AddSubRes(typeof(WillowLog), "Willow", 90.0, 1072652);
            AddSubRes(typeof(YewLog), "Yew", 95.0, 1072652);
            AddSubRes(typeof(HeartwoodLog), "Heartwood", 97.5, 1072652);
            AddSubRes(typeof(MystWoodLog), "Mystwood", 99.0, 1072652);
            AddSubRes(typeof(BloodwoodLog), "Bloodwood", 100.0, 1072652);
            AddSubRes(typeof(FrostwoodLog), "Frostwood", 100.0, 1072652);
		}
	}
}