using Server.Items;
using Server.Targeting;
using System; //Loki edit: added for TimeSpan

namespace Server.Spells.Second
{
    public class HarmSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Second; } }
        public override int ManaCost { get { return 11; } } //Loki edit: New PvP changes
        public override int Sound { get { return 0x1F1; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Harm", "An Mani",
				212,
				Core.AOS ? 9001 : 9041,
				Reagent.Nightshade,
				Reagent.SpidersSilk
			);

		public HarmSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        //Loki edit: New PvP changes
        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.1);
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
			else if ( CheckHSequence( m ) )
			{
				SpellHelper.CheckReflect( (int)Circle, Caster, ref m );

                //Loki edit: New PvP changes
                double damage = 21 + ((int)(GetDamageSkill(Caster) - GetResistSkill(m)) / 12);

				m.FixedEffect ( 0x374A, 1, 15 );
				m.PlaySound( Sound );
				SpellHelper.Damage( this, m, damage, 0, 0, 100, 0, 0 );
			}
			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly HarmSpell m_Owner;

			public InternalTarget( HarmSpell owner ) : base( 12, false, TargetFlags.Harmful )
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
