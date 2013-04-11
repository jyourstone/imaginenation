using Server.Spells.Fifth;

namespace Server.Items
{
    public class MagicReflectWand : BaseWand
    {
        [Constructable]
        public MagicReflectWand() : base(9)
        {
            ItemID = 0xdf5;
            Name = "Magic Reflect";
        }

        public MagicReflectWand(Serial serial)
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
            Cast(new MagicReflectSpell(from, this));
        }
    }
}