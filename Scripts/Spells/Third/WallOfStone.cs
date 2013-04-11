using System;
using Server.Items;
using Server.Misc;
using Server.Targeting;
using Server.Regions;

namespace Server.Spells.Third
{
    public class WallOfStoneSpell : MagerySpell
    {
        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override int Sound { get { return 0x1F6; } }
        
        public override bool CanTargetGround { get { return true; } }

		private static readonly SpellInfo m_Info = new SpellInfo(
				"Wall of Stone", "In Sanct Ylem",
				263,//269
				9011,
				Reagent.Bloodmoss,
				Reagent.Garlic
			);

		public WallOfStoneSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
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

                //Loki edit: Wall length reduced
                CustomRegion cR = Caster.Region as CustomRegion;
                int wallsize;
                if (cR != null && cR.Controller.LokiPvP)
                    wallsize = 2;
                else
                    wallsize = 3;

                for (int i = -wallsize; i <= wallsize; ++i) //End Loki edit: Was: int i = -3; i <= 3; ++i 
				{
                    //Iza -- Always fit
                    Point3D loc = new Point3D(eastToWest ? p.X + i : p.X, eastToWest ? p.Y : p.Y + i, p.Z);
                    /*bool canFit = SpellHelper.AdjustField(ref loc, Caster.Map, 22, true);

                    if (!canFit)
                        continue;*/

                    IPooledEnumerable eable = Caster.Map.GetMobilesInRange(loc, 0);
				    bool canFit = true;

                    foreach (Mobile m in eable)
                    {
                        if (m.AccessLevel != AccessLevel.Player || !m.Alive)
                            continue;
                        
                        if (m.Location.Z - loc.Z < 18 && m.Location.Z - loc.Z > -10)
                        {
                            //Taran: The whole field counts as a harmful action, not just the target
                            Caster.DoHarmful(m);
                            //Taran: Make a hole in the wall if a mobile is there
                            canFit = false;
                            break;
                        }
                    }

				    eable.Free();

                    if (!canFit)
                        continue;

                    //Rob edit: Delete existing wall items
                    eable = Caster.Map.GetItemsInRange(loc, 0);
                    foreach (Item item in eable)
                    {
                        if (item is InternalItem)
                            item.Delete();
                    }
                    eable.Free();

					new InternalItem( loc, Caster.Map, Caster );
				}
			}

			FinishSequence();
		}

		[DispellableField]
		private class InternalItem : Item
		{
			private Timer m_Timer;
			private DateTime m_End;

			//public override bool BlocksFit{ get{ return true; } }

			public InternalItem( Point3D loc, Map map, Mobile caster ) : base( 0x80 )
			{
				//Visible = false;
                Visible = true;
				Movable = false;

				MoveToWorld( loc, map );

				/*if ( caster.InLOS( this ) )
					Visible = true;
				else
					Delete();

				if ( Deleted )
					return;*/

                /* Loki edit: This was probably meant to be Utility.RandomMinMax()
                 * But either way it is way too long
                int timespan = Utility.Random(60, 120); //sphere style.
                 */
                int timespan = Utility.RandomMinMax(50, 70);

				m_Timer = new InternalTimer( this, TimeSpan.FromSeconds( timespan) );
				m_Timer.Start();

				m_End = DateTime.Now + TimeSpan.FromSeconds( timespan );
			}

			public InternalItem( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( 1 ); // version

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
						m_End = reader.ReadDeltaTime();

						m_Timer = new InternalTimer( this, m_End - DateTime.Now );
						m_Timer.Start();

						break;
					}
					case 0:
					{
						TimeSpan duration = TimeSpan.FromSeconds( 10.0 );

						m_Timer = new InternalTimer( this, duration );
						m_Timer.Start();

						m_End = DateTime.Now + duration;

						break;
					}
				}
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
			private readonly WallOfStoneSpell m_Owner;

			public InternalTarget( WallOfStoneSpell owner ) : base( 12, true, TargetFlags.None )
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