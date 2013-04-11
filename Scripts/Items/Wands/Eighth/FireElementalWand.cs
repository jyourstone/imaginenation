using Server.Spells.Eighth;

namespace Server.Items
{
    public class FireElementalWand : BaseWand
    {
        [Constructable]
        public FireElementalWand() : base(3)
        {
            ItemID = 0xdf4;
            Name = "Fire Elemental";
        }

        public FireElementalWand(Serial serial)
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
            Cast(new FireElementalSpell(from, this));
        }
    }
}