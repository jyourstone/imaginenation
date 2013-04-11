using Server.Spells.Seventh;

namespace Server.Items
{
    public class ChainLightningWand : BaseWand
    {
        [Constructable]
        public ChainLightningWand() : base(5)
        {
            ItemID = 0xdf2;
            Name = "Chain Lightning";
        }

        public ChainLightningWand(Serial serial)
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
            Cast(new ChainLightningSpell(from, this));
        }
    }
}