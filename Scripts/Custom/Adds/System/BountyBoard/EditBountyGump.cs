using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;

namespace Server.BountySystem
{
	public class EditBountyGump : Gump
	{
		private static int HtmlColor = 0xFFFFFF;
		private Mobile m_From;
		private BountyBoardEntry m_Entry;
		private int m_Page;

		private ArrayList m_Requested;
		private ArrayList m_Accepted;

		public EditBountyGump( Mobile from, BountyBoardEntry entry ) : this( from, entry, 0, null, null )
		{
		}
		public int GetIndexForPage( int page )
		{
			int index = 0;

			while ( page-- > 0 )
				index += 5;

			return index;
		}

		public string Color( string text, int color )
		{
			return String.Format( "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text );
		}

		public EditBountyGump( Mobile from, BountyBoardEntry entry, int page, ArrayList requestList,
			ArrayList acceptList ) : base( 50, 50 )
		{
			if(entry == null)
				return;

			m_From = from;
			m_Entry = entry;
			m_Page = page;

			if ( requestList == null )
			{
				requestList = new ArrayList( entry.Requested.Count );

				for ( int i = 0; i < entry.Requested.Count; ++i )
				{
					Mobile request = (Mobile) entry.Requested[i];
					requestList.Add( request );
				}
			}

			m_Requested = requestList;

			if ( acceptList == null )
			{
				acceptList = new ArrayList( entry.Accepted.Count );

				for ( int i = 0; i < entry.Accepted.Count; ++i )
				{
					Mobile accept = (Mobile) entry.Accepted[i];
					acceptList.Add( accept );
				}
			}

			m_Accepted = acceptList;

			int index = GetIndexForPage( page );
			int count = 5;
			int tableIndex = 0;

			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;

			AddPage(0);
			AddBackground(0, 0, 440, 324, 9200);
			AddHtml(160, 5, 200, 32, Color( "Edit Bounty", HtmlColor ), false, false );
			AddHtml(10, 32, 200, 32, Color( "Wanted", HtmlColor ), false, false );
			AddHtml(175, 32, 200, 32, Color( "Price", HtmlColor ), false, false );
			AddHtml(260, 32, 200, 32, Color( "Expires", HtmlColor ), false, false );
			AddImageTiled( 10, 50, 400, 2, 2624 );

			AddHtml(10, 96, 200, 32, Color( "Requested", HtmlColor ), false, false );
			AddHtml(230, 96, 200, 32, Color( "Approved", HtmlColor ), false, false );
			AddImageTiled( 10, 115, 400, 2, 2624 );

			AddHtmlLocalized( 370, 290, 200, 32, 1011441, HtmlColor, false, false ); // exit
			AddButton(340, 290, 4017, 4018, 0, GumpButtonType.Reply, 0); // exit

			AddHtml( 10, 64, 200, 32, Color( m_Entry.Wanted.Name, HtmlColor ), false, false );
			AddHtml( 175, 64, 200, 32, Color( m_Entry.Price.ToString(), HtmlColor ), false, false );
			AddHtml( 260, 64, 200, 32, Color( m_Entry.ExpireTime.ToString(), HtmlColor ), false, false );

			if ( page > 0 )
			{
				AddHtmlLocalized( 60, 290, 150, 20, 1011067, HtmlColor, false, false ); // Previous page
				AddButton(30, 290, 4014, 4016, 1, GumpButtonType.Reply, 0);
			}

			if ( ( GetIndexForPage( page + 1 ) < m_Requested.Count ) || 
				( GetIndexForPage( page + 1 ) < m_Accepted.Count ) )
			{
				AddHtmlLocalized( 230, 290, 150, 20, 1011066, HtmlColor, false, false ); // Next page
				AddButton(200, 290, 4005, 4007, 2, GumpButtonType.Reply, 0);
			}

			tableIndex = 0;

			for ( int i = index; i < (index + count) && i >= 0 && i < m_Requested.Count; ++i )
			{
				Mobile request = (Mobile) m_Requested[i];
				int y = 128 + (tableIndex++ * 32);
				AddButton(10, y, 2117, 2118, 3 + (i * 1), GumpButtonType.Reply, 0);
				AddHtml( 40, y, 200, 32, Color( request.Name, HtmlColor ), false, false );
			}

			tableIndex = 0;

			for ( int i = index; i < (index + count) && i >= 0 && i < m_Accepted.Count; ++i )
			{
				Mobile accept = (Mobile) m_Accepted[i];
				int y = 128 + (tableIndex++ * 32);
				AddHtml( 230, y, 200, 32, Color( accept.Name, HtmlColor ), false, false );
			}
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			int index = info.ButtonID;

			switch ( index )
			{
				case 0: // EXIT
				{
					break;
				}
				case 1: // Previous page
				{
					if ( m_Page > 0 )
						m_From.SendGump( new EditBountyGump( m_From, m_Entry, m_Page - 1, m_Requested, m_Accepted ) );

					break;
				}
				case 2: // Next page
				{
					if ( ( GetIndexForPage( m_Page + 1 ) < m_Requested.Count ) ||
						( GetIndexForPage( m_Page + 1 ) < m_Accepted.Count ) )
						m_From.SendGump( new EditBountyGump( m_From, m_Entry, m_Page + 1, m_Requested, m_Accepted ) );

					break;
				}
				default:
				{
					index -= 3;

					int type = index % 1;
					index /= 1;

					if ( index < 0 || index >= m_Requested.Count )
						break;

					Mobile request = (Mobile) m_Requested[index];

					m_Entry.Requested.Remove( request );
					m_Entry.Accepted.Add( request );


					string msg = String.Format( "{0} has granted permission to seek the bounty on {1}, on completion return the head to any Order Guard.", 
						m_Entry.Owner.Name, m_Entry.Wanted.Name );

					if( NetState.Instances.Contains( request.NetState ) )
						request.SendMessage( msg );
					else
					{
						(request as PlayerMobile).ShowBountyUpdate = true;
						(request as PlayerMobile).BountyUpdateList.Add( msg );
					}

					m_From.SendGump( new EditBountyGump( m_From, m_Entry ) );

					break;
				}
			}
		}
	}
}