using Server.Items;
using Server.Targeting;
using System; //Loki edit: For TimeSpan

namespace Server.Spells.Second
{
    public class CureSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Second; } }
        public override int Sound { get { return 0x1E0; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Cure", "An Nox",
                263,
				9061,
				Reagent.Garlic,
				Reagent.Ginseng
			);

		public CureSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        //Loki edit: New PvP changes
        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(0.9);
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

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckBSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				Poison p = m.Poison;

				if ( p != null )
				{
					int chanceToCure = 10000 + (int)(Caster.Skills[SkillName.Magery].Value * 75) - ((p.Level + 1) * (Core.AOS ? (p.Level < 4 ? 3300 : 3100) : 1750));
					chanceToCure /= 100;

					if ( chanceToCure > Utility.Random( 100 ) )
					{
						if ( m.CurePoison( Caster ) )
						{
							if ( Caster != m )
								Caster.SendLocalizedMessage( 1010058 ); // You have cured the target of all poisons!

							m.SendLocalizedMessage( 1010059 ); // You have been cured of all poisons.
						}
					}
					else
					{
						m.SendLocalizedMessage( 1010060 ); // You have failed to cure your target!
					}
				}

                m.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist); //Loki edit: Restored animation

				m.PlaySound( Sound );
			}

			FinishSequence();
		}

		public class InternalTarget : Target
		{
			private readonly CureSpell m_Owner;

			public InternalTarget( CureSpell owner ) : base( 12, false, TargetFlags.Beneficial )
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