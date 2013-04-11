using Server.Network;

namespace Server.Items
{
    public class DwarvenBattleAxe : BattleAxe
    {
        public override int GetSwingAnim(Mobile from)
        {
            if (from.Mounted)
                return 26;
            return Utility.RandomList(12, 13, 14);
        }

        [Constructable]
        public DwarvenBattleAxe()
        {
            Hue = 1942;
            Name = "Dwarven Battle Axe";
            Speed = 428;
            MinDamage = 20;
            MaxDamage = 34;
            AccuracyLevel = WeaponAccuracyLevel.Eminently;
            DamageLevel = WeaponDamageLevel.Power;
            DurabilityLevel = WeaponDurabilityLevel.Substantial;
            IsRenamed = true;
            Slayer = SlayerName.Ophidian;
        }

        public DwarvenBattleAxe(Serial serial)
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