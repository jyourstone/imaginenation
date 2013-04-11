using System.Collections;
using Server.Items;
using Server.Network;

namespace Server.Gumps
{
	public class KillSearch : Gump
	{
		private readonly string m_SearchString;
		
		private readonly ArrayList m_KillList;
		
		private readonly KillBook m_Book;
		
		public KillSearch( string searchString, KillBook book, ArrayList list ) : base( 100, 100 )
		{
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			
			m_SearchString = searchString;
			m_KillList = list;
			m_Book = book;
			
			AddPage( 0 );
			
			AddImage(3, 18, 2200); //Book
			AddImage(63, 89, 9804); //Skull
			AddLabel(70, 132, 136, "Kill Book");
			
			//Search
			AddLabel(35, 170, 0, "Search Name");
			AddTextEntry(33, 193, 125, 20, 0, 0, "Name");
			AddButton(122, 173, 5601, 5605, 2, GumpButtonType.Reply, 0);
			
			AddLabel(204, 32, 0, "Back");
			AddButton(271, 34, 5601, 5605, 1, GumpButtonType.Reply, 0);
			
			//Seperators
			for (int s = 0; s < 5; s++)
			{
				AddImage ( 216 + (s * 16), 53, 58);
				AddImage(186, 53, 57);
				AddImage(296, 53, 59);
			}
			
			AddPage( 1 );
			
			for (int i = 0; i < m_KillList.Count; i++)
			{
				foreach (DeathEntry o in m_KillList)
		
				if( o.Name == m_SearchString)
	 			{
					AddHtml( 204, 75, 100, 17, m_SearchString, false, false);
	 				AddHtml( 294, 75,  60, 17, o.Deaths.ToString(), false, false );
	 			}
	 			/*else
	 			{
	 				AddLabel( 204, 75, 0x25, "No entries found");
	 			}*/
			}
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

			switch ( info.ButtonID )
			{
				case 1:
				{
					from.CloseGump( typeof( KillSearch ) );
					from.SendGump( new KillIndex( from, m_Book, m_KillList ) );
					break;
				}
				case 2:
				{
					TextRelay s = info.GetTextEntry( 0 );
					
					if ( s != null )
					{
						string search = s.Text;
						from.SendGump( new KillSearch( search, m_Book, m_KillList ));
					}
					break;
				}
			}
		}
	}
}
