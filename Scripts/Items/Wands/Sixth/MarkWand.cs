using Server.Spells.Sixth;

namespace Server.Items
{
    public class MarkWand : BaseWand
    {
        [Constructable]
        public MarkWand() : base(7)
        {
            ItemID = 0xdf2;
            Name = "Mark";
        }

        public MarkWand(Serial serial)
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
            Cast(new MarkSpell(from, this));
        }
    }
}