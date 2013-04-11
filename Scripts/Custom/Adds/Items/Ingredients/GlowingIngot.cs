namespace Server.Items
{
    public class GlowingIngot : Item
    {
        [Constructable]
        public GlowingIngot() : base(7151)
        {
            Name = "Glowing ingot";
            Hue = 2517;
            Stackable = true;
        }

        public GlowingIngot(Serial serial) : base(serial)
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