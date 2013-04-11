namespace Server.Scripts.Custom.Adds.System.KillInfo.DatabaseEntries
{
    /// <summary>
    /// Represents an entry in the itemTable
    /// pkItem, fkKillEntryID (to bind it to the right kill), TypeName, Amount
    /// </summary>
    public class ItemEntry : DatabaseEntry
    {
        public ItemEntry(Item item)
        {

        }

        /// <summary>
        /// Executes and stores the item entry in the data base.
        /// </summary>
        /// <returns>the primary key assosiated with the entry, null if execution failed.</returns>
        public override int? Execute(int? killInfoID)
        {
            return 1;
        }
    }
}