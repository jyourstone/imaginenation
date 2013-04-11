using System;
using System.Collections.Generic;
using System.Text;
using Server.Mobiles;
using Server.Misc;
using Server.Items;

namespace Server.Custom.Games
{
    public abstract class BaseTeamGame : BaseGame
    {
        private List<BaseGameTeam> m_Teams = new List<BaseGameTeam>();

        public BaseTeamGame(int itemid) : base(itemid)
        {

        }

        public BaseTeamGame(Serial serial)
            : base(serial)
        {

        }

        #region properties
        [CommandProperty(AccessLevel.GameMaster)]
        public List<BaseGameTeam> Teams
        {
            get
            {
                return m_Teams;
            }
            set
            {
                m_Teams = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TeamCount
        {
            get
            {
                return m_Teams.Count;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, false)]
        public override bool Open
        {
            set
            {
                base.Open = value;
                if (Open)
                {
                    foreach (BaseGameTeam team in Teams)
                    {
                        team.CreateMoongate();
                    }
                }
                else
                {
                    foreach (BaseGameTeam team in Teams)
                    {
                        team.RemoveMoongate();
                    }
                }
            }
            get
            {
                return base.Open;
            }
        }
        #endregion

        #region start and endgame
        public override void StartGame(Mobile from)
        {
            base.StartGame(from);

            if (m_ScoreTimer == null)
                m_ScoreTimer = new ScoreTimer(this);
            m_ScoreTimer.Start();
        }

        public override void OnAfterStart()
        {
            base.OnAfterStart();

            foreach (BaseGameTeam team in Teams)
            {
                foreach (Mobile player in team.Players)
                {
                    player.Location = team.Home;
                    player.Map = team.HomeMap;
                }
            }
        }

        public override void EndGame()
        {
            base.EndGame();

            /*List<KeyValuePair<BaseGameTeam, int>> teamscores = new List<KeyValuePair<BaseGameTeam, int>>();

            foreach (BaseGameTeam t in Teams)
            {
                teamscores.Add(new KeyValuePair<BaseGameTeam,int>(t, t.Score));
            }
            teamscores.Sort((firstPair, nextPair) =>
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            }
            );

            foreach (KeyValuePair<BaseGameTeam, int> keyvaluepair in teamscores)
            {
                m_Winners.Insert(0, keyvaluepair.Key);
            }*/

            foreach (BaseGameTeam team in Teams)
            {
                m_Winners.Add(team);
            }
        }

        public override void ClearGame()
        {
            base.ClearGame();

            foreach (BaseGameTeam team in m_Teams)
            {
                team.Remove();
            }
        }
        #endregion

        #region team and player administration
        public override void AddPlayer(Mobile player)
        {
            if (Open && !Players.Contains(player))
            {
                if (((PlayerMobile)player).CurrentEvent != null)
                    throw new EventException("The player " + player.Name + " is already in an event");
            }
            if (GiveSupplies && !m_PlayerSupplies.ContainsKey((PlayerMobile)player))
                GiveSuppliesToPlayer(player, GetTeam(player));
            base.AddPlayer(player);
        }

        public void AddTeam(BaseGameTeam team)
        {
            m_Teams.Add(team);
        }

        public void RemoveTeam(BaseGameTeam team)
        {
            m_Teams.Remove(team);
        }

        public override void RemovePlayer(Mobile player)
        {
            base.RemovePlayer(player);

            foreach (BaseGameTeam team in m_Teams)
                if (team.HasPlayer(player))
                    team.RemovePlayer(player);
        }

        public BaseGameTeam GetTeam(Mobile m)
        {
            foreach (BaseGameTeam team in m_Teams)
            {
                if (team.HasPlayer(m))
                    return team;
            }
            return null;
        }

        public void SwitchTeam(Mobile player, BaseGameTeam team)
        {
            BaseGameTeam oldTeam = GetTeam(player);
            if (oldTeam == team)
                return;
            if (oldTeam != null)
            {
                oldTeam.RemovePlayer(player);
            }
            team.AddPlayer(player);
        }

        public void GiveSuppliesToPlayer(Mobile player, BaseGameTeam team)
        {
            if (player == null || team == null)
                return;
            Supplier.ClothHue = team.Hue;
            Supplier.TeamName = team.Name;

            EquipmentStorage storage = Supplier.Supply(player);
            m_PlayerSupplies.Add((PlayerMobile)player, storage);

            player.Location = team.Home;
            player.Map = team.HomeMap;
        }
        #endregion

        #region announcements
        public override void AnnounceScore(Mobile m)
        {
            m.SendAsciiMessage(0x489, string.Format("Time left: {0:0}:{1:00}:{2:00}", (int)(TimeLeft.TotalSeconds / 60 / 60), (int)(TimeLeft.TotalSeconds / 60) % 60, (int)(TimeLeft.TotalSeconds) % 60));
            foreach(BaseGameTeam team in Teams)
                m.SendAsciiMessage(0x489, string.Format("{0}: {1} points", team.Name, team.Score));
        }

        public override void AnnounceScore()
        {
            AnnounceToPlayers(string.Format("Time left: {0:0}:{1:00}:{2:00}", (int)(TimeLeft.TotalSeconds / 60 / 60), (int)(TimeLeft.TotalSeconds / 60) % 60, (int)(TimeLeft.TotalSeconds) % 60), 0x489);
            foreach (BaseGameTeam team in Teams)
                AnnounceToPlayers(string.Format("{0}: {1} points", team.Name, team.Score), 0x489);
        }
        #endregion

        #region events
        public override void OnAfterDelete()
        {
            m_Teams.Clear();

            base.OnAfterDelete();
        }
        #endregion

        #region serialize and deserialize
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1);//version

            //1
            writer.Write(m_Teams.Count);
            foreach (BaseGameTeam team in m_Teams)
            {
                writer.Write(team.GetType().FullName);
                team.Serialize(writer);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        int len = reader.ReadInt();
                        for (int i = 0; i < len; i++)
                        {
                            string type = reader.ReadString();
                            Type teamType = Type.GetType(type, true, true);
                            object[] args = new object[] { this, "" };
                            BaseGameTeam team = (BaseGameTeam)Activator.CreateInstance(teamType, args);
                            team.Deserialize(reader);
                            m_Teams.Add(team);
                            if (Open)
                            {
                                team.CreateMoongate();
                            }
                        }
                        break;
                    }
            }
        }
        #endregion
    }
}
