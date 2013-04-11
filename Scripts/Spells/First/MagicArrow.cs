using Server.Items;
using Server.Targeting;

namespace Server.Spells.First
{
    public class MagicArrowSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override int Sound { get { return 0x1E5; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Magic Arrow", "In Por Ylem",
				212,
				9041,
				Reagent.SulfurousAsh
			);

		public MagicArrowSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        public override void OnPlayerCast()
        {
            if (SphereSpellTarget is Mobile)
                Target((Mobile)SphereSpellTarget);
            else if (SphereSpellTarget is BaseExplosionPotion)
                iTarget((BaseExplosionPotion)SphereSpellTarget);
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

		public override bool DelayedDamage{ get{ return false; } }//XUO Chro blah useless

        public void iTarget(BaseExplosionPotion pot) //Taran: When casted on explosion pots they explode
        {
            if (!Caster.CanSee(pot))
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            else if (CheckSequence())
            {
                Mobile source = Caster;

                pot.Explode(source, true, pot.GetWorldLocation(), pot.Map);

                source.MovingParticles(pot, 0x36E4, 5, 0, false, true, 3006, 4006, 0);
                source.PlaySound(Sound);

                
            }
            FinishSequence();                    
        }

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
            else if (CheckHSequence(m))
            {
                Mobile source = Caster;

                SpellHelper.CheckReflect((int)Circle, ref source, ref m);

                double damage;

                if (Scroll != null)
                    damage = 8 + ((int)(GetDamageSkill(Caster) - GetResistSkill(m)) / 15);
                else
                    damage = 1 + ((int)(GetDamageSkill(Caster) - GetResistSkill(m)) / 15);

                source.MovingParticles(m, 0x36E4, 5, 0, false, false, 3006, 3006, 0, 0);
                source.PlaySound(Sound);

                SpellHelper.Damage(this, m, damage, 0, 0, 100, 0, 0);
            }
			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly MagicArrowSpell m_Owner;

			public InternalTarget( MagicArrowSpell owner ) : base( 12, false, TargetFlags.Harmful )
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
