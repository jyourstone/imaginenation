/*using System;
using System.Collections.Generic;
using Server.Menus.ItemLists;
using Server.Mobiles;
using Server.Network;
using Server.Spells.Fifth;

namespace Server.Custom.SummonCreature {
	public class CustomSummonEntry {
		public static readonly CustomSummonEntry GiantScorpion = new CustomSummonEntry(0x020e4, typeof(Scorpion), "Giant Scorpion", 75.0);

		public static readonly CustomSummonEntry BrownBear = new CustomSummonEntry(0x020CF, typeof(BrownBear), "Brown Bear", 50.0);

		public static readonly CustomSummonEntry Chicken = new CustomSummonEntry(8401, typeof(Chicken), "Chicken", 35.0);

		public static readonly CustomSummonEntry LargeDeer = new CustomSummonEntry(0x020D4, typeof(GreatHart), "Large Deer", 35.0);
		public static readonly CustomSummonEntry Deer = new CustomSummonEntry(0x020D4, typeof(Hind), "Deer", 35.0);
		public static readonly CustomSummonEntry Dog = new CustomSummonEntry(0x020D5, typeof(Dog), "Dog", 35.0);

		public static readonly CustomSummonEntry Alligator = new CustomSummonEntry(0x020DA, typeof(Alligator), "Alligator", 35.0);

		public static readonly CustomSummonEntry GrizzlyBear = new CustomSummonEntry(0x020DB, typeof(GrizzlyBear), "Grizzly Bear", 35.0);

		public static readonly CustomSummonEntry Horse = new CustomSummonEntry(0x02120, typeof(Horse), "Horse", 35.0);

		public static readonly CustomSummonEntry PolarBear = new CustomSummonEntry(0x020E1, typeof(PolarBear), "Polar Bear", 35.0);

		public static readonly CustomSummonEntry Rabbit = new CustomSummonEntry(8418, typeof(Rabbit), "Rabbit", 20.0);

		public static readonly CustomSummonEntry Serpent = new CustomSummonEntry(8444, typeof(GiantSerpent), "Serpent", 25.0);

		public static readonly CustomSummonEntry LargeRat = new CustomSummonEntry(0x20D0, typeof(GiantRat), "Large Rat", 35.0);

		public static readonly CustomSummonEntry Sheep = new CustomSummonEntry(0x20E6, typeof(Sheep), "Sheep", 35.0);

		private static readonly CustomSummonEntry[] m_Entries = new CustomSummonEntry[]
            {
                GiantScorpion,
                BrownBear,
                LargeDeer,
                Deer,
                Chicken,
                Dog,
                Alligator,
                GrizzlyBear,
                Horse,
                PolarBear,
                Rabbit,
                Serpent,
                LargeRat,
                Sheep
            };

		private readonly int m_Art;

		/* Storing an instance of the class does not work because each time the spell is called, the same object is returned. 
		 * The first time a player summons a specific creature type it works fine, but any consective summon will just
		 * change the position of the original creature - the same creature for every player. Storing the type of the creature 
		 * and creating a new instance each time solves the problem. That, or just not make the CustomSummonEntrys static. 
		 * - Jonny 
		 
		//private BaseCreature m_ToSummon; 

		private readonly Type m_ToSummonType;
		private readonly string m_LocName;
		private readonly double m_SkillRequired;
		private static Dictionary<ItemListEntry, CustomSummonEntry> m_SummonEntries;

		public static void Initialize() {
			m_SummonEntries = new Dictionary<ItemListEntry, CustomSummonEntry>();

			for (int i = 0; i < m_Entries.Length; ++i)
				m_SummonEntries.Add(new ItemListEntry(m_Entries[i].GetName(), m_Entries[i].ArtID), m_Entries[i]);
		}

		public int ArtID { get { return m_Art; } }
		public Type ToSummonType { get { return m_ToSummonType; } }
		public string LocName { get { return m_LocName; } }
		public double SkillRequired { get { return m_SkillRequired; } }

		static public Dictionary<ItemListEntry, CustomSummonEntry> EntryInfo { get { return m_SummonEntries; } }

		private CustomSummonEntry(int Art, Type toSummonType, string LocName, double SkillRequired) {
			m_Art = Art;
			m_ToSummonType = toSummonType;
			m_LocName = LocName;
			m_SkillRequired = SkillRequired;
		}

		public string GetName() {
			if (m_LocName != null)
				return m_LocName;
			else
				return "Name error";
		}
	}

	public class SummonCreatureMenu : ItemListMenu {
		private readonly List<ItemListEntry> localCreatureList;
		private readonly Item m_Scroll;
		private readonly Mobile m_Caster;

		public SummonCreatureMenu(Mobile caster, Item scroll)
			: base("What creature do you wish to summon?", null) {
			m_Scroll = scroll;
			m_Caster = caster;
			localCreatureList = new List<ItemListEntry>();

			foreach (ItemListEntry ile in CustomSummonEntry.EntryInfo.Keys) {
				if (caster.Skills.Magery.Base >= CustomSummonEntry.EntryInfo[ile].SkillRequired)
					localCreatureList.Add(ile);
			}

			Entries = localCreatureList.ToArray();
		}

		public override void OnResponse(NetState state, int index) {
			SummonCreatureSpell scs = new SummonCreatureSpell(m_Caster, m_Scroll, Activator.CreateInstance(CustomSummonEntry.EntryInfo[Entries[index]].ToSummonType) as BaseCreature);
			scs.Cast();
		}
	}
}
*/