using Server.Network;

namespace Server.Items
{
    public class ChuKoNu : Crossbow
    {
        [Constructable]
        public ChuKoNu()
        {
            Hue = 1940;
            Name = "Chu Ko Nu";
            Weight = 4;
            Speed = 310;
            MinDamage = 15;
            MaxDamage = 29;
            AccuracyLevel = WeaponAccuracyLevel.Exceedingly;
            DamageLevel = WeaponDamageLevel.Power;
            DurabilityLevel = WeaponDurabilityLevel.Substantial;
            IsRenamed = true;
        }

        public ChuKoNu(Serial serial)
            : base(serial)
        {
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, Sphere.ComputeCustomWeaponName(this));
        }

        public override bool OnEquip(Mobile from)
        {
            if (from.Skills[SkillName.Tactics].Base < 100.0)
            {
                from.PrivateOverheadMessage(MessageType.Regular, 0x3b2, true, "You don't know enough about tactics to equip this", from.NetState);
                return false;
            }

            return base.OnEquip(from);
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