namespace Server.Items
{
    public class BloodTentacle : Kryss
    {
        [Constructable]
        public BloodTentacle()
        {
            Hue = 1171;
            Name = "Blood Tentacle";
            Speed = 290;
            MinDamage = 10;
            MaxDamage = 18;
            AccuracyLevel = WeaponAccuracyLevel.Eminently;
            DamageLevel = WeaponDamageLevel.Force;
            DurabilityLevel = WeaponDurabilityLevel.Substantial;
            IsRenamed = true;
        }

        public BloodTentacle(Serial serial)
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