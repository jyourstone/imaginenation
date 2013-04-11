using Server.Items;

namespace Server.Scripts.Custom.Adds.System.KillInfo.DatabaseEntries
{
    /// <summary>
    /// Represents an entry in the weaponTable
    /// pkWeapon, fkItem, DamageLevel, Accuracy, Quality
    /// </summary>
    public class WeaponEntry : ItemEntry
    {
        public WeaponEntry(BaseWeapon weapon) : base(weapon)
        {

        }

        /// <summary>
        /// Executes and stores the weapon entry in the data base. Before doing so, the parent item entry is stored.
        /// </summary>
        /// <returns>the primary key assosiated with the entry, null if execution failed.</returns>
        public override int? Execute(int? killInfoID)
        {
            int? fkItemID = base.Execute(killInfoID);

            return 1;
        }
    }
}