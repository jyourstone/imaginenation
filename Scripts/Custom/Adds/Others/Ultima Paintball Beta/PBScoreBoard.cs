/*********************************************************************************/
/*                                                                               */
/*                              Ultima Paintball 						         */
/*                        Created by Aj9251 (Disturbed)                          */         
/*                                                                               */
/*                                 Credits:                                      */
/*                   Original Idea + Some Code - A_Li_N                          */
/*                   Some Ideas + Code - Aj9251 (Disturbed)                      */
/*********************************************************************************/

using Server;
using System;
using System.Collections;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Games.PaintBall
{
	//[Flipable( 0x1E5E, 0x1E5F )]
	public class PBScoreBoard : Item
	{
		public PBGameItem m_PBGI;
		public int Team1, Team2, Team3, Team4;

			public override string DefaultName
		{
			get { return "Paintball Scoreboard"; }
		}
		

		public PBScoreBoard( PBGameItem pbgi, int id ) : base( 0x1e5e )
		{
			Movable = false;
			m_PBGI = pbgi;
			Name = "Paintball Scoreboard";
			if ( id == 1 )
			{
				ItemID = 0x1e5e;
			}
			if ( id == 2 )
			{
				ItemID = 0x1E5F;
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( m_PBGI == null )
			{
				this.Delete();
			    from.SendMessage( "That scoreboard was not connected to a paintball game." );
			}
			else
			{
				from.CloseGump( typeof( PBScoreGump ) );
				from.SendGump( new PBScoreGump( m_PBGI, this ) );
			}
			
		}
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
            if (m_PBGI != null)
            {
                if (m_PBGI.Teams == 2)
                {
                    list.Add(m_PBGI.Team1Name + " VS " + m_PBGI.Team2Name);

                }
                if (m_PBGI.Teams == 3)
                {
                    list.Add(m_PBGI.Team1Name + " VS " + m_PBGI.Team2Name + " VS " + m_PBGI.Team3Name);
                }
                if (m_PBGI.Teams == 4)
                {
                    list.Add(m_PBGI.Team1Name + " VS " + m_PBGI.Team2Name + " VS " + m_PBGI.Team3Name + " VS " + m_PBGI.Team4Name);
                }
            }
		}
			

		public override void OnSingleClick( Mobile from )
		{
		
			
		/*	if (m_PBGI.Teams == 2 )
			{
				LabelTo( from, " Team 1 VS Team 2 " );
			}
			if (m_PBGI.Teams == 3 )
			{
				LabelTo( from, " Team 1 VS Team 2 VS Team 3 " );
			}
			if (m_PBGI.Teams == 4 )
			{
				LabelTo(from, " Team 1 VS Team 2 VS Team 3 VS Team 4 " );
			}*/
		}

		public PBScoreBoard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			if( m_PBGI != null )
				writer.Write( m_PBGI );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			switch ( version )
			{
				case 0:
				{
					m_PBGI = reader.ReadItem() as PBGameItem;
					break;
				}
			}
		}
	}
	
}
