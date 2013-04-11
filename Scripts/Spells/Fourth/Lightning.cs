using Server.Items;
using Server.Targeting;
using System; //Loki edit: added to support change in cast speed

namespace Server.Spells.Fourth
{
    public class LightningSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override int Sound { get { return 0x29; } }
        public override int ManaCost { get { return 10; } } //Loki edit

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Lightning", "Por Ort Grav",
				263,
				9021,
				Reagent.MandrakeRoot,
				Reagent.SulfurousAsh
			);

		public LightningSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        //Loki edit: New PvP changes
        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
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

            if ( CheckHSequence( m ) )
			{
				SpellHelper.CheckReflect( (int)Circle, Caster, ref m );

			    double damage = 16 + ((int)(GetDamageSkill(Caster) - GetResistSkill(m)) / 12);

				m.BoltEffect( 0 );

				SpellHelper.Damage( this, m, damage, 0, 0, 0, 0, 100 );
			    m.PlaySound(m.GetHurtSound());
			    m.Animate(!m.Mounted ? 20 : 29, 5, 1, true, false, 0);
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly LightningSpell m_Owner;

			public InternalTarget( LightningSpell owner ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target( (Mobile)o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
