namespace Server.Items
{
    [Furniture]
    [Flipable(2926, 2927, 2928, 2929, 2930, 2931, 2932, 2945, 2946, 2947, 2948, 2949, 2950, 2951)]
    public class Table : Item
    {
        [Constructable]
        public Table() : base(2926)
        {
            Weight = 2.0;
        }

        public Table(Serial serial) : base(serial)
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
    [Flipable(2933, 2934, 2935, 2936, 2937, 2938, 2939, 2952, 2953, 2954, 2955, 2956, 2957, 2958)]
    public class Table2 : Item
    {
        [Constructable]
        public Table2() : base(2933)
        {
            Weight = 2.0;
        }

        public Table2(Serial serial)
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

    [Furniture]
    [Flipable(2923, 2924, 2925, 2942, 2943, 2944)]
    public class LongTable : Item
    {
        [Constructable]
        public LongTable() : base(2923)
        {
            Weight = 2.0;
        }

        public LongTable(Serial serial)
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

    [Furniture]
    [Flipable(4495, 4496, 4497, 4498)]
    public class ColoredTable : Item
    {
        [Constructable]
        public ColoredTable() : base(4495)
        {
            Weight = 2.0;
        }

        public ColoredTable(Serial serial)
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

    [Furniture]
    [Flipable(5735, 5736, 5737, 5738, 5739, 5740)]
    public class ColoredTable2 : Item
    {
        [Constructable]
        public ColoredTable2()
            : base(5735)
        {
            Weight = 2.0;
        }

        public ColoredTable2(Serial serial)
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