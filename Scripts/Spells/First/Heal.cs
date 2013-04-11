using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.Targeting;

namespace Server.Spells.First
{
    public class HealSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override int Sound { get { return 0x1F2; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Heal", "In Mani",
				263,
				9061,
				Reagent.Garlic,
				Reagent.Ginseng,
				Reagent.SpidersSilk
			);

		public HealSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        public override TimeSpan GetCastDelay()
        {
            CustomRegion cR = Caster.Region as CustomRegion;

            if (cR != null && cR.Controller.FizzlePvP && Caster.AccessLevel == AccessLevel.Player)
                return TimeSpan.FromSeconds(1.5);

            return base.GetCastDelay();
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
			else if ( m is BaseCreature && ((BaseCreature)m).IsAnimatedDead )
			{
				Caster.SendLocalizedMessage( 1061654 ); // You cannot heal that which is not alive.
			}
			else if ( m is Golem )
			{
				Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 500951 ); // You cannot heal that.
			}
			else if ( CheckBSequence( m ) )
			{

				int toHeal;

                CustomRegion cR = Caster.Region as CustomRegion;

				if ( Core.AOS )
				{
					// TODO: / 100 or / 120 ? 1, 3 or 1, 4 ?
					toHeal = Caster.Skills.Magery.Fixed / 100;
					toHeal += Utility.RandomMinMax( 1, 3 );
				}
                else //Loki edit: New PvP changes
                {
                    toHeal = 3 + (int)(Caster.Skills[SkillName.Magery].Value * 0.1);
                    if (m.Poisoned)
                    {
                        toHeal -= 3;
                        if (toHeal < 1)
                            toHeal = 1;
                        Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "The poison mitigates your healing!");
                    }
                } 

                m.Heal(toHeal);

				m.FixedParticles( 0x376A, 5, 15, 5005, EffectLayer.Waist );
				m.PlaySound( Sound );
			}

			FinishSequence();
		}

		public class InternalTarget : Target
		{
			private readonly HealSpell m_Owner;

			public InternalTarget( HealSpell owner ) : base( 12, false, TargetFlags.Beneficial )
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