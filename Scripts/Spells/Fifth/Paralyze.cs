using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Fifth
{
    public class ParalyzeSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Fifth; } }
        public override int Sound { get { return 0x204; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Paralyze", "An Ex Por",
				212,
				9012,
				Reagent.Garlic,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

		public ParalyzeSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
			else if ( Core.AOS && (m.Frozen || m.Paralyzed || (m.Spell != null && m.Spell.IsCasting)) )
			{
				Caster.SendLocalizedMessage( 1061923 ); // The target is already frozen.
			}
			else if ( CheckHSequence( m ) )
			{
			    SpellHelper.CheckReflect((int) Circle, Caster, ref m);

                if (m.Spell != null)
                    m.Spell.OnCasterHurt();

				double duration;
				
				if ( Core.AOS )
				{
                    int secs = (int)((GetDamageSkill(Caster) / 10) - (GetResistSkill(m) / 10));

					if ( !m.Player )
						secs *= 3;

					if ( secs < 0 )
						secs = 0;

					duration = secs;
				}
				else
				{

                    //Loki edit: duration = 120.0 + (Caster.Skills.Magery.Value/2);
                    duration = 30.0 + (Caster.Skills.Magery.Value / 4);

					if ( CheckResisted( m ) )
						duration *= 0.65; //Loki edit: Was 0.85
				}

                if (m is PlagueBeastLord)
                {
                    ((PlagueBeastLord)m).OnParalyzed(Caster);
                    duration = 120;
                }

                if (m is BaseCreature && ((BaseCreature)m).ParalyzeImmune)
                    m.PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "The paralyze spell seems to have no effect");
                else
                    m.Paralyze(TimeSpan.FromSeconds(duration));

				m.PlaySound( Sound );
                m.FixedParticles(0x374A, 10, 15, 5028, EffectLayer.Waist);

                HarmfulSpell(m);
			}

			FinishSequence();
		}

		public class InternalTarget : Target
		{
			private readonly ParalyzeSpell m_Owner;

			public InternalTarget( ParalyzeSpell owner ) : base( 12, false, TargetFlags.Harmful )
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
