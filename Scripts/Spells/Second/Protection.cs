using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Spells.Second
{
    public class ProtectionSpell : MagerySpell
    {
        private static readonly Dictionary<Mobile, Timer> m_Timers = new Dictionary<Mobile, Timer>();

        public static int ArmorModValue { get { return 6; } }

        public override SpellCircle Circle { get { return SpellCircle.Second; } }
        public override int Sound { get { return 493; } }

        private static readonly SpellInfo m_Info = new SpellInfo(
                "Protection", "Uus Sanct",
                263,
                9011,
                Reagent.Garlic,
                Reagent.Ginseng,
                Reagent.SulfurousAsh
            );

        public ProtectionSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnPlayerCast()
        {
            if (SphereSpellTarget is Mobile)
                Target((Mobile)SphereSpellTarget);
            else if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }
            else
                DoFizzle();
        }

        public override void OnCast()
        {
            Target(Caster);
        }

        public void Target(Mobile m)
        {
            if (CheckSequence())
                ApplyProtectionEffect(m, Caster);

            FinishSequence();
        }

        public static Dictionary<Mobile, Timer> Registry
        {
            get { return m_Timers; }
        }

        public static void ApplyProtectionEffect(Mobile target)
        {
            ApplyProtectionEffect(target, null);
        }

        public static void ApplyProtectionEffect(Mobile target, Mobile caster)
        {
            if (caster != null && !caster.CanBeBeneficial(target, false))
                return;

            //Stop the old protection timer, if we have any.
            StopTimer(target);

            // Temporary "easy way out" fix for stacking protection on server restarts.
            // I'll get to fixing this properly soon. I know the problems it causes
            // but this can't be abused as much in the meantime - Malik
            //if (target is PlayerMobile)
            //    StopTimer(target);

            target.VirtualArmor += ArmorModValue;

            Timer protectionTimer = new InternalTimer(target);
            protectionTimer.Start();

            //Register it with the list
            m_Timers.Add(target, protectionTimer);

            target.FixedParticles(0x373A, 10, 15, 5016, EffectLayer.Waist);
            target.PlaySound(493);
        }
        
        public static bool StopTimer(Mobile m)
        {
            if (!m_Timers.ContainsKey(m))
                return false;

            //Stop the ticking timer
            m_Timers[m].Stop();
                m.VirtualArmor -= ArmorModValue;
            
            if (m.VirtualArmor < 0)
            {
                m.VirtualArmor = 0;
            }

            //Remove it from our dictionary
            m_Timers.Remove(m);

            return true;
        }

        private class InternalTimer : Timer
        {
            private readonly Mobile m_Owner;

            public InternalTimer(Mobile owner)
                : base(TimeSpan.Zero)
            {
                m_Owner = owner;

                double val = m_Owner.Skills[SkillName.Magery].Value * 2.4;

                Delay = TimeSpan.FromSeconds(val);
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                //Restore our mobile to its previous state
                m_Owner.VirtualArmor -= ArmorModValue;
                if (m_Owner.VirtualArmor < 0)
                {
                    m_Owner.VirtualArmor = 0;
                }
                //Remove the mobile and the timer from the dictionary
                m_Timers.Remove(m_Owner);
            }
        }
    }
}
