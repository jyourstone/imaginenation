using System;
using System.Reflection;
using Server.Commands;
using Server.Items;
using Server.Targeting;

namespace Server.Scripts.Commands
{
    public class Dupe
    {
        public static void Initialize()
        {
            CommandSystem.Register("Dupe", AccessLevel.GameMaster, Dupe_OnCommand);
            CommandSystem.Register("DupeInBag", AccessLevel.GameMaster, DupeInBag_OnCommand);
        }

        [Usage("Dupe [amount]")]
        [Description("Dupes a targeted item.")]
        private static void Dupe_OnCommand(CommandEventArgs e)
        {
            int amount = 1;
            if (e.Length >= 1)
                amount = e.GetInt32(0);
            e.Mobile.Target = new DupeTarget(false, amount > 0 ? amount : 1);
            e.Mobile.SendMessage("What do you wish to dupe?");
        }

        [Usage("DupeInBag <count>")]
        [Description("Dupes an item at it's current location (count) number of times.")]
        private static void DupeInBag_OnCommand(CommandEventArgs e)
        {
            int amount = 1;
            if (e.Length >= 1)
                amount = e.GetInt32(0);

            e.Mobile.Target = new DupeTarget(true, amount > 0 ? amount : 1);
            e.Mobile.SendMessage("What do you wish to dupe?");
        }

        private class DupeTarget : Target
        {
            private readonly bool m_InBag;
            private readonly int m_Amount;

            public DupeTarget(bool inbag, int amount)
                : base(15, false, TargetFlags.None)
            {
                m_InBag = inbag;
                m_Amount = amount;
            }

            protected override void OnTarget(Mobile from, object targ)
            {
                bool done = false;

                if (!(targ is Item))
                {
                    from.SendMessage("You can only dupe items.");
                    return;
                }

                if (targ is Container)
                {
                    int Total_ItemCount = m_Amount * ((Container)targ).TotalItems;

                    if (Total_ItemCount > 1000)
                    {
                        from.SendMessage("You are trying to create {0} new items on the server!\nSafety Limit is 1000.", Total_ItemCount);
                        return;
                    }
                }

                CommandLogging.WriteLine(from, "{0} {1} duping {2} (inBag={3}; amount={4})", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(targ), m_InBag, m_Amount);

                Item copy = (Item)targ;
                Container pack;

                if (m_InBag)
                {
                    if (copy.Parent is Container)
                        pack = (Container)copy.Parent;
                    else if (copy.Parent is Mobile)
                        pack = ((Mobile)copy.Parent).Backpack;
                    else
                        pack = null;
                }
                else
                    pack = from.Backpack;

                try
                {
                    from.SendMessage("Duping {0}...", m_Amount);

                    for (int i = 0; i < m_Amount; i++)
                    {
                        InternalDupe((Item)targ, pack, from);
                    }

                    from.SendMessage("Done");
                    done = true;
                }
                catch
                {
                    from.SendMessage("Error!");
                    return;
                }

                if (!done)
                {
                    from.SendMessage("Unable to dupe. Item must have a 0 parameter constructor.");
                }
            }


            public static void CopyProperties(Item dest, Item src)
            {
                PropertyInfo[] props = src.GetType().GetProperties();

                for (int i = 0; i < props.Length; i++)
                {
                    try
                    {
                        if (props[i].CanRead && props[i].CanWrite)
                        {

                            // These properties must not be copied during the dupe, they get set implicitely by placing
                            // items properly using "DropItem()" etc. .
                            switch (props[i].Name)
                            {
                                case "Parent":
                                case "TotalWeight":
                                case "TotalItems":
                                case "TotalGold":
                                    break;
                                default:
                                    props[i].SetValue(dest, props[i].GetValue(src, null), null);
                                    break;
                            }
                            // end exceptions 
                        }
                    }
                    catch
                    {
                        //Console.WriteLine( "Denied" );
                    }

                    // BaseArmor, BaseClothing, BaseJewel, BaseWeapon: copy nested classes
                    // ToDo: If someone knows something about dynamic casting these 4 blocks
                    //       could be integrated into one...
                    if (src is BaseWeapon)
                    {
                        object src_obj = ((BaseWeapon)src).Attributes;
                        object dest_obj = ((BaseWeapon)dest).Attributes;

                        if (src_obj != null && dest_obj != null)
                            CopyProperties(dest_obj, src_obj);

                        src_obj = ((BaseWeapon)src).SkillBonuses;
                        dest_obj = ((BaseWeapon)dest).SkillBonuses;

                        if (src_obj != null && dest_obj != null)
                            CopyProperties(dest_obj, src_obj);

                        src_obj = ((BaseWeapon)src).WeaponAttributes;
                        dest_obj = ((BaseWeapon)dest).WeaponAttributes;

                        if (src_obj != null && dest_obj != null)
                            CopyProperties(dest_obj, src_obj);

                        src_obj = ((BaseWeapon)src).AosElementDamages;
                        dest_obj = ((BaseWeapon)dest).AosElementDamages;

                        if (src_obj != null && dest_obj != null)
                            CopyProperties(dest_obj, src_obj);
                    }
                    else if (src is BaseArmor)
                    {
                        object src_obj = ((BaseArmor)src).Attributes;
                        object dest_obj = ((BaseArmor)dest).Attributes;

                        if (src_obj != null && dest_obj != null)
                            CopyProperties(dest_obj, src_obj);

                        src_obj = ((BaseArmor)src).SkillBonuses;
                        dest_obj = ((BaseArmor)dest).SkillBonuses;

                        if (src_obj != null && dest_obj != null)
                            CopyProperties(dest_obj, src_obj);

                        src_obj = ((BaseArmor)src).ArmorAttributes;
                        dest_obj = ((BaseArmor)dest).ArmorAttributes;

                        if (src_obj != null && dest_obj != null)
                            CopyProperties(dest_obj, src_obj);
                    }
                    else if (src is BaseJewel)
                    {
                        object src_obj = ((BaseJewel)src).Attributes;
                        object dest_obj = ((BaseJewel)dest).Attributes;

                        if (src_obj != null && dest_obj != null)
                            CopyProperties(dest_obj, src_obj);

                        src_obj = ((BaseJewel)src).SkillBonuses;
                        dest_obj = ((BaseJewel)dest).SkillBonuses;

                        if (src_obj != null && dest_obj != null)
                            CopyProperties(dest_obj, src_obj);

                        src_obj = ((BaseJewel)src).Resistances;
                        dest_obj = ((BaseJewel)dest).Resistances;

                        if (src_obj != null && dest_obj != null)
                            CopyProperties(dest_obj, src_obj);

                    }
                    else if (src is BaseClothing)
                    {
                        object src_obj = ((BaseClothing)src).Attributes;
                        object dest_obj = ((BaseClothing)dest).Attributes;

                        if (src_obj != null && dest_obj != null)
                            CopyProperties(dest_obj, src_obj);

                        src_obj = ((BaseClothing)src).SkillBonuses;
                        dest_obj = ((BaseClothing)dest).SkillBonuses;

                        if (src_obj != null && dest_obj != null)
                            CopyProperties(dest_obj, src_obj);

                        src_obj = ((BaseClothing)src).ClothingAttributes;
                        dest_obj = ((BaseClothing)dest).ClothingAttributes;

                        if (src_obj != null && dest_obj != null)
                            CopyProperties(dest_obj, src_obj);

                        src_obj = ((BaseClothing)src).Resistances;
                        dest_obj = ((BaseClothing)dest).Resistances;

                        if (src_obj != null && dest_obj != null)
                            CopyProperties(dest_obj, src_obj);

                    }
                    // end copying nested classes

                }
            }


            private static void CopyProperties(object dest, object src)
            {
                PropertyInfo[] props = src.GetType().GetProperties();

                for (int i = 0; i < props.Length; i++)
                {
                    try
                    {
                        if (props[i].CanRead && props[i].CanWrite)
                        {
                            props[i].SetValue(dest, props[i].GetValue(src, null), null);
                        }
                    }
                    catch
                    {
                        //Console.WriteLine( "Denied" );
                    }
                }
            }

            private static void InternalDupe(Item src, Container pack, Mobile from)
            {

                Type t = src.GetType();
                ConstructorInfo[] info = t.GetConstructors();

                foreach (ConstructorInfo c in info)
                {
                    ParameterInfo[] paramInfo = c.GetParameters();

                    if (paramInfo.Length == 0)
                    {
                        object[] objParams = new object[0];

                        object o = c.Invoke(objParams);

                        if (o != null && o is Item)
                        {
                            Item newItem = (Item)o;

                            CommandLogging.WriteLine(from, "{0} {1} duping successfull, new ItemID {2}", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(newItem));

                            if (newItem is Container)
                            {

                                // If a container gets duplicated it needs to get emptied.
                                // ("BagOfReagents" problem, items which create content in the constructors have unwanted
                                // contents after duping under certain circumstances.)
                                Item[] found;

                                found = ((Container)newItem).FindItemsByType(typeof(Item), false);

                                for (int j = 0; j < found.Length; j++)
                                {
                                    found[j].Delete();
                                }
                                // end emptying

                                Item[] items = ((Container)src).FindItemsByType(typeof(Item), false);

                                for (int i = 0; i < items.Length; i++)
                                {
                                    CommandLogging.WriteLine(from, "{0} {1} duping child {2} of {3}", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(items[i]), CommandLogging.Format(src));
                                    InternalDupe(items[i], (Container)newItem, from);
                                }
                            }

                            CopyProperties(newItem, src);

                            if (pack != null)
                                pack.DropItem(newItem);
                            else
                                newItem.MoveToWorld(from.Location, from.Map);

                            newItem.InvalidateProperties();
                        }
                        else
                        {
                            from.SendMessage("Target is not a valid item anymore or vanished before the dupe.");
                        }
                    }
                }

            }

        }
    }

}