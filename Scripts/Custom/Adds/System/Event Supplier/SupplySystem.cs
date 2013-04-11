using System;
using System.Collections.Generic;
using Server.Custom.Games;
using Server.Items;
using Server.Network;
using Solaris.CliLocHandler;
using Server.Mobiles;

namespace Server
{
    public class SupplySystem
    {
        private const string m_NameFormat = "event {0}";

        #region Sets

        public static BaseArmor[] DefaultArmorList(Mobile m)
        {
            BaseArmor[] armorList = new BaseArmor[] { new PlateHelm(), new PlateGorget(), new PlateArms(), new PlateGloves(), new PlateChest(), new PlateLegs(),new MetalKiteShield() };

            for (int i = 0; i < armorList.Length; ++i)
            {
                BaseArmor armor = armorList[i];
                if (armor.StrRequirement > m.Str)
                    armor.Delete();
            }

            return armorList;
        }

        public static BaseWeapon[] DefaultWeaponList(Mobile m)
        {
            BaseWeapon[] weaponList = new BaseWeapon[] { new Bardiche(), new BattleAxe(), new Katana(), new Broadsword(), new QuarterStaff(), new WarFork(), new HammerPick(), new Halberd(), new WarHammer(), new Kryss(), new Crossbow(), new Bow(), new HeavyCrossbow(), new Spear(), new ShortSpear() };

            return weaponList;
        }

        public static BaseClothing[] DefaultClothList()
        {
            switch (Utility.Random(9))
            {
                case 0:
                    return new BaseClothing[] { new HalfApron(), new Doublet(), new Skirt() };
                case 1:
                    return new BaseClothing[] { new HalfApron(), new Doublet(), new Skirt() };
                case 2:
                    return new BaseClothing[] { new HalfApron(), new Doublet(), new Skirt() };
                case 3:
                    return new BaseClothing[] { new HalfApron(), new Doublet(), new Skirt() };
                case 4:
                    return new BaseClothing[] { new HalfApron(), new Doublet(), new Skirt() };
                case 5:
                    return new BaseClothing[] { new HalfApron(), new Doublet(), new Skirt() };
                case 6:
                    return new BaseClothing[] { new HalfApron(), new Doublet(), new Skirt() };
                case 7:
                    return new BaseClothing[] { new HalfApron(), new Doublet(), new Skirt() };
                case 8:
                    return new BaseClothing[] { new HalfApron(), new Doublet(), new Skirt() };
                default:
                    return new BaseClothing[] { new HalfApron(), new Doublet(), new Skirt() };
            }
        }

        public static Item[] DefaultMiscList()
        {
            return new Item[] { new Spellbook(UInt64.MaxValue), new BagOfReagents(2), new BagOfScrolls(2), new ManaPotion(), new TotalManaPotion(), new GreaterHealPotion(), new TotalRefreshPotion(), new Bandage(2), new Arrow(2), new Bolt(2) };
        }

        #endregion

        private static void MakeEventItems(Item[] itemList)
        {
            MakeEventItems(itemList, 0, string.Empty, true, false, false);
        }

        private static void MakeEventItems(Item[] itemList, int hue, string teamName, bool allNewbied, bool stayEquipped, bool consumeItems)
        {
            for (int i = 0; i < itemList.Length; ++i)
            {
                Item item = itemList[i];
                //Jump to next item if this item was flagged to be an event item somewhere else.
                if (item.EventItem)
                    continue;

                //Make the items newbied if we have selected them to be so
                if (allNewbied)
                    item.LootType = LootType.Newbied;

                //Flag the event item
                item.EventItem = true;

                //If the eventitem consume flag is not set, set it to whatever the eventsupplier ConsumeItems is set to.
                if (!item.EventItemConsume)
                    item.EventItemConsume = consumeItems;

                //Tag the item with the event tag.
                if (string.IsNullOrEmpty(item.Name))
                    item.Name = string.Format(m_NameFormat, CliLoc.LocToString(item.LabelNumber));
                else
                    item.Name = string.Format(m_NameFormat, item.Name);

                //Tag the item with the team name
                if (!string.IsNullOrEmpty(teamName))
                {
                    if (item is BaseWeapon)
                        ((BaseWeapon)item).IsRenamed = true;

                    item.Name = string.Format("{0} {1}", teamName, item.Name);
                }

                //Color the event item
                if (hue > 0)
                    item.Hue = hue;

                //Flag all the items in the containers
                if (item is BaseContainer)
                    MakeEventItems(item.Items.ToArray(), hue, teamName, allNewbied, stayEquipped, consumeItems);
            }
        }

        public static void RemoveEventGear(Mobile m)
        {
            if (m != null && !m.Deleted)
            {
                m.DropHolding();
                List<Item> toRemove = new List<Item>();

                for (int i = 0; i < m.Items.Count; ++i)
                    if (m.Items[i].EventItem)
                        toRemove.Add(m.Items[i]);

                if (m.Backpack != null)
                {
                    for (int i = 0; i < m.Backpack.Items.Count; ++i)
                        if (m.Backpack.Items[i].EventItem)
                            toRemove.Add(m.Backpack.Items[i]);
                }

                foreach (Item i in toRemove)
                    i.Delete();

                if (m is PlayerMobile)
                    ((PlayerMobile)m).EventType = EventType.NoEvent;
            }
        }

        public static void SupplyGear(Mobile m, EventSupplier eventSupplier)
        {
            //Supply the right type of gear
            SupplyType supplyType = eventSupplier.SupplyType;
            if (supplyType == SupplyType.MaxGear)
                SupplyMaxGear(m, eventSupplier.ClothHue, eventSupplier.GearHue, eventSupplier.TeamName, eventSupplier.NewbieAllItems, eventSupplier.StayEquipped, eventSupplier.ConsumeItems);
            else if (supplyType == SupplyType.RegularGear)
                SupplyRegGear(m, eventSupplier.ClothHue, eventSupplier.GearHue, eventSupplier.TeamName, eventSupplier.NewbieAllItems, eventSupplier.StayEquipped, eventSupplier.ConsumeItems);
            else if (supplyType == SupplyType.Custom)
            {
                if (!eventSupplier.CanUseCustomGear)
                    m.PublicOverheadMessage(MessageType.Regular, 906, true, "Custom setup error.");

                //Copy all the items in our bags and make them to event items
                for (int i = 0; i < eventSupplier.ItemContaiers.Length; i++)
                    if (eventSupplier.ItemContaiers[i].Items.Count > 0)
                        SupplyCustomItems(eventSupplier.ItemContaiers[i].Items, m, eventSupplier.ItemContaiers[i].AcceptsType,
                            eventSupplier.NewbieAllItems, eventSupplier.ClothHue, eventSupplier.GearHue, eventSupplier.TeamName,
                            eventSupplier.StayEquipped, eventSupplier.ConsumeItems);
            }

            
        }

        public static void SupplyGear(Mobile m, CWTelepad cwTelepad)
        {
            SupplyMaxGear(m, cwTelepad.ClothHue, cwTelepad.GearHue, cwTelepad.TeamName, cwTelepad.NewbieAllItems, false, false);
        }

        public static void SupplyGear(Mobile m, CustomGameEventSupplier eventSupplier)
        {
            //Supply the right type of gear
            SupplyType supplyType = eventSupplier.SupplyType;
            if (supplyType == SupplyType.MaxGear)
                SupplyMaxGear(m, eventSupplier.ClothHue, eventSupplier.GearHue, eventSupplier.TeamName, eventSupplier.NewbieAllItems, eventSupplier.StayEquipped, eventSupplier.ConsumeItems);
            else if (supplyType == SupplyType.RegularGear)
                SupplyRegGear(m, eventSupplier.ClothHue, eventSupplier.GearHue, eventSupplier.TeamName, eventSupplier.NewbieAllItems, eventSupplier.StayEquipped, eventSupplier.ConsumeItems);
            else if (supplyType == SupplyType.Custom)
            {
                if (!eventSupplier.CanUseCustomGear)
                    m.PublicOverheadMessage(MessageType.Regular, 906, true, "Custom setup error.");

                //Copy all the items in our bags and make them to event items
                for (int i = 0; i < eventSupplier.ItemContaiers.Length; i++)
                    if (eventSupplier.ItemContaiers[i].Items.Count > 0)
                        SupplyCustomItems(eventSupplier.ItemContaiers[i].Items, m, eventSupplier.ItemContaiers[i].AcceptsType, 
                            eventSupplier.NewbieAllItems, eventSupplier.ClothHue, eventSupplier.GearHue, eventSupplier.TeamName, 
                            eventSupplier.StayEquipped, eventSupplier.ConsumeItems);
            }
        }

        public static void SupplyRegGear(Mobile m)
        {
            SupplyRegGear(m, 0);
        }

        public static void SupplyRegGear(Mobile m, int hue)
        {
            SupplyRegGear(m, hue, hue);
        }

        public static void SupplyRegGear(Mobile m, int clothHue, int gearHue)
        {
            SupplyRegGear(m, clothHue, gearHue, string.Empty,true, false, false);
        }

        public static void SupplyRegGear(Mobile m, int clothHue, int gearHue, string teamName, bool newbie, bool staysEquipped, bool consumeItems )
        {
            SupplyMisc(m, newbie, consumeItems);
            SupplyCloths(m, clothHue, teamName, newbie);
            SupplyArmor(m, gearHue, teamName, newbie, staysEquipped);
            SupplyWeapons(m, gearHue, teamName, newbie, consumeItems);
        }

        public static void SupplyMaxGear(Mobile m)
        {
            SupplyMaxGear(m, 0);
        }

        public static void SupplyMaxGear(Mobile m, int hue)
        {
            SupplyMaxGear(m, hue, hue);
        }

        public static void SupplyMaxGear(Mobile m, int clothHue, int gearHue)
        {
            SupplyMaxGear(m, clothHue, gearHue, string.Empty,true, false, false);
        }

        public static void SupplyMaxGear(Mobile m, int clothHue, int gearHue, string teamName,  bool newbied, bool staysEquipped, bool consumeItems)
        {
            SupplyMisc(m, gearHue, teamName, newbied, consumeItems);
            SupplyCloths(m, DefaultClothList(), clothHue, teamName, newbied);
            SupplyWeapons(m, DefaultWeaponList(m), WeaponDamageLevel.Vanq, WeaponAccuracyLevel.Supremely, gearHue, teamName, newbied, consumeItems);
            SupplyArmor(m, DefaultArmorList(m), ArmorProtectionLevel.Invulnerability, gearHue, teamName, staysEquipped, newbied);
        }


        #region Givers

        public static void SupplyWeapons(Mobile m)
        {
            SupplyWeapons(m, 0);
        }

        public static void SupplyWeapons(Mobile m, int hue)
        {
            SupplyWeapons(m, DefaultWeaponList(m), WeaponDamageLevel.Regular, WeaponAccuracyLevel.Regular, hue, string.Empty, true, false);
        }

        public static void SupplyWeapons(Mobile m, int hue, string teamName, bool newbie, bool consumeItems)
        {
            SupplyWeapons(m, DefaultWeaponList(m), WeaponDamageLevel.Regular, WeaponAccuracyLevel.Regular, hue, teamName, newbie, consumeItems);
        }

        public static void SupplyWeapons(Mobile m, BaseWeapon[] weaponList, WeaponDamageLevel dmgLevel, WeaponAccuracyLevel accLevel, int hue, string teamName, bool newbie, bool consumeItems)
        {
            MakeEventItems(weaponList, hue, teamName, true, false, consumeItems);
            
            Backpack bp = new Backpack {EventItemConsume = consumeItems, EventItem = true, Name = "event backpack"};

            if (newbie)
                bp.LootType = LootType.Newbied;

            for (int i = 0; i < weaponList.Length; ++i)
            {
                BaseWeapon weapon = weaponList[i];
                
                if (newbie)
                    weapon.LootType = LootType.Newbied;

                weapon.EventItemConsume = consumeItems;

                weapon.DamageLevel = dmgLevel;
                weapon.AccuracyLevel = accLevel;

                bp.AddItem(weapon);
            }

            m.AddToBackpack(bp);
        }

        public static void SupplyArmor(Mobile m)
        {
            SupplyArmor(m, 0);
        }

        public static void SupplyArmor(Mobile m, int hue)
        {
            SupplyArmor(m, DefaultArmorList(m), ArmorProtectionLevel.Regular, hue, string.Empty, true, false);
        }

        public static void SupplyArmor(Mobile m, int hue, string teamName, bool newbie, bool staysEquipped)
        {
            SupplyArmor(m, DefaultArmorList(m), ArmorProtectionLevel.Regular, hue, teamName, newbie, staysEquipped);
        }

        public static void SupplyArmor(Mobile m, BaseArmor[] armorList, ArmorProtectionLevel protLevel, int hue, string teamName,  bool newbie, bool staysEquipped)
        {
            MakeEventItems(armorList, hue, teamName, true, staysEquipped, false);

            for (int i = 0; i < armorList.Length; ++i)
            {
                BaseArmor armor = armorList[i];

                if (newbie)
                    armor.LootType = LootType.Blessed;

                if (armor is BaseShield)
                {
                    armor.Movable = true;
                    armor.ProtectionLevel = ArmorProtectionLevel.Fortification;
                }
                else
                {
                    armor.Movable = !staysEquipped;
                    armor.ProtectionLevel = protLevel;
                }

                EquipItem(m, armor); //m.EquipItem(armorList[i]);
            }
        }

        public static void SupplyCloths(Mobile m)
        {
            SupplyCloths(m, 0, string.Empty, true);
        }

        public static void SupplyCloths(Mobile m, int hue, string teamName, bool newbie)
        {
            SupplyCloths(m, DefaultClothList(), 0, teamName, newbie);
        }

        public static void SupplyCloths(Mobile m, BaseClothing[] clothList, int hue, string teamName, bool newbie)
        {
            MakeEventItems(clothList, hue, teamName, true, true, false);

            for (int i = 0; i < clothList.Length; ++i)
            {
                Item item = clothList[i];

                if (newbie)
                    item.LootType = LootType.Newbied;

                EquipItem(m, item);
                item.Movable = false;
            }
        }

        public static void SupplyMisc(Mobile m, bool newbie, bool consumeItems)
        {
            SupplyMisc(m, 0, string.Empty, newbie, consumeItems);
        }

        public static void SupplyMisc(Mobile m, int hue)
        {
            SupplyMisc(m, hue, string.Empty, true, false);
        }

        public static void SupplyMisc(Mobile m, int hue, string teamName, bool newbie, bool consumeItems)
        {
            SupplyMisc(m, DefaultMiscList(), 0, string.Empty, newbie, consumeItems);
        }

        public static void SupplyMisc(Mobile m, Item[] miscList, int hue, string teamName, bool newbie, bool consumeItems)
        {
            MakeEventItems(miscList, hue, teamName, true, false, consumeItems);

            for (int i = 0; i < miscList.Length; ++i)
            {
                //newbie
                Item item = miscList[i];
                if (newbie)
                    item.LootType = LootType.Newbied;
                if (consumeItems)
                    item.EventItemConsume = true;

                m.AddToBackpack(item);
            }
        }

        public static void SupplyCustomItems(List<Item> itemList, Mobile toSupply, AcceptedTypes bagContains, bool newbieAllItems)
        {
            SupplyCustomItems(itemList, toSupply, bagContains, newbieAllItems, 0, 0, string.Empty, false, false);
        }

        public static void SupplyCustomItems(List<Item> itemList, Mobile toSupply, AcceptedTypes bagContains, bool newbieAllItems, int clothHue, int gearHue, string teamName, bool stayEquipped, bool consumeItems)
        {
            List<Item> itemsToGive = new List<Item>();

            for (int i = 0; i < itemList.Count; i++)
                MakeCopy(itemList[i], ref itemsToGive);

            if ( bagContains == AcceptedTypes.Cloths)
                MakeEventItems(itemsToGive.ToArray(), clothHue, teamName, false, stayEquipped, consumeItems);
            else
                MakeEventItems(itemsToGive.ToArray(), gearHue, teamName, false, false, consumeItems);

            //Drop regular items in the backpack, equip all others
            if (bagContains != AcceptedTypes.Others)
                for (int i = 0; i < itemsToGive.Count; ++i)
                    EquipItem(toSupply, itemsToGive[i]);
            else
                for (int i = 0; i < itemsToGive.Count; ++i)
                    if ( itemsToGive[i].Parent == null) //Only add the item to our pack if it is not in a container
                        toSupply.AddToBackpack(itemsToGive[i]);
        }

        private static void EquipItem(Mobile m, Item i)
        {
            if (m.FindItemOnLayer(i.Layer) != null)
                m.AddToBackpack(m.FindItemOnLayer(i.Layer));

            m.EquipItem(i);
        }

        #endregion

        #region Dupe Code

        private static void MakeCopy(Item itemToCopy, ref List<Item> itemList)
        {
            MakeCopy(itemToCopy, ref itemList, null);
        }

        private static void MakeCopy(Item toCopy, ref List<Item> itemList, BaseContainer parentCont)
        {
            Item newItem = null;
            

            try
            {
                newItem = (Item) Activator.CreateInstance(toCopy.GetType());
            }
            catch
            {
                Console.WriteLine("SupplySystem error, cannot create {0}, no parameterless constructor defined?", toCopy.GetType());
            }

            if (newItem == null)
                return;

            //Copy all the "item" related properties
            CopyBaseProperties(toCopy, newItem);

            if (newItem is Container)
            {
                Container newContainer = ((Container)newItem);

                // If a container gets duplicated it needs to get emptied.
                // ("BagOfReagents" problem, items which create content in the constructors have unwanted
                // contents after duping under certain circumstances.)
                Item[] found;

                //Delete all the items from the constructor
                found = newContainer.FindItemsByType(typeof(Item), false);
                for (int j = 0; j < found.Length; j++)
                    found[j].Delete();

                //Add the items from the orginal bag.
                found = ((Container)toCopy).FindItemsByType(typeof(Item), false);
                for (int i = 0; i < found.Length; i++)
                    MakeCopy(found[i], ref itemList, (BaseContainer)newItem);

            }
            else if (newItem is BaseWeapon)
                CopyProperties((BaseWeapon)toCopy, (BaseWeapon)newItem);
            else if (newItem is BaseArmor)
                CopyProperties((BaseArmor)toCopy, (BaseArmor)newItem);
            else if (newItem is BaseClothing)
                CopyProperties((BaseClothing)toCopy, (BaseClothing)newItem);
            else if (toCopy is Spellbook && newItem is Spellbook)
            {
                Spellbook bookToCopy = toCopy as Spellbook;
                Spellbook newBook = newItem as Spellbook;

                newBook.Content = bookToCopy.Content;
            }

            if (parentCont != null)
                parentCont.DropItem(newItem);

            itemList.Add(newItem);

        }


        private static void CopyBaseProperties(Item toCopy, Item newItem)
        {
            newItem.Amount = toCopy.Amount;
            newItem.Hue = toCopy.Hue;
            newItem.ItemID = toCopy.ItemID; //in the event of someone changing the id
            newItem.Layer = toCopy.Layer;
            newItem.LootType = toCopy.LootType;
            //Map not included
            newItem.Movable = toCopy.Movable;
            newItem.Name = toCopy.Name;
            newItem.QuestItem = toCopy.QuestItem;
            newItem.Stackable = toCopy.Stackable;
            newItem.Weight = toCopy.Weight;
            //Visible is not included
            //X, Y, Z not included
        }

        private static void CopyProperties(BaseWeapon toCopy, BaseWeapon newItem)
        {
            newItem.AccuracyLevel = toCopy.AccuracyLevel;
            newItem.Animation = toCopy.Animation;
            newItem.HitSound = toCopy.HitSound;
            newItem.MissSound = toCopy.MissSound;
            //AosElementDamage
            //Attributes
            //Consecrated
            newItem.Crafter = toCopy.Crafter;
            newItem.Cursed = toCopy.Cursed;
            newItem.DamageLevel = toCopy.DamageLevel;
            newItem.DexRequirement = toCopy.DexRequirement;
            newItem.DurabilityLevel = toCopy.DurabilityLevel;
            newItem.MaxHitPoints = toCopy.MaxHitPoints;
            newItem.HitPoints = toCopy.HitPoints;
            //Stat requirements
            newItem.IsRenamed = toCopy.IsRenamed;
            newItem.MaxDamage = toCopy.MaxDamage;
            newItem.MaxRange = toCopy.MaxRange;
            newItem.MinDamage = toCopy.MinDamage;
            newItem.PlayerConstructed = toCopy.PlayerConstructed;
            newItem.Quality = toCopy.Quality;
            newItem.Resource = toCopy.Resource;
            newItem.Hue = toCopy.Hue;
            newItem.Skill = toCopy.Skill;
            newItem.Speed = toCopy.Speed;
            newItem.Type = toCopy.Type;
        }

        private static void CopyProperties(BaseArmor toCopy, BaseArmor newItem)
        {
            //AoSAttributes
            newItem.BaseArmorRating = toCopy.BaseArmorRating;
            //Bodyposition
            newItem.Crafter = toCopy.Crafter;
            //Bonus attributes
            //Stat requirements
            newItem.MaxHitPoints = toCopy.MaxHitPoints;
            newItem.HitPoints = toCopy.HitPoints;
            newItem.IsRenamed = toCopy.IsRenamed;
            newItem.PlayerConstructed = toCopy.PlayerConstructed;
            newItem.ProtectionLevel = toCopy.ProtectionLevel;
            newItem.Quality = toCopy.Quality;
            newItem.Resource = toCopy.Resource;
            newItem.Hue = toCopy.Hue;
        }

        private static void CopyProperties(BaseClothing toCopy, BaseClothing newItem)
        {
            //AoSAttributes
            newItem.Crafter = toCopy.Crafter;
            newItem.MaxHitPoints = toCopy.MaxHitPoints;
            newItem.HitPoints = toCopy.HitPoints;
            newItem.IsRenamed = toCopy.IsRenamed;
            newItem.PlayerConstructed = toCopy.PlayerConstructed;
            newItem.Quality = toCopy.Quality;
            //Resistances
            newItem.Resource = toCopy.Resource;
            newItem.Hue = toCopy.Hue;
            //Stat requirements
        }

        #endregion
    }
}