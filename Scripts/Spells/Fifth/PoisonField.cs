using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Fifth
{
    public class PoisonFieldSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Fifth; } }
        public override int Sound { get { return 550; } }

        public override bool CanTargetGround { get { return true; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Poison Field", "In Nox Grav",
				263,
                9052,
				Reagent.BlackPearl,
				Reagent.Nightshade,
				Reagent.SpidersSilk
			);

		public PoisonFieldSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        public override bool Cast()
        {
            if (Caster is PlayerMobile && ((PlayerMobile)Caster).Young)
            {
                Caster.SendAsciiMessage("You cannot cast this as a young player");
                return false;
            }

            return base.Cast();
        }

        public override void OnPlayerCast()
        {
            Target((IPoint3D)SphereSpellTarget);
        }

		public override void OnCast()
		{
            Caster.Target = new InternalTarget( this );
		}

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendAsciiMessage("Target is not in line of sight.");
                DoFizzle();
            }
            else if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }
            else if (/*SpellHelper.CheckTown(p, Caster) && */CheckSequence())
            {
                SpellHelper.GetSurfaceTop(ref p);

                int dx = Caster.Location.X - p.X;
                int dy = Caster.Location.Y - p.Y;
                int rx = (dx - dy)*44;
                int ry = (dx + dy)*44;

                bool eastToWest;

                if (rx >= 0 && ry >= 0)
                {
                    eastToWest = false;
                }
                else if (rx >= 0)
                {
                    eastToWest = true;
                }
                else if (ry >= 0)
                {
                    eastToWest = true;
                }
                else
                {
                    eastToWest = false;
                }

                Effects.PlaySound(p, Caster.Map, Sound);

                int itemID = eastToWest ? 0x3915 : 0x3922;

                TimeSpan duration = TimeSpan.FromSeconds(3 + (Caster.Skills.Magery.Fixed/25));


                List<InternalItem> itemList = new List<InternalItem>();

                for (int i = -3; i <= 3; ++i)
                {
                    Point3D loc = new Point3D(eastToWest ? p.X + i : p.X, eastToWest ? p.Y : p.Y + i, p.Z);

                    IPooledEnumerable eable = Caster.Map.GetMobilesInRange(loc, 0);

                    foreach (Mobile m in eable)
                    {
                        if (m.AccessLevel != AccessLevel.Player || !m.Alive || (m is PlayerMobile && ((PlayerMobile)m).Young))
                            continue;

                        //Taran: The whole field counts as a harmful action, not just the target
                        if (m.Location.Z - loc.Z < 18 && m.Location.Z - loc.Z > -10)
                            Caster.DoHarmful(m);
                    }

                    InternalItem item = new InternalItem(itemID, loc, Caster, Caster.Map, duration, i);
                    itemList.Add(item);
                }

                if (SphereSpellTarget is Mobile)
                {
                    InternalItem castItem = new InternalItem(itemID, (SphereSpellTarget as Mobile).Location, Caster, Caster.Map, duration, 3);
                    castItem.OnMoveOver(SphereSpellTarget as Mobile);
                    //Caster.DoHarmful(SphereSpellTarget as Mobile); - This check is now made for each field tile

                    castItem.Delete();
                }

                if (itemList.Count > 0)
                    new SoundTimer(itemList, 517).Start();

                FinishSequence();
            }
        }

        private class SoundTimer : Timer
        {
            private readonly List<InternalItem> m_ItemList;
            private readonly int m_SoundId;

            public SoundTimer(List<InternalItem> itemList, int soundId):base(TimeSpan.FromSeconds(2.0))
            {
                Priority = TimerPriority.OneSecond;
                m_SoundId = soundId;

                m_ItemList = itemList;
            }

            protected override void OnTick()
            {
                if (m_ItemList.Count > 0)
                {
                    InternalItem playFrom;

                    do
                    {
                        if (m_ItemList.Count > 1)
                            playFrom = m_ItemList[Utility.Random(m_ItemList.Count)];
                        else
                            playFrom = m_ItemList[0];

                        if (playFrom.Deleted)
                            m_ItemList.Remove(playFrom);

                    } while (m_ItemList.Count > 0 && playFrom.Deleted);

                    if (!playFrom.Deleted)
                    {
                        bool playSound = false;
                        foreach (InternalItem i in m_ItemList)
                        {
                            IPooledEnumerable ipe = i.GetMobilesInRange(0);
                            foreach (Mobile m in ipe)
                            {
                                if (m.AccessLevel != AccessLevel.Player || !m.Alive || (m is PlayerMobile && ((PlayerMobile)m).Young))
                                    continue;

                                i.OnMoveOver(m);
                                if (!playSound)
                                    playSound = true;
                            }

                            ipe.Free();
                        }

                        if (playSound)
                        {
                            IPooledEnumerable ipe = playFrom.GetMobilesInRange(12);
                            foreach (Mobile m in ipe)
                                m.PlaySound(m_SoundId);

                            ipe.Free();
                        }

                        new SoundTimer(m_ItemList, m_SoundId).Start();
                    }
                }
            }
        }

        [DispellableField]
        public class InternalItem : Item
        {
            private Timer m_Timer;
            private DateTime m_End;
            private Mobile m_Caster;

            public override bool BlocksFit { get { return true; } }

            public InternalItem(int itemID, Point3D loc, Mobile caster, Map map, TimeSpan duration, int val)
                : base(itemID)
            {
                bool canFit = SpellHelper.AdjustField(ref loc, map, 12, false);

                Visible = true;
                Movable = false;
                Light = LightType.Circle300;

                MoveToWorld(loc, map);

                m_Caster = caster;

                m_End = DateTime.Now + duration;


                int timespan = Utility.Random(23, 70);

                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(timespan), caster.InLOS(this), canFit);
                m_Timer.Start();
            }

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();

                if (m_Timer != null)
                    m_Timer.Stop();
            }

            public InternalItem(Serial serial)
                : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);

                writer.Write(1); // version

                writer.Write(m_Caster);
                writer.WriteDeltaTime(m_End);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);

                int version = reader.ReadInt();

                switch (version)
                {
                    case 1:
                        {
                            m_Caster = reader.ReadMobile();

                            goto case 0;
                        }
                    case 0:
                        {
                            m_End = reader.ReadDeltaTime();

                            m_Timer = new InternalTimer(this, TimeSpan.Zero, true, true);
                            m_Timer.Start();

                            break;
                        }
                }
            }

            public void ApplyPoisonTo(Mobile m)
            {
                if (m_Caster == null)
                    return;

                Poison p;

                if (Core.AOS)
                {
                    int total = (m_Caster.Skills.Magery.Fixed + m_Caster.Skills.Poisoning.Fixed) / 2;

                    if (total >= 1000)
                        p = Poison.Deadly;
                    else if (total > 850)
                        p = Poison.Greater;
                    else if (total > 650)
                        p = Poison.Regular;
                    else
                        p = Poison.Lesser;
                }
                else
                {
                    p = Poison.Regular;
                }

                m.ApplyPoison(m_Caster, p);
                //    if (SpellHelper.CanRevealCaster(m))
                //        m_Caster.RevealingAction();

                if (m is BaseCreature)
                    ((BaseCreature)m).OnHarmfulSpell(m_Caster);
            }

            public override bool OnMoveOver(Mobile m)
            {
                if (Visible)// && m_Caster != null && m_Caster.CanBeHarmful(m, false))
                {
                    if (Map != m.Map || !(m.Location.Z >= Location.Z && (m.Location.Z <= (Location.Z + 15))))
                        return true;

                    if (m.AccessLevel != AccessLevel.Player || !m.Alive || (m is PlayerMobile && ((PlayerMobile)m).Young))
                        return true;

                    ApplyPoisonTo(m);
                    m.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                }

                return true;
            }



            private class InternalTimer : Timer
            {
                private readonly InternalItem m_Item;
                private bool m_InLOS, m_CanFit;

                private static readonly Queue m_Queue = new Queue();

                public InternalTimer(InternalItem item, TimeSpan delay, bool inLOS, bool canFit)
                    : base(delay, TimeSpan.FromSeconds(1.5))
                {
                    m_Item = item;
                    m_InLOS = inLOS;
                    m_CanFit = canFit;

                    Priority = TimerPriority.FiftyMS;
                }

                protected override void OnTick()
                {
                    if (m_Item.Deleted)
                        return;

                    if (!m_Item.Visible)
                    {
                        //if (m_InLOS && m_CanFit)
                        //    m_Item.Visible = true;
                        //else
                        //    m_Item.Delete();

                        if (!m_Item.Deleted)
                        {
                            m_Item.ProcessDelta();
                            Effects.SendLocationParticles(EffectItem.Create(m_Item.Location, m_Item.Map, EffectItem.DefaultDuration), 0x376A, 9, 10, 5040);
                        }
                    }
                    else if (DateTime.Now > m_Item.m_End)
                    {
                        m_Item.Delete();
                        Stop();
                    }
                    else
                    {
                        Map map = m_Item.Map;
                        Mobile caster = m_Item.m_Caster;

                        if (map != null && caster != null)
                        {
                            bool eastToWest = (m_Item.ItemID == 0x3915);
                            IPooledEnumerable eable = map.GetMobilesInBounds(new Rectangle2D(m_Item.X - (eastToWest ? 0 : 1), m_Item.Y - (eastToWest ? 1 : 0), (eastToWest ? 1 : 2), (eastToWest ? 2 : 1))); ;

                            foreach (Mobile m in eable)
                            {
                                if (m.AccessLevel != AccessLevel.Player || !m.Alive)
                                    continue;

                                if ((m.Z + 16) > m_Item.Z && (m_Item.Z + 12) > m.Z )//&& SpellHelper.ValidIndirectTarget(caster, m) && caster.CanBeHarmful(m, false))
                                    m_Queue.Enqueue(m);
                            }

                            eable.Free();

                            while (m_Queue.Count > 0)
                            {
                                Mobile m = (Mobile) m_Queue.Dequeue();

                                m_Item.ApplyPoisonTo(m);
                                m.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                            }
                        }
                    }
                }
            }

        }

		private class InternalTarget : Target
		{
			private readonly PoisonFieldSpell m_Owner;

			public InternalTarget( PoisonFieldSpell owner ) : base( 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is IPoint3D )
					m_Owner.Target( (IPoint3D)o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
