using Server;
using Server.Items;

namespace Server.Custom.Games
{
	public class BombermanUpgrade : Item
	{
		public enum BombermanUpgradeType
		{
			None,
			Strength,
			BombAmount,
            SpreadSpeed,
            KickBombPlacer
		}

        private BombermanGame m_Game;
		private BombermanUpgradeType m_UpgradeType;

		[Constructable]
		public BombermanUpgrade( BombermanUpgradeType type, BombermanGame game ) : base( 0x1ED0 )
		{
            m_Game = game;
			this.Movable = false;
			this.Weight = 1.0;
			m_UpgradeType = type;
			if(type == BombermanUpgradeType.BombAmount)
			{
				Hue = 1169;
            }
            if (type == BombermanUpgradeType.SpreadSpeed)
            {
                this.Hue = 3;
            }
            if (type == BombermanUpgradeType.KickBombPlacer)
            {
                this.Hue = 1170;
            }
			Name = "Bomberman upgrade: " + Type;
		}
		
		public BombermanUpgrade( Serial serial ) : base( serial )
		{
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public BombermanUpgradeType Type
		{ 
			get{ return m_UpgradeType; } 
			set
			{ 
				m_UpgradeType = value;
			}
		}

		public override bool Decays{ get{ return false; } }

		public override bool OnMoveOver( Mobile m )
		{
            if(m_Game != null)
                m_Game.RemoveItemFromGameArea(this);
			Container pack = m.Backpack;
			Item[] placers = pack.FindItemsByType( typeof(BombermanBombPlacer) );
            foreach (Item placer in placers)
            {
                if(placer is BombermanBombPlacer)
                {
                    BombermanBombPlacer realPlacer = (BombermanBombPlacer)placer;
                    if (placer != null)
                    {
                        if (Type == BombermanUpgradeType.Strength)
                        {
                            realPlacer.Strength = realPlacer.Strength + 1;
                            m.SendAsciiMessage("Your bombs are now more powerful.");
                        }
                        else if (Type == BombermanUpgradeType.BombAmount)
                        {
                            realPlacer.MaxBombs = realPlacer.MaxBombs + 1;
                            m.SendAsciiMessage("You can now place more bombs at the same time.");
                        }
                        else if (Type == BombermanUpgradeType.SpreadSpeed)
                        {
                            realPlacer.SpreadSpeed -= 0.001f;
                            m.SendAsciiMessage("Your bombs will spread faster.");
                        }
                        else if (Type == BombermanUpgradeType.KickBombPlacer)
                        {
                            BombermanKickBombPlacer kplacer = (BombermanKickBombPlacer)pack.FindItemByType(typeof(BombermanKickBombPlacer));
                            if (kplacer == null)
                            {
                                BombermanKickBombPlacer kickplacer = new BombermanKickBombPlacer(m, m_Game);
                                kickplacer.EventItem = true;
                                kickplacer.LootType = LootType.Newbied;
                                kickplacer.Strength = realPlacer.Strength;
                                kickplacer.MaxBombs = realPlacer.MaxBombs;
                                kickplacer.SpreadSpeed = realPlacer.SpreadSpeed;

                                m.AddToBackpack(kickplacer);
                                m.SendAsciiMessage("You have received a kickbombplacer. You can now kick bombs over stones.");
                            }
                            else
                            {
                                kplacer.Range += 1;
                                m.SendAsciiMessage("The range of your kickbombplacer has increased.");
                            }
                        }
                    }
                    else
                        m.SendAsciiMessage("You do not have a Bomberman Bomb Placer in your backpack");
                }
            }

            Delete();
            return true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)1 );

            // 2
            writer.Write( m_Game);

            // 1
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
					m_UpgradeType = (BombermanUpgradeType)reader.ReadByte();

					break;
				}
			}
		}
	}
}

