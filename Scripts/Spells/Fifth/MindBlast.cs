using System;
using Server.Items;
using Server.Targeting;

namespace Server.Spells.Fifth
{
    public class MindBlastSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Fifth; } }
        public override int Sound { get { return 0x213; } }

        public override int ManaCost { get { return 17; } } //Loki edit

        private static readonly SpellInfo m_Info = new SpellInfo(
                "Mind Blast", "Por Corp Wis",
                212,
                Core.AOS ? 9002 : 9032,
                Reagent.BlackPearl,
                Reagent.MandrakeRoot,
                Reagent.Nightshade,
                Reagent.SulfurousAsh
            );

        public MindBlastSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
            if (Core.AOS)
                m_Info.LeftHandEffect = m_Info.RightHandEffect = 9002;
        }

        //Loki edit: Scroll and spell both cast same speed
        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.75);
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
            Caster.Target = new InternalTarget(this);
        }

        private void AosDelay_Callback(object state)
        {
            object[] states = (object[])state;
            Mobile caster = (Mobile)states[0];
            Mobile target = (Mobile)states[1];
            Mobile defender = (Mobile)states[2];
            int damage = (int)states[3];

            if (caster.HarmfulCheck(defender))
            {
                SpellHelper.Damage(this, target, Utility.RandomMinMax(damage, damage + 4), 0, 0, 100, 0, 0);

                target.FixedParticles(0x374A, 10, 15, 5038, 1181, 2, EffectLayer.Head);
                target.PlaySound(Sound);
            }
        }

        public override bool DelayedDamage { get { return !Core.AOS; } }

        public void Target(Mobile m)
        {
            if (!Caster.CanSee(m))
            {
                Caster.SendAsciiMessage("Target is not in line of sight."); DoFizzle();//One line so i could use VS Replace, feel free to change/remove comment:p
            }
            else if (Core.AOS)
            {
                if (Caster.CanBeHarmful(m) && CheckSequence())
                {
                    Mobile from = Caster, target = m;


                    SpellHelper.CheckReflect((int)Circle, ref from, ref target);

                    int damage = GetSphereDamage(Caster, m, 35);

                    if (damage > 60)
                        damage = 60;
                    Timer.DelayCall(TimeSpan.FromSeconds(0.1),
                        new TimerStateCallback(AosDelay_Callback),
                        new object[] { Caster, target, m, damage });
                }
            }
            else if (CheckHSequence(m))
            {
                Mobile from = Caster, target = m;

                SpellHelper.Turn(from, target);

                SpellHelper.CheckReflect((int)Circle, ref from, ref target);

                //Loki edit: New PvP changes
                double damage = 30 + ((int)(GetDamageSkill(Caster) - GetResistSkill(m)) / 10);
                int drain = 3;
                if (m is Mobiles.PlayerMobile && m.RawDex != m.RawInt)
                {
                    double statscalar = (Math.Max(m.RawDex, m.RawInt) - Math.Min(m.RawDex, m.RawInt)) / 8; //Loki: 10 was too high, 6 was too low...
                    damage += statscalar;
                    drain += 1;
                }
                m.Mana -= drain;
            }
            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private readonly MindBlastSpell m_Owner;

            public InternalTarget(MindBlastSpell owner)
                : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}

