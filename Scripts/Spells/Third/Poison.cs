using Server.Items;
using Server.Targeting;
using System; //Loki edit: For TimeSpan

namespace Server.Spells.Third
{
    public class PoisonSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override int Sound { get { return 517; } }
        
		private static readonly SpellInfo m_Info = new SpellInfo(
				"Poison", "In Nox",
                212,
				9051,
				Reagent.Nightshade
			);

		public PoisonSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
            /* Loki edit: Removing broken precast mana cost
			if (scroll is SpellScroll)
			{
				Caster.Mana -= 4;
			}
             */
		}

        //Loki edit: Scroll and spell cast at same speed
        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
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
				SpellHelper.CheckReflect( (int)Circle, Caster, ref m );

				if ( m.Spell != null )
					m.Spell.OnCasterHurt();

				m.Paralyzed = false;

				if ( Caster.Skills.Poisoning.Fixed < 1000 && CheckResisted( m ) )
				{
					m.SendMessage("You feel yourself resisting magic"); // You feel yourself resisting magical energy.
				}
				else
				{
					int level;

					if ( Core.AOS )
					{
						if ( Caster.InRange( m, 2 ) )
						{
							int total = (Caster.Skills.Magery.Fixed + Caster.Skills.Poisoning.Fixed) / 2;

							if ( total >= 1000 )
								level = 3;
							else if ( total > 850 )
								level = 2;
							else if ( total > 650 )
								level = 1;
							else
								level = 0;
						}
						else
						{
							level = 0;
						}
					}
					else
					{
						double total = Caster.Skills[SkillName.Magery].Value + Caster.Skills[SkillName.Poisoning].Value;

						double dist = Caster.GetDistanceToSqrt( m );

						if ( dist >= 3.0 )
							total -= (dist - 3.0) * 10.0;

						if ( total >= 200.0 && (Core.AOS || Utility.Random( 1, 100 ) <= 33) )
							level = 3;
						else if ( total > (Core.AOS ? 170.1 : 170.0) )
							level = 2;
						else if ( total > (Core.AOS ? 130.1 : 130.0) )
							level = 1;
						else
							level = 0;
					}

                     m.ApplyPoison(Caster, Poison.GetPoison(level));
                }

                m.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                m.PlaySound(Sound);

                HarmfulSpell(m);
			}
			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly PoisonSpell m_Owner;

			public InternalTarget( PoisonSpell owner ) : base( 12, false, TargetFlags.Harmful )
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