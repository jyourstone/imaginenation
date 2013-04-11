namespace Server.Items
{
    public class MahoganyBoard : BaseBoard
    {
        [Constructable]
        public MahoganyBoard()
            : this(1)
        {
        }

        [Constructable]
        public MahoganyBoard(int amount)
            : base(CraftResource.Mahoganywood, amount)
        {
            Name = "mahogany board";
        }

        public MahoganyBoard(Serial serial)
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

    public class CedarBoard : BaseBoard
    {
        [Constructable]
        public CedarBoard()
            : this(1)
        {
        }

        [Constructable]
        public CedarBoard(int amount)
            : base(CraftResource.Cedarwood, amount)
        {
            Name = "cedar board";
        }

        public CedarBoard(Serial serial)
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

    public class WillowBoard : BaseBoard
    {
        [Constructable]
        public WillowBoard()
            : this(1)
        {
        }

        [Constructable]
        public WillowBoard(int amount)
            : base(CraftResource.Willowwood, amount)
        {
            Name = "willow board";
        }

        public WillowBoard(Serial serial)
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

    public class MystWoodBoard : BaseBoard
    {
        [Constructable]
        public MystWoodBoard()
            : this(1)
        {
        }

        [Constructable]
        public MystWoodBoard(int amount)
            : base(CraftResource.Mystwood, amount)
        {
            Name = "mystwood board";
        }

        public MystWoodBoard(Serial serial)
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
