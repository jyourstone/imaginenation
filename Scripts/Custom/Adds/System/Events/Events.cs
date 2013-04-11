using System;
using System.Collections.Generic;
using System.Text;
using Server.Custom.PvpToolkit;
using Server.Custom.PvpToolkit.DMatch.Items;
using Server.Custom.PvpToolkit.Tournament;
using Server.Mobiles;
using Server.Scripts.Custom.Adds.System.Database;
using System.Threading;
using Server.Commands;
using Server.Scripts.Custom.Adds.System;
using Server.Targeting;
using Server.Network;

namespace Server.Custom.Games
{
    public enum EventType
    {
        NoEvent,
        CaptureTheFlag,
        DeathMatch,
        SuppliedTournament,
        Tournament,
        Bomberman
    }

    public class Announcement
    {
        public string message;
        public Mobile staffmember;
        public TimeSpan interval;
        public DateTime endtime;
        public DateTime nextevent;

        public Announcement(string msg, Mobile staff, uint inter, DateTime end)
        {
            message = msg;
            staffmember = staff;
            interval = TimeSpan.FromSeconds(inter);
            endtime = end;
            nextevent = DateTime.Now + interval;
        }

        public void Announce()
        {
            if (message.Contains("{REMAINING}"))
            {
                int remainingseconds = (endtime - DateTime.Now).Seconds;
                if (remainingseconds > 3600)
                    message = message.Replace("{REMAINING}", remainingseconds / 3600 + " hours");
                else if (remainingseconds > 60)
                    message = message.Replace("{REMAINING}", remainingseconds / 60 + " minutes");
                else
                    message = message.Replace("{REMAINING}", remainingseconds + " seconds");
            }
            CommandHandlers.BroadcastMessage_OnCommand(new CommandEventArgs(staffmember, "", message, new string[0]));
        }
    }

    public class Events
    {
        #region Command Handlers
        [Usage("GameStart")]
        [Description("Starts a game")]
        private static void StartEvent_Command(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetStateCallback(StartEvent_Target), false);
            e.Mobile.SendMessage("Select the game stone to start the game.");
        }

        private static void StartEvent_Target(Mobile from, object o, object state)
        {
            if (o is BaseGame)
            {
                BaseGame game = (BaseGame)o;
                if (game.Open)
                {
                    try
                    {
                        game.StartCommand(from);
                    }
                    catch (EventException e)
                    {
                        from.SendMessage(e.ToString());
                    }
                }
                else
                {
                    from.SendMessage("You have to open the game first.");
                }
            }
            else if (o is DMStone)
            {
                DMStone game = (DMStone)o;
                game.Started = true;
            }
            else if (o is TournamentStone)
            {
                TournamentStone game = (TournamentStone)o;
                game.Started = true;
            }
            else
            {
                from.BeginTarget(-1, false, TargetFlags.None, new TargetStateCallback(StartEvent_Target), false);
                from.SendMessage("Target the game stone.");
            }
        }

        [Usage("GameEnd")]
        [Description("Ends the current game")]
        private static void StopEvent_Command(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetStateCallback(StopEvent_Target), false);
            e.Mobile.SendMessage("Select the game stone to stop the game.");
        }

        private static void StopEvent_Target(Mobile from, object o, object state)
        {
            if (o is BaseGame)
            {
                BaseGame game = (BaseGame)o;
                try
                {
                    game.EndGameCommand();
                    from.SendMessage("The game has been stopped.");
                }
                catch (EventException e)
                {
                    from.SendMessage(e.ToString());
                }
            }
            else if (o is DMStone)
            {
                DMStone game = (DMStone)o;
                game.Started = false;
            }
            else if (o is TournamentStone)
            {
                TournamentStone game = (TournamentStone)o;
                game.Started = false;
            }
            else
            {
                from.BeginTarget(-1, false, TargetFlags.None, new TargetStateCallback(StopEvent_Target), false);
                from.SendMessage("Target the game stone.");
            }
        }

        [Usage("GameOpen")]
        [Description("Opens a game for joiners")]
        private static void OpenEvent_Command(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetStateCallback(OpenEvent_Target), false);
            e.Mobile.SendMessage("Select the game stone to open for joiners.");
        }

        private static void OpenEvent_Target(Mobile from, object o, object state)
        {
            if (o is BaseGame)
            {
                BaseGame game = (BaseGame)o;
                if (!game.Open)
                {
                    game.Open = true;
                    from.SendMessage("The game has been opened.");
                }
                else
                {
                    from.SendMessage("The game is already open.");
                }
            }
            else if (o is DMStone)
            {
                DMStone game = (DMStone)o;
                if (game.Started)
                {
                    game.AcceptingContestants = true;
                    from.SendAsciiMessage("The game is now open for players");
                }
                else
                    from.SendAsciiMessage("You must start the game first");
            }
            else if (o is TournamentStone)
            {
                TournamentStone game = (TournamentStone)o;
                if (game.Started)
                {
                    game.AcceptingContestants = true;
                    from.SendAsciiMessage("The game is now open for players");
                }
                else
                    from.SendAsciiMessage("You must start the game first");
            }
            else
            {
                from.BeginTarget(-1, false, TargetFlags.None, new TargetStateCallback(OpenEvent_Target), false);
                from.SendMessage("Target the game stone.");
            }
        }

        [Usage("GameClose")]
        [Description("Closes a game for joiners")]
        private static void CloseEvent_Command(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetStateCallback(CloseEvent_Target), false);
            e.Mobile.SendMessage("Select the game stone to close for joiners.");
        }

        private static void CloseEvent_Target(Mobile from, object o, object state)
        {
            if (o is BaseGame)
            {
                BaseGame game = (BaseGame)o;
                if (game.Open)
                {
                    game.Open = false;
                    from.SendMessage("The game has been closed.");
                }
                else
                {
                    from.SendMessage("The game already closed.");
                }
            }
            else if (o is DMStone)
            {
                DMStone game = (DMStone)o;
                if (game.Started)
                {
                    game.AcceptingContestants = false;
                    from.SendAsciiMessage("The game is now closed for players");
                }
                else
                    from.SendAsciiMessage("This game is not running");
            }
            else if (o is TournamentStone)
            {
                TournamentStone game = (TournamentStone)o;
                if (game.Started)
                {
                    game.AcceptingContestants = false;
                    from.SendAsciiMessage("The game is now closed for players");
                }
                else
                    from.SendAsciiMessage("This game is not running");
            }
            else
            {
                from.BeginTarget(-1, false, TargetFlags.None, new TargetStateCallback(CloseEvent_Target), false);
                from.SendMessage("Target the game stone.");
            }
        }

        private static void ClearEvent_Command(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetStateCallback(ClearEvent_Target), false);
            e.Mobile.SendMessage("Select the game stone to clear all data in.");
        }

        private static void ClearEvent_Target(Mobile from, object o, object state)
        {
            if (o is BaseGame)
            {
                BaseGame game = (BaseGame)o;
                game.ClearGame();
                from.SendMessage("The game has been cleared.");
            }
            else
            {
                from.BeginTarget(-1, false, TargetFlags.None, new TargetStateCallback(ClearEvent_Target), false);
                from.SendMessage("Target the game stone.");
            }
        }

        [Usage("GameKick")]
        [Description("Kicks a player out of a game")]
        private static void KickPlayerFromEvent_Command(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetStateCallback(KickPlayerFromEvent_Target), false);
            e.Mobile.SendMessage("Select the player to kick.");
        }

        private static void KickPlayerFromEvent_Target(Mobile from, object o, object state)
        {
            if (o is PlayerMobile)
            {
                PlayerMobile player = (PlayerMobile)o;
                if (player.CurrentEvent != null)
                {
                    player.CurrentEvent.RemovePlayer(player);
                    from.SendMessage("The player has been kicked.");
                }
                else if (PvpCore.IsInDeathmatch(player))
                {
                    PvpCore.TryLeaveDM(player, true);
                    from.SendMessage("The player has been kicked.");
                }
                else if (TournamentCore.IsInTournament(player))
                {
                    TournamentCore.TryLeaveTournament(player, true);
                    from.SendMessage("The player has been kicked.");
                }
                else
                {
                    from.SendMessage("That player is not in a game");
                }
            }
            else
            {
                from.BeginTarget(-1, false, TargetFlags.None, new TargetStateCallback(KickPlayerFromEvent_Target), false);
                from.SendMessage("Target the player.");
            }
        }

        [Usage("Score")]
        [Description("Shows the score of the game that you are in.")]
        private static void ScoreEvent_Command(CommandEventArgs e)
        {
            BaseGame Game = ((PlayerMobile)e.Mobile).CurrentEvent;
            if (Game != null)
                Game.AnnounceScore(e.Mobile);
            else if (PvpCore.IsInDeathmatch(e.Mobile))
                PvpCore.TryShowScoreBoard(e.Mobile);
            else
                e.Mobile.SendAsciiMessage("You are not in an event.");
        }

        [Usage("ShowScore")]
        [Description("Tells you what the score in the current game is")]
        private static void ShowScoreEvent_Command(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, ShowScore_Target);
            e.Mobile.SendMessage("Target a player in a game or a deathmatch stone to display scores.");
        }

        private static void ShowScore_Target(Mobile from, object o)
        {
            if (o is PlayerMobile)
            {
                PlayerMobile pm = o as PlayerMobile;
                BaseGame Game = pm.CurrentEvent;
                if (Game != null)
                    Game.AnnounceScore(from);
                else if (PvpCore.IsInDeathmatch(pm))
                {
                    DMStone s = PvpCore.GetPlayerStone(pm);
                    if (s != null)
                        s.ShowScore(from);
                }
                else
                {
                    from.BeginTarget(-1, false, TargetFlags.None, ShowScore_Target);
                    from.SendMessage("Target a player in a game or a deathmatch stone");
                }
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
                from.SendMessage("Target a player in a game or a deathmatch stone");
            }
        }

        [Usage("Team")]
        [Aliases("T")]
        [Description("Sends a message to everyone in your team")]
        private static void EventTeamSay_Command(CommandEventArgs e)
        {
            BaseGame Game = ((PlayerMobile)e.Mobile).CurrentEvent;
            if (Game != null && Game is BaseTeamGame)
            {
                BaseTeamGame TeamGame = Game as BaseTeamGame;
                BaseGameTeam team = TeamGame.GetTeam(e.Mobile);
                team.TeamMessage(e.Mobile, e.ArgString);
            }
            else
                e.Mobile.SendAsciiMessage("You are not in a team event.");
        }

        private static void EventStoneGump_Command(CommandEventArgs e)
        {
            if (e.Mobile is PlayerMobile && e.Mobile.AccessLevel == AccessLevel.Player)
            {
                PlayerMobile player = e.Mobile as PlayerMobile;
                if(player.CurrentEvent != null)
                    player.CurrentEvent.SendPlayerGump(player);
                if (player.GameInfoGumpType == GameInfoGumpType.Disabled)
                    player.GameInfoGumpType = GameInfoGumpType.Extended;
            }
            else if (e.Mobile.AccessLevel >= AccessLevel.Counselor)
            {
                BaseGame game = BaseGame.FindRunningGame(e.Mobile);
                if (game != null)
                    game.OnDoubleClick(e.Mobile);
                else
                    e.Mobile.SendMessage("You are not hosting an event.");
            }
        }

        [Usage("SendScore")]
        [Description("Announces the score to the players in the game")]
        private static void SendScore_Command(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, SendScore_Target);
            e.Mobile.SendMessage("Target a player in a game to announce scores to them.");
        }

        private static void SendScore_Target(Mobile from, object o)
        {
            if (o is PlayerMobile)
            {
                PlayerMobile pm = o as PlayerMobile;
                BaseGame Game = pm.CurrentEvent;
                if (Game != null)
                    Game.AnnounceScore();
                else
                {
                    from.BeginTarget(-1, false, TargetFlags.None, ShowScore_Target);
                    from.SendMessage("Target a player in a game");
                }
            }
            else
            {
                from.BeginTarget(-1, false, TargetFlags.None, ShowScore_Target);
                from.SendMessage("Target a player in a game");
            }
        }
        #endregion

        private static HashMap<PlayerMobile, int> m_UpdateRatings;
        private static HashMap<PlayerMobile, int> m_UpdateTournamentRatings;

        public static HashMap<Mobile, Announcement> m_Announcements;

        private static AnnouncementTimer m_AnnouncementTimer = new AnnouncementTimer();

        public Events()
        {
        }

        public static void Initialize()
        {
            EventSink.WorldSave += OnSave;
            CommandSystem.Register("SaveRatings", AccessLevel.Counselor, DoSaveRatings);
            CommandSystem.Register("Gamestart", AccessLevel.Counselor, StartEvent_Command);
            CommandSystem.Register("Gameend", AccessLevel.Counselor, StopEvent_Command);
            CommandSystem.Register("Score", AccessLevel.Player, ScoreEvent_Command);
            CommandSystem.Register("ShowScore", AccessLevel.Counselor, ShowScoreEvent_Command);
            CommandSystem.Register("Gameopen", AccessLevel.Counselor, OpenEvent_Command);
            CommandSystem.Register("Gameclose", AccessLevel.Counselor, CloseEvent_Command);
            CommandSystem.Register("Gameclear", AccessLevel.Counselor, ClearEvent_Command);
            CommandSystem.Register("Gamekick", AccessLevel.Counselor, KickPlayerFromEvent_Command);
            CommandSystem.Register("T", AccessLevel.Player, EventTeamSay_Command);
            CommandSystem.Register("Team", AccessLevel.Player, EventTeamSay_Command);
            CommandSystem.Register("Gamegump", AccessLevel.Player, EventStoneGump_Command);
            CommandSystem.Register("SendScore", AccessLevel.Counselor, SendScore_Command);

            m_UpdateRatings = new HashMap<PlayerMobile, int>();
            m_UpdateTournamentRatings = new HashMap<PlayerMobile, int>();
            m_Announcements = new HashMap<Mobile, Announcement>();
        }

        public static void DoSaveRatings(CommandEventArgs e)
        {
            SaveRatings();
        }

        public static void OnSave(WorldSaveEventArgs e)
        {
            SaveRatings();
        }

        public static void CalculateRating(PlayerMobile killer, PlayerMobile killed)
        {
            if (killer.EventType != killed.EventType)
                return;

            if (killer == killed)
                return;

            double ratinga, ratingb, expecteda, expectedb;
            int akfactor, bkfactor;

            switch (killer.EventType)
            {

                case EventType.SuppliedTournament:
                case EventType.Tournament:
                    {
                        ratinga = (double)killer.TournamentRating;
                        ratingb = (double)killed.TournamentRating;
                        expecteda = 1 / (1 + Math.Pow(10.0, ((ratinga - ratingb) / 400.0)));
                        expectedb = 1 / (1 + Math.Pow(10.0, ((ratingb - ratinga) / 400.0)));

                        akfactor = GetKFactor(killer.EventType, killer.TournamentRating);
                        bkfactor = GetKFactor(killed.EventType, killed.TournamentRating);

                        SetTournamentRating(killer, killed.EventType, (int)Math.Round(ratinga + (akfactor * (1.0 - expecteda)), 0));
                        SetTournamentRating(killed, killer.EventType, (int)Math.Round(ratingb + (bkfactor * (0.0 - expectedb)), 0));
                        break;
                    }
            }

            ratinga = (double)killer.Rating;
            ratingb = (double)killed.Rating;
            expecteda = 1 / (1 + Math.Pow(10.0, ((ratinga - ratingb) / 400.0))); // 
            expectedb = 1 / (1 + Math.Pow(10.0, ((ratingb - ratinga) / 400.0))); // 

            akfactor = GetKFactor(killer.EventType, killer.Rating); // 20
            bkfactor = GetKFactor(killed.EventType, killed.Rating); // 20

            SetRating( killer, killed.EventType, (int)Math.Round(ratinga + (akfactor * (1.0 - expecteda)), 0) );
            SetRating( killed, killer.EventType, (int)Math.Round(ratingb + (bkfactor * (0.0 - expectedb)), 0) );
        }

        private static void SetRating(PlayerMobile mob, EventType type, int rating)
        {
            //mob.SetRating(rating);
            //m_UpdateRatings.Add(mob, mob.Rating);
        }

        private static void SetTournamentRating(PlayerMobile mob, EventType type, int rating)
        {
            //mob.SetTournamentRating(rating);
            //m_UpdateTournamentRatings.Add(mob, mob.TournamentRating);
        }

        private static void SaveRatings()
        {
            //Thread t = new Thread(new ThreadStart(RunSaveRatings));
            //t.Name = "Save Ratings";
            //t.Start();
        }

        private static void RunSaveRatings()
        {
            /*
            HashMap<PlayerMobile, int> UpdateRatings = new HashMap<PlayerMobile,int>(m_UpdateRatings);
            HashMap<PlayerMobile, int> UpdateTournamentRatings = new HashMap<PlayerMobile, int>(m_UpdateTournamentRatings);
            m_UpdateRatings.Clear();
            m_UpdateTournamentRatings.Clear();

            foreach (PlayerMobile mob in UpdateRatings.Keys)
            {
                if (mob != null && !mob.Deleted)
                {
                    int rating = UpdateRatings.Get(mob);
                    if(INXDatabase.CheckMobile(mob))
                        INXDatabase.Query("UPDATE playermobiles SET rating = " + rating + " WHERE id = " + (int)mob.Serial, MySQLDriver.AdapterCommandType.Update);
                }
            }
            foreach (PlayerMobile mob in UpdateTournamentRatings.Keys)
            {
                if (mob != null && !mob.Deleted)
                {
                    int rating = UpdateTournamentRatings.Get(mob);
                    if(INXDatabase.CheckMobile(mob))
                        INXDatabase.Query("UPDATE playermobiles SET tournamentrating = " + rating + " WHERE id = " + (int)mob.Serial, MySQLDriver.AdapterCommandType.Update);
                }
            }
            */
        }

        private static int GetKFactor(EventType type, int rating)
        {
            int kfactor = 0;
            switch (type)
            {
                case EventType.NoEvent:
                    {
                        kfactor = 10;
                        break;
                    }
                case EventType.SuppliedTournament:
                    {
                        kfactor = 10;
                        break;
                    }
                case EventType.Tournament:
                    {
                        kfactor = 15;
                        break;
                    }
                case EventType.DeathMatch:
                case EventType.CaptureTheFlag:
                    {
                        kfactor = 5;
                        break;
                    }
            }
            if (rating < 1700)
                kfactor += 10;
            if (rating > 2400)
                kfactor -= 5;
            return kfactor;
        }

        #region announcements
        public static void AddAnnouncement(Announcement ann)
        {
            /*m_Announcements.Add(ann.staffmember, ann);
            if (m_Announcements.Count == 1)
            {
                m_AnnouncementTimer.Interval = ann.interval;
                m_AnnouncementTimer.Start();
            }
            else if (m_Announcements.Count > 1)
            {
                if (ann.interval < m_AnnouncementTimer.Interval)
                    m_AnnouncementTimer.Interval = ann.interval;
            }
            */
        }
        #endregion
    }

    public class AnnouncementTimer : Timer
    {
        public AnnouncementTimer() : base(TimeSpan.FromMinutes(1.0))
        {

        }

        protected override void OnTick()
        {
            base.OnTick();
            DateTime now = DateTime.Now;
            foreach (Announcement announcement in Events.m_Announcements)
            {
                if (announcement.nextevent < now)
                {
                    announcement.Announce();
                    announcement.nextevent = now + announcement.interval;
                }
            }
        }
    }
}
