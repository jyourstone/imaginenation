using Server.Spells.First;

namespace Server.Items
{
    public class CreateFoodWand : BaseWand
    {
        [Constructable]
        public CreateFoodWand() : base(17)
        {
            ItemID = 0xdf3;
            Name = "Create Food";
        }

        public CreateFoodWand(Serial serial)
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
            Cast(new CreateFoodSpell(from, this));
        }
    }
}