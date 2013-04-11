namespace Server.Items
{
    [Flipable(7939, 7940)] // Robe
    public class ShadowRobe : BaseOuterTorso
    {
        private Mobile m_Owner;
        private bool m_OwnerOnly;

        [Constructable] // Robe Hue
        public ShadowRobe()
            : this(1907)
        {
        }

        [Constructable]
        public ShadowRobe(int hue)
            : base(7939, hue)
        {
            Name = "Robe of Shadows";
            LootType = LootType.Blessed;
            Weight = 0.0;
        }

        public ShadowRobe(Serial serial)
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

    [Flipable(9860, 9859)] // Shroud
    public class ShadowShroud : BaseOuterTorso
    {
        private Mobile m_Owner;
        private bool m_OwnerOnly;

        [Constructable] // Dress Hue
        public ShadowShroud()
            : this(1907)
        {
        }

        [Constructable]
        public ShadowShroud(int hue)
            : base(9860, hue)
        {
            Name = "Shroud of Shadows";
            LootType = LootType.Blessed;
            Weight = 0.0;
        }

        public ShadowShroud(Serial serial)
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