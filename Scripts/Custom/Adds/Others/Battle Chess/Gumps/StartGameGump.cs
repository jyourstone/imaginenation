using Server;
using Server.Gumps;

namespace Arya.Chess
{
	/// <summary>
	/// This gump is used to start the game
	/// </summary>
	public class StartGameGump : Gump
	{
		private const int LabelHue = 0x480;
		private const int GreenHue = 0x40;

		private readonly ChessGame m_Game;
		private readonly Mobile m_User;
		private readonly bool m_IsOwner;
		private readonly bool m_AllowSpectators;

		public StartGameGump( Mobile m, ChessGame game, bool isOwner, bool allowSpectators ) : base( 200, 200 )
		{
			m_Game = game;
			m_User = m;
			m_IsOwner = isOwner;
			m_AllowSpectators = allowSpectators;

			m_User.CloseGump( typeof( StartGameGump ) );

			MakeGump();
		}

		private void MakeGump()
		{
			Closable=false;
			Disposable=true;
			Dragable=true;
			Resizable=false;

			AddPage(0);
			int height = 75;

			if ( m_IsOwner )
				height = 110;

			AddBackground(0, 0, 300, height, 9250);
			AddImageTiled(0, 0, 300, height, 9304);
			AddImageTiled(1, 1, 298, height - 2, 9274);
			AddAlphaRegion(1, 1, 298, height - 2);

			if ( m_IsOwner )
			{
				if ( m_Game.Guest == null )
					AddLabel(10, 5, GreenHue, @"Starting new chess game");
				else
					AddLabel(10, 5, GreenHue, @"Waiting for partner to accept");

				AddImageTiled(10, 25, 280, 1, 9304);

				// Bring again target : 1
				if ( m_Game.Guest == null )
				{
					AddButton(15, 30, 5601, 5605, 1, GumpButtonType.Reply, 0);
					AddLabel(35, 28, LabelHue, @"Please select your opponent...");
				}

				// Cancel : 0
				AddButton(15, 50, 5601, 5605, 2, GumpButtonType.Reply, 0);
				AddLabel(35, 48, LabelHue, @"Cancel");

				int bid = m_AllowSpectators ? 2153 : 2151;
				AddButton( 10, 75, bid, bid, 3, GumpButtonType.Reply, 0 );
				AddLabel( 45, 80, LabelHue, "Allow spectators on the Chessboard" );
			}
			else
			{
				AddLabel(10, 5, GreenHue, string.Format( "Play chess with {0}?", m_Game.Owner.Name ) );
				AddImageTiled(10, 25, 280, 1, 9304);

				// Accept : 1
				AddButton(15, 30, 5601, 5605, 1, GumpButtonType.Reply, 0);
				AddLabel(35, 28, LabelHue, @"Accept");

				// Refuse : 0
				AddButton(15, 50, 5601, 5605, 2, GumpButtonType.Reply, 0);
				AddLabel(35, 48, LabelHue, @"Refuse");
			}
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			if ( m_IsOwner )
			{
				if ( info.ButtonID == 3 )
				{
					// Switch the allow spectators flag
					m_Game.AllowSpectators = !m_AllowSpectators;
					sender.Mobile.SendGump( new StartGameGump( sender.Mobile, m_Game, m_IsOwner, !m_AllowSpectators ) );
				}
				else if ( info.ButtonID == 2 )
				{
					m_Game.CancelGameStart( sender.Mobile );
				}
				else if ( info.ButtonID == 1 )
				{
					sender.Mobile.Target = new ChessTarget( m_Game, sender.Mobile, "Please select your partner...",
						m_Game.ChooseOpponent );
					
					sender.Mobile.SendGump( new StartGameGump( sender.Mobile, m_Game, m_IsOwner, m_AllowSpectators ) );
				}
			}
			else
			{
				if ( info.ButtonID == 2 )
				{
					m_Game.CancelGameStart( sender.Mobile );
				}
				else if ( info.ButtonID == 1 )
				{
					m_Game.AcceptGame( sender.Mobile );
				}
			}
		}
	}
}