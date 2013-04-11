/* Created by Hammerhand */

using System;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Engines.Harvest
{
	public class ClayMining : HarvestSystem
    {
			private static ClayMining m_System;

		public static ClayMining System
		{
			get
			{
				if ( m_System == null )
					m_System = new ClayMining();

				return m_System;
			}
		}

		private readonly HarvestDefinition m_Clay;

        public HarvestDefinition Clay
		{
            get { return m_Clay; }
		}

        private ClayMining()
		{
			HarvestResource[] res;
			HarvestVein[] veins;

            #region Mining for Clay
            HarvestDefinition Clay = m_Clay = new HarvestDefinition();

            // Resource banks are every 8x8 tiles
            Clay.BankWidth = 8;
            Clay.BankHeight = 8;

            // Every bank holds from 6 to 12 Clay
            Clay.MinTotal = 5;
            Clay.MaxTotal = 8;

            // A resource bank will respawn its content every 10 to 20 minutes
            Clay.MinRespawn = TimeSpan.FromMinutes(10.0);
            Clay.MaxRespawn = TimeSpan.FromMinutes(20.0);

            // Skill checking is done on the Mining skill
            Clay.Skill = SkillName.Mining;

            // Set the list of harvestable tiles
            Clay.Tiles = m_SwampTiles;

            // Players must be within 2 tiles to harvest
            Clay.MaxRange = 2;

            // One Clay per harvest action
            Clay.ConsumedPerHarvest = 1;
            Clay.ConsumedPerFeluccaHarvest = 1;

            // The digging effect
            Clay.EffectActions = new int[] { 11 };
            Clay.EffectSounds = new int[] { 0x125, 0x126 };
            Clay.EffectCounts = new int[] { 1 };
            Clay.EffectDelay = TimeSpan.FromSeconds(1.6);
            Clay.EffectSoundDelay = TimeSpan.FromSeconds(0.9);

            Clay.NoResourcesMessage = "There is no Clay here to mine."; // There is no Clay here to mine.
            Clay.DoubleHarvestMessage = "There is no Clay here to mine."; // There is no Clay here to mine.
            Clay.TimedOutOfRangeMessage = "You have moved too far away to continue mining."; // You have moved too far away to continue mining.
            Clay.OutOfRangeMessage = "That is too far away."; // That is too far away.
            Clay.FailMessage = "You dig for a while but fail to find any Clay."; // You dig for a while but fail to find any Clay.
            Clay.PackFullMessage = "Your backpack can't hold the Clay, and it is lost!"; // Your backpack can't hold the Clay, and it is lost!
            Clay.ToolBrokeMessage = 1044038; // You have worn out your tool!

            res = new HarvestResource[]
				{
                    new HarvestResource( 100.0, 85.0, 400.0, "You dig some Clay and put it in your backpack", typeof (Clay)),
				
				};

            veins = new HarvestVein[]
				{
                    new HarvestVein( 100.0, 0.0, res[0], null   ),//Clay
				};

            Clay.Resources = res;
            Clay.Veins = veins;

            Definitions.Add(Clay);
            #endregion
		}

        public override bool CheckHarvest(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            if (!base.CheckHarvest(from, tool, def, toHarvest))
                return false;

            if (def == m_Clay && !(from is PlayerMobile && from.Skills[SkillName.Mining].Base >= 100.0 && ((PlayerMobile)from).ClayMining))
            {
                OnBadHarvestTarget(from, tool, toHarvest);
                return false;
            }
            /*
            else if (from.Mounted)
            {
                from.SendLocalizedMessage(501864); // You can't mine while riding.
                return false;
            }
            else if (from.IsBodyMod && !from.Body.IsHuman)
            {
                from.SendLocalizedMessage(501865); // You can't mine while polymorphed.
                return false;
            }
            */

            return true;
        }

                public override void OnHarvestFinished(Mobile from, Item tool, HarvestDefinition def, HarvestVein vein, HarvestBank bank, HarvestResource resource, object harvested)
                {
                    HarvestResource res = vein.PrimaryResource;

                    if (res == resource && res.Types.Length >= 3)
                    {
                        try
                        {
                            Map map = from.Map;

                            if (map == null)
                                return;
                        }
                        catch
                        {
                        }
                    }
                }
        public override bool BeginHarvesting(Mobile from, Item tool)
        {
            if (!base.BeginHarvesting(from, tool))
                return false;

            from.SendLocalizedMessage(503033); // Where do you wish to dig?
            return true;
        }
        public override void OnHarvestStarted(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            base.OnHarvestStarted(from, tool, def, toHarvest);
        }
		public override void OnBadHarvestTarget( Mobile from, Item tool, object toHarvest )
        {
            if (toHarvest is LandTarget)
                from.SendLocalizedMessage(501862); // You can't mine there.
            else
                from.SendLocalizedMessage(501863); // You can't mine that.
        }

		public static void Initialize()
		{
			Array.Sort( m_SwampTiles );
		}

        #region Tile lists
        private static readonly int[] m_SwampTiles = new[]
			{
             15717, 15808, 15809, 15810, 15811, 15812,
             15813, 15814, 15815, 15816, 15817, 15818,
             15819, 15820, 15821, 15822, 15823, 15824,
             15825, 15826, 15827, 15828, 15829, 15830,
             15831, 15832, 15833, 15835, 15836, 15838,
             15839, 15840, 15831, 15842, 15843, 15844,
             15845, 15846, 15847, 15848, 15849, 15850,
             15851, 15852, 15853, 15854, 15855, 15856,
             15857, 16112
			};
        #endregion
	}
}