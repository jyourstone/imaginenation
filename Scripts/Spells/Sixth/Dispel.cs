using Server.Items;
using Server.Mobiles;
using Server.Spells.Fifth;
using Server.Spells.Second;
using Server.Spells.Seventh;
using Server.Targeting;

namespace Server.Spells.Sixth
{
    public class DispelSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Sixth; } }
        public override int Sound { get { return 513; } }
        
		private static readonly SpellInfo m_Info = new SpellInfo(
				"Dispel", "An Ort",
				212,
				9002,
				Reagent.Garlic,
				Reagent.MandrakeRoot,
				Reagent.SulfurousAsh
			);

		public DispelSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
            Caster.Target = new InternalTarget(this);
		}

        public void Target(Mobile m)
        {
            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckHSequence(m))
            {
                if (m is BaseGuard)
                {
                    if (((BaseGuard)m).Disappears)
                        m.OnBeforeDeath();
                    else
                        Caster.SendAsciiMessage("You can't dispel that guard!");
                }
                else if (m is BaseCreature)
                    RemoveAllEffects((BaseCreature)m, Caster);
                else
                    RemoveAllEffects(m, Caster);

                m.PlaySound(Sound);
            }

            FinishSequence();
        }

        public static void RemoveAllEffects(Mobile target)
        {
            RemoveAllEffects(target, null);
        }

        public static void RemoveAllEffects(BaseCreature target)
        {
            RemoveAllEffects(target, null);
        }

        public static void RemoveAllEffects(Mobile target, Mobile caster)
        {
            //Remove magic reflect
            if (target.MagicDamageAbsorb > 0)
                target.MagicDamageAbsorb = 0;

            //dispel protection
            ProtectionSpell.StopTimer(target);

            //dispell polymorph
            PolymorphSpell.StopTimer(target);

            //dispell incognito
            IncognitoSpell.StopTimer(target);
        }

        public static void RemoveAllEffects(BaseCreature target, Mobile caster)
        {
            //We need to make sure that the creature has no timers on before we delete it
            RemoveAllEffects((Mobile)target, caster);

            if (target.Summoned && !target.IsAnimatedDead)
            {
                Effects.PlaySound(target, target.Map, 513);
                Effects.PlaySound(target, target.Map, 510);
                Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x3728, 20, 20, 2023);
                target.Delete();
            }
        }

		public class InternalTarget : Target
		{
			private readonly DispelSpell m_Owner;

			public InternalTarget( DispelSpell owner ) : base( 12, false, TargetFlags.None )
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