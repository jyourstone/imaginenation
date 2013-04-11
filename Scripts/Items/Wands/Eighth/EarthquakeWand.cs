using Server.Spells.Eighth;

namespace Server.Items
{
    public class EarthquakeWand : BaseWand
    {
        [Constructable]
        public EarthquakeWand() : base(3)
        {
            ItemID = 0xdf2;
            Name = "Earthquake";
        }

        public EarthquakeWand(Serial serial)
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
            Cast(new EarthquakeSpell(from, this));
        }
    }
}