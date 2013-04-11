using System;
using System.Collections;

namespace Server.Custom.Games
{
	public class BombermanBomb : Item
	{
        private BombermanGame m_Game;
		private BombermanBombPlacer m_Placer;
		
		public static void Initialize()
		{
			TileData.ItemTable[0x2256].Flags = TileFlag.Impassable | TileFlag.PartialHue;
		}
		
		[Constructable]
		public BombermanBomb( BombermanBombPlacer placer, BombermanGame game ) : base( 0x2256 )
		{
            m_Game = game;
			Movable = false;
			Weight = 1.0;
			Name = "Bomberman bomb";
			Hue = 1;
			
			m_Placer = placer;
			BombTimer m_Timer = new BombTimer( this );
			m_Timer.Start();
		}
		
		public BombermanBomb( Serial serial ) : base( serial )
		{
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public BombermanBombPlacer BombPlacer
		{ 
			get{ return m_Placer; } 
			set
			{ 
				m_Placer = value;
			} 
		}

        public override void OnAfterDelete()
        {
            m_Placer.BombDetonated();
            base.OnAfterDelete();
        }

		public override bool Decays{ get{ return false; } }
		
		public void detonate()
		{
			Visible = false;
			
			DetonateTimer detonation = new DetonateTimer( this, m_Game, m_Placer.Owner );
			detonation.Start();
		}

		public override bool OnMoveOver( Mobile m )
		{
			return false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)1 );

            // 1
            writer.Write(m_Game);

            // 0
			writer.Write( m_Placer );
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
					m_Placer = reader.ReadItem() as BombermanBombPlacer;

					break;
				}
            }
            BombTimer m_Timer = new BombTimer(this);
            m_Timer.Start();
		}
		
		private class DetonateTimer : Timer
		{
			private readonly BombermanBomb m_Bomb;
            private BombermanGame m_Game;
            private Mobile m_Owner;
		    private int count = 1;
			private readonly int maxcount;

            private Point3D DetonateLocation;
            private Map DetonateMap;
			private bool continueTop = true;
			private bool continueRight = true;
			private bool continueBottom = true;
			private bool continueLeft = true;
			
			public DetonateTimer(BombermanBomb bomb, BombermanGame game, Mobile owner) : base( TimeSpan.Zero, TimeSpan.FromSeconds( bomb.BombPlacer.SpreadSpeed ) )
			{
				m_Bomb = bomb;
                m_Game = game;
                m_Owner = owner;
                DetonateLocation = m_Bomb.Location;
                DetonateMap = m_Bomb.Map;
			    Priority = TimerPriority.TwoFiftyMS;
                maxcount = m_Bomb.BombPlacer.Strength;
                detonateLocation(DetonateLocation);
                m_Bomb.Delete();
			}
			
			protected override void OnTick()
			{
                //Console.WriteLine( "tick" );
                int x = m_Bomb.X;
                int y = m_Bomb.Y;
                int z = m_Bomb.Z;

                if (continueRight)
                    continueRight = detonateLocation(new Point3D(x + count, y, z));
                if (continueLeft)
                    continueLeft = detonateLocation(new Point3D(x - count, y, z));
                if (continueTop)
                    continueTop = detonateLocation(new Point3D(x, y + count, z));
                if (continueBottom)
                    continueBottom = detonateLocation(new Point3D(x, y - count, z));

                if (++count == maxcount)
                {
                    Stop();
                }
            }

            public bool detonateLocation(Point3D location)
            {
                IPooledEnumerable items = DetonateMap.GetItemsInRange(location, 0);
                ArrayList todestroy = new ArrayList();
                foreach (Item item in items)
                {
                    if (item is BombermanStone)
                    {
                        BombermanStone stone = (BombermanStone)item;
                        if (stone.Destructable)
                        {
                            Effects.SendLocationEffect(location, DetonateMap, 0x36CB, 10);
                            todestroy.Add(stone);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                items.Free();

                for (int i = 0; i < todestroy.Count; i++)
                {
                    if ((todestroy[i]) != null)
                        ((BombermanStone)todestroy[i]).destroy();
                }
                if (todestroy.Count > 0)
                    return false;

                Effects.SendLocationEffect(location, DetonateMap, 0x36CB, 10);
                ArrayList tokill = new ArrayList();
                IPooledEnumerable mobiles = DetonateMap.GetMobilesInRange(location, 0);
                foreach (Mobile mobile in mobiles)
                {
                    tokill.Add(mobile);
                }
                mobiles.Free();

                for (int i = 0; i < tokill.Count; i++)
                {
                    if (((Mobile)tokill[i]).NetState != null)
                    {
                        if (((Mobile)tokill[i]).CanBeDamaged())
                        {
                            /*if(m_Game == null)
                                Console.WriteLine("game");
                            if(tokill[i] == null)
                                Console.WriteLine("tokill");
                            if(m_Owner == null)
                                Console.WriteLine("owner");*/
                            if (tokill[i] == m_Owner)
                            {
                                m_Game.AnnounceToPlayers(2593, (m_Owner.Name + " has killed himself"));
                            }
                            else
                            {
                                m_Game.AnnounceToPlayers(2593, ((Mobile)tokill[i]).Name + " has been killed by " + m_Owner.Name);
                            }
                            ((Mobile)tokill[i]).Kill();
                        }
                    }
                }

                return true;
            }
        }
		
		private class BombTimer : Timer
		{
			public static readonly TimeSpan detonateTime = TimeSpan.FromSeconds( 3.0 );
			private readonly DateTime m_Start;
			private readonly BombermanBomb m_Bomb;

		    public BombTimer(BombermanBomb bomb) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Bomb = bomb;
			    m_Start = DateTime.Now;
				Priority = TimerPriority.TwoFiftyMS;
			}
			
			protected override void OnTick()
			{
                if (m_Bomb != null && !m_Bomb.Deleted)
                {
                    TimeSpan left = detonateTime - (DateTime.Now - m_Start);
                    if (left <= TimeSpan.FromSeconds(0.0))
                    {
                        m_Bomb.detonate();
                        Stop();
                    }
                }
                else
                    Stop();
			}
		}
	}
}
