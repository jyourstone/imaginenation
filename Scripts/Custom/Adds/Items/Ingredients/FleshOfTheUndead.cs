namespace Server.Items
{
    public class FleshOfTheUndead : Item
    {
        [Constructable]
        public FleshOfTheUndead() : base(3966)
        {
            Name = "Flesh of the undead";
            Hue = 1171;
            Stackable = true;
        }

        public FleshOfTheUndead(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            return;
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
    }
}