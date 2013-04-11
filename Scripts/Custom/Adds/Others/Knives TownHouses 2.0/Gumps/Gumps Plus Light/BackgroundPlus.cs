using Server.Gumps;

namespace Knives.TownHouses
{
	public class BackgroundPlus : GumpBackground
	{
	    public bool Override { get; set; }

	    public BackgroundPlus( int x, int y, int width, int height, int back ) : base( x, y, width, height, back )
		{
			Override = true;
		}

		public BackgroundPlus( int x, int y, int width, int height, int back, bool over ) : base( x, y, width, height, back )
		{
			Override = over;
		}
	}
}