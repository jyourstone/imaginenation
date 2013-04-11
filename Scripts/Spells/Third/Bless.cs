using System;
using System.Collections;
using Server.Items;
using Server.Spells.Fourth;
using Server.Targeting;

namespace Server.Spells.Third
{
    public class BlessSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override int Sound { get { return 0x1EA; } }
        
        //public override bool SpellDisabled { get { return true; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Bless", "Rel Sanct",
				212,
				9061,
				Reagent.Garlic,
				Reagent.MandrakeRoot
			);

		public BlessSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        public override void OnPlayerCast()
        {
            if (SphereSpellTarget is Mobile)
                Target((Mobile)SphereSpellTarget);
            else
                DoFizzle();
        }

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

        private static readonly Hashtable m_UnderEffect = new Hashtable();

        public static void RemoveEffect(object state)
        {
            Mobile m = (Mobile)state;

            m_UnderEffect.Remove(m);

            m.UpdateResistances();
        }

        public static bool UnderEffect(Mobile m)
        {
            return m_UnderEffect.Contains(m);
        }

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
                Caster.SendAsciiMessage("Target is not in line of sight."); DoFizzle();//One line so i could use VS Replace, feel free to change/remove comment:p
			}
            else if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }
			else if ( CheckBSequence( m ) )
			{
                if ((m.Player && m.Int <= 120 && m.Str <= 120 && m.Dex <= 120 && m.RawStatTotal <= 300) || (!m.Player))
                {
                    if (CurseSpell.UnderEffect(m))
                    {
                        bool message = true;

                        if (m.Str < m.RawStr)
                        {
                            m.RemoveStatMod(String.Format("[Magic] {0} Offset", StatType.Str));
                            message = false;
                        }
                        if (m.Dex < m.RawDex)
                        {
                            m.RemoveStatMod(String.Format("[Magic] {0} Offset", StatType.Dex));
                            message = false;
                        }
                        if (m.Int < m.RawInt)
                        {
                            m.RemoveStatMod(String.Format("[Magic] {0} Offset", StatType.Int));
                            message = false;
                        }

                        if (message)
                            m.SendAsciiMessage("You are under the effect of a curse spell and cannot get any stat bonuses");
                    }
                    else
                    {
                        SpellHelper.AddStatBonus(Caster, m, StatType.Str);
                        SpellHelper.DisableSkillCheck = true;
                        SpellHelper.AddStatBonus(Caster, m, StatType.Dex);
                        SpellHelper.AddStatBonus(Caster, m, StatType.Int);
                        SpellHelper.DisableSkillCheck = false;
                    }
                }

				m.FixedParticles( 0x373A, 10, 15, 5018, EffectLayer.Waist );
				m.PlaySound( Sound );

                int percentage = (int)(SpellHelper.GetOffsetScalar(Caster, m, false) * 100);
                TimeSpan length = SpellHelper.GetDuration(Caster, m);

                m_UnderEffect[m] = Timer.DelayCall(length, new TimerStateCallback(RemoveEffect), m);

                string args = String.Format("{0}\t{1}\t{2}", percentage, percentage, percentage);

			    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Bless, 1075847, 1075848, length, m, args));
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly BlessSpell m_Owner;

            public InternalTarget(BlessSpell owner) : base(Core.ML ? 10 : 12, false, TargetFlags.Beneficial)
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