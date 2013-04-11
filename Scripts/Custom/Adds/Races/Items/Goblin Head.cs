namespace Server.Items
{
    public class GoblinMask : Hair
    {
        [Constructable]
        public GoblinMask()
            : this(2212)
        {
        }

        [Constructable]
        public GoblinMask(int hue)
            : base(0x141B, hue)
        {
            Weight = 0.0;
            Name = "a goblins head";
            Movable = false;
        }

        public GoblinMask(Serial serial)
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