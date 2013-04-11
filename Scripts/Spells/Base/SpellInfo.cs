using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Spells
{
	public class SpellInfo
	{
	    private int[] m_Amounts;
	    private static Dictionary<Type, string> m_ReagentShortStringList;

        public static Dictionary<Type, string> ReagentShortStringList { get { return m_ReagentShortStringList; } }

        public static void Initialize()
        {
            m_ReagentShortStringList = new Dictionary<Type, string>();

            m_ReagentShortStringList.Add(typeof(BlackPearl), "Bp");
            m_ReagentShortStringList.Add(typeof(Bloodmoss), "Bm");
            m_ReagentShortStringList.Add(typeof(Garlic), "Ga");
            m_ReagentShortStringList.Add(typeof(Ginseng), "Gi");
            m_ReagentShortStringList.Add(typeof(MandrakeRoot), "Mr");
            m_ReagentShortStringList.Add(typeof(Nightshade), "Ns");
            m_ReagentShortStringList.Add(typeof(SulfurousAsh), "Sa");
            m_ReagentShortStringList.Add(typeof(SpidersSilk), "Ss");
            m_ReagentShortStringList.Add(typeof(BatWing), "Bw");
            m_ReagentShortStringList.Add(typeof(GraveDust), "Gd");
            m_ReagentShortStringList.Add(typeof(DaemonBlood), "Db");
            m_ReagentShortStringList.Add(typeof(NoxCrystal), "Nc");
            m_ReagentShortStringList.Add(typeof(PigIron), "Pi");
        }

        public SpellInfo(string name, string mantra, params Type[] regs)
            : this(name, mantra, 16, 0, 0, true, regs)
        {
        }

        public SpellInfo(string name, string mantra, bool allowTown, params Type[] regs)
            : this(name, mantra, 16, 0, 0, allowTown, regs)
        {
        }

        public SpellInfo(string name, string mantra, int action, params Type[] regs)
            : this(name, mantra, action, 0, 0, true, regs)
        {
        }

        public SpellInfo(string name, string mantra, int action, bool allowTown, params Type[] regs)
            : this(name, mantra, action, 0, 0, allowTown, regs)
        {
        }

        public SpellInfo(string name, string mantra, int action, int handEffect, params Type[] regs)
            : this(name, mantra, action, handEffect, handEffect, true, regs)
        {
        }

        public SpellInfo(string name, string mantra, int action, int handEffect, bool allowTown, params Type[] regs)
            : this(name, mantra, action, handEffect, handEffect, allowTown, regs)
        {
        }

        public SpellInfo(string name, string mantra, int action, int leftHandEffect, int rightHandEffect, bool allowTown, params Type[] regs)
        {
			Name = name;
			Mantra = mantra;
			Action = action;
			Reagents = regs;
			AllowTown = allowTown;

			LeftHandEffect = leftHandEffect;
			RightHandEffect = rightHandEffect;

			m_Amounts = new int[regs.Length];

			for ( int i = 0; i < regs.Length; ++i )
				m_Amounts[i] = 1;
		}

	    public int Action { get; set; }

	    public bool AllowTown { get; set; }

	    public int[] Amounts{ get{ return m_Amounts; } set{ m_Amounts = value; } }
	    public string Mantra { get; set; }

	    public string Name { get; set; }

	    public Type[] Reagents { get; set; }

	    public int LeftHandEffect { get; set; }

	    public int RightHandEffect { get; set; }
	}
}