using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;

namespace Server.Spells.Fifth
{
    public class MagicReflectSpell : MagerySpell
    {
        private static readonly Dictionary<Mobile, Timer> m_Timers = new Dictionary<Mobile, Timer>();

        public override SpellCircle Circle { get { return SpellCircle.Fifth; } }
        public override int Sound { get { return 488; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Magic Reflection", "In Jux Sanct",
				263,
				9012,
				Reagent.Garlic,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

		public MagicReflectSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			return true;
		}

        public static Dictionary<Mobile, Timer> Registry
        {
            get { return m_Timers; }
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
                //Stop the old reflect timer, if we have any.
                StopTimer(m);

				m.MagicDamageAbsorb = 15; //to make sure it will reflect any spell.

                Timer protectionTimer = new InternalTimer(m);
                protectionTimer.Start();

                //Register it with the list
                m_Timers.Add(m, protectionTimer);

                m.FixedParticles(0x373A, 10, 15, 5016, EffectLayer.Waist);
				m.PlaySound( Sound );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly MagicReflectSpell m_Owner;

			public InternalTarget( MagicReflectSpell owner ) : base( 12, false, TargetFlags.Beneficial )
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

        public static bool StopTimer(Mobile m)
        {
            if (!m_Timers.ContainsKey(m))
                return false;

            //Stop the ticking timer
            m_Timers[m].Stop();
            m.MagicDamageAbsorb = 0;

            //Remove it from our dictionary
            m_Timers.Remove(m);

            return true;
        }

        private class InternalTimer : Timer
        {
            private readonly Mobile m_Owner;

            public InternalTimer(Mobile owner)
                : base(TimeSpan.Zero)
            {
                m_Owner = owner;

                double val = m_Owner.Skills[SkillName.Magery].Value * 2.4;

                Delay = TimeSpan.FromSeconds(val);
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                //Remove reflect
                m_Owner.MagicDamageAbsorb = 0;

                //Remove the mobile and the timer from the dictionary
                m_Timers.Remove(m_Owner);
            }
        }
	}
}
