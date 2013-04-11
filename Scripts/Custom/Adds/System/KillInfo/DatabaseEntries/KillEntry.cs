using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Scripts.Custom.Adds.System.KillInfo.DatabaseEntries
{
    public class KillEntry : DatabaseEntry
    {
        private readonly List<DatabaseEntry> m_DatabaseEntries;
        
        private readonly BaseCreature m_CreatureKilled;

        private int m_PlayersDied;
        /*
        private int m_PlayerCount;
        private int m_BSCount;
        private int m_EVCount;
        */
        private TimeSpan m_KillTime;

        //Contains the type of the various killers and how many of them there were.
        private readonly Dictionary<Type, int> m_KillerTypeList = new Dictionary<Type, int>(10);

        /// <summary>
        /// Represents an entry in the killTable
        /// pkKillEntryID, fkMonsterEntryID, PlayersKilled, PlayerCount, BSCount, EVcount
        /// </summary>
        /// <param name="creatureKilled"></param>
        /// <param name="damageEntries"></param>
        /// <param name="mobilesKilled"></param>
        /// <param name="lootItems"></param>
        public KillEntry(BaseCreature creatureKilled, IList<DamageEntry> damageEntries, Dictionary<Mobile, int> mobilesKilled, IList<Item> lootItems)
        {
            m_CreatureKilled = creatureKilled;

            m_KillTime = m_CreatureKilled.KillDuration;

            //Used to determine if a killer has accoured
            List<Mobile> monsterKillerList = GetKillerList(damageEntries);
            
            SetKillerTypes(monsterKillerList);

            SetPlayersDied(mobilesKilled, monsterKillerList);

            //Initialize the list and add the gold entry to it
            m_DatabaseEntries = new List<DatabaseEntry>(lootItems.Count + 1) { new GoldValueEntry(lootItems) };

            //Loop through the remaining items (gold and stones excluded) and add them to the DatabaseEntry list
            for (int i = 0; i < lootItems.Count; i++)
            {
                Item item = lootItems[i];

                if (item is BaseWeapon)
                    m_DatabaseEntries.Add(new WeaponEntry((BaseWeapon)item));
                else if (item is BaseArmor)
                    m_DatabaseEntries.Add(new ArmorEntry((BaseArmor)item));
                else
                    m_DatabaseEntries.Add(new ItemEntry(item));
            }
        }

        private void SetPlayersDied(IEnumerable<KeyValuePair<Mobile, int>> mobilesKilled, ICollection<Mobile> monsterKillers)
        {
            int playersDied = 0;
            foreach (KeyValuePair<Mobile, int> pair in mobilesKilled)
                if (pair.Key is PlayerMobile && monsterKillers.Contains(pair.Key))
                    playersDied += pair.Value;

            m_PlayersDied = playersDied;
        }

        private void SetKillerTypes(IEnumerable<Mobile> monsterKillerList)
        {
            foreach (Mobile mobile in monsterKillerList)
            {
                Type killerType = mobile.GetType();
                if (m_KillerTypeList.ContainsKey(killerType))
                    m_KillerTypeList[killerType] = (m_KillerTypeList[killerType] + 1);
                else
                    m_KillerTypeList.Add(killerType, 1);
            }
        }

        private static List<Mobile> GetKillerList(IList<DamageEntry> damageEntries)
        {
            List<Mobile> killerList = new List<Mobile>();

            //Get a list of the killers involved
            for (int i = damageEntries.Count - 1; i >= 0; --i)
            {
                if (i >= damageEntries.Count)
                    continue;

                DamageEntry de = damageEntries[i];

                if (de.HasExpired || killerList.Contains(de.Damager))
                {
                    damageEntries.RemoveAt(i);
                    continue;
                }

                killerList.Add(de.Damager);
            }

            return killerList;
        }

        public override int? Execute(int? killInfoID)
        {
            //Inset KillInfoEntry into the database, retrieve the pkKillEntryID
            //Now run through each DataBaseEntry in m_DatabaseEntries, call Execute and supply the pkKillEntryID
            return 1;
        }

        //TODO:
        public override string ToString()
        {
            return "Hejhej";
        }
    }
}