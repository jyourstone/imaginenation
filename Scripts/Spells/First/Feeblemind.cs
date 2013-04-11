using System;
using Server.Items;
using Server.Spells.Third;
using Server.Targeting;

namespace Server.Spells.First
{
    public class FeeblemindSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override int Sound { get { return 0x1E4; } }

        //public override bool SpellDisabled { get { return true; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Feeblemind", "Rel Wis",
				212,
				9031,
				Reagent.Ginseng,
				Reagent.Nightshade
			);

		public FeeblemindSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckHSequence( m ) )
			{
                SpellHelper.CheckReflect((int)Circle, Caster, ref m);

                if (m.Spell != null)
                    m.Spell.OnCasterHurt();

                if (BlessSpell.UnderEffect(m))
                {
                    if (m.Int > m.RawInt)
                        m.RemoveStatMod(String.Format("[Magic] {0} Offset", StatType.Int));
                    else
                        m.SendAsciiMessage("You are under the effect of a bless spell and cannot get any stat penalties");
                }
                else
                    SpellHelper.AddStatCurse(Caster, m, StatType.Int);

				//m.Paralyzed = false;
                m.FixedParticles( 0x374A, 10, 15, 5004, EffectLayer.Head );
				m.PlaySound( Sound );

                HarmfulSpell(m);
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly FeeblemindSpell m_Owner;

			public InternalTarget( FeeblemindSpell owner ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile)o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}