namespace Server.Items
{
    public class GoblinClooba : Club
    {
        [Constructable]
        public GoblinClooba()
        {
            Hue = 2212;
            Name = "Goblin Clooba";
            Weight = 9.0;
            Speed = 302;
            MinDamage = 8;
            MaxDamage = 21;
            AccuracyLevel = WeaponAccuracyLevel.Surpassingly;
            DamageLevel = WeaponDamageLevel.Force;
            DurabilityLevel = WeaponDurabilityLevel.Substantial;
            IsRenamed = true;
            Slayer = SlayerName.OrcSlaying;
        }

        public GoblinClooba(Serial serial)
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