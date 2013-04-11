using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class DragonsBlade : Broadsword
    {
        [Constructable]
        public DragonsBlade()
        {
            Hue = 2881;
            Name = "Dragon's Blade";
            Weight = 5.0;
            Speed = 350;
            MinDamage = 14;
            MaxDamage = 28;
            AccuracyLevel = WeaponAccuracyLevel.Exceedingly;
            DamageLevel = WeaponDamageLevel.Vanq;
            DurabilityLevel = WeaponDurabilityLevel.Substantial;
            IsRenamed = true;
        }

        public DragonsBlade(Serial serial)
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

    public class SuperiorDragonsBlade : Broadsword
    {
        [Constructable]
        public SuperiorDragonsBlade()
        {
            Hue = 2881;
            Name = "Superior Dragon's Blade";
            Weight = 6.0;
            Speed = 340;
            MinDamage = 15;
            MaxDamage = 30;
            AccuracyLevel = WeaponAccuracyLevel.Supremely;
            DamageLevel = WeaponDamageLevel.Vanq;
            DurabilityLevel = WeaponDurabilityLevel.Substantial;
            IsRenamed = true;
            Slayer = SlayerName.DragonSlaying;
        }

        public SuperiorDragonsBlade(Serial serial)
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