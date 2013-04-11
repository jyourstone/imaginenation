using System;

namespace Server.Items
{
    public class GargoyleWings : BaseCloak
    {
        [Constructable]
        public GargoyleWings()
            : this(0)
        {
        }

        [Constructable]
        public GargoyleWings(int hue)
            : base(0x2B02, hue)
        {
            Weight = 5.0;
        }

        public GargoyleWings(Serial serial)
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