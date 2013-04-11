namespace Server.Items
{
    public class SkyDragonScale : Item
    {
        [Constructable]
        public SkyDragonScale()
            : this(1)
        {
        }

        [Constructable]
        public SkyDragonScale(int amount)
            : base(0x26B4)
        {
            Name = "Sky dragon scale";
            Hue = 2663;
            Stackable = true;
            Amount = amount;
        }

        public SkyDragonScale(Serial serial)
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

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class HellDragonScale : Item
    {
        [Constructable]
        public HellDragonScale()
            : this(1)
        {
        }

        [Constructable]
        public HellDragonScale(int amount)
            : base(0x26B4)
        {
            Name = "Hell dragon scale";
            Hue = 2870;
            Stackable = true;
            Amount = amount;
        }

        public HellDragonScale(Serial serial)
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

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}