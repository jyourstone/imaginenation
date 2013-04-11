using Server.Spells.Third;

namespace Server.Items
{
    public class WallOfStoneWand : BaseWand
    {
        [Constructable]
        public WallOfStoneWand() : base(13)
        {
            ItemID = 0xdf5;
            Name = "Wall of Stone";
        }

        public WallOfStoneWand(Serial serial)
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
            Cast(new WallOfStoneSpell(from, this));
        }
    }
}