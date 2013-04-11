using System;
using System.Collections.Generic;
using Server.Commands;

namespace Server.Custom.PvpToolkit.Tournament
{
    public class TournamentCore
    {
        public static string TournamentVersion { get { return "1.00"; } }

        private static List<TournamentStone> m_TournamentStones = new List<TournamentStone>();

        public static List<TournamentStone> TournamentStones { get { return m_TournamentStones; } set { m_TournamentStones = value; } }

        private static int AmountOfRunningTournamentGames;

        public static void Initialize()
        {
            OnLoad();
        }

        public static void AddRunningTournament()
        {
            if (AmountOfRunningTournamentGames < 0) //This can be negative when server is shut down or crashes when a tour is running
                AmountOfRunningTournamentGames = 0;

            if (++AmountOfRunningTournamentGames == 1)
            {
                EventSink.PlayerDeath += EventSink_PlayerDeath;
                EventSink.Disconnected += EventSink_Disconnected;
            }
        }

        public static void RemoveRunningTournament()
        {
            if (--AmountOfRunningTournamentGames == 0)
            {
                EventSink.PlayerDeath -= EventSink_PlayerDeath;
                EventSink.Disconnected -= EventSink_Disconnected;
            }

            if (AmountOfRunningTournamentGames < 0)
                AmountOfRunningTournamentGames = 0;
        }

        public static bool TryJoinTournament(Mobile from)
        {
            return TryJoinTournament(from, FindTournamentStone());
        }

        public static bool TryJoinTournament(Mobile from, TournamentStone tStone)
        {
            if (Factions.Sigil.ExistsOn(from))
                from.SendLocalizedMessage(1061632); // You can't do that while carrying the sigil.
            else if (IsInTournament(from))
                from.SendMessage("You are already in a tournament. Type .leavetour to leave.");
            else if (from.IsInEvent)
                from.SendMessage("You cannot join a tournament while in an event");
            else if (from.Spell != null)
                from.SendLocalizedMessage(1049616); // You are too busy to do that at the moment.
            else if (!from.Alive)
                from.SendMessage("You cannot join a tournament while dead.");
            else if (from.IsBodyMod && !from.Body.IsHuman)
                from.SendMessage("You cannot join a tournament while polymorphed.");
            else if (TournamentStones.Count < 1)
                from.SendMessage("No tournament system exists on the server.");
            else if (from.AccessLevel > AccessLevel.Player)
                from.SendMessage("You cannot join a tournament as staff.");
            else
                return AllowTournamentJoin(from, tStone);

            return false;
        }

        public static void TryLeaveTournament(Mobile from, bool kicked)
        {
            if (IsInTournament(from))
            {
                TournamentStone ts = GetPlayerStone(from);

                if (ts != null)
                {
                    if (ts.Fighting != null && ts.Fighting.Contains(from))
                        from.SendAsciiMessage("You cannot leave a tournament while fighting");
                    else
                        ts.RemovePlayer(from, false, kicked);
                }
            }
        }

        public static void SendMessage(List<Mobile> mobiles, string message)
        {
            SendMessage(mobiles, message, true);
        }

        public static void SendMessage(List<Mobile> mobiles, string message, bool toStaff)
        {
            for (int i = 0; i < mobiles.Count; i++)
                mobiles[i].SendMessage(38, message);

            if (toStaff)
                CommandHandlers.BroadcastMessage(AccessLevel.Counselor, 38, string.Format("[Tournament] {0}", message));
        }
        /*
        public static void TryShowScoreBoard(Mobile from)
        {
            if (IsInTournament(from))
            {
                TournamentStone ts = GetPlayerStone(from);

                if (ts != null)
                    ts.ShowScore(from);
            }
        }
        */
        private static bool AllowTournamentJoin(Mobile from, TournamentStone stone)
        {
            if (stone != null)
            {
                if (stone.EventSupplier != null)
                {
                    stone.AddPlayer(from);
                    return true;
                }

                from.SendAsciiMessage("There is no event supplier connected to the death match");
            }
            else
                from.SendMessage("Either a tournament has not been started or it's not accepting more players.");

            return false;
        }

        public static TournamentStone FindTournamentStone()
        {
            foreach (TournamentStone tStone in TournamentStones)
                if (tStone != null && tStone.Started && tStone.AcceptingContestants)
                    return tStone;

            return null;
        }

        private static void OnLoad()
        {

        }

        private static void EventSink_Disconnected(DisconnectedEventArgs e)
        {
            Mobile m = e.Mobile;

            if (m == null)
                return;

            TournamentStone stone = GetPlayerStone(m);

            if (stone != null && IsInTournament(m))
                new DisconnectTimer(stone, m).Start();
        }

        private static void EventSink_PlayerDeath(PlayerDeathEventArgs e)
        {
            Mobile m = e.Mobile;

            if (m == null)
                return;

            if (IsInTournament(m))
            {
                TournamentStone stone = GetPlayerStone(m);

                if (stone != null)
                    stone.HandleDeath(m);
            }
        }

        public static TournamentStone GetPlayerStone(Mobile m)
        {
            foreach (TournamentStone stone in TournamentStones)
                if (stone != null && stone.Contestants.Contains(m))
                    return stone;

            return null;
        }

        public static bool IsInTournament(Mobile m)
        {
            foreach (TournamentStone stone in TournamentStones)
                if (stone != null && stone.Contestants.Contains(m))
                    return true;

            return false;
        }

        #region Nested type: DisconnectTimer
        public class DisconnectTimer : Timer
        {
            private readonly TournamentStone m_Stone;
            private readonly Mobile m_Mobile;

            public DisconnectTimer(TournamentStone stone, Mobile m) : base(TimeSpan.FromMinutes(3.0))
            {
                m_Mobile = m;
                m_Stone = stone;
            }

            protected override void OnTick()
            {
                if (m_Stone != null && m_Stone.Started && m_Stone.Contestants.Contains(m_Mobile))
                {
                    if (m_Mobile.NetState == null) //Not logged in, kick from tournament
                    {
                        bool fighting = false;

                        if (m_Stone.Fighting.Contains(m_Mobile))
                            fighting = true;

                        m_Stone.RemovePlayer(m_Mobile, false, true);

                        if (fighting) //Only try to add more fighters if the disconnected player was fighting, otherwise bugs will occur
                            m_Stone.AddFighters(false);
                    }
                }
            }
        }
        #endregion
    }
}