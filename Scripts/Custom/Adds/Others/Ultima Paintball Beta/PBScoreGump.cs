/*********************************************************************************/
/*                                                                               */
/*                              Ultima Paintball 						         */
/*                        Created by Aj9251 (Disturbed)                          */         
/*                                                                               */
/*                                 Credits:                                      */
/*                   Original Idea + Some Code - A_Li_N                          */
/*                   Some Ideas + Code - Aj9251 (Disturbed)                      */
/*********************************************************************************/

using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using System.Collections;

namespace Server.Games.PaintBall
{
	public class PBScoreGump : Gump
	{
		private PBGameItem m_PBGI;
		private PBScoreBoard m_PBSB;

		public PBScoreGump( PBGameItem pbgi, PBScoreBoard Pbsb )	: base( 0, 0 )
		{
			m_PBGI = pbgi;
			m_PBSB = Pbsb;
			
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;

			AddPage(0);

			AddBackground(0, 0, 595, 400, 2600);
			AddLabel(233, 20, 1153, "Paintball Scoreboard");

			AddBackground(13, 50, 135, 338, 2620);
			AddBackground(159, 50, 135, 338, 2620);
			if( m_PBGI.Teams > 2 )
				AddBackground(305, 50, 135, 338, 2620);
			if( m_PBGI.Teams > 3 )
				AddBackground(450, 50, 135, 338, 2620);

			AddPlayers();
			Pbsb.InvalidateProperties();
		}

		private void AddPlayers()
		{
			int team, Team1, Team2, Team3, Team4;
		    Team1 = Team2 = Team3 = Team4 = 0;
			int team1hue, team2hue, team3hue, team4hue;
			team1hue = team2hue = team3hue = team4hue = 0;
			
			team1hue = m_PBGI.m_Team1Hue;
			team2hue = m_PBGI.m_Team2Hue;
			team3hue = m_PBGI.m_Team3Hue;
			team4hue = m_PBGI.m_Team4Hue;
			
			ArrayList PlayerTotal;
			PlayerTotal = new ArrayList();
			
			Item cloak;
			if ( m_PBGI.Players != null )
			{
			foreach(Mobile mob in m_PBGI.Players )
			{
				PlayerTotal.Add( mob );
			}
			}
			if ( m_PBGI.NpcPlayers != null )
			{
			foreach(Mobile npc in m_PBGI.NpcPlayers )
			{
				PlayerTotal.Add(npc);
			}
			}
			if ( PlayerTotal != null )
			{
		foreach( Mobile pm in PlayerTotal )
		{
				cloak = pm.FindItemOnLayer( Layer.Cloak );
			if ( cloak != null )
			{
				team = cloak.Hue;
				
			    if( team == team1hue )
			    {
					AddLabel( 25, 95+Team1*20, team1hue, pm.Name );
					Team1 += 1;
			    }

			    else if( team == team2hue )
			    {
					AddLabel( 172, 95+Team2*20, team2hue, pm.Name );
					Team2 += 1;
			    }

			    else if( team == team3hue )
			    {
					AddLabel( 318, 95+Team3*20, team3hue, pm.Name );
					Team3 += 1;
			    }
			    
			    else if( team == team4hue )
			    {
					AddLabel( 463, 95+Team4*20, team4hue, pm.Name );
					Team4 += 1;
			    }
			    else
			    {
			    	
			    }

			}
		}
			}

			AddLabel(44, 70, m_PBGI.m_Team1Hue, m_PBGI.Team1Name + " - " + Team1.ToString());
			AddLabel(188, 70, m_PBGI.m_Team2Hue, m_PBGI.Team2Name + " - "  + Team2.ToString());
			if( m_PBGI.Teams > 2 )
				AddLabel(334, 70, m_PBGI.m_Team3Hue, m_PBGI.Team3Name + " - "  + Team3.ToString());
			if( m_PBGI.Teams > 3 )
				AddLabel(478, 70, m_PBGI.m_Team4Hue, m_PBGI.Team4Name + " - "  + Team4.ToString());
			
		}
	}
}
