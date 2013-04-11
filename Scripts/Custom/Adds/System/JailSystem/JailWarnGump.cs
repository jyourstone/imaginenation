using System.Collections;
using Server.Accounting;
using Server.Jailing;
using Server.Network;

namespace Server.Gumps
{
	public class JailWarnGump : Gump
	{
		private readonly Mobile badBoy;
		private Mobile jailor;
		private readonly string m_reason = "Breaking Shard Rules";
		private readonly ArrayList m_warn;
		private int m_id;

		public JailWarnGump( Mobile from, Mobile m, string why, int id, ArrayList warnings ) : base( 100, 40 )
		{
			from.CloseGump( typeof( JailWarnGump ) );
			if( ( why == null ) || ( why == "" ) )
				why = JailGump.reasons[0];
			jailor = from;
			badBoy = m;
			m_reason = why;
			m_id = id;
			Closable = true;
			Dragable = true;
			AddPage( 0 );
			AddBackground( 0, 0, 326, 320, 5054 );
			AddImageTiled( 9, 6, 308, 140, 2624 );
			AddAlphaRegion( 9, 6, 308, 140 );
			AddLabel( 16, 98, 200, "Reason" );
			AddBackground( 14, 114, 290, 24, 0x2486 );
			AddTextEntry( 18, 116, 282, 20, 200, 0, m_reason );
			AddButton( 14, 11, 1209, 1210, 3, GumpButtonType.Reply, 0 );
			AddLabel( 30, 7, 200, JailGump.reasons[0] );
			AddButton( 14, 29, 1209, 1210, 4, GumpButtonType.Reply, 0 );
			AddLabel( 30, 25, 200, JailGump.reasons[1] );
			AddButton( 14, 47, 1209, 1210, 5, GumpButtonType.Reply, 0 );
			AddLabel( 30, 43, 200, JailGump.reasons[2] );
			AddButton( 150, 11, 1209, 1210, 6, GumpButtonType.Reply, 0 );
			AddLabel( 170, 7, 200, JailGump.reasons[3] );
			AddButton( 150, 29, 1209, 1210, 7, GumpButtonType.Reply, 0 );
			AddLabel( 170, 24, 200, JailGump.reasons[4] );
			AddButton( 150, 47, 1209, 1210, 8, GumpButtonType.Reply, 0 );
			AddLabel( 170, 43, 200, JailGump.reasons[5] );
			AddButton( 14, 66, 1209, 1210, 9, GumpButtonType.Reply, 0 );
			AddLabel( 30, 62, 200, JailGump.reasons[6] );
			AddButton( 14, 84, 1209, 1210, 10, GumpButtonType.Reply, 0 );
			AddLabel( 30, 80, 200, JailGump.reasons[7] );
			//warn button
			AddButton( 218, 152, 2472, 2473, 1, GumpButtonType.Reply, 0 );
			AddLabel( 248, 155, 200, "Warn them" );
			//Jail button
			AddButton( 20, 152, 2472, 2473, 2, GumpButtonType.Reply, 0 );
			AddLabel( 50, 155, 200, "Jail them" );
			//previous button
			AddButton( 10, 300, 2466, 2467, 20, GumpButtonType.Reply, 0 );
			//next Button
			AddButton( 90, 300, 2469, 2470, 21, GumpButtonType.Reply, 0 );
			if( warnings == null )
			{
				m_warn = new ArrayList();
				foreach( AccountComment note in ( (Account)m.Account ).Comments )
					if( ( note.AddedBy == JailSystem.JSName + "-warning" ) || ( note.AddedBy == JailSystem.JSName + "-jailed" ) )
						m_warn.Add( note );
				m_id = m_warn.Count - 1;
			}
			else
				m_warn = warnings;
			AddImageTiled( 9, 186, 308, 110, 2624 );
			AddAlphaRegion( 9, 186, 308, 110 );
			string temp = "No prior warnings.";
			if( m_warn.Count > 0 )
			{
				if( m_id < 0 )
					m_id = m_warn.Count - 1;
				if( m_id >= m_warn.Count )
					m_id = 0;
				temp = ( (AccountComment)m_warn[m_id] ).Content;
				AddLabel( 12, 190, 200, "Issued: " + ( (AccountComment)m_warn[m_id] ).LastModified );
			}
			else
				//no prior warning	
				m_id = -1;
			AddLabel( 12, 210, 200, "Event " + ( m_id + 1 ) + " of " + m_warn.Count + " warnings/Jailings" );
			//AddLabel( 12, 230, 200, temp );
			AddHtml( 12, 230, 300, 62, temp, true, true );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			switch( info.ButtonID )
			{
				case 20:
					//previous button
					m_id--;
					if( m_id < 0 )
						m_id = m_warn.Count - 1;
					from.SendGump( new JailWarnGump( from, badBoy, info.GetTextEntry( 0 ).Text, m_id, m_warn ) );
					break;
				case 21:
					//next button
					m_id++;
					if( m_id >= m_warn.Count )
						m_id = 0;
					from.SendGump( new JailWarnGump( from, badBoy, info.GetTextEntry( 0 ).Text, m_id, m_warn ) );
					break;
					//reason buttons
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
					from.SendGump( new JailWarnGump( from, badBoy, JailGump.reasons[info.ButtonID - 3], m_id, m_warn ) );
					break;
				case 1:
					//warn them
					from.CloseGump( typeof( JailWarnGump ) );
					if( m_reason == JailGump.reasons[0] )
						//they are macroing
						JailSystem.macroTest( from, badBoy );
					else
						//not Unattended macroing
						badBoy.SendGump( new JailWarningGump( from, badBoy, m_reason ) );
					break;
				case 2:
					//jail them
					from.CloseGump( typeof( JailWarnGump ) );
					from.SendGump( new JailGump( JailSystem.lockup( badBoy ), from, badBoy, 0, "", m_reason, "0", "0", "1", "0", "0", true ) );
					break;
				default:
					break;
			}
		}
	}
}