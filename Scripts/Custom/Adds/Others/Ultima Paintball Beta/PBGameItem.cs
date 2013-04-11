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
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Games.PaintBall;

namespace Server.Games.PaintBall
{
	public class PBGameItem : Item
	{
		public bool			m_Active;
		public bool			m_CanJoin;
		public bool			m_AddNpc;
		public int			m_Team1, m_Team2, m_Team3, m_Team4;
	
		public int          m_TempTeam;
		
		public int          m_Team1Hue, m_Team2Hue, m_Team3Hue, m_Team4Hue;
		public string       m_Team1Name, m_Team2Name, m_Team3Name, m_Team4Name;
		public ArrayList    m_Team1List, m_Team2List, m_Team3List, m_Team4List;
		
		public ArrayList	m_WinnersPrizes;

		private int			m_Teams = 2;
		private int			m_CurrentTeam = 0;
		private Point3D		m_Team1Dest;
		private Point3D		m_Team2Dest;
		private Point3D		m_Team3Dest;
		private Point3D		m_Team4Dest;

		private Point3D		m_Exit1Dest;
		private Point3D		m_Exit2Dest;
		private Point3D		m_Exit3Dest;
		private Point3D		m_Exit4Dest;

		private Map				m_MapDest;
		private ArrayList       m_Announcers;
		private ArrayList       m_Npcs;
		private PBScoreBoard	m_PBScoreBoard;
	//	private PBTimer			m_Timer;
		
		public int m_NadeDamage;
		public int m_NumNades;

        public enum Mod
        {
            AJMod = 0,
            Classic = 1
        }
        public Mod m_Mod;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Active
		{
			get { return m_Active; }
			set { m_Active = value; 
			InvalidateProperties();
			}
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanJoin		
		{
			get { return m_CanJoin; }
			set { m_CanJoin = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public bool AddNpc		
		{
			get { return m_AddNpc; }
			set { m_AddNpc = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public int Team1	
		{
			get { return m_Team1; }
			set { m_Team1 = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public int Team2	
		{
			get { return m_Team2; }
			set { m_Team2 = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public int Team3	
		{
			get { return m_Team3; }
			set { m_Team3 = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public int Team4
		{
			get { return m_Team4; }
			set { m_Team4 = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public string Team1Name		
		{
			get { return m_Team1Name; }
			set { m_Team1Name = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public string Team2Name		
		{
			get { return m_Team2Name; }
			set { m_Team2Name = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public string Team3Name		
		{
			get { return m_Team3Name; }
			set { m_Team3Name = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public string Team4Name		
		{
			get { return m_Team4Name; }
			set { m_Team4Name = value; }
		}
		

		[CommandProperty( AccessLevel.GameMaster )]
		public int Teams
		{
			get { return m_Teams; }
			set { m_Teams = value; }
		
		}

	    [CommandProperty( AccessLevel.GameMaster )]
		public int Team1Hue
		{
			get { return m_Team1Hue; }
			set { m_Team1Hue = value; }
		
		}
				[CommandProperty( AccessLevel.GameMaster )]
		public int Team2Hue
		{
			get { return m_Team2Hue; }
			set { m_Team2Hue = value; }
		
		}
				[CommandProperty( AccessLevel.GameMaster )]
		public int Team3Hue
		{
			get { return m_Team3Hue; }
			set { m_Team3Hue = value; }
		
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public int Team4Hue
		{
			get { return m_Team4Hue; }
			set { m_Team4Hue = value; }
		
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public int NadeDamage
		{
			get { return m_NadeDamage; }
			set { m_NadeDamage = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int NumberOfNades
		{
			get { return m_NumNades; }
			set { m_NumNades = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Team1Dest
		{
			get { return m_Team1Dest; }
			set { m_Team1Dest = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Team2Dest
		{
			get { return m_Team2Dest; }
			set { m_Team2Dest = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Team3Dest
		{
			get { return m_Team3Dest; }
			set { m_Team3Dest = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Team4Dest
		{
			get { return m_Team4Dest; }
			set { m_Team4Dest = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Exit1Dest
		{
			get { return m_Exit1Dest; }
			set { m_Exit1Dest = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Exit2Dest
		{
			get { return m_Exit2Dest; }
			set { m_Exit2Dest = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Exit3Dest
		{
			get { return m_Exit3Dest; }
			set { m_Exit3Dest = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Exit4Dest
		{
			get { return m_Exit4Dest; }
			set { m_Exit4Dest = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Map MapDest
		{
			get { return m_MapDest; }
			set { m_MapDest = value; }
		}
        [CommandProperty(AccessLevel.GameMaster)]
        public Mod SetMod
        {
            get
            {
                return m_Mod;
            }
            set
            {
                if (m_Mod != value)
                {
                    m_Mod = value;

              
                }
            }
        }


		public ArrayList Players;
		public ArrayList NpcPlayers;
		public ArrayList DeadPlayers;
		public ArrayList DeadNpcPlayers;
		public ArrayList Announcers{ get{ return m_Announcers; } }
		public ArrayList Npcs{ get{ return m_Npcs; } }
		public PBScoreBoard SB{ get{ return m_PBScoreBoard; } }



		[Constructable]
		public PBGameItem() : base( 0xED4 )
		{
			Movable = false;
			Name = "PaintBall Game";

            Reset();

			m_Team1Name = "Team 1";
			m_Team2Name = "Team 2";
			m_Team3Name = "Team 3"; 
			m_Team4Name = "Team 4";
			m_Team1Dest = m_Team2Dest = m_Team3Dest = m_Team4Dest = m_Exit1Dest = m_Exit2Dest = m_Exit3Dest = m_Exit4Dest = this.Location;
			m_MapDest = this.Map;


			NadeDamage = 4;
			m_NumNades = 5;
			
						
			m_Team1Hue = 3;
			m_Team2Hue = 38;
			m_Team3Hue = 68;
			m_Team4Hue = 53;
			
			m_WinnersPrizes.Clear();
            

			
			m_PBScoreBoard = new PBScoreBoard( this, 1 );
			m_PBScoreBoard.MoveToWorld( this.Location, this.Map );
			

		}

		public override void OnMapChange()
		{
			if( Deleted )
				return;
			m_MapDest = m_PBScoreBoard.Map = Map;
		}
		public void GetAnnouncers()
		{
			foreach( Mobile mob in m_Announcers )
			{
				if( m_Announcers != null )
				{
				mob.MoveToWorld( this.Location, this.MapDest );
				}
				else
				{
				mob.PublicOverheadMessage( MessageType.Regular, 0x22, false, "Announcer Table returns null." );
				}
			}
			
		}
        public void Reset()
        {
            m_CanJoin = false;
            m_Active = false;
            m_AddNpc = true;

            m_Team1 = m_Team2 = m_Team3 = m_Team4 = 0;
            m_CurrentTeam = 0;

            Players = new ArrayList();

            m_Team1List = new ArrayList();
            m_Team2List = new ArrayList();
            m_Team3List = new ArrayList();
            m_Team4List = new ArrayList();

            NpcPlayers = new ArrayList();
            m_WinnersPrizes = new ArrayList();
            DeadPlayers = new ArrayList();
            m_Announcers = new ArrayList();
            m_Npcs = new ArrayList();
        }
		

		public override void OnLocationChange( Point3D oldLoc )
		{
			if( Deleted )
				return;
			if( !Active )
				m_Team1Dest = m_Team2Dest = m_Team3Dest = m_Team4Dest = m_Exit1Dest = m_Exit2Dest = m_Exit3Dest = m_Exit4Dest = m_PBScoreBoard.Location = Location;
		}


		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Active == true )
				list.Add( 1060742 );//active

			if ( m_Active == false )
				list.Add( 1060743 );//inactive
		}

		public override void OnSingleClick( Mobile from )
		{
			if ( m_Active == true )
				LabelTo( from, 1060742 );
			if ( m_Active == false )
				LabelTo( from, 1060743 );
		}

		public override void OnAfterDelete()
		{
			EndGame(true);
			if( m_PBScoreBoard != null )
				m_PBScoreBoard.Delete();
		/*	if( m_Timer != null )
				m_Timer.Stop(); */
			
			if ( Announcers != null )
			{
				
				for ( int i = 0; i < Announcers.Count ; ++i )
				{
					Mobile mob;
					if( Announcers[i] is Mobile )
					{
						mob = Announcers[i] as Mobile;
						Announcers.Remove( mob );
						mob.Delete();
					}
				}
			}

			base.OnAfterDelete();
		}

		public override void OnDoubleClick( Mobile tfrom )
		{
			Mobile from = tfrom;			
			UpdateTeams();
			
			
			if( from.AccessLevel >= AccessLevel.Counselor )
			{
				from.SendGump( new PBGMGump( this ) );
				
			}

			else if( this.m_Active )
				from.SendMessage( "The game has already started." );
			
			else if ( !this.CanJoin )
				from.SendMessage( "The game has not been setup yet." );

			else if( from is Mobile && from.AccessLevel == AccessLevel.Player )
			{
				if( !from.InRange( GetWorldLocation(), 3 ) )
					from.SendLocalizedMessage( 500446 );
				else if( CheckAlreadyPlayer( from ) )
					from.SendMessage( "You are already on a team." );
				else
				{
					//AddPlayer(from);
					from.SendGump( new Paintball.PBSignup( from, this ) );
				}
			}
			else
				from.SendMessage( "You could not access that item for some reason.  Please page a GM." );
		}
		public void AddPlayer ( Mobile from )
		{
			if ( NpcPlayers != null )
			{
				
			}
			else
			{
				NpcPlayers = new ArrayList();
			}
			
			if ( Players != null )
			{
				
			}
			else
			{
				Players = new ArrayList();
			}
			
			if ( from is PBNpc )
			{
				if ( NpcPlayers != null )
				{
				NpcPlayers.Add( from  );
				AddToTeam( from );
	
				}
			}
			else
			{
				if (Players != null )
				{
					Players.Add( from );
					ArrayList ItemsToMove = new ArrayList();
					ArrayList LayerItems = new ArrayList();
					
					LayerPack LayerP = new LayerPack();
				    ItemsPack ItemsP = new ItemsPack();
					ItemsP.Hue = 1;
					LayerP.Hue = 1;
					LayerP.Name = "Layer from PaintBallGame";
					ItemsP.Name = "Items from PaintBallGame";
					BankBox bankBox = from.BankBox;

					foreach( Item item in from.Items )
						if( item.Layer != Layer.Bank && item.Layer != Layer.Hair && item.Layer != Layer.FacialHair && item.Layer != Layer.Mount && item.Layer != Layer.Backpack )
						LayerItems.Add( item );

                    if (from.Backpack != null)
                    {
                        foreach (Item item in from.Backpack.Items)
                            ItemsToMove.Add(item);
                    }

				    foreach( Item item in ItemsToMove )
						ItemsP.AddItem( item );
					
					foreach( Item item in LayerItems )
						LayerP.AddItem( item );

					
					
					bankBox.AddItem( ItemsP );
					bankBox.AddItem( LayerP );
					

					bankBox.AddItem( new PBPlayerStorage( from ) );

					AddToTeam( from );
				}
			}
			
		}

		public bool CheckAlreadyPlayer( Mobile player )
		{
			if (Players != null)
			{
			for( int i = 0; i < Players.Count; i++ )
				if( player == Players[i] as Mobile )
					return true;
			World.Broadcast( 0x22, true, " yes player" );
			}

			
			if (NpcPlayers != null)
			{
			for( int i = 0; i < NpcPlayers.Count; i++ )
				if( player == NpcPlayers[i] as Mobile )
					return true;
			World.Broadcast( 0x22, true, " yes npc" );
			}
	

			return false;	
		
	
		}

		
		private void AddToTeam( Mobile player )
		{
			if ( player != null && m_Team1List != null && m_Team2List != null && m_Team3List != null && m_Team4List != null)
			{
			player.Str = player.Dex = player.Int = 100;
			player.Fame = player.Karma = 0;
			player.Kills = 10;
			
			int TeamId = GetTeamHue();
			
			for( int i = 0; i < PowerScroll.Skills.Count; i++ )
				player.Skills[PowerScroll.Skills[i]].Base = 0;

			player.Skills[SkillName.Archery].Base = 100;
			player.Skills[SkillName.Anatomy].Base = 100;
			player.Skills[SkillName.Tactics].Base = 100;
			player.Skills[SkillName.Meditation].Base = 100;
			player.Skills[SkillName.Focus].Base = 100;

			EquipItems( player, TeamId );
	
			if ( TeamId == Team1Hue )
			{
				m_Team1List.Add(player);
			}
			else if ( TeamId == Team2Hue )
			{
				m_Team2List.Add(player);
			}
			else if ( TeamId == Team3Hue )
			{
				m_Team3List.Add(player);
			}
			else if ( TeamId == Team4Hue )
			{
				m_Team4List.Add(player);
			}
			}
			UpdateTeams();
		}
		public void UpdateTeams()
		{
			if ( m_Team1List != null)
			{
				m_Team1 = m_Team1List.Count;
			}
			if ( m_Team2List != null )
			{
				m_Team2 = m_Team2List.Count;
			}
			if ( m_Team3List != null )
			{
				m_Team3 = m_Team3List.Count;
			}
			if ( m_Team4List != null)
			{
				m_Team4 = m_Team4List.Count;
			}
			
			
		}

		private int GetTeamHue()
		{
			m_CurrentTeam ++;
			switch( m_Teams )
			{
				case 2:
				{
					switch( m_CurrentTeam )
					{
						case 1:	return m_Team1Hue;
						case 2:	return m_Team2Hue;
						default: m_CurrentTeam = 1; return m_Team1Hue;
					}
				}
				case 3:
				{
					switch( m_CurrentTeam )
					{
						case 1:	return m_Team1Hue;
						case 2:	return m_Team2Hue;
						case 3:	return m_Team3Hue;
						default:	m_CurrentTeam = 1; return m_Team1Hue;
					}
				}
				case 4:
				{
					switch( m_CurrentTeam )
					{
						case 1:	return m_Team1Hue;
						case 2:	return m_Team2Hue;
						case 3:	return m_Team3Hue;
						case 4:	return m_Team4Hue;
						default:	m_CurrentTeam = 1; return m_Team1Hue;
					}
				}
			}

			return 0;
		}

		private void EquipItems( Mobile Player, int TeamHue )
		{
			Player.SendMessage( "Equiping Team Items" );
			Player.AddItem( new PBArmor( TeamHue, 5136, Layer.Arms ) );
			Player.AddItem( new PBArmor( TeamHue, 5137, Layer.Pants ) );
			Player.AddItem( new PBArmor( TeamHue, 5138, Layer.Helm ) );
			Player.AddItem( new PBArmor( TeamHue, 5139, Layer.Neck ) );
			Player.AddItem( new PBArmor( TeamHue, 5140, Layer.Gloves ) );
			Player.AddItem( new PBArmor( TeamHue, 5141, Layer.InnerTorso ) );
			Player.AddItem( new PBWeapon( TeamHue, this ) );
			Cloak cloak = new Cloak( TeamHue );
			cloak.Movable = false;
			Player.AddItem( cloak );
			Player.AddToBackpack( new PaintBall( TeamHue, 200 ) );

            if (m_Mod == 0)
            {

                for (int i = 0; i < m_NumNades; ++i)
                {
                    Player.AddToBackpack(new PBGrenade(TeamHue, this));
                }
            }
			
			if ( TeamHue == m_Team1Hue)
			{
				Player.Name = Player.Name + " (" + m_Team1Name + ")";
				foreach( Mobile mob in Announcers )
				{
		
				mob.PublicOverheadMessage( MessageType.Regular, 0x22, false, Player.Name + " Has Joined " + m_Team1Name );
				}
				Player.MoveToWorld( Team1Dest, MapDest );	
				m_Team1 += 1;
			}
			else if ( TeamHue == m_Team2Hue )
			{
				Player.Name = Player.Name + " (" + m_Team2Name + ")";
				foreach( Mobile mob in Announcers )
				{
		
				mob.PublicOverheadMessage( MessageType.Regular, 0x22, false, Player.Name + " Has Joined " + m_Team2Name );
				}
				Player.MoveToWorld( Team2Dest, MapDest );	
				m_Team2 += 1;
			}
			else if ( TeamHue == m_Team3Hue )
			{
				Player.Name = Player.Name + " (" + m_Team3Name + ")";
				foreach( Mobile mob in Announcers )
				{
		
				mob.PublicOverheadMessage( MessageType.Regular, 0x22, false, Player.Name + " Has Joined " + m_Team3Name );
				}
				Player.MoveToWorld( Team3Dest, MapDest );	
				m_Team3 += 1;
			}
			else if ( TeamHue == m_Team4Hue )
			{
				Player.Name = Player.Name + " (" + m_Team4Name + ")";
				foreach( Mobile mob in Announcers )
				{
		
				mob.PublicOverheadMessage( MessageType.Regular, 0x22, false, Player.Name + " Has Joined " + m_Team4Name );
				}
				Player.MoveToWorld( Team4Dest, MapDest );	
				m_Team4 += 1;
			}
			
			Player.Frozen = true;
			
		}

		public void KillPlayer( Mobile player )
		{
			player.Warmode = false;
			if( player.FindItemOnLayer( Layer.TwoHanded ) != null )
			{
				player.FindItemOnLayer( Layer.TwoHanded ).Delete();
			}
			RemovePlayer( player );
			CheckDone();


		}
		
		public void RemovePlayer( Mobile m )
		{
			
			if ( m_Team1List.Contains(m) )
			{
				m_Team1List.Remove(m);
				if ( m is PBNpc )
				{
					NpcPlayers.Remove(m);
					m.Delete();
				}
				else
				{
					m.MoveToWorld( Exit1Dest, MapDest );
				}
			}
			else if ( m_Team2List.Contains(m) )
			{
				m_Team2List.Remove(m);
				if ( m is PBNpc )
				{
					NpcPlayers.Remove(m);
					m.Delete();
				}
				else
				{
					m.MoveToWorld( Exit2Dest, MapDest );
				}
			}
			else if ( m_Team3List.Contains(m) )
			{
				m_Team3List.Remove(m);
				if ( m is PBNpc )
				{
					NpcPlayers.Remove(m);
					m.Delete();
				}
				else
				{
					m.MoveToWorld( Exit3Dest, MapDest );
				}			
			}
			else if ( m_Team4List.Contains(m) )
			{
				m_Team4List.Remove(m);
				if ( m is PBNpc )
				{
					NpcPlayers.Remove(m);
					m.Delete();
				}
				else
				{
					m.MoveToWorld( Exit4Dest, MapDest );
				}
			}
			else
			{
				
			}
			UpdateTeams();
		}
		
		public void CheckDone( )
		{
			UpdateTeams();
			if( Active )
			{
				if (m_Team1List.Count != 0 && m_Team2List.Count <=0 && m_Team3List.Count <=0 && m_Team4List.Count <=0 )
				{
					// Team 1 Win
					EndGame(false);
				}
				else if (m_Team1List.Count <= 0 && m_Team2List.Count !=0 && m_Team3List.Count <=0 && m_Team4List.Count <=0 )
				{
					// Team 2 Win
					EndGame(false);
				}
				else if (m_Team1List.Count <= 0 && m_Team2List.Count <=0 && m_Team3List.Count !=0 && m_Team4List.Count <=0 )
				{
					// Team 3 Win
					EndGame(false);
				}
				else if (m_Team1List.Count <= 0 && m_Team2List.Count <=0 && m_Team3List.Count <=0 && m_Team4List.Count !=0 )
				{
					// Team 4 Win
					EndGame(false);
				}
				else
				{
				
				}
				
			}
			else	
			{
				
			}
		}
		
		public int GetTeam(int hue )
		{
			if ( hue == m_Team1Hue )
			{
				return 1;
			}
			else if ( hue == m_Team2Hue )
			{
				return 2;
			}
			else if ( hue == m_Team3Hue )
			{
			    return 3;
			}
			else if ( hue == m_Team4Hue )
			{
				return 4;
			}
			return 0;
		}
		
		public void StartGame()
		{
				Active = true;
				
				DeadPlayers = new ArrayList();
				InvalidateProperties();
				
if ( Players != null )
{
				foreach( Mobile pm in Players )
				{
					pm.Frozen = false;
					pm.Warmode = true;

					pm.SendMessage( "The game has started!  GO GO GOOOOOO!" );
			
				}
}
if (NpcPlayers != null)
{
				foreach(Mobile npc in NpcPlayers )
				{
					npc.Frozen = false;
					npc.Warmode = true;
				}
}
if ( Announcers != null )
{
				foreach( Mobile mob in Announcers )
				{
		
				mob.PublicOverheadMessage( MessageType.Regular, 0x22, false, "The Game Has Started!" );
				}
}
		}
		
		public void EstablishArrays()
		{
			if ( Npcs != null )
			{
				
			}
			else
			{
				m_Npcs = new ArrayList();
			}
		}

		public void EndGame(bool gmtrig)
		{
			
			
			this.Active = false;
			InvalidateProperties();

            if (gmtrig == false)
            {
                if (m_Team1 > 0 && m_Team2 == 0 && m_Team3 == 0 && m_Team4 == 0)
                {
                    World.Broadcast(0x22, true, m_Team1Name + " has won in Paintball!");
                    foreach (Mobile mob in Announcers)
                    {

                        mob.PublicOverheadMessage(MessageType.Regular, 0x22, false, m_Team1Name + " has won");
                    }
                }
                else if (m_Team2 > 0 && m_Team1 == 0 && m_Team3 == 0 && m_Team4 == 0)
                {
                    World.Broadcast(0x22, true, "Team 2 has won in Paintball!");
                    foreach (Mobile mob in Announcers)
                    {

                        mob.PublicOverheadMessage(MessageType.Regular, 0x22, false, m_Team2Name + " has won");
                    }
                }
                else if (m_Team3 > 0 && m_Team1 == 0 && m_Team2 == 0 && m_Team4 == 0)
                {
                    World.Broadcast(0x22, true, "Team 3 has won in Paintball!");
                    foreach (Mobile mob in Announcers)
                    {

                        mob.PublicOverheadMessage(MessageType.Regular, 0x22, false, m_Team3Name + " has won");
                    }
                }
                else if (m_Team4 > 0 && m_Team1 == 0 && m_Team2 == 0 && m_Team3 == 0)
                {
                    World.Broadcast(0x22, true, "Team 4 has won in Paintball!");
                    foreach (Mobile mob in Announcers)
                    {

                        mob.PublicOverheadMessage(MessageType.Regular, 0x22, false, m_Team4Name + " has won");
                    }
                }
                else
                {
                    World.Broadcast(0x22, true, "Error:");
                    World.Broadcast(0x22, true, m_Team1.ToString() + " = team 1");
                    World.Broadcast(0x22, true, m_Team2.ToString() + " = team 2");
                    World.Broadcast(0x22, true, m_Team3.ToString() + " = team 3");
                    World.Broadcast(0x22, true, m_Team4.ToString() + " = team 4");
                }

            }
            else if (gmtrig == true)
            {
                World.Broadcast(0x22, true, "The Paintball game was ended by the GM. No winner was declared.");
            }

			ArrayList MobDel = new ArrayList();

		if ( Players != null)
		{
		foreach ( Mobile pm in Players  )
		{
				

				pm.SendMessage( "The game has ended." );

				pm.Hidden = true;
				pm.Warmode = false;
				pm.Frozen = false;
                if (gmtrig == false)
                {
                    GivePrizes(pm);
                }
			
				Item PBSI;
				
				
				Item LPackI;
				Item IPackI;
					
				
				Container bank = pm.FindBankNoCreate();
				
				PBSI = bank.FindItemByType( typeof( PBPlayerStorage ) );
				LPackI = bank.FindItemByType( typeof( LayerPack ) );
				IPackI = bank.FindItemByType( typeof( ItemsPack ) );
				
				pm.AddToBackpack( PBSI );
			    	pm.AddToBackpack( LPackI );
			    	pm.AddToBackpack( IPackI );

                    List<Item> Layer = LPackI.Items;
                    if (Layer != null && LPackI != null)
                    {
                        if (Layer.Count > 0)
                        {

                           for (int i = Layer.Count - 1; i >= 0; --i)
                            {
                                if (i >= Layer.Count)
                                    continue;

                                pm.AddItem(Layer[i]);
                            }
                        }
                        LPackI.Delete();
                    }

                    List<Item> Pack = IPackI.Items;
                    if (Pack != null && IPackI != null)
                    {
                        if (Pack.Count > 0)
                        {

                            for (int i = Pack.Count - 1; i >= 0; --i)
                            {
                                if (i >= Pack.Count)
                                    continue;

                                pm.AddToBackpack(Pack[i]);
                            }
                        }
                        IPackI.Delete();
                    }
                    
				

				if (PBSI != null )
				{
					PBPlayerStorage PBStorage = PBSI as PBPlayerStorage;
					if ( PBStorage != null )
					{
						PBStorage.Use( pm );
					}
				}
				  

				ArrayList toDelete = new ArrayList();
				
				foreach( Item litem in pm.Items )
				{
					if( litem is PBArmor || litem is PBWeapon || litem is PaintBall || litem is Cloak || litem is Squash || litem is PBGrenade )
					{
						toDelete.Add( litem );
					}
				}

                if (pm.Backpack != null)
                {
                    foreach (Item bitem in pm.Backpack.Items)
                    {
                        if (bitem is PBArmor || bitem is PBWeapon || bitem is PaintBall || bitem is Cloak ||
                            bitem is Squash || bitem is PBGrenade)
                        {

                            toDelete.Add(bitem);
                        }

                    }
                }

		    foreach( Item ditem in toDelete )
				{
					ditem.Delete();
				}
			   
		
		}
		}
		if ( DeadPlayers != null )
		{
		foreach( Mobile dm in DeadPlayers )
		{
				dm.SendMessage( "The game has ended." );

				dm.Hidden = true;
				dm.Warmode = false;
				dm.Frozen = false;

                if (gmtrig == false)
                {
                    GivePrizes(dm);
                }
				
			
				Item PBSI;
				PBPlayerStorage PBS;
				
				Item LPackI;
				Item IPackI;
				
					
				
				Container bank = dm.FindBankNoCreate();
				
				PBSI = bank.FindItemByType( typeof( PBPlayerStorage ) );
				LPackI = bank.FindItemByType( typeof( LayerPack ) );
				IPackI = bank.FindItemByType( typeof( ItemsPack ) );
			   
			   
			    	dm.AddToBackpack( PBSI );
			    	dm.AddToBackpack( LPackI );
			    	dm.AddToBackpack( IPackI );

			    
			    	PBS = (PBPlayerStorage)PBSI;

                    List<Item> Layer = LPackI.Items;
                    if (Layer != null && LPackI != null)
                    {
                        if (Layer.Count > 0)
                        {

                            for (int i = Layer.Count - 1; i >= 0; --i)
                            {
                                if (i >= Layer.Count)
                                    continue;

                                dm.AddItem(Layer[i]);
                            }
                        }
                        LPackI.Delete();
                    }

                    List<Item> Pack = IPackI.Items;
                    if (Pack != null && IPackI != null)
                    {
                        if (Pack.Count > 0)
                        {

                            for (int i = Pack.Count - 1; i >= 0; --i)
                            {
                                if (i >= Pack.Count)
                                    continue;

                                dm.AddToBackpack(Pack[i]);
                            }
                        }
                        IPackI.Delete();
                    }
			
			   

				ArrayList toDelete = new ArrayList();
				
				foreach( Item item in dm.Items )
				{
					if( item is PBArmor || item is PBWeapon || item is PaintBall || item is Cloak || item is Squash || item is PBGrenade )
					{
						toDelete.Add( item );
					}
				}

				if ( dm.Backpack != null )
				{
				foreach( Item bpitem in dm.Backpack.Items )
				{
					if( bpitem is PBArmor || bpitem is PBWeapon || bpitem is PaintBall || bpitem is Cloak || bpitem is Squash || bpitem is PBGrenade )
					{
					
						toDelete.Add( bpitem );
					}
	
					if ( PBS != null )
					{
					PBS.Use(dm);
					}
				}
				}
				if ( toDelete != null )
				{
				foreach( Item item in toDelete )
				{
					item.Delete();
				}
				}
		}
		}
		if (NpcPlayers != null)
		{
		foreach( Mobile npc  in NpcPlayers )
		{
				MobDel.Add( npc);
				
					
		}
		NpcPlayers.Clear();
		NpcPlayers = new ArrayList();
		}
		
		if ( MobDel != null)
		{
		foreach( Mobile Mob in  MobDel )
		{
			if (Mob != null )
			    {
			Mob.Delete();
			    }
		}
		}
		
		if ( Players != null )
		{
			Players.Clear();
			Players = new ArrayList();
		}
			
			
			
		if ( Npcs != null )
		{
			Npcs.Clear();
		}
			
			
		if (m_WinnersPrizes != null)
		{
			m_WinnersPrizes.Clear();
		}
		if ( m_Team1List != null )
		{
			m_Team1List.Clear();
		}
		if ( m_Team2List != null )
		{
			m_Team2List.Clear();
		}
		if ( m_Team3List != null )
		{
			m_Team3List.Clear();
		}
		if ( m_Team4List != null )
		{
			m_Team4List.Clear();
		}
			

				m_Team1 = 0;
				m_Team2 = 0;
				m_Team3 = 0;
				m_Team4 = 0;
				
				UpdateTeams();

                m_CurrentTeam = 0;
				
			Players = new ArrayList();
			m_Npcs = new ArrayList();
			
			m_Team1List = new ArrayList();
			m_Team2List = new ArrayList();
			m_Team3List = new ArrayList();
			m_Team4List = new ArrayList();
			
			NpcPlayers = new ArrayList();
			m_WinnersPrizes = new ArrayList();
			DeadPlayers = new ArrayList();
            Reset();

			/* if( m_Timer != null )
				m_Timer.Stop(); */
		} 


		public void GivePrizes( Mobile pm )
		{
			bool Won = false;
			int team;
			if( pm.FindItemOnLayer( Layer.Cloak ) != null )
			{
				team = pm.FindItemOnLayer( Layer.Cloak ).Hue;
			}
			else
			{
				pm.SendMessage( "You have no team cloak.  Please ask the GM about this!" );
				return;
			}
			
			if( ( m_Team1 > 0 && m_Team2 == 0 && m_Team3 == 0 && m_Team4 == 0 ) && team == m_Team1Hue )
		{
			Won = true;
			pm.MoveToWorld( Exit1Dest, MapDest );
		}
			if(  (m_Team2 > 0 && m_Team1 == 0 && m_Team3 == 0 && m_Team4 == 0)  && team == m_Team2Hue )
		{
			Won = true;
			pm.MoveToWorld( Exit2Dest, MapDest );
		}
		
			if(  ( m_Team3 > 0 && m_Team2 == 0 && m_Team1 == 0 && m_Team4 == 0) && team == m_Team3Hue )
		{
			Won = true;
			pm.MoveToWorld( Exit3Dest, MapDest );
		}
			if( ( m_Team4 > 0 && m_Team2 == 0 && m_Team3 == 0 && m_Team1 == 0) && team == m_Team4Hue )
		{
			Won = true;	
			pm.MoveToWorld( Exit4Dest, MapDest );
		}
				
			

			if( Won )
			{
				pm.SendMessage( "Your team won!  Here is your prize." );
				if( m_WinnersPrizes == null || m_WinnersPrizes.Count == 0 )
				{
					pm.SendMessage( "The GM did not set an automatic prize.  Please ask them if they are planning on giving one out!" );
					return;
				}
				Bag winnersBag = new Bag();
				Item toGive;
				for( int i = 0; i < m_WinnersPrizes.Count; i++ )
				{
					if( m_WinnersPrizes[i] is BankCheck )
					{
						int bcWorth = ((BankCheck)m_WinnersPrizes[i]).Worth;
						toGive = new BankCheck( bcWorth );
					}
					else
					{
						toGive = (Item)Activator.CreateInstance( m_WinnersPrizes[i].GetType() );
						toGive.Amount = ((Item)m_WinnersPrizes[i]).Amount;
					}
					winnersBag.AddItem( toGive );
				}
				pm.AddToBackpack( winnersBag );
			}
			else
				pm.SendMessage( "Your team did not win this game.  Try again next time!" );
		}
		public PBGameItem( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );// version
			
			writer.Write( (int)m_Teams );

			writer.WriteMobileList( Players );
            writer.WriteMobileList(NpcPlayers);
			writer.WriteMobileList( m_Announcers );
			
			writer.WriteItemList( m_WinnersPrizes );

			writer.Write( m_Team1Dest );
			writer.Write( m_Team2Dest );
			writer.Write( m_Team3Dest );
			writer.Write( m_Team4Dest );
			writer.Write( m_Exit1Dest );
			writer.Write( m_Exit2Dest );
			writer.Write( m_Exit3Dest );
			writer.Write( m_Exit4Dest );

			
			writer.Write( (Map)m_MapDest );
            writer.Write( (int)m_Mod);
			

			
			writer.Write( (bool)m_Active );
			writer.Write( (int)m_Team1 );
			writer.Write( (int)m_Team2 );
			writer.Write( (int)m_Team3 );
			writer.Write( (int)m_Team4 );
			
			writer.Write( (int)m_Team1Hue );
			writer.Write( (int)m_Team2Hue);
			writer.Write( (int)m_Team3Hue );
			writer.Write( (int)m_Team4Hue );
			
			writer.Write( (string)m_Team1Name );
			writer.Write( (string)m_Team2Name );
			writer.Write( (string)m_Team3Name );
			writer.Write( (string)m_Team4Name );

			writer.Write( (int)m_NadeDamage );
			writer.Write( (int)m_NumNades );

			writer.Write( m_PBScoreBoard );
				
			


		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
				
					m_Teams = reader.ReadInt();
					
					Players = reader.ReadMobileList();
                    NpcPlayers = reader.ReadMobileList();
					m_Announcers = reader.ReadMobileList();
					
					m_WinnersPrizes = reader.ReadItemList();
					
					
					m_Team1Dest = reader.ReadPoint3D();
					m_Team2Dest = reader.ReadPoint3D();
					m_Team3Dest = reader.ReadPoint3D();
					m_Team4Dest = reader.ReadPoint3D();
					m_Exit1Dest = reader.ReadPoint3D();
					m_Exit2Dest = reader.ReadPoint3D();
					m_Exit3Dest = reader.ReadPoint3D();
					m_Exit4Dest = reader.ReadPoint3D();

					m_MapDest = reader.ReadMap();
                    m_Mod = (Mod)reader.ReadInt();

					m_Active = reader.ReadBool();
					m_Team1 = reader.ReadInt();
					m_Team2 = reader.ReadInt();
					m_Team3 = reader.ReadInt();
					m_Team4 = reader.ReadInt();
					
					m_Team1Hue = reader.ReadInt();
					m_Team2Hue = reader.ReadInt();
					m_Team3Hue = reader.ReadInt();
					m_Team4Hue = reader.ReadInt();
					
					m_Team1Name = reader.ReadString();
					m_Team2Name = reader.ReadString();
					m_Team3Name = reader.ReadString();
					m_Team4Name = reader.ReadString();
					
					m_NadeDamage = reader.ReadInt();
					m_NumNades = reader.ReadInt();
					

					m_PBScoreBoard = reader.ReadItem() as PBScoreBoard;
		

					m_Active = false;
                    Reset();
					//m_Timer = new PBTimer( this );
					
					
					
				
			
		
		}
	}
}
