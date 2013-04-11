using System;
using System.Collections.Generic;
using Server.Engines.Craft;
using Server.Network;

namespace Server.Items
{
    public class Hood : BaseHat
    {
        public override int BasePhysicalResistance { get { return 5; } }

        public override int InitMinHits { get { return 220; } }
        public override int InitMaxHits { get { return 330; } }

        [Constructable]
        public Hood()
            : base(0x3907)
        {
            Name = "Hood";
            Weight = 1.0;
        }

        public Hood(Serial serial)
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
