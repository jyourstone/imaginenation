using System;
using Server.Items;
using Server.Spells.Third;
using Server.Targeting;

namespace Server.Spells.First
{
    public class ClumsySpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override int Sound { get { return 0x1DF; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Clumsy", "Uus Jux",
				212,
				9031,
				Reagent.Bloodmoss,
				Reagent.Nightshade
			);

        public ClumsySpell(Mobile caster, Item scroll)
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

                if (BlessSpell.UnderEffect(m))
                {
                    if (m.Dex > m.RawDex)
                        m.RemoveStatMod(String.Format("[Magic] {0} Offset", StatType.Dex));
                    else
                        m.SendAsciiMessage("You are under the effect of a bless spell and cannot get any stat penalties");
                }
                else
                    SpellHelper.AddStatCurse(Caster, m, StatType.Dex);
                
                m.FixedParticles( 0x374A, 10, 15, 5002, EffectLayer.Head );
				m.PlaySound( Sound );

                //Any better way for this? //Clumsy is supposed to be a harmfull spell
                if (m.Spell != null)
                    m.Spell.OnCasterHurt();

                HarmfulSpell(m);
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly ClumsySpell m_Owner;

			public InternalTarget( ClumsySpell owner ) : base( 12, false, TargetFlags.Harmful )
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
