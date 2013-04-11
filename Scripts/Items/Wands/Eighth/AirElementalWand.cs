using Server.Spells.Eighth;

namespace Server.Items
{
    public class AirElementalWand : BaseWand
    {
        [Constructable]
        public AirElementalWand() : base(3)
        {
            ItemID = 0xdf5;
            Name = "Air Elemental";
        }

        public AirElementalWand(Serial serial)
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
            Cast(new AirElementalSpell(from, this));
        }
    }
}