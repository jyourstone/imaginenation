namespace Server.Items
{
    public class ArrowStone : Item
    {
        private int m_Price = 1000;
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
        public ArrowStone()
            : base(0xED5)
        {
            Name = "Arrow Stone";
            Movable = false;
            Hue = 1171;
        }

        public ArrowStone(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Mobiles.BaseVendor.ConsumeBackpackAndBankGold(from, m_Price))
            {
                from.SendAsciiMessage("You've bought some arrows.");
                from.PlaySound(247);

                from.AddToBackpack(new Arrow(m_Amount));
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
