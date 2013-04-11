using System;
using System.Collections.Generic;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Fifth;

namespace Server.Custom.Polymorph
{
    public class SummonCreatureEntry
    {
        //Syntax---> PolymorphEntry(MenuGraphic, Creature, Cliloc#/String, SkillRequired). Check the first example
        public static readonly SummonCreatureEntry chicken = new SummonCreatureEntry(8401, typeof(Chicken), "Chicken", 40.0);
        public static readonly SummonCreatureEntry rabbit = new SummonCreatureEntry(8418, typeof(Rabbit), "Rabbit", 40.0);
        public static readonly SummonCreatureEntry dog = new SummonCreatureEntry(0x020D5, typeof(Dog), "Dog", 40.0);
        public static readonly SummonCreatureEntry cat = new SummonCreatureEntry(8475, typeof(Cat), "Cat", 40.0);
        public static readonly SummonCreatureEntry giantrat = new SummonCreatureEntry(0x20D0, typeof(GiantRat), "Giant Rat", 40.0);
        public static readonly SummonCreatureEntry sheep = new SummonCreatureEntry(0x20E6, typeof(Sheep), "Sheep", 40.0);
        public static readonly SummonCreatureEntry greathart = new SummonCreatureEntry(0x020D4, typeof(GreatHart), "Great Hart", 40.0);
        public static readonly SummonCreatureEntry horse = new SummonCreatureEntry(0x02120, typeof(Horse), "Horse", 45.0);
        public static readonly SummonCreatureEntry alligator = new SummonCreatureEntry(0x020DA, typeof(Alligator), "Alligator", 45.0);
        public static readonly SummonCreatureEntry brownbear = new SummonCreatureEntry(0x020CF, typeof(BrownBear), "Brown Bear", 45.0);
        public static readonly SummonCreatureEntry polarbear = new SummonCreatureEntry(0x020E1, typeof(PolarBear), "Polar Bear", 45.0);
        public static readonly SummonCreatureEntry giantserpent = new SummonCreatureEntry(8444, typeof(GiantSerpent), "Giant Serpent", 60.0);
        public static readonly SummonCreatureEntry scorpion = new SummonCreatureEntry(0x020e4, typeof(Scorpion), "Scorpion", 60.0);
        public static readonly SummonCreatureEntry ghost = new SummonCreatureEntry(8457, typeof(Ghost), "Ghost", 80.0);

        private static readonly SummonCreatureEntry[] m_Entries = new[]
			{
				chicken,
				rabbit,
                dog,
                cat,
                giantrat,
                sheep,
                greathart,
                horse,
                alligator,
                brownbear,
                polarbear,
                giantserpent,
                scorpion,
                ghost
			};

        private readonly int m_Art;
        private readonly Type m_Creature;
        private readonly string m_LocName;
        private readonly double m_SkillRequired;
        private static Dictionary<int, SummonCreatureEntry> m_SummonCreatureEntries;

        public static void Initialize()
        {
            m_SummonCreatureEntries = new Dictionary<int, SummonCreatureEntry>();

            for (int i = 0; i < m_Entries.Length; ++i)
                m_SummonCreatureEntries.Add(m_Entries[i].ArtID, m_Entries[i]);
        }

        public int ArtID { get { return m_Art; } }
        public Type Creature{ get { return m_Creature; } }
        public string LocName { get { return m_LocName; } }
        public double SkillRequired { get { return m_SkillRequired; } }
        static public Dictionary<int, SummonCreatureEntry> EntryInfo { get { return m_SummonCreatureEntries; } }

        private SummonCreatureEntry(int Art, Type Creature, string LocName, double SkillRequired)
        {
            m_Art = Art;
            m_Creature = Creature;
            m_LocName = LocName;
            m_SkillRequired = SkillRequired;
        }
    }

    public class SummonCreatureGump : Gump
    {
        private class SummonCreatureCategory
        {
            private readonly int m_Num;
            private readonly SummonCreatureEntry[] m_Entries;

            public SummonCreatureCategory(int num, params SummonCreatureEntry[] entries)
            {
                m_Num = num;
                m_Entries = entries;
            }

            public SummonCreatureEntry[] Entries { get { return m_Entries; } }
            public int LocNumber { get { return m_Num; } }
        }

        private static readonly SummonCreatureCategory[] Categories = new[]
			{
				new SummonCreatureCategory( 1015235, // Animals
					SummonCreatureEntry.chicken,
					SummonCreatureEntry.rabbit ,
                    SummonCreatureEntry.dog ,
                    SummonCreatureEntry.cat ,
                    SummonCreatureEntry.sheep ,
                    SummonCreatureEntry.greathart ,
                    SummonCreatureEntry.horse ,
                    SummonCreatureEntry.brownbear ,
                    SummonCreatureEntry.polarbear ),

				new SummonCreatureCategory( 1015245, // Monsters
                    SummonCreatureEntry.giantrat ,
					SummonCreatureEntry.alligator,
                    SummonCreatureEntry.giantserpent,
                    SummonCreatureEntry.scorpion,
                    SummonCreatureEntry.ghost)
			};

        private readonly Mobile m_Caster;
        private readonly Item m_Scroll;

        public SummonCreatureGump(Mobile caster, Item scroll) : base(50, 50)
        {
            m_Caster = caster;
            m_Scroll = scroll;

            int x;
            AddPage(0);
            AddBackground(0, 0, 585, 393, 5054);
            AddBackground(195, 36, 387, 275, 3000);
            AddHtml(0, 0, 510, 18, @"<center>Summon Creature Selection Menu", false, false);
            AddHtmlLocalized(60, 355, 150, 18, 1011036, false, false); // OKAY
            AddButton(25, 355, 4005, 4007, 1, GumpButtonType.Reply, 1);
            AddHtmlLocalized(320, 355, 150, 18, 1011012, false, false); // CANCEL
            AddButton(285, 355, 4005, 4007, 0, GumpButtonType.Reply, 2);

            int y = 35;
            for (int i = 0; i < Categories.Length; i++)
            {
                SummonCreatureCategory cat = Categories[i];
                AddHtmlLocalized(5, y, 150, 25, cat.LocNumber, true, false);
                AddButton(155, y, 4005, 4007, 0, GumpButtonType.Page, i + 1);
                y += 25;
            }

            for (int i = 0; i < Categories.Length; i++)
            {
                SummonCreatureCategory cat = Categories[i];
                AddPage(i + 1);

                for (int c = 0; c < cat.Entries.Length; c++)
                {
                    SummonCreatureEntry entry = cat.Entries[c];
                    x = 198 + (c % 3) * 129;
                    y = 38 + (c / 3) * 67;

                    AddHtml(x, y, 100, 18, entry.LocName, false, false);
                    AddItem(x + 20, y + 25, entry.ArtID);
                    AddRadio(x, y + 20, 210, 211, false, (c << 8) + i);
                }
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (info.ButtonID == 1 && info.Switches.Length > 0)
            {
                int cnum = info.Switches[0];
                int cat = cnum % 256;
                int ent = cnum >> 8;

                if (cat >= 0 && cat < Categories.Length)
                {
                    if (ent >= 0 && ent < Categories[cat].Entries.Length)
                    {
                        if (m_Caster.Skills[SkillName.Magery].Base < Categories[cat].Entries[ent].SkillRequired)
                            m_Caster.SendAsciiMessage("You are not skilled enough to summon this");
                        else
                        {
                            Spell spell = new SummonCreatureSpell(m_Caster, m_Scroll, Categories[cat].Entries[ent]);
                            spell.Cast();
                        }
                    }
                }
            }
        }
    }
}
