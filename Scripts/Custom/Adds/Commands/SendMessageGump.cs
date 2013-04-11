using System;
using Server.Engines.Help;
using Server.Network;

namespace Server.Gumps
{
    public class SendMessageGump : Gump
    {
        private readonly Mobile m_From;
        private readonly string m_Name;
        private readonly string m_Text;

        public SendMessageGump(Mobile from, string name, string text) : base(0, 0)
        {
            m_From = from;
            m_Name = name;
            m_Text = text;

            AddBackground(50, 25, 540, 430, 2600);

            AddPage(0);

            AddHtml(150, 40, 360, 40, "<center><u>Private Message</u></center>", false, false);

            AddHtml(80, 90, 480, 290, String.Format("Message from {0}: {1}", name, text), true, true);

            AddHtml(80, 390, 480, 40, "Clicking the OKAY button will remove the message you have received", false, false);

            AddButton(400, 417, 2074, 2075, 1, GumpButtonType.Reply, 0); // OKAY

            AddButton(475, 417, 2073, 2072, 0, GumpButtonType.Reply, 0); // CANCEL
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID != 1)
                m_From.SendGump(new MessageSentGump(m_From, m_Name, m_Text, false));
        }
    }
}