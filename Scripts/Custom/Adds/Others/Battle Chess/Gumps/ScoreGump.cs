using Server;
using Server.Gumps;

namespace Arya.Chess
{
	/// <summary>
	/// Summary description for ScoreGump.
	/// </summary>
	public class ScoreGump : Gump
	{
		private ChessGame m_Game;
		private const int LabelHue = 0x480;
		private const int GreenHue = 0x40;

		public ScoreGump( Mobile m, ChessGame game, int[] white, int[] black, int totwhite, int totblack ) : base( 0, 25 )
		{
			m.CloseGump( typeof( ScoreGump ) );
			m_Game = game;

			MakeGump( white, black, totwhite, totblack );
		}

		private void MakeGump( int[] white, int[] black, int whitescore, int blackscore )
		{
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			AddPage(0);
			AddBackground(0, 0, 60, 345, 9350);
			AddAlphaRegion(0, 0, 60, 345);
			AddImage(5, 5, 2336);
			AddImage(30, 6, 2343);
			AddImage(5, 50, 2335);
			AddImage(30, 50, 2342);
			AddImage(5, 105, 2332);
			AddImage(30, 105, 2339);
			AddImage(5, 160, 2333);
			AddImage(30, 160, 2340);
			AddImage(5, 220, 2337);
			AddImage(30, 220, 2344);
			AddImageTiled(0, 285, 60, 1, 5124);
			AddImage(10, 305, 2331);
			AddImage(10, 325, 2338);

			AddLabel(11, 33, LabelHue, white[0].ToString() );
			AddLabel(5, 285, GreenHue, @"Score");
			AddLabel(36, 33, LabelHue, black[0].ToString() );
			AddLabel(11, 87, LabelHue, white[1].ToString() );
			AddLabel(36, 87, LabelHue, black[1].ToString() );
			AddLabel(11, 142, LabelHue, white[2].ToString() );
			AddLabel(36, 142, LabelHue, black[2].ToString() );
			AddLabel(11, 200, LabelHue, white[3].ToString() );
			AddLabel(36, 200, LabelHue, black[3].ToString() );
			AddLabel(11, 260, LabelHue, white[4].ToString() );
			AddLabel(36, 260, LabelHue, black[4].ToString() );
			AddLabel(30, 302, LabelHue, whitescore.ToString() );
			AddLabel(30, 322, LabelHue, blackscore.ToString() );
		}
	}
}
