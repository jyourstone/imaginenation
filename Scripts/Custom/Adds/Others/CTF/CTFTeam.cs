using System.Collections;
using System;

namespace Server.Custom.Games
{
	public class CTFTeam : BaseGameTeam
	{
		private Point3D m_FlagHome;
		private IGameFlag m_Flag;

		public CTFTeam( BaseTeamGame game, string name ) : base(game, name)
		{
            
		}

		public override void Serialize( GenericWriter writer )
		{
            base.Serialize(writer);

			writer.Write( 1 );//version

			writer.Write( (Item)m_Flag );

			writer.Write( m_FlagHome );
		}

		public override void Deserialize( GenericReader reader )
		{
            base.Deserialize(reader);

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_Flag = reader.ReadItem() as IGameFlag;
					m_FlagHome = reader.ReadPoint3D();
					break;
				}
			}
		}

		public IGameFlag Flag{ get{ return m_Flag; } set {m_Flag = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D FlagHome{ get{ return m_FlagHome; } set{ m_FlagHome = value; } }

	    [CommandProperty(AccessLevel.Developer)]
	    public int KillPoints { get; set; }

		[CommandProperty( AccessLevel.Counselor )]
		public int ActiveMemberCount
		{
			get
			{
				int count = 0;

                foreach (Mobile m in Players)
                {
                    if (m.NetState != null)
                        ++count;
                }

				return count;
			}
		}
	}
}
