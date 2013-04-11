using System;
using System.Data;
using System.Data.Odbc;
using System.Threading;

namespace Server.DuelSystem
{
	public class DuelstoneLadderUpdate
	{
		public static string tableName = "duelstones";
		public static string selectString = "SELECT * FROM " + tableName;

        public static string connectionString = "DRIVER={MySQL ODBC 3.51 Driver};" + "SERVER=localhost;" + "DATABASE=database;" + "UID=user;" + "PASSWORD=password;" + "OPTION=3";

		public OdbcConnection stoneDbConnecton;
		public DuelStone stoneToUpdate;
		public Mobile personWhoLost, personWhoWon;
		public DataTable duelstoneTable = new DataTable();
		public DateTime lastReset = new DateTime();

		public int loserPoints = 0, winnerPoints = 0;
		public bool usingPointSystem = false;

		public bool ConnectToVoteDB()
		{
			if( stoneDbConnecton == null || ( stoneDbConnecton != null && ( stoneDbConnecton.State == ConnectionState.Closed || stoneDbConnecton.State == ConnectionState.Broken ) ) )
			{
				OdbcConnection dbconnection = new OdbcConnection( connectionString );

				try
				{
					dbconnection.Open();
					stoneDbConnecton = dbconnection;

					if( FillDataTable() )
					{
						lastReset = DateTime.Now;
						return true;
					}
					else
					{
						dbconnection.Close();
						stoneDbConnecton = null;
						return false;
					}
				}
				catch
                {
					stoneDbConnecton = dbconnection = null;
					duelstoneTable = null;
					return false;
				}
			}
			else if( lastReset + new TimeSpan( 0, 15, 0 ) <= DateTime.Now )
			{
				stoneDbConnecton.Close();
				stoneDbConnecton = null;
				Thread updateStoneThread2 = new Thread( UpdateLadder );
				updateStoneThread2.Start();
				return false;
			}
			else if( FillDataTable() )
				return true;
			else
				return false;
		}

		public bool FillDataTable()
		{
			duelstoneTable = new DataTable();
			OdbcDataAdapter dataAdapter = new OdbcDataAdapter();
			OdbcCommand selectCommand = stoneDbConnecton.CreateCommand();
			selectCommand.CommandText = selectString;
			dataAdapter.SelectCommand = selectCommand;

			dataAdapter.Fill( duelstoneTable );

			//Set Primary key
			try
			{
				DataColumn[] primaryKey = new DataColumn[1];
				primaryKey[0] = duelstoneTable.Columns[0];
				duelstoneTable.PrimaryKey = primaryKey;
				return true;
			}
			catch
			{
				return false;
			}
		}

		public void AddWinner( Mobile winner )
		{
			DataTable tempTable = new DataTable();
			tempTable = duelstoneTable.Copy();

			#region Set Stone Name
			string stoneName = "";

			if( stoneToUpdate.StoneType.ToString() == "Money1vs1" )
				stoneName = "Money 1vs1";
			else if( stoneToUpdate.StoneType.ToString() == "Loot1vs1" )
				stoneName = "Loot 1vs1";
			if( stoneToUpdate.StoneType.ToString() == "Money2vs2" )
				stoneName = "Money 2vs2";
			else if( stoneToUpdate.StoneType.ToString() == "Loot2vs2" )
				stoneName = "Loot 2vs2";

			if( stoneName.Contains( "Money" ) )
				stoneName += " (" + stoneToUpdate.DuelCost + ")";
			#endregion

			if( duelstoneTable.Rows.Contains( ( (int)winner.Serial ).ToString() ) )
				UpdateStats( winner, "won" );
			else
			{
				string addString = "INSERT INTO " + tableName + " (" + " Mobile,  Name, Points, Won, Lost, MoneyEarned, Killed, KilledBy, StonesUsed)" + " VALUES ( " + winner.Serial.Value + ", '" + winner.Name + "', 1, 1, 0, " + stoneToUpdate.DuelCost + ", '1*" + personWhoLost.Serial.Value + ";','', '1*" + stoneName + ";')";

				OdbcCommand addCommand = stoneDbConnecton.CreateCommand();
				addCommand.CommandText = addString;

				//Add Killed to guild ladder

                addCommand.ExecuteNonQuery();
            }
		}

		public void AddLoser( Mobile loser )
		{
			#region Set Stone Name
			string stoneName = "";

			if( stoneToUpdate.StoneType.ToString() == "Money1vs1" )
				stoneName = "Money 1vs1";
			else if( stoneToUpdate.StoneType.ToString() == "Loot1vs1" )
				stoneName = "Loot 1vs1";
			if( stoneToUpdate.StoneType.ToString() == "Money2vs2" )
				stoneName = "Money 2vs2";
			else if( stoneToUpdate.StoneType.ToString() == "Loot2vs2" )
				stoneName = "Loot 2vs2";

			if( stoneName.Contains( "Money" ) )
				stoneName += " (" + stoneToUpdate.DuelCost + ")";
			#endregion

			if( duelstoneTable.Rows.Contains( ( (int)loser.Serial ).ToString() ) )
				UpdateStats( loser, "lost" );
			else
			{
				string addString = "INSERT INTO " + tableName + " (" + " Mobile,  Name, Points, Won, Lost, MoneyEarned, Killed, KilledBy, StonesUsed)" + " VALUES ( " + loser.Serial.Value + ", '" + loser.Name + "', -1, 0, 1, -" + stoneToUpdate.DuelCost + ",'', '1*" + personWhoWon.Serial.Value + ";', '1*" + stoneName + ";')";

				OdbcCommand addCommand = stoneDbConnecton.CreateCommand();
				addCommand.CommandText = addString;

    			addCommand.ExecuteNonQuery();
			}
		}

		public void UpdateStats( Mobile toUpdate, string outcome )
		{
			DataRow existingRow = duelstoneTable.Rows.Find( ( (int)toUpdate.Serial ).ToString() );

			string killed, killedBy, stonesUsed;
			string stringNumber = "";
			string updatePlayerString = "";

			int points = int.Parse( existingRow.ItemArray[2].ToString() ), won = int.Parse( existingRow.ItemArray[3].ToString() ), lost = int.Parse( existingRow.ItemArray[4].ToString() );
			int joined = won + lost;
			int repeatedNumber = 0, moneyCost = 0;

			//Set MoneyEarned
			int moneyEarned = 0;
			int.TryParse( existingRow.ItemArray[5].ToString(), out moneyEarned );

			#region Set Stone Name
			stonesUsed = existingRow.ItemArray[8].ToString();
			string stoneName = "";

			if( stoneToUpdate.StoneType.ToString() == "Money1vs1" )
				stoneName = "Money 1vs1";
			else if( stoneToUpdate.StoneType.ToString() == "Loot1vs1" )
				stoneName = "Loot 1vs1";
			if( stoneToUpdate.StoneType.ToString() == "Money2vs2" )
				stoneName = "Money 2vs2";
			else if( stoneToUpdate.StoneType.ToString() == "Loot2vs2" )
				stoneName = "Loot 2vs2";

			if( stoneName.Contains( "Money" ) )
			{
				stoneName += " (" + stoneToUpdate.DuelCost + ")";
				moneyCost = stoneToUpdate.DuelCost;
			}
			#endregion

			if( stonesUsed.Contains( "*" + stoneName ) )
			{
				while( ( stonesUsed.IndexOf( "*" + stoneName ) - 1 ) >= 0 && ';' != stonesUsed[( stonesUsed.IndexOf( "*" + stoneName ) - 1 )] )
				{
					stringNumber = stringNumber.Insert( 0, stonesUsed[( stonesUsed.IndexOf( "*" + stoneName ) - 1 )].ToString() );

					if( ( stonesUsed.IndexOf( "*" + stoneName ) - 1 ) >= 0 )
						stonesUsed = stonesUsed.Remove( stonesUsed.IndexOf( "*" + stoneName ) - 1, 1 );
				}

				int.TryParse( stringNumber, out repeatedNumber );

				repeatedNumber++;

				stonesUsed = stonesUsed.Insert( stonesUsed.IndexOf( "*" + stoneName ), repeatedNumber.ToString() );
			}
			else
				stonesUsed += "1*" + stoneName + ";";

			if( outcome == "won" )
			{
				killed = existingRow.ItemArray[6].ToString();
				stringNumber = "";

				//Update Kills
				if( killed.Contains( "*" + personWhoLost.Serial.Value ) )
				{
					while( ( killed.IndexOf( "*" + personWhoLost.Serial.Value ) - 1 ) >= 0 && ';' != killed[( killed.IndexOf( "*" + personWhoLost.Serial.Value ) - 1 )] )
					{
						stringNumber = stringNumber.Insert( 0, killed[( killed.IndexOf( "*" + personWhoLost.Serial.Value ) - 1 )].ToString() );

						if( ( killed.IndexOf( "*" + personWhoLost.Serial.Value ) - 1 ) >= 0 )
							killed = killed.Remove( killed.IndexOf( "*" + personWhoLost.Serial.Value ) - 1, 1 );
					}

					int.TryParse( stringNumber, out repeatedNumber );

					repeatedNumber++;

					killed = killed.Insert( killed.IndexOf( "*" + personWhoLost.Serial.Value ), repeatedNumber.ToString() );
				}
				else
					killed += "1*" + personWhoLost.Serial.Value + ";";

			    if( usingPointSystem || joined <= 15 )
				{
					points++;
				}

				//Update guildladder
				//if (this.personWhoLost.Guild != null && this.personWhoLost.Guild != null && this.personWhoLost.Guild != this.personWhoLost.Guild)
				//    Server.Misc.Guilds.GuildLadderUpdate.UpdateLadder(this.personWhoWon, this.personWhoLost, updatePoints);

				won++;
				moneyEarned += moneyCost;

				updatePlayerString = "UPDATE " + tableName + " SET " + "Name = '" + toUpdate.Name + "', " + "Points = " + points + ", " + "Won = " + won + ", " + "MoneyEarned  = " + moneyEarned + ", " + "Killed = '" + killed + "', " + "StonesUsed = '" + stonesUsed + "' " + "WHERE Mobile = " + toUpdate.Serial.Value;
			}
			else if( outcome == "lost" )
			{
				killedBy = existingRow.ItemArray[7].ToString();
				stringNumber = "";

				//Update Killers
				if( killedBy.Contains( "*" + personWhoWon.Serial.Value ) )
				{
					while( ( killedBy.IndexOf( "*" + personWhoWon.Serial.Value ) - 1 ) >= 0 && ';' != killedBy[( killedBy.IndexOf( "*" + personWhoWon.Serial.Value ) - 1 )] )
					{
						stringNumber = stringNumber.Insert( 0, killedBy[( killedBy.IndexOf( "*" + personWhoWon.Serial.Value ) - 1 )].ToString() );

						if( ( killedBy.IndexOf( "*" + personWhoWon.Serial.Value ) - 1 ) >= 0 )
							killedBy = killedBy.Remove( killedBy.IndexOf( "*" + personWhoWon.Serial.Value ) - 1, 1 );
					}

					int.TryParse( stringNumber, out repeatedNumber );

					repeatedNumber++;

					killedBy = killedBy.Insert( killedBy.IndexOf( "*" + personWhoWon.Serial.Value ), repeatedNumber.ToString() );
				}
				else
					killedBy += "1*" + personWhoWon.Serial.Value + ";";

				//Set Points:
				loserPoints = points;

				if( usingPointSystem )
					points--;

				lost++;
				moneyEarned -= moneyCost;

				updatePlayerString = "UPDATE " + tableName + " SET " + "Name = '" + toUpdate.Name + "', " + "Points = " + points + ", " + "Lost = " + lost + ", " + "MoneyEarned  = " + moneyEarned + ", " + "KilledBy = '" + killedBy + "', " + "StonesUsed = '" + stonesUsed + "' " + "WHERE Mobile = " + toUpdate.Serial.Value;
			}

			OdbcCommand updateCommand = stoneDbConnecton.CreateCommand();
			updateCommand.CommandText = updatePlayerString;

			updateCommand.ExecuteNonQuery();
		}

		public void SetUsingPoints()
		{
			DataRow killerRow = null;
			DataRow killedRow = null;

			if( duelstoneTable.Rows.Contains( ( (int)personWhoWon.Serial ).ToString() ) )
				killerRow = duelstoneTable.Rows.Find( ( (int)personWhoWon.Serial ).ToString() );

			if( duelstoneTable.Rows.Contains( ( (int)personWhoLost.Serial ).ToString() ) )
				killedRow = duelstoneTable.Rows.Find( ( (int)personWhoLost.Serial ).ToString() );

			int killerPoints = 0;
			if( killerRow != null )
				killerPoints = int.Parse( killerRow.ItemArray[2].ToString() );

			int killedPoints = 0;
			if( killedRow != null )
				killedPoints = int.Parse( killedRow.ItemArray[2].ToString() );

			if( killedPoints == 0 )
				killedPoints = 1;

			if( killerPoints == 0 )
				killerPoints = 1;

			if( ( killerPoints - killedPoints ) <= 10 )
				usingPointSystem = true;
		}

		public void UpdateLadder()
		{
			try
			{
			    if( !ConnectToVoteDB() || stoneToUpdate == null || duelstoneTable == null )
					return;

				#region Determin winner
				if( stoneToUpdate.Combatant1 != null && personWhoLost == stoneToUpdate.Combatant1 && stoneToUpdate.Combatant2 != null )
					personWhoWon = stoneToUpdate.Combatant2;
				else if( stoneToUpdate.Combatant2 != null && personWhoLost == stoneToUpdate.Combatant2 && stoneToUpdate.Combatant1 != null )
					personWhoWon = stoneToUpdate.Combatant1;
				#endregion

				//Check if players can gain points
				SetUsingPoints();

				//if (this.personWhoWon.Guild != null && this.personWhoLost.Guild != null && this.personWhoLost.Guild != this.personWhoWon.Guild)
				//    Server.Misc.Guilds.GuildLadderUpdate.UpdateLadder(this.personWhoWon, this.personWhoLost, usingPointSystem);

				if( personWhoLost != null )
					AddLoser( personWhoLost );

                if (personWhoWon != null)
                    AddWinner(personWhoWon);
			}
			catch
			{
				//Server.Misc.ErrorLog.ErrorLog.StartLogging(e);
			}
		}

		public static void StartUpdate( DuelStone ds, Mobile lost )
		{
			DuelstoneLadderUpdate ladderUpdate = new DuelstoneLadderUpdate();
			ladderUpdate.stoneToUpdate = ds;
			ladderUpdate.personWhoLost = lost;

			Thread updateStoneThread = new Thread( ladderUpdate.UpdateLadder );
			updateStoneThread.Start();
		}
	}
}