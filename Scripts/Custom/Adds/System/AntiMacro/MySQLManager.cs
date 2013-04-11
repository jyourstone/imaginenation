using System.Data;
using System.Data.Odbc;
using System;

namespace Server.AntiMacro
{
    public class MySQLManager
    {
        private enum AdapterCommandType
        {
            Select,
            Insert,
            Update,
            Delete,
            None

        }

        private static OdbcConnection m_MySQLConnection;
        private static OdbcDataAdapter m_DataAdapter;
        private static bool m_SQLEnabled = false;

        private const string c_TableName = "CurrentMobiles";
        private const string c_ConnectionString = "DRIVER={MySQL ODBC 5.1 Driver};" + "SERVER=localhost;" + "DATABASE=database;" + "UID=user;" + "PASSWORD=password;" + "OPTION=3";

        public static bool SQLEnabled
        {
            get { return m_SQLEnabled; }
        }

        public static void Initialize()
        {
            if ( m_SQLEnabled )
                m_SQLEnabled = ConnectToDB();
        }

        public static bool ConnectToDB()
        {
            try
            {
                OdbcConnection dbConnection = new OdbcConnection(c_ConnectionString);

                dbConnection.Open();
                m_MySQLConnection = dbConnection;

                //Set the data adapter 
                m_DataAdapter = new OdbcDataAdapter();

                return true;
            }
            catch
            {
                Console.WriteLine("Cannot connect to the MySQL server, using the old anti macro system.");
                m_MySQLConnection = null;
                return false;
            }
        }

        public static bool InsertToSQL(Mobile from, int seed, double endTime)
        {
            if (from == null)
                return false;

            int mobileHash = from.Serial.GetHashCode();
            string sqlString = string.Empty;

            try
            {
                sqlString = "INSERT INTO " + c_TableName + " (" + " mobileHash, seed,  endTime )" + " VALUES ( " + mobileHash + ", " +seed +", " + (int)endTime + " )";

                OdbcCommand command = GetCommand(AdapterCommandType.None, sqlString);

                return command.ExecuteNonQuery() != 0;
            }
            catch (InvalidOperationException) //Database is disconnected
            {
                return SafeExecuteNonQuery(AdapterCommandType.None, sqlString, from, endTime, seed);
            }
            catch (OdbcException) //Database already has the value
            {
                sqlString = "UPDATE " + c_TableName + " SET seed = " + seed + ", endTime = " + (int)endTime + ", answer = NULL WHERE mobileHash = " + mobileHash;

                return SafeExecuteNonQuery(AdapterCommandType.None, sqlString, from, endTime, seed);
            }
        }

        public static bool ValidateInput(Mobile from, string input, int triesLeft)
        {
            if (from == null)
                return true;

            int mobileHash = from.Serial.GetHashCode();

            DataTable dt = new DataTable();

            string sqlString = "SELECT answer FROM " + c_TableName + " WHERE mobileHash = " + mobileHash;

            SafeFill(dt, AdapterCommandType.Select, sqlString);

            if (dt.Rows == null || dt.Rows.Count == 0)
                return true;

            object answerObject = dt.Rows[0].ItemArray[0];
            if (answerObject == null || dt.Rows[0].ItemArray.Length == 0)
                return true;

            if (answerObject is System.DBNull || string.IsNullOrEmpty(answerObject.ToString()))
            {
                //if (triesLeft == 1)
                //    RemoveFromDataBase(mobileHash);
                //else
                from.SendAsciiMessage("Please open the web page, otherwise your answer will count as invalid.");
                return false;
            }

            string answer = (string)dt.Rows[0].ItemArray[0];
            bool valid = answer == input;

            //Remove the entry from the database
            //if (triesLeft == 1 || valid)
            if (valid)
                RemoveFromDataBase(mobileHash);

            return valid;
        }

        public static void RemoveFromDataBase(int mobileHash)
        {
            string sqlString = "DELETE FROM " + c_TableName + " WHERE mobileHash = " + mobileHash;

            SafeExecuteNonQuery(AdapterCommandType.None, sqlString, null, 0, 0);
        }

        private static bool SafeExecuteNonQuery(AdapterCommandType commandType, string commandText, IEntity from, double endTime, int seed)
        {
            OdbcCommand command;
            int mobileHash = 0;
            
            if (from != null)
                mobileHash = from.Serial.GetHashCode();

            try
            {
                command = GetCommand(commandType, commandText);
                return command.ExecuteNonQuery() != 0;
            }
            catch (InvalidOperationException)
            {
                if (ConnectToDB())
                {
                    //Get the new command (due to connection reset
                    command = GetCommand(commandType, commandText);
                    try
                    {
                        return command.ExecuteNonQuery() != 0;
                    }
                    catch (OdbcException) //Database already has the value
                    {
                        commandText = "UPDATE " + c_TableName + " SET seed = " + seed + ", endTime = " + (int)endTime + ", answer = NULL WHERE mobileHash = " + mobileHash;
                        command = GetCommand(commandType, commandText);

                        return command.ExecuteNonQuery() != 0;
                    }
                }

                return false;
            }
        }

        private static void SafeFill(DataTable table, AdapterCommandType commandType, string commandText)
        {
            OdbcCommand command = null;
            try
            {
                command = GetCommand(commandType, commandText);
                m_DataAdapter.Fill(table);
            }
            catch (InvalidOperationException)
            {
                if (ConnectToDB())
                {
                    //Get the new command (due to connection reset
                    command = GetCommand(commandType, commandText);
                    m_DataAdapter.Fill(table);
                }
            }
        }

        private static OdbcCommand GetCommand(AdapterCommandType commandType, string commandText)
        {
            OdbcCommand odbcCommand = m_MySQLConnection.CreateCommand();
            odbcCommand.CommandText = commandText;

            switch (commandType)
            {
                case AdapterCommandType.Select:
                    {
                        m_DataAdapter.SelectCommand = odbcCommand;
                        break;
                    }
                case AdapterCommandType.Insert:
                    {
                        m_DataAdapter.InsertCommand = odbcCommand;
                        break;
                    }
                case AdapterCommandType.Update:
                    {
                        m_DataAdapter.UpdateCommand = odbcCommand;
                        break;
                    }
                case AdapterCommandType.Delete:
                    {
                        m_DataAdapter.DeleteCommand = odbcCommand;
                        break;
                    }
                default:
                    break;
            }

            return odbcCommand;
        }
    }
}