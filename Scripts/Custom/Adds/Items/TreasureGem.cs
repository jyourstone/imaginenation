using Server.Targeting;

namespace Server.Items
{
    public class TreasureGem : Item
    {
        private Item m_TreasureItem;

        [CommandProperty(AccessLevel.GameMaster)]                                                                                                                                                                                                                                                                                                                                                                                                   
        public Item TreasureItem
        {
            get { return m_TreasureItem; }
            set {
                if (m_TreasureItem != null && value == null)
                    m_TreasureItem.Delete();
                m_TreasureItem = value;
                if (m_TreasureItem != null)
                    m_TreasureItem.Internalize();
            }
        }

        [Constructable]
        public TreasureGem() : base(0x1ED0)
		{
            Weight = 1.0;
            Name = "Treasure gem";
            Hue = Utility.RandomList(Sphere.RareHues);
            LootType = LootType.Regular;
		}

        public TreasureGem(Serial serial) : base(serial)
        {
        }

        public override void OnSingleClick(Mobile from)
        {
            if (from.AccessLevel > AccessLevel.Player && m_TreasureItem != null)
            {
                string item = m_TreasureItem.GetType().Name;
                if (m_TreasureItem.Amount > 1)
                    item += "(" + TreasureItem.Amount + ")";
                LabelTo(from, "Treasure gem [{0}]", item); 
            }
            else
                LabelTo(from, "Treasure gem"); 
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (m_TreasureItem == null && from.AccessLevel > AccessLevel.Player)
            {
                from.SendAsciiMessage("Target item to add to treasure gem.");
                from.Target = new InternalTarget(this);
                return;
            }
            if (from.Backpack == null)
            {
                from.SendAsciiMessage("You can't use this if you don't have a backpack.");
                return;
            }
            if (!IsChildOf(from.Backpack))
            {
                from.SendAsciiMessage("This item must be in your backpack when used.");
                return;
            }
            if (TreasureItem == null)
            {
                from.SendAsciiMessage("This gem contains no treasure.");
                Delete();
                return;
            }
            if (TreasureItem.Deleted)
            {
                from.SendAsciiMessage("Item in this gem does not exist. Please page a GM.");
                return;
            }
            if ((m_TreasureItem.Weight * m_TreasureItem.Amount + from.TotalWeight) > from.MaxWeight && from.AccessLevel < AccessLevel.GameMaster)
            {
                from.SendAsciiMessage("You are carrying too much weight to get items out of this treasure gem");
                return;
            }
            from.AddToBackpack(m_TreasureItem);
            from.SendAsciiMessage("Treasure gem turns into something else as you touch it!");
            m_TreasureItem = null;
            Delete();
        }

        public override void OnDelete()
        {
            if (m_TreasureItem != null)
                m_TreasureItem.Delete();

            base.OnDelete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version

            writer.Write(m_TreasureItem);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_TreasureItem = reader.ReadItem();
                        break;
                    }
            }
        }

        public bool SetTreasure(Item item)
        {
            InvalidateProperties();

            if (item != null && item.Movable)
            {
                m_TreasureItem = item;
                item.Internalize();
                InvalidateProperties();

                return true;
            }
                return false;
        }
        private class InternalTarget : Target
        {
            private readonly TreasureGem m_gem;

            public InternalTarget(TreasureGem gem)
                : base(10, false, TargetFlags.None)
            {
                m_gem = gem;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_gem.Deleted)
                    return;

                string message = "";

                if (m_gem.TreasureItem != null)
                {
                    message = "This gem already has an item in it.";
                }
                else if (targeted is ICommodity)
                {
                    message = "This item can only be added to treasure gem in a container or as a commodity deed.";
                }
                else if (targeted is Item && targeted != this && !(targeted is ICommodity))
                {
                    if (m_gem.SetTreasure((Item)targeted))
                    {
                        Item item = (Item)targeted;
                        if (item.Amount > 1)
                        {
                            message = item.Amount + " ";
                        }
                        message += item.GetType().Name + "s added to treasure gem.";
                    }
                    else
                        message = "That is not a valid item.";
                }
                else
                    message = "That is not a valid item.";
                from.SendAsciiMessage(message);
            }
        }
    }
}
