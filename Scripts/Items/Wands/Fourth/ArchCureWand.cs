using Server.Spells.Fourth;

namespace Server.Items
{
    public class ArchCureWand : BaseWand
    {
        [Constructable]
        public ArchCureWand() : base(11)
        {
            ItemID = 0xdf2;
            Name = "Arch Cure";
        }

        public ArchCureWand(Serial serial)
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
            Cast(new ArchCureSpell(from, this));
        }
    }
}