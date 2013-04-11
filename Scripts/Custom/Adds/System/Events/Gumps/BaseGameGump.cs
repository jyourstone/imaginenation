using System;
using Server;
using Server.Gumps;
using Server.Items;

namespace Server.Custom.Games
{
    public class BaseGameGump : Gump
    {
        private const int LabelHue = 0x480;
        private const int GreenHue = 0x40;

        private readonly BaseGame m_Game;
        private readonly Mobile m_User;

        public BaseGameGump(Mobile m, BaseGame game)
            : base(200, 200)
        {
            m_Game = game;
            m_User = m;

            if (m_User == null)
                return;
            if (m_User.HasGump(this.GetType()))
            {
                m_User.CloseGump(this.GetType());
            }

            MakeGump();
        }

        private void MakeGump()
        {
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 550, 200, 9250);
            AddAlphaRegion(0, 0, 550, 200);

            if (!m_Game.Open)
            {
                AddButton(35, 45, 5601, 5605, 4, GumpButtonType.Reply, 0);
                AddLabel(60, 43, LabelHue, "Open game");
            }
            else
            {
                AddButton(35, 45, 5601, 5605, 5, GumpButtonType.Reply, 0);
                AddLabel(60, 43, LabelHue, "Close game");
            }

            if (m_Game.Open && !m_Game.Running)
            {
                AddButton(35, 65, 5601, 5605, 6, GumpButtonType.Reply, 0);
                AddLabel(60, 63, LabelHue, "Start game");
            }
            else if (m_Game.Running)
            {
                AddButton(35, 65, 5601, 5605, 7, GumpButtonType.Reply, 0);
                AddLabel(60, 63, LabelHue, "End game");
            }

            AddButton(35, 105, 5601, 5605, 1, GumpButtonType.Reply, 0);
            AddLabel(60, 103, LabelHue, "Edit props");


            AddButton(35, 145, 5601, 5605, 2, GumpButtonType.Reply, 0);
            AddLabel(60, 143, LabelHue, "Edit supplier props");
            if (m_Game.Supplier != null)
            {
                if (m_Game.Supplier.SupplyType == SupplyType.Custom)
                {
                    AddButton(35, 165, 5601, 5605, 3, GumpButtonType.Reply, 0);
                    AddLabel(60, 163, LabelHue, "Edit supplies");
                }
            }

            AddLabel(60, 13, GreenHue, "Game stone gump");

            if(m_Game.Running)
                AddLabel(250, 13, LabelHue, "The game is currently running.");
            else if(m_Game.Open)
                AddLabel(250, 13, LabelHue, "The game is currently open.");
            else
                AddLabel(250, 13, LabelHue, "The game is currently closed.");


            AddButton(250, 45, 5601, 5605, 8, GumpButtonType.Reply, 0);
            AddLabel(280, 43, LabelHue, "Game administration");

            AddLabel(250, 65, GreenHue, "Time left: ");
            AddLabel(350, 65, LabelHue, m_Game.TimeLeft.Hours + ":" + m_Game.TimeLeft.Minutes + ":" + m_Game.TimeLeft.Seconds);
            AddLabel(250, 85, GreenHue, "Max score: ");
            AddLabel(350, 85, LabelHue, m_Game.MaxScore.ToString());
            if (m_Game is BaseTeamGame)
            {
                int y = 105;
                BaseTeamGame TeamGame = m_Game as BaseTeamGame;
                foreach(BaseGameTeam team in TeamGame.Teams)
                {
                    AddLabel(250, y, GreenHue, team.Name + ":");
                    AddLabel(350, y, LabelHue, team.Score.ToString());
                    y += 20;
                }
            }

        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            switch (info.ButtonID)
            {
                case 1:
                    {
                        from.SendGump(new PropertiesGump(m_User, m_Game));
                        m_Game.OnDoubleClick(m_User);
                        break;
                    }
                case 2:
                    {
                        from.SendGump(new PropertiesGump(m_User, m_Game.Supplier));
                        m_Game.OnDoubleClick(m_User);
                        break;
                    }
                case 3:
                    {
                        m_Game.Supplier.OnDoubleClick(m_User);
                        m_Game.OnDoubleClick(m_User);
                        break;
                    }
                case 4:
                    {
                        m_Game.Open = true;
                        m_Game.OnDoubleClick(m_User);
                        break;
                    }
                case 5:
                    {
                        m_Game.Open = false;
                        m_Game.OnDoubleClick(m_User);
                        break;
                    }
                case 6:
                    {
                        m_Game.StartCommand(m_User);
                        m_Game.OnDoubleClick(m_User);
                        break;
                    }
                case 7:
                    {
                        m_Game.EndGameCommand();
                        m_Game.OnDoubleClick(m_User);
                        break;
                    }
                case 8:
                    {
                        from.SendGump(new GameAdminGump(from, m_Game));
                        m_Game.OnDoubleClick(m_User);
                        break;
                    }
            }
        }

    }
}
