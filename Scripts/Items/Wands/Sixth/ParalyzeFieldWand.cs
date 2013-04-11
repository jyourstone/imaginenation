using Server.Spells.Sixth;

namespace Server.Items
{
    public class ParalyzeFieldWand : BaseWand
    {
        [Constructable]
        public ParalyzeFieldWand() : base(7)
        {
            ItemID = 0xdf4;
            Name = "Paralyze Field";
        }

        public ParalyzeFieldWand(Serial serial)
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
            Cast(new ParalyzeFieldSpell(from, this));
        }
    }
}