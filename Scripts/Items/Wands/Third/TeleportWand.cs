using Server.Spells.Third;

namespace Server.Items
{
    public class TeleportWand : BaseWand
    {
        [Constructable]
        public TeleportWand() : base(13)
        {
            ItemID = 0xdf3;
            Name = "Teleport";
        }

        public TeleportWand(Serial serial)
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
            Cast(new TeleportSpell(from, this));
        }
    }
}