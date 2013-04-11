using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Scripts.Custom.Adds.System
{
    public static class SystemCleanup
    {
        private static readonly Dictionary<Item, object> m_InvalidItems = new Dictionary<Item, object>();

        private static readonly Dictionary<Type, object> m_ValidTypes = new Dictionary<Type, object>();
        private static readonly List<Type> m_ExceptionTypes = new List<Type> { typeof(Bandana), typeof(SkullCap), typeof(Dagger) };

        public static void Initialize()
        {
            foreach (Type type in m_ExceptionTypes)
            {
                if (!m_ValidTypes.ContainsKey(type))
                    m_ValidTypes.Add(type, null);
            }
        }

        public static void DoCleanup()
        {
            //Find all the items that have PlayerVendors as their parents
            foreach (Mobile m in World.Mobiles.Values)
                if (m is PlayerVendor)
                    AddExcludedItems(m.Items);

            int excluded = 0;
            List<Container> containerList = new List<Container>();

            //Find all the types that we need to stack and all the containers holding them
            foreach (Item item in World.Items.Values)
            {
                if (!(item is Container))
                {
                    if (item.Stackable && !m_ValidTypes.ContainsKey(item.GetType()))
                        m_ValidTypes.Add(item.GetType(), null);

                    continue;
                }

                Container container = (Container) item;

                if (container.Layer == Layer.ShopBuy || container.Layer == Layer.ShopSell 
                    || container.Layer == Layer.ShopResale || container.RootParent is BaseVendor
                    || container.Parent is BaseVendor || container.Parent is PlayerVendor 
                    || container.RootParent is PlayerVendor )
                {
                    excluded++;
                }
                else
                    containerList.Add(container);
            }

            int oldItems = World.Items.Values.Count;
            Console.WriteLine(string.Format("Attempting to clean up {0} item types in {1} containers.", +m_ValidTypes.Count, containerList.Count));
            foreach (Container container in containerList)
            {
                foreach (Type type in m_ValidTypes.Keys)
                {
                    if ( type.IsSubclassOf(typeof(BaseWeapon)))
                    {
                        List<BaseWeapon> itemList = container.FindItemsByType<BaseWeapon>(false);

                        if (itemList.Count < 2)
                            continue;

                        for (int i = 0; i < itemList.Count; i++)
                            StackWith(itemList[i], itemList, i + 1);
                    }
                    else if (type.IsSubclassOf(typeof(BaseClothing)))
                    {
                        List<BaseClothing> itemList = container.FindItemsByType<BaseClothing>(false);

                        if (itemList.Count < 2)
                            continue;

                        for (int i = 0; i < itemList.Count; i++)
                            StackWith(itemList[i], itemList, i + 1);
                    }
                    else
                    {
                        Item[] itemList = container.FindItemsByType(type, false);

                        if (itemList.Length < 2)
                            continue;

                        for (int i = 0; i < itemList.Length; i++)
                            StackWith(itemList[i], itemList, i+1);
                    }
                }
            }

            Console.WriteLine(String.Format("Items deleted:{0} | Total containers:{1} | Excluded containers:{2} | Items left:{3}", 
                (oldItems - World.Items.Values.Count), containerList.Count, excluded, World.Items.Values.Count));
        }

        private static void AddExcludedItems(IEnumerable<Item> itemList)
        {
            foreach (Item item in itemList)
            {
                m_InvalidItems.Add(item, null);

                if (item.Items.Count > 0)
                    AddExcludedItems(item.Items);
            }
        }

        #region Validation
        private static bool ValidateItem(Item toStack, Item item)
        {
            return !(item == null || item.Deleted || item.IsSecure || item.IsLockedDown || toStack.IsSecure
                || toStack.IsLockedDown || !toStack.Stackable || !item.Stackable || toStack.ItemID != item.ItemID
                || item == toStack || item.LootType != toStack.LootType || toStack.Hue != item.Hue
                || toStack.Name != item.Name || (toStack.Amount + item.Amount) > 60000);
        }

        private static bool ValidateWeapon(BaseWeapon toStack, BaseWeapon item)
        {
            return !( !ValidateItem(toStack, item) || toStack.AccuracyLevel != item.AccuracyLevel || toStack.DamageLevel != item.DamageLevel
                        || toStack.Quality != item.Quality || toStack.MaxDamage != item.MaxDamage 
                        || toStack.MinDamage != item.MinDamage);
        }

        private static bool ValidateClothing(BaseClothing toStack, BaseClothing item)
        {
            return !( !ValidateItem(toStack, item) || (toStack is IArcaneEquip) != (item is IArcaneEquip) || toStack.Quality != item.Quality );
        }
        #endregion

        #region Stacking
        private static void StackWith(Item toStack, Item[] itemList, int startIndex)
        {
            if (toStack == null || toStack.Deleted)
                return;

            for (int i = startIndex; i < itemList.Length; i++)
            {
                Item item = itemList[i];
                if ( ValidateItem(toStack, item))
                {
                    item.Amount += toStack.Amount;
                    toStack.Delete();
                    return;
                }
            }
        }

        private static void StackWith(BaseWeapon toStack, List<BaseWeapon> itemList, int startIndex)
        {
            if (toStack == null || toStack.Deleted)
                return;

            for (int i = startIndex; i < itemList.Count; i++)
            {
                BaseWeapon item = itemList[i];
                if (ValidateWeapon(toStack, item))
                {
                    item.Amount += toStack.Amount;
                    toStack.Delete();
                    return;
                }
            }
        }

        private static void StackWith(BaseClothing toStack, List<BaseClothing> itemList, int startIndex)
        {
            if (toStack == null || toStack.Deleted)
                return;

            for (int i = startIndex; i < itemList.Count; i++)
            {
                BaseClothing item = itemList[i];
                if (ValidateClothing(toStack, item))
                {
                    item.Amount += toStack.Amount;
                    toStack.Delete();
                    return;
                }
            }
        }
        #endregion
    }
}