using Server.Spells.Seventh;

namespace Server.Items
{
    public class PolymorphWand : BaseWand
    {
        [Constructable]
        public PolymorphWand() : base(5)
        {
            ItemID = 0xdf5;
            Name = "Polymorph";
        }

        public PolymorphWand(Serial serial)
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

        public override void OnWandUse(Mobile from)
        {
            Cast(new PolymorphSpell(from, this));
        }
    }
}