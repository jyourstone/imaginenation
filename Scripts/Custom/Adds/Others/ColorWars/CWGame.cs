using System;
using System.Collections;
using System.Collections.Generic;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;

namespace Server.Custom.Games
{
	public class CWGame : BaseTeamGame
	{
        private readonly List<CWTeam> teamskilled = new List<CWTeam>();
        
        private static readonly Hashtable m_Table = new Hashtable();

        #region Getters & Setters
        [CommandProperty(AccessLevel.Counselor)]
        public int ResDelay { get; set; }

        [CommandProperty(AccessLevel.Counselor)]
        public int LastStandingPoints { get; set; }

        [CommandProperty(AccessLevel.Counselor)]
        public int KillPoints { get; set; }

        [CommandProperty(AccessLevel.Counselor)]
        public int DeathPoints { get; set; }
        #endregion

        [Constructable]
		public CWGame( int numTeams ) : base( 0xEDC )
		{
			Length = TimeSpan.FromMinutes( 30.0 );

			for(int i=0;i<numTeams;i++)
				Teams.Add( new CWTeam( this, "CW Team " + i ) );

			Movable = false;
			Name = "Color Wars Game Control Stone";

            MaxScore = 60;
            ResDelay = 0;
            LastStandingPoints = 15;
            KillPoints = 2;
            DeathPoints = 1;
            EventType = EventType.CaptureTheFlag;
            GameName = "Color Wars";
		}

        public CWGame(Serial serial)
            : base(serial)
        {
        }

	    public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 );//version
            
            //0
            writer.Write( ResDelay );
            writer.Write( LastStandingPoints );
	        writer.Write( KillPoints );
            writer.Write( DeathPoints );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
                case 0:
                {
                    ResDelay = reader.ReadInt();
                    LastStandingPoints = reader.ReadInt();
                    KillPoints = reader.ReadInt();
                    DeathPoints = reader.ReadInt();
					break;
				}
			}
		}

	    public override void OnAfterDelete()
		{
            teamskilled.Clear();

			base.OnAfterDelete();
		}

        public override void OnPlayerDeath(Mobile killed)
        {
            base.OnPlayerDeath(killed);

            Mobile killer = killed.LastKiller;
            CWTeam killerteam = (CWTeam)GetTeam(killer);
            CWTeam killedteam = (CWTeam)GetTeam(killed);

            if (killedteam == null)
                return;
            
            if (ResDelay <= 0)
            {
                bool teamkilled = true;

                for (int i = 0; i < killedteam.Players.Count; i++)
                {
                    Mobile m = (Mobile)killedteam.Players[i];

                    if (m.Alive)
                    {
                        teamkilled = false;
                        break;
                    }
                }

                if (teamkilled)
                {
                    AnnounceToPlayers(string.Format("{0} got taken out!", killedteam.Name));
                    teamskilled.Add(killedteam);
                }

                if (teamskilled.Count >= (TeamCount - 1))
                {
                    AnnounceToPlayers(string.Format("{0} is the only team standing and gets {1} points", killerteam.Name, LastStandingPoints));
                    killerteam.Score += LastStandingPoints;
                    teamskilled.Clear();
                    Reset();
                }
            }
            else
            {
                RemoveTimer(killed);

                Timer t = new DeathTimer(killed, killedteam);
                m_Table[killed] = t;
                t.Start();
            }

            if (killerteam != null && killerteam != killedteam)
            {
                killerteam.Score += KillPoints;
                AddPlayerScore(killer, KillPoints);
            }

            if (((CWGame)killedteam.Game).DeathPoints > 0 && killerteam != killedteam)
            {
                SubtractPlayerScore(killed, DeathPoints);
                killedteam.Score -= DeathPoints;
            }
        }

        public override void GivePrizesToPlayers()
        {
            if (m_Winners.Count < 2)
                return;
            BaseGameTeam first = m_Winners[0] as BaseGameTeam;
            BaseGameTeam second = m_Winners[1] as BaseGameTeam;
            if (first == null || second == null)
                return;
            int score = first.Score;
            if (first.Score == second.Score)
            {
                foreach (BaseGameTeam team in Teams)
                {
                    foreach (Mobile player in team.Players)
                    {
                        if (player is PlayerMobile)
                        {
                            Container bank = ((PlayerMobile)player).BankBox;
                            const int gold = 5000;
                            bank.AddItem(new BankCheck(gold));
                            player.SendAsciiMessage("A bank check worth {0} gold has been added in your bank", gold);
                        }
                    }
                }
            }
            else
            {
                foreach (Mobile player in first.Players)
                {
                    if (player is PlayerMobile)
                    {
                        Container bank = ((PlayerMobile)player).BankBox;
                        const int gold = 7000;
                        bank.AddItem(new BankCheck(gold));
                        player.SendAsciiMessage("A bank check worth {0} gold has been added in your bank", gold);
                    }
                }
            }
        }

	    public void Reset()
	    {
            for (int i = 0; i < Teams.Count; i++)
            {
                CWTeam team = (CWTeam)Teams[i];
                bool home1 = true;

                for (int j = 0; j < team.Players.Count; j++ )
                {
                    Mobile m = (Mobile)team.Players[j];

                    if (home1)
                    {
                        m.Location = team.Home;
                        m.LogoutLocation = team.Home;

                        if (team.Home2 != Point3D.Zero)
                            home1 = false;
                    }
                    else
                    {
                        m.Location = team.Home2;
                        m.LogoutLocation = team.Home2;
                        home1 = true;
                    }

                    m.Map = team.HomeMap;

                    if (m.Corpse != null && !m.Corpse.Deleted)
                        m.Corpse.Delete();

                    if (!m.Alive && m is PlayerMobile)
                        ((PlayerMobile)m).ForceResurrect();

                    m.Hits = m.HitsMax;
                    m.Mana = m.ManaMax;
                    m.Stam = m.StamMax;
                }
            }
	    }

	    public static void RemoveTimer(Mobile m)
        {
            Timer t = (Timer)m_Table[m];

            if (t != null)
            {
                t.Stop();
                m_Table.Remove(m);
            }
        }

		public override void EndGame()
		{
            base.EndGame();

            teamskilled.Clear();
		}

		public override void StartGame( Mobile from )
		{
            base.StartGame( from );

		    ResetGame();
		}

		public void ResetGame()
		{
		    string kp = KillPoints > 1 ? "points" : "point";
            string dp = DeathPoints == 1 ? "point" : "points";

            AnnounceToPlayers("The game has started.");
            AnnounceToPlayers("Game will end when one of the teams reaches {0} points.", MaxScore);
            if (ResDelay > 0)
            {
                if (DeathPoints == 0)
                    AnnounceToPlayers("Team gets {0} {1} for each kill and looses {2} {3} for each death.", KillPoints, kp, DeathPoints, dp);
                else
                    AnnounceToPlayers("Team gets {0} {1} for each kill.", KillPoints, kp);
            }
            else
            {
                if (DeathPoints == 0)
                    AnnounceToPlayers("Team gets {0} points for killing the other teams, {1} {2} for each kill and looses {3} {4} for each death.", LastStandingPoints, KillPoints, kp, DeathPoints, dp);
                else
                    AnnounceToPlayers("Team gets {0} points for killing the other teams and {1} {2} for each kill.", LastStandingPoints, KillPoints, kp);
            }

            teamskilled.Clear();

			for (int i=0;i<Teams.Count;i++)
			{
                CWTeam team = (CWTeam)Teams[i];

				team.Score = 0;
                bool home1 = true;

				for(int j=0;j<team.Players.Count;j++)
				{
                    Mobile m = (Mobile)team.Players[j];

                    if (home1)
                    {
                        m.Location = team.Home;
                        m.LogoutLocation = team.Home;

                        if (team.Home2 != Point3D.Zero)
                            home1 = false;
                    }
                    else
                    {
                        m.Location = team.Home2;
                        m.LogoutLocation = team.Home2;
                        home1 = true;
                    }

				    m.Map = team.HomeMap;
                    
					if (!m.Alive && m is PlayerMobile)
                        ((PlayerMobile)m).ForceResurrect();

					m.Hits = m.HitsMax;
					m.Mana = m.ManaMax;
					m.Stam = m.StamMax;
				}
			}
		}

        public static Point3D GetTeleportLocation( CWTeam team )
        {
            Point3D home = team.Home;
            Point3D home2 = team.Home2;

            if (home != Point3D.Zero && home2 != Point3D.Zero)
            {
                if (Utility.RandomDouble() > 0.5)
                    return home;
                return home2;
            }

            if (home != Point3D.Zero)
                return home;

            if (home2 != Point3D.Zero)
                return home2;

            return Point3D.Zero;
        }

		private class DeathTimer : Timer
		{
			private readonly CWTeam m_Team;
			private readonly Mobile m_Mob;

			public DeathTimer( Mobile m, CWTeam t ) : base( TimeSpan.FromSeconds(((CWGame)t.Game).ResDelay) )
			{
				m_Mob = m;
				m_Team = t;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				if ( m_Mob.Corpse != null && !m_Mob.Corpse.Deleted )
					m_Mob.Corpse.Delete();

				if ( !m_Mob.Alive && m_Team.Game.Running && m_Mob is PlayerMobile )
				{
				    m_Mob.Location = GetTeleportLocation(m_Team);
					m_Mob.Map = m_Team.HomeMap;
					((PlayerMobile)m_Mob).ForceResurrect();
					m_Mob.Hits = m_Mob.HitsMax;
					m_Mob.Mana = m_Mob.ManaMax;
				}
			}
		}
	}
}
