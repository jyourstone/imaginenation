using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Seventh
{
    public class MassDispelSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Seventh; } }
        public override int Sound { get { return 521; } }
        
		private static readonly SpellInfo m_Info = new SpellInfo(
				"Mass Dispel", "Vas An Ort",
				212,
				9002,
				Reagent.Garlic,
				Reagent.MandrakeRoot,
				Reagent.BlackPearl,
				Reagent.SulfurousAsh
			);

		public MassDispelSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        public override void OnPlayerCast()
        {
            if (SphereSpellTarget is IPoint3D)
                Target((IPoint3D)SphereSpellTarget);
            else
                DoFizzle();
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
            else if (CheckSequence())
            {
                SpellHelper.GetSurfaceTop(ref p);

                List<Mobile> targets = new List<Mobile>();

                Map map = Caster.Map;

                if (map != null)
                {
                    IPooledEnumerable eable = map.GetMobilesInRange(new Point3D(p), 8);

                    foreach (Mobile m in eable)
                    {
                        targets.Add(m);
                    }

                    eable.Free();
                }

                foreach (Mobile m in targets)
                {
                    if (m is BaseCreature)
                        Spells.Sixth.DispelSpell.RemoveAllEffects((BaseCreature)m, Caster);
                    else
                        Spells.Sixth.DispelSpell.RemoveAllEffects(m, Caster);
                }
            }

            Caster.PlaySound(Sound);
			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly MassDispelSpell m_Owner;

			public InternalTarget( MassDispelSpell owner ) : base( 12, true, TargetFlags.None )
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