using System;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class CasinoEntrance : Item
	{
		public Point3D m_Target;
		public Map m_TargetMap;

		[Constructable]
		public CasinoEntrance() : base( 0x519 )
		{
			m_Target = new Point3D( 1343, 1744, 20 );
			m_TargetMap = Map.Felucca;
			Movable = false;
			Visible = false;
			Hue = 2544;
			Name = "Casino entrance";
		}

		public CasinoEntrance( Serial serial ) : base( serial )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Target
		{
			get { return m_Target; }
			set { m_Target = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Map TargetMap
		{
			get { return m_TargetMap; }
			set { m_TargetMap = value; }
		}

		public static bool IsInGuild( Mobile m )
		{
			return ( m is PlayerMobile && ( (PlayerMobile)m ).NpcGuild == NpcGuild.ThievesGuild );
		}

		public override bool OnMoveOver( Mobile m )
		{
			if( m.SolidHueOverride != -1 )
				return true;
			else if( m.Player && !m.Alive ) // mobile is dead
			{
				m.SolidHueOverride = -1;

				if( m_TargetMap != null && m_TargetMap != Map.Internal )
				{
					m.MoveToWorld( m_Target, m_TargetMap );
					BaseCreature.TeleportPets( m, m_Target, m_TargetMap );
				}
				else
					m.SendAsciiMessage( "This has not yet been activated" );

				m.Combatant = null;
				m.Warmode = false;
				m.Resurrect();
				return false;
			}
			else if( ( m.Player ) && ( m.Hits < m.HitsMax ) )
				m.LocalOverheadMessage( MessageType.Regular, 906, true, "For liablity issues, we require all visitors to be in perfect health!" );
			else if( m.Player && m.SolidHueOverride != 2544 )
			{
                m_CasinoTimer = new CasinoTimer(this, m);
                m_CasinoTimer.Start();				
				m.SolidHueOverride = 2544;
				m.SendAsciiMessage( "You are about to enter the Casino!" );
			}

			return true;
		}

        public CasinoTimer m_CasinoTimer;
        public override bool OnMoveOff(Mobile m)
        {
            if ( m == null || ( m_CasinoTimer == null || m.SolidHueOverride != 2544 ) )
                return true;

            m_CasinoTimer.Stop();
            m.SolidHueOverride = -1;
            return base.OnMoveOff(m);
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // version

			writer.Write( m_Target );
			writer.Write( m_TargetMap );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			m_Target = reader.ReadPoint3D();
			m_TargetMap = reader.ReadMap();
		}
	}

	public class CasinoTimer : Timer
	{
		private readonly Mobile m_Mobile;
	    private int m_State;
	    private readonly int m_Count;
		private readonly CasinoEntrance m_Item;
	    private readonly Point3D m_OrigLoc;

		public CasinoTimer( CasinoEntrance item, Mobile m ) : this( m, 0 )
		{
			m_Item = item;
		    m_OrigLoc = item.Location;
		}

		public CasinoTimer( Mobile m, int count ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
		{
            m_State = 6;
			m_Mobile = m;
			m_Count = count;
		}

		protected override void OnTick()
		{
			m_State--;

			if( m_State >= m_Count && m_State >= 1 )
				m_Mobile.PublicOverheadMessage( MessageType.Spell, m_Mobile.SpeechHue, true, ( m_State ).ToString(), false );

			if( m_Mobile.Hits < m_Mobile.HitsMax ) // Health Cancel
			{
				m_Mobile.Say( "Keep your problems outside!" );
				m_Mobile.SolidHueOverride = -1;
				Stop();
			}
            if (m_Mobile.Location != m_OrigLoc) // Mobile changed location, most likely recalled
            {
                m_Mobile.SolidHueOverride = -1;
                Stop();
            }
			else if( m_State == m_Count ) // Complete
			{
				m_Mobile.SolidHueOverride = -1;

				if( m_Item.m_TargetMap != null && m_Item.m_TargetMap != Map.Internal )
				{
					m_Mobile.MoveToWorld( m_Item.m_Target, m_Item.m_TargetMap );
					BaseCreature.TeleportPets( m_Mobile, m_Item.m_Target, m_Item.m_TargetMap );
				}
				else
					m_Mobile.SendAsciiMessage( "This has not yet been activated" );

				m_Mobile.Combatant = null;
				m_Mobile.Warmode = false;

				Effects.SendLocationParticles( EffectItem.Create( m_Mobile.Location, m_Mobile.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				m_Mobile.PlaySound( 0x1FE );

				Stop();
			}
		}
	}
}