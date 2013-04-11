//TODO: Add logging of all players entering and logging of all items that are added to the bags.
//Also fix so that you cannot "drag" items from the bags, you will have to ".remove" them.

using Server.Custom.Games;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using SS = Server.SupplySystem;
using Server.Scripts.Custom.Adds.System;
using System;

namespace Server.Items
{
    public enum SupplyType
    {
        MaxGear,
        RegularGear,
        Custom
    }

    public class EventSupplier : Item
    {
        private EventItemContainer[] m_ItemContaiers;
        private SupplyType m_SupplyType = SupplyType.MaxGear;
        private bool m_NewbieAllItems = true;
        private bool m_StayEquipped = false;

        private string m_TeamName = "";
        private int m_GearHue = 2;
        private int m_ClothHue = 2;

        public EventSupplier()
            : base(0x17e5)
        {
            Movable = false;
            Name = "Event Supplier";
            Light = LightType.Circle300;

            TryCreateMissingBags();
        }

        public EventSupplier(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool StayEquipped
        {
            get { return m_StayEquipped; }
            set { m_StayEquipped = false; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool NewbieAllItems
        {
            get { return m_NewbieAllItems; }
            set { m_NewbieAllItems = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SupplyType SupplyType
        {
            get { return m_SupplyType; }
            set { m_SupplyType = value; }
        }

        public EventItemContainer[] ItemContaiers
        {
            get { return m_ItemContaiers; }
            set { m_ItemContaiers = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ClothHue
        {
            get { return m_ClothHue; }
            set
            {
                m_ClothHue = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int GearHue
        {
            get { return m_GearHue; }
            set { m_GearHue = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string TeamName
        {
            get { return m_TeamName; }
            set { m_TeamName = value; }
        }

        public void AddCustomItem(Item item)
        {
            if (SupplyType == SupplyType.Custom)
            {
                TryCreateMissingBags();
                TryMoveBags();

                foreach (EventItemContainer container in m_ItemContaiers)
                {
                    if (container.AcceptsItem(item))
                    {
                        container.DropItem(item);
                    }
                }
            }
        }

        public void RemoveCustomItem(Item item)
        {
            if (SupplyType == SupplyType.Custom)
            {
                TryCreateMissingBags();
                TryMoveBags();

                foreach (EventItemContainer container in m_ItemContaiers)
                {
                    if (container.AcceptsItem(item))
                    {
                        container.RemoveItem(item);
                    }
                }
            }
        }


        [CommandProperty(AccessLevel.GameMaster)]
        public bool ConsumeItems { get; set; }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel > AccessLevel.Counselor)
            {
                TryCreateMissingBags();

                TryMoveBags();

                if (from.HasGump(typeof(ConfigSelectionGump)))
                {
                    for (int i = 0; i < m_ItemContaiers.Length; i++)
                        m_ItemContaiers[i].DisplayTo(from);

                    from.CloseGump(typeof(ConfigSelectionGump));
                }
                else if (m_SupplyType == SupplyType.Custom)
                {
                    from.SendAsciiMessage("Double click the pad again to open all bags at once.");
                    from.SendGump(new ConfigSelectionGump(from, this));
                }
                else
                    from.SendAsciiMessage("You need to turn the pad on to custom mode to customize the gear.");
            }
        }

        public EquipmentStorage Supply(Mobile m)
        {
            EquipmentStorage playerGear = new EquipmentStorage(m);
            playerGear.StoreEquip();

            //Supply the right type of gear
            SupplySystem.SupplyGear(m, this);
            return playerGear;
        }

        public void TryCreateMissingBags()
        {
            if (m_ItemContaiers == null)
            {
                m_ItemContaiers = new EventItemContainer[] { new EventItemContainer(AcceptedTypes.Armors), new EventItemContainer(AcceptedTypes.Weapons), 
                    new EventItemContainer(AcceptedTypes.Cloths), new EventItemContainer(AcceptedTypes.Others) };
            }
            else
            {
                for (int i = 0; i < m_ItemContaiers.Length; i++)
                {
                    if (m_ItemContaiers[i] == null || m_ItemContaiers[i].Deleted)
                        m_ItemContaiers[i] = new EventItemContainer((AcceptedTypes)i);

                    m_ItemContaiers[i].Visible = false;
                    m_ItemContaiers[i].Movable = false;
                }
            }
        }

        public void TryMoveBags()
        {
            Point3D hiddenLoc = Location;
            hiddenLoc.Z -= 50;

            for (int i = 0; i < m_ItemContaiers.Length; i++)
            {
                //Hacky hack...it doesn't want to send the display packet from another map
                if (m_ItemContaiers[i].Location != hiddenLoc)
                    m_ItemContaiers[i].MoveToWorld(hiddenLoc, Map);
            }
        }

        public bool CanUseCustomGear
        {
            get 
            {
                bool toReturn = true;
                int totalItems = 0;

                for (int i = 0; i < m_ItemContaiers.Length && toReturn; i++)
                {
                    if (m_ItemContaiers[i] == null || m_ItemContaiers[i].Deleted)
                        toReturn = false;
                    else
                        totalItems += m_ItemContaiers[i].Items.Count;
                }

                return toReturn && totalItems > 0;
            }
        }

        public override void OnDelete()
        {
            //Delete the resource bags
            for (int i = 0; i < m_ItemContaiers.Length; i++)
            {
                //Hacky hack...it doesn't want to send the display packet from another map
                if (m_ItemContaiers[i] != null)
                    m_ItemContaiers[i].Delete();
            }

            base.OnDelete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); //version



            // 0
            writer.Write(m_StayEquipped);
            writer.Write(m_NewbieAllItems);
            writer.Write(m_ClothHue);
            writer.Write(m_GearHue);
            writer.Write((int)m_SupplyType);
            writer.Write(m_TeamName);
            writer.Write(ConsumeItems);

            //The amount of resource bags to read at load
            writer.Write(m_ItemContaiers.Length);

            //Save the resource bags
            for (int i = 0; i < m_ItemContaiers.Length; i++)
            {
                writer.Write(m_ItemContaiers[i]);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            
            switch (version)
            {

                case 0:
                {
                    m_StayEquipped = reader.ReadBool();
                    m_NewbieAllItems = reader.ReadBool();
                    m_ClothHue = reader.ReadInt();
                    m_GearHue = reader.ReadInt();
                    m_SupplyType = (SupplyType)reader.ReadInt();
                    m_TeamName = reader.ReadString();
                    ConsumeItems = reader.ReadBool();

                    //The amount of resource bags to read at load
                    int bagCount = reader.ReadInt();
                    
                    m_ItemContaiers = new EventItemContainer[bagCount];
                    //Save the resource bags
                    for (int i = 0; i < bagCount; i++)
                    {
                        m_ItemContaiers[i] = (EventItemContainer)reader.ReadItem();
                    }
                    break;
                }
            }
        }

        public class ConfigSelectionGump : Gump
        {
            private readonly Mobile m_From;
            private readonly EventSupplier m_EventSupplier;
            private static int OFFSET = 1;

            public ConfigSelectionGump(Mobile from, EventSupplier eventSupplier)
                : this(from, eventSupplier,180, 180)
            {
            }

            public ConfigSelectionGump(Mobile from, EventSupplier eventSupplier, int x, int y)
                : base(x, y)
            {
                m_From = from;
                m_EventSupplier = eventSupplier;

                AddPage(0);

                AddImage(105, 131, 3507);
                AddImage(105, 105, 3501);

                AddLabel(130, 110, 0, "Armors");
                AddButton(115, 115, 2362, 2360, (int)AcceptedTypes.Armors + OFFSET, GumpButtonType.Reply, 0);

                AddLabel(275, 110, 0, "Weapons");
                AddButton(325, 115, 2362, 2360, (int)AcceptedTypes.Weapons + OFFSET, GumpButtonType.Reply, 0);

                AddLabel(130, 130, 0, "Clothing");
                AddButton(115, 135, 2362, 2360, (int)AcceptedTypes.Cloths + OFFSET, GumpButtonType.Reply, 0);

                AddLabel(275, 130, 0, "Others");
                AddButton(325, 135, 2362, 2360, (int)AcceptedTypes.Others + OFFSET, GumpButtonType.Reply, 0);

                AddImage(80, 105, 3500);
                AddImage(346, 105, 3502);
                AddImage(79, 131, 3506);
                AddImage(346, 131, 3508);
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (info.ButtonID != 0)
                {
                    m_EventSupplier.m_ItemContaiers[info.ButtonID - OFFSET].DisplayTo(m_From);
                    m_From.SendGump(new ConfigSelectionGump(m_From, m_EventSupplier, X, Y));
                }
            }
        }
    }
}
