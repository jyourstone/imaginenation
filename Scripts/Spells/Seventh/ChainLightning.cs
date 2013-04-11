using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Seventh
{
    public class ChainLightningSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Seventh; } }
        public override int Sound { get { return 0x29; } }
        
        private static readonly SpellInfo m_Info = new SpellInfo(
				"Chain Lightning", "Vas Ort Grav",
				263,
				9022,
				true,
				Reagent.BlackPearl,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot,
				Reagent.SulfurousAsh
			);

		public ChainLightningSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			if(Caster is PlayerMobile)
				Target( (IPoint3D)SphereSpellTarget );
			else
				Caster.Target = new InternalTarget( this );
		}

		public override bool DelayedDamage{ get{ return false; } }

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

				if ( p is Item )
					p = ((Item)p).GetWorldLocation();

			    //if ( Core.AOS )
				//	damage = GetNewAosDamage( 48, 1, 5, true );

                double damage = Scroll != null ? Utility.Random(35, 32) : Utility.Random(40, 35);

				ArrayList targets = new ArrayList();

				Map map = Caster.Map;

				if ( map != null )
				{
					IPooledEnumerable eable = map.GetMobilesInRange( new Point3D( p ), 2 );

					foreach ( Mobile m in eable )
					{
						if ( Core.AOS && m == Caster )
							continue;

						if ( SpellHelper.ValidIndirectTarget( Caster, m ) && Caster.CanBeHarmful( m, false ) && Caster.InLOS(m))
							targets.Add( m );
					}

					eable.Free();
				}

				if ( targets.Count > 0 )
				{
					if ( Core.AOS && targets.Count > 1 )
						damage = (damage * 2) / targets.Count;
					else if ( !Core.AOS )
						damage /= targets.Count;

					for ( int i = 0; i < targets.Count; ++i )
					{
						Mobile m = (Mobile)targets[i];

						double toDeal = damage;

						if ( !Core.AOS && CheckResisted( m ) )
						{
							toDeal *= 0.5;

							m.SendMessage("You feel yourself resisting magic"); // You feel yourself resisting magical energy.
						}

						Caster.DoHarmful( m );
						SpellHelper.Damage( this, m, toDeal, 0, 0, 0, 0, 100 );

						m.BoltEffect( 0 );
					}
				}

                else
                {
                    Caster.PlaySound(Sound);
                }
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly ChainLightningSpell m_Owner;

			public InternalTarget( ChainLightningSpell owner ) : base( 12, true, TargetFlags.None )
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