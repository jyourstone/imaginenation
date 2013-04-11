using System;
using Server.Items;

namespace Server.Engines.Craft
{
    public class ClayCraft : CraftSystem
    {
        public override SkillName MainSkill
        {
            get { return SkillName.Tinkering; }
        }

        private static CraftSystem m_CraftSystem;

        public static CraftSystem CraftSystem
        {
            get
            {
                if (m_CraftSystem == null)
                    m_CraftSystem = new ClayCraft();

                return m_CraftSystem;
            }
        }

        public override double GetChanceAtMin(CraftItem item)
        {
            return 0.5; // 50%
        }

        private ClayCraft()
            : base(1, 1, 1.25)// base( 1, 1, 3.0 )
        {
        }

        public override int CanCraft(Mobile from, BaseTool tool, Type itemType)
        {
            if (tool.Deleted || tool.UsesRemaining < 0)
                return 1044038; // You have worn out your tool!
            else if (!BaseTool.CheckAccessible(tool, from))
                return 1044263; // The tool must be on your person to use.

            return 0;
        }

        public override void PlayCraftEffect(Mobile from)
        {
            // no animation
            //if ( from.Body.Type == BodyType.Human && !from.Mounted )
            //	from.Animate( 9, 5, 1, true, false, 0 );

            from.PlaySound(0x23D);
        }

        public override int PlayEndingEffect(Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item)
        {
            if (toolBroken)
                from.SendLocalizedMessage(1044038); // You have worn out your tool

            if (failed)
            {
                if (lostMaterial)
                    return 1044043; // You failed to create the item, and some of your materials are lost.
                else
                    return 1044157; // You failed to create the item, but no materials were lost.
            }
            else
            {
                if (quality == 0)
                    return 502785; // You were barely able to make this item.  It's quality is below average.
                else if (makersMark && quality == 2)
                    return 1044156; // You create an exceptional quality item and affix your maker's mark.
                else if (quality == 2)
                    return 1044155; // You create an exceptional quality item.
                else
                    return 1044154; // You create the item.
            }
        }

        public override void InitCraftList() //tinkering
        {
            int index = -1;
            {
                #region Animal Statues

                index = AddCraft(typeof(BillyGoatStatue), "Animal Statues", "Billy Goat", 70.0, 80.0, typeof(Clay), "Clay", 18, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(BlackWidowSpiderStatue), "Animal Statues", "Black Widow Spider", 72.5, 85.0, typeof(Clay), "Clay", 25, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(CougarStatue), "Animal Statues", "Cougar", 72.5, 80.0, typeof(Clay), "Clay", 21, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(DireWolfStatue), "Animal Statues", "Dire Wolf", 72.5, 85.0, typeof(Clay), "Clay", 24, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(DogHoundStatue), "Animal Statues", "Dog Hound", 72.5, 80.0, typeof(Clay), "Clay", 21, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(DreadSpiderStatue), "Animal Statues", "Dread Spider", 75.0, 90.0, typeof(Clay), "Clay", 27, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(FerretStatue), "Animal Statues", "Ferret", 95.5, 105.0, typeof(Clay), "Clay", 45, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(FrostSpiderStatue), "Animal Statues", "Frost Spider", 73.5, 82.0, typeof(Clay), "Clay", 25, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(GiantFrogStatue), "Animal Statues", "Giant Frog", 85.0, 100.0, typeof(Clay), "Clay", 30, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(GiantIceSnakeStatue), "Animal Statues", "Ice Snake", 80.0, 95.0, typeof(Clay), "Clay", 23, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(GiantLavaSnakeStatue), "Animal Statues", "Lava Snake", 80.0, 95.0, typeof(Clay), "Clay", 23, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(GiantScorpionStatue), "Animal Statues", "Giant Scorpion", 81.0, 96.0, typeof(Clay), "Clay", 24, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(GiantSilverSnakeStatue), "Animal Statues", "Silver Snake", 80.5, 95.5, typeof(Clay), "Clay", 23, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(GiantSnakeStatue), "Animal Statues", "Giant Snake", 79.0, 95.0, typeof(Clay), "Clay", 21, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(GiantSpiderStatue), "Animal Statues", "Giant Spider", 73.0, 82.0, typeof(Clay), "Clay", 24, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(GrayWolfStatue), "Animal Statues", "Gray Wolf", 70.0, 90.0, typeof(Clay), "Clay", 18, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(HellcatStatue), "Animal Statues", "Hellcat", 85.0, 95.0, typeof(Clay), "Clay", 20, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(HellHoundStatue), "Animal Statues", "Hell Hound", 85.0, 95.0, typeof(Clay), "Clay", 21, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(NightmareStatue), "Animal Statues", "Nightmare", 97.0, 104.0, typeof(Clay), "Clay", 28, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(PantherStatue), "Animal Statues", "Panther", 73.0, 85.0, typeof(Clay), "Clay", 21, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(RuneBeetleStatue), "Animal Statues", "Rune Beetle", 90.0, 105.0, typeof(Clay), "Clay", 30, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(SeaHorseStatue), "Animal Statues", "Sea Horse", 90.0, 105.0, typeof(Clay), "Clay", 26, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(SilverbackGorillaStatue), "Animal Statues", "Silverback Gorilla", 73.0, 85.0, typeof(Clay), "Clay", 23, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(SilverWolfStatue), "Animal Statues", "Silver Wolf", 80.0, 92.5, typeof(Clay), "Clay", 20, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(SnowLeopardStatue), "Animal Statues", "Snow Leopard", 80.0, 95.0, typeof(Clay), "Clay", 20, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(SquirrelStatue), "Animal Statues", "Squirrel", 95.5, 105.0, typeof(Clay), "Clay", 45, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(TimberWolfStatue), "Animal Statues", "Timber Wolf", 75.5, 95.0, typeof(Clay), "Clay", 22, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(TimberWolfStatue), "Animal Statues", "Vampire Bat", 90.0, 100.0, typeof(Clay), "Clay", 22, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(WolfStatue), "Animal Statues", "Wolf", 75.5, 95.0, typeof(Clay), "Clay", 22, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                #endregion
            }

            {
                #region Elemental Statues

                index = AddCraft(typeof(AcidElementalStatue), "Elemental Statues", "Acid Elemental", 94.0, 105.0, typeof(Clay), "Clay", 40, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(BloodElementalStatue), "Elemental Statues", "Blood Elemental", 94.0, 105.0, typeof(Clay), "Clay", 40, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(FireElementalStatue), "Elemental Statues", "Fire Elemental", 94.0, 105.0, typeof(Clay), "Clay", 40, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(IceElementalStatue), "Elemental Statues", "Ice Elemental", 94.0, 105.0, typeof(Clay), "Clay", 40, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(PoisonElementalStatue), "Elemental Statues", "Poison Elemental", 94.0, 105.0, typeof(Clay), "Clay", 40, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(SandVortexStatue), "Elemental Statues", "Sand Vortex", 94.0, 105.0, typeof(Clay), "Clay", 40, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(SnowElementalStatue), "Elemental Statues", "Snow Elemental", 94.0, 105.0, typeof(Clay), "Clay", 40, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(WaterElementalStatue), "Elemental Statues", "Water Elemental", 94.0, 105.0, typeof(Clay), "Clay", 40, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                #endregion
            }

            {
                #region Misc. Statues

                index = AddCraft(typeof(EvilMageStatue), "Misc. Statues", "Evil Mage", 88.0, 105.0, typeof(Clay), "Clay", 25, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(LordBlackthornStatue), "Misc. Statues", "Lord Blackthorn", 96.0, 105.0, typeof(Clay), "Clay", 100, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);
                AddSkill(index, SkillName.Blacksmith, 80.0, 100.0);
                AddSkill(index, SkillName.Carpentry, 75.0, 100.0);
                
                #endregion
            }

            {
                #region Monster Statues

                index = AddCraft(typeof(CorpserStatue), "Monster Statues", "Corpser", 80.0, 105.0, typeof(Clay), "Clay", 20, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(CyclopsStatue), "Monster Statues", "Cyclops", 85.0, 105.0, typeof(Clay), "Clay", 24, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(DaemonStatue), "Monster Statues", "Daemon", 84.0, 105.0, typeof(Clay), "Clay", 25, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(DragonStatue), "Monster Statues", "Dragon", 86.5, 105.0, typeof(Clay), "Clay", 28, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(ElderDaemonStatue), "Monster Statues", "Elder Daemon", 86.0, 105.0, typeof(Clay), "Clay", 27, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(GargoyleStatue), "Monster Statues", "Gargoyle", 81.0, 105.0, typeof(Clay), "Clay", 21, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(GazerStatue), "Monster Statues", "Gazer", 82.5, 105.0, typeof(Clay), "Clay", 23, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(IceFiendDaemonStatue), "Monster Statues", "Ice Fiend", 85.5, 105.0, typeof(Clay), "Clay", 26, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(ImpStatue), "Monster Statues", "Imp", 81.0, 105.0, typeof(Clay), "Clay", 20, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(KrakenStatue), "Monster Statues", "Kraken", 85.0, 105.0, typeof(Clay), "Clay", 23, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(MongbatStatue), "Monster Statues", "Mongbat", 74.0, 90.0, typeof(Clay), "Clay", 18, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(OgreLordStatue), "Monster Statues", "Ogre Lord", 87.0, 105.0, typeof(Clay), "Clay", 28, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(OphidianArchmageStatue), "Monster Statues", "Ophidian Archmage", 83.5, 105.0, typeof(Clay), "Clay", 24, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(OphidianKnightStatue), "Monster Statues", "Ophidian Knight", 83.0, 105.0, typeof(Clay), "Clay", 24, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(OphidianMageStatue), "Monster Statues", "Ophidian Mage", 83.0, 105.0, typeof(Clay), "Clay", 24, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(OphidianQueenStatue), "Monster Statues", "Ophidian Queen", 83.5, 105.0, typeof(Clay), "Clay", 24, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(OphidianWarriorStatue), "Monster Statues", "Ophidian Warrior", 83.0, 105.0, typeof(Clay), "Clay", 24, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(OrcCaptainStatue), "Monster Statues", "Orc Captain", 82.0, 105.0, typeof(Clay), "Clay", 20, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(OrcLordStatue), "Monster Statues", "Orc Lord", 82.0, 105.0, typeof(Clay), "Clay", 20, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(OrcShamanStatue), "Monster Statues", "Orc Shaman", 82.5, 105.0, typeof(Clay), "Clay", 20, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(OrcStatue), "Monster Statues", "Orc", 80.0, 105.0, typeof(Clay), "Clay", 20, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(RatmanStatue), "Monster Statues", "Ratman", 80.0, 105.0, typeof(Clay), "Clay", 21, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(SlimeStatue), "Monster Statues", "Slime", 83.0, 105.0, typeof(Clay), "Clay", 18, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(SolenQueenStatue), "Monster Statues", "Solen Queen", 83.0, 105.0, typeof(Clay), "Clay", 22, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(SolenWarriorStatue), "Monster Statues", "Solen Warrior", 82.5, 105.0, typeof(Clay), "Clay", 22, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(SolenWorkerStatue), "Monster Statues", "Solen Worker", 80.0, 105.0, typeof(Clay), "Clay", 20, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(StoneGargoyleStatue), "Monster Statues", "Stone Gargoyle", 85.0, 105.0, typeof(Clay), "Clay", 24, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(StoneHarpyStatue), "Monster Statues", "Stone Harpy", 85.0, 105.0, typeof(Clay), "Clay", 23, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(SwampTentaclesStatue), "Monster Statues", "Swamp Tentacles", 81.0, 105.0, typeof(Clay), "Clay", 19, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(TerathanAvengerStatue), "Monster Statues", "Terathan Avenger", 84.0, 105.0, typeof(Clay), "Clay", 23, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(TerathanDroneStatue), "Monster Statues", "Terathan Drone", 79.0, 105.0, typeof(Clay), "Clay", 17, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(TerathanQueenStatue), "Monster Statues", "Terathan Queen", 85.0, 105.0, typeof(Clay), "Clay", 24, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(TerathanWarriorStatue), "Monster Statues", "Terathan Warrior", 83.0, 105.0, typeof(Clay), "Clay", 22, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(TitanStatue), "Monster Statues", "Titan", 86.0, 105.0, typeof(Clay), "Clay", 26, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(WyvernStatue), "Monster Statues", "Wyvern", 93.0, 105.0, typeof(Clay), "Clay", 40, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

               #endregion

            {

            }

                #region Undead Statues

            index = AddCraft(typeof(GhostStatue), "Undead Statues", "Ghost", 81.0, 105.0, typeof(Clay), "Clay", 18, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(GhoulStatue), "Undead Statues", "Ghoul", 81.5, 105.0, typeof(Clay), "Clay", 19, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(HeadlessStatue), "Undead Statues", "Headless", 78.0, 105.0, typeof(Clay), "Clay", 18, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(LicheLordStatue), "Undead Statues", "Liche Lord", 86.0, 105.0, typeof(Clay), "Clay", 23, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(LicheStatue), "Undead Statues", "Liche", 85.0, 105.0, typeof(Clay), "Clay", 21, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(MummyStatue), "Undead Statues", "Mummy", 82.0, 105.0, typeof(Clay), "Clay", 20, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(RottingCorpseStatue), "Undead Statues", "Rotting Corpse", 83.0, 105.0, typeof(Clay), "Clay", 20, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(SkeletonKnightStatue), "Undead Statues", "Skeleton Knight", 82.5, 105.0, typeof(Clay), "Clay", 24, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(SkeletonMageStatue), "Undead Statues", "Skeleton Mage", 82.0, 105.0, typeof(Clay), "Clay", 23, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(SkeletonStatue), "Undead Statues", "Skeleton", 81.0, 105.0, typeof(Clay), "Clay", 22, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

                index = AddCraft(typeof(ZombieStatue), "Undead Statues", "Zombie", 80.5, 105.0, typeof(Clay), "Clay", 20, 1044253);
                AddRes(index, typeof(Dyes), 1024009, 1, 1044253);

               #endregion
            }

                MarkOption = true;
                Repair = Core.AOS;
            }
        }
    }

