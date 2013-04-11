using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Seventh
{
    public class MeteorSwarmSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Seventh; } }
        public override int Sound { get { return 0x160; } }
        
		private static readonly SpellInfo m_Info = new SpellInfo(
				"Meteor Swarm", "Flam Kal Des Ylem",
				263,
				9042,
				true,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot,
				Reagent.SulfurousAsh,
				Reagent.SpidersSilk
			);

		public MeteorSwarmSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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

		public override bool DelayedDamage{ get{ return false; } }

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckSequence() )
			{
				double damage;

				if ( Core.AOS )
					damage = GetNewAosDamage( 48, 1, 5, true );
				else
					damage = Utility.Random( 27, 22 );

				ArrayList targets = new ArrayList();

				Map map = Caster.Map;

				if ( map != null )
				{
					IPooledEnumerable eable = map.GetMobilesInRange( new Point3D( m ), 2 );

					foreach ( Mobile mob in eable )
					{
						if ( Caster != mob && SpellHelper.ValidIndirectTarget( Caster, mob ) && Caster.CanBeHarmful( mob, false ) && Caster.InLOS( mob ) )
							targets.Add( mob );
					}

					eable.Free();
				}

				if ( targets.Count > 0 )
				{
					Effects.PlaySound( m, Caster.Map, Sound );

					if ( Core.AOS && targets.Count > 1 )
						damage = (damage * 2) / targets.Count;
					else if ( !Core.AOS )
						damage /= targets.Count;

					for ( int i = 0; i < targets.Count; ++i )
					{
						Mobile mob = (Mobile)targets[i];

						double toDeal = damage;

						if ( !Core.AOS && CheckResisted( mob ) )
						{
							toDeal *= 0.5;

							m.SendMessage("You feel yourself resisting magic"); // You feel yourself resisting magical energy.
						}

						Caster.DoHarmful( mob );
						SpellHelper.Damage( this, mob, toDeal, 0, 100, 0, 0, 0 );

						Caster.MovingParticles( mob, 0x36D4, 7, 0, false, true, 9501, 1, 0, 0x100 );
					}
				}
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly MeteorSwarmSpell m_Owner;

			public InternalTarget( MeteorSwarmSpell owner ) : base( 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}