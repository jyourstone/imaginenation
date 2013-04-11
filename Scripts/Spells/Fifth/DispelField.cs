using System;
using Server.Items;
using Server.Misc;
using Server.Targeting;

namespace Server.Spells.Fifth
{
	public class DispelFieldSpell : MagerySpell
	{
        public override SpellCircle Circle { get { return SpellCircle.Fifth; } }
        public override int Sound { get { return 528; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Dispel Field", "An Grav",
				263,
				9002,
				Reagent.BlackPearl,
				Reagent.SpidersSilk,
				Reagent.SulfurousAsh,
				Reagent.Garlic
			);

		public DispelFieldSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        public override void OnPlayerCast()
        {
            if (SphereSpellTarget is Item)
                Target((Item)SphereSpellTarget);
            else
                DoFizzle();
        }

		public override void OnCast()
		{
            Caster.Target = new InternalTarget( this );
		}

		public void Target( Item item )
		{
			Type t = item.GetType();

			if ( !Caster.CanSee( item ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( !t.IsDefined( typeof( DispellableFieldAttribute ), false ) )
			{
				Caster.SendLocalizedMessage( 1005049 ); // That cannot be dispelled.
			}
			else if ( item is Moongate && !((Moongate)item).Dispellable )
			{
				Caster.SendLocalizedMessage( 1005047 ); // That magic is too chaotic
			}
            else if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }
			else if ( CheckSequence() )
			{

				//Effects.SendLocationParticles( EffectItem.Create( item.Location, item.Map, EffectItem.DefaultDuration ), 0x376A, 9, 20, 5042 );
				Effects.PlaySound( item.GetWorldLocation(), item.Map, Sound );

				item.Delete();
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly DispelFieldSpell m_Owner;

			public InternalTarget( DispelFieldSpell owner ) : base( 12, false, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Item )
				{
					m_Owner.Target( (Item)o );
				}
				else
				{
					m_Owner.Caster.SendLocalizedMessage( 1005049 ); // That cannot be dispelled.
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}