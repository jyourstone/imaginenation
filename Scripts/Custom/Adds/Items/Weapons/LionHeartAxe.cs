namespace Server.Items
{
    public class LionheartAxe : TwoHandedAxe
    {
        [Constructable]
        public LionheartAxe()
        {
            Hue = 1965;
            Name = "Lionheart Axe";
            Weight = 7.0;
            Speed = 450;
            MinDamage = 10;
            MaxDamage = 27;
            AccuracyLevel = WeaponAccuracyLevel.Surpassingly;
            DamageLevel = WeaponDamageLevel.Force;
            DurabilityLevel = WeaponDurabilityLevel.Substantial;
            IsRenamed = true;
            Slayer = SlayerName.SnakesBane;
        }

        public LionheartAxe(Serial serial)
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