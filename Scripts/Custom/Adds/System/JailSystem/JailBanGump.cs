using System;
using Server.Jailing;
using Server.Network;

namespace Server.Gumps
{
	public class JailBanGump : Gump
	{
		private const int LabelColor32 = 0xFFFFFF;
		private const int SelectedColor32 = 0x8080FF;
		private const int DisabledColor32 = 0x808080;
		private JailSystem m_js = null;

		public JailBanGump( JailSystem js ) : base( 10, 30 )
		{
			buildit( js );
		}

		public void AddBlackAlpha( int x, int y, int width, int height )
		{
			AddImageTiled( x, y, width, height, 2624 );
			AddAlphaRegion( x, y, width, height );
		}

		public string Color( string text, int color )
		{
			return String.Format( "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text );
		}

		public void AddColorLabel( int x, int y, int width, int height, string text )
		{
			AddHtml( x, y, width, height, Color( text, LabelColor32 ), false, false );
		}

		public void buildit( JailSystem js )
		{
			m_js = js;
			AddBackground( 0, 0, 300, 300, 5054 );
			AddBlackAlpha( 5, 5, 290, 290 );
			AddColorLabel( 8, 8, 288, 288, Color( "Are you sure you wish to ban this account?", SelectedColor32 ) );
			AddButton( 120, 200, 241, 243, 10, GumpButtonType.Reply, 0 );
			AddButton( 50, 200, 239, 240, 6, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			if( info.ButtonID == 6 )
				m_js.ban( from );
				//ban them here
		}
	}
}