namespace Server.Items
{
    public class DwarfWarHammer : WarMace
    {
        [Constructable]
        public DwarfWarHammer()
        {
            Hue = 1325;
            Name = "Dwarf War Hammer";
            Weight = 15.0;
            Speed = 428;
            MinDamage = 12;
            MaxDamage = 28;
            AccuracyLevel = WeaponAccuracyLevel.Accurate;
            DamageLevel = WeaponDamageLevel.Might;
            DurabilityLevel = WeaponDurabilityLevel.Substantial;
            IsRenamed = true;
            Slayer = SlayerName.Terathan;
        }

        public DwarfWarHammer(Serial serial)
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