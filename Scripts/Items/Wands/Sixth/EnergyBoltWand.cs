using Server.Spells.Sixth;

namespace Server.Items
{
    public class EnergyBoltWand : BaseWand
    {
        [Constructable]
        public EnergyBoltWand() : base(7)
        {
            ItemID = 0xdf3;
            Name = "Energy Bolt";
        }

        public EnergyBoltWand(Serial serial)
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
            Cast(new EnergyBoltSpell(from, this));
        }
    }
}