using Server.Spells.Second;

namespace Server.Items
{
    public class AgilityWand : BaseWand
    {
        [Constructable]
        public AgilityWand() : base(15)
        {
            Name = "Agility";
            ItemID = 0xdf2;
        }

        public AgilityWand(Serial serial) : base(serial)
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
            Cast(new AgilitySpell(from, this));
        }
    }
}