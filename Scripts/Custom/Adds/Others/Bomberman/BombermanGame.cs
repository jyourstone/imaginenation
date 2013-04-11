using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Targeting;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Scripts.Custom.Adds.System;

namespace Server.Custom.Games
{
    public class BombermanGame : BaseGame
    {

        #region The class
        private Rectangle2D m_GameArea;
        private Point3D m_WinnerLocation;

        private List<Item> m_GameItems = new List<Item>();

        #region Constructor And Serialize

        [Constructable]
        public BombermanGame()
            : base(0xEDC)
        {
            Name = "Bomberman stone";
            GameName = "Bomberman";
            m_UseGump = false;
            EventType = EventType.Bomberman;
        }

        public BombermanGame(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2);//version

            //2
            writer.Write(m_GameItems);
            writer.Write(WinnerLocation);

            //1
            writer.Write(m_GameArea);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        m_GameItems = reader.ReadStrongItemList();
                        m_WinnerLocation = reader.ReadPoint3D();
                        goto case 1;
                    }
                case 1:
                    {
                        m_GameArea = reader.ReadRect2D();
                        break;
                    }
            }
        }
        #endregion

        #region fields

        #region Script only
        public Rectangle2D GameArea
        {
            set
            {
                m_GameArea = value;
            }
            get
            {
                return m_GameArea;
            }
        }
        #endregion

        #region GameMaster
        [CommandProperty(AccessLevel.GameMaster)]
        public Point2D GameAreaStartPoint
        {
            set
            {
                m_GameArea.Start = value;
            }
            get
            {
                return m_GameArea.Start;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point2D GameAreaEndPoint
        {
            set
            {
                m_GameArea.End = value;
            }
            get
            {
                return m_GameArea.End;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D WinnerLocation
        {
            set
            {
                m_WinnerLocation = value;
            }
            get
            {
                return m_WinnerLocation;
            }
        }
        #endregion

        #endregion

        public override void StartGame(Mobile gm)
        {
            base.StartGame(gm);

            m_GameItems = new List<Item>();

            Rectangle2D playerarea = new Rectangle2D(GameAreaStartPoint.X, GameAreaStartPoint.Y,
                                                     GameAreaEndPoint.X - GameAreaStartPoint.X + 1,
                                                     GameAreaEndPoint.Y - GameAreaStartPoint.Y + 1);
            IPooledEnumerable players = Map.GetMobilesInBounds(playerarea);
            foreach (Mobile player in players)
            {
                if (player.AccessLevel == AccessLevel.Player)
                    AddPlayer(player);
            }

            #region Create stones for gamearea
            int startx = m_GameArea.Start.X;
            int starty = m_GameArea.Start.Y;
            int endx = m_GameArea.End.X;
            int endy = m_GameArea.End.Y;

            BombermanStone stone;
            for (int x = 0; x <= endx - startx; ++x)
            {
                for (int y = 0; y <= endy - starty; ++y)
                {
                    if(x != 0 && x != endx - startx && y != 0 && y != endy - starty)
                    {
                        if (x % 2 == 1 && y % 2 == 1)
                        {
                            stone = new BombermanStone(false, this);
                        }
                        else
                        {
                            stone = new BombermanStone(true, this);
                        }
                        stone.MoveToWorld(new Point3D(startx + x, starty + y, Map.Tiles.GetLandTile(startx + x, starty + y).Z), Map);
                        m_GameItems.Add(stone);
                    }
                    else if ((x == 0 || x == endx - startx) && y > 1 && y < endy - starty - 1)
                    {
                        stone = new BombermanStone(true, this);
                        stone.MoveToWorld(new Point3D(startx + x, starty + y, Map.Tiles.GetLandTile(startx + x, starty + y).Z), Map);
                        m_GameItems.Add(stone);
                    }
                    else if ((y == 0 || y == endy - starty) && x > 1 && x < endx - startx - 1)
                    {
                        stone = new BombermanStone(true, this);
                        stone.MoveToWorld(new Point3D(startx + x, starty + y, Map.Tiles.GetLandTile(startx + x, starty + y).Z), Map);
                        m_GameItems.Add(stone);
                    }
                }
            }
            #endregion

            #region Handle players: location, items, etc
            Point2D upleft = m_GameArea.Start;
            Point2D downright = m_GameArea.End;
            Point2D upright = new Point2D(downright.X, upleft.Y);
            Point2D downleft = new Point2D(upleft.X, downright.Y);

            byte index = 0;
            List<Mobile> toRemove = new List<Mobile>();
            foreach (Mobile m in Players)
            {
                switch (index++)
                {
                    case 0:
                        m.Location = new Point3D(upleft, Map.Tiles.GetLandTile(upleft.X, upleft.Y).Z);
                        break;
                    case 1:
                        m.Location = new Point3D(downright, Map.Tiles.GetLandTile(upright.X, upright.Y).Z);
                        break;
                    case 2:
                        m.Location = new Point3D(downleft, Map.Tiles.GetLandTile(downleft.X, downleft.Y).Z);
                        break;
                    case 3:
                        m.Location = new Point3D(upright, Map.Tiles.GetLandTile(downright.X, downright.Y).Z);
                        break;
                    default:
                        toRemove.Add(m);
                        PublicOverheadMessage(MessageType.Regular, 906, true, "There are more than 4 mobiles in the game area.");
                        break;
                }
            }

            foreach (Mobile mob in toRemove)
            {
                RemovePlayer(mob);
            }
            #endregion

            StartTimer timer = new StartTimer(this);
            timer.Start();

            gm.SendMessage("The game has been started.");
        }

        public override void OnAfterStart()
        {
            base.OnAfterStart();
            foreach (Mobile player in Players)
            {
                BombermanBombPlacer placer = new BombermanBombPlacer(player, this);
                player.AddToBackpack(placer);
            }
        }

        public override void EndGame()
        {
            base.EndGame();
            List<Item> toDelete = new List<Item>();
            Rectangle2D removeArea = m_GameArea;
            Point2D end = removeArea.End;
            ++end.X; ++end.Y;
            removeArea.End = end;
            /*IPooledEnumerable items = Map.GetItemsInBounds(m_GameArea);
            foreach (Item item in items)
            {
                if (item is BombermanStone || item is BombermanBomb || item is BombermanUpgrade)
                {
                    toDelete.Add(item);
                }
            }*/
            foreach (Item item in m_GameItems)
            {
                toDelete.Add(item);
            }
            foreach (Item del in toDelete)
            {
                del.Delete();
            }

            List<Mobile> toKill = new List<Mobile>();
            foreach (Mobile m in Players)
            {
                toKill.Add(m);
            }

            foreach (Mobile m in toKill)
            {
                RemovePlayer(m);
            }

            m_GameItems = new List<Item>();
        }

        public override void GivePrizesToPlayers()
        {
            
        }

        public void AddItemToGameArea(Item i)
        {
            m_GameItems.Add(i);
        }

        public void RemoveItemFromGameArea(Item i)
        {
            m_GameItems.Remove(i);
        }

        public List<BombermanBombPlacer> GetBombPlacerFromMobile(Mobile m)
        {
            List<BombermanBombPlacer> list = new List<BombermanBombPlacer>();
            if (m.Backpack != null)
            {
                foreach (Item i in m.Backpack.Items)
                {
                    if (i is BombermanBombPlacer || i is BombermanKickBombPlacer)
                        list.Add((BombermanBombPlacer) i);
                }
            }

            return list;
        }

        public void StartGameTimerFinished()
        {
            foreach (Mobile m in Players)
            {
                foreach(BombermanBombPlacer placer in GetBombPlacerFromMobile(m))
                    placer.Active = true;
            }
        }

        public override void OnPlayerDeath(Mobile mob)
        {
            base.OnPlayerDeath(mob);

            if(Players.Contains(mob))
            {
                RemovePlayer(mob);
                if (Players.Count == 1)
                {
                    Mobile lastPlayer = Players[0];
                    RemovePlayer(lastPlayer);
                    lastPlayer.Location = WinnerLocation;
                    EndGameCommand();
                }
            }
        }

        public override bool Decays { get { return false; } }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
        }
        #endregion

        #region timers
        private class StartTimer : Timer
        {
            private readonly BombermanGame m_Game;
            private int m_Count;

            public StartTimer(BombermanGame game)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_Game = game;
                m_Count = 5;
            }

            protected override void OnTick()
            {
                m_Count--;

                if (m_Count >= 1)
                {
                    if (m_Game.Running)
                        m_Game.AnnounceToPlayers(906, "START IN " + m_Count.ToString());
                    else
                        Stop();
                }

                if (m_Count == 0) // Complete
                {
                    m_Game.StartGameTimerFinished();
                    m_Game.AnnounceToPlayers(906, "GO GO GO");
                    Stop();
                }
            }
        }
        #endregion
    }
}
