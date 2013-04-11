using System;
using Server.Accounting;
using Server.Jailing;
using Server.Network;

namespace Server.Gumps
{
	public class JailWarningGump : Gump
	{
		public JailWarningGump( Mobile from, Mobile m, string why ) : base( 70, 40 )
		{
			//from.CloseGump(typeof ( JailWarningGump ));
			( (Account)m.Account ).Comments.Add( new AccountComment( JailSystem.JSName + "-warning", m.Name + " warned for \"" + why + "\" by:" + from.Name + " on:" + DateTime.Now ) );
			Closable = false;
			Dragable = false;
			Resizable = false;
			AddPage( 0 );
			if( ( why == null ) || ( why == "" ) )
				why = "Your actions are a violation of the shard rules";
			AddBackground( 0, 0, 500, 400, 5054 );
			//ok button
			AddLabel( 26, 30, 200, from.Name + " has issued a you a warning" );
			AddLabel( 26, 50, 200, why );
			AddLabel( 26, 100, 200, "Click ok to dismiss this window" );
			AddButton( 30 + (  new Random().Next( 100 ) ), 142 + ( new Random().Next( 100 ) ), 2128, 2130, 1, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			switch( info.ButtonID )
			{
				case 1:
					from.CloseGump( typeof( JailWarningGump ) );
					break;
				default:
					break;
			}
		}
	}
}