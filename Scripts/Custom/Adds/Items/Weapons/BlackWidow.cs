using System;
using Server.Network;
using Server.Regions;

namespace Server.Items
{
    public class BlackWidow : HeavyCrossbow
    {
        [Constructable]
        public BlackWidow()
        {
            Hue = 1174;
            Name = "Black Widow";
            Weight = 8;
            Speed = 510;
            MinDamage = 25;
            MaxDamage = 36;
            AccuracyLevel = WeaponAccuracyLevel.Supremely;
            DamageLevel = WeaponDamageLevel.Vanq;
            DurabilityLevel = WeaponDurabilityLevel.Substantial;
            IsRenamed = true;
        }

        public BlackWidow(Serial serial)
            : base(serial)
        {
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
                if (Utility.Random(20) <= 2) // 10% chance of scoring a critical hit
                {
                    attacker.SendAsciiMessage("You poison your target!");
                    defender.PublicOverheadMessage(MessageType.Emote, 34, false, string.Format("*{0} suddenly feels very ill*", defender.Name));
                    defender.ApplyPoison(attacker, Poison.GetPoison(0));
                    new InternalTimer(defender).Start();
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

            if (from.Skills[SkillName.Archery].Base < 100.0)
            {
                from.PrivateOverheadMessage(MessageType.Regular, 0x3b2, true, "You don't know enough about archery to equip this", from.NetState);
                return false;
            }

            return base.OnEquip(from);
        }

        private class InternalTimer : Timer
        {
            private readonly Mobile m_Defender;
            private int m_PoisonCount = 4;

            public InternalTimer(Mobile defender)
                : base(TimeSpan.FromSeconds(3.0), TimeSpan.FromSeconds( 3.0 ))
            {
                m_Defender = defender;
            }

            protected override void OnTick()
            {
                m_PoisonCount--;

                if (m_PoisonCount == 0 || !m_Defender.Alive || !m_Defender.Poisoned)
                {
                    if (m_Defender.Poisoned)
                        m_Defender.CurePoison(m_Defender);

                    Stop();
                }

                else
                {
                    m_Defender.Hits -= 10;
                    m_Defender.SendAsciiMessage("You feel very ill!");

                    if (m_Defender.Body.IsMale)
                        m_Defender.PlaySound(Utility.RandomList(340, 341, 342, 343, 344, 345));
                    else if (m_Defender.Body.IsFemale)
                        m_Defender.PlaySound(Utility.RandomList(331, 332, 333, 334, 335));

                    if (!m_Defender.Mounted)
                        m_Defender.Animate(20, 5, 1, true, false, 0);
                    else
                        m_Defender.Animate(29, 5, 1, true, false, 0);
                }
            }
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