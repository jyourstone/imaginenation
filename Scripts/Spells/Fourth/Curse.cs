using System;
using System.Collections;
using Server.Items;
using Server.Spells.Third;
using Server.Targeting;

namespace Server.Spells.Fourth
{
    public class CurseSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override int Sound { get { return 481; } }

        //public override bool SpellDisabled  {  get { return true; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Curse", "Des Sanct",
				227,
				9031,
				Reagent.Nightshade,
				Reagent.Garlic,
				Reagent.SulfurousAsh
			);

		public CurseSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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

		private static readonly Hashtable m_UnderEffect = new Hashtable();

		public static void RemoveEffect( object state )
		{
			Mobile m = (Mobile)state;

			m_UnderEffect.Remove( m );

			m.UpdateResistances();
		}

		public static bool UnderEffect( Mobile m )
		{
			return m_UnderEffect.Contains( m );
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
                    bool message = true;

                    if (m.Str > m.RawStr)
                    {
                        m.RemoveStatMod(String.Format("[Magic] {0} Offset", StatType.Str));
                        message = false;
                    }
                    if (m.Dex > m.RawDex)
                    {
                        m.RemoveStatMod(String.Format("[Magic] {0} Offset", StatType.Dex));
                        message = false;
                    }
                    if (m.Int > m.RawInt)
                    {
                        m.RemoveStatMod(String.Format("[Magic] {0} Offset", StatType.Int));
                        message = false;
                    }

                    if (message)
                        m.SendAsciiMessage("You are under the effect of a bless spell and cannot get any stat penalties");
                }
                else
                {
                    SpellHelper.AddStatCurse(Caster, m, StatType.Str);
                    SpellHelper.DisableSkillCheck = true;
                    SpellHelper.AddStatCurse(Caster, m, StatType.Dex);
                    SpellHelper.AddStatCurse(Caster, m, StatType.Int);
                    SpellHelper.DisableSkillCheck = false;
                }

			    TimeSpan duration = SpellHelper.GetDuration(Caster, m);
                m_UnderEffect[m] = Timer.DelayCall(duration, new TimerStateCallback(RemoveEffect), m);
                m.UpdateResistances();

				//m.Paralyzed = false;
                m.FixedParticles(0x374A, 10, 15, 5028, EffectLayer.Waist);
				m.PlaySound( Sound );

                int percentage = (int)(SpellHelper.GetOffsetScalar(Caster, m, true) * 100);
                TimeSpan length = SpellHelper.GetDuration(Caster, m);

                string args = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", percentage, percentage, percentage, 10, 10, 10, 10);

                BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Curse, 1075835, 1075836, length, m, args));

                HarmfulSpell(m);
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly CurseSpell m_Owner;

			public InternalTarget( CurseSpell owner ) : base( 12, false, TargetFlags.Harmful )
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