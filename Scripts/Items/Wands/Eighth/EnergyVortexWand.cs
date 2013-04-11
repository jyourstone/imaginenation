using Server.Spells.Eighth;

namespace Server.Items
{
    public class EnergyVortexWand : BaseWand
    {
        [Constructable]
        public EnergyVortexWand() : base(3)
        {
            ItemID = 0xdf3;
            Name = "Energy Vortex";
        }

        public EnergyVortexWand(Serial serial)
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
            Cast(new EnergyVortexSpell(from, this));
        }
    }
}