using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Fourth
{
    public class FireFieldSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override int Sound { get { return 477; } }

        public override bool CanTargetGround { get { return true; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Fire Field", "In Flam Grav",
				263,
                9041,
				Reagent.BlackPearl,
				Reagent.SpidersSilk,
				Reagent.SulfurousAsh
			);

		public FireFieldSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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

		public void Target( IPoint3D p )
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
            else if (/*SpellHelper.CheckTown(p, Caster) && */CheckSequence())
            {


                //if (CheckSequence())

				SpellHelper.GetSurfaceTop( ref p );

				int dx = Caster.Location.X - p.X;
				int dy = Caster.Location.Y - p.Y;
				int rx = (dx - dy) * 44;
				int ry = (dx + dy) * 44;

				bool eastToWest;

				if ( rx >= 0 && ry >= 0 )
				{
					eastToWest = false;
				}
				else if ( rx >= 0 )
				{
					eastToWest = true;
				}
				else if ( ry >= 0 )
				{
					eastToWest = true;
				}
				else
				{
					eastToWest = false;
				}

                Effects.PlaySound(p, Caster.Map, Sound);

				int itemID = eastToWest ? 0x398C : 0x3996;

				TimeSpan duration;

				if ( Core.AOS )
					duration = TimeSpan.FromSeconds( (15 + (Caster.Skills.Magery.Fixed / 5)) / 4 );
				else
					duration = TimeSpan.FromSeconds( 4.0 + (Caster.Skills[SkillName.Magery].Value * 0.5) );

                List<InternalItem> itemList = new List<InternalItem>();
				for ( int i = -3; i <= 3; ++i )
				{
					Point3D loc = new Point3D( eastToWest ? p.X + i : p.X, eastToWest ? p.Y : p.Y + i, p.Z );
                   
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

					InternalItem toAdd = new InternalItem( itemID, loc, Caster, Caster.Map, duration, i );
				    itemList.Add(toAdd);
                  
				}

                if (itemList.Count > 0)
                    new SoundTimer(itemList, Sound).Start();

                if (SphereSpellTarget is Mobile)
                {
                    InternalItem castItem = new InternalItem(itemID, (SphereSpellTarget as Mobile).Location, Caster, Caster.Map, duration, 3);
                    castItem.OnMoveOver(SphereSpellTarget as Mobile);
                    //Caster.DoHarmful(SphereSpellTarget as Mobile); - This check is now made for each field tile
                    castItem.Delete();
                }
			}

			FinishSequence();
		}

        private class SoundTimer : Timer
        {
            private readonly List<InternalItem> m_ItemList;
            private readonly int m_SoundId;

            public SoundTimer(List<InternalItem> itemList, int soundId)
                : base(TimeSpan.FromSeconds(2.0))
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
                        try
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
			private DateTime m_End;
			private Mobile m_Caster;

			public override bool BlocksFit{ get{ return true; } }

			public InternalItem( int itemID, Point3D loc, Mobile caster, Map map, TimeSpan duration, int val ) : base( itemID )
			{
				bool canFit = SpellHelper.AdjustField( ref loc, map, 12, false );

				Visible = true;
				Movable = false;
				Light = LightType.Circle300;

				MoveToWorld( loc, map );

				m_Caster = caster;

				m_End = DateTime.Now + duration;

                int timespan = Utility.Random(60, 120);

				m_Timer = new InternalTimer( this, TimeSpan.FromSeconds( timespan ), caster.InLOS( this ), canFit );
				m_Timer.Start();
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if ( m_Timer != null )
					m_Timer.Stop();
			}

			public InternalItem( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( 1 ); // version

				writer.Write( m_Caster );
				writer.WriteDeltaTime( m_End );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();

				switch ( version )
				{
					case 1:
					{
						m_Caster = reader.ReadMobile();

						goto case 0;
					}
					case 0:
					{
						m_End = reader.ReadDeltaTime();

						m_Timer = new InternalTimer( this, TimeSpan.Zero, true, true );
						m_Timer.Start();

						break;
					}
				}
			}

			public override bool OnMoveOver( Mobile m )
			{
                //if (Visible && m_Caster != null && m_Caster.CanBeHarmful(m, false))
				//{
                if (m.AccessLevel != AccessLevel.Player || !m.Alive || (m is PlayerMobile && ((PlayerMobile)m).Young))
                    return true;

                //if (SpellHelper.CanRevealCaster(m))
                //    m_Caster.RevealingAction();
					
                if (Map != m.Map || !(m.Location.Z >= Location.Z && (m.Location.Z <= (Location.Z + 15))))
                    return true;
                    
                int damage = 2;

                if (m.CheckSkill(SkillName.MagicResist, 0.0, 30.0))
                {
                    damage = 1;
                    m.SendMessage("You feel yourself resisting magic"); // You feel yourself resisting magical energy.
                }

                AOS.Damage(m, m_Caster, damage, 0, 100, 0, 0, 0);
				//}

				return true;
			}

			private class InternalTimer : Timer
			{
				private readonly InternalItem m_Item;
				private bool m_InLOS, m_CanFit;

				private static readonly Queue m_Queue = new Queue();

				public InternalTimer( InternalItem item, TimeSpan delay, bool inLOS, bool canFit ) : base( delay, TimeSpan.FromSeconds( 1.0 ) )
				{
					m_Item = item;
					m_InLOS = inLOS;
					m_CanFit = canFit;

					Priority = TimerPriority.FiftyMS;
				}

				protected override void OnTick()
				{
					if ( m_Item.Deleted )
						return;

					if ( !m_Item.Visible )
					{
                        //if ( m_InLOS && m_CanFit )
                        //    m_Item.Visible = true;
                        //else
                        //    m_Item.Delete();

						if ( !m_Item.Deleted )
						{
							m_Item.ProcessDelta();
							Effects.SendLocationParticles( EffectItem.Create( m_Item.Location, m_Item.Map, EffectItem.DefaultDuration ), 0x376A, 9, 10, 5029 );
						}
					}
					else if ( DateTime.Now > m_Item.m_End )
					{
						m_Item.Delete();
						Stop();
					}
					else
					{
						Map map = m_Item.Map;
						Mobile caster = m_Item.m_Caster;

						if ( map != null && caster != null )
						{
							foreach ( Mobile m in m_Item.GetMobilesInRange( 0 ) )
							{
                                if (m.AccessLevel != AccessLevel.Player || !m.Alive)
                                    continue;

								if ( (m.Z + 16) > m_Item.Z && (m_Item.Z + 12) > m.Z )// && SpellHelper.ValidIndirectTarget( caster, m ) && caster.CanBeHarmful( m, false ) )
									m_Queue.Enqueue( m );
							}

							while (m_Queue.Count > 0)
							{
								Mobile m = (Mobile)m_Queue.Dequeue();

                                //if (SpellHelper.CanRevealCaster(m))
                                //    caster.RevealingAction();

								int damage = 2;

								if ( !Core.AOS && m.CheckSkill( SkillName.MagicResist, 0.0, 30.0 ) )
								{
									damage = 1;

									m.SendMessage("You feel yourself resisting magic"); // You feel yourself resisting magical energy.
								}

                                AOS.Damage(m, caster, damage, 0, 100, 0, 0, 0);
								m.PlaySound( 0x208 );
							}
						}
					}
				}
			}
		}

		private class InternalTarget : Target
		{
			private readonly FireFieldSpell m_Owner;

			public InternalTarget( FireFieldSpell owner ) : base( 12, true, TargetFlags.None )
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