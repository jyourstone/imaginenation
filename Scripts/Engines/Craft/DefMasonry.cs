using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Craft 
{ 
	public class DefMasonry : CraftSystem 
	{ 
		public override SkillName MainSkill 
		{ 
			get{ return SkillName.Carpentry; } 
		} 

		public override int GumpTitleNumber 
		{ 
			get{ return 1044500; } // <CENTER>MASONRY MENU</CENTER> 
		} 

		private static CraftSystem m_CraftSystem; 

		public static CraftSystem CraftSystem 
		{ 
			get 
			{ 
				if ( m_CraftSystem == null ) 
					m_CraftSystem = new DefMasonry(); 

				return m_CraftSystem; 
			} 
		} 

		public override double GetChanceAtMin( CraftItem item ) 
		{ 
			return 0.0; // 0% 
		} 

		private DefMasonry() : base( 2, 5, 1.0 )// base( 1, 2, 1.7 ) 
		{ 
		} 

		public override bool RetainsColorFrom( CraftItem item, Type type )
		{
			return true;
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if( tool == null || tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckTool( tool, from ) )
				return 1048146; // If you have a tool equipped, you must use that tool.
			else if ( !(from is PlayerMobile && ((PlayerMobile)from).Masonry && from.Skills[SkillName.Carpentry].Base >= 100.0) )
				return 1044633; // You havent learned stonecraft.
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.

			return 0;
		} 

		public override void PlayCraftEffect( Mobile from ) 
		{ 
			// no effects
			//if ( from.Body.Type == BodyType.Human && !from.Mounted ) 
			//	from.Animate( 9, 5, 1, true, false, 0 ); 
			//new InternalTimer( from ).Start(); 
		} 

		// Delay to synchronize the sound with the hit on the anvil 
		private class InternalTimer : Timer 
		{ 
			private readonly Mobile m_From; 

			public InternalTimer( Mobile from ) : base( TimeSpan.FromSeconds( 0.7 ) ) 
			{ 
				m_From = from; 
			} 

			protected override void OnTick() 
			{ 
				m_From.PlaySound( 0x23D ); 
			} 
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
				else if ( makersMark && quality == 2 ) 
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
			// Decorations
			AddCraft( typeof( Vase ), 1044501, 1022888, 52.5, 102.5, typeof( Granite ), 1044514, 1, 1044513 );
			AddCraft( typeof( LargeVase ), 1044501, 1022887, 52.5, 102.5, typeof( Granite ), 1044514, 3, 1044513 );
			
			/*if( Core.SE )
			{
				int index = AddCraft( typeof( SmallUrn ), 1044501, 1029244, 82.0, 132.0, typeof( Granite ), 1044514, 3, 1044513 );
				SetNeededExpansion( index, Expansion.SE );

				index = AddCraft( typeof( SmallTowerSculpture ), 1044501, 1029242, 82.0, 132.0, typeof( Granite ), 1044514, 3, 1044513 );
				SetNeededExpansion( index, Expansion.SE );
			}*/

			// Furniture
			AddCraft( typeof( StoneChair ), 1044502, 1024635, 55.0, 105.0, typeof( Granite ), 1044514, 4, 1044513 );
            AddCraft( typeof( StoneBench ), 1044502, "stone bench", 60.0, 110.0, typeof(Granite), 1044514, 3, 1044513);
            AddCraft( typeof( StoneBenchSingle ), 1044502, "stone bench single", 60.0, 110.0, typeof(Granite), 1044514, 4, 1044513);
            AddCraft( typeof( StoneTable ), 1044502, "stone table", 65.0, 115.0, typeof(Granite), 1044514, 5, 1044513);
		    AddCraft( typeof (EmptyStoneBookcase), 1044502, "stone bookcase", 85.0, 105.0, typeof (Granite), 1044514, 12, 1044513);

            index = AddCraft(typeof (FullStoneBookcase), 1044503, "full stone bookcase", 90.0, 105.0, typeof (Granite), 1044514, 2, 1044513);
            AddSkill(index, SkillName.Inscribe, 55.0, 60.0);
            AddRes(index, typeof(EmptyStoneBookcase), "stone bookcase", 1, "You need a stone bookcase to make that.");
            AddRes(index, typeof(BlueBook), "blue book", 6, "You do not have sufficient books to make that.");
            AddRes(index, typeof(RedBook), "red book", 6, "You do not have sufficient books to make that.");
            AddRes(index, typeof(BrownBook), "brown book", 6, "You do not have sufficient books to make that.");
            AddRes(index, typeof(TanBook), "tan book", 6, "You do not have sufficient books to make that.");

            AddCraft(typeof(StoneVanityAddonDeed), 1044503, "stone vanity south", 95.0, 105.0, typeof(Granite), 1044514, 15, 1044513);
            AddCraft(typeof(StoneVanityEastAddonDeed), 1044503, "stone vanity east", 95.0, 105.0, typeof(Granite), 1044514, 15, 1044513);

			// Statues
			AddCraft( typeof( StatueSouth ), 1044503, 1044505, 60.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatueNorth ), 1044503, 1044506, 60.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatueEast ), 1044503, 1044507, 60.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuePegasus ), 1044503, 1044510, 70.0, 130.0, typeof( Granite ), 1044514, 4, 1044513 );

            // Marble furniture
            AddCraft(typeof(MarbleBench), "Marble Furniture", "marble bench", 80.0, 120.0, typeof(Granite), 1044514, 3, 1044513);
            AddCraft(typeof(MarbleBenchSingle), "Marble Furniture", "marble bench single", 80.0, 120.0, typeof(Granite), 1044514, 4, 1044513);
            AddCraft(typeof(MarbleTable), "Marble Furniture", "marble table", 90.0, 120.0, typeof(Granite), 1044514, 5, 1044513);

			SetSubRes( typeof( Granite ), 1044525 );

			AddSubRes( typeof( Granite ),			"Granite", 00.0, 1044514 );
			AddSubRes( typeof( OldCopperGranite ),	"Old Copper", 65.0, 1044514 );
			AddSubRes( typeof( ShadowIronGranite ),	"Shadow Iron", 70.0, 1044514 );
			AddSubRes( typeof( SilverGranite ),		"Silver", 75.0, 1044514 );
			AddSubRes( typeof( VeriteGranite ),		"Verite", 80.0, 1044514 );
			AddSubRes( typeof( RoseGranite ),		"Rose", 85.0, 1044514 );
			AddSubRes( typeof( GoldGranite ),	    "Gold", 90.0, 1044514 );
            AddSubRes(typeof(IceGranite),           "Ice", 95.0, 1044514);
            AddSubRes(typeof(AmethystGranite),      "Amethyst", 97.5, 1044514);
			AddSubRes( typeof( ValoriteGranite ),	"Valorite", 99.0, 1044514 );
            AddSubRes(typeof(BloodRockGranite), "BloodRock", 99.2, 1044514);
            AddSubRes(typeof(AquaGranite), "Aqua", 99.4, 1044514);
            AddSubRes(typeof(MytherilGranite), "Mytheril", 99.6, 1044514);
            AddSubRes(typeof(DwarvenGranite), "Dwarven", 99.8, 1044514);
		}
	}
}