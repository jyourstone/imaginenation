namespace Server.Items
{
    public class MahoganyLog : BaseLog
    {
        [Constructable]
        public MahoganyLog()
            : this(1)
        {
        }

        [Constructable]
        public MahoganyLog(int amount)
            : base(CraftResource.Mahoganywood, amount)
        {
            Name = "mahogany log";
        }

        public MahoganyLog(Serial serial)
            : base(serial)
        {
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

    public class CedarLog : BaseLog
    {
        [Constructable]
        public CedarLog()
            : this(1)
        {
        }

        [Constructable]
        public CedarLog(int amount)
            : base(CraftResource.Cedarwood, amount)
        {
            Name = "cedar log";
        }

        public CedarLog(Serial serial)
            : base(serial)
        {
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

    public class WillowLog : BaseLog
    {
        [Constructable]
        public WillowLog()
            : this(1)
        {
        }

        [Constructable]
        public WillowLog(int amount)
            : base(CraftResource.Willowwood, amount)
        {
            Name = "willow log";
        }

        public WillowLog(Serial serial)
            : base(serial)
        {
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

    public class MystWoodLog : BaseLog
    {
        [Constructable]
        public MystWoodLog()
            : this(1)
        {
        }

        [Constructable]
        public MystWoodLog(int amount)
            : base(CraftResource.Mystwood, amount)
        {
            Name = "mystwood log";
        }

        public MystWoodLog(Serial serial)
            : base(serial)
        {
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
