using System;
using System.Text;
using System.Collections;
using Server;
using Server.Gumps;

namespace Server.BountySystem
{
	public class BountyStatusGump : Gump
	{
		private static int HtmlColor = 0xFFFFFF;
		private Mobile m_From;
		private ArrayList m_Lines;

		public ArrayList Lines
		{
			get{ return m_Lines; }
		}

		public string Color( string text, int color )
		{
			return String.Format( "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text );
		}

		public BountyStatusGump( Mobile from, ArrayList lines ) : base( 50, 50 )
		{
			m_From = from;
			m_Lines = lines;

			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			AddPage(0);
			AddBackground(0, 0, 350, 150, 9200);
			AddHtml( 106, 3, 200, 32, Color( "Bounty Status", HtmlColor ), false, false );

			StringBuilder sb = new StringBuilder();

			for(int i=0; i<m_Lines.Count; ++i)
			{
				if( i>0 )
					sb.Append( "<br>" );

				sb.Append( (string) m_Lines[i] );
			}

			AddHtml( 20, 40, 310, 90, sb.ToString(), true, true );
		}
	}
}