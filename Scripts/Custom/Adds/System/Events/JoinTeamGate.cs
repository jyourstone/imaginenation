using System;
using System.Collections.Generic;
using System.Text;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Games
{
    class JoinTeamGate : Teleporter
    {
        private BaseGameTeam m_Team;

        public JoinTeamGate()
            : base()
        {
            ItemID = 3948;
            Visible = true;
        }

        public JoinTeamGate(BaseGameTeam team)
            : this()
        {
            Team = team;
        }

        public JoinTeamGate(Serial serial)
            : base(serial)
        {

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            Delete();
        }

        public BaseGameTeam Team
        {
            get
            {
                return m_Team;
            }
            set
            {
                m_Team = value;
                Hue = m_Team.Hue;
                Name = "Join " + m_Team.Name;
                PointDest = m_Team.Home;
                MapDest = m_Team.HomeMap;
                Location = m_Team.TeamGateLocation;
                Map = m_Team.TeamGateMap;
            }
        }

        public override void StartTeleport(Mobile m)
        {
            if (Team == null || Team.Game == null)
                return;

            if (!Team.Game.Open)
            {
                m.SendMessage("The game is closed.");
                return;
            }
            if (((PlayerMobile)m).CurrentEvent != null)
            {
                m.SendMessage("You are already in another game.");
                return;
            }
            if (Team.Game.Players.Contains(m))
            {
                m.SendMessage("You are already in the game.");
                return;
            }

            base.StartTeleport(m);

            Team.AddPlayer(m);
            m.IsInEvent = true;
            Team.Game.AddPlayer(m);
        }

        public override void Delete()
        {
            base.Delete();
        }
    }
}
