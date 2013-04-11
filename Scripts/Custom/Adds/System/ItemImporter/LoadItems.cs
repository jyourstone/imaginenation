using System;
using System.Data;
using System.IO;
using Server.Commands;

namespace Server
{
	public class XmlToItems
	{
		public static void Initialize()
		{
			CommandSystem.Register( "loaditems", AccessLevel.Administrator, LoadItems_OnCommand );
		}

		[Usage( "LoadItems" )]
		[Description( "Loads all items from an xmlfile." )]
		public static void LoadItems_OnCommand(CommandEventArgs e)
		{
			LoadItems();
		}

		public static void LoadItems()
		{
			FileStream fs = null;

			try
			{
				fs = File.Open( "items.xml", FileMode.Open, FileAccess.Read );
			}
			catch
			{
				Console.WriteLine( "Could not open file" );
			}

			if( fs == null )
			{
				World.Broadcast( 0x35, false, "Loading failed." );
				return;
			}

			DataSet ds = new DataSet( "static");

			try
			{
				ds.ReadXml( fs );
			}
			catch
			{
				World.Broadcast( 0x35, false, "Error with xml" );
				return;
			}
			finally
			{
				fs.Close();
			}

			if( ds.Tables != null && ds.Tables.Count > 0 )
			{
				World.Broadcast( 0x35, false, "Table successfully populated." );

				if( ds.Tables["item"] != null && ds.Tables["item"].Rows.Count > 0 )
				{
					foreach( DataRow dr in ds.Tables["item"].Rows )
					{
						string temp = string.Empty;
						Type type = null;
						try
						{
							type = ScriptCompiler.FindTypeByFullName( dr["Type"].ToString(), false );
						}
						catch
						{
							Console.WriteLine("Type loading failed");
						}

						if( type == null )
							continue;

						Item item;
						try
						{
							item = Activator.CreateInstance( type ) as Item;
						}
						catch
						{
							Console.WriteLine("Type creation failed.");
							continue;
						}

						if( item == null )
							continue;

						try
						{
							item.ItemID = int.Parse( dr["ItemID"].ToString() );
						}
						catch
						{
							Console.WriteLine( "ItemID loading failed" );
						}

						try
						{
							item.Amount = int.Parse( dr["Amount"].ToString() );
						}
						catch
						{
							Console.WriteLine( "Amount loading failed" );
							item.Amount = 1;
						}

						try
						{
							temp = dr["Movable"].ToString();
							switch( temp )
							{
								case "True":
									item.Movable = true;
									break;
								case "False":
									item.Movable = false;
									break;
							}
						}
						catch
						{
							Console.WriteLine( "Movable loading failed" );
							item.Movable = false;
						}

						try
						{
							temp = dr["Direction"].ToString();

							switch( temp )
							{
								case "North":
									item.Direction = Direction.North;
									break;
								case "Right":
									item.Direction = Direction.Right;
									break;
								case "East":
									item.Direction = Direction.East;
									break;
								case "Down":
									item.Direction = Direction.Down;
									break;
								case "South":
									item.Direction = Direction.South;
									break;
								case "Left":
									item.Direction = Direction.Left;
									break;
								case "West":
									item.Direction = Direction.West;
									break;
								case "Up":
									item.Direction = Direction.Up;
									break;
							}
						}
						catch
						{
							Console.WriteLine( "Direction loading failed" );
						}

						try
						{
							item.Hue = int.Parse( dr["Hue"].ToString() );
						}
						catch
						{
							Console.WriteLine( "Hue loading failed" );
						}

						try
						{
							temp = dr["Layer"].ToString();
							switch( temp )
							{
								case "Invalid":
									item.Layer = Layer.Invalid;
									break;
								case "FirstValid":
									item.Layer = Layer.FirstValid;
									break;
								case "OneHanded":
									item.Layer = Layer.OneHanded;
									break;
								case "TwoHanded":
									item.Layer = Layer.TwoHanded;
									break;
								case "Shoes":
									item.Layer = Layer.Shoes;
									break;
								case "Pants":
									item.Layer = Layer.Pants;
									break;
								case "Shirt":
									item.Layer = Layer.Shirt;
									break;
								case "Helm":
									item.Layer = Layer.Helm;
									break;
								case "Gloves":
									item.Layer = Layer.Gloves;
									break;
								case "Ring":
									item.Layer = Layer.Ring;
									break;
								case "Talisman":
									item.Layer = Layer.Talisman;
									break;
								case "Neck":
									item.Layer = Layer.Neck;
									break;
								case "Hair":
									item.Layer = Layer.Hair;
									break;
								case "Waist":
									item.Layer = Layer.Waist;
									break;
								case "InnerTorso":
									item.Layer = Layer.InnerTorso;
									break;
								case "Bracelet":
									item.Layer = Layer.Bracelet;
									break;
								case "Unused_xF":
									item.Layer = Layer.Unused_xF;
									break;
								case "FacialHair":
									item.Layer = Layer.FacialHair;
									break;
								case "MiddleTorso":
									item.Layer = Layer.MiddleTorso;
									break;
								case "Earrings":
									item.Layer = Layer.Earrings;
									break;
								case "Arms":
									item.Layer = Layer.Arms;
									break;
								case "Cloak":
									item.Layer = Layer.Cloak;
									break;
								case "Backpack":
									item.Layer = Layer.Backpack;
									break;
								case "OuterTorso":
									item.Layer = Layer.OuterTorso;
									break;
								case "OuterLegs":
									item.Layer = Layer.OuterLegs;
									break;
								case "InnerLegs":
									item.Layer = Layer.InnerLegs;
									break;
								case "LastUserValid":
									item.Layer = Layer.LastUserValid;
									break;
								case "Mount":
									item.Layer = Layer.Mount;
									break;
								case "ShopBuy":
									item.Layer = Layer.ShopBuy;
									break;
								case "ShopResale":
									item.Layer = Layer.ShopResale;
									break;
								case "ShopSell":
									item.Layer = Layer.ShopSell;
									break;
								case "Bank":
									item.Layer = Layer.Bank;
									break;
								case "LastValid":
									item.Layer = Layer.LastValid;
									break;
							}
						}
						catch
						{
							Console.WriteLine( "Layer loading failed" );
						}

						try
						{
							temp = dr["Light"].ToString();

							switch( temp )
							{
								case "ArchedWindowEast":
									item.Light = LightType.ArchedWindowEast;
									break;
								case "Circle225":
									item.Light = LightType.Circle225;
									break;
								case "Circle150":
									item.Light = LightType.Circle150;
									break;
								case "DoorSouth":
									item.Light = LightType.DoorSouth;
									break;
								case "DoorEast":
									item.Light = LightType.DoorEast;
									break;
								case "NorthBig":
									item.Light = LightType.NorthBig;
									break;
								case "NorthEastBig":
									item.Light = LightType.NorthEastBig;
									break;
								case "EastBig":
									item.Light = LightType.EastBig;
									break;
								case "WestBig":
									item.Light = LightType.WestBig;
									break;
								case "SouthWestBig":
									item.Light = LightType.SouthWestBig;
									break;
								case "SouthBig":
									item.Light = LightType.SouthBig;
									break;
								case "NorthSmall":
									item.Light = LightType.NorthSmall;
									break;
								case "NorthEastSmall":
									item.Light = LightType.NorthEastSmall;
									break;
								case "EastSmall":
									item.Light = LightType.EastSmall;
									break;
								case "WestSmall":
									item.Light = LightType.WestSmall;
									break;
								case "SouthSmall":
									item.Light = LightType.SouthSmall;
									break;
								case "DecorationNorth":
									item.Light = LightType.DecorationNorth;
									break;
								case "DecorationNorthEast":
									item.Light = LightType.DecorationNorthEast;
									break;
								case "EastTiny":
									item.Light = LightType.EastTiny;
									break;
								case "DecorationWest":
									item.Light = LightType.DecorationWest;
									break;
								case "DecorationSouthWest":
									item.Light = LightType.DecorationSouthWest;
									break;
								case "SouthTiny":
									item.Light = LightType.SouthTiny;
									break;
								case "RectWindowSouthNoRay":
									item.Light = LightType.RectWindowSouthNoRay;
									break;
								case "RectWindowEastNoRay":
									item.Light = LightType.RectWindowEastNoRay;
									break;
								case "RectWindowSouth":
									item.Light = LightType.RectWindowSouth;
									break;
								case "RectWindowEast":
									item.Light = LightType.RectWindowEast;
									break;
								case "ArchedWindowSouthNoRay":
									item.Light = LightType.ArchedWindowSouthNoRay;
									break;
								case "ArchedWindowEastNoRay":
									item.Light = LightType.ArchedWindowEastNoRay;
									break;
								case "ArchedWindowSouth":
									item.Light = LightType.ArchedWindowSouth;
									break;
								case "Cirle300":
									item.Light = LightType.Circle300;
									break;
								case "NorthWestBig":
									item.Light = LightType.NorthWestBig;
									break;
								case "DarkSouthEast":
									item.Light = LightType.DarkSouthEast;
									break;
								case "DarkSouth":
									item.Light = LightType.DarkSouth;
									break;
								case "DarkNorthWest":
									item.Light = LightType.DarkNorthWest;
									break;
								case "DarkSouthEast2":
									item.Light = LightType.DarkSouthEast2;
									break;
								case "DarkEast":
									item.Light = LightType.DarkEast;
									break;
								case "DarkCircle300":
									item.Light = LightType.DarkCircle300;
									break;
								case "DoorOpenSouth":
									item.Light = LightType.DoorOpenSouth;
									break;
								case "DoorOpenEast":
									item.Light = LightType.DoorOpenEast;
									break;
								case "SquareWindowEast":
									item.Light = LightType.SquareWindowEast;
									break;
								case "SquareWindowEastNoRay":
									item.Light = LightType.SquareWindowEastNoRay;
									break;
								case "SquareWindowSouth":
									item.Light = LightType.SquareWindowSouth;
									break;
								case "SquareWindowSouthNoRay":
									item.Light = LightType.SquareWindowSouthNoRay;
									break;
								case "Empty":
									item.Light = LightType.Empty;
									break;
								case "SkinnyWindowSouthNoRay":
									item.Light = LightType.SkinnyWindowSouthNoRay;
									break;
								case "SkinnyWindowEast":
									item.Light = LightType.SkinnyWindowEast;
									break;
								case "SkinnyWindowEastNoRay":
									item.Light = LightType.SkinnyWindowEastNoRay;
									break;
								case "HoleSouth":
									item.Light = LightType.HoleSouth;
									break;
								case "HoleEast":
									item.Light = LightType.HoleEast;
									break;
								case "Moongate":
									item.Light = LightType.Moongate;
									break;
								case "Strips":
									item.Light = LightType.Strips;
									break;
								case "SmallHoleSouth":
									item.Light = LightType.SmallHoleSouth;
									break;
								case "SmallHoleEast":
									item.Light = LightType.SmallHoleEast;
									break;
								case "NorthBig2":
									item.Light = LightType.NorthBig2;
									break;
								case "WestBig2":
									item.Light = LightType.WestBig2;
									break;
								case "NorthWestBig2":
									item.Light = LightType.NorthWestBig2;
									break;
							}
						}
						catch
						{
							Console.WriteLine( "Light loading failed" );
						}

						try
						{
							temp = dr["LootType"].ToString();
							switch( temp )
							{
								case "Regular":
									item.LootType = LootType.Regular;
									break;
								case "Newbied":
									item.LootType = LootType.Newbied;
									break;
								case "Blessed":
									item.LootType = LootType.Blessed;
									break;
								case "Cursed":
									item.LootType = LootType.Cursed;
									break;
							}
						}
						catch
						{
							Console.WriteLine( "LootType loading failed" );
						}

						try
						{
							item.Name = dr["Name"].ToString();
						}
						catch
						{
							Console.WriteLine( "Name loading failed" );
						}

						try
						{
							temp = dr["Stackable"].ToString();
							switch( temp )
							{
								case "True":
									item.Stackable = true;
									break;
								case "False":
									item.Stackable = false;
									break;
							}
						}
						catch
						{
							Console.WriteLine( "Stackable loading failed" );
						}

						try
						{
							item.Weight = double.Parse( dr["Weight"].ToString() );
						}
						catch
						{
							Console.WriteLine( "Weight loading failed" );
						}

						try
						{
							temp = dr["Visible"].ToString();
							switch( temp )
							{
								case "True":
									item.Visible = true;
									break;
								case "False":
									item.Visible = false;
									break;
							}
						}
						catch
						{
							Console.WriteLine( "Visible loading failed" );
						}

						try
						{
							item.X = int.Parse( dr["X"].ToString() );
						}
						catch
						{
							Console.WriteLine( "X loading failed" );
						}

						try
						{
							item.Y = int.Parse( dr["Y"].ToString() );
						}
						catch
						{
							Console.WriteLine( "Y loading failed" );
						}

						try
						{
							item.Z = int.Parse( dr["Z"].ToString() );
						}
						catch
						{
							Console.WriteLine( "Z loading failed" );
						}

						try
						{
							temp = dr["Map"].ToString();

							switch( temp )
							{
								case "Felucca":
									item.Map = Map.Felucca;
									break;
								case "Trammel":
									item.Map = Map.Trammel;
									break;
								case "Ilshenar":
									item.Map = Map.Ilshenar;
									break;
								case "Malas":
									item.Map = Map.Malas;
									break;
								case "Tokuno":
									item.Map = Map.Tokuno;
									break;
								case "Internal":
									item.Map = Map.Internal;
									break;
							}
						}
						catch
						{
							Console.WriteLine( "Map loading failed" );
						}
					}
					World.Broadcast( 0x35, false, "Items should now populate the shard...");
				}
			}
		}
	}
}