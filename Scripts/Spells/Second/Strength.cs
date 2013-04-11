using System;
using Server.Items;
using Server.Spells.Fourth;
using Server.Targeting;

namespace Server.Spells.Second
{
    public class StrengthSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Second; } }
        public override int Sound { get { return 0x1EE; } }

        //public override bool SpellDisabled { get { return true; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Strength", "Uus Mani",
				263,
				9061,
				Reagent.MandrakeRoot,
				Reagent.Nightshade
			);

		public StrengthSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
			else if ( CheckBSequence( m ) )
			{
                if ((m.Player && m.Str <= 120) || (!m.Player))
                {
                    if (CurseSpell.UnderEffect(m))
                    {
                        if (m.Str < m.RawStr)
                            m.RemoveStatMod(String.Format("[Magic] {0} Offset", StatType.Str));
                        else
                            m.SendAsciiMessage("You are under the effect of a curse spell and cannot get any stat bonuses");
                    }
                    else
                        SpellHelper.AddStatBonus(Caster, m, StatType.Str);
                }
                else
                {
                    m.SendAsciiMessage(33, "You are too strong to benefit from this spell!");
                    return;
                }

				m.FixedParticles( 0x373A, 10, 15, 5017, EffectLayer.Waist );
				m.PlaySound( Sound );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly StrengthSpell m_Owner;

			public InternalTarget( StrengthSpell owner ) : base( 12, false, TargetFlags.Beneficial )
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