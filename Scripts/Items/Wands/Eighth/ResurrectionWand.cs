using Server.Spells.Eighth;

namespace Server.Items
{
    public class ResurrectionWand : BaseWand
    {
        [Constructable]
        public ResurrectionWand() : base(3)
        {
            ItemID = 0xdf4;
            Name = "Resurrection";
        }

        public ResurrectionWand(Serial serial)
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
            Cast(new ResurrectionSpell(from, this));
        }
    }
}