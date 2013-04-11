namespace Server.Items
{
    [Furniture]
    [Flipable(7629, 7630, 7631, 7632, 7633, 7634)]
    public class MarbleBench : Item
    {
        [Constructable]
        public MarbleBench() : base(7629)
        {
            Weight = 5.0;
        }

        public MarbleBench(Serial serial) : base(serial)
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
    [Flipable(1113, 1114)]
    public class MarbleBenchSingle : Item
    {
        [Constructable]
        public MarbleBenchSingle() : base(1113)
        {
            Weight = 5.0;
        }

        public MarbleBenchSingle(Serial serial) : base(serial)
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
    [Flipable(7617, 7618, 7619, 7620, 7621, 7622)]
    public class MarbleTable : Item
    {
        [Constructable]
        public MarbleTable() : base(7617)
        {
            Weight = 5.0;
        }

        public MarbleTable(Serial serial) : base(serial)
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