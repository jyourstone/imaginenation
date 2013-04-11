using System;
using System.Collections.Generic;
using System.Text;
using Server.Mobiles;

namespace Server.Custom.Games
{
    public class BaseGameTeam : Participant
    {
        private JoinTeamGate m_Moongate;
        private BaseGame m_Game;

        private List<Mobile> m_Players = new List<Mobile>();
        private string m_Name = "-unnamed-";
        private int m_Hue = 0;
        private int m_Score = 0;

        private Point3D m_Home = new Point3D(0, 0, 0);
        private Map m_HomeMap = Map.Felucca;

        private Point3D m_TeamGateLocation = new Point3D(0, 0, 0);
        private Map m_TeamGateMap = Map.Malas;

        public BaseGameTeam(BaseGame game, string name)
        {
            m_Game = game;
            m_Name = name;
        }

        #region properties
        [CommandProperty(AccessLevel.GameMaster)]
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
                if(m_Moongate != null)
                    m_Moongate.Name = "Join " + value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Home
        {
            get
            {
                return m_Home;
            }
            set
            {
                if(m_Moongate != null)
                    m_Moongate.PointDest = value;
                m_Home = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map HomeMap
        {
            get
            {
                return m_HomeMap;
            }
            set
            {
                if (m_Moongate != null)
                    m_Moongate.MapDest = value;
                m_HomeMap = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D TeamGateLocation
        {
            get
            {
                return m_TeamGateLocation;
            }
            set
            {
                m_TeamGateLocation = value;
                if (m_Moongate != null)
                    m_Moongate.Location = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map TeamGateMap
        {
            get
            {
                return m_TeamGateMap;
            }
            set
            {
                m_TeamGateMap = value;
                if (m_Moongate != null)
                    m_Moongate.Map = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Score
        {
            get
            {
                return m_Score;
            }
            set
            {
                m_Score = value;
                if (m_Score >= Game.MaxScore)
                    Game.EndGameCommand();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Hue
        {
            get
            {
                return m_Hue;
            }
            set
            {
                m_Hue = value;
                if (m_Moongate != null)
                    m_Moongate.Hue = value;
            }
        }
        public BaseGame Game
        {
            get
            {
                return m_Game;
            }
            set
            {
                m_Game = value;
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
        #endregion

        #region player administration
        public void AddPlayer(Mobile player)
        {
            if (Game.Open && !m_Players.Contains(player))
            {
                m_Players.Add(player);
                player.SendMessage("You have joined team " + Name);
            }
        }

        public void RemovePlayer(Mobile player)
        {
            if(m_Players.Contains(player))
                m_Players.Remove(player);
        }

        public bool HasPlayer(Mobile m)
        {
            return m_Players.Contains(m);
        }

        public void Remove()
        {
            List<Mobile> toRemove = new List<Mobile>();
            foreach(Mobile player in Players)
            {
                toRemove.Add(player);
            }
            foreach (Mobile player in toRemove)
            {
                Game.RemovePlayer(player);
            }
            Score = 0;
        }

        public void TeamMessage(Mobile sender, String message)
        {
            foreach (PlayerMobile player in m_Players)
            {
                player.SendAsciiMessage(52, "["+sender.Name+"] " + message);
            }
        }
        #endregion

        public void CreateMoongate()
        {
            if (m_Moongate == null)
            {
                m_Moongate = new JoinTeamGate(this);
            }
            m_Moongate.MoveToWorld(TeamGateLocation, TeamGateMap);
        }

        public void RemoveMoongate()
        {
            if (m_Moongate != null)
            {
                m_Moongate.Delete();
                m_Moongate = null;
            }
        }

        #region serialize and deserialize
        public virtual void Serialize(GenericWriter writer)
        {
            writer.Write((int)1);//version

            //1
            writer.Write(m_Players);

            writer.Write(m_Name);
            writer.Write(m_Hue);
            writer.Write(m_Score);
            writer.Write(m_Home);
            writer.Write(m_HomeMap);
            writer.Write(m_TeamGateLocation);
            writer.Write(m_TeamGateMap);
        }

        public virtual void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        goto case 1;
                    }
                case 1:
                    {
                        m_Players = reader.ReadStrongMobileList();
                        m_Name = reader.ReadString();
                        m_Hue = reader.ReadInt();
                        m_Score = reader.ReadInt();
                        m_Home = reader.ReadPoint3D();
                        m_HomeMap = reader.ReadMap();
                        m_TeamGateLocation = reader.ReadPoint3D();
                        m_TeamGateMap = reader.ReadMap();
                        break;
                    }
            }
            if(m_Moongate != null)
                m_Moongate.Team = this;
        }
        #endregion

        public override string ToString()
        {
            return Name;
        }

        #region IComparable Members

        public int CompareTo(Participant p)
        {
            return Score.CompareTo(p.Score);
        }

        #endregion
    }
}
