using Server.Network;
using Solaris.CliLocHandler;

namespace Server.Items
{
    public class BaseShield : BaseArmor
    {
        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

        public BaseShield(int itemID)
            : base(itemID)
        {
        }

        public BaseShield(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version < 1)
            {
                if (this is Aegis)
                    return;

                // The 15 bonus points to resistances are not applied to shields on OSI.
                PhysicalBonus = 0;
                FireBonus = 0;
                ColdBonus = 0;
                PoisonBonus = 0;
                EnergyBonus = 0;
            }
        }

        public override double ArmorRating
        {
            get
            {
                Mobile m = Parent as Mobile;
                double ar = base.ArmorRating;

                if (m != null)
                    return (ar * (m.Skills[SkillName.Parry].Value / 100.0));
                else
                    return ar;
            }
        }

        public override int OnHit(BaseWeapon weapon, int damage)
        {
            Mobile owner = Parent as Mobile;

            if (owner == null)
                return damage;

            double chance = (owner.Skills[SkillName.Parry].Value) / (15 / ((owner.Skills[SkillName.Parry].Value) / 100));
            chance /= 100;

            if (chance < 0.005)
                chance = 0.005;

            //gains
            owner.CheckSkill(SkillName.Parry, chance);

            if (Utility.RandomDouble() <= chance)
            {
                damage = 0;
                owner.SendAsciiMessage("You parry the blow.");
            }

            if (Durability != ArmorDurabilityLevel.Indestructible && 3 > Utility.Random(100)) // 3%(1.5 due to code below) chance to lower durability if not invul
            {
                int wear = Utility.Random(2);

                if (wear > 0)
                {
                    if (HitPoints > wear)
                    {
                        HitPoints -= wear;

                        if (Parent is Mobile && HitPoints * 6 < MaxHitPoints) //Display message at 1/6th of the hp
                            ((Mobile)Parent).LocalOverheadMessage(MessageType.Regular, 0x3B2, 1061121); // Your equipment is severely damaged.

                    }
                    else
                    {
                        if (Parent is Mobile)
                            ((Mobile)Parent).PublicOverheadMessage(MessageType.Emote, 0x22, true, string.Format("*{0}'s {1} is destroyed", ((Mobile)Parent).Name, string.IsNullOrEmpty(Name) ? CliLoc.LocToString(LabelNumber) : Name));

                        Delete();
                    }
                }
            }

            return damage;
        }
    }
}
