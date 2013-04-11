using System;
using System.Collections.Generic;
using System.Text;
using Server.Mobiles;
using Server.Custom.Games;
using Server.Gumps;
using Server.Items;

namespace Server.Custom.Games
{
    public class EndGameEventArgs : EventArgs
    {
        private BaseGame m_Game;

        public BaseGame Game { get { return m_Game; } }

        public EndGameEventArgs(BaseGame game)
        {
            m_Game = game;
        }
    }

    public abstract class BaseGame : Item
    {
        public static List<BaseGame> m_RunningGames = new List<BaseGame>();

        private List<Mobile> m_Players = new List<Mobile>();
        private Mobile m_GameMaster;
        private EventType m_EventType;
        private String m_GameName;

        private bool m_MsgStaff = false;
        private bool m_Running = false;
        private bool m_Open = false;
        protected bool m_UseGump = true;
        private bool m_BeginGame = false;

        private DateTime m_StartTime;
        private TimeSpan m_Length;

        private int m_MaxScore;

        private Point3D m_Lobby;
        private Map m_LobbyMap;

        protected ScoreTimer m_ScoreTimer;
        protected GameTimer m_GameTimer;
        protected GameGumpTimer m_GumpTimer;

        private bool m_GiveSupplies = false;
        private EventSupplier m_Supplier;
        protected Dictionary<Mobile, EquipmentStorage> m_PlayerSupplies = new Dictionary<Mobile, EquipmentStorage>();
        protected Dictionary<Mobile, int> m_PlayerScores = new Dictionary<Mobile, int>();
        protected List<Participant> m_Winners = new List<Participant>();

        public event EndGameEventHandler EndGameEvent;
        public delegate void EndGameEventHandler(EndGameEventArgs e);

        public BaseGame(int itemid) : base(itemid)
        {
            Movable = false;
            Visible = false;
            EventSink.WorldLoad += EndGameCommand;
        }

        public BaseGame(Serial serial)
            : base(serial)
        {
            EventSink.WorldLoad += EndGameCommand;
        }

        public static BaseGame FindRunningGame(Mobile gamemaster)
        {
            foreach (BaseGame game in m_RunningGames)
            {
                if (game.GameMaster == gamemaster)
                    return game;
            }
            return null;
        }

        #region properties
        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public EventType EventType
        {
            get
            {
                return m_EventType;
            }
            set
            {
                m_EventType = value;
            }
        }
        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public String GameName
        {
            get
            {
                return m_GameName;
            }
            set
            {
                m_GameName = value;
            }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public Mobile GameMaster
        {
            get
            {
                return m_GameMaster;
            }
        }

        public List<Mobile> Players
        {
            get
            {
                return m_Players;
            }
            set
            {
                m_Players = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool MsgStaff
        {
            get
            {
                return m_MsgStaff;
            }
            set
            {
                m_MsgStaff = value;
            }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public bool Running
        {
            get
            {
                return m_Running;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxScore
        {
            get
            {
                return m_MaxScore;
            }
            set
            {
                m_MaxScore = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Length
        {
            get
            {
                return m_Length;
            }
            set
            {
                if (Running)
                {
                    if (m_GameTimer != null)
                    {
                        m_GameTimer.SetEnd(m_StartTime + value);
                    }
                }

                m_Length = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, false)]
        public virtual bool Open
        {
            get
            {
                return m_Open;
            }
            set
            {
                m_Open = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool GiveSupplies
        {
            get
            {
                return m_GiveSupplies;
            }
            set
            {
                if (value && Supplier == null)
                {
                    Supplier = new EventSupplier();
                }
                m_GiveSupplies = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool BeginGame 
        {
            get
            {
                return m_BeginGame;
            }
            set
            {
                m_BeginGame = value;
            }
        }

        public EventSupplier Supplier
        {

            get
            {
                if (m_Supplier == null && GiveSupplies)
                {
                    Supplier = new EventSupplier();
                }
                return m_Supplier;
            }
            set
            {
                m_Supplier = value;
            }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public TimeSpan TimeLeft { get { if (Running) return m_Length - (DateTime.Now - m_StartTime); else return Length; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Lobby { get { return m_Lobby; } set { m_Lobby = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map LobbyMap { get { return m_LobbyMap; } set { m_LobbyMap = value; } }

        #endregion

        #region start and endgame
        public void StartCommand(Mobile from)
        {
            try
            {
                StartGame(from);
                OnAfterStart();
            }
            catch (EventException e)
            {
                from.SendMessage(e.Message);
            }
        }

        public virtual void StartGame(Mobile from)
        {
            if (Open && BeginGame)
            {
                try
                {
                    m_RunningGames.Add(this);
                    m_GameMaster = from;
                    m_StartTime = DateTime.Now;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            if (!Open)
            {
                throw new EventException("You cannot start a game that is not open!");
            }
            if (Running)
            {
                throw new EventException("The game is already running!");
            }
            try
            {
            m_RunningGames.Add(this);
            m_GameMaster = from;
            m_StartTime = DateTime.Now;
            }
            catch (Exception e)
            {
            	Console.WriteLine(e.ToString());
        	}

            if (m_GameTimer != null)
            {
                m_GameTimer.Stop();
                m_GameTimer = null;
            }

            if (m_ScoreTimer != null)
            {
                m_ScoreTimer.Stop();
                m_ScoreTimer = null;
            }

            if (m_GumpTimer != null)
            {
                m_GumpTimer.Stop();
                m_GumpTimer = null;
            }

            if (Length.TotalSeconds > 0)
            {
                m_GameTimer = new GameTimer(this);
                m_GameTimer.Start();
            }

            if (m_UseGump)
            {
                //m_GumpTimer = new GameGumpTimer(this);
                //m_GumpTimer.Start();
            }
        }

        public virtual void OnAfterStart()
        {

            foreach (PlayerMobile player in Players)
            {
                player.IsInEvent = true;
                player.CurrentEvent = this;

                player.LogoutLocation = Lobby;
                player.LogoutMap = LobbyMap;

                player.Hits = player.HitsMax;
                player.Mana = player.ManaMax;
                player.Stam = player.StamMax;
                player.Blessed = false;
            }
            SendPlayerGumps();
            m_Running = true;
            BeginGame = false;
        }

        public void EndGameCommand()
        {
            SendGMGump();
            SendPlayerEndGameGump();
            EndGame();
            OnAfterEnd();
        }

        public virtual void EndGame()
        {
            if (!Running)
                return;

            m_Winners.Clear();

            if (!(this is BaseTeamGame))
            {
                foreach (Mobile player in Players)
                {
                    if (player is PlayerMobile)
                    {
                        PlayerMobile playermobile = player as PlayerMobile;
                        m_Winners.Add(playermobile);
                    }
                }
            }

            AnnounceToPlayers("The game is over.");
        }

        public virtual void OnAfterEnd()
        {
            m_Winners.Sort();
            m_Winners.Reverse();
            
            if(m_Winners.Count >= 1 && m_Winners[0] != null)
                AnnounceToPlayers(m_Winners[0] + " has won the game with " + m_Winners[0].Score + " points!");
            if (m_Winners.Count >= 2 && m_Winners[1] != null)
                AnnounceToPlayers(m_Winners[1] + " got the second place with " + m_Winners[1].Score + " points!");

            GivePrizesToPlayers();

            ClearGame();

            if (EndGameEvent != null)
                EndGameEvent(new EndGameEventArgs(this));
        }

        public virtual void ClearGame()
        {
            if (m_GameTimer != null)
            {
                m_GameTimer.Stop();
                m_GameTimer = null;
            }

            if (m_ScoreTimer != null)
            {
                m_ScoreTimer.Stop();
                m_ScoreTimer = null;
            }

            if (m_GumpTimer != null)
            {
                m_GumpTimer.Stop();
                m_GumpTimer = null;
            }

            m_Winners.Clear();
            m_PlayerScores.Clear();

            List<Mobile> toRemove = new List<Mobile>();
            foreach (Mobile player in Players)
            {
                toRemove.Add(player);
            }

            foreach (Mobile player in toRemove)
            {
                RemovePlayer(player);
            }
            m_RunningGames.Remove(this);
            m_Running = false;
            Open = false;
            BeginGame = false;
            m_GameMaster = null;
        }
        #endregion

        #region announcements
        public virtual void AnnounceScore(Mobile m)
        {
            m.SendAsciiMessage(0x489, "This game does not use a score.");
        }

        public virtual void AnnounceScore()
        {

        }

        public virtual void AnnounceTimeLeft()
        {
            if (Length.TotalSeconds > 0)
                AnnounceToPlayers("Time left: {0:0}:{1:00}:{2:00}", (int)(TimeLeft.TotalSeconds / 60 / 60), (int)(TimeLeft.TotalSeconds / 60) % 60, (int)(TimeLeft.TotalSeconds) % 60);
        }

        public void AnnounceToPlayers(string message)
        {
            AnnounceToPlayers(0x489, message);
        }

        public void AnnounceToPlayers(string message, params object[] args)
        {
            AnnounceToPlayers(0x489, String.Format(message, args));
        }

        public void AnnounceToPlayers(int hue, string message, params object[] args)
        {
            AnnounceToPlayers(hue, String.Format(message, args));
        }

        public void AnnounceToPlayers(int hue, string message)
        {
            foreach(PlayerMobile player in m_Players)
                player.SendAsciiMessage(hue, message);

            if (m_MsgStaff)
                Server.Commands.CommandHandlers.BroadcastMessage(AccessLevel.Counselor, hue, message);
        }
        #endregion

        #region player administration
        public virtual void AddPlayer(Mobile player)
        {
            if (Open && !Players.Contains(player))
            {
                if (((PlayerMobile)player).CurrentEvent != null)
                    throw new EventException("The player " + player.Name + " is already in an event");
                m_Players.Add(player);
            }
            if (GiveSupplies && !m_PlayerSupplies.ContainsKey((PlayerMobile)player))
            {
                GiveSuppliesToPlayer(player);
            }
            if (Running)
            {
                ((PlayerMobile)player).CurrentEvent = this;

                player.LogoutLocation = Lobby;
                player.LogoutMap = LobbyMap;

                player.Hits = player.HitsMax;
                player.Mana = player.ManaMax;
                player.Stam = player.StamMax;
                player.Blessed = false;
            }
            else
            {
                player.Blessed = true;
            }
            SendPlayerGumps();
        }

        public virtual void RemovePlayer(Mobile player)
        {
            if (player == null || player.Deleted)
                return;

            if (!Players.Contains(player))
                return;

            if (!player.Alive)
                player.Resurrect();

            if (player is PlayerMobile)
            {
                ((PlayerMobile)player).CurrentEvent = null;
                ((PlayerMobile)player).IsInEvent = false;
                ((PlayerMobile)player).Score = 0;
            }

            player.Blessed = false;

            if (player.Map != Map.Internal)
            {
                player.Location = m_Lobby;
                player.Map = m_LobbyMap;
            }
            else
            {
                player.LogoutLocation = m_Lobby;
                player.LogoutMap = m_LobbyMap;
            }
            m_Players.Remove(player);

            if (m_PlayerSupplies.ContainsKey((PlayerMobile)player))
            {
                SupplySystem.RemoveEventGear(player);
                EquipmentStorage storage = m_PlayerSupplies[(PlayerMobile)player];
                if (storage != null)
                {
                    storage.OnDoubleClick(player);
                    m_PlayerSupplies.Remove((PlayerMobile)player);
                }
            }
            player.Delta(MobileDelta.Noto);
            SendPlayerGumps();
        }

        public void GiveSuppliesToPlayer(Mobile player)
        {
            EquipmentStorage storage;
            if (m_PlayerSupplies.ContainsKey(player))
            {
                storage = m_PlayerSupplies[(PlayerMobile)player];
                if (storage != null)
                {
                    storage.OnDoubleClick(player);
                }
                m_PlayerSupplies.Remove((PlayerMobile)player);
            }

            player.IsInEvent = true;
            storage = Supplier.Supply(player);
            m_PlayerSupplies.Add((PlayerMobile)player, storage);
        }

        public void SetPlayerScore(Mobile player, int score)
        {
            if (!m_PlayerScores.ContainsKey(player))
                m_PlayerScores.Add(player, score);
            else
                m_PlayerScores[player] = score;
        }

        public void AddPlayerScore(Mobile player, int score)
        {
            if (!m_PlayerScores.ContainsKey(player))
                m_PlayerScores.Add(player, score);
            else
                m_PlayerScores[player] += score;
        }

        public void SubtractPlayerScore(Mobile player, int score)
        {
            if (!m_PlayerScores.ContainsKey(player))
                m_PlayerScores.Add(player, -score);
            else
                m_PlayerScores[player] -= score;
        }

        public int GetPlayerScore(Mobile player)
        {
            if(m_PlayerScores.ContainsKey(player))
                return m_PlayerScores[player];
            return 0;
        }

        public void SendGMGump()
        {
            if(GameMaster != null)
                OnDoubleClick(GameMaster);
        }

        public void SendPlayerGumps()
        {
            if (!m_UseGump)
                return;
            foreach (Mobile player in Players)
            {
                SendPlayerGump(player);
            }
        }

        public void SendPlayerEndGameGump()
        {
            if (!m_UseGump)
                return;
            foreach (Mobile player in Players)
            {
                player.SendGump(new GameInfoGump(player, this));
            }
        }

        public void SendPlayerGump(Mobile player)
        {
            if (!m_UseGump || !(player is PlayerMobile))
                return;
            if(((PlayerMobile)player).GameInfoGumpType != GameInfoGumpType.Disabled)
                player.SendGump(new GameInfoGump(player, this));
        }
        #endregion

        #region abstract methods
        public virtual void OnPlayerDeath(Mobile m)
        {
            SendPlayerGumps();
            SendGMGump();
        }
        public virtual void OnPlayerResurrect(Mobile m)
        {
            SendPlayerGumps();
            SendGMGump();
        }
        public abstract void GivePrizesToPlayers();
        #endregion

        #region events

        public override void OnDoubleClick(Mobile from)
        {
            /*Point3D loc = Location;
            loc.Z -= 50;
            m_Supplier.Location = loc;
            m_Supplier.Map = Map;*/
            if (from.AccessLevel == AccessLevel.Player && !Running && Open)
            {
                try
                {
                    StartGame(from);
                    OnAfterStart();
                }
                catch (EventException e)
                {
                    from.SendMessage(e.Message);
                }
            }
            if (from.AccessLevel >= AccessLevel.Counselor)
            if (!GiveSupplies)
            {
                from.SendGump(new PropertiesGump(from, this));
            }
            else
            {
                from.SendGump(new BaseGameGump(from, this));
            }
            
                
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (Open)
            {
                Open = false;
            }
            if (BeginGame)
            {
                BeginGame = false;
            }
            if (Running)
            {
                EndGameCommand();
            }
            else
            {
                ClearGame();
            }
            if(m_Supplier != null)
                m_Supplier.Delete();
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Time left: {0:00}:{1:00}:{2:00}", (int)(TimeLeft.TotalSeconds / 60 / 60), (int)(TimeLeft.TotalSeconds / 60) % 60, (int)(TimeLeft.TotalSeconds) % 60);
            base.OnSingleClick(from);
        }
        #endregion

        #region Serialize and Deserialize
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)5);//version
            //5
            {
                writer.Write(m_BeginGame);
            }
            //4
            writer.Write(m_PlayerSupplies.Count);
            foreach (PlayerMobile player in m_PlayerSupplies.Keys)
            {
                writer.Write(player);
                writer.Write(m_PlayerSupplies[player]);
            }

            //3
            writer.Write(m_UseGump);
            writer.Write(m_GameName);
            writer.Write((int)m_EventType);
            writer.Write(m_PlayerScores.Count);
            foreach (Mobile player in m_PlayerScores.Keys)
            {
                writer.Write(player);
                writer.Write(m_PlayerScores[player]);
            }

            //2
            writer.Write(Open);
            writer.Write(m_GiveSupplies);
            writer.Write(m_Supplier);

            //1
            writer.Write(m_Players);
            writer.Write(m_GameMaster);
            writer.Write(m_MsgStaff);
            writer.Write(m_Running);
            writer.Write(m_StartTime);
            writer.Write(m_Length);
            writer.Write(m_MaxScore);
            writer.Write(m_Lobby);
            writer.Write(m_LobbyMap);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 5:
                    {
                        m_BeginGame = reader.ReadBool();
                    }
                    goto case 4;
                case 4:
                    {
                        int count = reader.ReadInt();

                        for (int i = 0; i < count; ++i)
                        {
                            PlayerMobile player = reader.ReadMobile() as PlayerMobile;
                            EquipmentStorage storage = reader.ReadItem() as EquipmentStorage;
                            m_PlayerSupplies.Add(player, storage);
                        }
                        goto case 3;
                    }
                case 3:
                    {
                        m_UseGump = reader.ReadBool();
                        m_GameName = reader.ReadString();
                        m_EventType = (EventType)reader.ReadInt();
                        int count = reader.ReadInt();
                        for (int i = 0; i < count; ++i)
                        {
                            m_PlayerScores.Add(reader.ReadMobile(), reader.ReadInt());
                        }
                        goto case 2;
                    }
                case 2:
                    {
                        Open = reader.ReadBool();
                        m_GiveSupplies = reader.ReadBool();
                        m_Supplier = reader.ReadItem() as EventSupplier;
                        goto case 1;
                    }
                case 1:
                    {
                        m_Players = reader.ReadStrongMobileList();
                        m_GameMaster = reader.ReadMobile();
                        m_MsgStaff = reader.ReadBool();
                        m_Running = reader.ReadBool();
                        m_StartTime = reader.ReadDateTime();
                        m_Length = reader.ReadTimeSpan();
                        m_MaxScore = reader.ReadInt();
                        m_Lobby = reader.ReadPoint3D();
                        m_LobbyMap = reader.ReadMap();
                        break;
                    }
            }
        }
        #endregion

        #region timers
        protected class GameTimer : Timer
        {
            private readonly BaseGame m_Game;
            private DateTime m_EndTime;
            private DateTime m_NextUpdate;

            public GameTimer(BaseGame game) : base(TimeSpan.Zero, TimeSpan.FromSeconds(30))
            {
                m_Game = game;
            }

            public void SetEnd(DateTime end)
            {
                m_EndTime = end;
                OnTick();

                if (m_Game.m_UseGump)
                {
                    m_Game.SendGMGump();
                    m_Game.SendPlayerGumps();
                }
            }

            protected override void OnTick()
            {
                if (m_EndTime == DateTime.MinValue)
                {
                    m_EndTime = DateTime.Now + m_Game.Length;
                    m_NextUpdate = DateTime.Now + TimeSpan.FromSeconds(10);
                }

                if (DateTime.Now > m_EndTime)
                {
                    m_Game.EndGameCommand();
                    Stop();
                }

                if (m_EndTime - DateTime.Now < TimeSpan.FromSeconds(60))
                {
                    this.Interval = TimeSpan.FromMilliseconds(500);
                }


                if (DateTime.Now >= m_NextUpdate)
                {
                    if (m_Game.m_UseGump)
                    {
                        m_Game.SendGMGump();
                        m_Game.SendPlayerGumps();
                    }
                    if (m_EndTime - DateTime.Now < TimeSpan.FromSeconds(60))
                        m_NextUpdate = DateTime.Now + TimeSpan.FromSeconds(1);
                    else
                        m_NextUpdate = DateTime.Now + TimeSpan.FromSeconds(10);
                }
            }
        }

        protected class ScoreTimer : Timer
        {
            private readonly BaseGame m_Game;

            public ScoreTimer(BaseGame g)
                : base(TimeSpan.FromMinutes(2.0), TimeSpan.FromMinutes(2.0))
            {
                m_Game = g;
            }

            protected override void OnTick()
            {
                m_Game.AnnounceTimeLeft();
                m_Game.AnnounceScore();
            }
        }

        protected class GameGumpTimer : Timer
        {
            private readonly BaseGame m_Game;

            public GameGumpTimer(BaseGame g)
                : base(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30))
            {
                m_Game = g;
            }

            protected override void OnTick()
            {
                m_Game.GameMaster.SendGump(new BaseGameGump(m_Game.GameMaster, m_Game));
                m_Game.SendPlayerGumps();
            }
        }
        #endregion
    }
}
