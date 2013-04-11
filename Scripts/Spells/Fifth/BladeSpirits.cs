using System;
using Server.Items;
using Server.Mobiles;
using Server.Regions;
using Server.Targeting;

namespace Server.Spells.Fifth
{
	public class BladeSpiritsSpell : MagerySpell
	{
        public override SpellCircle Circle { get { return SpellCircle.Fifth; } }
        public override int Sound { get { return 0x212; } }

        public override bool CanTargetGround { get { return true; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Blade Spirits", "In Jux Hur Ylem", 
				263,
				9040,
				true,
				Reagent.BlackPearl,
				Reagent.MandrakeRoot,
				Reagent.Nightshade
			);

		public BladeSpiritsSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

            /**
             * - Check Disabled -
            if( (Caster.Followers + (Core.SE ? 2 : 1)) > Caster.FollowersMax )
            {
                Caster.SendLocalizedMessage( 1049645 ); // You have too many followers to summon that creature.
                return false;
            }
            **/

			return true;
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
			Map map = Caster.Map;

			SpellHelper.GetSurfaceTop( ref p );

            if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }
            else if ((map == null || !map.CanSpawnMobile(p.X, p.Y, p.Z)) && !(SphereSpellTarget is Mobile))
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
			else if (/*SpellHelper.CheckTown( p, Caster ) && */CheckSequence())
			{
				TimeSpan duration;

				if ( Core.AOS )
					duration = TimeSpan.FromSeconds( 120 );
				else
					duration = TimeSpan.FromSeconds( Utility.Random( 180, 120 ) );

                if (Caster.InLOS(p))
                {
                    GuardedRegion reg = (GuardedRegion)Region.Find(new Point3D(p), Caster.Map).GetRegion(typeof(GuardedRegion));
                    if (reg != null && !reg.Disabled)
                        Caster.CriminalAction(true);
                    BaseCreature.Summon(new BladeSpirits(), false, Caster, new Point3D(p), Sound, duration);
                }
                else
                    Caster.SendAsciiMessage("You can't see that!");
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private BladeSpiritsSpell m_Owner;

			public InternalTarget( BladeSpiritsSpell owner ) : base( 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is IPoint3D )
					m_Owner.Target( (IPoint3D)o );
			}

			protected override void OnTargetOutOfLOS( Mobile from, object o )
			{
				from.SendLocalizedMessage( 501943 ); // Target cannot be seen. Try again.
				from.Target = new InternalTarget( m_Owner );
				from.Target.BeginTimeout( from, TimeoutTime - DateTime.Now );
				m_Owner = null;
			}

			protected override void OnTargetFinish( Mobile from )
			{
				if ( m_Owner != null )
					m_Owner.FinishSequence();
			}
		}
	}
}
