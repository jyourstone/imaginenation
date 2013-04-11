using Server;

namespace Server.Custom.Games
{
	public class BombermanStone : Item
	{
        private BombermanGame m_Game;

		private bool m_Destructable;
		private BombermanUpgrade.BombermanUpgradeType m_UpgradeType;

		[Constructable]
		public BombermanStone( bool destructable, BombermanGame game ) : base( 0x1363 )
		{
            m_Game = game;
			Movable = false;
			Weight = 1.0;
			Name = "Bomberman stone";
			if(destructable)
				Hue = 1301;
			m_Destructable = destructable;
			
			switch( Utility.Random( 22 ) )
			{
				case 1:
				m_UpgradeType = BombermanUpgrade.BombermanUpgradeType.Strength;
				break;
				case 2:
				m_UpgradeType = BombermanUpgrade.BombermanUpgradeType.BombAmount;
				break;
                case 3:
                m_UpgradeType = BombermanUpgrade.BombermanUpgradeType.Strength;
                break;
                case 4:
                m_UpgradeType = BombermanUpgrade.BombermanUpgradeType.BombAmount;
                break;
                case 5:
                m_UpgradeType = BombermanUpgrade.BombermanUpgradeType.SpreadSpeed;
                break;
                case 6:
                m_UpgradeType = BombermanUpgrade.BombermanUpgradeType.KickBombPlacer;
                break;
				default:
				m_UpgradeType = BombermanUpgrade.BombermanUpgradeType.None;
				break;
			}
		}
		
		public BombermanStone( Serial serial ) : base( serial )
		{
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Destructable
		{ 
			get{ return m_Destructable; } 
			set
			{ 
				m_Destructable = value;
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public BombermanUpgrade.BombermanUpgradeType UpgradeType
		{ 
			get{ return m_UpgradeType; } 
			set
			{ 
				m_UpgradeType = value;
			}
		}

		public override bool Decays{ get{ return false; } }
		
		public void destroy()
		{
			if(!m_Destructable)
			{
				return;
			}
			
			if(m_UpgradeType != BombermanUpgrade.BombermanUpgradeType.None)
			{
				BombermanUpgrade upgrade = new BombermanUpgrade( m_UpgradeType, m_Game );
				upgrade.MoveToWorld( Location, Map );
                if(m_Game != null)
                    m_Game.AddItemToGameArea(upgrade);
			}
			
			Delete();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)1 );

            // 1
            writer.Write(m_Game);

            // 0
			writer.Write( m_Destructable );
			writer.Write( (byte)m_UpgradeType );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			switch ( version )
			{
                case 1:
                    m_Game = reader.ReadItem() as BombermanGame;
                    goto case 0;
				case 0:
				{
					m_Destructable = reader.ReadBool();
					m_UpgradeType = (BombermanUpgrade.BombermanUpgradeType)reader.ReadByte();

					break;
				}
			}
		}
	}
}

