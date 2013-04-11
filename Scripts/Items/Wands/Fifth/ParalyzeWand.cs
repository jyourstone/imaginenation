using Server.Spells.Fifth;

namespace Server.Items
{
    public class ParalyzeWand : BaseWand
    {
        [Constructable]
        public ParalyzeWand() : base(9)
        {
            ItemID = 0xdf3;
            Name = "Paralyze";
        }

        public ParalyzeWand(Serial serial)
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
            Cast(new ParalyzeSpell(from, this));
        }
    }
}