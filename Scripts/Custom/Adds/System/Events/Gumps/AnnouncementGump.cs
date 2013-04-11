using System;
using Server;
using Server.Gumps;
using Server.Items;

namespace Server.Custom.Games
{
    /// <summary>
    /// Summary description for ChessSetGump.
    /// </summary>
    public class AnnouncementGump : Gump
    {
        private const int LabelHue = 0x480;
        private const int GreenHue = 0x40;

        public AnnouncementGump(Mobile m)
            : base(200, 200)
        {
            MakeGump();
        }

        private void MakeGump()
        {
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddLabel(60, 13, GreenHue, "Announcement gump");

            AddBackground(0, 0, 320, 170, 9250);
            AddAlphaRegion(0, 0, 320, 170);

            foreach (Announcement ann in Events.m_Announcements)
            {
                AddButton(35, 45, 5601, 5605, 1, GumpButtonType.Reply, 0);
                AddLabel(60, 43, LabelHue, "Edit props");
            }


            AddButton(35, 85, 5601, 5605, 2, GumpButtonType.Reply, 0);
            AddLabel(60, 83, LabelHue, "Edit supplier props");

        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            switch (info.ButtonID)
            {
                default:
                    break;
            }
        }

    }
}
