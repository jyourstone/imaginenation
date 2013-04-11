using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Scripts.Custom.Adds.System.KillInfo.DatabaseEntries
{
    public class GoldValueEntry : DatabaseEntry
    {
        private static Type GoldType = typeof(Gold);

        private readonly int m_Amount;
        public GoldValueEntry(IList<Item> itemList)
        {
            GetGoldValueRecursive(itemList, ref m_Amount);
        }

        /// <summary>
        /// Executes and stores the gold as a item entry in the data base.
        /// </summary>
        /// <returns>the primary key assosiated with the entry, null if execution failed.</returns>
        public override int? Execute(int? killInfoID)
        {
            return 1;
        }

        /// <summary>
        /// Will count each undefined item as 1 gold, but i do not think that makes a huge diffrence
        /// </summary>
        /// <param name="itemList"></param>
        /// <param name="goldValue"></param>
        /// <returns></returns>
        private static void GetGoldValueRecursive(IList<Item> itemList, ref int goldValue)
        {
            ////Get a list of the killers involved
            //for (int i = damageEntries.Count - 1; i >= 0; --i)
            //{
            //    if (i >= damageEntries.Count)
            //        continue;

            //    DamageEntry de = damageEntries[i];

            //    if (de.HasExpired || killerList.Contains(de.Damager))
            //    {
            //        damageEntries.RemoveAt(i);
            //        continue;
            //    }

            //    killerList.Add(de.Damager);
            //}

            List<Item> toRemove = new List<Item>();
            int lastIndex = 0;
            try
            {
                for (int i = itemList.Count - 1; i >= 0; --i)
                {
                    if (i >= itemList.Count)
                        continue;

                    lastIndex = i;
                    Item item = itemList[i];
                    itemList.RemoveAt(i);

                    GetGoldValueRecursive(item.Items, ref goldValue);

                    int itemValue = item.Amount;
                    if (item is Amber || item is Citrine)
                        itemValue *= 25;
                    else if (item is Sapphire || item is Emerald || item is Amethyst)
                        itemValue *= 50;
                    else if (item is Diamond)
                        itemValue *= 100;
                    else if (item is StarSapphire)
                        itemValue *= 62;
                    else if (item is Ruby)
                        itemValue *= 37;
                    else if (item is Tourmaline)
                        itemValue *= 47;

                    goldValue += itemValue;
                    continue;
                }
            }
            catch {
                Console.WriteLine(lastIndex);
            }
        }
    }
}
