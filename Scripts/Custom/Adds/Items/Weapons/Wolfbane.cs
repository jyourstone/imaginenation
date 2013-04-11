namespace Server.Items
{
    public class Wolfbane : Longsword
    {
        [Constructable]
        public Wolfbane()
        {
            Hue = 1175;
            Name = "Wolfbane";
            Weight = 6.0;
            Speed = 400;
            MinDamage = 10;
            MaxDamage = 28;
            AccuracyLevel = WeaponAccuracyLevel.Eminently;
            DamageLevel = WeaponDamageLevel.Force;
            DurabilityLevel = WeaponDurabilityLevel.Substantial;
            IsRenamed = true;
            Slayer = SlayerName.Repond;
        }

        public Wolfbane(Serial serial)
            : base(serial)
        {
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, Sphere.ComputeCustomWeaponName(this));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}