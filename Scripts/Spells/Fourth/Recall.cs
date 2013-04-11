using System;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Spells.Necromancy;
using Server.Targeting;

namespace Server.Spells.Fourth
{
    public class RecallSpell : MagerySpell
    {
        public override bool SpellFizzlesOnHurt { get  { return true; } }
        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override int Sound { get { return 0x1FC; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Recall", "Kal Ort Por",
				263,
				9031,
				Reagent.BlackPearl,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot
			);

		private readonly RunebookEntry m_Entry;
		private readonly Runebook m_Book;
        private readonly Item m_Scroll;

		public RecallSpell( Mobile caster, Item scroll ) : this( caster, scroll, null, null )
		{
            m_Scroll = scroll;
		}

		public RecallSpell( Mobile caster, Item scroll, RunebookEntry entry, Runebook book ) : base( caster, scroll, m_Info )
		{
			m_Entry = entry;
			m_Book = book;
            m_Scroll = scroll;
		}

        public override TimeSpan GetCastDelay()
        {
            if (Scroll != null)
                return TimeSpan.FromSeconds(1.85);

            return base.GetCastDelay();
        }

	    public override void GetCastSkills( out double min, out double max )
		{
			if ( Spells.TransformationSpellHelper.UnderTransformation( Caster, typeof( WraithFormSpell ) ) )
				min = max = 0;
			else
				base.GetCastSkills( out min, out max );
		}

        public override void OnCast()
        {
            RecallSpell m_Owner = this;
            Mobile from = Caster;

            if (SphereSpellTarget is RecallRune)
            {
                RecallRune rune = (RecallRune) SphereSpellTarget;

                if (Caster.InLOS(rune) || (from.BankBox != null && rune.IsChildOf(from.BankBox)))
                {
                    if (rune.Marked)
                    {
                        if (rune.ChargesLeft == 0)
                        {
                            from.LocalOverheadMessage(MessageType.Regular, 906, true, "The recall rune's magic has faded");
                            DoFizzle();
                            return;
                        }

                        if (rune.ChargesLeft <= 10 && rune.ChargesLeft >= 1)
                            from.LocalOverheadMessage(MessageType.Regular, 906, true, "The recall rune is starting to fade");

                        m_Owner.Effect(rune.Target, rune.TargetMap, true);
                    }
                    else
                    {
                        from.SendLocalizedMessage(501805); // That rune is not yet marked.

                        if (from is PlayerMobile)
                            ((PlayerMobile)from).SpellCheck();
                    }
                }
                else
                {
                    Caster.LocalOverheadMessage(MessageType.Regular, 0x3B2, 501031); // I cannot see that object.

                    if (from is PlayerMobile)
                        ((PlayerMobile) from).SpellCheck();
                }
            }
            else if (SphereSpellTarget is Runebook)
            {
                RunebookEntry e = ((Runebook)SphereSpellTarget).Default;

                if (e != null)
                    m_Owner.Effect(e.Location, e.Map, true);
                else
                    from.SendLocalizedMessage(502354); // Target is not marked.
            }
            else if (SphereSpellTarget is Key && ((Key)SphereSpellTarget).KeyValue != 0 &&
                     ((Key)SphereSpellTarget).Link is BaseBoat)
            {
                BaseBoat boat = ((Key)SphereSpellTarget).Link as BaseBoat;

                if (!boat.Deleted && boat.CheckKey(((Key)SphereSpellTarget).KeyValue))
                {
                    m_Owner.Effect(boat.GetMarkedLocation(), boat.Map, false);
                    boat.Refresh();
                }
                else
                    from.LocalOverheadMessage(MessageType.Regular, 906, 502357); // I can not recall from that object.
            }
            else if (m_Entry != null)
            {
                /*if (m_Entry.ChargesLeft == 0)
                {
                    from.LocalOverheadMessage(MessageType.Regular, 906, true, "The runebook's recall rune's magic has faded");
                    DoFizzle();
                    return;
                }
                else if (m_Entry.ChargesLeft <= 10 && m_Entry.ChargesLeft >= 1)
                    from.LocalOverheadMessage(MessageType.Regular, 906, true, "The recall rune in your runebook is starting to fade");
                */
                m_Owner.Effect(m_Entry.Location, m_Entry.Map, true);
            }
            else if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }
            else
            {
                from.LocalOverheadMessage(MessageType.Regular, 906, 502357); // I can not recall from that object.

                if (from is PlayerMobile)
                    ((PlayerMobile)from).SpellCheck();
            }
        }

        public override bool Cast()
        {
            if (m_Entry != null)
                return DirectCast(); // Recall from runebook
            else if (PlayerCaster != null) //Player cast
                return RequestTargetBeforCasting();
            else
                return DirectCast(); //Mobile cast
        }

		public void Effect( Point3D loc, Map map, bool checkMulti )
		{
			if ( !SpellHelper.CheckTravel( Caster, TravelCheckType.RecallFrom ) )
			{
			}
			else if ( !SpellHelper.CheckTravel( Caster, map, loc, TravelCheckType.RecallTo ) )
			{
			}
			else if ( (checkMulti && SpellHelper.CheckMulti( loc, map )) )
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
            else if (m_Book != null && m_Book.CurCharges <= 0 && m_Scroll != null)
			{
				Caster.SendLocalizedMessage( 502412 ); // There are no charges left on that item.
			}
			else if ( CheckSequence() )
			{
                Regions.CustomRegion cR = null;
                ISpell markSpell = new Sixth.MarkSpell(Caster, null);

                if ((cR = Caster.Region as Regions.CustomRegion) != null && cR.Controller.IsRestrictedSpell(this))
                    Caster.SendAsciiMessage("You can't recall here.");
                else if (Region.Find(loc, map) is Regions.HouseRegion || ((cR = Region.Find(loc, map) as Regions.CustomRegion) != null && (cR.Controller.IsRestrictedSpell(this) || cR.Controller.IsRestrictedSpell(markSpell))))
                    Caster.SendAsciiMessage("You can't recall to that spot.");
                else if (Region.Find(loc, map) is Regions.GreenAcres && Caster.AccessLevel == AccessLevel.Player)
                    Caster.SendAsciiMessage("You cannot recall to Green Acres");
                else
                {
                    BaseCreature.TeleportPets(Caster, loc, map, true);

                    /*if (m_Entry != null && m_Book != null && m_Scroll == null)
                        --m_Entry.ChargesLeft;
                    else*/
                    if (m_Book != null && m_Scroll != null)
                    {
                        --m_Book.CurCharges;
                        /*--m_Entry.ChargesLeft;*/
                    }

                    if (SphereSpellTarget is RecallRune)
                    {
                        RecallRune rune = (RecallRune) SphereSpellTarget;

                        if (rune.ChargesLeft > 0)
                            --rune.ChargesLeft;
                        else if (rune.ChargesLeft == 0)
                        {
                            FinishSequence();
                            return;
                        }
                    }

                    if (SphereSpellTarget is Runebook)
                    {
                        RunebookEntry e = ((Runebook) SphereSpellTarget).Default;

                        /*if (e.ChargesLeft > 0)
                            --e.ChargesLeft;
                        else*/
                        if (e.ChargesLeft == 0)
                        {
                            FinishSequence();
                            return;
                        }
                    }

                    Caster.MoveToWorld(loc, map);
                    Caster.PlaySound(Sound);
                    Caster.RevealingAction();
                }
			}
			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly RecallSpell m_Owner;

			public InternalTarget( RecallSpell owner ) : base( 12, false, TargetFlags.None )
			{
				m_Owner = owner;

				owner.Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 501029 ); // Select Marked item.
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is RecallRune )
				{
					RecallRune rune = (RecallRune)o;

					if ( rune.Marked )
						m_Owner.Effect( rune.Target, rune.TargetMap, true );
					else
						from.SendLocalizedMessage( 501805 ); // That rune is not yet marked.
				}
				else if ( o is Runebook )
				{
					RunebookEntry e = ((Runebook)o).Default;

					if ( e != null )
						m_Owner.Effect( e.Location, e.Map, true );
					else
						from.SendLocalizedMessage( 502354 ); // Target is not marked.
				}
				else if ( o is Key && ((Key)o).KeyValue != 0 && ((Key)o).Link is BaseBoat )
				{
					BaseBoat boat = ((Key)o).Link as BaseBoat;

					if ( !boat.Deleted && boat.CheckKey( ((Key)o).KeyValue ) )
						m_Owner.Effect( boat.GetMarkedLocation(), boat.Map, false );
					else
                        from.LocalOverheadMessage(MessageType.Regular, 906, 502357); // I can not recall from that object.
				}
                else if (o is HouseRaffleDeed && ((HouseRaffleDeed)o).ValidLocation())
                {
                    HouseRaffleDeed deed = (HouseRaffleDeed)o;

                    m_Owner.Effect(deed.PlotLocation, deed.PlotFacet, true);
                }
				else
                    from.LocalOverheadMessage(MessageType.Regular, 906, 502357); // I can not recall from that object.
			}

            protected override void OnNonlocalTarget(Mobile from, object o)
            {
            }

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}

