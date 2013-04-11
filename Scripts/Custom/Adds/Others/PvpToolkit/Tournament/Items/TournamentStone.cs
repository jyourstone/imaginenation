using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Custom.PvpToolkit.Tournament
{
    public class TournamentStone : Item
    {
        #region Area properties
        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D FirstNW { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D FirstSE { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D SecondNW { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D SecondSE { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D ThirdNW { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D ThirdSE { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D FourthNW { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D FourthSE { get; set; }
        #endregion

        #region EndType - Rob
        public enum ForceEndType
        {
            Referee,
            ItemRemoval
        }

        private ForceEndType m_forceEndType;

        [CommandProperty( AccessLevel.GameMaster )]
		public ForceEndType ForceEnd
		{
			get
			{
				return m_forceEndType;
			}
			set
			{
				if ( m_forceEndType != value )
				{
					m_forceEndType = value;
				}
			}
		}
        private List<Mobile> NeedRestock { get; set; }
        #endregion

        #region Variables
        private CountdownTimer m_CountdownTimer;
        private bool m_Started;
        private List<Rectangle2D> m_Areas;
        private Dictionary<Rectangle2D, Timer> m_MatchTimers;
        private static int m_Participants;
        #endregion

        #region Properties
        public List<Mobile> Contestants { get; set; }
        public List<Mobile> Fighting { get; set; }
        public List<Mobile> Winners { get; set; }

        public Dictionary<Serial, ScoreKeeper> ScoreTable { get; set; }

        public bool Active { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int StartDelay { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map LeaveMap { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D LeaveLocation { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map WinMap { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D WinLocation { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map JoinMap { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D JoinLocation { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AcceptingContestants { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public EventSupplier EventSupplier { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Started
        {
            get { return m_Started; }
            set
            {
                if (!CheckAreas())
                {
                    CommandHandlers.BroadcastMessage(AccessLevel.Counselor, 38, string.Format("[Tournament] Not started due to all four areas not properly set"));
                    AcceptingContestants = false;
                    m_Started = false;
                    return;
                }

                if (JoinLocation == Point3D.Zero || LeaveLocation == Point3D.Zero || WinLocation == Point3D.Zero)
                {
                    CommandHandlers.BroadcastMessage(AccessLevel.Counselor, 38, string.Format("[Tournament] Not started due to join, leave or win locations not set"));
                    AcceptingContestants = false;
                    m_Started = false;
                    return;
                }

                if (value)
                {
                    if (!m_Started)
                    {
                        AcceptingContestants = true;
                        BeginTimer();
                        TournamentCore.AddRunningTournament();

                        m_Areas.Clear();
                        m_Areas.Add(new Rectangle2D(FirstNW.X, FirstNW.Y, FirstSE.X - FirstNW.X + 1, FirstSE.Y - FirstNW.Y + 1));
                        m_Areas.Add(new Rectangle2D(SecondNW.X, SecondNW.Y, SecondSE.X - SecondNW.X + 1, SecondSE.Y - SecondNW.Y + 1));
                        m_Areas.Add(new Rectangle2D(ThirdNW.X, ThirdNW.Y, ThirdSE.X - ThirdNW.X + 1, ThirdSE.Y - ThirdNW.Y + 1));
                        m_Areas.Add(new Rectangle2D(FourthNW.X, FourthNW.Y, FourthSE.X - FourthNW.X + 1, FourthSE.Y - FourthNW.Y + 1));
                    }
                }
                else
                {
                    if (m_Started)
                        EndTournament(false);
                }

                m_Started = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int FightDelayInSecs { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MatchTimerInMins { get; set; }

        #endregion

        [Constructable]
        public TournamentStone() : base(0xEDD)
        {
            Name = "Tournament stone";
            Movable = false;
            TournamentCore.TournamentStones.Add(this);
            Contestants = new List<Mobile>();
            m_Areas = new List<Rectangle2D>();
            m_MatchTimers = new Dictionary<Rectangle2D, Timer>();
            Fighting = new List<Mobile>();
            Winners = new List<Mobile>();
            ScoreTable = new Dictionary<Serial, ScoreKeeper>();
            AcceptingContestants = false;
            m_Started = false;
            StartDelay = 15;
            LeaveMap = Map.Felucca;
            JoinMap = Map.Felucca;
            WinMap = Map.Felucca;
            FightDelayInSecs = 20;
            MatchTimerInMins = 10;
            // Rob
            NeedRestock = new List<Mobile>();
        }

        public TournamentStone(Serial serial) : base(serial)
        {
            TournamentCore.TournamentStones.Add(this);
            Contestants = new List<Mobile>();
            Fighting = new List<Mobile>();
            Winners = new List<Mobile>();
            m_Areas = new List<Rectangle2D>();
            m_MatchTimers = new Dictionary<Rectangle2D, Timer>();
            ScoreTable = new Dictionary<Serial, ScoreKeeper>();
            // Rob
            NeedRestock = new List<Mobile>();
        }

        public void RemovePlayer(Mobile m, bool defeated, bool kicked)
        {
            SupplySystem.RemoveEventGear(m);
            m.IsInEvent = false;
            m.Blessed = false;

            m.MoveToWorld(LeaveLocation, LeaveMap);

            if (Contestants.Contains(m))
                Contestants.Remove(m);

            if (Fighting.Contains(m))
                Fighting.Remove(m);

            if (Winners.Contains(m))
                Winners.Remove(m);

            // Rob
            if (NeedRestock.Contains(m))
            {
                NeedRestock.Remove(m);
            }
            // end Rob

            if (m.NetState != null)
            {
                if (defeated)
                    m.SendAsciiMessage(38, "You have been defeated");
                else if (kicked)
                    m.SendAsciiMessage(38, "You have been kicked from the tournament");
                else
                    m.SendAsciiMessage(38, "You have left the tournament");
            }

            //Only print the message if the match is onging
            if (!m_Started) 
                return;

            string leaveMessage;

            if (defeated)
                leaveMessage = string.Format("{0} has been defeated", m.Name);
            else if (kicked)
                leaveMessage = string.Format("{0} has been kicked from a tournament", m.Name);
            else
                leaveMessage = string.Format("{0} has left a tournament", m.Name);

            TournamentCore.SendMessage(Contestants, leaveMessage, true);
        }

        public void AddPlayer(Mobile m)
        {
            if (m_Started && AcceptingContestants)
            {
                string joinMessage = string.Format("{0} has entered the tournament", m.Name);
                TournamentCore.SendMessage(Contestants, joinMessage, true);

                ReadyPlayer(m);
                m.SendMessage(38,"If you wish to leave, please type .leavetour");
            }
            else
                m.SendMessage("This event is either closed or full, please try again later");
        }

        private void ReadyPlayer(Mobile m)
        {
            if (!ScoreTable.ContainsKey(m.Serial))
                ScoreTable.Add(m.Serial, new ScoreKeeper(m));
        
            if (EventSupplier != null)
                EventSupplier.OnMoveOver(m);

            Contestants.Add(m);

            m.MoveToWorld(JoinLocation, JoinMap);
            m.Blessed = true;

            if (m.NetState != null)
                m.SendMessage(38, "You have joined a tournament");
        }

        private Rectangle2D GetArea(Mobile m)
        {
            for (int i = 0; i < m_Areas.Count; i++)
            {
                IPooledEnumerable eable = JoinMap.GetMobilesInBounds(m_Areas[i]);

                foreach (Mobile mob in eable)
                {
                    if (mob == m)
                        return m_Areas[i];
                }

                eable.Free();
            }
            return new Rectangle2D();
        }

        public override void OnDoubleClick(Mobile from)
        {
            TournamentCore.TryJoinTournament(from, this);
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, Name);
            LabelTo(from, Started ? "Active" : "Not Active");
        }

        public void SpawnMobile(Mobile m)
        {
            for (int i = 0; i < m_Areas.Count; ++i)
            {
                int mobCount = 0;
                IPooledEnumerable eable = JoinMap.GetMobilesInBounds(m_Areas[i]);

                foreach (Mobile mob in eable)
                {
                    if (Contestants.Contains(mob) && mob.Alive)
                        mobCount++;
                }

                eable.Free();

                if (mobCount <= 1) //Found an available area
                {
                    if (mobCount == 1) //Start area fight timer
                    {
                        Timer t;
                        m_MatchTimers.TryGetValue(m_Areas[i], out t);

                        if (t != null)
                        {
                            t.Stop();
                            m_MatchTimers.Remove(m_Areas[i]);
                        }

                        t = Contestants.Count > 2 ? new MatchTimer(this, m_Areas[i], false) : new MatchTimer(this, m_Areas[i], true);
                        t.Start();
                        m_MatchTimers.Add(m_Areas[i], t);
                    }

                    m.MoveToWorld(new Point3D(m_Areas[i].Start, JoinMap.GetAverageZ(m_Areas[i].X, m_Areas[i].Y)), JoinMap);
                    m.LocalOverheadMessage(MessageType.Regular, 38, true, string.Format("Fight will begin in {0} seconds!", FightDelayInSecs));
                    Fighting.Add(m);
                    FixPlayer(m);
                    new StartFightTimer(this, m).Start();
                    break;
                }
            }
        }

        public void HandleDeath(Mobile m)
        {      
            if (!m_Started)
                return;

            List<Mobile> opponents = new List<Mobile>();
            List<Mobile> referees = new List<Mobile>();

            Rectangle2D area = GetArea(m);

            // Rob - remove and stop the Match or EndMatch timer
            if (m_MatchTimers.ContainsKey(area))
            {
                Timer t = m_MatchTimers[area];
                t.Stop();
                m_MatchTimers.Remove(area);
            }
            // end Rob
            IPooledEnumerable eable = JoinMap.GetMobilesInBounds(area);

            foreach (Mobile mob in eable)
            {
                if (mob != m && Contestants.Contains(mob))
                    opponents.Add(mob);
                else if (mob is Referee)
                    referees.Add(mob);
            }

            eable.Free();

            for (int i = 0; i < opponents.Count && opponents.Count <= 1; ++i)
            {
                Mobile mob = opponents[i];
                UpdateScores(m, mob);
                mob.MoveToWorld(WinLocation, WinMap);
                FixPlayer(mob);
                mob.Blessed = true;
                Fighting.Remove(mob);
                Winners.Add(mob);

                //Rob -re-add gear in case gear was removed
                if (ForceEnd == ForceEndType.ItemRemoval && NeedRestock.Contains(mob))
                {
                    SupplySystem.RemoveEventGear(mob);
                    SupplySystem.SupplyGear(mob, EventSupplier);
                    NeedRestock.Remove(mob);
                    mob.SendAsciiMessage("You have been resupplied!");
                }
            }

            NeedRestock.Remove(m); // loser doesn't need a restock
            // end - Rob

            for (int i = 0; i < referees.Count; ++i)
            {
                Mobile mob = referees[i];
                mob.Delete();
            }

            Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerStateCallback(ResurrectPlayer), m);

            HandleCorpse(m);
            
            Fighting.Remove(m);
            RemovePlayer(m, true, false);

            m.LastKiller = null;
            
            AddFighters(false);
        }

        private void ResurrectPlayer(object o)
        {
            if (o is PlayerMobile)
            {
                Mobile m = o as Mobile;

                m.Resurrect();
                m.MoveToWorld(LeaveLocation, LeaveMap);
                FixPlayer(m);
            }
        }

        private void FixPlayer(Mobile m)
        {
            if (m == null) 
                return;

            m.CurePoison(m);

            m.Mana = m.ManaMax;
            m.Stam = m.StamMax;
            m.Hits = m.HitsMax;         
        }
        
        private void UpdateScores(Mobile died, Mobile killer)
        {
           if ( ScoreTable == null)
                return;

            ScoreKeeper scoreKeeper;

            if (died != null && ScoreTable.TryGetValue(died.Serial, out scoreKeeper))
                scoreKeeper.Deaths++;

            if (killer != null && killer is PlayerMobile && ScoreTable.TryGetValue(killer.Serial, out scoreKeeper))
                scoreKeeper.Kills++;
        }
        
        public void HandleCorpse(Mobile from)
        {
            if (from.Corpse != null)
            {
                if (EventSupplier != null && EventSupplier.NewbieAllItems)
                {
                    from.Corpse.Delete();
                    return;
                }

                Corpse c = (Corpse)from.Corpse;
                c.Open(from, true);
                c.Delete();
                from.SendMessage(38, "The contents of your corpse have been safely placed into your backpack");
            }
        }

        #region Serialization/Deserialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1);

            //ver 1
            writer.Write((int)ForceEnd);

            //ver 0
            writer.Write(FirstNW);
            writer.Write(FirstSE);
            writer.Write(SecondNW);
            writer.Write(SecondSE);
            writer.Write(ThirdNW);
            writer.Write(ThirdSE);
            writer.Write(FourthNW);
            writer.Write(FourthSE);
            writer.Write(m_Started);
            writer.Write(Active);
            writer.Write(StartDelay);
            writer.Write(MatchTimerInMins);
            writer.Write(FightDelayInSecs);
            writer.Write(LeaveMap);
            writer.Write(LeaveLocation);
            writer.Write(WinMap);
            writer.Write(WinLocation);
            writer.Write(JoinMap);
            writer.Write(JoinLocation);
            writer.Write(AcceptingContestants);
            writer.WriteItem(EventSupplier);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            ForceEnd = ForceEndType.Referee;

            switch (version)
            {
                case 1:
                    {
                        ForceEnd = (ForceEndType)reader.ReadInt();
                        goto case 0;
                    }
                case 0:
                    {
                        FirstNW = reader.ReadPoint3D();
                        FirstSE = reader.ReadPoint3D();
                        SecondNW = reader.ReadPoint3D();
                        SecondSE = reader.ReadPoint3D();
                        ThirdNW = reader.ReadPoint3D();
                        ThirdSE = reader.ReadPoint3D();
                        FourthNW = reader.ReadPoint3D();
                        FourthSE = reader.ReadPoint3D();
                        m_Started = reader.ReadBool();
                        Active = reader.ReadBool();
                        StartDelay = reader.ReadInt();
                        MatchTimerInMins = reader.ReadInt();
                        FightDelayInSecs = reader.ReadInt();
                        LeaveMap = reader.ReadMap();
                        LeaveLocation = reader.ReadPoint3D();
                        WinMap = reader.ReadMap();
                        WinLocation = reader.ReadPoint3D();
                        JoinMap = reader.ReadMap();
                        JoinLocation = reader.ReadPoint3D();
                        AcceptingContestants = reader.ReadBool();
                        EventSupplier = reader.ReadItem<EventSupplier>();
                        break;
                    }
            }

            m_Started = false;
            AcceptingContestants = false;
            Active = false;
        }
        #endregion

        public void BeginTimer()
        {
            if (m_CountdownTimer != null)
                m_CountdownTimer.Stop();

            m_CountdownTimer = new CountdownTimer(this);
            m_CountdownTimer.Start();
        }

        public void EndTournament(bool worldLoaded)
        {
            if (m_CountdownTimer != null)
                m_CountdownTimer.Stop();

            TournamentCore.RemoveRunningTournament();
            CommandHandlers.BroadcastMessage(AccessLevel.Player, 38, "A tournament has ended");

            if (ScoreTable != null && ScoreTable.Count > 0)
            {
                List<ScoreKeeper> scoreKeeperList = new List<ScoreKeeper>(ScoreTable.Values);

                scoreKeeperList.Sort();

                if (scoreKeeperList[0] != null && worldLoaded)
                {
                    CommandHandlers.BroadcastMessage(AccessLevel.Player, 38, String.Format("{0} has won the tournament!", scoreKeeperList[0].Player.Name));
                        RewardPlayers(scoreKeeperList);
                }
            }

            if (Contestants != null && Contestants.Count > 0)
            {
                List<Mobile> removeList = new List<Mobile>(Contestants);
                foreach (Mobile m in removeList)
                    RemovePlayer(m, false, false);
            }

            m_Started = false;
            AcceptingContestants = false;

            ScoreTable = new Dictionary<Serial, ScoreKeeper>();
            Contestants = new List<Mobile>();
            Fighting = new List<Mobile>();
            Winners = new List<Mobile>();
            m_Areas = new List<Rectangle2D>();
            m_MatchTimers = new Dictionary<Rectangle2D, Timer>();
            NeedRestock = new List<Mobile>(); // - Rob
        }

        private bool CheckAreas()
        {
            return FirstNW != Point3D.Zero && FirstSE != Point3D.Zero && SecondNW != Point3D.Zero && SecondSE != Point3D.Zero && ThirdNW != Point3D.Zero && ThirdSE != Point3D.Zero && FourthNW != Point3D.Zero && FourthSE != Point3D.Zero;
        }

        public void AddFighters(bool firstRound)
        {
            if (firstRound)
            {
                m_Participants = Contestants.Count;
                Contestants = new List<Mobile>(RandomizeGenericList(Contestants)); //Randomize
            }

            if (Contestants.Count > 1)
            {
                if (!firstRound && Fighting.Count <= 0)
                {
                    for (int i = 0; i < Winners.Count; ++i)
                        Winners[i].MoveToWorld(JoinLocation, JoinMap);

                    TournamentCore.SendMessage(Contestants, "Next round will start in 60 seconds!");
                    Contestants = new List<Mobile>(RandomizeGenericList(Contestants)); //Randomize
                    Winners.Clear();
                    Timer.DelayCall(TimeSpan.FromSeconds(60), new TimerCallback(SpawnContestants));
                }
                else
                    SpawnContestants();
            }
            else
                EndTournament(true);
        }

        private void SpawnContestants()
        {
            for (int i = 0; i < Contestants.Count; ++i)
            {
                if (Fighting.Contains(Contestants[i]) || Winners.Contains(Contestants[i]))
                    continue;

                SpawnMobile(Contestants[i]);
            }

            Timer.DelayCall(TimeSpan.FromSeconds(3), new TimerCallback(CheckAloneFighter));
        }

        private void CheckAloneFighter()
        {
            for (int i = 0; i < m_Areas.Count; ++i)
            {
                IPooledEnumerable eable = JoinMap.GetMobilesInBounds(m_Areas[i]);
                List<Mobile> players = new List<Mobile>();

                foreach (Mobile mob in eable)
                {
                    if (Contestants.Contains(mob))
                        players.Add(mob);
                }

                eable.Free();

                if (players.Count == 1) //Alone
                {
                    Mobile m = players[0];
                    m.MoveToWorld(WinLocation, WinMap);
                    m.PublicOverheadMessage(MessageType.Regular, 38, true, "Skipping to next round...");
                    m.Blessed = true;
                    Fighting.Remove(m);
                    Winners.Add(m);
                    UpdateScores(null, m);
                    AddFighters(false);
                }
            }
        }

        // method for randomizing a generic list of type T
        public static List<T> RandomizeGenericList<T>(IList<T> originalList)
        {
            List<T> randomList = new List<T>();
            Random random = new Random();
            T value;

            //now loop through all the values in the list
            while (originalList.Count > 0)
            {
                //pick a random item from th original list
                var nextIndex = random.Next(0, originalList.Count);
                //get the value for that random index
                value = originalList[nextIndex];
                //add item to the new randomized list
                randomList.Add(value);
                //remove value from original list (prevents
                //getting duplicates
                originalList.RemoveAt(nextIndex);
            }

            //return the randomized list
            return randomList;
        }

        #region Reward players
        private static void RewardPlayers(List<ScoreKeeper> scoreKeeperList)
        {
            PlayerMobile pm;
            int gold;

            for (int i = 0; i < scoreKeeperList.Count && i < 2; i++)
            {
                if (scoreKeeperList[i].Player == null || !(scoreKeeperList[i].Player is PlayerMobile))
                    return;

                pm = scoreKeeperList[i].Player as PlayerMobile;

                switch (i)
                {
                    case 0: //Reward the winner
                        {
                            gold = scoreKeeperList[i].Kills*3000;
                            if (gold > 0)
                            {
                                pm.AddToBackpack(new Gold(gold));
                                pm.SendAsciiMessage("Congratulations! You won the tournament and have been rewarded with {0} gold", gold, scoreKeeperList[i].Kills);
                            }
                            if (m_Participants >= 10)
                            {
                                pm.AddToBackpack(new ImagineNickel(scoreKeeperList[i].Kills*3));
                                pm.SendAsciiMessage("Due to the high participation you also get {0} imagine nickels", scoreKeeperList[i].Kills*4);
                            }
                            break;
                        }
                    case 1: //Now reward the runner up
                        {
                            gold = scoreKeeperList[i].Kills*2000;
                            if (gold > 0)
                            {
                                pm.AddToBackpack(new Gold(gold));
                                pm.SendAsciiMessage("Congratulations! You placed second and have been rewarded with {0} gold", gold, scoreKeeperList[i].Kills);
                            }
                            if (m_Participants >= 10)
                            {
                                pm.AddToBackpack(new ImagineNickel(scoreKeeperList[i].Kills*2));
                                pm.SendAsciiMessage("Due to the high participation you also get {0} imagine nickels", scoreKeeperList[i].Kills*2);
                            }
                            break;
                        }
                }
            }
        }
        #endregion

        #region Nested type: MatchTimer
        public class MatchTimer : Timer
        {
            private readonly TournamentStone m_TournamentStone;
            private readonly Rectangle2D m_Area;
            private int m_Count;

            public MatchTimer(TournamentStone stone, Rectangle2D area, bool final) : base(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1))
            {
                m_TournamentStone = stone;
                m_Count = final ? m_TournamentStone.MatchTimerInMins * 3 : m_TournamentStone.MatchTimerInMins;
                m_Area = area;
            }

            protected override void OnTick()
            {
                m_Count--;

                if (m_TournamentStone != null && m_TournamentStone.Started)
                {
                    if (m_Count <= 2)
                    {
                        IPooledEnumerable eable = m_TournamentStone.JoinMap.GetMobilesInBounds(m_Area);
                        List<Mobile> mobs = new List<Mobile>();

                        foreach (Mobile mob in eable)
                        {
                            if (m_TournamentStone.Contestants.Contains(mob))
                                mobs.Add(mob);
                        }

                        eable.Free();

                        if (m_Count > 0)
                        {
                            for (int i = 0; i < mobs.Count; ++i)
                                mobs[i].LocalOverheadMessage(MessageType.Regular, 38, true, string.Format( "Better hurry up! Only {0} minute{1} remaining...", m_Count, m_Count == 1 ? "" : "s"));
                        }
                        else
                        {
                            if (mobs.Count >= 1)
                            {
                                if (m_TournamentStone.ForceEnd == ForceEndType.ItemRemoval)
                                {
                                    m_TournamentStone.StartEndTimer(m_Area);
                                }
                                else
                                {
                                    m_TournamentStone.SpawnReferee(m_Area);                            
                                }
                            }
                            Stop();
                        }
                    }
                }
                else
                    Stop();
            }
        }
        #endregion

        #region Nested type: CountdownTimer
        public class CountdownTimer : Timer
        {
            private readonly TournamentStone m_TournamentStone;
            private int count;

            public CountdownTimer(TournamentStone stone) : base(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1))
            {
                m_TournamentStone = stone;
                count = m_TournamentStone.StartDelay;

                CommandHandlers.BroadcastMessage(AccessLevel.Player, 38, string.Format("An automated and supplied 1v1 tournament will start in {0} minute{1}! Type .jointour to join or .watchtour to watch", count, count == 1 ? "" : "s"));
            }

            protected override void OnTick()
            {
                count--;

                if (m_TournamentStone != null && m_TournamentStone.Started)
                {
                    if (count <= 0)
                    {
                        if (m_TournamentStone.Contestants.Count >= 2)
                        {
                            CommandHandlers.BroadcastMessage(AccessLevel.Player, 38, "An automated tournament has begun");
                            m_TournamentStone.AcceptingContestants = false;
                            m_TournamentStone.AddFighters(true);
                            Stop();
                        }
                        else
                        {
                            TournamentCore.SendMessage(m_TournamentStone.Contestants, "Not enough players joined, ending tournament");
                            m_TournamentStone.EndTournament(false);
                            Stop();
                        }
                    }
                    else if (count <= 10)
                        CommandHandlers.BroadcastMessage(AccessLevel.Player, 38, string.Format("An automated and supplied 1v1 tournament will start in {0} minute{1}! Type .jointour to join or .watchtour to watch", count, count == 1 ? "" : "s"));
                }
                else
                    Stop();
            }
        }
        #endregion

        #region Nested type: StartFightTimer
        public class StartFightTimer : Timer
        {
            private readonly TournamentStone m_Stone;
            private readonly Mobile m_Mobile;
            private int m_Count;

            public StartFightTimer(TournamentStone stone, Mobile m) : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
            {
                m_Mobile = m;
                m_Stone = stone;
                m_Count = m_Stone.FightDelayInSecs;
            }

            protected override void OnTick()
            {
                if (m_Stone != null && m_Stone.Started && m_Stone.Fighting.Contains(m_Mobile))
                {
                    if (m_Count < 1)
                    {
                        m_Mobile.LocalOverheadMessage(MessageType.Regular, 38, true, "FIGHT!");
                        m_Stone.FixPlayer(m_Mobile);
                        m_Mobile.Blessed = false;
                        Stop();
                    }
                    else if (m_Count <= 5)
                        m_Mobile.LocalOverheadMessage(MessageType.Regular, 38, true, m_Count.ToString());

                    m_Count--;
                }
                else
                    Stop();
            }
        }
        #endregion

        internal void SpawnReferee(Rectangle2D area)
        {
            int minX = area.Start.X;
            int maxX = area.End.X - 2;
            int minY = area.Start.Y;
            int maxY = area.End.Y - 2;
            int Z = JoinMap.GetAverageZ(area.X, area.Y);
            Referee referee = new Referee();
            referee.MoveToWorld(new Point3D(Utility.RandomMinMax(minX, maxX), Utility.RandomMinMax(minY, maxY), Z), JoinMap);
            referee.PublicOverheadMessage(MessageType.Regular, 906, true, "Get a move on!");
            referee.IsInEvent = true;
        }

        internal void StartEndTimer(Rectangle2D m_Area)
        {
            Timer t = m_MatchTimers[m_Area];
            if (t != null)
            {
                t.Stop();
                m_MatchTimers.Remove(m_Area);
            }

            t = new EndMatchTimer(this, m_Area);
            m_MatchTimers[m_Area] = t;
            t.Start();
        }

        class EndMatchTimer : Timer
        {
            private TournamentStone m_Stone;
            private Rectangle2D m_Area;
            private int m_Count = 7;
            private List<Mobile> m_Fighters;
            private static string m_Message = "Your armor crumbles";

            public EndMatchTimer(TournamentStone stone, Rectangle2D area) : base(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(50))
            {
                this.m_Stone = stone;
                this.m_Area = area;
                IPooledEnumerable eable = m_Stone.JoinMap.GetMobilesInBounds(m_Area);
                m_Fighters = new List<Mobile>();
                foreach (Mobile m in eable)
                {
                    if (m_Stone.Fighting.Contains(m))
                        m_Fighters.Add(m);
                }
                eable.Free();
            }

            protected override void OnTick()
            {
                if (m_Fighters.Count != 2)
                    Stop();

                switch (m_Count)
                {
                    case 7:
                        RemoveItem(typeof (PlateHelm));
                        break;
                    case 6:
                        RemoveItem(typeof(PlateGorget));
                        break;
                    case 5:
                        RemoveItem(typeof(PlateGloves));
                        break;
                    case 4:
                        RemoveItem(typeof(PlateArms));
                        break;
                    case 3:
                        RemoveItem(typeof(PlateLegs));
                        break;
                    case 2:
                        RemoveItem(typeof(MetalKiteShield));
                        break;
                    case 1:
                        RemoveItem(typeof(PlateChest));
                        break;
                    case 0:
                        m_Stone.SpawnReferee(m_Area);
                        Stop(); // stop timer once you spawn the ref
                        break;
                }
                m_Count--;
            }

            internal void RemoveItem(Type t)
            {
                foreach (Mobile m in m_Fighters)
                {
                    if (!m_Stone.Fighting.Contains(m)) // not fighting so exit
                    {
                        return;
                    }
                    if (!m_Stone.NeedRestock.Contains(m))
                    {
                        m_Stone.NeedRestock.Add(m);
                    }
                    Item item = m.Items.Find(i => i.GetType() == t);
                    if (item == null)
                    {
                        item = m.Backpack.FindItemByType(t);
                    }
                    if (item != null)
                    {
                        m.PrivateOverheadMessage(MessageType.Regular, 38, true, String.Format(m_Message), m.NetState);
                        item.Delete();
                    }
                }
            }
        }
    }
}