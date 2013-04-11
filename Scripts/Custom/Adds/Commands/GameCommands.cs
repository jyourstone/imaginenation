using System;
using Server.Commands;
using Server.Custom.PvpToolkit;
using Server.Custom.PvpToolkit.DMatch.Items;
using Server.Targeting;

namespace Server.Custom.Games
{
    /*public class GameCommands
    {
        public static void Initialize()
        {
            CommandSystem.Register("GameKick", AccessLevel.Counselor, GameKick_Command);
            CommandSystem.Register("GameScore", AccessLevel.Counselor, GameScore_Command);
            CommandSystem.Register("EndGame", AccessLevel.Counselor, EndGame_Command);
            CommandSystem.Register("StartGame", AccessLevel.Counselor, StartGame_Command);
            CommandSystem.Register("ShowScore", AccessLevel.Counselor, ShowScore_Command);
            CommandSystem.Register("Team", AccessLevel.Player, TeamMessage_Command);
            CommandSystem.Register("T", AccessLevel.Player, TeamMessage_Command);
            CommandSystem.Register("Score", AccessLevel.Player, Score_Command);
        }

        [Usage("GameScore")]
        [Description("Announces the score to the players in the game")]
        private static void GameScore_Command(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, GameScore_Target);
            e.Mobile.SendMessage("Target the game control stone to announce scores.");
        }

        private static void GameScore_Target(Mobile from, object o)
        {
            if (o is CTFGame)
            {
                CTFGame game = (CTFGame)o;
                game.AnnounceScore();
            }
            else if (o is CWGame)
            {
                CWGame game = (CWGame) o;
                game.AnnounceScore();
            }
            else
            {
                from.BeginTarget(-1, false, TargetFlags.None, GameScore_Target);
                from.SendMessage("Target the game stone.");
            }
        }

        [Usage("ShowScore")]
        [Description("Tells you what the score in the current game is")]
        private static void ShowScore_Command(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, ShowScore_Target);
            e.Mobile.SendMessage("Target the game control stone to display scores.");
        }

        private static void ShowScore_Target(Mobile from, object o)
        {
            if (o is CTFGame)
            {
                CTFGame game = (CTFGame)o;

                if (game.Running)
                {
                    from.SendAsciiMessage(0x489, string.Format("Time left: {0:0}:{1:00}:{2:00}", (int)(game.TimeLeft.TotalSeconds / 60 / 60), (int)(game.TimeLeft.TotalSeconds / 60) % 60, (int)(game.TimeLeft.TotalSeconds) % 60));
                    for (int i = 0; i < game.Teams.Count; i++)
                    {
                        CTFTeam team = (CTFTeam)game.Teams[i];
                        from.SendAsciiMessage(0x489, string.Format("{0}: {1} points", team.Name, team.Points));
                    }
                }
                else
                    from.SendAsciiMessage("This CTF is not running");
            }
            else if (o is CWGame)
            {
                CWGame game = (CWGame)o;

                if (game.Running)
                {
                    from.SendAsciiMessage(0x489, string.Format("Time left: {0:0}:{1:00}:{2:00}", (int)(game.TimeLeft.TotalSeconds / 60 / 60), (int)(game.TimeLeft.TotalSeconds / 60) % 60, (int)(game.TimeLeft.TotalSeconds) % 60));
                    for (int i = 0; i < game.Teams.Count; i++)
                    {
                        CWTeam team = (CWTeam)game.Teams[i];
                        from.SendAsciiMessage(0x489, string.Format("{0}: {1} points", team.Name, team.Points));
                    }
                }
                else
                    from.SendAsciiMessage("This CW is not running");
            }
            else if (o is DMStone)
            {
                DMStone game = (DMStone)o;

                if (game.Started)
                    game.ShowScore(from);
                else
                    from.SendAsciiMessage("This deathmatch is not running");
            }
            else
            {
                from.BeginTarget(-1, false, TargetFlags.None, ShowScore_Target);
                from.SendMessage("Target the game stone.");
            }
        }

        [Usage("Team or T")]
        [Description("Sends a message to everyone in your team")]
        private static void TeamMessage_Command(CommandEventArgs e)
        {
            string msg = e.ArgString;
            if (msg == null)
                return;
            msg = msg.Trim();
            if (msg.Length <= 0)
                return;

            CTFTeam ctfteam = CTFGame.FindTeamFor(e.Mobile);
            CWTeam cwteam = CWGame.FindTeamFor(e.Mobile);

            if (ctfteam != null)
            {
                msg = String.Format("Team [{0}]: {1}", e.Mobile.Name, msg);
                for (int m = 0; m < ctfteam.Members.Count; m++)
                    ((Mobile)ctfteam.Members[m]).SendMessage(0x35, msg);
            }
            else if (cwteam != null)
            {
                msg = String.Format("Team [{0}]: {1}", e.Mobile.Name, msg);
                for (int m = 0; m < cwteam.Members.Count; m++)
                    ((Mobile)cwteam.Members[m]).SendMessage(0x35, msg);
            }
            else
                e.Mobile.SendMessage("You are not in a game.");
        }

        private static void GameKick_Command(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, GameKick_Target);
            e.Mobile.SendMessage("Target the player to be kicked from the game.");
        }

        [Usage("GameKick")]
        [Description("Kicks a player out of a game")]
        private static void GameKick_Target(Mobile from, object o)
        {
            if (o is Mobile)
            {
                Mobile m = (Mobile)o;
                CTFTeam ctfteam = CTFGame.FindTeamFor(m);
                CWTeam cwteam = CWGame.FindTeamFor(m);

                if (ctfteam != null)
                {
                    // Remove the player from game in here
                    ctfteam.Kick(m);

                    m.SendMessage("You have been kicked out of CTF game by {0}.", from.Name);
                    from.SendMessage("The player has been kicked.");
                }
                else if (cwteam != null)
                {
                    // Remove the player from game in here
                    cwteam.Kick(m);

                    m.SendMessage("You have been kicked out of Color Wars game by {0}.", from.Name);
                    from.SendMessage("The player has been kicked.");
                }
                else
                {
                    from.SendMessage("That player isn't in a game.");
                }
            }
            else
            {
                from.SendMessage("That is not a player.");
            }
        }

        [Usage("EndGame")]
        [Description("Ends the current game")]
        private static void EndGame_Command(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, EndGame_Target);
            e.Mobile.SendMessage("Target the game control stone to END a game.");
        }

        private static void EndGame_Target(Mobile from, object o)
        {
            if (o is CTFGame)
            {
                CTFGame game = (CTFGame)o;
                game.EndGame();
                from.SendMessage("The game has been ended.");
            }
            else if (o is CWGame)
            {
                CWGame game = (CWGame) o;
                game.EndGame();
                from.SendMessage("The game has been ended.");
            }
            else if (o is BombermanGame)
            {
                BombermanGame game = (BombermanGame)o;
                game.StopGame();
                from.SendMessage("The game has been ended.");
            }
            else if (o is DMStone)
            {
                DMStone game = (DMStone)o;
                game.Started = false;
            }
            else
            {
                from.BeginTarget(-1, false, TargetFlags.None, EndGame_Target);
                from.SendMessage("Target the game stone.");
            }
        }

        [Usage("StartGame")]
        [Description("Starts a game")]
        private static void StartGame_Command(CommandEventArgs e)
        {
            if (e.Arguments.Length < 1)
            {
                e.Mobile.SendMessage("Usage: startgame <ResetTeams>");
                e.Mobile.SendMessage("So, if you want to start the game and force everyone to choose a new team, do [startgame true");
            }

            string str = e.GetString(0).ToUpper().Trim();
            bool reset;
            if (str == "YES" || str == "TRUE" || str == "Y" || str == "T" || str == "1")
                reset = true;
            else
                reset = false;

            e.Mobile.BeginTarget(-1, false, TargetFlags.None, StartGame_Target, reset);
            e.Mobile.SendMessage("Target the game control stone to START a game.");
        }

        private static void StartGame_Target(Mobile from, object o, object state)
        {
            bool reset = state is bool ? (bool)state : false;

            if (o is CTFGame)
            {
                CTFGame game = (CTFGame)o;
                game.StartGame(reset);
                from.SendMessage("The game has been started.");
            }
            else if (o is CWGame)
            {
                CWGame game = (CWGame)o;
                game.StartGame(reset);
                from.SendMessage("The game has been started.");
            }
            else if (o is BombermanGame)
            {
                BombermanGame game = (BombermanGame)o;
                game.StartGame(from);
            }
            else if (o is DMStone)
            {
                DMStone game = (DMStone)o;
                game.Started = true;
            }
            else
            {
                from.BeginTarget(-1, false, TargetFlags.None, StartGame_Target, reset);
                from.SendMessage("Target the game stone.");
            }
        }

        [Usage("Score")]
        [Description("Shows the score of the game that you are in.")]
        private static void Score_Command(CommandEventArgs e)
        {
            CTFTeam ctfteam = CTFGame.FindTeamFor(e.Mobile);
            CWTeam cwteam = CWGame.FindTeamFor(e.Mobile);

            if (ctfteam!= null)
                ctfteam.Game.AnnounceScore(e.Mobile);
            else if (cwteam != null)
                cwteam.Game.AnnounceScore(e.Mobile);
            else if (PvpCore.IsInDeathmatch(e.Mobile))
                PvpCore.TryShowScoreBoard(e.Mobile);
            else
                e.Mobile.SendAsciiMessage("You are not in a game");
        }
    }*/
}
