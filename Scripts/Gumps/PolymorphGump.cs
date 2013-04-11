using System.Collections.Generic;
using Server.Gumps;
using Server.Network;
using Server.Spells;
using Server.Spells.Seventh;
using Solaris.CliLocHandler;

namespace Server.Custom.Polymorph
{
    public enum StatModType
    {
        None,
        Strength,
        Dexterity,
        Intelligence,
        Armor,
        MinDamage,
        MaxDamage,
        SwingSpeed
    }

    public class StatMod
    {
        private readonly int m_Value;
        private readonly StatModType m_Type;

        public int Value { get { return m_Value; } }
        public StatModType Type { get { return m_Type; } }

        public StatMod(StatModType type, int value)
        {
            m_Type = type;
            m_Value = value;
        }
    }

    public class PolymorphEntry
    {
        //Syntax---> PolymorphEntry(MenuGraphic, BodyId, Cliloc#/String, StatMods, SkillRequired). Check the first example
        public static readonly PolymorphEntry Chicken = new PolymorphEntry(8401, 0xD0, 1015236, new[] { new StatMod(StatModType.Strength, -50), new StatMod(StatModType.Dexterity, -50) }, 40.0);
        public static readonly PolymorphEntry Dog = new PolymorphEntry(8405, 0xD9, 1015237, new[] { new StatMod(StatModType.Strength, -50), new StatMod(StatModType.Dexterity, -50) }, 40.0);
        public static readonly PolymorphEntry Cat = new PolymorphEntry(8475, 0xC9, "Cat", new[] { new StatMod(StatModType.Strength, -50), new StatMod(StatModType.Dexterity, -50) }, 40.0);
        public static readonly PolymorphEntry Wolf = new PolymorphEntry(8426, 0xE1, 1015238, new[] { new StatMod(StatModType.Armor, 5), new StatMod(StatModType.MinDamage, 5), new StatMod(StatModType.MaxDamage, 10), new StatMod(StatModType.SwingSpeed, 250) }, 40.0);
        public static readonly PolymorphEntry Panther = new PolymorphEntry(8473, 0xD6, 1015239, new[] { new StatMod(StatModType.Dexterity, 10), new StatMod(StatModType.Armor, 10), new StatMod(StatModType.MinDamage, 10), new StatMod(StatModType.MaxDamage, 15), new StatMod(StatModType.SwingSpeed, 250) }, 40.0);
        public static readonly PolymorphEntry Gorilla = new PolymorphEntry(8437, 0x1D, 1015240, new[] { new StatMod(StatModType.Armor, 10), new StatMod(StatModType.MinDamage, 10), new StatMod(StatModType.MaxDamage, 15), new StatMod(StatModType.SwingSpeed, 300) }, 40.0);
        public static readonly PolymorphEntry BlackBear = new PolymorphEntry(8399, 0xD3, 1015241, new[] { new StatMod(StatModType.Armor, 15), new StatMod(StatModType.MinDamage, 10), new StatMod(StatModType.MaxDamage, 20), new StatMod(StatModType.SwingSpeed, 300) }, 45.0);
        public static readonly PolymorphEntry GrizzlyBear = new PolymorphEntry(8411, 0xD4, 1015242, new[] { new StatMod(StatModType.Armor, 15), new StatMod(StatModType.MinDamage, 10), new StatMod(StatModType.MaxDamage, 20), new StatMod(StatModType.SwingSpeed, 300) }, 45.0);
        public static readonly PolymorphEntry PolarBear = new PolymorphEntry(8417, 0xD5, 1015243, new[] { new StatMod(StatModType.Armor, 15), new StatMod(StatModType.MinDamage, 10), new StatMod(StatModType.MaxDamage, 20), new StatMod(StatModType.SwingSpeed, 300) }, 45.0);
        public static readonly PolymorphEntry Horse = new PolymorphEntry(8481, 0xCC, "Horse", new[] { new StatMod(StatModType.None, 0) }, 45.0);
        public static readonly PolymorphEntry HumanMale = new PolymorphEntry(8397, 0x190, 1015244, new[] { new StatMod(StatModType.None, 0) }, 40.0);
        public static readonly PolymorphEntry HumanFemale = new PolymorphEntry(8398, 0x191, 1015254, new[] { new StatMod(StatModType.None, 0) }, 40.0);
        public static readonly PolymorphEntry Slime = new PolymorphEntry(8424, 0x33, 1015246, new[] { new StatMod(StatModType.Strength, -20), new StatMod(StatModType.Dexterity, 20), new StatMod(StatModType.MinDamage, 5), new StatMod(StatModType.MaxDamage, 15), new StatMod(StatModType.SwingSpeed, 200) }, 50.0);
        public static readonly PolymorphEntry Orc = new PolymorphEntry(8416, 0x11, 1015247, new[] { new StatMod(StatModType.Armor, 20), new StatMod(StatModType.MinDamage, 15), new StatMod(StatModType.MaxDamage, 20), new StatMod(StatModType.SwingSpeed, 300) }, 55.0);
        public static readonly PolymorphEntry LizardMan = new PolymorphEntry(8414, 0x24, 1015248, new[] { new StatMod(StatModType.Armor, 20), new StatMod(StatModType.MinDamage, 15), new StatMod(StatModType.MaxDamage, 25), new StatMod(StatModType.SwingSpeed, 300) }, 60.0);
        public static readonly PolymorphEntry Gargoyle = new PolymorphEntry(8409, 0x04, 1015249, new[] { new StatMod(StatModType.Strength, 30), new StatMod(StatModType.Dexterity, -8), new StatMod(StatModType.Armor, 25), new StatMod(StatModType.MinDamage, 15), new StatMod(StatModType.MaxDamage, 35), new StatMod(StatModType.SwingSpeed, 350) }, 63.0);
        public static readonly PolymorphEntry Ogre = new PolymorphEntry(8415, 0x01, 1015250, new[] { new StatMod(StatModType.Strength, 50), new StatMod(StatModType.Dexterity, -11), new StatMod(StatModType.Armor, 30), new StatMod(StatModType.MinDamage, 20), new StatMod(StatModType.MaxDamage, 40), new StatMod(StatModType.SwingSpeed, 400) }, 68.0);
        public static readonly PolymorphEntry Troll = new PolymorphEntry(8425, 0x36, 1015251, new[] { new StatMod(StatModType.Strength, 60), new StatMod(StatModType.Dexterity, -13), new StatMod(StatModType.Armor, 30), new StatMod(StatModType.MinDamage, 20), new StatMod(StatModType.MaxDamage, 45), new StatMod(StatModType.SwingSpeed, 400) }, 70.0);
        public static readonly PolymorphEntry Ettin = new PolymorphEntry(8408, 0x02, 1015252, new[] { new StatMod(StatModType.Strength, 80), new StatMod(StatModType.Dexterity, -19), new StatMod(StatModType.Armor, 35), new StatMod(StatModType.MinDamage, 25), new StatMod(StatModType.MaxDamage, 50), new StatMod(StatModType.SwingSpeed, 400) }, 73.0);
        public static readonly PolymorphEntry GiantSerpent = new PolymorphEntry(8446, 0x15, "Giant Serpent", new[] { new StatMod(StatModType.Strength, 115), new StatMod(StatModType.Dexterity, -23), new StatMod(StatModType.Armor, 35), new StatMod(StatModType.MinDamage, 30), new StatMod(StatModType.MaxDamage, 55), new StatMod(StatModType.SwingSpeed, 450) }, 75.0);
        public static readonly PolymorphEntry Daemon = new PolymorphEntry(8403, 0x09, 1015253, new[] { new StatMod(StatModType.Strength, 150), new StatMod(StatModType.Dexterity, -39), new StatMod(StatModType.Armor, 40) }, 85.0);
        public static readonly PolymorphEntry Dragon = new PolymorphEntry(8406, 0x3B, "Dragon", new[] { new StatMod(StatModType.Strength, 150), new StatMod(StatModType.Dexterity, -47), new StatMod(StatModType.Armor, 45), new StatMod(StatModType.MinDamage, 35), new StatMod(StatModType.MaxDamage, 60), new StatMod(StatModType.SwingSpeed, 500) }, 98.0);

        private static readonly PolymorphEntry[] m_Entries = new[]
			{
				Chicken,
				Dog,
                Cat,
				Wolf,
				Panther,
				Gorilla,
				BlackBear,
				GrizzlyBear,
				PolarBear,
                Horse,
				HumanMale,
				HumanFemale,
				Slime,
				Orc,
				LizardMan,
				Gargoyle,
				Ogre,
				Troll,
				Ettin,
                GiantSerpent,
				Daemon,
                Dragon
			};

        private readonly int m_Art;
        private readonly int m_Body;
        private readonly int m_Num;
        private readonly string m_LocName;
        private readonly StatMod[] m_StatMods;
        private readonly double m_SkillRequired;
        private static Dictionary<int, PolymorphEntry> m_PolymorphEntries;

        public static void Initialize()
        {
            m_PolymorphEntries = new Dictionary<int, PolymorphEntry>();

            for (int i = 0; i < m_Entries.Length; ++i)
                m_PolymorphEntries.Add(m_Entries[i].ArtID, m_Entries[i]);
        }

        public int ArtID { get { return m_Art; } }
        public int BodyID { get { return m_Body; } }
        public int LocNumber { get { return m_Num; } }
        public string LocName { get { return m_LocName; } }
        public StatMod[] StatMods { get { return m_StatMods; } }
        public double SkillRequired { get { return m_SkillRequired; } }
        static public Dictionary<int, PolymorphEntry> EntryInfo { get { return m_PolymorphEntries; } }

        private PolymorphEntry(int Art, int Body, int LocNum, StatMod[] StatMods, double SkillRequired)
        {
            m_Art = Art;
            m_Body = Body;
            m_Num = LocNum;
            m_StatMods = StatMods;
            m_SkillRequired = SkillRequired;
        }

        private PolymorphEntry(int Art, int Body, string LocName, StatMod[] StatMods, double SkillRequired)
        {
            m_Art = Art;
            m_Body = Body;
            m_LocName = LocName;
            m_StatMods = StatMods;
            m_SkillRequired = SkillRequired;
        }

        public string GetName()
        {
            if (m_LocName != null)
                return m_LocName;
            if (m_Num > 0)
                return CliLoc.LocToString(m_Num);
            return "Name error";
        }
    }

    public class PolymorphGump : Gump
    {
        private class PolymorphCategory
        {
            private readonly int m_Num;
            private readonly PolymorphEntry[] m_Entries;

            public PolymorphCategory(int num, params PolymorphEntry[] entries)
            {
                m_Num = num;
                m_Entries = entries;
            }

            public PolymorphEntry[] Entries { get { return m_Entries; } }
            public int LocNumber { get { return m_Num; } }
        }

        private static readonly PolymorphCategory[] Categories = new[]
			{
				new PolymorphCategory( 1015235, // Animals
					PolymorphEntry.Chicken,
					PolymorphEntry.Dog,
                    PolymorphEntry.Cat,
					PolymorphEntry.Wolf,
					PolymorphEntry.Panther,
					PolymorphEntry.Gorilla,
					PolymorphEntry.BlackBear,
					PolymorphEntry.GrizzlyBear,
					PolymorphEntry.PolarBear,
                    PolymorphEntry.Horse,
					PolymorphEntry.HumanMale ),

				new PolymorphCategory( 1015245, // Monsters
					PolymorphEntry.Slime,
					PolymorphEntry.Orc,
					PolymorphEntry.LizardMan,
					PolymorphEntry.Gargoyle,
					PolymorphEntry.Ogre,
					PolymorphEntry.Troll,
					PolymorphEntry.Ettin,
                    PolymorphEntry.GiantSerpent,
					PolymorphEntry.Daemon,
                    PolymorphEntry.Dragon,
					PolymorphEntry.HumanFemale )
			};


        private readonly Mobile m_Caster;
        private readonly Item m_Scroll;

        public PolymorphGump(Mobile caster, Item scroll)
            : base(50, 50)
        {
            m_Caster = caster;
            m_Scroll = scroll;

            int x;
            AddPage(0);
            AddBackground(0, 0, 585, 393, 5054);
            AddBackground(195, 36, 387, 275, 3000);
            AddHtmlLocalized(0, 0, 510, 18, 1015234, false, false); // <center>Polymorph Selection Menu</center>
            AddHtmlLocalized(60, 355, 150, 18, 1011036, false, false); // OKAY
            AddButton(25, 355, 4005, 4007, 1, GumpButtonType.Reply, 1);
            AddHtmlLocalized(320, 355, 150, 18, 1011012, false, false); // CANCEL
            AddButton(285, 355, 4005, 4007, 0, GumpButtonType.Reply, 2);

            int y = 35;
            for (int i = 0; i < Categories.Length; i++)
            {
                PolymorphCategory cat = Categories[i];
                AddHtmlLocalized(5, y, 150, 25, cat.LocNumber, true, false);
                AddButton(155, y, 4005, 4007, 0, GumpButtonType.Page, i + 1);
                y += 25;
            }

            for (int i = 0; i < Categories.Length; i++)
            {
                PolymorphCategory cat = Categories[i];
                AddPage(i + 1);

                for (int c = 0; c < cat.Entries.Length; c++)
                {
                    PolymorphEntry entry = cat.Entries[c];
                    x = 198 + (c % 3) * 129;
                    y = 38 + (c / 3) * 67;

                    AddHtml(x, y, 100, 18, entry.GetName(), false, false);
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
                            m_Caster.SendAsciiMessage("You are not skilled enough to polymorph into this");
                        else
                        {
                            Spell spell = new PolymorphSpell(m_Caster, m_Scroll, Categories[cat].Entries[ent]);
                            spell.Cast();
                        }
                    }
                }
            }
        }
    }
}
