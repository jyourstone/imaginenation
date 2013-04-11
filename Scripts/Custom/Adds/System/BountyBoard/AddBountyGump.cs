using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;

namespace Server.BountySystem
{
	public class AddBountyGump : Gump
	{
		private Mobile m_Killer;
		private Mobile m_Victim;

		public AddBountyGump( Mobile victim, Mobile killer ) : base( 100, 100 )
		{
			m_Killer = killer;
			m_Victim = victim;
			BuildGump();
		}

		private void BuildGump() 
		{
			Resizable = false;
			
			AddPage( 0 );

			AddBackground( 0, 0, 400, 250, 2600 );

			AddHtml( 0, 20, 400, 35, "<center>Place Bounty</center>", false, false );

			string text = String.Format( "Would you like to place a bounty on {0}'s head?", m_Killer.Name );
			AddHtml( 50, 55, 300, 50, text, true, true );

			AddHtml( 50, 120, 40, 35, "Amount", false, false );
			AddTextEntry( 100, 120, 120, 20, 0x480, 0, String.Format( "{0}", BountyBoardEntry.DefaultMinBounty ) ); 

			AddButton( 200, 175, 4005, 4007, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 235, 175, 110, 35, 1046363, false, false ); // CANCEL

			AddButton( 65, 175, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 100, 175, 110, 35, 1046362, false, false ); // CONTINUE
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			switch ( info.ButtonID )
			{
				case 1: 
				{    
					try
					{
						TextRelay te = info.GetTextEntry( 0 );

						if( te != null )
						{
							int price = Convert.ToInt32( te.Text, 10 );

							if( price < BountyBoardEntry.DefaultMinBounty )
							{
								from.SendMessage( "The bounty must be at least {0} gold.", BountyBoardEntry.DefaultMinBounty );
								from.SendGump( new AddBountyGump( from, m_Killer ) );
								return;
							}

							if( m_Killer != null && !m_Killer.Deleted )
							{
								//remove bounty gold
							
								if ( !Banker.Withdraw( from, price ) )
								{
									from.SendMessage( "You cannot afford a bounty of {0}!", price );
									from.SendGump( new AddBountyGump( from, m_Killer ), false );
									return;
								}

								BountyBoard.AddEntry( from, m_Killer, price, DateTime.Now + BountyBoardEntry.DefaultDecayRate );
								m_Killer.SendMessage( "A bounty hath been issued for thee!" );
                                from.SendAsciiMessage("You placed a bounty on {0} for {1} gold", m_Killer.Name, price);
							}
						}
					}
					catch
					{
						from.SendMessage( "Bad format. #### expected." );
						from.SendGump( new AddBountyGump( from, m_Killer ) );
					}
					break; 
				}
				case 2: 
				{
					from.SendLocalizedMessage( 500518 );
					break; 
				}
			}
		}

		public string Color( string text, int color )
		{
			return String.Format( "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text );
		}
	}
}
