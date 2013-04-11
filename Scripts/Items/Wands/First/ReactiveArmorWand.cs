using Server.Spells.First;

namespace Server.Items
{
    public class ReactiveArmorWand : BaseWand
    {
        [Constructable]
        public ReactiveArmorWand() : base(17)
        {
            ItemID = 0xdf4;
            Name = "Reactive Armor";
        }

        public ReactiveArmorWand(Serial serial)
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
            Cast(new ReactiveArmorSpell(from, this));
        }
    }
}