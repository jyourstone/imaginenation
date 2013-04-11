using Server.Items;

namespace Server.Custom.Games
{
    class BombermanKickBombPlacer : BombermanBombPlacer
    {
        private int range = 3;

        [Constructable()]
        public BombermanKickBombPlacer(Mobile owner, BombermanGame game)
            : base(owner, game)
        {
            this.ItemID = 7885;
            Name = "Kickbombplacer";
        }
		
		public BombermanKickBombPlacer( Serial serial ) : base( serial )
		{
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public int Range
        {
            set
            {
                range = value;
            }
            get
            {
                return range;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.CanBeDamaged())
            {
                from.SendMessage("You cannot place bombs in this area");
                return;
            }
            if (BombsInWorld < MaxBombs)
            {
                bool cont;
                byte inc = 0;
                Map map = from.Map;
                Direction direction = from.Direction;
                int xinc = 0;
                int yinc = 0;

                while (true)
                {
                    cont = false;
                    if (++inc > range)
                    {
                        from.SendMessage("You cannot kick bombs that far");
                        return;
                    }
                    if (direction == Direction.North || direction == Direction.Running)
                    {
                        yinc -= 1;
                    }
                    else if (direction == Direction.East || direction.ToString() == "130")
                    {
                        xinc += 1;
                    }
                    else if (direction == Direction.South || direction.ToString() == "132")
                    {
                        yinc += 1;
                    }
                    else if (direction == Direction.West || direction.ToString() == "134")
                    {
                        xinc -= 1;
                    }

                    int x = from.X + xinc;
                    int y = from.Y + yinc;

                    int z = map.Tiles.GetLandTile(x, y).Z;

                    Point3D location = new Point3D(x, y, z);

                    IPooledEnumerable objects = map.GetObjectsInRange(location, 0);
                    foreach (object o in objects)
                    {
                        if (o is LOSBlocker)
                        {
                            from.SendMessage("You cannot kick a bomb to a place you cannot see.");
                            return;
                        }
                        if (o is Mobile)
                        {
                            from.SendMessage("You cannot kick a bomb on someone else.");
                            return;
                        }
                        if (o is BombermanStone)
                        {
                            cont = true;
                        }
                    }

                    if (!cont)
                    {
                        BombermanBomb bomb = new BombermanBomb(this, m_Game);
                        bomb.MoveToWorld(location, from.Map);
                        ++BombsInWorld;
                        break;
                    }
                }
            }
            else
            {
                from.SendMessage("You cannot place anymore bombs.");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            writer.Write((int)0); // version;

            //0
            writer.Write(range);

            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();
            switch (version)
            {
                case 0:
                    reader.ReadInt();
                    break;
            }
            base.Deserialize(reader);
        }
    }
}
