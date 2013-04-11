using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Custom.Games
{

    public enum GameInfoGumpType
    {
        Extended,
        Compact,
        Disabled
    }

    public class GameInfoGump : Gump
    {
        private const int LabelHue = 0x480;
        private const int GreenHue = 0x40;
        private const int GreyHue = 926;

        private readonly BaseGame m_Game;
        private readonly Mobile m_User;

        private static Dictionary<PlayerMobile, List<CustomGumpItem>> m_CustomPlayerFields = new Dictionary<PlayerMobile,List<CustomGumpItem>>();
        private static Dictionary<BaseGame, List<CustomGumpItem>> m_CustomGameFields = new Dictionary<BaseGame,List<CustomGumpItem>>();

        private int m_Y = 0;
        private int m_Height = 0;

        public GameInfoGump(Mobile m, BaseGame game)
            : base(200, 200)
        {
            m_Game = game;
            m_User = m;

            if (m_User.HasGump(this.GetType()))
            {
                m_User.CloseGump(this.GetType());
            }

            if (m is PlayerMobile)
            {
                PlayerMobile user = m as PlayerMobile;
                if (user.GameInfoGumpType == GameInfoGumpType.Extended)
                    MakeGump(false);
                else if (user.GameInfoGumpType == GameInfoGumpType.Compact)
                    MakeCompactGump();
            }
            else
            {
                MakeGump(false);
            }
        }

        public GameInfoGump(Mobile m, BaseGame game, bool endgame)
            : base(200, 200)
        {
            m_Game = game;
            m_User = m;

            if (m_User.HasGump(this.GetType()))
            {
                m_User.CloseGump(this.GetType());
            }
            
            MakeGump(endgame);
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

        public static void AddCustomMessage(PlayerMobile player, CustomGumpItem item)
        {
            if (m_CustomPlayerFields.ContainsKey(player))
                m_CustomPlayerFields[player].Add(item);
            else
            {
                List<CustomGumpItem> list = new List<CustomGumpItem>();
                list.Add(item);
                m_CustomPlayerFields.Add(player, list);
            }
            item.Game.SendPlayerGump(player);
        }

        public static void AddCustomMessage(BaseGame game, CustomGumpItem item)
        {
            if (m_CustomGameFields.ContainsKey(game))
                m_CustomGameFields[game].Add(item);
            else
            {
                List<CustomGumpItem> list = new List<CustomGumpItem>();
                list.Add(item);
                m_CustomGameFields.Add(game, list);
            }
            game.SendPlayerGumps();
        }

        public static void RemoveCustomMessage(PlayerMobile player, CustomGumpItem item)
        {
            if(m_CustomPlayerFields.ContainsKey(player))
                m_CustomPlayerFields[player].Remove(item);
            item.Game.SendPlayerGump(player);
        }

        public static void RemoveCustomMessage(BaseGame game, CustomGumpItem item)
        {
            if (m_CustomGameFields.ContainsKey(game))
                m_CustomGameFields[game].Remove(item);
            game.SendPlayerGumps();
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

        private void MakeGump(bool endgame)
        {
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            int x = 30;
            Y = 15;

            AddLabel(x, Y, GreenHue, Game.GameName);

            if(m_Game.Running)
                AddLabel(x + 170, Y, LabelHue, "The game is currently running.");
            else if(m_Game.Open)
                AddLabel(x + 170, Y, LabelHue, "The game is currently open.");
            else if(endgame)
                AddLabel(x + 170, Y, LabelHue, "The game has ended.");
            else
                AddLabel(x + 170, Y, LabelHue, "The game is currently closed.");

            Y = 45;
            AddLabel(x, Y, GreenHue, "Time left: ");
            AddLabel(x + 90, Y, LabelHue, m_Game.TimeLeft.Hours + ":" + m_Game.TimeLeft.Minutes + ":" + m_Game.TimeLeft.Seconds);
            Y += 20;
            AddLabel(x, Y, GreenHue, "Max score: ");
            AddLabel(x + 90, Y, LabelHue, m_Game.MaxScore.ToString());

            Y = 45;
            x += 170;
            if(m_CustomGameFields.ContainsKey(Game))
            {
                foreach (CustomGumpItem item in m_CustomGameFields[Game])
                {
                    AddLabel(x, Y, GreenHue, item.Label);
                    AddLabel(x + 90, Y, LabelHue, item.Message);
                    Y += 20;
                }
            }
            if (User is PlayerMobile && m_CustomPlayerFields.ContainsKey((PlayerMobile)User))
            {
                foreach (CustomGumpItem item in m_CustomPlayerFields[(PlayerMobile)User])
                {
                    AddLabel(x, Y, GreenHue, item.Label);
                    AddLabel(x + 90, Y, LabelHue, item.Message);
                    Y += 20;
                }
            }
            x -= 170;


            int teamy = 0;
            if (m_Game is BaseTeamGame)
            {
                BaseTeamGame TeamGame = m_Game as BaseTeamGame;
                foreach(BaseGameTeam team in TeamGame.Teams)
                {
                    if (Y < 105)
                        teamy = 105;
                    else
                        teamy = Y;
                    AddLabel(x, teamy, team.Hue, team.Name + ":");
                    AddLabel(x + 100, teamy, team.Hue, team.Score.ToString());
                    teamy += 20;
                    foreach (Mobile player in team.Players)
                    {
                        if(player.SolidHueOverride != -1)
                            AddLabel(x, teamy, player.SolidHueOverride, player.Name);
                        else if(!player.Alive)
                            AddLabel(x, teamy, GreyHue, player.Name);
                        else
                            AddLabel(x, teamy, LabelHue, player.Name);
                        AddLabel(x + 100, teamy, LabelHue, Game.GetPlayerScore(player).ToString());
                        teamy += 20;
                    }
                    x += 170;
                }
            }

            Y += teamy + 30;
            x = 30;
            if (!endgame)
            {
                AddButton(x, Y, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0);
                AddLabel(x + 30, Y, LabelHue, "Disable this gump");
                AddButton(x + 160, Y, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0);
                AddLabel(x + 190, Y, LabelHue, "Compact gump");
            }

            int width = 220;
            int height = 230;
            if (endgame)
                height -= 30;
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

                AddAlphaRegion(0, 0, width, Height + 40);
                AddBackground(0, 0, width, Height + 40, 9250);
            }
            else
            {
                AddAlphaRegion(0, 0, width, Height + 40);
                AddBackground(0, 0, width, Height + 40, 9250);
            }

            Entries.Reverse();
            Entries.Reverse(2, Entries.Count - 2);
        }

        private void MakeCompactGump()
        {
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            if (Game is BaseTeamGame)
            {
                AddAlphaRegion(0, 0, 300, 90 + ((BaseTeamGame)Game).TeamCount * 20);
                AddBackground(0, 0, 300, 90 + ((BaseTeamGame)Game).TeamCount * 20, 9250);
            }
            else
            {
                AddAlphaRegion(0, 0, 300, 90);
                AddBackground(0, 0, 300, 90, 9250);
            }

            AddLabel(15, 13, GreenHue, Game.GameName);

            if (m_Game.Running)
                AddLabel(170, 13, LabelHue, "Game is running.");
            else if (m_Game.Open)
                AddLabel(170, 13, LabelHue, "Game is open.");
            else
                AddLabel(170, 13, LabelHue, "Game is closed.");

            AddLabel(15, 45, GreenHue, "Time left: ");
            AddLabel(105, 45, LabelHue, m_Game.TimeLeft.Hours + ":" + m_Game.TimeLeft.Minutes + ":" + m_Game.TimeLeft.Seconds);
            AddLabel(15, 65, GreenHue, "Max score: ");
            AddLabel(105, 65, LabelHue, m_Game.MaxScore.ToString());
            int x = 170;
            int y = 45;
            if (m_Game is BaseTeamGame)
            {
                BaseTeamGame TeamGame = m_Game as BaseTeamGame;
                foreach (BaseGameTeam team in TeamGame.Teams)
                {
                    AddLabel(x, y, team.Hue, team.Name + ":");
                    AddLabel(x + 100, y, team.Hue, team.Score.ToString());
                    y += 20;
                }
            }

            y += 10;

            AddButton(15, y, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0);
            AddLabel(45, y, LabelHue, "Disable");
            AddButton(105, y, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0);
            AddLabel(135, y, LabelHue, "Extend");
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            if (sender.Mobile is PlayerMobile)
            {
                PlayerMobile from = sender.Mobile as PlayerMobile;
                switch (info.ButtonID)
                {
                    case 1:
                        {
                               from.GameInfoGumpType = GameInfoGumpType.Disabled;
                            break;
                        }
                    case 2:
                        {
                            if(from.GameInfoGumpType == GameInfoGumpType.Extended)
                                from.GameInfoGumpType = GameInfoGumpType.Compact;
                            else
                                from.GameInfoGumpType = GameInfoGumpType.Extended;
                            m_Game.SendPlayerGump(from);
                            break;
                        }
                }
            }
        }

    }
}
