using System;
using Server.Items;
using Server.Spells.Fourth;
using Server.Targeting;

namespace Server.Spells.Second
{
    public class CunningSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Second; } }
        public override int Sound { get { return 0x1EB; } }

        //public override bool SpellDisabled { get { return true; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Cunning", "Uus Wis",
				263,
				9061,
				Reagent.MandrakeRoot,
				Reagent.Nightshade
			);

		public CunningSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
                if ((m.Player && m.Int <= 120) || (!m.Player))
                {
                    if (CurseSpell.UnderEffect(m))
                    {
                        if (m.Int < m.RawInt)
                            m.RemoveStatMod(String.Format("[Magic] {0} Offset", StatType.Int));
                        else
                            m.SendAsciiMessage("You are under the effect of a curse spell and cannot get any stat bonuses");
                    }
                    else
                        SpellHelper.AddStatBonus(Caster, m, StatType.Int);
                }
                else
                {
                    m.SendAsciiMessage(33, "You are too intelligent to benefit from that!");
                    return;
                }

				m.FixedParticles( 0x373A, 10, 15, 5011, EffectLayer.Head );
				m.PlaySound( Sound );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly CunningSpell m_Owner;

			public InternalTarget( CunningSpell owner ) : base( 12, false, TargetFlags.Beneficial )
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