using System.Data;
using System.Data.Odbc;
using System;

namespace Server.Scripts.Custom.Adds.System.Database
{
    public class Resource : DataTable
    {
        private int index = 0;

        public Resource()
            : base()
        {
        }

        public DataRow nextRow()
        {
            if (index < this.Rows.Count)
                return this.Rows[index++];
            return null;
        }
    }
}