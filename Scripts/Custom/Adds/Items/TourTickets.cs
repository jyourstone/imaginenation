using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Misc
{
	public class TourTicket : Item
	{
		private TourTicket ttt;

		[Constructable]
		public TourTicket() : base( 0x14F0 )
		{
			Weight = 1.0;
			Hue = 1109;
			Name = "Tourney ticket";
		}

		public TourTicket( Serial serial ) : base( serial )
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
			if( !IsChildOf( from.Backpack ) && !IsChildOf( from.BankBox ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else
				from.SendGump( new SelectTicketGump( from, ttt ) );
		}
	}

	public class SelectTicketGump : Gump
	{
		private readonly Mobile m_From;
		private readonly TourTicket rttt;

		public SelectTicketGump( Mobile from, TourTicket ttt ) : base( 50, 50 )
		{
			m_From = from;
			rttt = ttt;
			from.CloseGump( typeof( SelectTicketGump ) );

			AddBackground( 100, 10, 230, 180, 9390 );
			AddHtml( 157, 18, 400, 35, "Tour ticket selection", false, false );
			
			AddHtml( 126, 55, 200, 35, "This ticket can be used at a PVP Reward Stone", false, false );
			//AddHtml( 150, 75, 200, 35, "Convert into 80k gold", false, false );
			AddHtml( 150, 105, 170, 35, "Convert a Tourney ticket into 12 Team tour tickets", false, false );

			//AddButton( 118, 73, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 ); //Turn to gold:
			AddButton( 118, 111, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 ); //Get 12 team tourney ticket:
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if( rttt == null )
				return;

			Container bag = new Backpack();
			bag.Hue = 1175;
			switch( info.ButtonID )
			{
				case 0:
					return;
/*				case 1:
					m_From.BankBox.DropItem( new BankCheck( 80000 ) );
                    m_From.SendMessage("You convert the tour tourney ticket into a bankcheck worth 80000gp which has been placed in your bankbox"
);
					rttt.Delete();
					;
					break;*/
					   
				case 1:
					if( m_From.Backpack != null && m_From.Backpack.ConsumeTotal( typeof( TourTicket ), 1 ) )
					{
						m_From.BankBox.DropItem( bag );
						for( int i = 1; i <= 12; i++ )
							bag.DropItem( new TeamTourTicket() );
						m_From.SendMessage( "Your team tourney ticket has been placed in a container in your bankbox." );
					}
					else if( m_From.BankBox != null && m_From.BankBox.ConsumeTotal( typeof( TourTicket ), 1 ) )
					{
						m_From.BankBox.DropItem( bag );
						for( int i = 1; i <= 12; i++ )
							bag.DropItem( new TeamTourTicket() );
						m_From.SendMessage( "Your team tourney ticket has been placed in a container in your bankbox." );
					}
					else
						m_From.SendMessage( "You do not any any tourney tickets on you!" );
					;
					break;
			}
		}
	}
}