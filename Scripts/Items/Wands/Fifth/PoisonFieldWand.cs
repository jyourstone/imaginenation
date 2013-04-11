using Server.Spells.Fifth;

namespace Server.Items
{
    public class PoisonFieldWand : BaseWand
    {
        [Constructable]
        public PoisonFieldWand() : base(9)
        {
            ItemID = 0xdf4;
            Name = "Poison Field";
        }

        public PoisonFieldWand(Serial serial)
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
            Cast(new PoisonFieldSpell(from, this));
        }
    }
}