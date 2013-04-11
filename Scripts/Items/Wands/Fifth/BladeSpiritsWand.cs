using Server.Spells.Fifth;

namespace Server.Items
{
    public class BladeSpiritsWand : BaseWand
    {
        [Constructable]
        public BladeSpiritsWand() : base(9)
        {
            ItemID = 0xdf2;
            Name = "Blade Spirits";
        }

        public BladeSpiritsWand(Serial serial)
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
            Cast(new BladeSpiritsSpell(from, this));
        }
    }
}