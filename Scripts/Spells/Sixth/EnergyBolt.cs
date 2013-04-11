using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Sixth
{
    public class EnergyBoltSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Sixth; } }
        public override int Sound { get { return 0x20A; } }
        
        private static readonly SpellInfo m_Info = new SpellInfo(
                "Energy Bolt", "Corp Por",
                212,
                9022,
                Reagent.BlackPearl,
                Reagent.Nightshade
            );

        public EnergyBoltSpell(Mobile caster, Item scroll)
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
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            if (!Caster.CanSee(m))
            {
                Caster.SendAsciiMessage("Target is not in line of sight.");
                DoFizzle();
            }

            if (CheckHSequence(m))
            {
                Mobile source = Caster;

                SpellHelper.CheckReflect((int)Circle, ref source, ref m);

                double damage = GetSphereDamage(Caster, m, Scroll != null ? 41 : 44);

                #region Taran - Damage based on AR
                if (m is PlayerMobile)
                {
                    double armorRating = ((PlayerMobile)m).BaseArmorRatingSpells;

                    damage = (damage * 2) - armorRating;

                    if (damage > 49)
                        damage = 49;

                    if (damage < 20)
                        damage = 20;
                }
                #endregion

                // Do the effects
                source.MovingParticles(m, 0x379F, 7, 0, false, true, 3043, 4043, 0x211);
                source.PlaySound(Sound);

                // Deal the damage
                SpellHelper.Damage(this, m, damage, 0, 0, 0, 0, 100);
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private readonly EnergyBoltSpell m_Owner;

            public InternalTarget(EnergyBoltSpell owner)
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
