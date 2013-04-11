using System;
using Server.Multis;
using Server.Network;
using Server.Prompts;
using Server.Targeting;

namespace Server.Items
{
    public enum KeyType
    {
        Copper = 0x100E,
        Gold = 0x100F,
        Iron = 0x1010,
        Rusty = 0x1013
    }

    public interface ILockable
    {
        bool Locked { get; set; }
        uint KeyValue { get; set; }
    }

    public class Key : Item
    {
        private string m_Description;
        private uint m_KeyVal;
        private Item m_Link;
        private int m_MaxRange;

        public static uint RandomValue()
        {
            return (uint)(0xFFFFFFFE * Utility.RandomDouble()) + 1;
        }

        public static void RemoveKeys(Mobile m, uint keyValue)
        {
            if (keyValue == 0)
                return;

            RemoveKeys(m.Backpack, keyValue);
            RemoveKeys(m.BankBox, keyValue);
        }

        public static void RemoveKeys(Container cont, uint keyValue)
        {
            if (cont == null || keyValue == 0)
                return;

            Item[] items = cont.FindItemsByType(new Type[] { typeof(Key), typeof(KeyRing) });

            foreach (Item item in items)
            {
                if (item is Key)
                {
                    Key key = (Key)item;

                    if (key.KeyValue == keyValue)
                        key.Delete();
                }
                else
                {
                    KeyRing keyRing = (KeyRing)item;

                    keyRing.RemoveKeys(keyValue);
                }
            }
        }

        public static bool ContainsKey(Container cont, uint keyValue)
        {
            if (cont == null)
                return false;

            Item[] items = cont.FindItemsByType(new Type[] { typeof(Key), typeof(KeyRing) });

            foreach (Item item in items)
            {
                if (item is Key)
                {
                    Key key = (Key)item;

                    if (key.KeyValue == keyValue)
                        return true;
                }
                else
                {
                    KeyRing keyRing = (KeyRing)item;

                    if (keyRing.ContainsKey(keyValue))
                        return true;
                }
            }

            return false;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Description
        {
            get
            {
                return m_Description;
            }
            set
            {
                m_Description = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxRange
        {
            get
            {
                return m_MaxRange;
            }

            set
            {
                m_MaxRange = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public uint KeyValue
        {
            get
            {
                return m_KeyVal;
            }

            set
            {
                m_KeyVal = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Item Link
        {
            get
            {
                return m_Link;
            }

            set
            {
                m_Link = value;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(2); // version

            writer.Write(m_MaxRange);

            writer.Write(m_Link);

            writer.Write(m_Description);
            writer.Write(m_KeyVal);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        m_MaxRange = reader.ReadInt();

                        goto case 1;
                    }
                case 1:
                    {
                        m_Link = reader.ReadItem();

                        goto case 0;
                    }
                case 0:
                    {
                        if (version < 2 || m_MaxRange == 0)
                            m_MaxRange = 3;

                        m_Description = reader.ReadString();

                        m_KeyVal = reader.ReadUInt();

                        break;
                    }
            }
        }

        [Constructable]
        public Key()
            : this(KeyType.Iron, 0)
        {
        }

        [Constructable]
        public Key(KeyType type)
            : this(type, 0)
        {
        }

        [Constructable]
        public Key(uint val)
            : this(KeyType.Iron, val)
        {
        }

        [Constructable]
        public Key(KeyType type, uint LockVal)
            : this(type, LockVal, null)
        {
        }

        public Key(KeyType type, uint LockVal, Item link)
            : base((int)type)
        {
            Weight = 1.0;

            m_MaxRange = 3;
            m_KeyVal = LockVal;
            m_Link = link;
        }

        public Key(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(501661); // That key is unreachable.
                return;
            }

            Target t;

            if (m_KeyVal != 0)
            {
                from.SendAsciiMessage("Select the item to use the key on.");
                t = new UnlockTarget(this);
            }
            else
            {
                from.SendAsciiMessage("This key is a key blank. Which key would you like to make a copy of?");
                t = new CopyTarget(this);
            }

            from.Target = t;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            string desc;

            if (m_KeyVal == 0)
                desc = "(blank)";
            else if ((desc = m_Description) == null || (desc = desc.Trim()).Length <= 0)
                desc = null;

            if (desc != null)
                list.Add(desc);
        }

        public override void OnSingleClick(Mobile from)
        {
            string desc = string.Empty;

            if (Description != null)
                desc = Description.Trim();

            if (string.IsNullOrEmpty(desc) || m_KeyVal == 0)
                base.OnSingleClick(from);
            else
                LabelTo(from, string.Format("Key to: {0}", desc));
        }

        public bool UseOn(Mobile from, ILockable o)
        {
            if (o.KeyValue == KeyValue)
            {
                if (o is BaseDoor && !((BaseDoor)o).UseLocks())
                {
                    from.SendAsciiMessage("This door does not use locks");
                    return false;
                }

                o.Locked = !o.Locked;

                if (o is LockableContainer)
                {
                    LockableContainer cont = (LockableContainer)o;

                    if (cont.LockLevel == -255)
                        cont.LockLevel = cont.RequiredSkill - 10;
                }

                if (o is Item)
                {
                    Item item = (Item)o;

                    if (o.Locked)
                        item.SendLocalizedMessageTo(from, 1048000); // You lock it.
                    else
                        item.SendLocalizedMessageTo(from, 1048001); // You unlock it.

                    from.PlaySound(73);

                    if (item is LockableContainer)
                    {
                        LockableContainer cont = (LockableContainer)item;

                        if (cont.TrapType != TrapType.None && cont.TrapOnLockpick)
                        {
                            if (o.Locked)
                                item.SendLocalizedMessageTo(from, 501673); // You re-enable the trap.
                            else
                                item.SendLocalizedMessageTo(from, 501672); // You disable the trap temporarily.  Lock it again to re-enable it.
                        }
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private class RenamePrompt : Prompt
        {
            private readonly Key m_Key;

            public RenamePrompt(Key key)
            {
                m_Key = key;
            }

            public override void OnResponse(Mobile from, string text)
            {
                if (m_Key.Deleted || !m_Key.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(501661); // That key is unreachable.
                    return;
                }

                m_Key.Description = Utility.FixHtml(text);
            }
        }

        private class UnlockTarget : Target
        {
            private readonly Key m_Key;

            public UnlockTarget(Key key)
                : base(key.MaxRange, false, TargetFlags.None)
            {
                m_Key = key;
                CheckLOS = false;
            }

            protected override void OnTargetNotAccessible(Mobile from, object targeted)
            {
                OnTarget(from, targeted);
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Key.Deleted || !m_Key.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(501661); // That key is unreachable.
                    return;
                }
                if (targeted == m_Key)
                {
                    from.SendLocalizedMessage(501665); // Enter a description for this key.
                    from.Prompt = new RenamePrompt(m_Key);
                }
                else if (!from.InRange(targeted, 3) || !from.InLOS(targeted))
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                else if (targeted is Item)
                {
                    Item target = (Item) targeted;
                    Region itemRegion = Region.Find(target.Location, target.Map);

                    if (itemRegion is Regions.HouseRegion)
                    {
                        BaseHouse house = ((Regions.HouseRegion) itemRegion).House;

                        if (house == null || !from.Alive || house.Deleted)
                            return;
                        if (house.HouseKeyVal == m_Key.KeyValue)
                        {
                            if (target.RootParent != null)
                            {
                                from.SendAsciiMessage("You can not lock that down");
                                return;
                            }
                            if (target is BaseDoor)
                            {
                                if (m_Key.UseOn(from, (ILockable) targeted))
                                    return;

                                from.SendAsciiMessage("The key does not fit that lock");
                                return;
                            }
                            if (!target.Movable && !house.IsLockedDown(target))
                            {
                                from.SendAsciiMessage("You can't unlock that!");
                                return;
                            }
                            if (m_Key.OnHouseItemTarget(from, target, ((Regions.HouseRegion) itemRegion).House))
                                return;
                        }
                    }
                    if (target is ILockable)
                    {
                        if (m_Key.UseOn(from, (ILockable)target))
                            return;

                        from.SendAsciiMessage("The key does not fit that lock");
                        return;
                    }
                    if (itemRegion is Regions.HouseRegion)
                    {
                        from.SendAsciiMessage(((Regions.HouseRegion) itemRegion).House != null ? "You must use the house key to lock down or unlock items." : "That does not have a lock.");
                        return;
                    }
                }
                else
                    from.SendAsciiMessage("You can't use a key on that!");
                /*
                if (targeted is ILockable && m_Key.UseOn(from, (ILockable)targeted))
                        number = -1;
                    else
                    {
                        Item target = (Item)targeted;
                        BaseHouse house = BaseHouse.FindHouseAt(from);

                        if (target.RootParent == null)
                        {
                            if (house != null )
                            {
                                m_Key.OnHouseItemTarget(from, target, house);
                                return;
                            }
                        }

                        from.SendAsciiMessage("That does not have a lock.");
                        number = -1;
                    }
                }
                else
                {
                    number = 501666; // You can't unlock that!
                }
                if (number != -1)
                    from.SendLocalizedMessage(number);*/
            }
        }

        private void HouseContainerUnlock(Mobile from, Container targeted, BaseHouse house)
        {
            //Release all items in the containers and the sub containers
            foreach (Item i in targeted.Items)
                if (i is Container)
                    HouseContainerUnlock(from, (Container)i, house);
                else
                    house.Release(from, i);
        }

        public bool OnHouseItemTarget(Mobile from, Item targeted, BaseHouse house)
        {
            if (targeted == null || house == null || !from.Alive || house.Deleted)
                return false;

            if (house.HouseKeyVal != KeyValue)
            {
                from.SendAsciiMessage("You must use the house key to lock down items.");
                return false;
            }
            if (house.IsLockedDown(targeted))
            {
                house.Release(from, targeted);
                from.SendAsciiMessage("The item has been unlocked from the structure.");
                return true;
            }
            if (house.LockDown(from, targeted))
            {
                from.SendAsciiMessage("The item has been locked down.");
                return true;
            }
            
            return false;
        }

        private class CopyTarget : Target
        {
            private readonly Key m_Key;

            public CopyTarget(Key key)
                : base(3, false, TargetFlags.None)
            {
                m_Key = key;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Key.Deleted || !m_Key.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(501661); // That key is unreachable.
                    return;
                }

                int number;

                if (targeted is Key)
                {
                    Key k = (Key)targeted;

                    if (k.m_KeyVal == 0)
                    {
                        number = 501675; // This key is also blank.
                    }
                    else if (from.CheckTargetSkill(SkillName.Tinkering, k, 0, 75.0))
                    {
                        number = 501676; // You make a copy of the key.

                        m_Key.Description = k.Description;
                        m_Key.KeyValue = k.KeyValue;
                        m_Key.Link = k.Link;
                        m_Key.MaxRange = k.MaxRange;
                    }
                    else if (Utility.RandomDouble() <= 0.1) // 10% chance to destroy the key
                    {
                        from.SendLocalizedMessage(501677); // You fail to make a copy of the key.

                        number = 501678; // The key was destroyed in the attempt.

                        m_Key.Delete();
                    }
                    else
                    {
                        number = 501677; // You fail to make a copy of the key.
                    }
                }
                else
                {
                    number = 501688; // Not a key.
                }

                from.SendLocalizedMessage(number);
            }
        }
    }
}