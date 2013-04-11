using Server.Spells.Second;

namespace Server.Items
{
    public class CureWand : BaseWand
    {
        [Constructable]
        public CureWand() : base(15)
        {
            ItemID = 0xdf4;
            Name = "Cure";
        }

        public CureWand(Serial serial)
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
            Cast(new CureSpell(from, this));
        }
    }
}