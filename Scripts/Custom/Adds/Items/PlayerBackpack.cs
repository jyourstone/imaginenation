namespace Server.Items
{
    public class PlayerBackpack : Backpack
    {

        public override int DefaultMaxWeight
        {
            get
            {
                return m_Owner == null ? 0 : WeightLeft != 0 ? WeightLeft : -1;
            }
        }

        public int WeightLeft
        {
            get
            {
                return ((m_Owner.Str * 4) + 27 - GetGearWeight());
            }
        }

        private Mobile m_Owner;

        [CommandProperty(AccessLevel.Seer)]
        private Mobile Owner 
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }

        [Constructable]
        public PlayerBackpack(Mobile owner)
        {
            m_Owner = owner;
            Layer = Layer.Backpack;
            Weight = 3.0;
        }

        public PlayerBackpack(Serial serial) : base(serial)
        {
        }

        public override void OnSingleClick(Mobile from)
        {
            if (string.IsNullOrEmpty(Name))
                LabelTo(from, "Backpack ({0} items)", Items.Count);
            else
                base.OnSingleClick(from);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version

            writer.Write(m_Owner);
        }

        private int GetGearWeight()
        {
            if (m_Owner == null)
                return 0;

            int gearWeight = 0;

            //Wearables
            foreach (Item item in m_Owner.Items)
                if (item != null && item.Layer != Layer.Bank && item.Layer != Layer.Mount && item.Layer != Layer.Backpack && item.Layer != Layer.Hair && item.Layer != Layer.FacialHair)
                    gearWeight += (int)item.Weight;

            return gearWeight;
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Owner = reader.ReadMobile();
        }
    }
}
