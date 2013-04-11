using System;

namespace Server.Gumps
{
	public class SaveGump : Gump
	{
		public SaveGump() : base( 50, 50 )
		{
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			AddPage(0);
			AddBackground(5, 5, 415, 100, 9270);
			AddLabel(165, 30, 2062, String.Format( "{0}", Server.Misc.ServerList.ServerName ) );
			AddLabel(105, 55, 1165, @"The world is saving...   Please be patient.");
			AddImage(25, 25, 5608);
			AddItem(360, 50, 6168);
		}
	}
}