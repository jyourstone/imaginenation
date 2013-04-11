using Server.Spells.Third;

namespace Server.Items
{
    public class MagicLockWand : BaseWand
    {
        [Constructable]
        public MagicLockWand() : base(13)
        {
            ItemID = 0xdf4;
            Name = "Magic Lock";
        }

        public MagicLockWand(Serial serial)
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
            Cast(new MagicLockSpell(from, this));
        }
    }
}