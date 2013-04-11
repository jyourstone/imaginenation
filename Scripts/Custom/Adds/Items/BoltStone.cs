namespace Server.Items
{
    public class BoltStone : Item
    {
        private int m_Price = 1100;
        private int m_Amount = 100;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Price
        {
            get { return m_Price; }
            set { m_Price = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public new int Amount
        {
            get { return m_Amount; }
            set { m_Amount = value; }
        }

        [Constructable]
        public BoltStone()
            : base(0xED5)
        {
            Name = "Bolt Stone";
            Movable = false;
            Hue = 1963;
        }

        public BoltStone(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Mobiles.BaseVendor.ConsumeBackpackAndBankGold(from, m_Price))
            {
                from.SendAsciiMessage("You've bought some bolts.");
                from.PlaySound(247);

                from.AddToBackpack(new Bolt(m_Amount));
            }
            else
                from.SendAsciiMessage("You do not have enough money!");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, string.Format("{0} [{1} for {2}]", Name, m_Amount, m_Price));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version 

            writer.Write(m_Price);
            writer.Write(m_Amount);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Price = reader.ReadInt();
            m_Amount = reader.ReadInt();
        }
    }
}
