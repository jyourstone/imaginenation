using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Craft
{
	public class DefAquarium : CraftSystem
	{
		public override SkillName MainSkill
		{
			get{ return SkillName.Tinkering; }
		}

		public override string GumpTitleString
		{
			get { return "<basefont color=#FFFFFF><CENTER>AQUARIUM MENU</CENTER></basefont>"; } 
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefAquarium();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.0; // 0%
		}

		private DefAquarium() : base( 1, 1, 1.25 )// base( 1, 2, 1.7 )
		{
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if ( tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.

			return 0;
		}

		public override void PlayCraftEffect( Mobile from )
		{
			from.PlaySound( 0x1C6 ); 
		}

		private class InternalTimer : Timer
		{
			private Mobile m_From;

			public InternalTimer( Mobile from ) : base( TimeSpan.FromSeconds( 0.7 ) )
			{
				m_From = from;
			}

			protected override void OnTick()
			{
				m_From.PlaySound( 0x1C6 );
			}
		}

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item )
		{
			if ( toolBroken )
				from.SendLocalizedMessage( 1044038 );

			if ( failed )
			{
				if ( lostMaterial )
					return 1044043;
				else
					return 1044157;
			}
			else
			{
				from.PlaySound( 0x1c6 );

				if ( quality == 0 )
					return 502785;
				else if ( makersMark && quality == 2 )
					return 1044156;
				else if ( quality == 2 )
					return 1044155;
				else
					return 1044154;
			}
		}

		public override void InitCraftList()
		{

			

			int index = AddCraft( typeof( SeaGrassTankAddonDeed ), "Deco-Tanks", "Sea Grass", 95.0, 102.0, typeof( Sand ), 1044625, 25, 1044627 );
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( SeaGrass ), "Sea Grass", 1, "You need sea grass to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(PearlTankAddonDeed), "Deco-Tanks", "Pearl", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( Pearl ), "Pearl", 1, "You need a pearl to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(CoralTankAddonDeed), "Deco-Tanks", "Coral", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( Coral ), "Coral", 1, "You need a piece of coral to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(BonesTankAddonDeed), "Deco-Tanks", "Dead Fish", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( FishBones ), "Fish Bones", 1, 1049063 );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

			index = AddCraft( typeof( EmptyTankAddonDeed ), "Deco-Tanks", "Empty Tank", 95.0, 101.0, typeof( Sand ), 1044625, 25, 1044627 );
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(SeveredAnchorTankAddonDeed), "Deco-Tanks", "Severed Anchor", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( SeveredAnchor ), "Severed Anchor", 1, "You need a severed anchor to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(SunkenShipTankAddonDeed), "Deco-Tanks", "Sunken Ship", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( SunkenShip ), "Sunken Ship", 1, "You need an H.M.s Destiny to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(TreasureChestTankAddonDeed), "Deco-Tanks", "Treasure Chest", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( MetalBox ), "Metal Box", 1, "You need a metal box to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(AlbinoAngelfishTankAddonDeed), "Fish-Tanks", "Albino Angel", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( AlbinoAngelfish ), "Albino Angelfish", 1, "You need an albino angelfish to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(CopperbandedButterflyfishTankAddonDeed), "Fish-Tanks", "Copperbanded Butterflyfish", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( CopperbandedButterflyfish ), "Copperbanded butterflyfish", 1, "You need a copperbanded butterflyfish to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(PowderblueTangTankAddonDeed), "Fish-Tanks", "Powder Blue Tang", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( PowderblueTang ), "Powder blue Tang", 1, "You need a powder blue tang to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(BrineShrimpTankAddonDeed), "Fish-Tanks", "Brine Shrimp", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( BrineShrimp ), "Brine Shrimp", 1, "You need a brine shrimp to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(ButterflyfishTankAddonDeed), "Fish-Tanks", "Butterflyfish", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( Butterflyfish ), "Butterflyfish", 1, "You need a Butterflyfish to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(AngelfishTankAddonDeed), "Fish-Tanks", "Angelfish", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( Angelfish ), "Angelfish", 1, "You need an angelfish to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(AnteniAngelfishTankAddonDeed), "Fish-Tanks", "Anteni Angelfish", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( AnteniAngelfish ), "Anteni Angelfish", 1, "You need an anteni angelfish to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(SmallFishiesTankAddonDeed), "Fish-Tanks", "Small Fishies", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( SmallFishies ), "Small Fishies", 1, "You need small fishies to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(FantailGoldfishTankAddonDeed), "Fish-Tanks", "Fantail Goldfish", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( FantailGoldfish ), "Fantail Goldfish", 1, "You need a fantail goldfish to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(JellyfishTankAddonDeed), "Fish-Tanks", "Jelly Fish", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( Jellyfish ), "Jellyfish", 1, "You need a Jellyfish to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(BlueFishTankAddonDeed), "Fish-Tanks", "Blue Fish", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( BlueFish ), "Blue Fish", 1, "You need a blue fish to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(YellowTangTankAddonDeed), "Fish-Tanks", "Yellow Tang", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( YellowTang ), "Yellow Tang", 1, "You need a Yellow Tang to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(QueenAngelfishTankAddonDeed), "Fish-Tanks", "Queen Angelfish", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( QueenAngelfish ), "Queen Angelfish", 1, "You need a Queen Angelfish to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(ShrimpTankAddonDeed), "Fish-Tanks", "Shrimp", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( Shrimp ), "Shrimp", 1, "You need a Shrimp to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(AnthiasTankAddonDeed), "Fish-Tanks", "Anthias", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( Anthias ), "Anthias", 1, "You need an Anthias to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(CrabTankAddonDeed), "Fish-Tanks", "Crab", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( Crab ), "Crab", 1, "You need a Crab to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(BambooSharkTankAddonDeed), "Fish-Tanks", "Bamboo Shark", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( BambooShark ), "Bamboo Shark", 1, "You need a Bamboo Shark to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(PufferfishTankAddonDeed), "Fish-Tanks", "Pufferfish", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( Pufferfish ), "Pufferfish", 1, "You need a pufferfish to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(BaronessButterflyfishTankAddonDeed), "Fish-Tanks", "Baroness Butterflyfish", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( BaronessButterflyfish ), "Baroness Butterflyfish", 1, "You need a Baroness Butterflyfish to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

            index = AddCraft(typeof(FlameAngelfishTankAddonDeed), "Fish-Tanks", "Flame Angelfish", 95.0, 102.0, typeof(Sand), 1044625, 25, 1044627);
			AddRes( index, typeof( BaseBeverage ), 1046458, 5, 1044253 );
			AddRes( index, typeof( FlameAngelfish ), "FlameAngelfish", 1, "You need a FlameAngelfish to make that." );
            AddSkill(index, SkillName.Fishing, 95.0, 95.0);
            AddSkill(index, SkillName.Carpentry, 80.0, 80.0);

		}
	}
}