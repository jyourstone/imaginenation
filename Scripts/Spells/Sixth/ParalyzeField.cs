using System;
using System.Collections.Generic;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Sixth
{
    public class ParalyzeFieldSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Sixth; } }
        public override int Sound { get { return 529; } }
        
		public override bool CanTargetGround { get { return true; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
						"Paralyze Field", "In Ex Grav",
						263,
						9012,
						Reagent.BlackPearl,
						Reagent.Ginseng,
						Reagent.SpidersSilk
				);

		public ParalyzeFieldSpell(Mobile caster, Item scroll)
			: base(caster, scroll, m_Info)
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
            Caster.Target = new InternalTarget(this);
		}

		public void Target(IPoint3D p)
		{
			if ( !Caster.CanSee( p ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if (SphereSpellTarget is BaseWand)
            {
                BaseWand bw = SphereSpellTarget as BaseWand;
                bw.RechargeWand(Caster, this);
            }
			else if (CheckSequence())
			{
				SpellHelper.GetSurfaceTop(ref p);

				int dx = Caster.Location.X - p.X;
				int dy = Caster.Location.Y - p.Y;
				int rx = (dx - dy) * 44;
				int ry = (dx + dy) * 44;

				bool eastToWest;

				if (rx >= 0 && ry >= 0)
					eastToWest = false;
				else if (rx >= 0)
					eastToWest = true;
				else if (ry >= 0)
					eastToWest = true;
				else
					eastToWest = false;

				Effects.PlaySound(p, Caster.Map, Sound);

				int itemID = eastToWest ? 0x3967 : 0x3979;

				TimeSpan duration = TimeSpan.FromSeconds(3.0 + (Caster.Skills[SkillName.Magery].Value / 3.0));

				List<Item> itemList = new List<Item>();
				for (int i = -3; i <= 3; ++i)
				{
					Point3D loc = new Point3D(eastToWest ? p.X + i : p.X, eastToWest ? p.Y : p.Y + i, p.Z);
                    //bool canFit = SpellHelper.AdjustField(ref loc, Caster.Map, 12, false);

                    //if (!canFit)
                    //    continue;

                    IPooledEnumerable eable = Caster.Map.GetMobilesInRange(loc, 0);

                    foreach (Mobile m in eable)
                    {
                        if (m.AccessLevel != AccessLevel.Player || !m.Alive || (m is PlayerMobile && ((PlayerMobile)m).Young))
                            continue;

                        //Taran: The whole field counts as a harmful action, not just the target
                        if (m.Location.Z - loc.Z < 18 && m.Location.Z - loc.Z > -10)
                            Caster.DoHarmful(m);

                        if (m is BaseCreature)
                            ((BaseCreature)m).OnHarmfulSpell(Caster);
                    }

					Item item = new InternalItem(Caster, itemID, loc, Caster.Map, duration);
					itemList.Add(item);
					item.ProcessDelta();
				}

				if (itemList.Count > 0)
					new FreezeTimer(itemList, Sound, duration.Seconds).Start();

                if (SphereSpellTarget is Mobile)
                {
                    InternalItem castItem = new InternalItem(Caster, itemID, (SphereSpellTarget as Mobile).Location, Caster.Map, duration);
                    castItem.OnMoveOver(SphereSpellTarget as Mobile);
                    //Caster.DoHarmful(SphereSpellTarget as Mobile); - This check is now made for each field tile
                    castItem.Delete();
                    if (SphereSpellTarget is BaseCreature && ((BaseCreature)SphereSpellTarget).ParalyzeImmune)
                        ((Mobile)SphereSpellTarget).PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "The paralyze spell seems to have no effect");
                }

				//Mobile mob = SphereSpellTarget as Mobile;

				/*if (mob != null)
				{
					InternalItem castItem = new InternalItem(Caster, itemID, mob.Location, Caster.Map, duration);
					castItem.OnMoveOver(mob);
					Caster.DoHarmful(mob);


					castItem.Delete();
				}*/
			}

			FinishSequence();
		}

		private class FreezeTimer : Timer
		{
			private readonly List<Item> m_ItemList;
			private readonly int m_SoundId;
		    private readonly int m_Duration;

            public FreezeTimer(List<Item> itemList, int soundId, int duration) : base(TimeSpan.FromSeconds(2.0))
			{
				Priority = TimerPriority.OneSecond;
                m_ItemList = itemList;
				m_SoundId = soundId;
                m_Duration = duration;
			}

			protected override void OnTick()
			{
				if (m_ItemList.Count > 0)
				{
					Item playFrom;

					do
					{
						if (m_ItemList.Count > 1)
							playFrom = m_ItemList[Utility.Random(m_ItemList.Count)];
						else
							playFrom = m_ItemList[0];

						if (playFrom.Deleted)
							m_ItemList.Remove(playFrom);
					}
					while (m_ItemList.Count > 0 && playFrom.Deleted);

                    if (!playFrom.Deleted)
                    {
                        try
                        {
                            bool playSound = false;
                            bool fireField = false;
                            foreach (Item i in m_ItemList)
                            {
                                IPooledEnumerable ipe = i.GetMobilesInRange(0);
                                IPooledEnumerable ip = i.GetItemsInRange(0);

                                foreach (Item item in ip)
                                {
                                    if (item.ItemID == 0x398C || item.ItemID == 0x3996) //Firefield exists, ignore paralyze field
                                    {
                                        fireField = true;
                                        break;
                                    }
                                }

                                if (!fireField)
                                {
                                    foreach (Mobile m in ipe)
                                    {
                                        if (m.AccessLevel != AccessLevel.Player || !m.Alive || (m is PlayerMobile && ((PlayerMobile)m).Young))
                                            continue;

                                        i.OnMoveOver(m);

                                        if (!playSound)
                                            playSound = true;
                                    }
                                }

                                ipe.Free();
                                ip.Free();
                            }

                            if (playSound)
                            {
                                IPooledEnumerable ipe = playFrom.GetMobilesInRange(12);
                                foreach (Mobile m in ipe)
                                    m.PlaySound(m_SoundId);

                                ipe.Free();
                            }

                            new FreezeTimer(m_ItemList, m_SoundId, m_Duration).Start();
                        }
                        catch
                        {
                        }
                    }
				}
			}
		}

		[DispellableField]
		public class InternalItem : Item
		{
			private Timer m_Timer;
			private Mobile m_Caster;
			private DateTime m_End;
		    private int m_Duration;

			public override bool BlocksFit { get { return true; } }

			public InternalItem(Mobile caster, int itemID, Point3D loc, Map map, TimeSpan duration)
				: base(itemID)
			{
				//Visible = false;
				Movable = false;
				Light = LightType.Circle300;

				MoveToWorld(loc, map);

				Visible = true;

				m_Caster = caster;

                m_Duration = Utility.Random(60, 120);

                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(m_Duration));
				m_Timer.Start();

                m_End = DateTime.Now + TimeSpan.FromSeconds(m_Duration);
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

				writer.Write(0); // version

				writer.Write(m_Caster);
				writer.WriteDeltaTime(m_End);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				int version = reader.ReadInt();

				switch (version)
				{
					case 0:
						{
							m_Caster = reader.ReadMobile();
							m_End = reader.ReadDeltaTime();

							m_Timer = new InternalTimer(this, m_End - DateTime.Now);
							m_Timer.Start();

							break;
						}
				}
			}

			public override bool OnMoveOver(Mobile m)
			{
                //if (Visible && m_Caster != null && m_Caster.CanBeHarmful(m, false))
				//if (Visible && m_Caster != null && SpellHelper.ValidIndirectTarget(m_Caster, m) && m_Caster.CanBeHarmful(m, false))
				//{
                if (m.AccessLevel != AccessLevel.Player || !m.Alive || (m is PlayerMobile && ((PlayerMobile)m).Young))
                    return true;

                if (m is BaseCreature && ((BaseCreature)m).ParalyzeImmune)
                    return true;

                IPooledEnumerable ip = GetItemsInRange(0);
                foreach (Item item in ip)
                {
                    if (item.ItemID == 0x398C || item.ItemID == 0x3996) //Firefield exists, ignore paralyze field
                        return true;
                }

                //if (SpellHelper.CanRevealCaster(m))
                //    m_Caster.RevealingAction();

                if (Map != m.Map || !(m.Location.Z >= Location.Z && (m.Location.Z <= (Location.Z + 15))))
                    return true;

                double duration;

				if (Core.AOS)
				{
					duration = (m_Caster.Skills[SkillName.EvalInt].Value - m.Skills[SkillName.MagicResist].Value) * 0.3;

					if (duration < 0.0)
						duration = 0.0;
				}
				else
				{
					duration = 7.0 + (m_Caster.Skills[SkillName.Magery].Value * 0.2);
				}

                m.Paralyze(TimeSpan.FromSeconds(duration));

				m.PlaySound(0x204);
				m.FixedParticles(0x374A, 10, 15, 5028, EffectLayer.Waist);
				//}

				return true;
			}

			private class InternalTimer : Timer
			{
				private readonly Item m_Item;

				public InternalTimer(Item item, TimeSpan duration)
					: base(duration)
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

		private class InternalTarget : Target
		{
			private readonly ParalyzeFieldSpell m_Owner;

			public InternalTarget(ParalyzeFieldSpell owner)
				: base(12, true, TargetFlags.None)
			{
				m_Owner = owner;
			}

			protected override void OnTarget(Mobile from, object o)
			{
				if (o is IPoint3D)
					m_Owner.Target((IPoint3D)o);
			}

			protected override void OnTargetFinish(Mobile from)
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
