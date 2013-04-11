using System;
using Server.Items;

namespace Server.Engines.Craft
{

	public class DefWeaving : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Tailoring; }
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefWeaving();

				return m_CraftSystem;
			}
		}

		public override CraftECA ECA{ get{ return CraftECA.ChanceMinusSixtyToFourtyFive; } }

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.5; // 50%
		}

		private DefWeaving() : base( 1, 1, 1.25 )// base( 1, 1, 4.5 )
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
			from.PlaySound( 0x248 );
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

            #region Skinned Rugs
            index = AddCraft(typeof(GoatRugAddonDeed), "Skinned Rugs", "Goat Rug", 85.0, 105.0, typeof(Hides), "Hides", 35, "You don't have enough hides to make that");
            AddRes(index, typeof(Cloth), "Cloth", 10, "You do not have enough cloth to make that");
            AddSkill(index, SkillName.Forensics, 50.0, 50.0);

            index = AddCraft(typeof(BrownBearRugEastDeed), "Skinned Rugs", "Brown Bear Rug East", 99.5, 109.0, typeof(Hides), "Hides", 100, "You don't have enough hides to make that");
            AddRes(index, typeof(Cloth), "Cloth", 10, "You do not have enough cloth to make that");
            AddSkill(index, SkillName.Forensics, 100.0, 100.0);

            index = AddCraft(typeof(BrownBearRugSouthDeed), "Skinned Rugs", "Brown Bear Rug South", 99.5, 109.0, typeof(Hides), "Hides", 100, "You don't have enough hides to make that");
            AddRes(index, typeof(Cloth), "Cloth", 10, "You do not have enough cloth to make that");
            AddSkill(index, SkillName.Forensics, 100.0, 100.0);

            index = AddCraft(typeof(PolarBearRugEastDeed), "Skinned Rugs", "Polar Bear Rug East", 99.5, 113.0, typeof(Hides), "Hides", 100, "You don't have enough hides to make that");
            AddRes(index, typeof(Cloth), "Cloth", 10, "You do not have enough cloth to make that");
            AddSkill(index, SkillName.Forensics, 100.0, 100.0);

            index = AddCraft(typeof(PolarBearRugSouthDeed), "Skinned Rugs", "Polar Bear Rug South", 99.5, 113.0, typeof(Hides), "Hides", 100, "You don't have enough hides to make that");
            AddRes(index, typeof(Cloth), "Cloth", 10, "You do not have enough cloth to make that");
            AddSkill(index, SkillName.Forensics, 100.0, 100.0);

            index = AddCraft(typeof(BovineRugAddonDeed), "Skinned Rugs", "Bovine Rug", 99.5, 113.0, typeof(Hides), "Hides", 100, "You don't have enough hides to make that");
            AddRes(index, typeof(Cloth), "Cloth", 10, "You do not have enough cloth to make that");
            AddSkill(index, SkillName.Forensics, 100.0, 100.0);

            index = AddCraft(typeof(CheetahRug1AddonDeed), "Skinned Rugs", "Cheetah Rug", 99.5, 113.0, typeof(Hides), "Hides", 100, "You don't have enough hides to make that");
            AddRes(index, typeof(Cloth), "Cloth", 10, "You do not have enough cloth to make that");
            AddSkill(index, SkillName.Forensics, 100.0, 100.0);

            index = AddCraft(typeof(tigerrugAddonDeed), "Skinned Rugs", "Tiger Rug", 99.5, 113.0, typeof(Hides), "Hides", 100, "You don't have enough hides to make that");
            AddRes(index, typeof(Cloth), "Cloth", 10, "You do not have enough cloth to make that");
            AddSkill(index, SkillName.Forensics, 100.0, 100.0);

            #endregion

            #region Rugs
			index = AddCraft( typeof( BlueRugDeed ), 1076602, "Blue rug", 94.5, 107.0, typeof( Wool ), "Wool", 24, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 6, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 10, 1044253 );

            index = AddCraft(typeof(BlueMediumRugDeed), 1076602, "Blue medium rug", 94.5, 107.5, typeof(Wool), "Wool", 24, "You don't have enough wool to make that");
            AddRes(index, typeof(Dyes), 1024009, 7, 1044253);
            AddRes(index, typeof(Flax), 1026809, 12, 1044253);

            index = AddCraft(typeof(BlueLargeRugDeed), 1076602, "Blue large rug", 94.5, 108.0, typeof(Wool), "Wool", 24, "You don't have enough wool to make that");
            AddRes(index, typeof(Dyes), 1024009, 8, 1044253);
            AddRes(index, typeof(Flax), 1026809, 15, 1044253);
			
			index = AddCraft( typeof( BluePatternRugDeed ), 1076602, "Blue pattern rug", 96.0, 109.0, typeof( Wool ), "Wool", 25, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 7, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 10, 1044253 );

            index = AddCraft(typeof(BluePatternMediumRugDeed), 1076602, "Blue pattern medium rug", 96.0, 109.5, typeof(Wool), "Wool", 25, "You don't have enough wool to make that");
            AddRes(index, typeof(Dyes), 1024009, 8, 1044253);
            AddRes(index, typeof(Flax), 1026809, 10, 1044253);

            index = AddCraft(typeof(BluePatternLargeRugDeed), 1076602, "Blue pattern large rug", 96.0, 110.0, typeof(Wool), "Wool", 25, "You don't have enough wool to make that");
            AddRes(index, typeof(Dyes), 1024009, 9, 1044253);
            AddRes(index, typeof(Flax), 1026809, 15, 1044253);

			index = AddCraft( typeof( BlueFancyRugDeed ), 1076602, 1076273, 97.5, 110.0, typeof( Wool ), "Wool", 26, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 8, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 12, 1044253 );
			
			index = AddCraft( typeof( BlueDecorativeRugDeed ), 1076602, 1076589, 99.0, 111.5, typeof( Wool ), "Wool", 28, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 8, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 12, 1044253 );
			
			index = AddCraft( typeof( RedRugDeed ), 1076602, "Red rug", 94.5, 107.0, typeof( Wool ), "Wool", 24, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 6, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 10, 1044253 );

            index = AddCraft(typeof(RedMediumRugDeed), 1076602, "Red medium rug", 94.5, 107.5, typeof(Wool), "Wool", 24, "You don't have enough wool to make that");
            AddRes(index, typeof(Dyes), 1024009, 7, 1044253);
            AddRes(index, typeof(Flax), 1026809, 12, 1044253);

            index = AddCraft(typeof(RedLargeRugDeed), 1076602, "Red large rug", 94.5, 108.0, typeof(Wool), "Wool", 24, "You don't have enough wool to make that");
            AddRes(index, typeof(Dyes), 1024009, 8, 1044253);
            AddRes(index, typeof(Flax), 1026809, 15, 1044253);
			
			index = AddCraft( typeof( RedPatternRugDeed ), 1076602, "Red pattern rug", 96.0, 109.0, typeof( Wool ), "Wool", 25, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 7, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 10, 1044253 );

            index = AddCraft(typeof(BluePatternMediumRugDeed), 1076602, "Blue pattern medium rug", 96.0, 109.5, typeof(Wool), "Wool", 25, "You don't have enough wool to make that");
            AddRes(index, typeof(Dyes), 1024009, 8, 1044253);
            AddRes(index, typeof(Flax), 1026809, 10, 1044253);

            index = AddCraft(typeof(BluePatternLargeRugDeed), 1076602, "Blue pattern large rug", 96.0, 110.0, typeof(Wool), "Wool", 25, "You don't have enough wool to make that");
            AddRes(index, typeof(Dyes), 1024009, 9, 1044253);
            AddRes(index, typeof(Flax), 1026809, 15, 1044253);
			
			index = AddCraft( typeof( CinnamonRugDeed ), 1076602, "Large cinnamon rug", 97.5, 110.0, typeof( Wool ), "Wool", 30, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 6, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 12, 1044253 );
			
			index = AddCraft( typeof( CinnamonFancyRugDeed ), 1076602, 1076587, 98.0, 110.0, typeof( Wool ), "Wool", 28, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 6, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 13, 1044253 );
			
			index = AddCraft( typeof( CinnamonFancyRug2Deed ), 1076602, "Cinnamon decorative rug", 99.0, 110.0, typeof( Wool ), "Wool", 28, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 8, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 13, 1044253 );
			
			index = AddCraft( typeof( CinnamonFancyRug3Deed ), 1076602, "Cinnamon artisan rug", 99.0, 110.5, typeof( Wool ), "Wool", 26, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 9, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 14, 1044253 );

            index = AddCraft(typeof(FullRoomCinnamonRugDeed), 1076602, "Full room cinnamon rug (5x5)", 99.5, 110.5, typeof(Wool), "Wool", 40, "You don't have enough wool to make that");
            AddRes(index, typeof(Dyes), 1024009, 11, 1044253);
            AddRes(index, typeof(Flax), 1026809, 20, 1044253);
			
			index = AddCraft( typeof( GoldenRugDeed ), 1076602, "Large golden rug", 96.0, 110.0, typeof( Wool ), "Wool", 30, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 5, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 12, 1044253 );
			
			index = AddCraft( typeof( GoldenDecorativeRugDeed ), 1076602, 1076586, 99.5, 114.0, typeof( Wool ), "Wool", 36, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 10, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 15, 1044253 ); 

			#endregion

            #region Misc.
            index = AddCraft( typeof( BlueRunnerNSDeed ), 3001016, "Blue runner N/S", 96.0, 108.0, typeof( Wool ), "Wool", 12, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 4, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 5, 1044253 );
			
			index = AddCraft( typeof( BlueRunnerEWDeed ), 3001016, "Blue runner E/W", 96.0, 108.0, typeof( Wool ), "Wool", 12, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 4, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 5, 1044253 );
			
			index = AddCraft( typeof( RedRunnerNSDeed ), 3001016, "Red runner N/S", 96.0, 108.0, typeof( Wool ), "Wool", 12, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 4, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 5, 1044253 );
			
			index = AddCraft( typeof( RedRunnerEWDeed ), 3001016, "Red runner E/W", 96.0, 108.0, typeof( Wool ), "Wool", 12, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 4, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 5, 1044253 );
			
			index = AddCraft( typeof( GoldenRunnerNSDeed ), 3001016, "Golden runner N/S", 97.0, 108.0, typeof( Wool ), "Wool", 12, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 4, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 5, 1044253 );
			
			index = AddCraft( typeof( GoldenRunnerEWDeed ), 3001016, "Golden runner E/W", 97.0, 108.0, typeof( Wool ), "Wool", 12, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 4, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 5, 1044253 );
			
			index = AddCraft( typeof( CinnamonRunnerNSDeed ), 3001016, "Cinnamon runner N/S", 97.0, 108.0, typeof( Wool ), "Wool", 12, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 4, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 5, 1044253 );
			
			index = AddCraft( typeof( CinnamonRunnerEWDeed ), 3001016, "Cinnamon runner E/W", 97.0, 108.0, typeof( Wool ), "Wool", 12, "You don't have enough wool to make that" );
			AddRes( index, typeof( Dyes ), 1024009, 4, 1044253 );
			AddRes( index, typeof( Flax ), 1026809, 5, 1044253 );

            index = AddCraft(typeof(MediumCinnamonRunnerNSDeed), 3001016, "Medium Cinnamon runner N/S", 97.5, 108.0, typeof(Wool), "Wool", 12, "You don't have enough wool to make that");
            AddRes(index, typeof(Dyes), 1024009, 4, 1044253);
            AddRes(index, typeof(Flax), 1026809, 5, 1044253);

            index = AddCraft(typeof(MediumCinnamonRunnerEWDeed), 3001016, "Medium Cinnamon runner E/W", 97.5, 108.0, typeof(Wool), "Wool", 12, "You don't have enough wool to make that");
            AddRes(index, typeof(Dyes), 1024009, 4, 1044253);
            AddRes(index, typeof(Flax), 1026809, 5, 1044253);
	
			#endregion

            
			
			MarkOption = true;
			
		}
	}
}
