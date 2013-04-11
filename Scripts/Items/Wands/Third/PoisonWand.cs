using Server.Spells.Third;

namespace Server.Items
{
    public class PoisonWand : BaseWand
    {
        [Constructable]
        public PoisonWand() : base(13)
        {
            ItemID = 0xdf5;
            Name = "Poison";
        }

        public PoisonWand(Serial serial)
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
            Cast(new PoisonSpell(from, this));
        }
    }
}