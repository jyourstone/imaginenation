using Server.Items;
using Server.Targeting;
using System; //Loki edit: Added for TimeSpan
using Server.Mobiles; //Loki edit: Added for AR dependence

namespace Server.Spells.Third
{
    public class FireballSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override int Sound { get { return 0x15f; } }
        
		private static readonly SpellInfo m_Info = new SpellInfo(
				"Fireball", "Vas Flam",
				212,
				9041,
				Reagent.BlackPearl
			);

		public FireballSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        //Loki edit: Scroll and spell both cast at same speed
        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.25);
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

        public void iTarget(BaseExplosionPotion pot) //Taran: When casted on explosion pots they explode
        {
            if (!Caster.CanSee(pot))
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            else if (CheckSequence())
            {
                Mobile source = Caster;

                pot.Explode(source, true, pot.GetWorldLocation(), pot.Map);

                source.MovingParticles(pot, 0x36D4, 7, 0, false, true, 9502, 4019, 0x160);
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

            else if (CheckHSequence(m)) //Loki edit: New PvP changes
            {
                Mobile source = Caster;

                SpellHelper.CheckReflect((int)Circle, ref source, ref m);

                double damage = 14 + ((int)(GetDamageSkill(Caster) - GetResistSkill(m)) / 10);

                if (m is PlayerMobile)
                {
                    double aR = ((PlayerMobile)m).BaseArmorRatingSpells;

                    if (aR < 30)
                        damage += 8;
                    else if (aR < 40)
                        damage += 4;
                    else if (aR < 45)
                        damage += 2;
                }

                source.MovingParticles(m, 0x36D4, 7, 0, false, true, 9502, 4019, 0x160);
                source.PlaySound(Core.AOS ? 0x15E : 0x15f);

                SpellHelper.Damage(this, m, damage, 0, 100, 0, 0, 0);
            }
			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly FireballSpell m_Owner;

			public InternalTarget( FireballSpell owner ) : base( 12, false, TargetFlags.Harmful )
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

