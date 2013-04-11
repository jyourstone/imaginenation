using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Craft
{
	public class DefGlassblowing : CraftSystem
	{
		public override SkillName MainSkill
		{
			get{ return SkillName.Alchemy; }
		}

		public override int GumpTitleNumber
		{
			get{ return 1044622; } // <CENTER>Glassblowing MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefGlassblowing();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin( CraftItem item )
		{
            if (item.ItemType == typeof(HollowPrism))
                return 0.5; // 50%

			return 0.0; // 0%
		}

		private DefGlassblowing() : base( 1, 1, 1.25 )// base( 1, 2, 1.7 )
		{
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if( tool == null || tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckTool( tool, from ) )
				return 1048146; // If you have a tool equipped, you must use that tool.
			else if ( !(from is PlayerMobile && ((PlayerMobile)from).Glassblowing && from.Skills[SkillName.Alchemy].Base >= 100.0) )
				return 1044634; // You havent learned glassblowing.
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.

			bool anvil, forge;

			DefBlacksmithy.CheckAnvilAndForge( from, 2, out anvil, out forge );

			if ( forge )
				return 0;

			return 1044628; // You must be near a forge to blow glass.
		}

		public override void PlayCraftEffect( Mobile from )
		{
			from.PlaySound( 0x2B ); // bellows

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
				m_From.PlaySound( 0x2A );
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
				from.PlaySound( 0x41 ); // glass breaking

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
            {
                #region Miscellaneous

                index = AddCraft(typeof(Bottle), 1044050, 1023854, 52.5, 102.5, typeof(Sand), 1044625, 1, 1044627);
                SetUseAllRes(index, true);

                AddCraft(typeof(BlueBeaker), 1044050, "small blue flask", 52.5, 102.5, typeof(Sand), 1044625, 2, 1044627);
                AddCraft(typeof(GreenBeaker), 1044050, "small green flask", 52.5, 102.5, typeof(Sand), 1044625, 2, 1044627);
                AddCraft(typeof(RedBeaker), 1044050, "small red flask", 52.5, 102.5, typeof(Sand), 1044625, 2, 1044627);
                AddCraft(typeof(YellowBeaker), 1044050, "small yellow flask", 52.5, 102.5, typeof(Sand), 1044625, 2, 1044627);
                AddCraft(typeof(MediumFlask), 1044050, "medium flask", 52.5, 102.5, typeof(Sand), 1044625, 3, 1044627);
                AddCraft(typeof(CurvedFlask), 1044050, "curved flask", 55.0, 105.0, typeof(Sand), 1044625, 2, 1044627);
                AddCraft(typeof(RedCurvedFlask), 1044050, "red curved flask", 55.0, 105.0, typeof(Sand), 1044625, 2, 1044627);
                AddCraft(typeof(LtBlueCurvedFlask), 1044050, "teal curved flask", 55.0, 105.0, typeof(Sand), 1044625, 2, 1044627);
                AddCraft(typeof(BlueCurvedFlask), 1044050, "blue curved flask", 55.0, 105.0, typeof(Sand), 1044625, 2, 1044627);
                AddCraft(typeof(GreenCurvedFlask), 1044050, "green curved flask", 55.0, 105.0, typeof(Sand), 1044625, 2, 1044627);
                AddCraft(typeof(EmptyRibbedFlask), 1044050, "ribbed flask", 57.5, 107.5, typeof(Sand), 1044625, 4, 1044627);
                AddCraft(typeof(RedRibbedFlask), 1044050, "red ribbed flask", 57.5, 107.5, typeof(Sand), 1044625, 4, 1044627);
                AddCraft(typeof(VioletRibbedFlask), 1044050, "violet ribbed flask", 57.5, 107.5, typeof(Sand), 1044625, 4, 1044627);
                AddCraft(typeof(LargeYellowFlask), 1044050, "large yellow flask", 60.0, 110.0, typeof(Sand), 1044625, 5, 1044627);
                AddCraft(typeof(LargeEmptyFlask), 1044050, "large flask", 60.0, 110.0, typeof(Sand), 1044625, 5, 1044627);
                AddCraft(typeof(LargeVioletFlask), 1044050, "large violet flask", 60.0, 110.0, typeof(Sand), 1044625, 5, 1044627);
                AddCraft(typeof(AniSmallBlueFlask), 1044050, "blue bubbling flask", 60.0, 110.0, typeof(Sand), 1044625, 5, 1044627);
                AddCraft(typeof(AniLargeVioletFlask), 1044050, "purple bubbling flask", 60.0, 110.0, typeof(Sand), 1044625, 5, 1044627);
                AddCraft(typeof(AniRedRibbedFlask), 1044050, "red bubbling flask", 60.0, 110.0, typeof(Sand), 1044625, 7, 1044627);

                AddCraft(typeof(EmptyVialsWRack), 1044050, "empty vials", 65.0, 115.0, typeof(Sand), 1044625, 8, 1044627);
                AddCraft(typeof(FullVialsWRack), 1044050, "full vials", 65.0, 115.0, typeof(Sand), 1044625, 9, 1044627);
                AddCraft(typeof(Hourglass), 1044050, "hourglass", 75.0, 120.0, typeof(Sand), 1044625, 10, 1044627);
                AddCraft(typeof(HourglassAni), 1044050, "spinning hourglass", 75.0, 125.0, typeof(Sand), 1044625, 10, 1044627);
                AddCraft(typeof(GlassSkull), 1044050, "glass skull", 90.0, 110.8, typeof(Sand), 1044625, 40, 1044627);
                index = AddCraft(typeof(Mirror), 1044050, "mirror", 90.0, 111.5, typeof(Sand), 1044625, 50, 1044627);
                AddRes(index, typeof(Clay), "Clay", 20, 1044253);
                AddRes(index, typeof(Granite), "Granite", 5, 1044253);

                if (Core.ML)
                {
                    index = AddCraft(typeof(HollowPrism), 1044050, 1072895, 100.0, 150.0, typeof(Sand), 1044625, 8, 1044627);
                    SetNeededExpansion(index, Expansion.ML);
                }

                #endregion

                #region Decorative Bottles

                AddCraft(typeof(VioletStemmedBottle), "Decorative Bottles", "violet stemmed bottle", 75.0, 120.0, typeof(Sand), 1044625, 10, 1044627);
                AddCraft(typeof(GlassBottle), "Decorative Bottles", "green bottle", 75.0, 120.0, typeof(Sand), 1044625, 10, 1044627);
                AddCraft(typeof(GreenBottle), "Decorative Bottles", "teal bottle", 75.0, 120.0, typeof(Sand), 1044625, 10, 1044627);
                AddCraft(typeof(RedBottle), "Decorative Bottles", "red bottle", 75.0, 120.0, typeof(Sand), 1044625, 10, 1044627);
                AddCraft(typeof(SmallBlueBottle), "Decorative Bottles", "blue bottle", 75.0, 120.0, typeof(Sand), 1044625, 10, 1044627);
                AddCraft(typeof(SmallBrownBottle), "Decorative Bottles", "brown bottle", 75.0, 120.0, typeof(Sand), 1044625, 10, 1044627);
                AddCraft(typeof(SmallGreenBottle), "Decorative Bottles", "round teal bottle", 75.0, 120.0, typeof(Sand), 1044625, 10, 1044627);
                AddCraft(typeof(SmallGreenBottle2), "Decorative Bottles", "round green bottle", 75.0, 120.0, typeof(Sand), 1044625, 10, 1044627);
                AddCraft(typeof(SmallVioletBottle), "Decorative Bottles", "violet bottle", 75.0, 120.0, typeof(Sand), 1044625, 10, 1044627);

                #endregion
            }
        }
	}
}