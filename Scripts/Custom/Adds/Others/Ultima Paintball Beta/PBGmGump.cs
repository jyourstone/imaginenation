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
using Server.Network;
using Server.Items;
using System.Text;
using Server.Mobiles;
using System.Collections;
using Server.Targeting;
using Server.Commands;
using System.Reflection;
using Server.Prompts;

namespace Server.Games.PaintBall
{

	public class PBGMGump : Gump
    {
      public PBGameItem m_PBGI;

        public PBGMGump( PBGameItem pbgi ) : base( 0, 0 )
        {
        	m_PBGI = pbgi;
        	
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);
			AddBackground(55, 40, 642, 429, 9270);
			AddLabel(292, 66, 42, @"Ultima Paintball Game Control");
			AddButton(94, 103, 4005, 4007, (int)Buttons.Button1, GumpButtonType.Page, 1);
			AddLabel(135, 102, 2307, @"General");
			AddLabel(254, 103, 2307, @"Players");
			AddButton(216, 103, 4005, 4007, (int)Buttons.Button2, GumpButtonType.Page, 2);
			AddButton(324, 103, 4005, 4007, (int)Buttons.Button3, GumpButtonType.Page, 3);
			AddLabel(364, 103, 2307, @"Config");
			AddButton(422, 103, 4005, 4007, (int)Buttons.Button4, GumpButtonType.Page, 4);
			AddLabel(465, 103, 2307, @"Teams");
			AddButton(526, 103, 4005, 4007, (int)Buttons.Button5, GumpButtonType.Page, 5);
			AddLabel(565, 103, 2307, @"About");
			AddPage(1);
			AddButton(80, 427, 2443, 2444, (int)Buttons.Button6, GumpButtonType.Reply, 0);
			AddButton(157, 427, 2443, 2444, (int)Buttons.Button7, GumpButtonType.Reply, 0);
			AddButton(233, 427, 2443, 2444, (int)Buttons.Button8, GumpButtonType.Reply, 0);
			AddButton(307, 427, 2443, 2444, (int)Buttons.Button9, GumpButtonType.Reply, 0);
			AddButton(80, 390, 2443, 2444, (int)Buttons.Button12, GumpButtonType.Reply, 0);
			AddLabel(90, 429, 62, @"Start");
			AddLabel(174, 429, 37, @"Stop");
			AddLabel(243, 429, 52, @"Pause");
			AddLabel(323, 429, 3, @"Join");
			AddLabel(91, 163, 2307, @"Active: ");
		
		
			if ( pbgi.Active == true )
			{
				AddLabel(142, 163, 60, @"Yes");
			}
			else
			{
				AddLabel(142, 163, 60, @"No");
			}
			AddLabel(91, 188, 2307, @"Allow Join:");
			if ( pbgi.CanJoin == true )
			{
			AddLabel(94, 391, 37, @"Close");
				AddLabel(164, 188, 60, @"Yes");
				
			}
			else
			{
			AddLabel(94, 391, 62, @"Open");
				AddLabel(164, 188, 60, @"No");
				
			}
		
			AddLabel(91, 210, 2307, @"Teams:");
			AddLabel(139, 210, 60, pbgi.Teams.ToString());
            AddLabel(91, 232, 2307, @"Mod:");
            AddLabel(139, 232, 60, pbgi.m_Mod.ToString());
			AddBackground(441, 161, 232, 289, 9300);
			AddButton(458, 185, 2443, 2444, (int)Buttons.Button10, GumpButtonType.Reply, 0);
			AddButton(534, 185, 2443, 2444, (int)Buttons.Button11, GumpButtonType.Reply, 0);
			AddLabel(538, 164, 87, @"Prizes");
			AddLabel(477, 185, 267, @"Add");
			AddLabel(550, 185, 137, @"Clear");
			
			Item item;
			for( int i = 0; i < m_PBGI.m_WinnersPrizes.Count; i++ )
			{
				item = m_PBGI.m_WinnersPrizes[i] as Item;
				AddLabel( 450, 221+i*20, 1153, string.Format( "{0} {1}", item.Amount, item.Name ) );
			}
			
			
			
			
			
			AddPage(2);
			AddLabel(112, 161, 2307, @"Team 1");
			AddLabel(265, 161, 2307, @"Team 2");
			AddLabel(419, 161, 2307, @"Team 3");
			AddLabel(574, 161, 2307, @"Team 4");
			AddLabel(191, 136, 4, @"Npc");
			AddLabel(484, 136, 62, @"Player");
			AddLabel(335, 136, 422, @"Staff");
		
			AddBackground(84, 195, 125, 253, 9350);
			AddBackground(233, 195, 125, 253, 9350);
			AddBackground(382, 195, 125, 253, 9350);
			AddBackground(530, 195, 125, 253, 9350);
		
	
			
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
			    	if ( m_PBGI.NpcPlayers.Contains( pm ) )
			    	{
			    		AddLabel( 93, 198+Team1*20, 4, pm.Name );
			    	}
			    	else if ( m_PBGI.Players.Contains( pm ) )
			    	{
			    		if ( pm.AccessLevel >= AccessLevel.Counselor )
			    		{
			    			AddLabel( 93, 198+Team1*20, 422, pm.Name );
			    		}
			    		else
			    		{
			    			AddLabel( 93, 198+Team1*20, 62, pm.Name );
			    		}
			    	}
					
					Team1 += 1;
			    }

			    else if( team == team2hue )
			    {
			    	if ( m_PBGI.NpcPlayers.Contains( pm ) )
			    	{
			    		AddLabel( 241, 198+Team2*20, 4, pm.Name );
			    	}
			    	else if ( m_PBGI.Players.Contains( pm ) )
			    	{
			    		if ( pm.AccessLevel >= AccessLevel.Counselor )
			    		{
			    			AddLabel( 241, 198+Team2*20, 422, pm.Name );
			    		}
			    		else
			    		{
			    			AddLabel( 241, 198+Team2*20, 62, pm.Name );
			    		}
			    	}
					
		
					Team2 += 1;
			    }

			    else if( team == team3hue )
			    {
			   		if ( m_PBGI.NpcPlayers.Contains( pm ) )
			    	{
			    		AddLabel( 386, 198+Team3*20, 4, pm.Name );
			    	}
			    	else if ( m_PBGI.Players.Contains( pm ) )
			    	{
			    		if ( pm.AccessLevel >= AccessLevel.Counselor )
			    		{
			    			AddLabel( 386, 198+Team3*20, 422, pm.Name );
			    		}
			    		else
			    		{
			    			AddLabel( 386, 198+Team3*20, 62, pm.Name );
			    		}
			    	}
					
		
					Team3 += 1;
			    }
			    
			    else if( team == team4hue )
			    {
			    	if ( m_PBGI.NpcPlayers.Contains( pm ) )
			    	{
			    		AddLabel( 538, 198+Team4*20, 4, pm.Name );
			    	}
			    	else if ( m_PBGI.Players.Contains( pm ) )
			    	{
			    		if ( pm.AccessLevel >= AccessLevel.Counselor )
			    		{
			    			AddLabel( 538, 198+Team4*20, 422, pm.Name );
			    		}
			    		else
			    		{
			    			AddLabel( 538, 198+Team4*20, 62, pm.Name );
			    		}
			    	}
					
				
					Team4 += 1;
			    }
			    else
			    {
			    	
			    }

			}
		}
			}
		/*	AddLabel(93, 199, 25, @"Player");
			
			
			AddLabel(241, 198, 25, @"Player");
			
			
			AddLabel(386, 198, 25, @"Player");
			
			
			AddLabel(538, 198, 25, @"Player");
			*/
			
			AddPage(3);
	
			AddButton(312, 160, 4005, 4007, (int)Buttons.Button13, GumpButtonType.Reply, 0);
			AddLabel(352, 160, 88, @"Add Announcer");
			AddLabel(341, 198, 88, @"Add Scoreboard");
			AddButton(312, 220, 4005, 4007, (int)Buttons.Button14, GumpButtonType.Reply, 0);
			AddLabel(349, 222, 87, @"South");
			AddButton(412, 220, 4005, 4007, (int)Buttons.Button15, GumpButtonType.Reply, 0);
			AddLabel(451, 223, 88, @"East");
			AddButton(312, 267, 4005, 4007, (int)Buttons.Button24, GumpButtonType.Reply, 0);
			AddLabel(352, 267, 88, @"Add Npc Player");
			
			AddPage(4);
			AddLabel(108, 148, 160, @"Team 1");
			AddButton(77, 244, 2443, 2444, (int)Buttons.Button16, GumpButtonType.Reply, 0);
			AddLabel(76, 170, 160, @"Name:");
			AddTextEntry(122, 170, 80, 20, 490, 0, m_PBGI.Team1Name);
			AddLabel(77, 197, 160, @"Hue:");
			AddTextEntry(115, 195, 38, 20, 490, 1, m_PBGI.Team1Hue.ToString());
			AddLabel(91, 244, 160, @"Entry");
			AddLabel(83, 272, 160, m_PBGI.Team1Dest.X + " , " + m_PBGI.Team1Dest.Y);
			AddButton(77, 311, 2443, 2444, (int)Buttons.Button17, GumpButtonType.Reply, 0);
			AddLabel(93, 313, 160, @"Exit");
			AddLabel(83, 340, 160, m_PBGI.Exit1Dest.X + " , " + m_PBGI.Exit1Dest.Y);
			AddLabel(258, 145, 160, @"Team 2");
			AddButton(227, 241, 2443, 2444, (int)Buttons.Button18, GumpButtonType.Reply, 0);
			AddLabel(226, 167, 160, @"Name:");
			AddTextEntry(272, 167, 80, 20, 490, 2, m_PBGI.Team2Name);
			AddLabel(227, 194, 160, @"Hue:");
			AddTextEntry(265, 192, 38, 20, 490, 3, m_PBGI.Team2Hue.ToString());
			AddLabel(241, 241, 160, @"Entry");
			AddLabel(233, 269, 160, m_PBGI.Team2Dest.X + " , " + m_PBGI.Team2Dest.Y);
			AddButton(227, 308, 2443, 2444, (int)Buttons.Button19, GumpButtonType.Reply, 0);
			AddLabel(243, 310, 160, @"Exit");
			AddLabel(233, 337, 160, m_PBGI.Exit2Dest.X + " , " + m_PBGI.Exit2Dest.Y);
			AddLabel(403, 144, 160, @"Team 3");
			AddButton(372, 240, 2443, 2444, (int)Buttons.Button20, GumpButtonType.Reply, 0);
			AddLabel(371, 166, 160, @"Name:");
			AddTextEntry(417, 166, 80, 20, 490, 4, m_PBGI.Team3Name);
			AddLabel(372, 193, 160, @"Hue:");
			AddTextEntry(410, 191, 38, 20, 490, 5, m_PBGI.Team3Hue.ToString());
			AddLabel(386, 240, 160, @"Entry");
			AddLabel(378, 268, 160, m_PBGI.Team3Dest.X + " , " + m_PBGI.Team3Dest.Y);
			AddButton(372, 307, 2443, 2444, (int)Buttons.Button21, GumpButtonType.Reply, 0);
			AddLabel(388, 309, 160, @"Exit");
			AddLabel(378, 336, 160, m_PBGI.Exit3Dest.X + " , " + m_PBGI.Exit3Dest.Y);
			AddLabel(556, 144, 160, @"Team 4");
			AddButton(525, 240, 2443, 2444, (int)Buttons.Button22, GumpButtonType.Reply, 0);
			AddLabel(524, 166, 160, @"Name:");
			AddTextEntry(570, 166, 80, 20, 490, 6, m_PBGI.Team4Name);
			AddLabel(525, 193, 160, @"Hue:");
			AddTextEntry(563, 191, 38, 20, 490, 7, m_PBGI.Team4Hue.ToString());
			AddLabel(539, 240, 160, @"Entry");
			AddLabel(531, 268, 160, m_PBGI.Team4Dest.X + " , " + m_PBGI.Team4Dest.Y);
			AddButton(525, 307, 2443, 2444, (int)Buttons.Button23, GumpButtonType.Reply, 0);
			AddLabel(541, 309, 160, @"Exit");
			AddLabel(531, 336, 160, m_PBGI.Exit4Dest.X + " , " + m_PBGI.Exit4Dest.Y);
			AddLabel(113, 416, 53, @"Teams:");
			AddLabel(163, 416, 189, m_PBGI.Teams.ToString());
			
			AddButton(352, 425, 247, 248, (int)Buttons.Button25, GumpButtonType.Reply, 0);
			AddButton(87, 414, 5541, 5542, (int)Buttons.Button26, GumpButtonType.Reply, 0);
			
			
			AddPage(5);
			AddLabel(320, 159, 398, @"Version: 1.1.0 STABLE");


        }

        		public enum Buttons
		{
			Button1,
			Button2,
			Button3,
			Button4,
			Button5,
			Button6,
			Button7,
			Button8,
			Button9,
			Button10,
			Button11,
			Button12,
			Button13,
			Button14,
			Button15,
			Button16,
			Button17,
			Button18,
			Button19,
			Button20,
			Button21,
			Button22,
			Button23,
			Button24,
			Button25,
			Button26,
			
		}


        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            
            if (info.ButtonID == (int)Buttons.Button16 || info.ButtonID == (int)Buttons.Button17 || info.ButtonID == (int)Buttons.Button18 || info.ButtonID == (int)Buttons.Button19 || info.ButtonID == (int)Buttons.Button20
                || info.ButtonID == (int)Buttons.Button21 || info.ButtonID == (int)Buttons.Button22 || info.ButtonID == (int)Buttons.Button23)
            {
            	from.Target = new PBLocTarget( m_PBGI, info.ButtonID );
            }
            else
            {

            switch(info.ButtonID)
            {
               	case (int)Buttons.Button1:
				{

					break;
				}
				case (int)Buttons.Button2:
				{

					break;
				}
				case (int)Buttons.Button3:
				{

					break;
				}
				case (int)Buttons.Button4:
				{

					break;
				}
				case (int)Buttons.Button5:
				{
					
					break;
				}
				case (int)Buttons.Button6:
				{
				m_PBGI.StartGame();
				
				from.SendMessage( "You have started the game." );
				from.SendGump( new PBGMGump( m_PBGI ) );
					break;
				}
				case (int)Buttons.Button7:
				{
				m_PBGI.EndGame(true);
				from.SendMessage( "You have ended the game." );
				foreach( Mobile mob in m_PBGI.Announcers )
				{
		
				mob.PublicOverheadMessage( MessageType.Regular, 0x22, false, "The Game Has Ended!" );
				}
					from.SendGump( new PBGMGump( m_PBGI ) );
					break;
				}
				case (int)Buttons.Button8:
				{
					m_PBGI.Active = false;
				foreach( PlayerMobile pm in m_PBGI.Players )
				{
					pm.Frozen = true;
					pm.Warmode = false;
					pm.SendMessage( "A GM has paused the game" );
				}
				foreach( Mobile mob in m_PBGI.Announcers )
				{
		
				mob.PublicOverheadMessage( MessageType.Regular, 0x22, false, "The Game Has Been Paused!" );
				}
				from.SendMessage( "You have paused the game." );
				from.SendGump( new PBGMGump( m_PBGI ) );
					break;
				}
				case (int)Buttons.Button9:
				{
					// Add staff to game
                    if (m_PBGI.CheckAlreadyPlayer(from))
                    { 
                        from.SendMessage("You have already joined the game.");
                    }
                    else
                    {
                        m_PBGI.AddPlayer(from);
                    }
            		from.SendGump( new PBGMGump( m_PBGI ) );
					break;
				}
				case (int)Buttons.Button10:
				{
					from.Target = new PBPrizeTarget( m_PBGI );
					
					break;
				}
				case (int)Buttons.Button11:
				{
            			m_PBGI.m_WinnersPrizes.Clear();
            		from.SendGump( new PBGMGump( m_PBGI ) );
					break;
				}
				case (int)Buttons.Button12:
				{
            			if	(m_PBGI.CanJoin == true )
            			{
            				m_PBGI.CanJoin = false;
            			}
            			else
            			{
            				m_PBGI.CanJoin = true;
            			}
            		from.SendGump( new PBGMGump( m_PBGI ) );
					break;
				}
				case (int)Buttons.Button13:
				{
            		//Announcer
            		PBAnnouncer pba = new PBAnnouncer( m_PBGI );
            		if ( pba != null )
            		{
            		pba.MoveToWorld( m_PBGI.Location, m_PBGI.Map );
            		}
					from.SendGump( new PBGMGump( m_PBGI ) );
					break;
				}
				case (int)Buttons.Button14:
				{
            			//Score south
            		PBScoreBoard pbss = new PBScoreBoard( m_PBGI, 1 );
            		if ( pbss != null )
            		{
            		pbss.MoveToWorld( m_PBGI.Location, m_PBGI.Map );
            		}
					from.SendGump( new PBGMGump( m_PBGI ) );
					break;
				}
				case (int)Buttons.Button15:
				{
            		//Score East
            		PBScoreBoard pbse = new PBScoreBoard( m_PBGI, 2 );
            		if ( pbse != null )
            		{
            		pbse.MoveToWorld( m_PBGI.Location, m_PBGI.Map );
            		}
					from.SendGump( new PBGMGump( m_PBGI ) );
					break;
				}

            	case (int)Buttons.Button24:
				{
            		PBNpc npc;
            			npc = new PBNpc(m_PBGI);
            			from.SendGump( new PBGMGump( m_PBGI ) );
            			
            	/*if (npc != null && m_PBGI != null )
           
            			{
            			
							m_PBGI.AddPlayer(npc);
							
            			}
            			from.SendGump( new PBGMGump( m_PBGI ) ); */
					break;
				}
            	case (int)Buttons.Button25:
				{
            			
			TextRelay entry0 = info.GetTextEntry(0);
			string text0 = (entry0 == null ? "" : entry0.Text.Trim());
			
			TextRelay entry1 = info.GetTextEntry(1);
            try
            {
           		 m_PBGI.Team1Hue = Convert.ToInt32(entry1.Text);
            }
            catch
            {
                from.SendMessage("Invalid entry for Team 1 Hue.");
            }
            
			
			TextRelay entry2 = info.GetTextEntry(2);
			string text2 = (entry2 == null ? "" : entry2.Text.Trim());
			
			TextRelay entry3 = info.GetTextEntry(3);
	        try
            {
           		 m_PBGI.Team2Hue = Convert.ToInt32(entry3.Text);
            }
            catch
            {
                 from.SendMessage("Invalid entry for Team 2 Hue.");
            }
		
			
			TextRelay entry4 = info.GetTextEntry(4);
			string text4 = (entry4 == null ? "" : entry4.Text.Trim());
			
			TextRelay entry5 = info.GetTextEntry(5);
		    try
            {
           		 m_PBGI.Team3Hue = Convert.ToInt32(entry5.Text);
            }
            catch
            {
                 from.SendMessage("Invalid entry for Team 3 Hue.");
            }
		
			
			TextRelay entry6 = info.GetTextEntry(6);
			string text6 = (entry6 == null ? "" : entry6.Text.Trim());
			
			TextRelay entry7 = info.GetTextEntry(7);
		    try
            {
           		 m_PBGI.Team4Hue = Convert.ToInt32(entry7.Text);
            }
            catch
            {
                 from.SendMessage("Invalid entry for Team 4 Hue.");
            }
			

			
			m_PBGI.m_Team1Name = text0;
			
			
			m_PBGI.m_Team2Name = text2;
		
			
			m_PBGI.m_Team3Name = text4;
	
			
			m_PBGI.m_Team4Name = text6;
		

			
		
			
			
            			from.SendGump( new PBGMGump( m_PBGI ) );
					break;
				}
            	case (int)Buttons.Button26:
				{
            			 from.SendMessage("right 1");
  				  m_PBGI.Teams += 1;
				if( m_PBGI.Teams > 4 || m_PBGI.Teams < 2 )
					m_PBGI.Teams = 2;
  
					from.SendGump( new PBGMGump( m_PBGI ) );
					break;
				}
				
            }
            }
        }
    }
	public class PBLocTarget : Target
		{
			private PBGameItem m_PBGI;
			
			private int m_ID;

			public PBLocTarget( PBGameItem pbgi, int id ) : base( -1, true, TargetFlags.None )
			{
				m_PBGI = pbgi;
	
				m_ID = id;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				IPoint3D t = targeted as IPoint3D;
				if( t == null )
					return;

				Point3D loc = new Point3D( t );
				switch( m_ID )
				{
					case (int)PBGMGump.Buttons.Button16	:	{ from.SendMessage( "Setting Team 1's Entry Point" );	m_PBGI.Team1Dest = loc;	break; }
					case (int)PBGMGump.Buttons.Button18	:	{ from.SendMessage( "Setting Team 2's Entry Point" );	m_PBGI.Team2Dest = loc;	break; }
					case (int)PBGMGump.Buttons.Button20	:	{ from.SendMessage( "Setting Team 3's Entry Point" );	m_PBGI.Team3Dest = loc;	break; }
					case (int)PBGMGump.Buttons.Button22	:	{ from.SendMessage( "Setting Team 4's Entry Point" );	m_PBGI.Team4Dest = loc;	break; }
					case (int)PBGMGump.Buttons.Button17	:	{ from.SendMessage( "Setting Team 1's Exit Point" );	m_PBGI.Exit1Dest = loc;	break; }
					case (int)PBGMGump.Buttons.Button19	:	{ from.SendMessage( "Setting Team 2's Exit Point" );	m_PBGI.Exit2Dest = loc;	break; }
					case (int)PBGMGump.Buttons.Button21	:	{ from.SendMessage( "Setting Team 3's Exit Point" );	m_PBGI.Exit3Dest = loc;	break; }
					case (int)PBGMGump.Buttons.Button23	:	{ from.SendMessage( "Setting Team 4's Exit Point" );	m_PBGI.Exit4Dest = loc;	break; }
				}
				from.SendGump( new PBGMGump( m_PBGI ) );
			}

			protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
			{
				from.SendGump( new PBGMGump( m_PBGI ) );
			}
		} 

		public class PBPrizeTarget : Target
		{
			private PBGameItem m_PBGI;

			public PBPrizeTarget( PBGameItem pbgi ) : base( -1, false, TargetFlags.None )
			{
				m_PBGI = pbgi;
			}

			protected override void OnTarget( Mobile from, object targ )
			{
				/*
				if( targeted is Container )
					foreach( Item item in ((Container)targeted).Items )
					{
						from.SendMessage( string.Format("Adding {0} {1} to the Prize List", item.Amount, item.GetType().Name ) );
						m_PBGI.m_WinnersPrizes.Add( item );
					}
				else
					from.SendMessage( "Please target a bag with the prizes in it" );
				*/
				
				bool done = false;
				if ( !( targ is Item ) )
				{
					from.SendMessage( "You can only add items." );
					return;
				}

				

				Item copy = (Item)targ;


				Type t = copy.GetType();

				//ConstructorInfo[] info = t.GetConstructors();

				ConstructorInfo c = t.GetConstructor( Type.EmptyTypes );

				if ( c != null )
				{
					try
					{
						from.SendMessage( string.Format("Adding {0} {1} to the Prize List", copy.Amount, copy.Name ) );
			
							object o = c.Invoke( null );

							if ( o != null && o is Item )
							{
								Item newItem = (Item)o;
								CopyProperties( newItem, copy );//copy.Dupe( item, copy.Amount );
								copy.OnAfterDuped( newItem );
								newItem.Parent = null;

								m_PBGI.m_WinnersPrizes.Add( newItem );

								newItem.InvalidateProperties();

								
							}
						
						from.SendMessage( "Done" );
						done = true;
					}
					catch
					{
						from.SendMessage( "Error!" );
						return;
					}
				}

				if ( !done )
				{
					from.SendMessage( "Unable to dupe.  Item must have a 0 parameter constructor." );
				}
			
				
				
				from.SendGump( new PBGMGump( m_PBGI ) );
			}
		
		public static void CopyProperties( Item dest, Item src )
		{
			PropertyInfo[] props = src.GetType().GetProperties();

			for ( int i = 0; i < props.Length; i++ )
			{
				try
				{
					if ( props[i].CanRead && props[i].CanWrite )
					{
						//Console.WriteLine( "Setting {0} = {1}", props[i].Name, props[i].GetValue( src, null ) );
						props[i].SetValue( dest, props[i].GetValue( src, null ), null );
					}
				}
				catch
				{
					//Console.WriteLine( "Denied" );
				}
			}
		}

			protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
			{
				from.SendGump( new PBGMGump( m_PBGI ) );
			}
		}
}