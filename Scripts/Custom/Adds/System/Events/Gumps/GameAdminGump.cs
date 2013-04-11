using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Games
{
    public class GameAdminGump : Gump
    {
        private const int LabelHue = 0x480;
        private const int GreenHue = 0x40;
        private const int GreyHue = 926;

        private readonly BaseGame m_Game;
        private readonly Mobile m_User;

        private int m_Y = 0;
        private int m_Height = 0;

        private int m_Hours;
        private int m_Minutes;
        private int m_Seconds;

        public GameAdminGump(Mobile m, BaseGame game)
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

        private new int Y
        {
            get
            {
                return m_Y;
            }
            set
            {
                m_Y = value;
                if (m_Y > Height)
                    Height = m_Y + 5;
            }
        }

        private int Height
        {
            get
            {
                return m_Height;
            }
            set
            {
                m_Height = value;
            }
        }

        public BaseGame Game
        {
            get
            {
                return m_Game;
            }
        }

        public Mobile User
        {
            get
            {
                return m_User;
            }
        }

        private void MakeGump()
        {
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            int x = 30;
            Y = 15;

            AddLabel(x, Y, GreenHue, Game.GameName);

            if (m_Game.Running)
                AddLabel(x + 170, Y, LabelHue, "The game is currently running.");
            else if (m_Game.Open)
                AddLabel(x + 170, Y, LabelHue, "The game is currently open.");
            else
                AddLabel(x + 170, Y, LabelHue, "The game is currently closed.");

            m_Hours = m_Game.TimeLeft.Hours;
            m_Minutes = m_Game.TimeLeft.Minutes;
            m_Seconds = m_Game.TimeLeft.Seconds;

            Y = 45;
            AddLabel(x, Y, GreenHue, "Time left: ");
            AddImageTiled(x + 100, Y, 30, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
            AddTextEntry(x + 100, Y, 30, PropsConfig.EntryHeight, PropsConfig.TextHue, 0, "" + m_Game.TimeLeft.Hours);
            AddImageTiled(x + 135, Y, 30, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
            AddTextEntry(x + 135, Y, 30, PropsConfig.EntryHeight, PropsConfig.TextHue, 1, "" + m_Game.TimeLeft.Minutes);
            AddImageTiled(x + 170, Y, 30, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
            AddTextEntry(x + 170, Y, 30, PropsConfig.EntryHeight, PropsConfig.TextHue, 2, "" + m_Game.TimeLeft.Seconds);
            Y += 20;
            AddLabel(x, Y, GreenHue, "Max score: ");
            AddImageTiled(x + 100, Y, 30, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
            AddTextEntry(x + 100, Y, 30, PropsConfig.EntryHeight, PropsConfig.TextHue, 3, "" + m_Game.MaxScore);


            int teamy = 0;
            if (m_Game is BaseTeamGame)
            {
                int count = 4;
                BaseTeamGame TeamGame = m_Game as BaseTeamGame;
                foreach (BaseGameTeam team in TeamGame.Teams)
                {
                    if (Y < 105)
                        teamy = 105;
                    else
                        teamy = Y;
                    AddLabel(x, teamy, team.Hue, team.Name + ":");

                    AddImageTiled(x + 100, teamy, 30, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
                    AddTextEntry(x + 100, teamy, 30, PropsConfig.EntryHeight, PropsConfig.TextHue, count++, "" + team.Score);
                    
                    teamy += 20;
                    foreach (Mobile player in team.Players)
                    {
                        if (player.SolidHueOverride != -1)
                            AddLabel(x, teamy, player.SolidHueOverride, player.Name);
                        else if (!player.Alive)
                            AddLabel(x, teamy, GreyHue, player.Name);
                        else
                            AddLabel(x, teamy, LabelHue, player.Name);
                        AddImageTiled(x + 100, teamy, 30, PropsConfig.EntryHeight, PropsConfig.EntryGumpID);
                        AddTextEntry(x + 100, teamy, 30, PropsConfig.EntryHeight, PropsConfig.TextHue, player.Serial, "" + Game.GetPlayerScore(player).ToString());
                        teamy += 20;
                    }
                    x += 170;
                }
            }

            Y += teamy + 30;
            x = 30;

            AddButton(x, Y, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0);
            AddLabel(x + 30, Y, LabelHue, "Save changes");

            int width = 220;
            int height = 210;

            if (m_Game is BaseTeamGame)
            {
                BaseTeamGame TeamGame = m_Game as BaseTeamGame;
                int maxcount = 0;
                foreach (BaseGameTeam team in TeamGame.Teams)
                {
                    if (team.Players.Count > maxcount)
                        maxcount = team.Players.Count;
                }
                height += maxcount * 20;
                width += TeamGame.Teams.Count * 170 - 170;
                AddAlphaRegion(0, 0, width, Height);
                AddBackground(0, 0, width, Height, 9250);
            }
            else
            {
                AddAlphaRegion(0, 0, width, Height);
                AddBackground(0, 0, width, Height, 9250);
            }
            Entries.Reverse();
            Entries.Reverse(2, Entries.Count - 2);
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            switch (info.ButtonID)
            {
                case 1:
                    {
                        int hour, min, sec, score;
                        if (Int32.TryParse(info.GetTextEntry(0).Text, out hour) && Int32.TryParse(info.GetTextEntry(1).Text, out min) && Int32.TryParse(info.GetTextEntry(2).Text, out sec))
                        {
                            if(hour != m_Hours || min != m_Minutes || sec != m_Seconds)
                                Game.Length = new TimeSpan(hour, min, sec);
                        }
                        if(Int32.TryParse(info.GetTextEntry(3).Text, out score))
                        {
                            Game.MaxScore = score;
                        }
                        if (Game is BaseTeamGame)
                        {
                            int count = 4;
                            BaseTeamGame teamgame = Game as BaseTeamGame;
                            foreach (BaseGameTeam team in teamgame.Teams)
                            {
                                if (info.GetTextEntry(count) != null && Int32.TryParse(info.GetTextEntry(count++).Text, out score))
                                {
                                    team.Score = score;
                                }
                                foreach (Mobile player in team.Players)
                                {
                                    if (info.GetTextEntry(player.Serial) != null && Int32.TryParse(info.GetTextEntry(player.Serial).Text, out score))
                                    {
                                        Game.SetPlayerScore(player, score);
                                    }
                                }
                            }
                        }
                        from.SendGump(new GameAdminGump(from, m_Game));
                        break;
                    }
            }
        }

    }
}
