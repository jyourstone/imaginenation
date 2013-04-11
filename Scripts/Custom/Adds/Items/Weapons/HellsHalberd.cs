using Server.Mobiles;
using Server.Network;
using Server.Regions;

namespace Server.Items
{
    public class HellsHalberd : Halberd
    {
        [Constructable]
        public HellsHalberd()
        {
            Hue = 1171;
            Name = "Hell's Halberd";
            Weight = 12;
            Speed = 583;
            MinDamage = 19;
            MaxDamage = 45;
            AccuracyLevel = WeaponAccuracyLevel.Supremely;
            DamageLevel = WeaponDamageLevel.Vanq;
            DurabilityLevel = WeaponDurabilityLevel.Substantial;
            IsRenamed = true;
        }

        public HellsHalberd(Serial serial)
            : base(serial)
        {
        }

        public override int OldStrengthReq
        {
            get { return 90; }
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, Sphere.ComputeCustomWeaponName(this));
        }

        public override void OnHit(Mobile attacker, Mobile defender)
        {
            CustomRegion cR = defender.Region as CustomRegion;

            if (cR == null || cR.Controller.AllowSpecialAttacks)
            {
                if (Utility.Random(28) <= 2) // 7% chance of scoring a critical hit
                {
                    attacker.SendAsciiMessage("You score a critical hit!");
                    defender.PublicOverheadMessage(MessageType.Emote, 34, false, string.Format("*Critical hit!*"));
                    defender.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                    defender.PlaySound(283);

                    int damage = 15;

                    #region Damage based on AR
                    if (defender is PlayerMobile)
                    {
                        int basedamage = damage;
                        damage = (damage * 4) - (int)defender.ArmorRating - 5;

                        //Minimum damage 15
                        if (damage < basedamage)
                            damage = basedamage;

                        //Maximum damage 30
                        if (damage > 30)
                            damage = 30;
                    }
                    else //Always deal maxdamage when fighting monsters
                        damage = 30;
                    #endregion

                    defender.Hits -= damage;
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

            if (from.Skills[SkillName.Swords].Base < 100.0)
            {
                from.PrivateOverheadMessage(MessageType.Regular, 0x3b2, true, "You don't know enough about swordfighting to equip this", from.NetState);
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