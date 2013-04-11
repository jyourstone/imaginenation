namespace Server.Items
{
    [Flipable(0x38D5, 0x38D6)]
    public class MarbleSink : Item
    {
        [Constructable]
        public MarbleSink(): base (0x38D5)
        {
            Weight = 10;
            Name = "Marble sink";
        }
    

    public MarbleSink(Serial serial)
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

    [Flipable(0x38D1, 0x38D2)]
    public class BathroomMirror : Item
    {
        [Constructable]
        public BathroomMirror(): base (0x38D1)
        {
            Weight = 5;
            Name = "Bathroom mirror";
        }
    

    public BathroomMirror(Serial serial)
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

    [Flipable(0x38D3, 0x38D4)]
    public class MarbleShower : Item
    {
        [Constructable]
        public MarbleShower(): base (0x38D3)
        {
            Weight = 30;
            Name = "Marble shower";
        }
    

    public MarbleShower(Serial serial)
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

    [Flipable(0x38D7, 0x38D8)]
    public class MarbleToilet : Item
    {
        [Constructable]
        public MarbleToilet(): base (0x38D7)
        {
            Weight = 10;
            Name = "Marble toilet";
        }
    

    public MarbleToilet(Serial serial)
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