//Author plus
using Server.Items;

namespace Server.Mobiles
{ 
	public class KillBookSystem
	{		
		public static void Initialize()
		{ 
			EventSink.PlayerDeath += EventSink_PlayerDeath; 	
		}		

		public static void EventSink_PlayerDeath(PlayerDeathEventArgs e)
		{   
			Killed(e.Mobile);
		}
           	
		public static void Killed( Mobile m ) //done
		{
		
			PlayerMobile owner = m as PlayerMobile; 
			
			Mobile m_Killer = m.LastKiller;
			

			if ( m_Killer != null && m_Killer.Player && owner != null && owner.Player )
			{
			    KillBook book = null;
 				
                if (m_Killer.Backpack != null)
                    book = m_Killer.Backpack.FindItemByType( typeof( KillBook ), true ) as KillBook;
				
				if( book != null )
					{
						if( ( owner != book.BookOwner ) && ( m_Killer == book.BookOwner ) )
						{					
							book.AddEntry(owner.Name, 1);
							book.TotKills++;
						}
					}
			}
			
			if( owner != null && owner.Player && m_Killer != null && m_Killer.Player )
			{
				KillBook deathbook = owner.Backpack.FindItemByType( typeof( KillBook ), true) as KillBook;
				
				if ( deathbook != null )
				{
					if ( owner == deathbook.BookOwner )
					{
						if( deathbook.TotDeaths >= 0 )
							deathbook.TotDeaths++;
					}
				}
			}
		}
	} 
} 
