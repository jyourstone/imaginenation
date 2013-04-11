using Server.Items;
using Server.Targeting;

namespace Server.Spells.Second
{
    public class RemoveTrapSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Second; } }
        public override int Sound { get { return 0x1F0; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Remove Trap", "An Jux",
				263,
				9001,
				Reagent.Bloodmoss,
				Reagent.SulfurousAsh
			);

		public RemoveTrapSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        public override void OnPlayerCast()
        {
            if (SphereSpellTarget is TrapableContainer)
                Target((TrapableContainer)SphereSpellTarget);
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

		public void Target( TrapableContainer item )
		{
			if ( !Caster.CanSee( item ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( item.TrapType != TrapType.None && item.TrapType != TrapType.MagicTrap )
			{
				base.DoFizzle();
			}
			else if ( CheckSequence() )
			{

				Point3D loc = item.GetWorldLocation();

				Effects.SendLocationParticles( EffectItem.Create( loc, item.Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, 5015 );
				Effects.PlaySound( loc, item.Map, Sound );

				item.TrapType = TrapType.None;
				item.TrapPower = 0;
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly RemoveTrapSpell m_Owner;

			public InternalTarget( RemoveTrapSpell owner ) : base( 12, false, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is TrapableContainer )
				{
					m_Owner.Target( (TrapableContainer)o );
				}
				else
				{
					from.SendMessage( "You can't disarm that" );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}