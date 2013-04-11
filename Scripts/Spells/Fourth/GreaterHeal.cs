using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.Targeting;

namespace Server.Spells.Fourth
{
    public class GreaterHealSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override int Sound { get { return 0x202; } }

        private static readonly SpellInfo m_Info = new SpellInfo(
				"Greater Heal", "In Vas Mani",
				263,
				9061,
				Reagent.Garlic,
				Reagent.Ginseng,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

		public GreaterHealSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        public override TimeSpan GetCastDelay()
        {
            CustomRegion cR = Caster.Region as CustomRegion;

            if (cR != null && cR.Controller.FizzlePvP && Caster.AccessLevel == AccessLevel.Player)
                return TimeSpan.FromSeconds(3.5);
            
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
            {
                DoFizzle();
            }
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
			/*else if ( m.Poisoned || Server.Items.MortalStrike.IsWounded( m ) )
			{
				Caster.LocalOverheadMessage( MessageType.Regular, 0x22, (Caster == m) ? 1005000 : 1010398 );
			}*/
            else if (CheckBSequence(m))
            {
                int healamount = 0;
                if (m.Poisoned)
                {
                    healamount = 30;
                    Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "The poison mitigates your healing!");
                }
                else
                    healamount = 38;

                if (Scroll != null)
                {
                    healamount -= 3;
                }

                m.Heal(healamount);

                m.FixedParticles(0x376A, 10, 15, 5030, EffectLayer.Waist);
                m.PlaySound(Sound);
            }	

			FinishSequence();
		}

		public class InternalTarget : Target
		{
			private readonly GreaterHealSpell m_Owner;

			public InternalTarget( GreaterHealSpell owner ) : base( 12, false, TargetFlags.Beneficial )
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