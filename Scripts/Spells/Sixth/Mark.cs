using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Sixth
{
    public class MarkSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Sixth; } }
        public override int Sound { get { return 0x1FA; } }
        
		private static readonly SpellInfo m_Info = new SpellInfo(
				"Mark", "Kal Por Ylem",
				263,
				9002,
				Reagent.BlackPearl,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot
			);

		public MarkSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        public override void OnPlayerCast()
        {
            if (SphereSpellTarget is RecallRune)
                Target((RecallRune)SphereSpellTarget);
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

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			return SpellHelper.CheckTravel( Caster, TravelCheckType.Mark );
		}

		public void Target( RecallRune rune )
		{
			if ( !Caster.CanSee( rune ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( !SpellHelper.CheckTravel( Caster, TravelCheckType.Mark ) )
			{
                //Caster.SendAsciiMessage("ros");
			}
			else if ( SpellHelper.CheckMulti( Caster.Location, Caster.Map, !Core.AOS ) )
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
			else if ( !rune.IsChildOf( Caster.Backpack ) )
			{
				Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1062422 ); // You must have this rune in your backpack in order to mark it.
			}
			else if ( CheckSequence() )
			{
                if (Caster.Region is Regions.HouseRegion || (Caster.Region is Regions.CustomRegion && ((Regions.CustomRegion)Caster.Region).Controller.IsRestrictedSpell(this)))
                    Caster.SendAsciiMessage("You can't mark here.");
                else if (Caster.Region is Regions.GreenAcres && Caster.AccessLevel == AccessLevel.Player)
                    Caster.SendAsciiMessage("You cannot mark runes in Green Acres");
                else
                {
                    rune.Mark(Caster);

                    Caster.PlaySound(Sound);
                }
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly MarkSpell m_Owner;

			public InternalTarget( MarkSpell owner ) : base( 12, false, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is RecallRune )
				{
					m_Owner.Target( (RecallRune) o );
				}
				else
				{
                    from.LocalOverheadMessage(MessageType.Regular, 906, 501797); // I cannot mark that object.
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}