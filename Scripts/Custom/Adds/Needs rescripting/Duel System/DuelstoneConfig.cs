using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;

namespace Server.DuelSystem
{
	public enum DuelStoneEventType
	{
		Money1vs1 = 0,
		Loot1vs1,
		Money2vs2,
		Loot2vs2
	}

	public class DuelStoneConfig
	{
		public static ArrayList usingStone = new ArrayList();

		public static int duelAreaWidth = 4; //Tiles
		public static int maxDuelLenght = 30; //Minute
		public static Point3D outLocation;

		#region Event Handlers
		private static void OnPlayerDeath( PlayerDeathEventArgs e )
		{
			PlayerMobile pm = e.Mobile as PlayerMobile;

			if( pm == null || pm.DuelStone == null )
				return;

			#region DuelstoneCode
			DuelStone ds = pm.DuelStone;

			//Reset combatants
			if( pm.LastKiller != null )
			{
                //Taran: Had to add this to reward fame, since combatant is nulled below
                int fameAward = pm.Fame / 10;
                Titles.AwardFame(pm.LastKiller, fameAward, true);

				pm.LastKiller.Combatant = null;
				pm.Combatant = null;
			}

			//Update duelstone
			if( ds.IsLadderStone )
				DuelstoneLadderUpdate.StartUpdate( ds, pm );

			if( ds.StoneType.ToString().Contains( "Loot" ) )
			{
				if( pm.Corpse != null )
					((Corpse)pm.Corpse).Carve( pm, null );
			}
			else
				GiveOpponentMoney( ds, pm );

			pm.SendAsciiMessage( "You will be teleported out in " + ds.StoneMaxIdleTime + " seconds." );

			if( ds.MoveTimer == null )
				ds.MoveTimer = Timer.DelayCall( TimeSpan.FromSeconds( ds.StoneMaxIdleTime + 1 ), new TimerStateCallback( TeleOut ), ds );
			if( ds.CountTimer == null )
				ds.CountTimer = Timer.DelayCall( TimeSpan.Zero, TimeSpan.FromSeconds( 1.0 ), ds.StoneMaxIdleTime + 1, new TimerStateCallback( CountToEnd ), new object[] { ds, ds.StoneMaxIdleTime } );
			#endregion
		}
		#endregion

		public static void Initialize()
		{
			EventSink.PlayerDeath += OnPlayerDeath;
			//EventSink.Logout += new LogoutEventHandler(OnLogout);
		}

		private static void OnLogout( PlayerDeathEventArgs e )
		{
			PlayerMobile pm = e.Mobile as PlayerMobile;

			if( pm == null )
				return;

			if( pm.DuelStone != null )
				if( pm.DuelStone.Combatant1 == e.Mobile )
				{
					pm.DuelStone.Combatant1 = null;
					pm.DuelStone = null;
				}
				else if( pm.DuelStone.Combatant2 == e.Mobile )
				{
					pm.DuelStone.Combatant2 = null;
					pm.DuelStone = null;
				}
				else if( pm.DuelStone.Combatant3 == e.Mobile )
				{
					pm.DuelStone.Combatant3 = null;
					pm.DuelStone = null;
				}
				else if( pm.DuelStone.Combatant4 == e.Mobile )
				{
					pm.DuelStone.Combatant4 = null;
					pm.DuelStone = null;
				}
		}

		public static bool AffordsDuel( DuelStone ds, Mobile from, int price )
		{
			if( price == 0 )
				return true;

			if( ds.IsUsingStone( from ) )
				return true;

			if( from.Backpack.TotalGold >= price )
			{
				from.Backpack.ConsumeTotal( typeof( Gold ), price );
				from.SendAsciiMessage( price + " has been withdrawn from your account." );
				return true;
			}
			else if( ( from.BankBox.TotalGold + from.Backpack.TotalGold ) >= price )
			{
				from.BankBox.ConsumeTotal( typeof( Gold ), price - from.Backpack.TotalGold );
				from.Backpack.ConsumeTotal( typeof( Gold ), from.Backpack.TotalGold );
				from.SendAsciiMessage( price + " has been withdrawn from your account." );
				return true;
			}
			else
			{
				from.SendAsciiMessage( "You do not have enough money for this duel!" );
				return false;
			}
		}

		public static bool CanUseStone( DuelStone ds, Mobile from )
		{
			#region MutualChecks
			if( ds.DuelerLocation1 == new Point3D( 0, 0, 0 ) )
			{
				from.SendAsciiMessage( "This stone has not yet been set, page a GM." );
				return false;
			}

			if( ( ds.StoneType == DuelStoneEventType.Loot2vs2 || ds.StoneType == DuelStoneEventType.Money2vs2 ) && ds.DuelerLocation2 == new Point3D( null ) )
			{
				from.SendAsciiMessage( "This stone has not yet been set, page a GM." );
				return false;
			}

			/*if( ( ds.DuelCost == 0 ) && ( ds.StoneType == DuelStoneEventType.Money1vs1 || ds.StoneType == DuelStoneEventType.Money2vs2 ) )
			{
				from.SendAsciiMessage( "This stone has not yet been set, page a GM." );
				return false;
			}*/

			if( from.Hits != from.HitsMax && !ds.IsUsingStone( from ) )
			{
				from.SendAsciiMessage( "You do not have enough HP to join this duel." );
				return false;
			}
			#endregion

			if( ( ( ds.StoneType == DuelStoneEventType.Money1vs1 || ds.StoneType == DuelStoneEventType.Loot1vs1 ) && ( ds.Combatant1 != null && ds.Combatant2 != null ) && ( ds.Combatant1 != from && ds.Combatant2 != from ) ) )
			{
				from.SendAsciiMessage( "This stone is busy!" );
				return false;
			}

			return true;
		}

		public static void LeaveDuel( DuelStone ds, Mobile from )
		{
			if( ( ( ds.Combatant1 == from && ds.Combatant2 != null && ds.Combatant2.Alive && ds.Combatant1.Alive && ds.MoveTimer == null ) || ( ds.Combatant2 == from && ds.Combatant1 != null && ds.Combatant1.Alive && ds.Combatant2.Alive && ds.MoveTimer == null ) ) )
			{
				from.SendAsciiMessage( "Someone has to die first!" );
				from.PublicOverheadMessage( MessageType.Regular, from.SpeechHue, true, "I am trying to escape a certain death!" );
				return;
			}

			if( !from.Alive )
			{
				if( ds.Combatant1 == from && ds.Combatant2 != null )
				{
					((PlayerMobile)ds.Combatant1).DuelStone = null;
					ds.Combatant1 = null;
				}
				else if( ds.Combatant2 == from && ds.Combatant1 != null )
				{
					((PlayerMobile)ds.Combatant2).DuelStone = null;
					ds.Combatant2 = null;
				}
			}
			else if( ds.Combatant1 == from && ( ( ds.Combatant2 != null && !ds.Combatant2.Alive ) || ds.Combatant2 == null ) )
			{
				GiveGold( ds, from );
				((PlayerMobile)ds.Combatant1).DuelStone = null;
				ds.Combatant1 = null;

				if( ds.Combatant2 != null )
				{
					EndTeleOut( ds, ds.Combatant2 );
					((PlayerMobile)ds.Combatant2).DuelStone = null;
					ds.Combatant2 = null;
				}

				//End movetimer
				if( ds.MoveTimer != null )
				{
					ds.MoveTimer.Stop();
					ds.MoveTimer = null;
				}
			}
			else if( ds.Combatant2 == from && ( ( ds.Combatant1 != null && !ds.Combatant1.Alive ) || ds.Combatant1 == null ) )
			{
				GiveGold( ds, from );
				((PlayerMobile)ds.Combatant2).DuelStone = null;
				ds.Combatant2 = null;

				if( ds.Combatant1 != null )
				{
					EndTeleOut( ds, ds.Combatant1 );
					((PlayerMobile)ds.Combatant1).DuelStone = null;
					ds.Combatant1 = null;
				}

				//End movetimer
				if( ds.MoveTimer != null )
				{
					ds.MoveTimer.Stop();
					ds.MoveTimer = null;
				}
			}

			ds.EndTimer = true;

            //if(ds.Combatant1 != null && ds.Combatant1 != from)
            //    if(from.Aggressors.Contains((AggressorInfo) ds.Combatant1))

			EndTeleOut( ds, from );
		}

		public static void GiveOpponentMoney( DuelStone ds, Mobile from )
		{
			if( !from.Alive )
			{
				if( ds.Combatant1 == from && ds.Combatant2 != null )
					GiveGold( ds, ds.Combatant2 );
				else if( ds.Combatant2 == from && ds.Combatant1 != null )
					GiveGold( ds, ds.Combatant1 );
			}
			else if( ds.Combatant1 == from && ds.Combatant2 != null && !ds.Combatant2.Alive )
				GiveGold( ds, from );
			else if( ds.Combatant2 == from && ds.Combatant1 != null && !ds.Combatant1.Alive )
				GiveGold( ds, from );
		}

		public static void JoinDuel( DuelStone ds, Mobile to )
		{
			if( to != null && ds.DuelerLocation1 != new Point3D( 0, 0, 0 ) && ( ds.Combatant1 == null || ds.Combatant2 == null ) )
			{
				if( ds.CountTimer != null )
				{
					to.SendAsciiMessage( "The stone is busy!" );
					return;
				}

				if( !AffordsDuel( ds, to, ds.DuelCost ) )
					return;

				to.Location = ds.DuelerLocation1;

				if( ds.Combatant1 == null )
				{
					ds.Combatant1 = to;
					((PlayerMobile)ds.Combatant1).DuelStone = ds;
				}
				else if( ds.Combatant2 == null )
				{
					ds.Combatant2 = to;
					((PlayerMobile)ds.Combatant2).DuelStone = ds;
				}

				if( ds.Combatant1 != null && ds.Combatant2 != null )
				{
					ResetTimers( ds );

					ds.EndDuelTimer = Timer.DelayCall( TimeSpan.FromMinutes( maxDuelLenght ), TimeSpan.FromMinutes( maxDuelLenght ), 2, new TimerStateCallback( EndLongDuel ), ds );

					ds.Combatant1.SendAsciiMessage( "The duel will end in " + maxDuelLenght + " minutes if both players are still alive." );
					ds.Combatant2.SendAsciiMessage( "The duel will end in " + maxDuelLenght + " minutes if both players are still alive." );

					(ds.Combatant1).HasFilter = true;
					(ds.Combatant2).HasFilter = true;
				}
			}
		}

		public static void GiveGold( DuelStone ds, Mobile to )
		{
			if( ds.DuelCost != 0 && !ds.StoneType.ToString().Contains( "Loot" ) )
			{
				to.Backpack.AddItem( new Gold( (int)( ds.DuelCost * 0.9 ) ) );
				to.SendAsciiMessage( (int)( ds.DuelCost * 0.9 ) + " gold has been added to your backpack." );
			}
		}

		public static void EndTeleOut( DuelStone ds, Mobile toTele )
		{
			if( toTele != null )
				if( ds != null )
				{
					if( !ds.StoneType.ToString().Contains( "Loot" ) && !toTele.Alive )
					{
						if( toTele.Corpse != null )
							toTele.MoveToWorld( toTele.Corpse.Location, toTele.Corpse.Map );

						toTele.Resurrect();
					}
					else if( toTele.Alive )
					{
						toTele.Hits = toTele.HitsMax;
						toTele.Mana = toTele.ManaMax;
						toTele.Stam = toTele.StamMax;
					}

					toTele.Location = ds.AfterDuelLocation;
					toTele.Frozen = false;

					if( !toTele.Alive )
						toTele.Resurrect();

					(toTele).HasFilter = false;

                    // Clear aggressor list
                    List<AggressorInfo> toRemove = new List<AggressorInfo>();
                    foreach (AggressorInfo aI in toTele.Aggressors)
                    {
                        if (aI.Attacker == toTele || aI.Defender == toTele)
                            toRemove.Add(aI);
                    }
                    if (toRemove.Count > 0)
                        foreach (AggressorInfo aI in toRemove)
                            toTele.Aggressors.Remove(aI);

                    toRemove.Clear();
                    foreach (AggressorInfo aI in toTele.Aggressed)
                    {
                        if (aI.Attacker == toTele || aI.Defender == toTele)
                            toRemove.Add(aI);
                    }
                    if (toRemove.Count > 0)
                        foreach (AggressorInfo aI in toRemove)
                            toTele.Aggressed.Remove(aI);


                    //AggressorInfo toRemove = null;
                    //foreach (AggressorInfo aI in toTele.Aggressors)
                    //{
                    //    if (aI.Attacker == toTele) // Might also want to 
                    //        toRemove = aI; //Add a break here
                    //}

				}
				else
					toTele.SendAsciiMessage( "Error with the stone, page a GM" );
		}

		public static void TeleOut( object state )
		{
			Mobile toExit = null;
			DuelStone ds = (DuelStone)state;

			if( ( ds.Combatant2 != null && !ds.Combatant2.Alive ) && ( ds.Combatant1 != null && !ds.Combatant1.Alive ) )
			{
				LeaveDuel( ds, ds.Combatant1 );

				LeaveDuel( ds, ds.Combatant2 );
				((PlayerMobile)ds.Combatant2).DuelStone = null;
				ds.Combatant2 = null;

				toExit = null;
			}

			if( ds.Combatant2 != null && !ds.Combatant2.Alive )
			{
				toExit = ds.Combatant2;
				((PlayerMobile)ds.Combatant2).DuelStone = null;
				ds.Combatant2 = null;
			}
			else if( ds.Combatant1 != null && !ds.Combatant1.Alive )
			{
				toExit = ds.Combatant1;
				((PlayerMobile)ds.Combatant1).DuelStone = null;
				ds.Combatant1 = null;
			}

			if( toExit != null )
				LeaveDuel( ds, toExit );
		}

		public static void CountToEnd( object state )
		{
			object[] states = (object[])state;
			DuelStone ds = (DuelStone)states[0];
			int timer = (int)states[1];

			if( ds.EndDuelTimer != null )
			{
				ds.EndDuelTimer.Stop();
				ds.EndDuelTimer = null;
			}

			if( ds.EndTimer )
			{
				timer -= ( ds.StoneMaxIdleTime - ds.StoneIdleTime );
				ds.EndTimer = false;
			}

			if( timer < 1 )
			{
				ds.PublicOverheadMessage( MessageType.Regular, 906, true, "Duelstone active!" );

				ResetTimers( ds );

				TeleOut( ds );
			}
			else
				ds.PublicOverheadMessage( MessageType.Regular, 906, true, timer.ToString() );

			states[1] = timer - 1;
			ds.TimeLeft = (int)states[1];
		}

		public static void EndLongDuel( object state )
		{
			DuelStone ds = (DuelStone)state;

			if( ds.Combatant1 != null )
			{
				EndTeleOut( ds, ds.Combatant1 );

				if( ds.DuelCost != 0 && !ds.StoneType.ToString().Contains( "Loot" ) )
				{
					ds.Combatant1.Backpack.AddItem( new Gold( (int)( ds.DuelCost * 0.5 ) ) );
					ds.Combatant1.SendAsciiMessage( (int)( ds.DuelCost * 0.5 ) + " gold has been added to your backpack. You have lost 50% due to the long duel." );
				}
				((PlayerMobile)ds.Combatant1).DuelStone = null;
				ds.Combatant1 = null;
			}

			if( ds.Combatant2 != null )
			{
				EndTeleOut( ds, ds.Combatant2 );

				if( ds.DuelCost != 0 && !ds.StoneType.ToString().Contains( "Loot" ) )
				{
					ds.Combatant2.Backpack.AddItem( new Gold( (int)( ds.DuelCost * 0.5 ) ) );
					ds.Combatant2.SendAsciiMessage( (int)( ds.DuelCost * 0.5 ) + " gold has been added to your backpack. You have lost 50% due to the long duel." );
				}

				((PlayerMobile)ds.Combatant2).DuelStone = null;
				ds.Combatant2 = null;
			}

			ResetTimers( ds );
		}

		public static void ResetTimers( DuelStone ds )
		{
			if( ds == null )
				return;

			if( ds.CountTimer != null )
			{
				ds.CountTimer.Stop();
				ds.CountTimer = null;
			}

			if( ds.MoveTimer != null )
			{
				ds.MoveTimer.Stop();
				ds.MoveTimer = null;
			}

			if( ds.EndDuelTimer != null )
			{
				ds.EndDuelTimer.Stop();
				ds.EndDuelTimer = null;
			}

			ds.EndTimer = false;
		}
	}
}