using System;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Regions;

namespace Server.Spells.Seventh
{
    public class GateTravelSpell : MagerySpell
    {
        public override bool SpellFizzlesOnHurt { get { return true; } }
        public override SpellCircle Circle { get { return SpellCircle.Seventh; } }
        public override int Sound { get { return 0x20E; } }

        private static readonly SpellInfo m_Info = new SpellInfo(
                "Gate Travel", "Vas Rel Por",
                263,
                9032,
                Reagent.BlackPearl,
                Reagent.MandrakeRoot,
                Reagent.SulfurousAsh
            );

        private readonly RunebookEntry m_Entry;
        private readonly Runebook m_Book;
        private readonly Item m_Scroll;

        public GateTravelSpell(Mobile caster, Item scroll)
            : this(caster, scroll, null, null)
        {
        }

        public GateTravelSpell(Mobile caster, Item scroll, RunebookEntry entry, Runebook book)
            : base(caster, scroll, m_Info)
        {
            m_Entry = entry;
            m_Book = book;
            m_Scroll = scroll;
        }

        public override void OnCast()  //xuo chro : probably foox up the runebook
        {
            Mobile from = Caster;

            if (Caster.Region is HouseRegion)
            {
                Caster.SendMessage("You cannot cast that spell here.");
                return;
            }

            if (SphereSpellTarget is RecallRune && Caster.InLOS(SphereSpellTarget))
            {
                RecallRune rune = (RecallRune)SphereSpellTarget;

                if (rune.Marked)
                {
                    if (rune.ChargesLeft == 0)
                    {
                        Caster.LocalOverheadMessage(MessageType.Regular, 906, true, "The recall rune's magic has faded");
                        DoFizzle();
                        return;
                    }
                    else if (rune.ChargesLeft <= 10 && rune.ChargesLeft >= 1)
                        Caster.LocalOverheadMessage(MessageType.Regular, 906, true, "The recall rune is starting to fade");

                    Effect(rune.Target, rune.TargetMap, true);
                }
                else
                {
                    Caster.SendLocalizedMessage(501805); // That rune is not yet marked.

                    if (from is PlayerMobile)
                        ((PlayerMobile) from).SpellCheck();
                }
            }
            else if (SphereSpellTarget is Runebook)
            {
                RunebookEntry e = ((Runebook)SphereSpellTarget).Default;

                if (e != null)
                    Effect(e.Location, e.Map, true);
                else
                    Caster.SendLocalizedMessage(502354); // Target is not marked.
            }
            else if (SphereSpellTarget is Key && ((Key)SphereSpellTarget).KeyValue != 0 && ((Key)SphereSpellTarget).Link is BaseBoat)
            {
                BaseBoat boat = (BaseBoat)((Key)SphereSpellTarget).Link;

                if (!boat.Deleted && boat.CheckKey(((Key)SphereSpellTarget).KeyValue))
                    Effect(boat.GetMarkedLocation(), boat.Map, false);
                else
                    Caster.LocalOverheadMessage(MessageType.Regular, 906, 502357); // I can not recall from that object.
            }
            else if (m_Entry != null)
            {
                /*if (m_Entry.ChargesLeft == 0)
                {
                    from.LocalOverheadMessage(MessageType.Regular, 906, true, "The recall rune in your runebook has faded");
                    DoFizzle();
                    return;
                }
                else if (m_Entry.ChargesLeft <= 10 && m_Entry.ChargesLeft >= 1)
                    from.LocalOverheadMessage(MessageType.Regular, 906, true, "The recall rune in your runebook is starting to fade");
                */
                Effect(m_Entry.Location, m_Entry.Map, true);
            }
            else if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }/*
            else if (SphereSpellTarget is HouseRaffleDeed && ((HouseRaffleDeed)SphereSpellTarget).ValidLocation())
            {
                HouseRaffleDeed deed = (HouseRaffleDeed)SphereSpellTarget;

                m_Owner.Effect(deed.PlotLocation, deed.PlotFacet, true);
            }*/
            else
            {
                if (!Caster.InLOS(SphereSpellTarget))
                    Caster.LocalOverheadMessage(MessageType.Regular, 0x3B2, 501031); // I cannot see that object.
                else
                    Caster.LocalOverheadMessage(MessageType.Regular, 906, 502357); // I can not recall from that object.
                  
                if (from is PlayerMobile)
                        ((PlayerMobile) from).SpellCheck();
            }
        }

        public override bool Cast()
        {
            if (m_Entry != null)
                return DirectCast(); // Gate from runebook
            else if (PlayerCaster != null) //Player cast
                return RequestTargetBeforCasting();
            else
                return DirectCast(); //Mobile cast
        }

        public void Effect(Point3D loc, Map map, bool checkMulti)
        {
            if (!SpellHelper.CheckTravel(Caster, TravelCheckType.GateFrom))
            {
            }
            else if (!SpellHelper.CheckTravel(Caster, map, loc, TravelCheckType.GateTo))
            {
            }
            else if ((checkMulti && SpellHelper.CheckMulti(loc, map)))
            {
                Caster.SendLocalizedMessage(501942); // That location is blocked.
            }
            else if (m_Book != null && m_Book.CurCharges <= 0 && m_Scroll != null)
            {
                Caster.SendLocalizedMessage(502412); // There are no charges left on that item.
            }
            else if (CheckSequence())
            {
                /*if (m_Entry != null && m_Book != null && m_Scroll == null)
                    --m_Entry.ChargesLeft;
                else */
                if (SphereSpellTarget is RecallRune)
                {
                    RecallRune rune = (RecallRune)SphereSpellTarget;

                    if (rune.ChargesLeft > 0)
                        --rune.ChargesLeft;
                    else if (rune.ChargesLeft == 0)
                    {
                        FinishSequence();
                        return;
                    }
                }
                else if (SphereSpellTarget is Runebook)
                {
                    RunebookEntry e = ((Runebook)SphereSpellTarget).Default;

                    /*if (e.ChargesLeft > 0)
                        --e.ChargesLeft;
                    else*/
                    if (e.ChargesLeft == 0)
                    {
                        FinishSequence();
                        return;
                    }
                }

                CustomRegion cR;
                ISpell markSpell = new Sixth.MarkSpell(Caster, null); 
                
                if ((cR = Caster.Region as CustomRegion) != null && cR.Controller.IsRestrictedSpell(this))
                    Caster.SendAsciiMessage("You can't gate here.");
                else if (Region.Find(loc, map) is HouseRegion || ((cR = Region.Find(loc, map) as CustomRegion) != null && (cR.Controller.IsRestrictedSpell(this) || cR.Controller.IsRestrictedSpell(markSpell))))
                    Caster.SendAsciiMessage("You can't gate to that spot.");
                else
                {
                    InternalItem firstGate = new InternalItem(loc, map);
                    InternalItem secondGate = new InternalItem(Caster.Location, Caster.Map);

                    firstGate.MoveToWorld(Caster.Location, Caster.Map);
                    Effects.PlaySound(Caster.Location, Caster.Map, Sound);
                    secondGate.MoveToWorld(loc, map);
                    Effects.PlaySound(loc, map, Sound);
                }
            }

            FinishSequence();
        }

        [DispellableField]
        private class InternalItem : Moongate
        {
            public override bool ShowFeluccaWarning { get { return Core.AOS; } }

            public InternalItem(Point3D target, Map map)
                : base(target, map)
            {
                Region from = (Region.Find(target, map));
                GuardedRegion reg = (GuardedRegion)from.GetRegion(typeof(GuardedRegion));

                if (reg == null || reg.Disabled) // Gate leading to place where guards are disabled, gate turns red
                    Hue = 0x22;

                else // Gate leading to guarded region, gate turns blue
                    Hue = 0;

                Map = map;

                Dispellable = true;

                new InternalTimer(this).Start();
            }

            public InternalItem(Serial serial)
                : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);

                Delete();
            }

            private class InternalTimer : Timer
            {
                private readonly Item m_Item;

                public InternalTimer(Item item)
                    : base(TimeSpan.FromSeconds(60.0))
                {
                    Priority = TimerPriority.OneSecond;
                    m_Item = item;
                }

                protected override void OnTick()
                {
                    m_Item.Delete();
                }
            }
        }
    }
}
