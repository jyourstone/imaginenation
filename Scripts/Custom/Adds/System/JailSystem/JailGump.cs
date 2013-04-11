using System;
using Server.Jailing;
using Server.Network;

namespace Server.Gumps
{
	public class JailGump : Gump
	{
		public static string[] reasons = new string[]
			{
				"Unattended Macroing",
				"Disruptive behavior",
				"Arguing with Staff",
				"Harassing other players",
				"Exploiting Bugs",
				"Scamming",
				"Breaking out of Character", 
				"Exposing a Staff Members Player Account"
			};

		private Mobile badBoy;
		private Mobile jailor;
		private int m_page;
		private bool m_return;
		private JailSystem js;

		private string m_reason = "Breaking Shard Rules";

		public JailGump( JailSystem tjs, Mobile owner, Mobile prisoner, int page, string error, string reason ) : base( 100, 40 )
		{
			buildIt( tjs, owner, prisoner, page, error, reason, "0", "0", "1", "0", "0", true );
		}

		public JailGump( JailSystem tjs, Mobile owner, Mobile prisoner, int page, string error, string reason, string month, string week, string day, string hour, string minute, bool fullreturn ) : base( 100, 40 )
		{
			buildIt( tjs, owner, prisoner, page, error, reason, month, week, day, hour, minute, fullreturn );
		}

		public void buildIt( JailSystem tjs, Mobile owner, Mobile prisoner, int page, string error, string reason, string month, string week, string day, string hour, string minute, bool fullreturn )
		{
			js = tjs;
			m_return = fullreturn;
			m_page = page;
			if( ( reason != "" ) && ( reason != null ) )
				m_reason = reason;
			else
				m_reason = reasons[1];
			jailor = owner;
			badBoy = prisoner;
			m_reason = reason;
			jailor.CloseGump( typeof( JailGump ) );
			Closable = false;
			Dragable = false;
			AddPage( 0 );
			AddBackground( 0, 0, 326, 295, 5054 );
			AddImageTiled( 9, 6, 308, 140, 2624 );
			AddAlphaRegion( 9, 6, 308, 140 );
			AddLabel( 16, 98, 200, "Reason" );
			AddBackground( 14, 114, 290, 24, 0x2486 );
			AddTextEntry( 18, 116, 282, 20, 200, 0, m_reason );
			AddButton( 14, 11, 1209, 1210, 3, GumpButtonType.Reply, 0 );
			AddLabel( 30, 7, 200, reasons[0] );
			AddButton( 14, 29, 1209, 1210, 4, GumpButtonType.Reply, 0 );
			AddLabel( 30, 25, 200, reasons[1] );
			AddButton( 14, 47, 1209, 1210, 5, GumpButtonType.Reply, 0 );
			AddLabel( 30, 43, 200, reasons[2] );
			AddButton( 150, 11, 1209, 1210, 6, GumpButtonType.Reply, 0 );
			AddLabel( 170, 7, 200, reasons[3] );
			AddButton( 150, 29, 1209, 1210, 7, GumpButtonType.Reply, 0 );
			AddLabel( 170, 24, 200, reasons[4] );
			AddButton( 150, 47, 1209, 1210, 8, GumpButtonType.Reply, 0 );
			AddLabel( 170, 43, 200, reasons[5] );
			AddButton( 14, 66, 1209, 1210, 9, GumpButtonType.Reply, 0 );
			AddLabel( 30, 62, 200, reasons[6] );
			AddButton( 14, 84, 1209, 1210, 10, GumpButtonType.Reply, 0 );
			AddLabel( 30, 80, 200, reasons[7] );
			//ok button
			AddButton( 258, 268, 2128, 2130, 1, GumpButtonType.Reply, 0 );
			AddImageTiled( 8, 153, 308, 113, 2624 );
			AddAlphaRegion( 8, 153, 308, 113 );
			if( m_return )
				AddButton( 15, 210, 2153, 2151, 2, GumpButtonType.Reply, 0 );
			else
				AddButton( 15, 210, 2151, 2153, 2, GumpButtonType.Reply, 0 );
			AddLabel( 50, 212, 200, "Return to where jailed from on release" );
			if( ( error != "" ) && ( error != null ) )
				AddLabel( 10, 235, 200, error );
			if( m_page == 0 )
			{
				//auto
				//auto/manual
				AddButton( 11, 268, 2111, 2114, 25, GumpButtonType.Reply, 0 );
				AddLabel( 16, 160, 200, "Months" );
				AddBackground( 19, 178, 34, 24, 0x2486 );
				AddTextEntry( 21, 180, 30, 20, 0, 7, month );
				AddLabel( 62, 160, 200, "Weeks" );
				AddBackground( 63, 178, 34, 24, 0x2486 );
				AddTextEntry( 65, 180, 30, 20, 0, 6, week );
				AddLabel( 106, 160, 200, "Days" );
				AddBackground( 104, 178, 34, 24, 0x2486 );
				AddTextEntry( 107, 180, 30, 20, 0, 5, day );
				AddLabel( 145, 160, 200, "Hours" );
				AddBackground( 145, 178, 34, 24, 0x2486 );
				AddTextEntry( 147, 180, 30, 20, 0, 9, hour );
				AddLabel( 185, 160, 200, "Minutes" );
				AddBackground( 191, 178, 34, 24, 0x2486 );
				AddTextEntry( 194, 180, 30, 20, 0, 8, minute );
			}
			else
			{
				AddButton( 11, 268, 2114, 2111, 27, GumpButtonType.Reply, 0 );
				AddLabel( 14, 160, 200, "Account will be Jailed for one year" );
				AddLabel( 14, 178, 200, "or until released, which comes first" );
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			switch( info.ButtonID )
			{
					//reason buttons
				case 25:
					from.SendGump( ( new JailGump( js, from, badBoy, 1, "", info.TextEntries[0].Text, "0", "0", "1", "0", "0", m_return ) ) );
					//, info.GetTextEntry(7), info.GetTextEntry(6), info.GetTextEntry(5), info.GetTextEntry(9), info.GetTextEntry(8),m_return
					break;
				case 27:
					from.SendGump( ( new JailGump( js, from, badBoy, 0, "", info.TextEntries[0].Text, "0", "0", "1", "0", "0", m_return ) ) );
					break;
				case 2:
					m_return = !m_return;
					if( m_page == 1 )
						from.SendGump( ( new JailGump( js, from, badBoy, m_page, "", info.TextEntries[0].Text, "0", "0", "1", "0", "0", m_return ) ) );
					else
						from.SendGump( ( new JailGump( js, from, badBoy, m_page, "", info.TextEntries[0].Text, info.GetTextEntry( 7 ).Text, info.GetTextEntry( 6 ).Text, info.GetTextEntry( 5 ).Text, info.GetTextEntry( 9 ).Text, info.GetTextEntry( 8 ).Text, m_return ) ) );
					break;
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
					if( m_page == 1 )
						from.SendGump( ( new JailGump( js, from, badBoy, m_page, "", reasons[info.ButtonID - 3], "0", "0", "1", "0", "0", m_return ) ) );
					else
						from.SendGump( ( new JailGump( js, from, badBoy, m_page, "", reasons[info.ButtonID - 3], info.GetTextEntry( 7 ).Text, info.GetTextEntry( 6 ).Text, info.GetTextEntry( 5 ).Text, info.GetTextEntry( 9 ).Text, info.GetTextEntry( 8 ).Text, m_return ) ) );
					break;
				case 1:
				{
					DateTime dt_unJail = DateTime.Now;
					string m_Error = "";
					int i_days = 0;
					int i_weeks = 0;
					int i_months = 0;
					int i_minutes = 0;
					int i_hours = 0;
					if( m_page == 0 )
					{
						try
						{
							i_days = Convert.ToInt32( ( info.GetTextEntry( 5 ) ).Text.Trim() );
						}
						catch
						{
							m_Error = "Bad day(s) entry! No negative values or chars.";
						}
						try
						{
							i_weeks = Convert.ToInt32( ( info.GetTextEntry( 6 ) ).Text.Trim() );
						}
						catch
						{
							if( m_Error == "" )
								m_Error = "Bad week(s) entry! No negative values or chars.";
						}
						try
						{
							i_months = Convert.ToInt32( ( info.GetTextEntry( 7 ) ).Text.Trim() );
						}
						catch
						{
							if( m_Error == "" )
								m_Error = "Bad month(s) entry! No negative values or chars.";
						}
						try
						{
							i_minutes = Convert.ToInt32( ( info.GetTextEntry( 8 ) ).Text.Trim() );
						}
						catch
						{
							if( m_Error == "" )
								m_Error = "Bad minute(s) entry! No negative values or chars.";
						}
						try
						{
							i_hours = Convert.ToInt32( ( info.GetTextEntry( 9 ) ).Text.Trim() );
						}
						catch
						{
							if( m_Error == "" )
								m_Error = "Bad hour(s) entry! No negative values or chars.";
						}
						if( ( ( i_days > 7 ) || ( i_days < 0 ) ) && ( m_Error == "" ) )
							if( m_Error == "" )
								m_Error = "Bad day(s) entry! No negative values. 7 days max.";
						if( ( ( i_weeks > 4 ) || ( i_weeks < 0 ) ) && ( m_Error == "" ) )
							if( m_Error == "" )
								m_Error = "Bad week(s) entry! No negative values. 4 weeks max.";
						if( ( ( i_months > 12 ) || ( i_months < 0 ) ) && ( m_Error == "" ) )
							if( m_Error == "" )
								m_Error = "Bad month(s) entry! No negative values. 1 year max.";
						if( ( ( i_minutes > 60 ) || ( i_minutes < 0 ) ) && ( m_Error == "" ) )
							if( m_Error == "" )
								m_Error = "Bad minute(s) entry! No negative values. 1 hour max.";
						if( ( ( i_hours > 24 ) || ( i_hours < 0 ) ) && ( m_Error == "" ) )
							if( m_Error == "" )
								m_Error = "Bad hour(s) entry! No negative values. 1 day max.";
						if( m_Error != "" )
						{
							from.SendGump( new JailGump( js, from, badBoy, m_page, m_Error, info.TextEntries[0].Text, i_months.ToString(), i_weeks.ToString(), i_days.ToString(), i_hours.ToString(), i_minutes.ToString(), m_return ) );
							break;
						}
						if( i_days > 0 )
							dt_unJail = dt_unJail.AddDays( i_days );
						if( i_weeks > 0 )
							dt_unJail = dt_unJail.AddDays( ( i_weeks * 7 ) );
						if( i_months > 0 )
							dt_unJail = dt_unJail.AddMonths( i_months );
						if( i_minutes > 0 )
							dt_unJail = dt_unJail.AddMinutes( i_minutes );
						if( i_hours > 0 )
							dt_unJail = dt_unJail.AddHours( i_hours );
						if( dt_unJail.Ticks <= DateTime.Now.Ticks )
						{
							m_Error = "Calculated date is in the past. Adjust your entries.";
							from.SendGump( new JailGump( js, from, badBoy, m_page, m_Error, info.TextEntries[0].Text, i_months.ToString(), i_weeks.ToString(), i_days.ToString(), i_hours.ToString(), i_minutes.ToString(), m_return ) );
							break;
						}
					}
					else
					{
						//page isn’t the time span
						dt_unJail = dt_unJail.AddYears( 1 );
						if( dt_unJail.Ticks <= DateTime.Now.Ticks )
						{
							m_Error = "Calculated date is in the past. Adjust your entries.";
							from.SendGump( new JailGump( js, from, badBoy, m_page, m_Error, info.TextEntries[0].Text, "12", "0", "0", "0", "0", m_return ) );
							break;
						}
					}
					js.fillJailReport( badBoy, dt_unJail, info.TextEntries[0].Text, m_return, from.Name );
				}
					from.CloseGump( typeof( JailGump ) );
					from.SendGump( new JailReviewGump( from, badBoy, 0, null ) );
					break;
				default:
					//they hit an unknown button
					if( m_page == 1 )
						from.SendGump( ( new JailGump( js, from, badBoy, m_page, "", info.TextEntries[0].Text, "0", "0", "1", "0", "0", m_return ) ) );
					else
						from.SendGump( ( new JailGump( js, from, badBoy, m_page, "", info.TextEntries[0].Text, info.GetTextEntry( 7 ).Text, info.GetTextEntry( 6 ).Text, info.GetTextEntry( 5 ).Text, info.GetTextEntry( 9 ).Text, info.GetTextEntry( 8 ).Text, m_return ) ) );
					//close the Gump, we're done
					break;
			}
		}
	}
}