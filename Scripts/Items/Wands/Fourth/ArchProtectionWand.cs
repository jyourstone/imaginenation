using Server.Spells.Fourth;

namespace Server.Items
{
    public class ArchProtectionWand : BaseWand
    {
        [Constructable]
        public ArchProtectionWand() : base(11)
        {
            ItemID = 0xdf3;
            Name = "Arch Protection";
        }

        public ArchProtectionWand(Serial serial)
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
            Cast(new ArchProtectionSpell(from, this));
        }
    }
}