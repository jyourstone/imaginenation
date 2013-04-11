using System;
using System.Collections.Generic;
using System.Text;
using Server.Mobiles;
using System.Data;
using System.Runtime.CompilerServices;

namespace Server.Scripts.Custom.Adds.System.Database
{
    public static class INXDatabase
    {
        private static MySQLDriver db;

        public static void Initialize()
        {
            db = new MySQLDriver("localhost", "database", "user", "password");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static bool CheckMobile(PlayerMobile mob)
        {
            Resource resource = db.query("SELECT count(*) FROM playermobiles WHERE id = " + (int)mob.Serial, MySQLDriver.AdapterCommandType.Select);
            if (db.Connected)
            {
                DataRow row = resource.nextRow();

                if (row == null)
                    return false;
                if (row[0] != null && row[0].ToString() == "0")
                {
                    InsertNewMobile(mob);
                    return true;
                }
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void InsertNewMobile(PlayerMobile mob)
        {
            db.query("INSERT INTO playermobiles (id, name, rating, tournamentrating) VALUES (" + (int)mob.Serial + ", '" + mob.Name + "', " + mob.Rating + ", " + mob.TournamentRating + ");", MySQLDriver.AdapterCommandType.Insert);
        }

        public static void ResetDatabase()
        {

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Resource Query(string query, MySQLDriver.AdapterCommandType commandType)
        {
            return db.query(query, commandType);
        }
    }
}
