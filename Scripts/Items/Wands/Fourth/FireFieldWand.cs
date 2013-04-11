using Server.Spells.Fourth;

namespace Server.Items
{
    public class FireFieldWand : BaseWand
    {
        [Constructable]
        public FireFieldWand() : base(11)
        {
            ItemID = 0xdf5;
            Name = "Fire Field";
        }

        public FireFieldWand(Serial serial)
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
            Cast(new FireFieldSpell(from, this));
        }
    }
}