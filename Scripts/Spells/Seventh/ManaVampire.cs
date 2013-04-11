using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Seventh
{
    public class ManaVampireSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Seventh; } }
        public override int Sound { get { return 0x1F9; } }
        
		private static readonly SpellInfo m_Info = new SpellInfo(
				"Mana Vampire", "Ort Sanct",
				212,
				9032,
				Reagent.BlackPearl,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

		public ManaVampireSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			if(Caster is PlayerMobile){
					if ( SphereSpellTarget is Mobile && Caster.InLOS( SphereSpellTarget ) )
					{
						Target( (Mobile)SphereSpellTarget );
					}
					else{

						FinishSequence();
						Caster.PlaySound( 0x5C );
					}
			}
			else
				Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{

			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
            else if (m is PlayerMobile)
            {
                Caster.SendAsciiMessage("You can't cast this spell on players.");
            }
            else if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }
			else if ( CheckHSequence( m ) )
			{

				SpellHelper.CheckReflect( (int)Circle, Caster, ref m );

				if ( m.Spell != null )
					m.Spell.OnCasterHurt();

				m.Paralyzed = false;

				int toDrain = 0;

				if ( Core.AOS )
				{
					toDrain = (int)(GetDamageSkill( Caster ) - GetResistSkill( m ));

					if ( !m.Player )
						toDrain /= 2;

					if ( toDrain < 0 )
						toDrain = 0;
					else if ( toDrain > m.Mana )
						toDrain = m.Mana;
				}
				else
				{
					if ( CheckResisted( m ) )
						m.SendMessage("You feel yourself resisting magic"); // You feel yourself resisting magical energy.
					else
						toDrain = m.Mana;
				}

				if ( toDrain > (Caster.ManaMax - Caster.Mana) )
					toDrain = Caster.ManaMax - Caster.Mana;

				m.Mana -= toDrain;
				Caster.Mana += toDrain;

				if ( Core.AOS )
				{
					m.FixedParticles( 0x374A, 1, 15, 5054, 23, 7, EffectLayer.Head );
					m.PlaySound( Sound );

					Caster.FixedParticles( 0x0000, 10, 5, 2054, EffectLayer.Head );
				}
				else
				{
					m.FixedParticles( 0x374A, 10, 15, 5054, EffectLayer.Head );
					m.PlaySound( Sound );
				}

                HarmfulSpell(m);
			}

			FinishSequence();
		}

		public override double GetResistPercent( Mobile target )
		{
			return 98.0;
		}

		private class InternalTarget : Target
		{
			private readonly ManaVampireSpell m_Owner;

			public InternalTarget( ManaVampireSpell owner ) : base( 12, false, TargetFlags.Harmful )
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