using Server.Spells.Fifth;

namespace Server.Items
{
    public class DispelFieldWand : BaseWand
    {
        [Constructable]
        public DispelFieldWand() : base(9)
        {
            ItemID = 0xdf3;
            Name = "Dispel Field";
        }

        public DispelFieldWand(Serial serial)
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
            Cast(new DispelFieldSpell(from, this));
        }
    }
}