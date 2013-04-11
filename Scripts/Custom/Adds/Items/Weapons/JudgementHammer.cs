using Server.Network;
using Server.Regions;

namespace Server.Items
{
    public class JudgementHammer : WarHammer
    {
        [Constructable]
        public JudgementHammer()
        {
            Hue = 1154;
            Name = "Judgement Hammer";
            Weight = 10.0;
            Speed = 545;
            MinDamage = 20;
            MaxDamage = 43;
            AccuracyLevel = WeaponAccuracyLevel.Exceedingly;
            DamageLevel = WeaponDamageLevel.Power;
            DurabilityLevel = WeaponDurabilityLevel.Regular;
            IsRenamed = true;
        }

        public JudgementHammer(Serial serial)
            : base(serial)
        {
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, Sphere.ComputeCustomWeaponName(this));
        }

        public override int OldStrengthReq
        {
            get { return 90; }
        }

        public override void OnHit(Mobile attacker, Mobile defender)
        {
            CustomRegion cR = defender.Region as CustomRegion;

            if (cR == null || cR.Controller.AllowSpecialAttacks)
            {
                if (Utility.Random(20) <= 2) // 10% chance of scoring a critical hit
                {
                    attacker.SendAsciiMessage("You score a critical hit!");
                    defender.PublicOverheadMessage(MessageType.Emote, 34, false, string.Format("*Critical hit!*"));
                    defender.BoltEffect(0);
                    defender.Hits -= 10;
                }
            }

            base.OnHit(attacker, defender);
        }

        public override bool OnEquip(Mobile from)
        {
            if (from.Skills[SkillName.Tactics].Base < 100.0)
            {
                from.PrivateOverheadMessage(MessageType.Regular, 0x3b2, true, "You don't know enough about tactics to equip this", from.NetState);
                return false;
            }

            if (from.Skills[SkillName.Macing].Base < 100.0)
            {
                from.PrivateOverheadMessage(MessageType.Regular, 0x3b2, true, "You don't know enough about macefighting to equip this", from.NetState);
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