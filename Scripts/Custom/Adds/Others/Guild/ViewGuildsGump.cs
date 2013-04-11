using System;
using System.Collections.Generic;
using Server.Guilds;
using Server.Network;

namespace Server.Gumps
{
    public class ViewGuildsGump : Gump
    {
        private readonly List<Guild> m_Guilds;

        public static readonly int GumpOffsetX = 30;
        public static readonly int GumpOffsetY = 30;

        public static readonly int TextHue = 0;
        public static readonly int TextOffsetX = 2;

        public static readonly int OffsetGumpID = 0x0A40; // Pure black
        public static readonly int HeaderGumpID = 0x0E14; //Dark navy blue, textured
        public static readonly int EntryGumpID = 0x0BBC; // Light offwhite, textured
        public static readonly int BackGumpID = 0x13BE; // Gray slate/stoney
        public static readonly int SetGumpID = 0x0E14; // Dark navy blue, textured

        public static readonly int SetWidth = 20;
        public static readonly int SetOffsetX = 2, SetOffsetY = 2;
        public static readonly int SetButtonID1 = 0x15E1; // Arrow pointing right
        public static readonly int SetButtonID2 = 0x15E5; // " pressed

        public static readonly int PrevWidth = 20;
        public static readonly int PrevOffsetX = 2, PrevOffsetY = 2;
        public static readonly int PrevButtonID1 = 0x15E3; // Arrow pointing left
        public static readonly int PrevButtonID2 = 0x15E7; // " pressed

        public static readonly int NextWidth = 20;
        public static readonly int NextOffsetX = 2, NextOffsetY = 2;
        public static readonly int NextButtonID1 = 0x15E1; // Arrow pointing right
        public static readonly int NextButtonID2 = 0x15E5; // " pressed

        public static readonly int OffsetSize = 1;

        public static readonly int EntryHeight = 20;
        public static readonly int BorderSize = 10;

        private static bool PrevLabel = false, NextLabel = false;

        private static readonly int PrevLabelOffsetX = PrevWidth + 1;
        private static readonly int PrevLabelOffsetY = 0;

        private static readonly int NextLabelOffsetX = -29;
        private static readonly int NextLabelOffsetY = 0;

        private static readonly int EntryWidth = 250;
        private static readonly int EntryCount = 15;

        private static readonly int TotalWidth = OffsetSize + EntryWidth + OffsetSize + SetWidth + OffsetSize;

        private static readonly int BackWidth = BorderSize + TotalWidth + BorderSize;

        private int m_Page;

        public ViewGuildsGump(Mobile owner, List<Guild> guilds) : this(guilds, 0)
        {
            owner.CloseGump(typeof(ViewGuildsGump));
        }

        public ViewGuildsGump(List<Guild> guilds, int page) : base(GumpOffsetX, GumpOffsetY)
        {
            m_Guilds = guilds;

            Initialize(page);
        }

        public void Initialize(int page)
        {
            m_Page = page;

            int count = m_Guilds.Count - (page * EntryCount);

            if (count < 0)
                count = 0;
            else if (count > EntryCount)
                count = EntryCount;

            int totalHeight = OffsetSize + ((EntryHeight + OffsetSize) * (count + 1));

            AddPage(0);

            AddBackground(0, 0, BackWidth, BorderSize + totalHeight + BorderSize, BackGumpID);
            AddImageTiled(BorderSize, BorderSize, TotalWidth, totalHeight, OffsetGumpID);

            int x = BorderSize + OffsetSize;
            int y = BorderSize + OffsetSize;

            int emptyWidth = TotalWidth - PrevWidth - NextWidth - (OffsetSize * 4);

            AddImageTiled(x, y, emptyWidth, EntryHeight, EntryGumpID);

            AddLabel(x + TextOffsetX, y, TextHue, String.Format("Page {0} of {1} ({2})", page + 1, (m_Guilds.Count + EntryCount - 1) / EntryCount, m_Guilds.Count));

            x += emptyWidth + OffsetSize;

            AddImageTiled(x, y, PrevWidth, EntryHeight, HeaderGumpID);

            if (page > 0)
            {
                AddButton(x + PrevOffsetX, y + PrevOffsetY, PrevButtonID1, PrevButtonID2, 1, GumpButtonType.Reply, 0);

                if (PrevLabel)
                    AddLabel(x + PrevLabelOffsetX, y + PrevLabelOffsetY, TextHue, "Previous");
            }

            x += PrevWidth + OffsetSize;

            AddImageTiled(x, y, NextWidth, EntryHeight, HeaderGumpID);

            if ((page + 1) * EntryCount < m_Guilds.Count)
            {
                AddButton(x + NextOffsetX, y + NextOffsetY, NextButtonID1, NextButtonID2, 2, GumpButtonType.Reply, 1);

                if (NextLabel)
                    AddLabel(x + NextLabelOffsetX, y + NextLabelOffsetY, TextHue, "Next");
            }

            for (int i = 0, index = page * EntryCount; i < EntryCount && index < m_Guilds.Count; ++i, ++index)
            {
                x = BorderSize + OffsetSize;
                y += EntryHeight + OffsetSize;

                Guild g = m_Guilds[index];

                AddImageTiled(x, y, EntryWidth, EntryHeight, EntryGumpID);
                AddLabelCropped(x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, GetHueFor(g), g == null || g.Disbanded ? "(deleted)" : g.Name + " [" + g.Abbreviation + "]");

                x += EntryWidth + OffsetSize;

                if (SetGumpID != 0)
                    AddImageTiled(x, y, SetWidth, EntryHeight, SetGumpID);

                if (g != null && !g.Disbanded)
                    AddButton(x + SetOffsetX, y + SetOffsetY, SetButtonID1, SetButtonID2, i + 3, GumpButtonType.Reply, 0);
            }
        }

        private static int GetHueFor(Guild g)
        {
            switch (g.Type)
            {
                case GuildType.Regular:
                    return 99;
                case GuildType.Order:
                    return 0x3F;
                case GuildType.Chaos:
                    return 0x90;
                default:
                    return 99;
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            switch (info.ButtonID)
            {
                case 0: // Closed
                    {
                        return;
                    }
                case 1: // Previous
                    {
                        if (m_Page > 0)
                            from.SendGump(new ViewGuildsGump(m_Guilds, m_Page - 1));

                        break;
                    }
                case 2: // Next
                    {
                        if ((m_Page + 1) * EntryCount < m_Guilds.Count)
                            from.SendGump(new ViewGuildsGump(m_Guilds, m_Page + 1));

                        break;
                    }
                default:
                    {
                        int index = (m_Page * EntryCount) + (info.ButtonID - 3);

                        if (index >= 0 && index < m_Guilds.Count)
                        {
                            Guild g = m_Guilds[index];

                            if (g == null || g.Disbanded)
                                from.SendMessage("That guild was removed.");
                            else
                                from.SendGump(new ViewGuildGump(from, g));

                            from.SendGump(new ViewGuildsGump(m_Guilds, m_Page));
                        }

                        break;
                    }
            }
        }
    }
}