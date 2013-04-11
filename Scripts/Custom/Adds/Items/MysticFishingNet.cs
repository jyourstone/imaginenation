using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;

namespace Server.Items
{
	public class MysticFishingNet : Item
	{
		private bool m_InUse;

        [Constructable]
        public MysticFishingNet()
            : base(0x0DCA)
        {
            Weight = 1.0;
            Hue = 2972;
            Name = "Mystic fishing net";
        }

		public MysticFishingNet( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version

			writer.Write( m_InUse );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_InUse = reader.ReadBool();

					if ( m_InUse )
						Delete();

					break;
				}
			}

			Stackable = false;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( m_InUse )
			{
				from.SendLocalizedMessage( 1010483 ); // Someone is already using that net!
			}
			else if ( IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1010484 ); // Where do you wish to use the net?
				from.BeginTarget( -1, true, TargetFlags.None, OnTarget );
			}
			else
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
		}

		public void OnTarget( Mobile from, object obj )
		{
			if ( Deleted || m_InUse )
				return;

			IPoint3D p3D = obj as IPoint3D;

			if ( p3D == null )
				return;

			Map map = from.Map;

			if ( map == null || map == Map.Internal )
				return;

			int x = p3D.X, y = p3D.Y;

			if ( !from.InRange( p3D, 6 ) )
			{
				from.SendLocalizedMessage( 500976 ); // You need to be closer to the water to fish!
			}
			else if ( FullValidation( map, x, y ) )
			{
				Point3D p = new Point3D( x, y, map.GetAverageZ( x, y ) );

				for ( int i = 1; i < Amount; ++i ) // these were stackable before, doh
					from.AddToBackpack( new SpecialFishingNet() );

				m_InUse = true;
				Movable = false;
				MoveToWorld( p, map );

				from.Animate( 12, 5, 1, true, false, 0 );

				Timer.DelayCall( TimeSpan.FromSeconds( 1.5 ), TimeSpan.FromSeconds( 1.0 ), 20, new TimerStateCallback( DoEffect ), new object[]{ p, 0, from } );

				from.SendLocalizedMessage( 1010487 ); // You plunge the net into the sea...

                new InternalTimer(from).Start();
			}
			else
			{
				from.SendLocalizedMessage( 1010485 ); // You can only use this net in deep water!
			}
		}

        private class InternalTimer : Timer 
        {
            private readonly Mobile m_From;

            public InternalTimer(Mobile from)
                : base(TimeSpan.FromSeconds(2.0))
            {
                m_From = from;
            }

            protected override void OnTick()
            {
                m_From.SendAsciiMessage("A terrifying sound comes from below the waters as you see a huge shadow rise from the depths");
            }
        }

		private void DoEffect( object state )
		{
			if ( Deleted )
				return;

			object[] states = (object[])state;

			Point3D p = (Point3D)states[0];
			int index = (int)states[1];
			Mobile from = (Mobile)states[2];

			states[1] = ++index;

			if ( index == 1 )
			{
				Effects.SendLocationEffect( p, Map, 0x352D, 16, 4 );
				Effects.PlaySound( p, Map, 0x364 );
			}
			else if ( index <= 10 || index == 20 )
			{
				for ( int i = 0; i < 10; ++i )
				{
					int x, y;

					switch ( Utility.Random( 32 ) )
					{
						default:
					        x = -5; y = +5;
					        break; 
                        case 1: x = -5; y = +1;
					        break;
                        case 2: x = -5; y = +6;
					        break;
                        case 3: x = -5; y = +3;
					        break;
                        case 4: x = -6; y = +3;
					        break;
                        case 5: x = -6; y = +1;
					        break;
                        case 6: x = -6; y = +5;
					        break;
                        case 7: x = -6; y = +6;
					        break;
                        case 8: x = -4; y = +5;
					        break;
                        case 9: x = -4; y = +1;
					        break;
                        case 10: x = -4; y = +6;
					        break;
                        case 11: x = -4; y = +3;
					        break;
                        case 12: x = -3; y = +3;
					        break;
                        case 13: x = -3; y = +1;
					        break;
                        case 14: x = -3; y = +5;
					        break;
                        case 15: x = -3; y = +6;
					        break;
                        case 16: x = +2; y = +5;
					        break;
                        case 17: x = +2; y = +1;
					        break;
                        case 18: x = +2; y = +6;
					        break;
                        case 19: x = +2; y = +3;
					        break;
                        case 20: x = -1; y = +3;
					        break;
                        case 21: x = -1; y = +1;
					        break;
                        case 22: x = -1; y = +5;
					        break;
                        case 23: x = -1; y = +6;
					        break;
                        case 24: x = +3; y = +5;
					        break;
                        case 25: x = +3; y = +1;
					        break;
                        case 26: x = +3; y = +6;
					        break;
                        case 27: x = +3; y = +3;
					        break;
                        case 28: x = +1; y = +3;
					        break;
                        case 29: x = +1; y = +1;
					        break;
                        case 30: x = +1; y = +5;
					        break;
                        case 31: x = +1; y = +6;
					        break;
					}

					Effects.SendLocationEffect( new Point3D( p.X + x, p.Y + y, p.Z ), Map, 0x352D, 16, 4 );
                    Effects.PlaySound(p, Map, 896);
				}

				Effects.PlaySound( p, Map, 0x364 );

				if ( index == 20 )
					FinishEffect( p, Map, from );
				else
					Z -= 1;
			}
		}

		protected virtual int GetSpawnCount()
		{
			int count = Utility.RandomMinMax( 4, 5 );

			return count;
		}

		protected static void Spawn( Point3D p, Map map, BaseCreature spawn )
		{
			if ( map == null )
			{
				spawn.Delete();
				return;
			}

			int x = p.X, y = p.Y;

			for ( int j = 0; j < 20; ++j )
			{
				int tx = p.X - 2 + Utility.Random( 5 );
				int ty = p.Y - 2 + Utility.Random( 5 );

				LandTile t = map.Tiles.GetLandTile( tx, ty );

				if ( t.Z == p.Z && ( (t.ID >= 0xA8 && t.ID <= 0xAB) || (t.ID >= 0x136 && t.ID <= 0x137) ) && !SpellHelper.CheckMulti( new Point3D( tx, ty, p.Z ), map ) )
				{
					x = tx;
					y = ty;
					break;
				}
			}

			spawn.MoveToWorld( new Point3D( x, y, p.Z ), map );

			if ( spawn is AncientLich && 0.1 > Utility.RandomDouble() )
				spawn.PackItem( new MysticFishingNet() );
		}

		protected virtual void FinishEffect( Point3D p, Map map, Mobile from )
		{
			from.RevealingAction();

            int spawncount = GetSpawnCount();
		    int z = p.Z;
            //Ghost ship Z, should be 10 lower than the spawn Z
            int gsZ = z - 30; //Set to -30 so it spawns under the sea and then emerges up
            //Spawn Z, needs to be 10 higher than ghost ship Z so they get on top of the boat
		    int spawnZ = z - 20;

            //Create the ghost ship here
            GhostShip gs = new GhostShip();

            //Add treasure
            MetalChest tc = new MetalChest {ItemID = 0xE7C, LiftOverride = true};
            TreasureMapChest.Fill(tc, 5);

            //Now declare an area the same size as the ship, to look for items that might block
            Point2D start = new Point2D( p.X - 10, p.Y - 7 ); //Starting location of the area
            Point2D end = new Point2D(p.X + 10, p.Y + 7); //Ending location of the area
			Rectangle2D rect = new Rectangle2D( start, end ); //Declaring the entire area as a rectangle
            
            //Create a new list that will contain all items in the rectangle
            List<Item> list = new List<Item>();

		    IPooledEnumerable eable = map.GetItemsInBounds(rect); //Get all items in the rectangle

            foreach (Item item in eable) //Add all items in the rectangle to the list
                list.Add(item);

            eable.Free();

            //While an item exists in the rectangle, move the spawnlocation of the boat/monsters
            while (list.Count > 0 )
            {
                if (Utility.RandomDouble() < 0.5 )
                    p.X += 1;
                else
                    p.X -= 1;

                if (Utility.RandomDouble() < 0.5)
                    p.Y += 1;
                else
                    p.Y -= 1;

                start = new Point2D(p.X - 10, p.Y - 7);
                end = new Point2D(p.X + 10, p.Y + 7);
                rect = new Rectangle2D(start, end);

                eable = map.GetItemsInBounds(rect);

                //Clear the list as we need to create a new one with the new location of the ship
                list.Clear();

                foreach (Item item in eable)
                    list.Add(item); //Add the items (if any) in the new spawnlocation to the list

                eable.Free();
            }

            //No items blocking, move the ship to the world
            p.Z = gsZ;
		    gs.MoveToWorld(p, map);

            //Move the treasure chest to the world
            p.Z = spawnZ + 2;
		    p.X -= 9;
            tc.MoveToWorld(p, map);

            //Add the boat and all items inside the boat here
            gs.Itemlist.Add(gs);
            gs.Itemlist.Add(tc);

            //Add as many spawns as spawncount allows
            for (int i = 0; i < spawncount; ++i)
            {
                
                BaseCreature spawn;

                switch (Utility.Random(4))
                {
                    default:
                        spawn = new LichLord();
                        break;
                    case 1:
                        spawn = new AncientLich();
                        break;
                    case 2:
                        spawn = new Lich();
                        break;
                    case 3:
                        spawn = new SkeletalCaptain();
                        break;
                }

                p.Z = spawnZ;
                Spawn(p, map, spawn);

                spawn.Combatant = from;

                //Add the spawn to the list "spawns", needed for the boat decay timer and emerge timer
                gs.Spawnlist.Add(spawn);
            }

            //Start the emerge timer, so the boat doesn't just appear instantly on top of the water
            new EmergeTimer(gs.Itemlist, gs.Spawnlist).Start();

			Delete();
		}

		public static bool FullValidation( Map map, int x, int y )
		{
			bool valid = ValidateDeepWater( map, x, y );

			for ( int j = 1, offset = 5; valid && j <= 5; ++j, offset += 5 )
			{
				if ( !ValidateDeepWater( map, x + offset, y + offset ) )
					valid = false;
				else if ( !ValidateDeepWater( map, x + offset, y - offset ) )
					valid = false;
				else if ( !ValidateDeepWater( map, x - offset, y + offset ) )
					valid = false;
				else if ( !ValidateDeepWater( map, x - offset, y - offset ) )
					valid = false;
			}

			return valid;
		}

		private static readonly int[] m_WaterTiles = new int[]
			{
				0x00A8, 0x00AB,
				0x0136, 0x0137
			};

		private static bool ValidateDeepWater( Map map, int x, int y )
		{
			int tileID = map.Tiles.GetLandTile( x, y ).ID;
			bool water = false;

			for( int i = 0; !water && i < m_WaterTiles.Length; i += 2 )
				water = (tileID >= m_WaterTiles[i] && tileID <= m_WaterTiles[i + 1]);

			return water;
		}

        private class EmergeTimer : Timer
        {
            private readonly List<Item> items;
            private readonly List<Mobile> spawns;
            private int count = 11;

            public EmergeTimer(List<Item> i, List<Mobile> mobs)
                : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(0.8))
            {
                items = i;
                spawns = mobs;
            }

            protected override void OnTick()
            {
                count--;

                if (count >= 1)
                {
                    for (int i = 0; i < items.Count; i++ ) //Get the boat and all items inside and increase their Z
                    {
                        Item item = items[i];
                        item.Z += 2;
                    }

                    for (int i = 0; i < spawns.Count; i++) //Get all spawns in the boat and increase their Z
                    {
                        Mobile m = spawns[i];
                        m.Z += 2;
                    }
                }

            if (count == 0) //Timer complete
                Stop();
            }
        }
	}
}