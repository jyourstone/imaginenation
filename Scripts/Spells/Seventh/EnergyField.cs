using System;
using System.Collections.Generic;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Seventh
{
    public class EnergyFieldSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Seventh; } }
        public override int Sound { get { return 0x20B; } }
        
        public override bool CanTargetGround { get { return true; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Energy Field", "In Sanct Grav",
				263,
                9022,
				Reagent.BlackPearl,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk,
				Reagent.SulfurousAsh
			);

		public EnergyFieldSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			if(Caster is PlayerMobile)
				Target( (IPoint3D)SphereSpellTarget );
			else
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
			else if ( CheckSequence() )
			{

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

				Effects.PlaySound( p, Caster.Map, Sound );

				TimeSpan duration;

				if ( Core.AOS )
					duration = TimeSpan.FromSeconds( (15 + (Caster.Skills.Magery.Fixed / 5)) / 7 );
				else
					duration = TimeSpan.FromSeconds( Caster.Skills[SkillName.Magery].Value * 0.28 + 2.0 ); // (28% of magery) + 2.0 seconds

				int itemID = eastToWest ? 0x3946 : 0x3956;

                List<Item> itemList = new List<Item>();
				for ( int i = -3; i <= 3; ++i )
				{
                    Point3D loc = new Point3D(eastToWest ? p.X + i : p.X, eastToWest ? p.Y : p.Y + i, p.Z);
                    bool canFit = SpellHelper.AdjustField(ref loc, Caster.Map, 12, false);

                    if (!canFit)
                        continue;

					Item item = new InternalItem( loc, Caster.Map, duration, itemID, Caster );
				    itemList.Add(item);
					item.ProcessDelta();

					Effects.SendLocationParticles( EffectItem.Create( loc, Caster.Map, EffectItem.DefaultDuration ), 0x376A, 9, 10, 5051 );
				}

                if (itemList.Count > 0)
                    new SoundTimer(itemList, Sound).Start();
			}

			FinishSequence();
		}

        private class SoundTimer : Timer
        {
            private readonly List<Item> m_ItemList;
            private readonly int m_SoundId;

            public SoundTimer(List<Item> itemList, int soundId)
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
                    Item playFrom;

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
                            foreach (Item i in m_ItemList)
                            {
                                IPooledEnumerable ipe = i.GetMobilesInRange(0);
                                foreach (Mobile m in ipe)
                                {
                                    if (m.AccessLevel != AccessLevel.Player || !m.Alive)
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
		private class InternalItem : Item
		{
			private readonly Timer m_Timer;

			public override bool BlocksFit{ get{ return true; } }

			public InternalItem( Point3D loc, Map map, TimeSpan duration, int itemID, Mobile caster ) : base( itemID )
			{
                Visible = true;
				//Visible = false;
				Movable = false;
				Light = LightType.Circle300;

				MoveToWorld( loc, map );

				/*if ( caster.InLOS( this ) )
					Visible = true;
				else
					Delete();

				if ( Deleted )
					return;*/
                int timespan = Utility.Random(60, 120);
                m_Timer = new InternalTimer( this, TimeSpan.FromSeconds( timespan) );

				m_Timer.Start();
			}

			public InternalItem( Serial serial ) : base( serial )
			{
				m_Timer = new InternalTimer( this, TimeSpan.FromSeconds( 5.0 ) );
				m_Timer.Start();
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( 0 ); // version
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if ( m_Timer != null )
					m_Timer.Stop();
			}

			private class InternalTimer : Timer
			{
				private readonly InternalItem m_Item;

				public InternalTimer( InternalItem item, TimeSpan duration ) : base( duration )
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
			private readonly EnergyFieldSpell m_Owner;

			public InternalTarget( EnergyFieldSpell owner ) : base( 12, true, TargetFlags.None )
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