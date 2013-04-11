using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Sixth
{
    public class InvisibilitySpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Sixth; } }
        public override int Sound { get { return 515; } }
        
        private static readonly SpellInfo m_Info = new SpellInfo(
                "Invisibility", "An Lor Xen",
                263,
                9002,
                Reagent.Bloodmoss,
                Reagent.Nightshade
            );

        public InvisibilitySpell(Mobile caster, Item scroll)
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

            else if (CheckBSequence(m))
            {
                //Effects.SendLocationParticles(EffectItem.Create(new Point3D(m.X, m.Y, m.Z + 16), Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5045);
                m.PlaySound(Sound);

                m.Hidden = true;

                if (m is PlayerMobile && m.Account.AccessLevel == AccessLevel.Player)  //Taran: Check account level to prevent issues with speech for staff when toggling GM
                    (m as PlayerMobile).HiddenWithSpell = true;
            }

            FinishSequence();
        }

        public class InternalTarget : Target
        {
            private readonly InvisibilitySpell m_Owner;

            public InternalTarget(InvisibilitySpell owner)
                : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    m_Owner.Target((Mobile)o);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}