using Server.Spells.Eighth;

namespace Server.Items
{
    public class WaterElementalWand : BaseWand
    {
        [Constructable]
        public WaterElementalWand() : base(3)
        {
            ItemID = 0xdf5;
            Name = "Water Elemental";
        }

        public WaterElementalWand(Serial serial)
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
            Cast(new WaterElementalSpell(from, this));
        }
    }
}