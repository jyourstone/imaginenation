using Server.Gumps;
using Server.Network;

namespace Server.Misc
{
	public class TeamTourTicket : Item
	{
		private TeamTourTicket ttt;
		public bool fromBackpack = true;

		[Constructable]
		public TeamTourTicket() : base( 0x14F0 )
		{
			Weight = 1.0;
			Hue = Utility.RandomList( Sphere.RareHues );
			Name = "Team tourney ticket";
		}

		public TeamTourTicket( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			ttt = this;
			if( IsChildOf( from.Backpack ) )
			{
				fromBackpack = true;
				from.SendGump( new SelectGump( from, ttt ) );
			}
			else if( IsChildOf( from.BankBox ) )
			{
				fromBackpack = false;
				from.SendGump( new SelectGump( from, ttt ) );
			}
			else
				from.SendMessage( "This item has to either be in your backpack or bank" );
		}
	}

	public class SelectGump : Gump
	{
		private readonly Mobile m_From;
		private readonly TeamTourTicket rttt;

		public SelectGump( Mobile from, TeamTourTicket ttt ) : base( 50, 50 )
		{
			m_From = from;
			rttt = ttt;
			from.CloseGump( typeof( SelectGump ) );

			AddBackground( 100, 10, 230, 180, 9390 );
			AddHtml( 140, 18, 400, 35, "Team tour ticket selection", false, false );

			AddHtml( 126, 55, 200, 35, "This ticket can be used at a PVP Reward Stone", false, false );
			//AddHtml( 150, 75, 200, 35, "Convert into 10k gold", false, false );
			AddHtml( 150, 105, 150, 35, "Convert 12 Team tour tickets into a Tourney ticket", false, false );

			//AddButton( 118, 73, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 ); //Turn to gold:
			AddButton( 118, 111, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 ); //Get a tourney ticket:
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if( rttt == null )
				return;

			switch( info.ButtonID )
			{
				case 0:
					return;
/*				case 1:
					m_From.BankBox.DropItem( new BankCheck( 10000 ) );
                    m_From.SendMessage("You convert the team tourney ticket into a bankcheck worth 10000gp which has been placed in your bankbox"
);
					rttt.Delete();
					;
					break;*/
					
				case 1:
					if( m_From.Backpack != null && m_From.Backpack.ConsumeTotal( typeof( TeamTourTicket ), 12 ) )
					{
						m_From.BankBox.DropItem( new TourTicket() );
						m_From.SendMessage( "Your tourney ticket has been placed in your bankbox." );
					}
					else if( m_From.BankBox != null && rttt.fromBackpack && m_From.BankBox.ConsumeTotal( typeof( TeamTourTicket ), 11 ) )
					{
						m_From.BankBox.DropItem( new TourTicket() );
						m_From.SendMessage( "Your tourney ticket has been placed in your bankbox." );
						rttt.Delete();
					}
					else if( m_From.BankBox != null && rttt.fromBackpack == false && m_From.BankBox != null && m_From.BankBox.ConsumeTotal( typeof( TeamTourTicket ), 12 ) )
					{
						m_From.BankBox.DropItem( new TourTicket() );
						m_From.SendMessage( "Your tourney ticket has been placed in your bankbox." );
					}
					else
						m_From.SendMessage( "You do not have the 12 required team tourney tickets in once place." );
					;
					break;
			}
		}
	}
}