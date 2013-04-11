using System;
using Server.Gumps;
using Server.Misc;
using Server.Network;
using Server.Regions;

namespace Server.Menus.Questions
{
	public class StuckMenuEntry
	{
		private readonly int m_Name;
		private readonly Point3D[] m_Locations;

		public int Name{ get{ return m_Name; } }
		public Point3D[] Locations{ get{ return m_Locations; } }

		public StuckMenuEntry( int name, Point3D[] locations )
		{
			m_Name = name;
			m_Locations = locations;
		}
	}

	public class StuckMenu : Gump
	{
		private static readonly StuckMenuEntry[] m_Entries = new StuckMenuEntry[]
			{
				// Felucca/Trammel Spawn Places
				new StuckMenuEntry( 1011028, new Point3D[]
					{
                        new Point3D( 3031, 3392, 15 ),
						new Point3D( 2958, 3447, 15 ),
						new Point3D( 3651, 2519, 0 ),
						new Point3D( 3650, 2619, 0 ),
						new Point3D( 3688, 2572, 7 ),
						new Point3D( 2892, 3476,  15 ),
						new Point3D( 1522, 1757, 28 ),
						new Point3D( 1519, 1619, 10 ),
						new Point3D( 1457, 1538, 30 ),
						new Point3D( 1607, 1568, 20 ),
						new Point3D( 1643, 1680, 18 ),
						//new Point3D( 2005, 2754, 30 ), Trinsic inside DOOM HQ
						new Point3D( 1993, 2827,  0 ),
						new Point3D( 2044, 2883,  0 ),
						new Point3D( 1876, 2859, 20 ),
						new Point3D( 1865, 2687,  0 ),
						new Point3D( 2973, 891, 0 ),
						new Point3D( 3003, 776, 0 ),
						new Point3D( 2910, 727, 0 ),
						new Point3D( 2865, 804, 0 ),
						new Point3D( 2832, 927, 0 ),
						new Point3D( 2498, 392,  0 ),
						new Point3D( 2433, 541,  0 ),
						new Point3D( 2445, 501, 15 ),
						new Point3D( 2501, 469, 15 ),
						new Point3D( 2444, 420, 15 ),
						new Point3D( 490, 1166, 0 ),
						new Point3D( 652, 1098, 0 ),
						new Point3D( 650, 1013, 0 ),
						new Point3D( 536,  979, 0 ),
						new Point3D( 464,  970, 0 ),
						new Point3D( 2230, 1159, 0 ),
						new Point3D( 2218, 1203, 0 ),
						new Point3D( 2247, 1194, 0 ),
						new Point3D( 2236, 1224, 0 ),
						new Point3D( 2273, 1231, 0 ),
                        new Point3D( 2498, 392,  0 )
					} )
			};

        private static readonly StuckMenuEntry[] m_UnguardedEntries = new StuckMenuEntry[]
			{
				// Felucca/Trammel Spawn Places
				new StuckMenuEntry( 1011028, new Point3D[]
					{
                        new Point3D( 3031, 3392, 15 ),
						new Point3D( 2958, 3447, 15 ),
						new Point3D( 3651, 2519, 0 ),
						new Point3D( 3650, 2619, 0 ),
						new Point3D( 3688, 2572, 7 ),
						new Point3D( 2892, 3476,  15 ),
						new Point3D( 1522, 1757, 28 ),
						new Point3D( 1607, 1568, 20 ),
						new Point3D( 1643, 1680, 18 ),
					} )
			};

		private static readonly StuckMenuEntry[] m_T2AEntries = new StuckMenuEntry[]
			{
				// T2A Spawn Places
				new StuckMenuEntry( 1011057, new Point3D[]
					{
                        new Point3D( 5228, 3978, 37 )
						/*new Point3D( 5720, 3109, -1 ),
						new Point3D( 5677, 3176, -3 ),
						new Point3D( 5678, 3227,  0 ),
						new Point3D( 5769, 3206, -2 ),
						new Point3D( 5777, 3270, -1 ),
						new Point3D( 5216, 4033, 37 ),
						new Point3D( 5262, 4049, 37 ),
						new Point3D( 5284, 4006, 37 ),
						new Point3D( 5189, 3971, 39 ),
						new Point3D( 5243, 3960, 37 )*/
					} )
			};

		private static bool IsInSecondAgeArea( Mobile m )
		{
			if ( m.Map != Map.Trammel && m.Map != Map.Felucca )
				return false;

			if ( m.X >= 5120 && m.Y >= 2304 )
				return true;

			if ( m.Region.Name == "Terathan Keep" )
				return true;

			return false;
		}

		private readonly Mobile m_Mobile;
	    private readonly Mobile m_Sender;
	    private readonly bool m_MarkUse;

		private Timer m_Timer;

		public StuckMenu( Mobile beholder, Mobile beheld, bool markUse ) : base( 150, 50 )
		{
			m_Sender = beholder;
			m_Mobile = beheld;
			m_MarkUse = markUse;

			Closable = false; 
			Dragable = false; 
			Disposable = false;

			AddBackground( 0, 0, 300, 175, 2600 );

			AddLabel(50, 20, 0, @"Are you sure that you are");
			AddLabel(50, 40, 0, @"physically stuck and you can't");
			AddLabel(50, 60, 0, @"continue playing normally?");

			StuckMenuEntry[] entries;
            
            if (IsInSecondAgeArea( beheld ))
                entries = m_T2AEntries;
            else
            {
                entries = NotorietyHandlers.IsGuardCandidate(beheld) ? m_UnguardedEntries : m_Entries;
            }

			AddButton( 55, 120, 4005, 4007, 0, GumpButtonType.Reply, 0 );
			AddLabel(90, 120, 0, @"No");
			AddLabel(200, 120, 0, @"Yes");

			for ( int i = 0; i < entries.Length; i++ )
			{
				StuckMenuEntry entry = entries[i];

				AddButton( 165, 120 + 35 * i, 4005, 4007, i + 1, GumpButtonType.Reply, 0 );
			}
		}

		public void BeginClose()
		{
			StopClose();

			m_Timer = new CloseTimer( m_Mobile );
			m_Timer.Start();

			m_Mobile.Frozen = true;
		}

		public void StopClose()
		{
			if ( m_Timer != null )
				m_Timer.Stop();

			m_Mobile.Frozen = false;
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			StopClose();

			if ( Factions.Sigil.ExistsOn( m_Mobile ) )
			{
				m_Mobile.SendLocalizedMessage( 1061632 ); // You can't do that while carrying the sigil.
			}
			else if ( info.ButtonID == 0 )
			{
				if ( m_Mobile == m_Sender )
					m_Mobile.SendLocalizedMessage( 1010588 ); // You choose not to go to any city.
			}
			else
			{
				int index = info.ButtonID - 1;
                StuckMenuEntry[] entries;

                if (IsInSecondAgeArea(m_Mobile))
                    entries = m_T2AEntries;
                else
                {
                    if (m_Mobile.Kills >= NotorietyHandlers.KILLS_FOR_MURDER || m_Mobile.Criminal || m_Mobile.Karma <= NotorietyHandlers.PLAYER_KARMA_RED)
                        entries = m_UnguardedEntries;
                    else
                        entries = m_Entries;
                }

				if ( index >= 0 && index < entries.Length )
					Teleport( entries[index] );
			}
		}

		private void Teleport( StuckMenuEntry entry )
		{
			if ( m_MarkUse ) 
			{
				m_Mobile.SendLocalizedMessage( 1010589 ); // You will be teleported within the next two minutes.

				new TeleportTimer( m_Mobile, entry, TimeSpan.FromSeconds( 10.0 + (Utility.RandomDouble() * 110.0) ) ).Start();

				m_Mobile.UsedStuckMenu();
			}
			else
			{
				new TeleportTimer( m_Mobile, entry, TimeSpan.Zero ).Start();
			}
		}

		private class CloseTimer : Timer
		{
			private readonly Mobile m_Mobile;
			private readonly DateTime m_End;

			public CloseTimer( Mobile m ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Mobile = m;
				m_End = DateTime.Now + TimeSpan.FromMinutes( 3.0 );
			}

			protected override void OnTick()
			{
				if ( m_Mobile.NetState == null || DateTime.Now > m_End )
				{
					m_Mobile.Frozen = false;
					m_Mobile.CloseGump( typeof( StuckMenu ) );

					Stop();
				}
				else
				{
					m_Mobile.Frozen = true;
				}
			} 
		} 

		private class TeleportTimer : Timer
		{
			private readonly Mobile m_Mobile;
			private readonly StuckMenuEntry m_Destination;
			private readonly DateTime m_End;

			public TeleportTimer( Mobile mobile, StuckMenuEntry destination, TimeSpan delay ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 1.0 ) )
			{
				Priority = TimerPriority.TwoFiftyMS;

				m_Mobile = mobile;
				m_Destination = destination;
				m_End = DateTime.Now + delay;
			}

			protected override void OnTick()
			{
				if ( DateTime.Now < m_End )
				{
					m_Mobile.Frozen = true;
				}
				else
				{
					m_Mobile.Frozen = false;
					Stop();

					if ( Factions.Sigil.ExistsOn( m_Mobile ) )
					{
						m_Mobile.SendLocalizedMessage( 1061632 ); // You can't do that while carrying the sigil.
						return;
					}

					int idx = Utility.Random( m_Destination.Locations.Length );
					Point3D dest = m_Destination.Locations[idx];

				    Region r = Region.Find(dest, Map.Felucca);
				    int i = 0;

                    while (r is HouseRegion && i < m_Destination.Locations.Length)
                    {
                        idx = Utility.Random(m_Destination.Locations.Length);
                        dest = m_Destination.Locations[idx];
                        i++;
                    }

				    Map destMap = Map.Felucca;

					Mobiles.BaseCreature.TeleportPets( m_Mobile, dest, destMap );
					m_Mobile.MoveToWorld( dest, destMap );

                    if (m_Mobile.IsInEvent)
                    {
                        m_Mobile.IsInEvent = false;
                        m_Mobile.SendAsciiMessage("You auto supply has been removed.");

                        SupplySystem.RemoveEventGear(m_Mobile);
                    }
				}
			}
		}
	}
}