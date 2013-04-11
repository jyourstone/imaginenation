using Server.Spells.Fourth;

namespace Server.Items
{
    public class CurseWand : BaseWand
    {
        [Constructable]
        public CurseWand() : base(12)
        {
            ItemID = 0xdf4;
            Name = "Curse";
        }

        public CurseWand(Serial serial)
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
            Cast(new CurseSpell(from, this));
        }
    }
}