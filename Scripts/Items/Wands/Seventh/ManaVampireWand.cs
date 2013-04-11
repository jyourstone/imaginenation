using Server.Spells.Seventh;

namespace Server.Items
{
    public class ManaVampireWand : BaseWand
    {
        [Constructable]
        public ManaVampireWand() : base(5)
        {
            ItemID = 0xdf2;
            Name = "Mana Vampire";
        }

        public ManaVampireWand(Serial serial)
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
            Cast(new ManaVampireSpell(from, this));
        }
    }
}