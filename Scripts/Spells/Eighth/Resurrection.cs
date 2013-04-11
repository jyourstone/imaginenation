using Server.Items;
using Server.Multis;
using Server.Regions;
using Server.Targeting;

namespace Server.Spells.Eighth
{
    public class ResurrectionSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Eighth; } }
        public override int Sound { get { return 0x214; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Resurrection", "An Corp",
				263,
				9062,
				Reagent.Bloodmoss,
				Reagent.Garlic,
				Reagent.Ginseng
			);

		public ResurrectionSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( m == Caster )
			{
				Caster.SendLocalizedMessage( 501039 ); // Thou can not resurrect thyself.
			}
			else if ( !Caster.Alive )
			{
				Caster.SendLocalizedMessage( 501040 ); // The resurrecter must be alive.
			}
			else if ( m.Alive )
			{
				Caster.SendLocalizedMessage( 501041 ); // Target is not dead.
			}
			else if ( !Caster.InRange( m, 10 ) )//XUO Chro xuo respell style
			{
				Caster.SendLocalizedMessage( 501042 ); // Target is not close enough.
			}
			else if ( !m.Player )
			{
				Caster.SendLocalizedMessage( 501043 ); // Target is not a being.
			}
			else if ( m.Map == null || !m.Map.CanFit( m.Location, 16, false, false ) )
			{
				Caster.SendLocalizedMessage( 501042 ); // Target can not be resurrected at that location.
				m.SendLocalizedMessage( 502391 ); // Thou can not be resurrected there!
			}
			else if ( CheckBSequence( m, true ) )
			{
                 BaseHouse house = null;

                if (m.Region is HouseRegion)
                    house = (m.Region as HouseRegion).House;

                if (house != null)
                {
                    //if (house.IsCoOwner(m) || house.IsFriend(m) || house.IsOwner(m))
                    //{
                    //    m.PlaySound(0x214);
                    //    m.FixedEffect(0x376A, 10, 16);
                    //    m.Resurrect();
                    //}
                    //else if (house.IsCoOwner(Caster) || house.IsOwner(Caster))
                    //{
                    //    m.PlaySound(0x214);
                    //    m.FixedEffect(0x376A, 10, 16);
                    //    m.Resurrect();
                    //}
                    //else
                    //{
                        m.SendAsciiMessage("You cannot be resurrected in a house!");
                        Caster.SendAsciiMessage("You cannot resurrect in a house!");
                    //}
                }
                else
                {
                    m.PlaySound(Sound);
                    m.FixedEffect(0x376A, 10, 15);
                    m.Resurrect();
                }
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly ResurrectionSpell m_Owner;

			public InternalTarget( ResurrectionSpell owner ) : base( 1, false, TargetFlags.Beneficial )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile)o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
