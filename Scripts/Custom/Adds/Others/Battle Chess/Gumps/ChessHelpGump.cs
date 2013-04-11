using Server;
using Server.Gumps;

namespace Arya.Chess
{
	public class ChessHelpGump : Gump
	{
		private const int LabelHue = 0x480;
		private const int GreenHue = 0x40;

		public ChessHelpGump( Mobile m ) : base( 150, 150 )
		{
			m.CloseGump( typeof( ChessHelpGump ) );
			MakeGump();
		}

		private void MakeGump()
		{
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			AddPage(0);
			AddBackground(0, 0, 420, 271, 9300);
			AddAlphaRegion( 0, 0, 420, 271 );

			AddLabel(134, 5, GreenHue, @"Chess Game Information");

			AddButton(20, 32, 5601, 5605, 1, GumpButtonType.Reply, 0);
			AddLabel(45, 30, LabelHue, @"Basic rules and pieces moves");
			AddButton(20, 57, 5601, 5605, 2, GumpButtonType.Reply, 0);
			AddLabel(45, 55, LabelHue, @"General Chess FAQ");
			AddButton(20, 82, 5601, 5605, 3, GumpButtonType.Reply, 0);
			AddLabel(45, 80, LabelHue, @"Check, Checkmate and Stalemate FAQ");
			AddButton(20, 107, 5601, 5605, 4, GumpButtonType.Reply, 0);
			AddLabel(45, 105, LabelHue, @"Castle FAQ");
			AddLabel(45, 120, LabelHue, @"Note: To castle in Battle Chess, select the King and move it");
			AddLabel(45, 135, LabelHue, @"on the Rook (or do exactly the opposite)");
			AddButton(20, 162, 5601, 5605, 5, GumpButtonType.Reply, 0);
			AddLabel(45, 160, LabelHue, @"The En Passant Move");
			AddButton(20, 207, 5601, 5605, 6, GumpButtonType.Reply, 0);
			AddLabel(20, 185, GreenHue, @"Pieces FAQs");
			AddLabel(40, 205, LabelHue, @"Pawn");
			AddButton(85, 207, 5601, 5605, 7, GumpButtonType.Reply, 0);
			AddLabel(105, 205, LabelHue, @"Knight");
			AddButton(155, 207, 5601, 5605, 8, GumpButtonType.Reply, 0);
			AddLabel(175, 205, LabelHue, @"Queen");
			AddButton(220, 207, 5601, 5605, 9, GumpButtonType.Reply, 0);
			AddLabel(240, 205, LabelHue, @"King");
			AddButton(20, 242, 5601, 5605, 0, GumpButtonType.Reply, 0);
			AddLabel(40, 240, LabelHue, @"Exit");
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			string web = null;

			switch ( info.ButtonID )
			{
				case 1 : web = "http://www.chessvariants.com/d.chess/chess.html";
					break;

				case 2 : web = "http://www.chessvariants.com/d.chess/faq.html";
					break;

				case 3 : web = "http://www.chessvariants.com/d.chess/matefaq.html";
					break;

				case 4 : web = "http://www.chessvariants.com/d.chess/castlefaq.html";
					break;

				case 5 : web = "http://www.chessvariants.com/d.chess/enpassant.html";
					break;

				case 6 : web = "http://www.chessvariants.com/d.chess/pawnfaq.html";
					break;

				case 7 : web = "http://www.chessvariants.com/d.chess/knightfaq.html";
					break;

				case 8 : web = "http://www.chessvariants.com/d.chess/queenfaq.html";
					break;

				case 9 : web = "http://www.chessvariants.com/d.chess/kingfaq.html";
					break;
			}

			if ( web != null )
			{
				sender.Mobile.LaunchBrowser( web );
				sender.Mobile.SendGump( this );
			}
		}
	}
}
