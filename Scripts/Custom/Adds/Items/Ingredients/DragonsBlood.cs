namespace Server.Items
{
    public class DragonsBlood : Item
    {
        [Constructable]
		public DragonsBlood() : this( 1 )
		{
		}
        [Constructable]
        public DragonsBlood(int amount) : base(3970)
        {
            Name = "Dragon's blood";
            Stackable = true;
            Amount = amount;
        }

        public override void OnSingleClick(Mobile from)
        {
            {
                if (Amount > 1)
                    LabelTo(from, Amount + " Dragons' blood");
                else
                    LabelTo(from, "Dragon's blood");
            }
        }

        public DragonsBlood(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            return;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}