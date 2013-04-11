namespace Server.Items
{
    [Flipable(7939, 7940)] // Robe
    public class WoodlandRobe : BaseOuterTorso
    {
        private Mobile m_Owner;
        private bool m_OwnerOnly;

        [Constructable] // Robe Hue
        public WoodlandRobe() : this(1434)
        {
        }

        [Constructable]
        public WoodlandRobe(int hue) : base(7939, hue)
        {
            Name = "Woodland Merchants";
            LootType = LootType.Blessed;
            Weight = 0.0;
        }

        public WoodlandRobe(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public new Mobile Owner
        {
            get { return m_Owner; }
            set
            {
                m_Owner = value;
                InvalidateProperties();
            }
        }

        public override bool DisplayLootType
        {
            get { return false; }
        }

        public override bool OnEquip(Mobile from)
        {
            if (from == Owner)
                return base.OnEquip(from);

            else if (Owner == null)
                Delete();

            return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version

            writer.Write(m_Owner);
            writer.Write(m_OwnerOnly);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Owner = reader.ReadMobile();
            m_OwnerOnly = reader.ReadBool();
        }
    }

    [Flipable(7937, 7938)] // Dress
    public class WoodlandDress : BaseOuterTorso
    {
        private Mobile m_Owner;
        private bool m_OwnerOnly;

        [Constructable] // Dress Hue
        public WoodlandDress() : this(1434)
        {
        }

        [Constructable]
        public WoodlandDress(int hue) : base(7937, hue)
        {
            Name = "Woodland Merchants";
            LootType = LootType.Blessed;
            Weight = 0.0;
        }

        public WoodlandDress(Serial serial) : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public new Mobile Owner
        {
            get { return m_Owner; }
            set
            {
                m_Owner = value;
                InvalidateProperties();
            }
        }

        public override bool DisplayLootType
        {
            get { return false; }
        }

        public override bool OnEquip(Mobile from)
        {
            if (from == Owner)
                return base.OnEquip(from);

            else if (Owner == null)
                Delete();

            return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version

            writer.Write(m_Owner);
            writer.Write(m_OwnerOnly);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Owner = reader.ReadMobile();
            m_OwnerOnly = reader.ReadBool();
        }
    }

    public class WoodlandHat : BaseHat
    {
        private Mobile m_Owner;
        private bool m_OwnerOnly;

        [Constructable] // Hat Hue 1434
        public WoodlandHat()
            : this(0x59A)
        {
        }

        [Constructable]
        public WoodlandHat(int hue)
            : base(0x171A, hue)
        {
            Name = "Woodland Merchants";
            LootType = LootType.Blessed;
            Weight = 0.0;
        }

        public WoodlandHat(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public new Mobile Owner
        {
            get { return m_Owner; }
            set
            {
                m_Owner = value;
                InvalidateProperties();
            }
        }

        public override bool DisplayLootType
        {
            get { return false; }
        }

        public override bool OnEquip(Mobile from)
        {
            if (from == Owner)
                return base.OnEquip(from);

            else if (Owner == null)
                Delete();

            return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version

            writer.Write(m_Owner);
            writer.Write(m_OwnerOnly);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Owner = reader.ReadMobile();
            m_OwnerOnly = reader.ReadBool();
        }
    }
}