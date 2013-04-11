using System;
using Server.Accounting;
using Server.Jailing;
using Server.Network;

namespace Server.Gumps
{
	public class JailAdminGump : Gump
	{
		public enum AdminJailGumpPage
		{
			General,
			OOC,
			Language,
			Review
		}

		private const int LabelColor = 0x7FFF;
		private const int SelectedColor = 0x421F;
		private const int DisabledColor = 0x4210;

		private const int LabelColor32 = 0xFFFFFF;
		private const int SelectedColor32 = 0x8080FF;
		private const int DisabledColor32 = 0x808080;

		private const int TitleX = 210;
		private const int TitleY = 7;
		private const int BodyX = 5;
		private const int BodyY = 111;
		private const int MessageX = 5;
		private const int MessageY = 387;
		private const int gutterOffset = 3;
		private const int LineStep = 25;

		private AdminJailGumpPage m_page = AdminJailGumpPage.General;
		private int m_subpage = 0;
		private int m_id = 0;
		private JailSystem js = null;

		public JailAdminGump() : base( 10, 30 )
		{
			buildit( AdminJailGumpPage.Review, 0, 0 );
		}

		public JailAdminGump( AdminJailGumpPage page ) : base( 10, 30 )
		{
			buildit( page, 0, 0 );
		}

		public JailAdminGump( AdminJailGumpPage page, int subpage, int id ) : base( 10, 30 )
		{
			buildit( page, subpage, id );
		}

		public void AddTextField( int x, int y, int width, int height, int index )
		{
			AddTextField( x, y, width, height, index, "" );
		}

		public void AddTextField( int x, int y, int width, int height, int index, string content )
		{
			AddBackground( x - 2, y - 2, width + 4, height + 4, 0x2486 );
			AddTextEntry( x + 2, y + 2, width - 4, height - 4, 0, index, content );
		}

		public string Color( string text, int color )
		{
			return String.Format( "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text );
		}

		public void AddButtonLabeled( int x, int y, int buttonID, string text )
		{
			AddButton( x, y - 1, 4005, 4007, buttonID, GumpButtonType.Reply, 0 );
			AddHtml( x + 35, y, 240, 20, Color( text, LabelColor32 ), false, false );
		}

		public void AddToggleLabeled( int x, int y, int buttonID, string text, bool selected )
		{
			AddButton( x, y - 1, selected ? 2154 : 2152, selected ? 2152 : 2154, buttonID, GumpButtonType.Reply, 0 );
			AddHtml( x + 35, y, 240, 20, Color( text, LabelColor32 ), false, false );
		}

		public void AddPageLabeled( int x, int y, int buttonID, string text, AdminJailGumpPage page )
		{
			AddButton( x, y - 1, ( m_page == page ) ? 4006 : 4005, 4007, buttonID, GumpButtonType.Reply, 0 );
			AddHtml( x + 35, y, 240, 20, ( m_page == page ) ? Color( text, LabelColor32 ) : Color( text, SelectedColor32 ), false, false );
		}

		public void AddBlackAlpha( int x, int y, int width, int height )
		{
			AddImageTiled( x, y, width, height, 2624 );
			AddAlphaRegion( x, y, width, height );
		}

		public void message( string text )
		{
			AddColorLabel( MessageX + gutterOffset, MessageY + gutterOffset, 390, 40, text );
		}

		public void message()
		{
			message( "Settings changes will be saved during the world save." );
		}

		private static int basex( int x )
		{
			return x + gutterOffset;
		}

		private static int basey( int y )
		{
			return basey( y, 0 );
		}

		private static int basey( int y, int lines )
		{
			return ( y + gutterOffset ) + ( lines * LineStep );
		}

		public void AddColorLabel( int x, int y, string text )
		{
			AddColorLabel( x, y, 240, 20, text );
		}

		public void AddColorLabel( int x, int y, int width, int height, string text )
		{
			AddHtml( x, y, width, height, Color( text, LabelColor32 ), false, false );
		}

		public void AddColorLabelScroll( int x, int y, int width, int height, string text )
		{
			AddHtml( x, y, width, height, Color( text, LabelColor32 ), true, true );
		}

		private void buildit( AdminJailGumpPage page, int subpage, int id )
		{
			Closable = true;
			Dragable = true;
			m_id = id;
			m_page = page;
			m_subpage = subpage;
			AddPage( 0 );

			AddBackground( 0, 0, 412, 439, 5054 );
			AddBlackAlpha( 5, 8, 200, 98 );
			AddBlackAlpha( TitleX, TitleY, 190, 98 );
			AddBlackAlpha( BodyX, BodyY, 396, 271 );
			AddBlackAlpha( MessageX, MessageY, 396, 46 );
			AddPageLabeled( 7, 12, 4, "Review Current Jailings", AdminJailGumpPage.Review );
			AddPageLabeled( 7, 36, 3, "Language Settings", AdminJailGumpPage.Language );
			AddPageLabeled( 7, 59, 2, "OOC Settings", AdminJailGumpPage.OOC );
			AddPageLabeled( 7, 82, 1, "General Settings", AdminJailGumpPage.General );

			AddButton( TitleX + 120, TitleY + 75, 241, 243, 5, GumpButtonType.Reply, 0 );
			switch( m_page )
			{
				case AdminJailGumpPage.Review:
					buildReviews();
					break;
				case AdminJailGumpPage.General:
					buildSettings();
					break;
				case AdminJailGumpPage.Language:
					buildLanguage();
					break;
				case AdminJailGumpPage.OOC:
					buildOOC();
					break;
				default:
					break;
			}
		}

		private void buildLanguage()
		{
			AddToggleLabeled( basex( TitleX ), basey( TitleY, 0 ), 13, "Use Language", JailSystem.useLanguageFilter );
			message();
			if( !JailSystem.useLanguageFilter )
				return;
			AddButton( TitleX + 50, TitleY + 75, 239, 240, 15, GumpButtonType.Reply, 0 );
			AddLabel( basex( BodyX ), basey( BodyY, 0 ), 200, "Misc." );
			AddLabel( basex( BodyX ) + 15, basey( BodyY, 1 ), 200, "Foul Jailor" );
			AddTextField( basex( BodyX ) + 80, basey( BodyY, 1 ), 150, 20, 12, JailSystem.foulJailorName );
			AddToggleLabeled( basex( BodyX ) + 15, basey( BodyY, 2 ), 14, "Allow Staff to use bad words", JailSystem.allowStaffBadWords );

			AddLabel( basex( BodyX ) + 240, basey( BodyY, 0 ), 200, "Bad words" );
			string temp = "";
			foreach( string p in JailSystem.badWords )
				temp += string.Format( "{0}\n", p );
			AddColorLabelScroll( basex( BodyX ) + 240, basey( BodyY, 1 ), 150, 60, temp.Trim() );

			AddTextField( basex( BodyX ) + 240, basey( BodyY, 1 ) + 65, 150, 20, 13 );
			AddButton( BodyX + 240, basey( BodyY, 1 ) + 90, 2461, 2462, 26, GumpButtonType.Reply, 0 );
			AddButton( BodyX + 295, basey( BodyY, 1 ) + 90, 2464, 2465, 27, GumpButtonType.Reply, 0 );

			AddLabel( basex( BodyX ) + 240, basey( BodyY, 5 ) + 10, 200, "Jail Terms" );
			temp = "";
			foreach( TimeSpan t in JailSystem.FoulMouthJailTimes )
				temp += string.Format( "d={0} h={1} m={2}\n", t.Days, t.Hours, t.Minutes );
			AddColorLabelScroll( basex( BodyX ) + 240, basey( BodyY, 6 ) + 10, 150, 60, temp.Trim() );

			AddLabel( basex( BodyX ) + 240, basey( BodyY, 6 ) + 75, 200, "D" );
			AddLabel( basex( BodyX ) + 290, basey( BodyY, 6 ) + 75, 200, "H" );
			AddLabel( basex( BodyX ) + 340, basey( BodyY, 6 ) + 75, 200, "M" );
			AddTextField( basex( BodyX ) + 255, basey( BodyY, 6 ) + 75, 30, 20, 8 );
			AddTextField( basex( BodyX ) + 305, basey( BodyY, 6 ) + 75, 30, 20, 9 );
			AddTextField( basex( BodyX ) + 355, basey( BodyY, 6 ) + 75, 30, 20, 10 );
			AddButton( BodyX + 240, basey( BodyY, 6 ) + 100, 2461, 2462, 28, GumpButtonType.Reply, 0 );
			AddButton( BodyX + 295, basey( BodyY, 6 ) + 100, 2464, 2465, 29, GumpButtonType.Reply, 0 );
		}

		private void buildOOC()
		{
			AddToggleLabeled( basex( TitleX ), basey( TitleY, 0 ), 9, "Use OOC Filter", JailSystem.useOOCFilter );
			message();
			if( !JailSystem.useOOCFilter )
				return;
			AddButton( TitleX + 50, TitleY + 75, 239, 240, 10, GumpButtonType.Reply, 0 );
			//AddLabel(basex( BodyX),basey(BodyY,0),200,"Commands");
			AddLabel( basex( BodyX ) + 15, basey( BodyY, 0 ), 200, "OOCList" );
			AddTextField( basex( BodyX ) + 90, basey( BodyY, 0 ), 130, 20, 11, JailSystem.ooclistCommand );
			//AddLabel(basex( BodyX),basey(BodyY,2),200,"Misc.");
			AddLabel( basex( BodyX ) + 15, basey( BodyY, 1 ), 200, "OOC Jailor" );
			AddTextField( basex( BodyX ) + 90, basey( BodyY, 1 ), 130, 20, 12, JailSystem.oocJailorName );
			AddToggleLabeled( basex( BodyX ) + 15, basey( BodyY, 2 ), 11, "Block OOC speech", JailSystem.blockOOCSpeech );
			AddToggleLabeled( basex( BodyX ) + 15, basey( BodyY, 3 ), 12, "Allow Staff to go OOC", JailSystem.AllowStaffOOC );
			AddLabel( basex( BodyX ) + 15, basey( BodyY, 4 ), 200, "OOC Warnings" );
			AddTextField( basex( BodyX ) + 120, basey( BodyY, 4 ), 50, 20, 13, JailSystem.oocwarns.ToString() );

			AddLabel( basex( BodyX ), basey( BodyY, 5 ), 200, "OOC Parts" );
			string temp = "";
			foreach( string p in JailSystem.oocParts )
				temp += string.Format( "{0}\n", p );
			AddColorLabelScroll( basex( BodyX ), basey( BodyY, 6 ), 150, 60, temp.Trim() );

			AddTextField( basex( BodyX ), basey( BodyY, 6 ) + 65, 150, 20, 15 );
			AddButton( basex( BodyX ), basey( BodyY, 6 ) + 90, 2461, 2462, 34, GumpButtonType.Reply, 0 );
			AddButton( basex( BodyX ) + 55, basey( BodyY, 6 ) + 90, 2464, 2465, 35, GumpButtonType.Reply, 0 );

			AddLabel( basex( BodyX ) + 240, basey( BodyY, 0 ), 200, "OOC Words" );
			temp = "";
			foreach( string p in JailSystem.oocWords )
				temp += string.Format( "{0}\n", p );
			AddColorLabelScroll( basex( BodyX ) + 240, basey( BodyY, 1 ), 150, 60, temp.Trim() );

			AddTextField( basex( BodyX ) + 240, basey( BodyY, 1 ) + 65, 150, 20, 14 );
			AddButton( BodyX + 240, basey( BodyY, 1 ) + 90, 2461, 2462, 32, GumpButtonType.Reply, 0 );
			AddButton( BodyX + 295, basey( BodyY, 1 ) + 90, 2464, 2465, 33, GumpButtonType.Reply, 0 );

			AddLabel( basex( BodyX ) + 240, basey( BodyY, 5 ) + 10, 200, "Jail Terms" );
			temp = "";
			foreach( TimeSpan t in JailSystem.FoulMouthJailTimes )
				temp += string.Format( "d={0} h={1} m={2}\n", t.Days, t.Hours, t.Minutes );
			AddColorLabelScroll( basex( BodyX ) + 240, basey( BodyY, 6 ) + 10, 150, 60, temp.Trim() );

			AddLabel( basex( BodyX ) + 240, basey( BodyY, 6 ) + 75, 200, "D" );
			AddLabel( basex( BodyX ) + 290, basey( BodyY, 6 ) + 75, 200, "H" );
			AddLabel( basex( BodyX ) + 340, basey( BodyY, 6 ) + 75, 200, "M" );
			AddTextField( basex( BodyX ) + 255, basey( BodyY, 6 ) + 75, 30, 20, 8 );
			AddTextField( basex( BodyX ) + 305, basey( BodyY, 6 ) + 75, 30, 20, 9 );
			AddTextField( basex( BodyX ) + 355, basey( BodyY, 6 ) + 75, 30, 20, 10 );
			AddButton( BodyX + 240, basey( BodyY, 6 ) + 100, 2461, 2462, 30, GumpButtonType.Reply, 0 );
			AddButton( BodyX + 295, basey( BodyY, 6 ) + 100, 2464, 2465, 31, GumpButtonType.Reply, 0 );
		}

		private void buildSettings()
		{
			message();
			AddButton( TitleX + 50, TitleY + 75, 239, 240, 6, GumpButtonType.Reply, 0 );

			AddLabel( basex( BodyX ), basey( BodyY, 0 ), 200, "Commands" );
			AddLabel( basex( BodyX ) + 15, basey( BodyY, 1 ), 200, "Status" );
			AddLabel( basex( BodyX ) + 15, basey( BodyY, 2 ), 200, "Time" );
			AddTextField( basex( BodyX ) + 60, basey( BodyY, 1 ), 150, 20, 1, JailSystem.statusCommand );
			AddTextField( basex( BodyX ) + 60, basey( BodyY, 2 ), 150, 20, 2, JailSystem.timeCommand );

			AddLabel( basex( BodyX ), basey( BodyY, 3 ), 200, "Misc." );
			AddLabel( basex( BodyX ) + 15, basey( BodyY, 4 ), 200, "Name" );
			AddTextField( basex( BodyX ) + 60, basey( BodyY, 4 ), 150, 20, 3, JailSystem.JSName );
			AddLabel( basex( BodyX ) + 65, basey( BodyY, 5 ), 200, string.Format( "Jail Facet:{0}", JailSystem.jailMap.Name ) );
			AddButton( basex( BodyX ) + 15, basey( BodyY, 5 ), 2471, 2470, 20, GumpButtonType.Reply, 0 );
			AddToggleLabeled( basex( BodyX ) + 15, basey( BodyY, 6 ), 7, "Use Smoking Shoes", JailSystem.useSmokingFootGear );

			AddLabel( basex( BodyX ), basey( BodyY, 7 ), 200, "Non-Default Release Setting" );
			AddToggleLabeled( basex( BodyX ) + 15, basey( BodyY, 8 ), 8, "Single Facet release", JailSystem.SingleFacetOnly );
			AddLabel( basex( BodyX ) + 65, basey( BodyY, 9 ) + 10, 200, string.Format( "Release Facet:{0}", JailSystem.defaultReleaseFacet.Name ) );
			AddButton( BodyX + 15, basey( BodyY, 9 ) + 10, 2471, 2470, 21, GumpButtonType.Reply, 0 );

			AddLabel( basex( BodyX ) + 240, basey( BodyY, 0 ), 200, "Cells" );
			string temp = "";
			foreach( Point3D p in JailSystem.cells )
				temp += p + "\n";
			AddColorLabelScroll( basex( BodyX ) + 240, basey( BodyY, 1 ), 150, 60, temp.Trim() );

			AddTextField( basex( BodyX ) + 240, basey( BodyY, 1 ) + 65, 45, 20, 5 );
			AddTextField( basex( BodyX ) + 290, basey( BodyY, 1 ) + 65, 45, 20, 6 );
			AddTextField( basex( BodyX ) + 340, basey( BodyY, 1 ) + 65, 45, 20, 7 );
			AddButton( BodyX + 240, basey( BodyY, 1 ) + 90, 2461, 2462, 22, GumpButtonType.Reply, 0 );
			AddButton( BodyX + 295, basey( BodyY, 1 ) + 90, 2464, 2465, 23, GumpButtonType.Reply, 0 );

			AddLabel( basex( BodyX ) + 240, basey( BodyY, 5 ) + 10, 200, "Default Release Loctions" );
			temp = "";
			foreach( Point3D p in JailSystem.defaultRelease )
				temp += p + "\n";
			AddColorLabelScroll( basex( BodyX ) + 240, basey( BodyY, 6 ) + 10, 150, 60, temp.Trim() );

			AddTextField( basex( BodyX ) + 240, basey( BodyY, 6 ) + 75, 45, 20, 8 );
			AddTextField( basex( BodyX ) + 290, basey( BodyY, 6 ) + 75, 45, 20, 9 );
			AddTextField( basex( BodyX ) + 340, basey( BodyY, 6 ) + 75, 45, 20, 10 );
			AddButton( BodyX + 240, basey( BodyY, 6 ) + 100, 2461, 2462, 24, GumpButtonType.Reply, 0 );
			AddButton( BodyX + 295, basey( BodyY, 6 ) + 100, 2464, 2465, 25, GumpButtonType.Reply, 0 );
		}

		private void buildReviews()
		{
			if( JailSystem.list.Count < m_id )
				m_id = 0;
			if( m_id < 0 )
				m_id = JailSystem.list.Count - 1;
			if( JailSystem.list.Count == 0 )
				m_id = -1;
			int i = 0;
			if( m_id >= 0 )
				foreach( JailSystem tj in JailSystem.list.Values )
				{
					if( ( i == 0 ) || ( i == m_id ) )
						js = tj;
					i++;
				}
			if( m_id == -1 )
			{
				AddLabel( BodyX + gutterOffset, BodyY + gutterOffset, 200, "No accounts are currently jailed." );
				return;
			}
			AddLabel( TitleX + gutterOffset, TitleY + gutterOffset, 200, "Reviewing: " + js.Name );
			AddLabel( TitleX + gutterOffset, TitleY + gutterOffset + LineStep, 200, string.Format( "Jailed Account {0} of {1}", m_id + 1, JailSystem.list.Count ) );
			//previous button
			AddButton( TitleX + gutterOffset, TitleY + gutterOffset + LineStep + LineStep + 5, 2466, 2467, 44, GumpButtonType.Reply, 0 );
			//next Button
			AddButton( TitleX + gutterOffset + 80, TitleY + gutterOffset + LineStep + LineStep + 5, 2469, 2470, 45, GumpButtonType.Reply, 0 );
			string temp = "";
			if( js.Prisoner == null )
				js.killJail();
			else
			{
				foreach( AccountComment note in js.Prisoner.Comments )
					if( ( note.AddedBy == JailSystem.JSName + "-warning" ) || ( note.AddedBy == JailSystem.JSName + "-jailed" ) || ( note.AddedBy == JailSystem.JSName + "-note" ) )
						temp = temp + note.AddedBy + "\n\r" + note.Content + "\n\r***********\n\r";
				AddLabel( BodyX + 17, BodyY + 8, 200, "History" );
				AddHtml( BodyX + 13, 141, 300, 82, temp, true, true );
				//release
				AddButton( BodyX + 13, BodyY + 120, 2472, 2473, 41, GumpButtonType.Reply, 0 );
				AddLabel( BodyX + 43, BodyY + 123, 200, "Release" );
				AddButton( BodyX + 13, BodyY + 150, 2472, 2473, 50, GumpButtonType.Reply, 0 );
				AddLabel( BodyX + 43, BodyY + 153, 200, "Ban" );
				//add day
				AddButton( BodyX + 101, BodyY + 120, 250, 251, 43, GumpButtonType.Reply, 0 );
				AddButton( BodyX + 116, BodyY + 120, 252, 253, 47, GumpButtonType.Reply, 0 );
				AddLabel( 135 + BodyX, BodyY + 123, 200, "Week" );
				//add week
				AddButton( BodyX + 176, BodyY + 120, 250, 251, 42, GumpButtonType.Reply, 0 );
				AddButton( BodyX + 191, BodyY + 120, 252, 253, 46, GumpButtonType.Reply, 0 );
				AddLabel( BodyX + 210, BodyY + 123, 200, "Day" );
				//hours
				AddButton( BodyX + 251, BodyY + 120, 250, 251, 48, GumpButtonType.Reply, 0 );
				AddButton( BodyX + 266, BodyY + 120, 252, 253, 49, GumpButtonType.Reply, 0 );
				AddLabel( BodyX + 284, BodyY + 123, 200, "Hour" );

				AddLabel( BodyX + 13, BodyY + 170, 200, "Release at: " + js.ReleaseDate );
				if( !js.jailed )
					message( "This account has been released but currently has characters in jail." );
				else
					message( "This account is currently jailed." );
				AddHtml( BodyX + 13, BodyY + 189, 300, 74, js.reason, true, true );
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			string temp;
			switch( info.ButtonID )
			{
				case 50:
					//ban an account;
					from.SendGump( new JailBanGump( js ) );
					break;
				case 10:
					temp = info.GetTextEntry( 11 ).Text.Trim().ToLower();
					if( !( temp == "" ) && !( temp == null ) )
						JailSystem.ooclistCommand = temp;
					temp = info.GetTextEntry( 12 ).Text.Trim();
					if( !( temp == "" ) && !( temp == null ) )
						JailSystem.oocJailorName = temp;

					temp = info.GetTextEntry( 13 ).Text.Trim().ToLower();
					if( !( temp == "" ) && !( temp == null ) )
						try
						{
							JailSystem.oocwarns = Convert.ToInt32( temp );
						}
						catch
						{
							from.SendMessage( "Bad number of OOC Warnings." );
						}
					goto case 2;
				case 11:
					JailSystem.blockOOCSpeech = !JailSystem.blockOOCSpeech;
					goto case 10;
				case 12:
					JailSystem.AllowStaffOOC = !JailSystem.AllowStaffOOC;
					goto case 10;
				case 15:
					//language section
					temp = info.GetTextEntry( 12 ).Text.Trim();
					if( !( temp == "" ) && !( temp == null ) )
						JailSystem.foulJailorName = temp;
					JailSystem.FoulMouthJailTimes.Sort();
					goto case 3;
				case 13:
					JailSystem.useLanguageFilter = !JailSystem.useLanguageFilter;
					goto case 3;
				case 14:
					JailSystem.allowStaffBadWords = !JailSystem.allowStaffBadWords;
					goto case 15;
				case 9:
					JailSystem.useOOCFilter = !JailSystem.useOOCFilter;
					goto case 2;
					//generenal section
				case 1:
					from.SendGump( new JailAdminGump( AdminJailGumpPage.General ) );
					break;
				case 2:
					from.SendGump( new JailAdminGump( AdminJailGumpPage.OOC ) );
					break;
				case 3:
					from.SendGump( new JailAdminGump( AdminJailGumpPage.Language ) );
					break;
				case 4:
					from.SendGump( new JailAdminGump( AdminJailGumpPage.Review ) );
					break;
				case 5:
					from.CloseGump( typeof( JailAdminGump ) );
					break;
				case 6:
					temp = info.GetTextEntry( 1 ).Text.Trim().ToLower();
					if( !( temp == "" ) && !( temp == null ) )
						JailSystem.statusCommand = temp;

					temp = info.GetTextEntry( 2 ).Text.Trim().ToLower();
					if( !( temp == "" ) && !( temp == null ) )
						JailSystem.timeCommand = temp;

					temp = info.GetTextEntry( 3 ).Text.Trim();
					if( !( temp == "" ) && !( temp == null ) )
						JailSystem.JSName = temp;
					goto case 1;
				case 7:
					JailSystem.useSmokingFootGear = !JailSystem.useSmokingFootGear;
					goto case 6;
				case 8:
					JailSystem.SingleFacetOnly = !JailSystem.SingleFacetOnly;
					goto case 6;
				case 20:
					if( JailSystem.jailMap == Map.Felucca )
						JailSystem.jailMap = Map.Trammel;
					else if( JailSystem.jailMap == Map.Trammel )
						JailSystem.jailMap = Map.Ilshenar;
					else if( JailSystem.jailMap == Map.Ilshenar )
						JailSystem.jailMap = Map.Malas;
					else if( JailSystem.jailMap == Map.Malas )
						JailSystem.jailMap = Map.Felucca;
					goto case 6;
				case 21:
					if( JailSystem.defaultReleaseFacet == Map.Felucca )
						JailSystem.defaultReleaseFacet = Map.Trammel;
					else if( JailSystem.defaultReleaseFacet == Map.Trammel )
						JailSystem.defaultReleaseFacet = Map.Ilshenar;
					else if( JailSystem.defaultReleaseFacet == Map.Ilshenar )
						JailSystem.defaultReleaseFacet = Map.Malas;
					else if( JailSystem.defaultReleaseFacet == Map.Malas )
						JailSystem.defaultReleaseFacet = Map.Felucca;
					//change facet
					goto case 6;
				case 22:
					//add cell
					try
					{
						Point3D p = new Point3D( Convert.ToInt32( info.GetTextEntry( 5 ).Text.Trim() ), Convert.ToInt32( info.GetTextEntry( 6 ).Text.Trim() ), Convert.ToInt32( info.GetTextEntry( 7 ).Text.Trim() ) );
						if( JailSystem.cells.Contains( p ) )
							from.SendMessage( "Unable to add jail cell. It is already listed." );
						else
							JailSystem.cells.Add( p );
					}
					catch
					{
						from.SendMessage( "Unable to add jail cell. Bad x,y,z." );
					}
					goto case 6;
				case 23:
					//remove cell
					try
					{
						Point3D p = new Point3D( Convert.ToInt32( info.GetTextEntry( 5 ).Text.Trim() ), Convert.ToInt32( info.GetTextEntry( 6 ).Text.Trim() ), Convert.ToInt32( info.GetTextEntry( 7 ).Text.Trim() ) );
						if( JailSystem.cells.Contains( p ) )
							JailSystem.cells.Remove( p );
						else
							from.SendMessage( "Unable to remove jail cell. Cell not listed." );
					}
					catch
					{
						from.SendMessage( "Unable to remove jail cell. Bad x,y,z." );
					}
					goto case 6;
				case 24:
					//add release
					try
					{
						Point3D p = new Point3D( Convert.ToInt32( info.GetTextEntry( 8 ).Text.Trim() ), Convert.ToInt32( info.GetTextEntry( 9 ).Text.Trim() ), Convert.ToInt32( info.GetTextEntry( 10 ).Text.Trim() ) );
						if( JailSystem.defaultRelease.Contains( p ) )
							from.SendMessage( "Unable to add default release location. It is already listed." );
						else
							JailSystem.defaultRelease.Add( p );
					}
					catch
					{
						from.SendMessage( "Unable to add release location. Bad x,y,z." );
					}
					goto case 6;
				case 25:
					//remove release
					try
					{
						Point3D p = new Point3D( Convert.ToInt32( info.GetTextEntry( 8 ).Text.Trim() ), Convert.ToInt32( info.GetTextEntry( 9 ).Text.Trim() ), Convert.ToInt32( info.GetTextEntry( 10 ).Text.Trim() ) );
						if( JailSystem.defaultRelease.Contains( p ) )
							JailSystem.defaultRelease.Remove( p );
						else
							from.SendMessage( "Release location not listed." );
					}
					catch
					{
						from.SendMessage( "Unable to remove release location. Bad x,y,z." );
					}
					goto case 6;
				case 26:
					//add foul word
					try
					{
						temp = info.GetTextEntry( 13 ).Text.ToLower().Trim();
						if( ( temp == "" ) || ( temp == null ) )
							from.SendMessage( "Unable to add word" );
						else if( JailSystem.badWords.Contains( temp ) )
							from.SendMessage( "Word is already in the list." );
						else
							JailSystem.badWords.Add( temp );
					}
					catch
					{
						from.SendMessage( "Unable to add word" );
					}
					goto case 15;
				case 27:
					//remove foul word
					try
					{
						temp = info.GetTextEntry( 13 ).Text.ToLower().Trim();
						if( ( temp == "" ) || ( temp == null ) )
							from.SendMessage( "Unable to remove word" );
						else if( JailSystem.badWords.Contains( temp ) )
							JailSystem.badWords.Remove( temp );
						else
							from.SendMessage( "Word is not in the list." );
					}
					catch
					{
						from.SendMessage( "Unable to remove word" );
					}
					goto case 15;
				case 28:
					//add jail term
					try
					{
						TimeSpan p = new TimeSpan( Convert.ToInt32( info.GetTextEntry( 8 ).Text.Trim() ), Convert.ToInt32( info.GetTextEntry( 9 ).Text.Trim() ), Convert.ToInt32( info.GetTextEntry( 10 ).Text.Trim() ), 0, 0 );
						if( JailSystem.FoulMouthJailTimes.Contains( p ) )
							from.SendMessage( "Unable to add jail term. It is already listed." );
						else
							JailSystem.FoulMouthJailTimes.Add( p );
					}
					catch
					{
						from.SendMessage( "Unable to add jail term. Bad D,H,M." );
					}
					goto case 15;
				case 29:
					//remove jail term
					try
					{
						TimeSpan p = new TimeSpan( Convert.ToInt32( info.GetTextEntry( 8 ).Text.Trim() ), Convert.ToInt32( info.GetTextEntry( 9 ).Text.Trim() ), Convert.ToInt32( info.GetTextEntry( 10 ).Text.Trim() ), 0, 0 );
						if( JailSystem.FoulMouthJailTimes.Contains( p ) )
							JailSystem.FoulMouthJailTimes.Remove( p );
						else
							from.SendMessage( "Jail term not listed." );
					}
					catch
					{
						from.SendMessage( "Unable to remove Jail term. Bad D,H,M." );
					}
					goto case 15;

				case 30:
					//add jail term
					try
					{
						TimeSpan p = new TimeSpan( Convert.ToInt32( info.GetTextEntry( 8 ).Text.Trim() ), Convert.ToInt32( info.GetTextEntry( 9 ).Text.Trim() ), Convert.ToInt32( info.GetTextEntry( 10 ).Text.Trim() ), 0, 0 );
						if( JailSystem.oocJailTimes.Contains( p ) )
							from.SendMessage( "Unable to add jail term. It is already listed." );
						else
							JailSystem.oocJailTimes.Add( p );
					}
					catch
					{
						from.SendMessage( "Unable to add jail term. Bad D,H,M." );
					}
					goto case 10;
				case 31:
					//remove jail term
					try
					{
						TimeSpan p = new TimeSpan( Convert.ToInt32( info.GetTextEntry( 8 ).Text.Trim() ), Convert.ToInt32( info.GetTextEntry( 9 ).Text.Trim() ), Convert.ToInt32( info.GetTextEntry( 10 ).Text.Trim() ), 0, 0 );
						if( JailSystem.oocJailTimes.Contains( p ) )
							JailSystem.oocJailTimes.Remove( p );
						else
							from.SendMessage( "Jail term not listed." );
					}
					catch
					{
						from.SendMessage( "Unable to remove Jail term. Bad D,H,M." );
					}
					goto case 10;
				case 32:
					//add ooc word
					try
					{
						temp = info.GetTextEntry( 14 ).Text.ToLower().Trim();
						if( ( temp == "" ) || ( temp == null ) )
							from.SendMessage( "Unable to add word" );
						else if( JailSystem.oocWords.Contains( temp ) )
							from.SendMessage( "Word is already in the list." );
						else
							JailSystem.oocWords.Add( temp );
					}
					catch
					{
						from.SendMessage( "Unable to add word" );
					}
					goto case 10;
				case 33:
					//remove ooc word
					try
					{
						temp = info.GetTextEntry( 14 ).Text.ToLower().Trim();
						if( ( temp == "" ) || ( temp == null ) )
							from.SendMessage( "Unable to remove word" );
						else if( JailSystem.oocWords.Contains( temp ) )
							JailSystem.oocWords.Remove( temp );
						else
							from.SendMessage( "Word is not in the list." );
					}
					catch
					{
						from.SendMessage( "Unable to remove word" );
					}
					goto case 10;
				case 34:
					//add ooc part
					try
					{
						temp = info.GetTextEntry( 15 ).Text.ToLower().Trim();
						if( ( temp == "" ) || ( temp == null ) )
							from.SendMessage( "Unable to add word" );
						else if( JailSystem.oocParts.Contains( temp ) )
							from.SendMessage( "Word is already in the list." );
						else
							JailSystem.oocParts.Add( temp );
					}
					catch
					{
						from.SendMessage( "Unable to add word" );
					}
					goto case 10;
				case 35:
					//remove ooc part
					try
					{
						temp = info.GetTextEntry( 15 ).Text.ToLower().Trim();
						if( ( temp == "" ) || ( temp == null ) )
							from.SendMessage( "Unable to remove word" );
						else if( JailSystem.oocParts.Contains( temp ) )
							JailSystem.oocParts.Remove( temp );
						else
							from.SendMessage( "Word is not in the list." );
					}
					catch
					{
						from.SendMessage( "Unable to remove word" );
					}
					goto case 10;
				case 41:
					js.forceRelease( from );
					from.SendGump( new JailAdminGump( m_page, m_subpage, m_id ) );
					break;
				case 42:
					js.AddDays( 1 );
					from.SendGump( new JailAdminGump( m_page, m_subpage, m_id ) );
					break;
				case 46:
					js.subtractDays( 1 );
					from.SendGump( new JailAdminGump( m_page, m_subpage, m_id ) );
					break;
				case 47:
					js.subtractDays( 7 );
					from.SendGump( new JailAdminGump( m_page, m_subpage, m_id ) );
					break;
				case 48:
					js.AddHours( 1 );
					from.SendGump( new JailAdminGump( m_page, m_subpage, m_id ) );
					break;
				case 49:
					js.subtractHours( 1 );
					from.SendGump( new JailAdminGump( m_page, m_subpage, m_id ) );
					break;
				case 43:
					js.AddDays( 7 );
					from.SendGump( new JailAdminGump( m_page, m_subpage, m_id ) );
					break;
				case 44:
					//previous button
					m_id--;
					if( m_id < 0 )
						m_id = JailSystem.list.Count - 1;
					from.SendGump( new JailAdminGump( m_page, m_subpage, m_id ) );
					break;
				case 45:
					//next button
					m_id++;
					if( m_id >= JailSystem.list.Count )
						m_id = 0;
					from.SendGump( new JailAdminGump( m_page, m_subpage, m_id ) );
					break;
				default:
					break;
			}
			//from.CloseGump(typeof ( JailAdminGump ));
		}
	}
}