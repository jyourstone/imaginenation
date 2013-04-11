using System;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class PitsRune : Item
	{
		public Point3D m_Target;
		public Map m_TargetMap;

		[Constructable]
		public PitsRune() : base( 0x1F14 )
		{
			m_Target = new Point3D( 1987, 3093, 0 );
			m_TargetMap = Map.Felucca;
			Movable = false;
			Hue = 2996;
			Name = "Pits rune";
		}

		public PitsRune( Serial serial ) : base( serial )
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

		public override bool OnMoveOver( Mobile m )
		{
			if( m.SolidHueOverride != -1 )
				return true;
		    if( m.Player && !m.Alive ) // mobile is dead
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
		    if( ( m.Player ) && ( m.Hits < m.HitsMax ) )
		        m.LocalOverheadMessage( MessageType.Regular, 906, true, "You do not have full HP." );
		    else if( m.Player && m.SolidHueOverride != 2535 )
		    {
		        new RuneTimer( this, m ).Start();
		        m.SolidHueOverride = 2535;
		    }

		    return true;
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

	public class RuneTimer : Timer
	{
		private readonly Mobile m_Mobile;
	    private int m_State;
	    private readonly int m_Count;
		private readonly PitsRune m_Item;
        private readonly bool m_Alive;

		public RuneTimer( PitsRune item, Mobile m ) : this( m, 0 )
		{
			m_Item = item;
		}

		public RuneTimer( Mobile m, int count ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
		{
            m_State = 6;
			m_Mobile = m;
			m_Count = count;
		    m_Alive = m_Mobile.Alive;
		}

		protected override void OnTick()
		{
			m_State--;

			if( m_State >= m_Count && m_State >= 1 )
				m_Mobile.PublicOverheadMessage( MessageType.Spell, m_Mobile.SpeechHue, true, ( m_State ).ToString(), false );

			if( m_Mobile.Hits < m_Mobile.HitsMax ) // Health Cancel
			{
				m_Mobile.Say( "Interrupted!" );
				m_Mobile.SolidHueOverride = -1;
				Stop();
			}
            else if (m_Alive && !m_Mobile.Alive)
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