using System;
using System.Collections.Generic;
using System.Threading;
using Server.Commands;
using Server.Network;

namespace Server.Gumps
{
    public class GuildListGump_ : Gump
    {
        private static Mobile m_Mobile;

        public static void Initialize()
        {
            CommandSystem.Register("GuildList", AccessLevel.Player, On_OnCommand);
            CommandSystem.Register("GList", AccessLevel.Player, On_OnCommand);
            CommandSystem.Register("GL", AccessLevel.Player, On_OnCommand);
        }

        [Usage("GuildList")]
        [Aliases("GList", "GL")]
        [Description("Displays a list of guild members and their locations.")]
        private static void On_OnCommand(CommandEventArgs e)
        {
            m_Mobile = e.Mobile;

            if (m_Mobile.Guild == null || m_Mobile.Guild.Disbanded)
            {
                m_Mobile.SendAsciiMessage("You are not in a guild.");
                return;
            }

            new Thread(ThreadedList).Start();
        }

        private static void ThreadedList()
        {
            m_Mobile.SendGump(new GuildListGump_(m_Mobile));
        }

        public static readonly int GumpOffsetX = 30;
        public static readonly int GumpOffsetY = 30;

        public static readonly int TextHue = 0;
        public static readonly int TextOffsetX = -2;

        public static readonly int OffsetGumpID = 0x0A40; // Pure black
        public static readonly int HeaderGumpID = 0x0E14; // Dark navy blue, textured
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

        private static readonly int EntryWidth = 160;
        private static readonly int EntryCount = 15;

        private static readonly int RegionWidth = 180;
        private static readonly int CoordinatesWidth = 110;

        private static readonly int TotalWidth = OffsetSize + EntryWidth + OffsetSize + SetWidth + OffsetSize + RegionWidth + CoordinatesWidth;

        private static readonly int BackWidth = BorderSize + TotalWidth + BorderSize;

        private readonly Mobile m_Owner;
        private readonly List<Mobile> m_Mobiles;
        private int m_Page;

        private class InternalComparer : IComparer<Mobile>
        {
            public static readonly IComparer<Mobile> Instance = new InternalComparer();

            public int Compare(Mobile x, Mobile y)
            {
                if (x == null || y == null)
                    throw new ArgumentException();

                if (x.AccessLevel > y.AccessLevel)
                    return -1;
                if (x.AccessLevel < y.AccessLevel)
                    return 1;
                return Insensitive.Compare(x.Name, y.Name);
            }
        }

        public GuildListGump_(Mobile owner) : this(owner, BuildList(owner), 0)
        {
            owner.CloseGump(typeof(GuildListGump_));
        }

        public GuildListGump_(Mobile owner, List<Mobile> list, int page) : base(GumpOffsetX, GumpOffsetY)
        {
            m_Owner = owner;
            m_Mobiles = list;

            Initialize(page);
        }

        public static List<Mobile> BuildList(Mobile owner)
        {
            List<Mobile> list = new List<Mobile>();
            List<NetState> states = NetState.Instances;

            for (int i = 0; i < states.Count; ++i)
            {
                Mobile m = states[i].Mobile;

                if (m == null || m.Guild == null || m.Guild.Disbanded)
                    continue;

                if (m.Guild == owner.Guild)
                    list.Add(m);
            }

            list.Sort(InternalComparer.Instance);

            return list;
        }

        public void Initialize(int page)
        {
            m_Page = page;

            int count = m_Mobiles.Count - (page * EntryCount);

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

            int emptyWidth = TotalWidth - RegionWidth - CoordinatesWidth - PrevWidth - NextWidth - (OffsetSize * 4);

            AddImageTiled(x, y, emptyWidth - 20, EntryHeight, EntryGumpID);

            AddLabel(x + TextOffsetX + 4, y, TextHue, String.Format("Page {0} of {1} ({2})", page + 1, (m_Mobiles.Count + EntryCount - 1) / EntryCount, m_Mobiles.Count));

            x += emptyWidth + OffsetSize;

            AddImageTiled(x - 19, y, NextWidth + 61, EntryHeight, HeaderGumpID); //Next/Previous buttons background

            if (page > 0)
                AddButton(x + PrevOffsetX - 20, y + PrevOffsetY, PrevButtonID1, PrevButtonID2, 1, GumpButtonType.Reply, 0);

            x += PrevWidth + OffsetSize;

            if ((page + 1) * EntryCount < m_Mobiles.Count)
                AddButton(x + NextOffsetX - 20, y + NextOffsetY, NextButtonID1, NextButtonID2, 2, GumpButtonType.Reply, 1);

            AddImageTiled(x + SetOffsetX, y, RegionWidth, EntryHeight, EntryGumpID); //Region text background
            AddLabel(x + SetOffsetX + 3, y, TextHue, "Region");

            AddImageTiled(x + SetOffsetX + RegionWidth + 1, y, CoordinatesWidth + 17, EntryHeight, EntryGumpID); //Coordinates text background
            AddLabel(x + SetOffsetX + RegionWidth + 3, y, TextHue, "Coordinates");

            for (int i = 0, index = page * EntryCount; i < EntryCount && index < m_Mobiles.Count; ++i, ++index)
            {
                x = BorderSize + OffsetSize;
                y += EntryHeight + OffsetSize;

                Mobile m = m_Mobiles[index];

                AddImageTiled(x, y, EntryWidth + 2, EntryHeight, EntryGumpID);
                AddLabelCropped(x + TextOffsetX + 4, y, EntryWidth - TextOffsetX, EntryHeight, Notoriety.GetHue(Notoriety.Compute(m_Owner, m)), m.Deleted ? "(deleted)" : m.Name);

                x += EntryWidth + OffsetSize;

                if (m.NetState != null && !m.Deleted)
                {
                    AddImageTiled(x + SetOffsetX, y, RegionWidth, EntryHeight, EntryGumpID);
                    AddLabelCropped(x + SetOffsetX + 3, y, RegionWidth, EntryHeight, TextHue, GetRegion(m));

                    AddImageTiled(x + SetOffsetX + RegionWidth + 1, y, CoordinatesWidth + 17, EntryHeight, EntryGumpID);
                    AddLabelCropped(x + SetOffsetX + RegionWidth + 3, y, CoordinatesWidth, EntryHeight, TextHue, m.Deleted || m.Hidden ? "" : m.Location.ToString());
                }
            }
        }

        private static string GetRegion(Mobile m)
        {
            if (m == null || m.Deleted)
                return "deleted";

            string reg = !string.IsNullOrEmpty(m.Region.Name) ? m.Region.Name : m.Region.ToString();

            if (reg == "Region")
                reg = "Britannia";

            return reg;
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
                            from.SendGump(new ShowPlayersGump(from, m_Mobiles, m_Page - 1));

                        break;
                    }
                case 2: // Next
                    {
                        if ((m_Page + 1) * EntryCount < m_Mobiles.Count)
                            from.SendGump(new ShowPlayersGump(from, m_Mobiles, m_Page + 1));

                        break;
                    }
            }
        }
    }
}