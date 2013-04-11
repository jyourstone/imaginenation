using Server.Items;

namespace Server.Scripts.Custom.Adds.System.KillInfo.DatabaseEntries
{
    /// <summary>
    /// Represents an entry in the armorTable
    /// pkArmor, fkItem, ProtectionLevel, Quality
    /// </summary>
    public class ArmorEntry : ItemEntry
    {
        public ArmorEntry(BaseArmor armor) : base(armor)
        {

        }

        /// <summary>
        /// Executes and stores the armor entry in the data base. Before doing so, the parent item entry is stored.
        /// </summary>
        /// <returns>the primary key assosiated with the entry, null if execution failed.</returns>
        public override int? Execute(int? killInfoID)
        {
            int? fkItemID = base.Execute(killInfoID);

            return 1;
        }
    }
}