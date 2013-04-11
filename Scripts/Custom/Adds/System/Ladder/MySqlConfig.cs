using System.Data;
using System.Data.Odbc;

namespace Server.Ladder
{
    public static class MySQLConnection
    {
        static private OdbcConnection currentConnection;

        private static DataTable ladder1vs1, ladder2vs2, ladderTotal;

        public static string connectionString = "DRIVER={MySQL ODBC 3.51 Driver};" +
             "SERVER=localhost;" +
             "DATABASE=database;" +
             "UID=user;" +
             "PASSWORD=password;" +
             "OPTION=3";

        public static string ladderUrl = "http://www.in-uo.com";

        static public OdbcConnection GetmySQLConnection
        {
            get { return currentConnection; }
            set { currentConnection = value; }
        }

        static public DataTable Get1vs1Ladder
        {
            get { return ladder1vs1; }
            set { ladder1vs1 = value; }
        }

        static public DataTable Get2vs2Ladder
        {
            get { return ladder2vs2; }
            set { ladder2vs2 = value; }
        }

        static public DataTable GetTotalLadder
        {
            get { return ladderTotal; }
            set { ladderTotal = value; }
        }


        public static bool OpenConnection()
        {
            if (MySQLConnection.GetmySQLConnection == null || (MySQLConnection.GetmySQLConnection != null && (MySQLConnection.GetmySQLConnection.State == ConnectionState.Closed || MySQLConnection.GetmySQLConnection.State == ConnectionState.Broken)))
            {
                //Create connection
                OdbcConnection dbConnection = new OdbcConnection(MySQLConnection.connectionString);

                try
                {
                    dbConnection.Open();
                    MySQLConnection.GetmySQLConnection = dbConnection;
                    return true;
                }
                catch
                {
                    MySQLConnection.GetmySQLConnection = dbConnection = null;
                    return false;
                }
            }
            else
                return true;
        }

    }
}