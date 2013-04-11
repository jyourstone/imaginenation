namespace Server.Items
{
    [Furniture]
    [Flipable(4615, 4616, 4617, 4618, 4619, 4620)]
    public class StoneBench : Item
    {
        [Constructable]
        public StoneBench(): base(4615)
        {
            Weight = 5.0;
        }

        public StoneBench(Serial serial) : base(serial)
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
    [Flipable(15871, 15872)]
    public class StoneBenchSingle : Item
    {
        [Constructable]
        public StoneBenchSingle() : base(15871)
        {
            Weight = 5.0;
        }

        public StoneBenchSingle(Serial serial) : base(serial)
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
    [Flipable(4609, 4610, 4611, 4612, 4613, 4614)]
    public class StoneTable : Item
    {
        [Constructable]
        public StoneTable() : base(4609)
        {
            Weight = 5.0;
        }

        public StoneTable(Serial serial)
            : base(serial)
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