using Server.Items;
using Server.Regions;
using Server.Targeting;

namespace Server.Spells.Third
{
    public class TeleportSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override int Sound { get { return 0x1FE; } }
        
        public override bool CanTargetGround { get { return true; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Teleport", "Rel Por",
				263,
				9031,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot
			);

		public TeleportSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
            if (Caster.Region is HouseRegion)
            {
                Caster.SendAsciiMessage("You cannot cast that spell here!");
                return false;
            }

		    return SpellHelper.CheckTravel( Caster, TravelCheckType.TeleportFrom );
		}

        public override void OnPlayerCast()
        {
            Target(SphereSpellTarget);
        }

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( object op )
		{
            IPoint3D p = op as IPoint3D;
            Mobile mobileTarget = op as Mobile;

			IPoint3D orig = p;
			Map map = Caster.Map;

			SpellHelper.GetSurfaceTop( ref p );

			if ( !SpellHelper.CheckTravel( Caster, TravelCheckType.TeleportFrom ) )
			{
			}
			else if ( !SpellHelper.CheckTravel( Caster, map, new Point3D( p ), TravelCheckType.TeleportTo ) )
			{
			}
            else if (mobileTarget == null && ( map == null || !map.CanSpawnMobile(p.X, p.Y, p.Z)))
            {
                Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
            }
            else if (Region.Find(new Point3D(p),map) is HouseRegion)
            {
                Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
            }
			else if ( SpellHelper.CheckMulti( new Point3D( p ), map ) )
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
            else if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }
			else if ( CheckSequence() )
			{
                if (op != null && Caster.InLOS(p) && Caster.InRange(new Point2D(p.X,p.Y),14))
                {
                    Mobile m = Caster;

                    Point3D from = m.Location;
                    Point3D to = new Point3D(p);

                    m.Location = to;
                    m.RevealingAction();
                    m.ProcessDelta();

                    Effects.SendLocationParticles(EffectItem.Create(from, m.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                    Effects.SendLocationParticles(EffectItem.Create(to, m.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

                    m.PlaySound(Sound);

                    IPooledEnumerable eable = m.GetItemsInRange(0);

                    foreach (Item item in eable)
                    {
                        if (item is Server.Spells.Sixth.ParalyzeFieldSpell.InternalItem || item is Server.Spells.Fifth.PoisonFieldSpell.InternalItem || item is Server.Spells.Fourth.FireFieldSpell.InternalItem)
                            item.OnMoveOver(m);
                    }

                    eable.Free();
                }
                else
                    Caster.SendAsciiMessage("You can't see that target!");
			}

			FinishSequence();
		}

		public class InternalTarget : Target
		{
			private readonly TeleportSpell m_Owner;

			public InternalTarget( TeleportSpell owner ) : base( 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				IPoint3D p = o as IPoint3D;

				if ( p != null )
					m_Owner.Target( o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
