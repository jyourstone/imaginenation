using System;
using Server.Commands;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Jailing
{
	public class WraithJailEffect
	{
		private readonly PlayerMobile m_prisoner;
		private readonly PlayerMobile m_jailor;

		public WraithJailEffect( PlayerMobile prisoner, PlayerMobile jailor )
		{
			m_jailor = jailor;
			m_prisoner = prisoner;
			m_prisoner.CantWalk = true;
			m_prisoner.Squelched = true;
			Effects.PlaySound( jailor.Location, jailor.Map, 0x1DD );

			Point3D loc = new Point3D( prisoner.X, prisoner.Y, prisoner.Z );
			int mushx;
			int mushy;
			int mushz;

			InternalItem firstFlamea = new InternalItem( prisoner.Location, prisoner.Map, jailor );
			mushx = loc.X - 2;
			mushy = loc.Y - 2;
			mushz = loc.Z;
			Point3D mushxyz = new Point3D( mushx, mushy, mushz );
			firstFlamea.MoveToWorld( mushxyz, prisoner.Map );

			InternalItem firstFlamec = new InternalItem( prisoner.Location, prisoner.Map, jailor );
			mushx = loc.X;
			mushy = loc.Y - 3;
			mushz = loc.Z;
			Point3D mushxyzb = new Point3D( mushx, mushy, mushz );
			firstFlamec.MoveToWorld( mushxyzb, prisoner.Map );

			InternalItem firstFlamed = new InternalItem( prisoner.Location, prisoner.Map, jailor );
			firstFlamed.ItemID = 0x3709;
			mushx = loc.X + 2;
			mushy = loc.Y - 2;
			mushz = loc.Z;
			Point3D mushxyzc = new Point3D( mushx, mushy, mushz );
			firstFlamed.MoveToWorld( mushxyzc, prisoner.Map );
			InternalItem firstFlamee = new InternalItem( prisoner.Location, prisoner.Map, jailor );
			mushx = loc.X + 3;
			firstFlamee.ItemID = 0x3709;
			mushy = loc.Y;
			mushz = loc.Z;
			Point3D mushxyzd = new Point3D( mushx, mushy, mushz );
			firstFlamee.MoveToWorld( mushxyzd, prisoner.Map );
			InternalItem firstFlamef = new InternalItem( prisoner.Location, prisoner.Map, jailor );
			firstFlamef.ItemID = 0x3709;
			mushx = loc.X + 2;
			mushy = loc.Y + 2;
			mushz = loc.Z;
			Point3D mushxyze = new Point3D( mushx, mushy, mushz );
			firstFlamef.MoveToWorld( mushxyze, prisoner.Map );
			InternalItem firstFlameg = new InternalItem( prisoner.Location, prisoner.Map, jailor );
			mushx = loc.X;
			firstFlameg.ItemID = 0x3709;
			mushy = loc.Y + 3;
			mushz = loc.Z;
			Point3D mushxyzf = new Point3D( mushx, mushy, mushz );
			firstFlameg.MoveToWorld( mushxyzf, prisoner.Map );
			InternalItem firstFlameh = new InternalItem( prisoner.Location, prisoner.Map, jailor );
			mushx = loc.X - 2;
			firstFlameh.ItemID = 0x3709;
			mushy = loc.Y + 2;
			mushz = loc.Z;
			Point3D mushxyzg = new Point3D( mushx, mushy, mushz );
			firstFlameh.MoveToWorld( mushxyzg, prisoner.Map );
			InternalItem firstFlamei = new InternalItem( prisoner.Location, prisoner.Map, jailor );
			mushx = loc.X - 3;
			firstFlamei.ItemID = 0x3709;
			mushy = loc.Y;
			mushz = loc.Z;
			Point3D mushxyzh = new Point3D( mushx, mushy, mushz );
			firstFlamei.MoveToWorld( mushxyzh, prisoner.Map );
			new JailWraith( this, prisoner.X + 15, prisoner.Y + 15, m_jailor );
		}

		public Mobile Prisoner
		{
			get { return m_prisoner; }
		}

		public void jail()
		{
			JailSystem.Jail( m_prisoner, TimeSpan.FromDays( 2 ), "Interefering with a Role-Playing event.", true, m_jailor.Name, AccessLevel.Seer );
			m_prisoner.CantWalk = false;
			m_prisoner.Squelched = false;
			m_prisoner.SendMessage( "You are now in jail for disrupting an event.  Do not expect to see the staff member who jailed you until after the event has ended." );
		}

		public static void Initialize()
		{
			CommandSystem.Register( "jailwraith", AccessLevel.GameMaster, jail_OnCommand );
		}

		[Usage( "jailwraith" )]
		[Description( "Places the selected player in jail by a wraith." )]
		public static void jail_OnCommand( CommandEventArgs e )
		{
			if( e.Mobile is PlayerMobile )
			{
				e.Mobile.Target = new InternalTarget();
				e.Mobile.SendLocalizedMessage( 3000218 );
			}
		}

		private class InternalTarget : Target
		{
			public InternalTarget() : base( -1, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
                if (from is PlayerMobile && targeted is PlayerMobile)
                {
                    PlayerMobile pm = (PlayerMobile) targeted;
                    if (from.AccessLevel > pm.AccessLevel)
                        new WraithJailEffect(targeted as PlayerMobile, from as PlayerMobile);
                    else
                        pm.SendAsciiMessage("{0} tried to use .jailwraith on you", from.Name);
                }
			}
		}

		private class InternalItem : Item
		{
			private Timer m_Timer;
			private DateTime m_End;
			private Mobile m_Caster;

			public InternalItem( Point3D loc, Map map, Mobile caster ) : base( 0x3709 )
			{
				Visible = false;
				Movable = false;
				Light = LightType.Circle150;
				MoveToWorld( loc, map );
				m_Caster = caster;
				Visible = true;
				m_Timer = new InternalTimer( this, TimeSpan.FromSeconds( 30.0 ) );
				m_Timer.Start();

				m_End = DateTime.Now + TimeSpan.FromSeconds( 30.0 );
			}

			public InternalItem( Serial serial ) : base( serial )
			{
			}

			public override bool BlocksFit
			{
				get { return true; }
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( 1 ); // version

				writer.Write( m_End - DateTime.Now );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();

				switch( version )
				{
					case 1:
					{
						TimeSpan duration = reader.ReadTimeSpan();

						m_Timer = new InternalTimer( this, duration );
						m_Timer.Start();

						m_End = DateTime.Now + duration;

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

				if( m_Timer != null )
					m_Timer.Stop();
			}

			private class InternalTimer : Timer
			{
				private readonly InternalItem m_Item;

				public InternalTimer( InternalItem item, TimeSpan duration ) : base( duration )
				{
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