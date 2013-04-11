using Server.Targeting;

namespace Server.Items
{
    public class TreasureGemRandom : Item
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
        public static int[] TypeGem = new int[]
                    {
                        0x1ED0,0x14F0,0x14F1,0x14f2,0x14ef,0xefa,0xefb,0xefc,0xefd,0xefe,0xeff,0xf00,0xf01,0xf02,0xf03,0xf04,0xe75,0xe76,0xe79,0xe7a,0xe7d,0xe7e,0xe7f,0xe80,0xe42,0xe38,0xe39,0xe3a,0xe3c,0xe3d,0xe3e,0xe3f,0xdf8,0xdf9,0x103b,0x103c,0x103f,0x1040,0x1041,0x1042,0xff2,0xff1,0xff0,0xfef,0xff6,0xff8,0x1f1c,0x1f1d,0x1f1e,0x1f19,0x1f1a,0x1f1b,0xe21,0xe22,0xe23,0xe1f,0xe20,0xe1e,0xe1d,0x1bdd,0x102e,0x1030,0x1bd7,0x1032

                    };
        [Constructable]
        public TreasureGemRandom() : base(Utility.RandomList(TypeGem))
		{
            Weight = 1.0;
            Name = "Treasure gem ";
            Hue = Utility.RandomList(Sphere.RareHues);
            LootType = LootType.Regular;
		}

        public TreasureGemRandom(Serial serial) : base(serial)
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
            private readonly TreasureGemRandom m_gem;

            public InternalTarget(TreasureGemRandom gem)
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
