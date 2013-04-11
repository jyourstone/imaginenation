using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class WebStone : Item
	{
		[Constructable]
		public WebStone() : base( 0xED4 )
		{
			Name = "Web Stone";
			Movable = false;
			Hue = 2558;
		}

		public WebStone( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( from.InRange( this, 4 ) )
			{
				from.SendGump( new webstonegump() );
				from.PlaySound(47);
			}
			else
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
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
	}


	public class webstonegump : Gump
	{
        private Mobile m_From;

        public enum Buttons
        {
            IN = 1,
            INForums = 2,
        }

		public webstonegump() : base( 0, 0 )
		{
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			AddPage(0);
			AddImageTiled(109, 134, 219, 116, 2604);
			AddImageTiled(109, 107, 219, 27, 2601);
			AddImageTiled(109, 249, 219, 44, 2607);
			AddImageTiled(65, 107, 44, 46, 2600);
			AddImageTiled(328, 107, 44, 46, 2602);
			AddImageTiled(328, 250, 44, 43, 2608);
			AddImageTiled(65, 250, 44, 43, 2606);
			AddImageTiled(65, 151, 44, 100, 2603);
			AddImageTiled(328, 151, 44, 100, 2605);
			AddLabel(163, 125, 2960, @"IN WEB STONE");
			AddButton(109, 170, 1209, 1210, (int)Buttons.IN, GumpButtonType.Reply, 0);
			AddButton(109, 216, 1209, 1210, (int)Buttons.INForums, GumpButtonType.Reply, 0);
			AddLabel(134, 167, 1174, @"IN News");
			AddLabel(134, 213, 1174, @"IN Forums");
			AddImage(340, 53, 10410);
			AddImage(340, 203, 10411);
			AddImage(322, 335, 10402);

		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{	
			m_From = sender.Mobile;
			
			string page = "You open the homepage.";

			switch( info.ButtonID )
			{
				case 0:
				{
					m_From.PlaySound( 46 );
					break;
				}	
				
				case 1:
				{
						m_From.PlaySound( 579 );
                        m_From.LaunchBrowser(string.Format("http://in-uo.net/"));
                        m_From.SendAsciiMessage( page );

					break;
				}
				
				case 2:
				{
						m_From.PlaySound( 579 );
                        m_From.LaunchBrowser(string.Format("http://in-uo.net/forums/index.php"));
                        m_From.SendAsciiMessage( page );

					break;
				}
			}
		}
	}
}