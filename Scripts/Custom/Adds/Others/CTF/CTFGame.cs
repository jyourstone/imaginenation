using System;
using System.Collections;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.Games
{
	public class CTFGame : BaseTeamGame
	{

		//public static ArrayList Registry{ get{ return m_Registry; } }
        /*
		public static void Initialize()
		{
            CommandSystem.Register("kickctf", AccessLevel.Counselor, KickCtf_Command);
            CommandSystem.Register("ctfscore", AccessLevel.Counselor, CtfScore_Command);
            CommandSystem.Register("endgame", AccessLevel.Counselor, EndGame_Command);
            CommandSystem.Register("startgame", AccessLevel.Counselor, StartGame_Command);
            CommandSystem.Register("Team", AccessLevel.Player, TeamMessage_Command);
            CommandSystem.Register("t", AccessLevel.Player, TeamMessage_Command);
		}

        private static void CtfScore_Command( CommandEventArgs e )
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, CtfScore_Target);
            e.Mobile.SendMessage("Target the game control stone to announce scores.");
        }

        private static void CtfScore_Target(Mobile from, object o)
        {
            if (o is CTFGame)
            {
                CTFGame game = (CTFGame)o;
                game.AnnounceScore();
            }
            else
            {
                from.BeginTarget(-1, false, TargetFlags.None, EndGame_Target);
                from.SendMessage("Target the game stone.");
            }
        }

		private static void TeamMessage_Command( CommandEventArgs e )
		{
			string msg = e.ArgString;
			if ( msg == null )
				return;
			msg = msg.Trim();
			if ( msg.Length <= 0 )
				return;
			
			CTFTeam team = FindTeamFor( e.Mobile );
            if (team != null)
            {
                msg = String.Format("Team [{0}]: {1}", e.Mobile.Name, msg);
                for (int m = 0; m < team.Members.Count; m++)
                    ((Mobile)team.Members[m]).SendMessage(0x35, msg);
            }
            else
                e.Mobile.SendMessage("You are not in a CTF game.");
		}

        private static void KickCtf_Command(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, KickCtf_Target);
            e.Mobile.SendMessage("Target the player to be kicked from CTF match.");
        }

        private static void KickCtf_Target(Mobile from, object o)
        {
            if (o is Mobile)
            {
                Mobile m = (Mobile)o;
                CTFTeam team = FindTeamFor(m);

                if (team != null)
                {
                    // Remove the player from game in here
                    team.Kick(m);

                    m.SendMessage("You have been kicked out of CTF game by {0}.", from.Name);
                    from.SendMessage("The player has been kicked.");
                }
                else
                {
                    from.SendMessage("That player isn't in a CTF game.");
                }
            }
            else
            {
                from.SendMessage("That is not a player.");
            }
        }

		private static void EndGame_Command( CommandEventArgs e )
		{
			e.Mobile.BeginTarget( -1, false, TargetFlags.None, EndGame_Target );
			e.Mobile.SendMessage( "Target the game control stone to END a game." );
		}

		private static void EndGame_Target( Mobile from, object o )
		{
			if ( o is CTFGame )
			{
				CTFGame game = (CTFGame)o;
				game.EndGame();
				from.SendMessage( "The game has been ended." );
			}
			else
			{
				from.BeginTarget( -1, false, TargetFlags.None, EndGame_Target );
				from.SendMessage( "Target the game stone." );
			}
		}

		private static void StartGame_Command( CommandEventArgs e )
		{
			if ( e.Arguments.Length < 1 )
			{
				e.Mobile.SendMessage( "Usage: startgame <ResetTeams>" );
				e.Mobile.SendMessage( "So, if you want to start the game and force everyone to choose a new team, do [startgame true" );
			}
			
			string str = e.GetString( 0 ).ToUpper().Trim();
			bool reset;
			if ( str == "YES" || str == "TRUE" || str == "Y" || str == "T" || str == "1" )
				reset = true;
			else
				reset = false;

			e.Mobile.BeginTarget( -1, false, TargetFlags.None, StartGame_Target, reset );
			e.Mobile.SendMessage( "Target the game control stone to START a game." );
		}

		private static void StartGame_Target( Mobile from, object o, object state )
		{
			bool reset = state is bool ? (bool)state : false;

			if ( o is CTFGame )
			{
				CTFGame game = (CTFGame)o;
				game.StartGame( reset );
				from.SendMessage( "The game has been started." );
			}
			else
			{
				from.BeginTarget( -1, false, TargetFlags.None, StartGame_Target, reset );
				from.SendMessage( "Target the game stone." );
			}
		}
        */

        private static Hashtable m_Table = new Hashtable();

		private bool m_DeathPoint;
        private TimeSpan m_DeathDelay;

		[Constructable]
		public CTFGame( int numTeams ) : base( 0xEDC )
		{
            for (int i = 0; i < numTeams; ++i)
                Teams.Add(new CTFTeam(this, "CTF Team " + i));
			Length = TimeSpan.FromHours( 1.0 );
            DeathDelay = TimeSpan.FromSeconds(30);
            
			Name = "CTF Game Control Stone";

            MaxScore = 45;
            EventType = EventType.CaptureTheFlag;
            GameName = "Capture The Flag";
		}

		public CTFGame( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 );//version

            //1
            writer.Write(m_DeathDelay);

			//0
			writer.Write( m_DeathPoint );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
                case 1:
                {
                    m_DeathDelay = reader.ReadTimeSpan();
                    goto case 0;
                }
				case 0:
				{
					m_DeathPoint = reader.ReadBool();
					break;
				}
			}

            if (Running)
            {
                foreach (CTFTeam team in Teams)
                {
                    CTFFlag flag = new CTFFlag(this, team);
                    flag.EventItem = true;
                }
            }
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();
		}

        public override void OnPlayerDeath(Mobile m)
        {
            base.OnPlayerDeath(m);

            CTFTeam team = GetTeam(m) as CTFTeam;

            if (team == null)
                return;

            RemoveTimer(m);

            Timer t = new DeathTimer(m, team, this, DeathDelay);
            m_Table[m] = t;
            t.Start();

            Mobile killer = m.LastKiller;
            CTFTeam killerteam = GetTeam(killer) as CTFTeam;

            if (killerteam != null && team != null && ((CTFGame)killerteam.Game).DeathPoint && killerteam != team)
            {
                ++killerteam.Score;
                AddPlayerScore(killer, 1);
                SendGMGump();
                SendPlayerGumps();
            }
        }

        public override void OnPlayerResurrect(Mobile m)
        {
            base.OnPlayerResurrect(m);

            RemoveTimer(m);
        }

        public override void RemovePlayer(Mobile player)
        {
            base.RemovePlayer(player);

            if (player.Backpack != null)
            {
                List<Item> packlist = player.Backpack.Items;
                for (int i = 0; i < packlist.Count; ++i)
                {
                    if (packlist[i] is CTFFlag) //Player has the flag, return it to home
                    {
                        ((CTFFlag)packlist[i]).ReturnToHome();
                        break;
                    }
                }
            }
            player.SolidHueOverride = -1;
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

        public static void RemoveTimer(Mobile m)
        {
            Timer t = (Timer)m_Table[m];

            if (t != null)
            {
                t.Stop();
                m_Table.Remove(m);
            }
        }

		[CommandProperty( AccessLevel.GameMaster )]
        public bool DeathPoint { get { return m_DeathPoint; } set { m_DeathPoint = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan DeathDelay { get { return m_DeathDelay; } set { m_DeathDelay = value; } }

		public override void EndGame()
		{
			if ( !Running )
				return;

            base.EndGame();

			foreach(BaseGameTeam t in Teams)
			{
				CTFTeam team = (CTFTeam)t;
                if (team.Flag != null)
                {
                    team.Flag.ReturnToHome();

                    Item flagItem = (Item)team.Flag;
                    flagItem.EventItem = false;
                }
			}
		}

		public override void StartGame( Mobile from )
		{
            base.StartGame(from);

				ResetGame();
		}

		private class StartTimer : Timer
		{
			public static readonly TimeSpan StartDelay = TimeSpan.FromMinutes( 0.5 );
			private readonly CTFGame m_Game;
			private readonly ArrayList m_List;

			public StartTimer( CTFGame game, ArrayList list ) : base( StartDelay )
			{
				m_Game = game;
				m_List = list;
				Priority = TimerPriority.TwoFiftyMS;
			}
			
			protected override void OnTick()
			{
				int sm = -1, ns = -1;
				int[] amc = new int[m_Game.Teams.Count];
				for(int i=0;i<m_Game.Teams.Count;i++)
				{
					amc[i] = ((CTFTeam)m_Game.Teams[i]).ActiveMemberCount;
					if ( sm == -1 || amc[i] < amc[sm] )
					{
						ns = sm;
						sm = i;
					}
					else if ( amc[i] < amc[ns] )
					{
						ns = i;
					}
				}

				for (int i=0;i<m_List.Count;i++)
				{
					Mobile m = (Mobile)m_List[i];

					m.Frozen = false;

					if ( m_Game.GetTeam( m ) == null )
					{
						int t;
						if ( m.NetState == null )
						{
							t = Utility.Random( amc.Length );
						}
						else
						{
							if ( amc[sm] >= amc[ns] )
								t = Utility.Random( amc.Length );
							else
								t = sm;
							amc[t]++;
						}

						CTFTeam team = (CTFTeam)m_Game.Teams[t];

						m_Game.SwitchTeam( m, team );
						m.SendMessage( "You have joined {0}!", team.Name );
					}
				}
				m_Game.ResetGame();
			}
		}

		public void ResetGame()
		{
			AnnounceToPlayers("The game has started.");
            AnnounceToPlayers("Game will end when one of the teams reaches {0} Score.", MaxScore);
            AnnounceToPlayers("Team gets 15 Score for each flag captured and 1 point for each kill.");

			for (int i=0;i<Teams.Count;i++)
			{
				CTFTeam team = (CTFTeam)Teams[i];

				team.Score = 0;
                if (team.Flag != null)
                {
                    team.Flag.ReturnToHome();

                    Item flagItem = (Item)team.Flag;
                    flagItem.EventItem = true;
                }
                else
                {
                    CTFFlag flag = new CTFFlag(this, team);
                    flag.EventItem = true;
                }
			}
		}

		private class DeathTimer : Timer
		{
            private readonly CTFGame m_Game;
			private readonly CTFTeam m_Team;
			private readonly Mobile m_Mob;
            private readonly DateTime m_Res;
            private CustomGumpItem m_GumpItem;
            private DateTime m_LastSecond = DateTime.Now;

			public DeathTimer( Mobile m, CTFTeam t, CTFGame g, TimeSpan DeathDelay ) : base( TimeSpan.Zero, TimeSpan.FromMilliseconds(250) )
			{
				m_Mob = m;
				m_Team = t;
                m_Game = g;
                m_Res = DateTime.Now + DeathDelay;

                if (m is PlayerMobile)
                {
                    m_GumpItem = new CustomGumpItem("Resurrect in", DeathDelay.Hours + ":" + DeathDelay.Minutes + ":" + DeathDelay.Seconds, m_Game, (PlayerMobile)m);
                    GameInfoGump.AddCustomMessage((PlayerMobile)m, m_GumpItem);
                }
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
                TimeSpan delay = m_Res - DateTime.Now;
                if (DateTime.Now - m_LastSecond > TimeSpan.FromSeconds(1))
                {
                    m_GumpItem.Message = delay.Hours + ":" + delay.Minutes + ":" + delay.Seconds;
                    m_LastSecond = DateTime.Now;
                }
                if (DateTime.Now > m_Res)
                {
                    if (m_Mob.Corpse != null && !m_Mob.Corpse.Deleted)
                        m_Mob.Corpse.Delete();

                    if (!m_Mob.Alive)
                    {
                        m_Mob.Location = m_Team.Home;
                        m_Mob.Map = m_Team.HomeMap;
                        m_Mob.Resurrect();
                        m_Mob.Hits = m_Mob.HitsMax;
                        m_Mob.Mana = m_Mob.ManaMax;
                    }
                    GameInfoGump.RemoveCustomMessage((PlayerMobile)m_Mob, m_GumpItem);
                    Stop();
                }
			}
		}
	}
}
