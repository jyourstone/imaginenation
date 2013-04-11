using System;
using Server.Items;
using Server.Mobiles;
using Server.Regions;
using Server.Targeting;

namespace Server.Spells.Seventh
{
    public class FlameStrikeSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Seventh; } }
        public override int Sound { get { return 0x208; } }
        
		private static readonly SpellInfo m_Info = new SpellInfo(
				"Flame Strike", "Kal Vas Flam",
				263,
				9042,
				Reagent.SpidersSilk,
				Reagent.SulfurousAsh
			);

		public FlameStrikeSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        public override TimeSpan GetCastDelay()
        {
            CustomRegion cR = Caster.Region as CustomRegion;

            if (cR != null && cR.Controller.FizzlePvP && Caster.AccessLevel == AccessLevel.Player)
                return TimeSpan.FromSeconds(3.5);
            
            return base.GetCastDelay();
        }

		public override void OnCast()
		{
			if(Caster is PlayerMobile)
            {
				if ( SphereSpellTarget is Mobile && Caster.InLOS( SphereSpellTarget ))
					Target( (Mobile)SphereSpellTarget );
                else if (SphereSpellTarget is BaseExplosionPotion && Caster.InLOS(SphereSpellTarget))
                    iTarget((BaseExplosionPotion)SphereSpellTarget);
                else if (SphereSpellTarget is BaseWand)
                {
                    BaseWand bw = SphereSpellTarget as BaseWand;
                    bw.RechargeWand(Caster, this);
                }
                else
                    DoFizzle();
			}
			else
				Caster.Target = new InternalTarget( this );
		}

		public override bool DelayedDamage{ get{ return false; } }//XUO insta damage

        public void iTarget(BaseExplosionPotion pot) //Taran: When casted on explosion pots they explode
        {
            if (!Caster.CanSee(pot))
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            else
            {
                Mobile source = Caster;

                pot.Explode(source, true, pot.GetWorldLocation(), pot.Map);

                //pot.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                source.PlaySound(Sound);
            }
            CheckSequence();
            FinishSequence();
        }

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}

            CustomRegion cR = Caster.Region as CustomRegion;

            if (cR != null && cR.Controller.FizzlePvP && CheckHSequence(m))
            {
                SpellHelper.CheckReflect((int)Circle, Caster, ref m);

                double damage = Utility.RandomMinMax(15, 20) + ((int)((GetDamageSkill(Caster) * 3) - GetResistSkill(m)) / 8);

                m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                m.PlaySound(Sound);

                SpellHelper.Damage(this, m, damage, 0, 100, 0, 0, 0);
            }
            else if (CheckHSequence(m)) //Loki edit: New PvP changes
            {
                SpellHelper.CheckReflect((int)Circle, Caster, ref m);

                double damage = 17 + ((int)((GetDamageSkill(Caster) * 3) - GetResistSkill(m)) / 8);

                if (Scroll == null)
                    damage += 5;

                if (m is PlayerMobile)
                {
                    double aR = ((PlayerMobile)m).BaseArmorRatingSpells;

                    if (aR < 55)
                        damage += 1;
                    if (aR < 45)
                        damage += 1;
                    if (aR < 35)
                        damage += 1;
                    if (aR < 25)
                        damage += 1;
                }

                m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                m.PlaySound(Sound);

                SpellHelper.Damage(this, m, damage, 0, 100, 0, 0, 0);
            }
            FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly FlameStrikeSpell m_Owner;

			public InternalTarget( FlameStrikeSpell owner ) : base( 12, false, TargetFlags.Harmful )
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
