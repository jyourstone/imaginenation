using Server.Items;
using Server.Spells.Second;
using Server.Targeting;

namespace Server.Spells.Fourth
{
    public class ArchProtectionSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override int Sound { get { return 493; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Arch Protection", "Vas Uus Sanct",
				Core.AOS ? 239 : 215,
				9011,
				Reagent.Garlic,
				Reagent.Ginseng,
				Reagent.MandrakeRoot,
				Reagent.SulfurousAsh
			);

		public ArchProtectionSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }
            else if (CheckSequence())
            {
                SpellHelper.GetSurfaceTop(ref p);

                Map map = Caster.Map;

                if (map != null)
                {
                    IPooledEnumerable eable = map.GetMobilesInRange(new Point3D(p), Core.AOS ? 2 : 3);

                    foreach (Mobile m in eable)
                        ProtectionSpell.ApplyProtectionEffect(m, Caster);

                    eable.Free();
                }
            }

            FinishSequence();
        }

		private class InternalTarget : Target
		{
			private readonly ArchProtectionSpell m_Owner;

			public InternalTarget( ArchProtectionSpell owner ) : base( 12, true, TargetFlags.None )
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
