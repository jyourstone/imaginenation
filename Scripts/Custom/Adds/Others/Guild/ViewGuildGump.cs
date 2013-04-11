using System;
using Server.Guilds;
using Server.Network;

namespace Server.Gumps
{
    public class ViewGuildGump : Gump
    {
        private readonly Mobile m_From;
        private readonly Guild m_Guild;
        private readonly Map m_StoneMap;
        private readonly Point3D m_StoneLocation;

        public ViewGuildGump(Mobile from, Guild guild) : base(50, 40)
        {
            if (guild == null || guild.Disbanded)
            {
                from.SendAsciiMessage("That guild was removed");
                return;
            }

            m_From = from;
            m_Guild = guild;
            m_StoneMap = m_Guild.Guildstone.Map;
            m_StoneLocation = m_Guild.Guildstone.Location;

            from.CloseGump(typeof(ViewGuildGump));

            AddPage(0);

            AddBackground(0, 0, 240, 230, 5054);
            AddBlackAlpha(10, 10, 220, 210);

            string guildName = m_Guild.Name;
            string guildAbb = m_Guild.Abbreviation;
            string guildId = m_Guild.Id.ToString();
            string guildMaster = m_Guild.Leader.Name;
            string members = m_Guild.Members.Count.ToString();
            string type = m_Guild.Type.ToString();

            AddHtml(10, 15, 220, 20, Color(Center("Guild Properties"), White), false, false);

            AddHtml(15, 40, 210, 20, Color("Name:", White), false, false);
            AddHtml(15, 40, 210, 20, Color(Right(guildName), White), false, false);

            AddHtml(15, 60, 210, 20, Color("Abbreviation:", White), false, false);
            AddHtml(15, 60, 210, 20, Color(Right(guildAbb), White), false, false);

            AddHtml(15, 80, 210, 20, Color("Guildmaster:", White), false, false);
            AddHtml(15, 80, 210, 20, Color(Right(guildMaster), White), false, false);

            AddHtml(15, 100, 210, 20, Color("Type:", White), false, false);
            AddHtml(15, 100, 210, 20, Color(Right(type), White), false, false);

            AddHtml(15, 120, 210, 20, Color("Members:", White), false, false);
            AddHtml(15, 120, 210, 20, Color(Right(members), White), false, false);

            AddHtml(15, 140, 210, 20, Color("ID:", White), false, false);
            AddHtml(15, 140, 210, 20, Color(Right(guildId), White), false, false);

            AddButton(15, 165, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddHtml(50, 165, 120, 20, Color("Go to guildstone", White), false, false);

            AddButton(15, 185, 4005, 4007, 2, GumpButtonType.Reply, 0);
            AddHtml(50, 185, 160, 20, Color("Open guildstone menu", White), false, false);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case 1:
                    {
                        if (m_StoneMap != null && m_StoneMap != Map.Internal)
                            m_From.MoveToWorld(m_StoneLocation, m_StoneMap);

                        m_From.SendGump(new ViewGuildGump(m_From, m_Guild));

                        break;
                    }
                case 2:
                    {
                        m_From.SendGump(new ViewGuildGump(m_From, m_Guild));

                        Item stone = m_Guild.Guildstone;

                        if (stone != null && !stone.Deleted)
                            stone.OnDoubleClick(m_From);

                        break;
                    }
            }
        }

        private const int White = 0xFFFFFF;

        public string Right(string text)
        {
            return String.Format("<div align=right>{0}</div>", text);
        }

        public string Center(string text)
        {
            return String.Format("<CENTER>{0}</CENTER>", text);
        }

        public string Color(string text, int color)
        {
            return String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text);
        }

        public void AddBlackAlpha(int x, int y, int width, int height)
        {
            AddImageTiled(x, y, width, height, 2624);
            AddAlphaRegion(x, y, width, height);
        }
    }
}