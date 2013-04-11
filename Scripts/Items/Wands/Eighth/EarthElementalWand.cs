using Server.Spells.Eighth;

namespace Server.Items
{
    public class EarthElementalWand : BaseWand
    {
        [Constructable]
        public EarthElementalWand() : base(3)
        {
            ItemID = 0xdf3;
            Name = "Earth Elemental";
        }

        public EarthElementalWand(Serial serial)
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
            Cast(new EarthElementalSpell(from, this));
        }
    }
}