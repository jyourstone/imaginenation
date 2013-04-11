using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Fourth
{
    public class ManaDrainSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override int Sound { get { return 0x1F8; } }
        public override int ManaCost { get { return 9; } } //Loki edit

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Mana Drain", "Ort Rel",
				212,
				9031,
				Reagent.BlackPearl,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

		public ManaDrainSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        //Loki edit: Scroll and spell both cast at same speed
        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.00);
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

		private static readonly Dictionary<Mobile, Timer> m_Table = new Dictionary<Mobile, Timer>();

		private void AosDelay_Callback( object state )
		{
			object[] states = (object[])state;

			Mobile m = (Mobile)states[0];
			int mana = (int)states[1];

			if ( m.Alive && !m.IsDeadBondedPet )
			{
				m.Mana += mana;

                //Loki edit: Half mana awarded on return if over mana cap
                if (m.Mana > m.ManaMax)
                    m.Mana = m.ManaMax + (int)(0.5 * (m.Mana - m.ManaMax));

                m.FixedEffect(0x374A, 10, 15);
				m.PlaySound( 504 );
			}

			m_Table.Remove( m );
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

                if (Core.AOS)
                {
                    int toDrain = 40 + (int)(GetDamageSkill(Caster) - GetResistSkill(m));

                    if (toDrain < 0)
                        toDrain = 0;
                    else if (toDrain > m.Mana)
                        toDrain = m.Mana;

                    if (m_Table.ContainsKey(m))
                        toDrain = 0;

                    m.FixedParticles(0x3789, 10, 25, 5032, EffectLayer.Head);
                    m.PlaySound(0x1F8);

                    if (toDrain > 0)
                    {
                        m.Mana -= toDrain;

                        m_Table[m] = Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerStateCallback(AosDelay_Callback), new object[] { m, toDrain });
                    }
                }
                else
                {
                    int toDrain = 18 + (int)((GetDamageSkill(Caster) - GetResistSkill(m)) / 10);

                    if (!(m is PlayerMobile))
                        toDrain += 32;

                    if (toDrain < 0)
                        toDrain = 0;
                    else if (toDrain > m.Mana)
                        toDrain = m.Mana;

                    if (m_Table.ContainsKey(m))
                    {
                        toDrain = 0;
                        Caster.SendMessage("You feel the target's mana has already been drained!");
                    }

                    if (toDrain > 0)
                    {
                        m.Mana -= toDrain;

                        m_Table[m] = Timer.DelayCall(TimeSpan.FromSeconds(12.0), new TimerStateCallback(AosDelay_Callback), new object[] { m, toDrain });
                    }


                    m.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                    m.PlaySound(Sound);
                }

                HarmfulSpell(m);
			}

			FinishSequence();
		}

		public override double GetResistPercent( Mobile target )
		{
			return 99.0;
		}

		private class InternalTarget : Target
		{
			private readonly ManaDrainSpell m_Owner;

			public InternalTarget( ManaDrainSpell owner ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target( (Mobile)o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}