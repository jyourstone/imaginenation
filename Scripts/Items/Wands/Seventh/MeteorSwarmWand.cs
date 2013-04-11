using Server.Spells.Seventh;

namespace Server.Items
{
    public class MeteorSwarmWand : BaseWand
    {
        [Constructable]
        public MeteorSwarmWand() : base(5)
        {
            ItemID = 0xdf4;
            Name = "Meteor Swarm";
        }

        public MeteorSwarmWand(Serial serial)
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
            Cast(new MeteorSwarmSpell(from, this));
        }
    }
}