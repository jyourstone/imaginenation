using System.Collections.Generic;
using Server.Misc;
using Server.Mobiles;

namespace Server.Gumps
{
	public class SkillBonusGump : Gump
	{
        private readonly Mobile m_player;
        private readonly List<int> m_skills = new List<int>();
        private readonly int[] m_choises = new int[49];
        private readonly int[] m_buttonid = new int[49];
        private static readonly int[] m_pointstoboost = new int[49];
        private readonly string m_caption = "";

        public SkillBonusGump(Mobile from, List<int> skills)
			: base( 50, 40 )
		{
            from.CloseAllGumps();

            m_player = from;
            m_skills = skills;
            m_caption = "Choose your skills: " + m_skills.Count + " / 4";

            for (int i = 0; i < 49; ++i)
                m_buttonid[i] = 210;

            if (m_skills.Count > 0)
            {
                foreach (int SkillIndex in m_skills)
                {
                    m_choises[SkillIndex - 1] = 32;
                    m_buttonid[SkillIndex - 1] = 211;
                }
            }

            #region Points to boost
            m_pointstoboost[0] = 55;     //Alchemy
            m_pointstoboost[1] = 100;    //Anatomy
            m_pointstoboost[2] = 100;    //Animal lore
            m_pointstoboost[3] = 100;    //Item ID
            m_pointstoboost[4] = 100;    //Arms Lore
            m_pointstoboost[5] = 95;    //Parrying
            m_pointstoboost[6] = 100;    //Begging
            m_pointstoboost[7] = 55;    //Blacksmith
            m_pointstoboost[8] = 60;    //Fletching
            m_pointstoboost[9] = 75;    //Peacemaking
            m_pointstoboost[10] = 100;   //Camping
            m_pointstoboost[11] = 60;   //Carpentry
            m_pointstoboost[12] = 85;   //Cartography
            m_pointstoboost[13] = 95;   //Cooking
            m_pointstoboost[14] = 100;   //Detect Hidden
            m_pointstoboost[15] = 95;   //Discordance
            m_pointstoboost[16] = 100;   //Eval int
            m_pointstoboost[17] = 100;   //Healing
            m_pointstoboost[18] = 80;   //Fishing
            m_pointstoboost[19] = 100;   //Forensics
            m_pointstoboost[20] = 100;   //Herding
            m_pointstoboost[21] = 90;   //Hiding
            m_pointstoboost[22] = 90;   //Provocation
            m_pointstoboost[23] = 55;   //Inscription
            m_pointstoboost[24] = 80;   //Lockpicking
            m_pointstoboost[25] = 90;   //Magery
            m_pointstoboost[26] = 90;   //Magic Resist
            m_pointstoboost[27] = 95;   //Tactics
            m_pointstoboost[28] = 100;   //Snooping
            m_pointstoboost[29] = 90;   //Musicianship
            m_pointstoboost[30] = 80;   //Poisoning
            m_pointstoboost[31] = 90;   //Archery
            m_pointstoboost[32] = 100;   //SpiritSpeak
            m_pointstoboost[33] = 75;   //Stealing
            m_pointstoboost[34] = 60;   //Tailoring
            m_pointstoboost[35] = 55;   //Animal Taming
            m_pointstoboost[36] = 100;   //Taste ID
            m_pointstoboost[37] = 65;   //Tinkering
            m_pointstoboost[38] = 100;   //Tracking
            m_pointstoboost[39] = 100;   //Veterinary
            m_pointstoboost[40] = 95;   //Swords
            m_pointstoboost[41] = 95;   //Macing
            m_pointstoboost[42] = 95;   //Fencing
            m_pointstoboost[43] = 100;   //Wrestling
            m_pointstoboost[44] = 60;   //Lumberjacking
            m_pointstoboost[45] = 75;   //Mining
            m_pointstoboost[46] = 100;   //Meditation
            m_pointstoboost[47] = 95;   //Stealth
            m_pointstoboost[48] = 100;   //Remove trap
            #endregion

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(0, 0, 686, 406, 9200);
            //First column
            AddLabel(40, 56, m_choises[0], string.Format("{0} Alchemy", m_pointstoboost[0]));
            AddLabel(40, 80, m_choises[1], string.Format("{0} Anatomy", m_pointstoboost[1]));
            AddLabel(40, 104, m_choises[2], string.Format("{0} Animal Lore", m_pointstoboost[2]));
            AddLabel(40, 128, m_choises[3], string.Format("{0} Item ID", m_pointstoboost[3]));
            AddLabel(40, 152, m_choises[4], string.Format("{0} Arms Lore", m_pointstoboost[4]));
            AddLabel(40, 176, m_choises[5], string.Format("{0} Parrying", m_pointstoboost[5]));
            AddLabel(40, 200, m_choises[6], string.Format("{0} Begging", m_pointstoboost[6]));
            AddLabel(40, 224, m_choises[7], string.Format("{0} Blacksmith", m_pointstoboost[7]));
            AddLabel(40, 247, m_choises[8], string.Format("{0} Fletching", m_pointstoboost[8]));
            AddLabel(40, 272, m_choises[9], string.Format("{0} Peacemaking", m_pointstoboost[9]));
            AddLabel(40, 296, m_choises[10], string.Format("{0} Camping", m_pointstoboost[10]));
            AddLabel(40, 320, m_choises[11], string.Format("{0} Carpentry", m_pointstoboost[11]));
            AddLabel(40, 344, m_choises[12], string.Format("{0} Cartography", m_pointstoboost[12]));

            //Second column
            AddLabel(208, 56, m_choises[13], string.Format("{0} Cooking", m_pointstoboost[13]));
            AddLabel(208, 80, m_choises[14], string.Format("{0} Detect Hidden", m_pointstoboost[14]));
            AddLabel(208, 104, m_choises[15], string.Format("{0} Discordance", m_pointstoboost[15]));
            AddLabel(208, 128, m_choises[16], string.Format("{0} Eval Int", m_pointstoboost[16]));
            AddLabel(208, 152, m_choises[17], string.Format("{0} Healing", m_pointstoboost[17]));
            AddLabel(208, 176, m_choises[18], string.Format("{0} Fishing", m_pointstoboost[18]));
            AddLabel(208, 200, m_choises[19], string.Format("{0} Forensics", m_pointstoboost[19]));
            AddLabel(208, 224, m_choises[20], string.Format("{0} Herding", m_pointstoboost[20]));
            AddLabel(208, 248, m_choises[21], string.Format("{0} Hiding", m_pointstoboost[21]));
            AddLabel(208, 272, m_choises[22], string.Format("{0} Provocation", m_pointstoboost[22]));
            AddLabel(208, 296, m_choises[23], string.Format("{0} Inscription", m_pointstoboost[23]));
            AddLabel(208, 320, m_choises[24], string.Format("{0} Lockpicking", m_pointstoboost[24]));
            AddLabel(208, 344, m_choises[25], string.Format("{0} Magery", m_pointstoboost[25]));

            //Third column
            AddLabel(376, 56, m_choises[26], string.Format("{0} Magic Resist", m_pointstoboost[26]));
            AddLabel(376, 80, m_choises[27], string.Format("{0} Tactics", m_pointstoboost[27]));
            AddLabel(376, 104, m_choises[28], string.Format("{0} Snooping", m_pointstoboost[28]));
            AddLabel(376, 128, m_choises[29], string.Format("{0} Musicianship", m_pointstoboost[29]));
            AddLabel(376, 152, m_choises[30], string.Format("{0} Poisoning", m_pointstoboost[30]));
            AddLabel(376, 176, m_choises[31], string.Format("{0} Archery", m_pointstoboost[31]));
            AddLabel(376, 200, m_choises[32], string.Format("{0} SpiritSpeak", m_pointstoboost[32]));
            AddLabel(376, 224, m_choises[33], string.Format("{0} Stealing", m_pointstoboost[33]));
            AddLabel(376, 248, m_choises[34], string.Format("{0} Tailoring", m_pointstoboost[34]));
            AddLabel(376, 272, m_choises[35], string.Format("{0} Animal Taming", m_pointstoboost[35]));
            AddLabel(376, 296, m_choises[36], string.Format("{0} Taste ID", m_pointstoboost[36]));
            AddLabel(376, 320, m_choises[37], string.Format("{0} Tinkering", m_pointstoboost[37]));
            AddLabel(376, 344, m_choises[38], string.Format("{0} Tracking", m_pointstoboost[38]));

            //Fourth column
            AddLabel(544, 56, m_choises[39], string.Format("{0} Veterinary", m_pointstoboost[39]));
            AddLabel(544, 80, m_choises[40], string.Format("{0} Swordsmanship", m_pointstoboost[40]));
            AddLabel(544, 104, m_choises[41], string.Format("{0} Macing", m_pointstoboost[41]));
            AddLabel(544, 128, m_choises[42], string.Format("{0} Fencing", m_pointstoboost[42]));
            AddLabel(544, 152, m_choises[43], string.Format("{0} Wrestling", m_pointstoboost[43]));
            AddLabel(544, 176, m_choises[44], string.Format("{0} Lumberjacking", m_pointstoboost[44]));
            AddLabel(544, 200, m_choises[45], string.Format("{0} Mining", m_pointstoboost[45]));
            AddLabel(544, 224, m_choises[46], string.Format("{0} Meditation", m_pointstoboost[46]));
            AddLabel(544, 248, m_choises[47], string.Format("{0} Stealth", m_pointstoboost[47]));
            AddLabel(544, 272, m_choises[48], string.Format("{0} Remove Trap", m_pointstoboost[48]));

            AddLabel(16, 16, 292, m_caption);

            // -- 1st column
            AddButton(16, 56, m_buttonid[0], 211, (int)Buttons.Button1, GumpButtonType.Reply, 0);
            AddButton(16, 80, m_buttonid[1], 211, (int)Buttons.Button2, GumpButtonType.Reply, 0);
            AddButton(16, 104, m_buttonid[2], 211, (int)Buttons.Button3, GumpButtonType.Reply, 0);
            AddButton(16, 128, m_buttonid[3], 211, (int)Buttons.Button4, GumpButtonType.Reply, 0);
            AddButton(16, 152, m_buttonid[4], 211, (int)Buttons.Button5, GumpButtonType.Reply, 0);
            AddButton(16, 176, m_buttonid[5], 211, (int)Buttons.Button6, GumpButtonType.Reply, 0);
            AddButton(16, 200, m_buttonid[6], 211, (int)Buttons.Button7, GumpButtonType.Reply, 0);
            AddButton(16, 224, m_buttonid[7], 211, (int)Buttons.Button8, GumpButtonType.Reply, 0);
            AddButton(16, 248, m_buttonid[8], 211, (int)Buttons.Button9, GumpButtonType.Reply, 0);
            AddButton(16, 272, m_buttonid[9], 211, (int)Buttons.Button10, GumpButtonType.Reply, 0);
            AddButton(16, 296, m_buttonid[10], 211, (int)Buttons.Button11, GumpButtonType.Reply, 0);
            AddButton(16, 320, m_buttonid[11], 211, (int)Buttons.Button12, GumpButtonType.Reply, 0);
            AddButton(16, 344, m_buttonid[12], 211, (int)Buttons.Button13, GumpButtonType.Reply, 0);

            // -- 2nd column
            AddButton(184, 56, m_buttonid[13], 211, (int)Buttons.Button14, GumpButtonType.Reply, 0);
            AddButton(184, 80, m_buttonid[14], 211, (int)Buttons.Button15, GumpButtonType.Reply, 0);
            AddButton(184, 104, m_buttonid[15], 211, (int)Buttons.Button16, GumpButtonType.Reply, 0);
            AddButton(184, 128, m_buttonid[16], 211, (int)Buttons.Button17, GumpButtonType.Reply, 0);
            AddButton(184, 152, m_buttonid[17], 211, (int)Buttons.Button18, GumpButtonType.Reply, 0);
            AddButton(184, 176, m_buttonid[18], 211, (int)Buttons.Button19, GumpButtonType.Reply, 0);
            AddButton(184, 200, m_buttonid[19], 211, (int)Buttons.Button20, GumpButtonType.Reply, 0);
            AddButton(184, 224, m_buttonid[20], 211, (int)Buttons.Button21, GumpButtonType.Reply, 0);
            AddButton(184, 248, m_buttonid[21], 211, (int)Buttons.Button22, GumpButtonType.Reply, 0);
            AddButton(184, 272, m_buttonid[22], 211, (int)Buttons.Button23, GumpButtonType.Reply, 0);
            AddButton(184, 296, m_buttonid[23], 211, (int)Buttons.Button24, GumpButtonType.Reply, 0);
            AddButton(184, 320, m_buttonid[24], 211, (int)Buttons.Button25, GumpButtonType.Reply, 0);
            AddButton(184, 344, m_buttonid[25], 211, (int)Buttons.Button26, GumpButtonType.Reply, 0);

            // -- 3rd column
            AddButton(352, 56, m_buttonid[26], 211, (int)Buttons.Button27, GumpButtonType.Reply, 0);
            AddButton(352, 80, m_buttonid[27], 211, (int)Buttons.Button28, GumpButtonType.Reply, 0);
            AddButton(352, 104, m_buttonid[28], 211, (int)Buttons.Button29, GumpButtonType.Reply, 0);
            AddButton(352, 128, m_buttonid[29], 211, (int)Buttons.Button30, GumpButtonType.Reply, 0);
            AddButton(352, 152, m_buttonid[30], 211, (int)Buttons.Button31, GumpButtonType.Reply, 0);
            AddButton(352, 176, m_buttonid[31], 211, (int)Buttons.Button32, GumpButtonType.Reply, 0);
            AddButton(352, 200, m_buttonid[32], 211, (int)Buttons.Button33, GumpButtonType.Reply, 0);
            AddButton(352, 224, m_buttonid[33], 211, (int)Buttons.Button34, GumpButtonType.Reply, 0);
            AddButton(352, 248, m_buttonid[34], 211, (int)Buttons.Button35, GumpButtonType.Reply, 0);
            AddButton(352, 272, m_buttonid[35], 211, (int)Buttons.Button36, GumpButtonType.Reply, 0);
            AddButton(352, 296, m_buttonid[36], 211, (int)Buttons.Button37, GumpButtonType.Reply, 0);
            AddButton(352, 320, m_buttonid[37], 211, (int)Buttons.Button38, GumpButtonType.Reply, 0);
            AddButton(352, 344, m_buttonid[38], 211, (int)Buttons.Button39, GumpButtonType.Reply, 0);

            // -- 4th column
            AddButton(520, 56, m_buttonid[39], 211, (int)Buttons.Button40, GumpButtonType.Reply, 0);
            AddButton(520, 80, m_buttonid[40], 211, (int)Buttons.Button41, GumpButtonType.Reply, 0);
            AddButton(520, 104, m_buttonid[41], 211, (int)Buttons.Button42, GumpButtonType.Reply, 0);
            AddButton(520, 128, m_buttonid[42], 211, (int)Buttons.Button43, GumpButtonType.Reply, 0);
            AddButton(520, 152, m_buttonid[43], 211, (int)Buttons.Button44, GumpButtonType.Reply, 0);
            AddButton(520, 176, m_buttonid[44], 211, (int)Buttons.Button45, GumpButtonType.Reply, 0);
            AddButton(520, 200, m_buttonid[45], 211, (int)Buttons.Button46, GumpButtonType.Reply, 0);
            AddButton(520, 224, m_buttonid[46], 211, (int)Buttons.Button47, GumpButtonType.Reply, 0);
            AddButton(520, 248, m_buttonid[47], 211, (int)Buttons.Button48, GumpButtonType.Reply, 0);
            AddButton(520, 272, m_buttonid[48], 211, (int)Buttons.Button49, GumpButtonType.Reply, 0);

            // -- 'okay' button
            AddButton(351, 376, 247, 248, (int)Buttons.Button50, GumpButtonType.Reply, 0);
            AddButton(279, 376, 242, 241, (int)Buttons.Button51, GumpButtonType.Reply, 0);
        }

        public enum Buttons
        {
            Button51,
            Button1,
            Button2,
            Button3,
            Button4,
            Button5,
            Button6,
            Button7,
            Button8,
            Button9,
            Button10,
            Button11,
            Button12,
            Button13,
            Button14,
            Button15,
            Button16,
            Button17,
            Button18,
            Button19,
            Button20,
            Button21,
            Button22,
            Button23,
            Button24,
            Button25,
            Button26,
            Button27,
            Button28,
            Button29,
            Button30,
            Button31,
            Button32,
            Button33,
            Button34,
            Button35,
            Button36,
            Button37,
            Button38,
            Button39,
            Button40,
            Button41,
            Button42,
            Button43,
            Button44,
            Button45,
            Button46,
            Button47,
            Button48,
            Button49,
            Button50,
        }

        public static void EnsureClosed(Mobile m)
        {
            m.CloseGump(typeof(SkillBonusGump));
        }

        public override void OnResponse(Network.NetState sender, RelayInfo info)
        {
            // TODO: need to be able to close the gump
            if (info.ButtonID >= 1 && info.ButtonID < 50)
            {
                if (m_skills.Contains(info.ButtonID))
                {
                    m_skills.Remove(info.ButtonID);
                    sender.Mobile.SendGump(new SkillBonusGump(m_player, m_skills));
                }
                else if (m_skills.Count < 4)
                {
                    m_skills.Add(info.ButtonID);
                    if (info.ButtonID == 4)
                        sender.Mobile.SendGump(new SkillBonusWarningGump(info.ButtonID, m_skills));
                    else if (sender.Mobile.Skills[info.ButtonID - 1].Base >= m_pointstoboost[info.ButtonID - 1])
                        sender.Mobile.SendGump(new SkillBonusWarningGump(100, m_skills));
                    else
                        sender.Mobile.SendGump(new SkillBonusGump(m_player, m_skills));
                }
                else
                    sender.Mobile.SendGump(new SkillBonusGump(m_player, m_skills));
            }
            else if (info.ButtonID == 50)
            {
                if (m_skills.Count == 4)
                {
                    if (sender.Mobile is PlayerMobile)
                        ApplyResults((PlayerMobile)sender.Mobile, m_skills);
                }
                else
                    sender.Mobile.SendGump(new SkillBonusGump(m_player, m_skills));
            }
            else if (info.ButtonID == 0)
                EnsureClosed(sender.Mobile);
        }

	    static void ApplyResults(PlayerMobile from, ICollection<int> skillpicks)
        {
            if (skillpicks.Count < 4)
                EnsureClosed(from);
            else if (!from.HasStartingSkillBoost)
            {
                foreach (int SkillIndex in skillpicks)
                {
                    if (from.Skills[SkillIndex - 1].Base < m_pointstoboost[SkillIndex - 1])
                    {
                        from.Skills[SkillIndex - 1].Base = m_pointstoboost[SkillIndex - 1];
                        CharacterCreation.AddSkillItems(from.Skills[SkillIndex - 1].SkillName, from);
                    }
                }
                from.HasStartingSkillBoost = true;
            }
            else
                from.SendAsciiMessage("You have already selected your skill bonuses!");
        }
    }

    public class SkillBonusWarningGump : Gump
    {
        private readonly List<int>m_SkillList;
        private readonly string m_message;

        public SkillBonusWarningGump(int skill, List<int>list)
            : base(100, 100)
        {
            m_SkillList = list;

            if (skill == 4)
                m_message = "Warning: At the moment, all items in the world are automatically identified making this skill obsolete. You may select this skill anyway but it is not advisable.";
            else if (skill == 100)
                m_message = "Warning: You already have equal to or higher points in this skill than you can get boosted. You will not get any boost in this skill.";
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(0, 0, 331, 134, 9200);
            AddHtml(9, 11, 310, 80, m_message, true, false);
            AddButton(136, 104, 247, 248, (int)Buttons.Button1, GumpButtonType.Reply, 0);
        }

        public enum Buttons
        {
            Button1,
        }

        public override void OnResponse(Network.NetState sender, RelayInfo info)
        {
            sender.Mobile.SendGump(new SkillBonusGump(sender.Mobile, m_SkillList));
        }
    }

    public class SkillBonusStoneInfoGump : Gump
    {
        readonly List<int> m_skills = new List<int>();

        public SkillBonusStoneInfoGump()
            : base(200, 100)
        {
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(0, 0, 427, 208, 9200);
            AddHtml(8, 32, 411, 139, @"After this dialog you will be given a possibility to choose 4 starting skills. Values before skill names indicate to what value your skill will be boosted if you choose to have that skill. You can leave this step for later if you so choose and enter the game world directly by not choosing any skills. To choose your skills later find the Skill Stone located in the starting area. This is a one time bonus for each character.", true, false);
            AddButton(180, 177, 247, 248, (int)Buttons.Button1, GumpButtonType.Reply, 0);
            AddLabel(159, 8, 92, @"Welcome to IN!");

        }

        public enum Buttons
        {
            Button1,
        }

        public override void OnResponse(Network.NetState sender, RelayInfo info)
        {
            sender.Mobile.SendGump(new SkillBonusGump(sender.Mobile, m_skills));
        }

    }
}