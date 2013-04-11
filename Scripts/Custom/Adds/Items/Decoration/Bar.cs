namespace Server.Items
{
    [Furniture]
    [Flipable(6416, 6417, 6418, 6419, 6424, 6425, 6426, 6427, 6428, 6429, 6430, 6431)]
    public class BarTable : Item
    {
        [Constructable]
        public BarTable() : base(6425)
        {
            Weight = 1.0;
        }

        public BarTable(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(6414, 6415)]
    public class BarDoor : Item
    {
        [Constructable]
        public BarDoor() : base(6414)
        {
            Weight = 1.0;
        }

        public BarDoor(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}