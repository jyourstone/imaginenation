using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Fourth
{
    public class ArchCureSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override int Sound { get { return 488; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Arch Cure", "Vas An Nox",
				263,
				9061,
				Reagent.Garlic,
				Reagent.Ginseng,
				Reagent.MandrakeRoot
			);

		public ArchCureSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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

				List<Mobile> targets = new List<Mobile>();

				Map map = Caster.Map;
                Mobile directTarget = p as Mobile;

                if (map != null)
                {
                    bool feluccaRules = (map.Rules == MapRules.FeluccaRules);

                    // You can target any living mobile directly, beneficial checks apply
                    if (directTarget != null && Caster.CanBeBeneficial(directTarget, false))
                        targets.Add(directTarget);

                    IPooledEnumerable eable = map.GetMobilesInRange(new Point3D(p), 2);

                    foreach (Mobile m in eable)
                    {
                        if (m == directTarget)
                            continue;

                        if (AreaCanTarget(m, feluccaRules))
                            targets.Add(m);
                    }

                    eable.Free();
                }

                Effects.PlaySound(p, Caster.Map, Sound);

				if ( targets.Count > 0 )
				{
					int cured = 0;

					for ( int i = 0; i < targets.Count; ++i )
					{
						Mobile m = targets[i];

						Caster.DoBeneficial( m );

						Poison poison = m.Poison;

						if ( poison != null )
						{
							int chanceToCure = 10000 + (int)(Caster.Skills[SkillName.Magery].Value * 75) - ((poison.Level + 1) * 1750);
							chanceToCure /= 100;

							if ( m.CurePoison( Caster ) )
								++cured;
						}

						//m.FixedParticles( 0x373A, 10, 15, 5012, EffectLayer.Waist );
						m.PlaySound( Sound );
					}

					//if ( cured > 0 )
					//	Caster.SendLocalizedMessage( 1010058 ); // You have cured the target of all poisons!
				}
			}

			FinishSequence();
		}

        private bool AreaCanTarget(Mobile target, bool feluccaRules)
        {
            /* Arch cure area effect won't cure aggressors, victims, murderers, criminals or monsters.
             * In Felucca, it will also not cure summons and pets.
             * For red players it will only cure themselves and guild members.
             */

            if (!Caster.CanBeBeneficial(target, false))
                return false;

            if (Core.AOS && target != Caster)
            {
                if (IsAggressor(target) || IsAggressed(target))
                    return false;

                if ((!IsInnocentTo(Caster, target) || !IsInnocentTo(target, Caster)) && !IsAllyTo(Caster, target))
                    return false;

                if (feluccaRules && !(target is PlayerMobile))
                    return false;
            }

            return true;
        }

		private bool IsAggressor( Mobile m )
		{
			foreach ( AggressorInfo info in Caster.Aggressors )
			{
				if ( m == info.Attacker && !info.Expired )
					return true;
			}

			return false;
		}

		private bool IsAggressed( Mobile m )
		{
			foreach ( AggressorInfo info in Caster.Aggressed )
			{
				if ( m == info.Defender && !info.Expired )
					return true;
			}

			return false;
		}

        private static bool IsInnocentTo(Mobile from, Mobile to)
        {
            return (Notoriety.Compute(from, (Mobile)to) == Notoriety.Innocent);
        }

        private static bool IsAllyTo(Mobile from, Mobile to)
        {
            return (Notoriety.Compute(from, (Mobile)to) == Notoriety.Ally);
        }

		private class InternalTarget : Target
		{
			private readonly ArchCureSpell m_Owner;

			public InternalTarget( ArchCureSpell owner ) : base( 12, true, TargetFlags.None )
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