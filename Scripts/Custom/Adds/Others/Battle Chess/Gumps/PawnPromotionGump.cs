using Server;
using Server.Gumps;

namespace Arya.Chess
{
	public class PawnPromotionGump : Gump
	{
		private const int LabelHue = 0x480;
		private const int GreenHue = 0x40;

	    readonly ChessGame m_Game;

		public PawnPromotionGump( Mobile m, ChessGame game ) : base( 200, 200 )
		{
			m.CloseGump( typeof( PawnPromotionGump ) );
			m_Game = game;
			MakeGump();
		}

		private void MakeGump()
		{
			Closable=false;
			Disposable=true;
			Dragable=true;
			Resizable=false;

			AddPage(0);
			AddBackground(0, 0, 255, 185, 9200);
			AddImageTiled(0, 0, 255, 185, 9304);
			AddImageTiled(1, 1, 253, 183, 9274);
			AddAlphaRegion(1, 1, 253, 183);

			AddLabel(20, 10, GreenHue, @"Your pawn is being promoted!");

			AddLabel(20, 35, LabelHue, @"Promote to:");

			AddButton(35, 70, 2337, 2344, 1, GumpButtonType.Reply, 0);
			AddLabel(25, 120, LabelHue, @"Queen");

			AddButton(85, 70, 2333, 2340, 2, GumpButtonType.Reply, 0);
			AddLabel(80, 120, LabelHue, @"Rook");

			AddButton(135, 70, 2335, 2344, 3, GumpButtonType.Reply, 0);
			AddLabel(130, 120, LabelHue, @"Knight");

			AddButton(195, 70, 2332, 2339, 4, GumpButtonType.Reply, 0);
			AddLabel(185, 120, LabelHue, @"Bishop");
			
			AddButton(25, 152, 9702, 248, 5, GumpButtonType.Reply, 0);
			AddLabel(50, 150, 0, @"Do not promote pawn");
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			PawnPromotion type = PawnPromotion.None;

			switch ( info.ButtonID )
			{
				case 0 :
						return; // This fixes a crash when staff deletes the pawn being promoted

				case 1 : type = PawnPromotion.Queen;
					break;

				case 2 : type = PawnPromotion.Rook;
					break;

				case 3 : type = PawnPromotion.Knight;
					break;

				case 4 : type = PawnPromotion.Bishop;
					break;

				case 5: type = PawnPromotion.None;
					break;
			}

			m_Game.OnPawnPromoted( type );
		}
	}
}
