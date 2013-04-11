using System;
using Server.Accounting;
using Server.Jailing;
using Server.Network;

namespace Server.Gumps
{
	public class UnattendedMacroGump : Gump
	{
		private readonly Mobile badBoy;
		private readonly Mobile jailor;
		private readonly DateTime issued = DateTime.Now;
		private readonly UAResponseTimer myTimer;
		private readonly int tbutton = 2;
		private bool caughtFired = false;

		public UnattendedMacroGump( Mobile from, Mobile m ) : base( 70, 40 )
		{
			tbutton = ( new Random() ).Next( 6 );
			if( tbutton < 1 )
				tbutton = 1;
			if( tbutton > 6 )
				tbutton = 6;
			( (Account)m.Account ).Comments.Add( new AccountComment( JailSystem.JSName + "-warning", from.Name + " checked to see if " + m.Name + " was macroing unattended on: " + DateTime.Now ) );
			jailor = from;
			badBoy = m;
			Closable = false;
			Dragable = true;
			AddPage( 0 );
			AddBackground( 0, 0, 326, 320, 5054 );
			AddImageTiled( 9, 65, 308, 240, 2624 );
			AddAlphaRegion( 9, 65, 308, 240 );
			//AddLabel( 16, 20, 200, string.Format("{0} is checking to see if you are macroing unattended", jailor.Name));
			AddHtml( 16, 10, 250, 50, string.Format( "{0} is checking to see if you are macroing unattended", jailor.Name ), false, false );
			//let them show that they are there by selecting these buttons
			AddButton( 20, 72, 2472, 2473, 5, GumpButtonType.Reply, 0 );
			AddLabel( 50, 75, 200, tbutton == 5 ? "I'm here!" : "I confess I was macroing unattended." );
			AddButton( 20, 112, 2472, 2473, 1, GumpButtonType.Reply, 0 );
			AddLabel( 50, 115, 200, tbutton == 1 ? "I'm here!" : "I confess I was macroing unattended." );
			AddButton( 20, 152, 2472, 2473, 2, GumpButtonType.Reply, 0 );
			AddLabel( 50, 155, 200, tbutton == 2 ? "I'm here!" : "I confess I was macroing unattended." );
			AddButton( 20, 192, 2472, 2473, 3, GumpButtonType.Reply, 0 );
			AddLabel( 50, 195, 200, tbutton == 3 ? "I'm here!" : "I confess I was macroing unattended." );
			AddButton( 20, 232, 2472, 2473, 4, GumpButtonType.Reply, 0 );
			AddLabel( 50, 235, 200, tbutton == 4 ? "I'm here!" : "I confess I was macroing unattended." );
			AddButton( 20, 272, 2472, 2473, 6, GumpButtonType.Reply, 0 );
			AddLabel( 50, 275, 200, tbutton == 6 ? "I'm here!" : "I confess I was macroing unattended." );
			myTimer = new UAResponseTimer( this );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			if( myTimer != null )
				myTimer.Stop();
			if( tbutton == info.ButtonID )
			{
				string mtemp = string.Format( "{0} responded to the unattended macroing check in {1} seconds.", from.Name, DateTime.Now.Subtract( issued ).Seconds );
				( (Account)badBoy.Account ).Comments.Add( new AccountComment( JailSystem.JSName + "-warning", mtemp ) );
				jailor.SendMessage( mtemp );
			}
			else
				caughtInTheAct( false );
			from.CloseGump( typeof( UnattendedMacroGump ) );
		}

		public void caughtInTheAct( bool confessed )
		{
			if( caughtFired )
				return;
			caughtFired = true;
			if( !confessed )
			{
				JailSystem.Jail( badBoy, 1, 0, 0, JailGump.reasons[0], true, jailor.Name );
				jailor.SendMessage( "{0} has been jailed for {1} from the warning you issued.", badBoy.Name, JailGump.reasons[0] );
			}
			else
			{
				JailSystem.Jail( badBoy, 0, 5, 0, JailGump.reasons[0], true, jailor.Name );
				jailor.SendMessage( "{0} was been jailed for {1} when they confessed on the warning you issued.", badBoy.Name, JailGump.reasons[0] );
			}
			if( myTimer != null )
				myTimer.Stop();
		}

		public class UAResponseTimer : Timer
		{
			public UnattendedMacroGump m_gump;
			private int counts = 60;

			public UAResponseTimer( UnattendedMacroGump myGump ) : base( TimeSpan.FromSeconds( 10 ), TimeSpan.FromSeconds( 10 ) )
			{
				m_gump = myGump;
				Start();
			}

			protected override void OnTick()
			{
				counts -= Interval.Seconds;
				switch( counts )
				{
					case 50:
					case 40:
					case 30:
					case 20:
						Interval = TimeSpan.FromSeconds( 1 );
						goto case 10;
					case 10:
					case 9:
					case 8:
					case 7:
					case 6:
					case 5:
					case 4:
					case 3:
					case 2:
					case 1:
						m_gump.badBoy.SendMessage( "Warning closing in {0} seconds", counts );
						break;
					case 0:
						m_gump.caughtInTheAct( false );
						m_gump.badBoy.CloseGump( typeof( UnattendedMacroGump ) );
						Stop();
						break;
					default:
						break;
				}
			}
		}
	}
}