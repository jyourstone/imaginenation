using Server.Spells.Second;

namespace Server.Items
{
    public class MagicTrapWand : BaseWand
    {
        [Constructable]
        public MagicTrapWand() : base(15)
        {
            ItemID = 0xdf2;
            Name = "Magic Trap";
        }

        public MagicTrapWand(Serial serial)
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
            Cast(new MagicTrapSpell(from, this));
        }
    }
}