using Server;
using System;

namespace Server.Custom.Games
{
	public class BombermanBombPlacer : Item
	{
        protected BombermanGame m_Game;
        protected Mobile m_Owner;

		private int m_Strength = 3;
		private int m_MaxBombs = 1;
		private int m_BombsInWorld = 0;
        private float m_Speed = 0.01f;

        private bool m_Active;

		[Constructable()]
		public BombermanBombPlacer( Mobile owner, BombermanGame game ) : base( 0x1ED0 )
		{
            m_Owner = owner;
            m_Game = game;
            Active = false;
            EventItem = true;
			Movable = false;
            LootType = LootType.Newbied;
			Weight = 1.0;
			Name = "Bombplacer";
		}

        public BombermanBombPlacer()
        {

        }
		
		public BombermanBombPlacer( Serial serial ) : base( serial )
		{
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get { return m_Owner; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Active
        {
            get
            {
                return m_Active; 
            }
            set
            {
                m_Active = value;
            }
        }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int Strength
		{ 
			get{ return m_Strength; } 
			set
			{ 
				m_Strength = value;
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxBombs
		{ 
			get{ return m_MaxBombs; } 
			set
			{ 
				m_MaxBombs = value;
			}
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public float SpreadSpeed
        {
            get { return m_Speed; }
            set
            {
                if(value > 0.003)
                    m_Speed = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BombsInWorld
        {
            get { return m_BombsInWorld; }
            protected set { m_BombsInWorld = value; }
        }
		
		public void BombDetonated()
		{
			--m_BombsInWorld;
		}

		public override bool Decays{ get{ return false; } }
		
		public override void OnDoubleClick( Mobile from )
		{
            if (!Active)
            {
                from.SendMessage("This bombplacer is not active. Wait until the game has started!");
                return;
            }

			if(!from.CanBeDamaged())
			{
				from.SendMessage( "You cannot place bombs in this area" );
				return;
			}
			if(m_BombsInWorld < m_MaxBombs)
			{
				BombermanBomb bomb = new BombermanBomb( this, m_Game );
				bomb.MoveToWorld( from.Location, from.Map );
			    ++m_BombsInWorld;
			} else {
				from.SendMessage( "You cannot place anymore bombs." );
			}
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)2 );

            // 2
            writer.Write(m_Game);
            writer.Write(m_Owner);

            // 1
            writer.Write(m_Speed);

            // 0
			writer.Write( m_Strength );
			writer.Write( m_MaxBombs );
            writer.Write(m_BombsInWorld);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			switch ( version )
            {
                case 2:
                {
                    m_Game = reader.ReadItem() as BombermanGame;
                    m_Owner = reader.ReadMobile();
                    goto case 1;
                }
				case 1:
                {

                        m_Speed = reader.ReadFloat();

                    goto case 0;
                }
                case 0:
                {
                    m_Strength = reader.ReadInt();
                    m_MaxBombs = reader.ReadInt();
                    m_BombsInWorld = reader.ReadInt();

                    break;
                }
			}
		}
	}
}

