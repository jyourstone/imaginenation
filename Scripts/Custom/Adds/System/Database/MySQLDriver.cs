using System.Data;
using System.Data.Odbc;
using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace Server.Scripts.Custom.Adds.System.Database
{
    public class MySQLDriver
    {
        public enum AdapterCommandType
        {
            Select,
            Insert,
            Update,
            Delete,
            None

        }

        private string host;
        private string db;
        private string user;
        private string password;

        private OdbcConnection connection;
        private OdbcDataAdapter adapter;

        private bool connected = false;

        public MySQLDriver(string host, string db, string user, string password)
        {
            this.host = host;
            this.db = db;
            this.user = user;
            this.password = password;
            connect(host, db, user, password);
        }

        public bool Connected
        {
            get
            {
                return connected;
            }
        }

        public bool connect(string host, string db, string user, string password)
        {
            string connectString = "DRIVER={MySQL ODBC 5.1 Driver};" + "SERVER=" + host + ";" + "DATABASE=" + db + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";" + "OPTION=67108867";
            //Console.WriteLine("connecting to db: " + connectString);
            try
            {
                connection = new OdbcConnection(connectString);

                connection.Open();

                //Set the data adapter 
                adapter = new OdbcDataAdapter();

                connected = true;
                //Console.WriteLine("connected");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot connect to the MySQL server.");
                Console.WriteLine(e.StackTrace);
                connected = false;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Resource query(string query, AdapterCommandType type)
        {
            //Console.WriteLine("Query: " + query);
            Resource datatable = new Resource();

            if (!connected || connection.State != ConnectionState.Open)
            {
                connect(host, db, user, password);
                if (connected)
                {
                    return this.query(query, type);
                }
                else
                {
                    return datatable;
                }
            }
            try
            {
                if (type == AdapterCommandType.Update || type == AdapterCommandType.Insert || type == AdapterCommandType.Delete)
                {
                    OdbcCommand odbcCommand = connection.CreateCommand();
                    odbcCommand.CommandText = query;
                    odbcCommand.Prepare();
                    odbcCommand.ExecuteNonQuery();

                    return datatable;
                }
                else if (type == AdapterCommandType.Select)
                {

                    OdbcCommand command = connection.CreateCommand();
                    command.CommandText = query;

                    adapter.SelectCommand = command;
                    adapter.Fill(datatable);
                    return datatable;
                }
            }
            catch (InvalidOperationException e) //Database is disconnected
            {
                Console.WriteLine("Invalid Operation Exception at Query: " + query);
                Console.WriteLine("Message: " + e.Message);
                Console.WriteLine(e.StackTrace);
                /*connect(host, db, user, password);
                if(connected)
                	return this.query(query, type);*/
                return datatable;
            }
            catch (OdbcException e) //Database already has the value
            {
                Console.WriteLine("OdbcException at Query: " + query);
                Console.WriteLine("Message: " + e.Message);
                Console.WriteLine(e.StackTrace);
                return datatable;
            }
            return datatable;
        }
    }
}