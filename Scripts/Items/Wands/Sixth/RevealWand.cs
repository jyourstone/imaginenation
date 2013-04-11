using Server.Spells.Sixth;

namespace Server.Items
{
    public class RevealWand : BaseWand
    {
        [Constructable]
        public RevealWand() : base(7)
        {
            ItemID = 0xdf5;
            Name = "Reveal";
        }

        public RevealWand(Serial serial)
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
            Cast(new RevealSpell(from, this));
        }
    }
}