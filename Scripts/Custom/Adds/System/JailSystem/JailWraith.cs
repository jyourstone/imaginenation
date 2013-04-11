using System;
using Server.Jailing;
using Server.Regions;

namespace Server.Mobiles
{
	[CorpseName( "a errie corpse" )]
	public class JailWraith : BaseCreature
	{
		private readonly WraithJailEffect m_effect;
		private bool drag = false;
		private readonly int m_endPointX;
		private readonly int m_endPointY;

		public JailWraith( WraithJailEffect effect, int endPointX, int endPointY, Mobile m_jailor ) : base( AIType.AI_Use_Default, FightMode.None, 10, 1, 0.2, 0.4 )
		{
			m_endPointY = endPointY;
			m_endPointX = endPointX;
			m_effect = effect;
			Name = "a soulless demon";
			Body = 26;
			Hue = 0x4001;
			BaseSoundID = 0x482;
			Blessed = true;

			Fame = 4000;
			Karma = -4000;

			VirtualArmor = 28;
			CantWalk = true;
			new InternalTimer( this );
			X = m_jailor.X;
			Y = m_jailor.Y;
			Map = m_jailor.Map;
		}

		public JailWraith( Serial serial ) : base( serial )
		{
		}

		protected override void OnLocationChange( Point3D loc )
		{
			base.OnLocationChange( loc );
			m_effect.Prisoner.Hidden = false;
			Stam = 1000;
			if( drag )
			{
				Direction = GetDirectionTo( m_endPointX, m_endPointY );
				if( ( X == m_endPointX ) && ( Y == m_endPointY ) )
				{
				    m_effect.jail();
					Delete();
				}
				else if( !( m_effect.Prisoner.Region is Jail ) )
					m_effect.Prisoner.Location = loc;
			}
			else if( ( X == m_effect.Prisoner.X ) && ( Y == m_effect.Prisoner.Y ) )
			{
				Direction = GetDirectionTo( m_endPointX, m_endPointY );
                m_effect.Prisoner.Kill();
                m_effect.Prisoner.Hidden = false;
			    drag = true;
				PlaySound( GetAngerSound() );
			}
			else
				Direction = GetDirectionTo( m_effect.Prisoner.Location );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

		private class InternalTimer : Timer
		{
			private readonly JailWraith m_Item;

			public InternalTimer( JailWraith item ) : base( TimeSpan.FromMilliseconds( 200 ), TimeSpan.FromMilliseconds( 200 ) )
			{
				m_Item = item;
				Start();
				Priority = TimerPriority.FiftyMS;
			}

			protected override void OnTick()
			{
				//yeah baby move me
				if( m_Item.Deleted )
					Stop();
				int x;
				int y;
				x = m_Item.X;
				y = m_Item.Y;
				switch( m_Item.Direction )
				{
					case Direction.North:
						x = x - 1;
						break;
					case Direction.Left:
						y++;
						x = x - 1;
						break;
					case Direction.West:
						y++;
						break;
					case Direction.Down:
						y++;
						x++;
						break;
					case Direction.South:
						x++;
						break;
					case Direction.Right:
						y = y - 1;
						x++;
						break;
					case Direction.East:
						y = y - 1;
						break;
					case Direction.Up:
						y = y - 1;
						x = x - 1;
						break;
					default:
						break;
				}
				m_Item.Location = new Point3D( x, y, m_Item.Z );
			}
		}
	}
}