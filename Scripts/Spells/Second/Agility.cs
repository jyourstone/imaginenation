using System;
using Server.Items;
using Server.Spells.Fourth;
using Server.Targeting;

namespace Server.Spells.Second
{
    public class AgilitySpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Second; } }
        public override int Sound { get { return 487; } }

        //public override bool SpellDisabled { get { return true; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Agility", "Ex Uus",
				263,
				9061,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot
			);

		public AgilitySpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
                if ((m.Player && m.Dex <= 120) || (!m.Player))
                {
                    if (CurseSpell.UnderEffect(m))
                    {
                        if (m.Dex < m.RawDex)
                            m.RemoveStatMod(String.Format("[Magic] {0} Offset", StatType.Dex));
                        else
                            m.SendAsciiMessage("You are under the effect of a curse spell and cannot get any stat bonuses");
                    }
                    else
                        SpellHelper.AddStatBonus(Caster, m, StatType.Dex);
                }
                else
                {
                    m.SendAsciiMessage(33, "You are too fast to benefit from that!");
                    return;
                }

				m.FixedParticles( 0x373A, 10, 15, 5010, EffectLayer.Waist );
				m.PlaySound( Sound );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly AgilitySpell m_Owner;

			public InternalTarget( AgilitySpell owner ) : base( 12, false, TargetFlags.Beneficial )
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