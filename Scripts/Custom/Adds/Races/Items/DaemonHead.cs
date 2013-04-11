namespace Server.Items
{
    public class DaemonHead : Hair
    {
        [Constructable]
        public DaemonHead() : base(7947)
        {
            Weight = 0.0;
            Hue = 1171;
            Name = "a daemons head";
            Movable = false;
        }

        public DaemonHead(Serial serial)
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