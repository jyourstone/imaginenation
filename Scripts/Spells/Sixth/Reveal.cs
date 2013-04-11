using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Spells.Sixth
{
    public class RevealSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Sixth; } }
        public override int Sound { get { return 0x1FD; } }
        
        public override bool CanTargetGround { get { return true; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Reveal", "Wis Quas",
				263,
				9002,
				Reagent.Bloodmoss,
				Reagent.SulfurousAsh
			);

		public RevealSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        public override void OnPlayerCast()
        {
            Target((IPoint3D)SphereSpellTarget);
        }

		public override void OnCast()
		{
            Caster.Target = new InternalTarget( this );
		}

		public void Target( IPoint3D p )
		{
			if ( !Caster.CanSee( p ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
            else if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }
			else if ( CheckSequence() )
			{
				SpellHelper.GetSurfaceTop( ref p );

				ArrayList targets = new ArrayList();

				Map map = Caster.Map;

				if ( map != null )
				{
					IPooledEnumerable eable = map.GetMobilesInRange( new Point3D( p ), 1 + (int)(Caster.Skills[SkillName.Magery].Value / 20.0) );

					foreach ( Mobile m in eable )
					{
                        //This should enable detecting in pits
                        if (m.Hidden && (m.AccessLevel == AccessLevel.Player || Caster.AccessLevel > m.AccessLevel) && CheckDifficulty(Caster, m))
                            targets.Add(m);
					}

					eable.Free();
				}

				for ( int i = 0; i < targets.Count; ++i )
				{
					Mobile m = (Mobile)targets[i];

					m.RevealingAction();

					m.FixedParticles( 0x376A, 10, 15, 5049, EffectLayer.Head );
					//m.PlaySound( 0x1FD );
				}
                //Ize - always play a sound
                Effects.PlaySound(p,map,Sound);
			}

			FinishSequence();
		}

        // Not sure we want it like this - Maka 
        private static bool CheckDifficulty(Mobile from, Mobile m)
        {
            return true;

            // Reveal always reveals vs. invisibility spell 
            /*
            if (!Core.AOS || InvisibilitySpell.HasTimer(m))
                return true;

            int magery = from.Skills[SkillName.Magery].Fixed;
            int detectHidden = from.Skills[SkillName.DetectHidden].Fixed;

            int hiding = m.Skills[SkillName.Hiding].Fixed;
            int stealth = m.Skills[SkillName.Stealth].Fixed;
            int divisor = hiding + stealth;

            int chance;
            if (divisor > 0)
                chance = 50 * (magery + detectHidden) / divisor;
            else
                chance = 100;

            return chance > Utility.Random(100);
            */
        }

		public class InternalTarget : Target
		{
			private readonly RevealSpell m_Owner;

			public InternalTarget( RevealSpell owner ) : base( 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				IPoint3D p = o as IPoint3D;

				if ( p != null )
					m_Owner.Target( p );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}