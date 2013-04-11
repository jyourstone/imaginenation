using Server.Spells.Seventh;

namespace Server.Items
{
    public class EnergyFieldWand : BaseWand
    {
        [Constructable]
        public EnergyFieldWand() : base(5)
        {
            ItemID = 0xdf3;
            Name = "Energy Field";
        }

        public EnergyFieldWand(Serial serial)
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
            Cast(new EnergyFieldSpell(from, this));
        }
    }
}