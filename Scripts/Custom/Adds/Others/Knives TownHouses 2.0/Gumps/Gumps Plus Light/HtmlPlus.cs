using Server.Gumps;

namespace Knives.TownHouses
{
	public class HtmlPlus : GumpHtml
	{
	    public bool Override { get; set; }

	    public HtmlPlus( int x, int y, int width, int height, string text, bool back, bool scroll ) : base( x, y, width, height, text, back, scroll )
		{
			Override = true;
		}

		public HtmlPlus( int x, int y, int width, int height, string text, bool back, bool scroll, bool over ) : base( x, y, width, height, text, back, scroll )
		{
			Override = over;
		}
	}
}