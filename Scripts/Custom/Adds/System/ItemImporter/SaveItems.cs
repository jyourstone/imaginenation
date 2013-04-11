using System;
using System.Collections;
using System.Data;
using System.IO;
using Server.Commands;

namespace Server
{
	public class ItemsToXml
	{
		public static void Initialize()
		{
			CommandSystem.Register( "saveitems", AccessLevel.Administrator, SaveItems_OnCommand );
		}

		[Usage( "SaveItems" )]
		[Description( "Saves all items into an xmlfile." )]
		public static void SaveItems_OnCommand(CommandEventArgs e)
		{
			ArrayList items = new ArrayList();

			foreach( Item i in World.Items.Values )
				items.Add( i );

			SaveItems( items );
		}

		public static void SaveItems( ArrayList list )
		{
			DataSet ds = new DataSet( "static" );

			ds.Tables.Add( "item" );

			ds.Tables["item"].Columns.Add( "Type" );
			ds.Tables["item"].Columns.Add( "Amount" );
			ds.Tables["item"].Columns.Add( "Decays" );
			ds.Tables["item"].Columns.Add( "DecayTime" );
			ds.Tables["item"].Columns.Add( "Direction" );
			ds.Tables["item"].Columns.Add( "Hue" );
			ds.Tables["item"].Columns.Add( "ItemID" );
			ds.Tables["item"].Columns.Add( "Layer" );
			ds.Tables["item"].Columns.Add( "Light" );
			ds.Tables["item"].Columns.Add( "Location" );
			ds.Tables["item"].Columns.Add( "LootType" );
			ds.Tables["item"].Columns.Add( "Map" );
			ds.Tables["item"].Columns.Add( "Movable" );
			ds.Tables["item"].Columns.Add( "Name" );
			ds.Tables["item"].Columns.Add( "PileWeight" );
			ds.Tables["item"].Columns.Add( "Serial" );
			ds.Tables["item"].Columns.Add( "Stackable" );
			ds.Tables["item"].Columns.Add( "TotalGold" );
			ds.Tables["item"].Columns.Add( "TotalItems" );
			ds.Tables["item"].Columns.Add( "TotalWeight" );
			ds.Tables["item"].Columns.Add( "Visible" );
			ds.Tables["item"].Columns.Add( "Weight" );
			ds.Tables["item"].Columns.Add( "X" );
			ds.Tables["item"].Columns.Add( "Y" );
			ds.Tables["item"].Columns.Add( "Z" );

			foreach( Item i in list )
			{
				if( i == null || i.Map == null || i.Deleted )
					continue;

				DataRow dr = ds.Tables["item"].NewRow();

				dr["Type"] = i.GetType().ToString();
				dr["Amount"] = i.Amount.ToString();
				dr["Decays"] = i.Decays.ToString();
				dr["DecayTime"] = i.DecayTime.Seconds.ToString();
				dr["Direction"] = i.Direction.ToString();
				dr["Hue"] = i.Hue.ToString();
				dr["ItemID"] = i.ItemID.ToString();
				dr["Layer"] = i.Layer.ToString();
				dr["Light"] = i.Light.ToString();
				dr["Location"] = i.Location.ToString();
				dr["LootType"] = i.LootType.ToString();
				dr["Map"] = i.Map.ToString();
				dr["Movable"] = i.Movable.ToString();
				if( i.Name != null )
					dr["Name"] = i.Name;
				else
					dr["Name"] = "~null~";
				dr["PileWeight"] = i.PileWeight.ToString();
				dr["Serial"] = i.Serial.ToString();
				dr["Stackable"] = i.Stackable.ToString();
				dr["TotalGold"] = i.TotalGold.ToString();
				dr["TotalItems"] = i.TotalItems.ToString();
				dr["TotalWeight"] = i.TotalWeight.ToString();
				dr["Visible"] = i.Visible.ToString();
				dr["Weight"] = i.Weight.ToString();
				dr["X"] = i.X.ToString();
				dr["Y"] = i.Y.ToString();
				dr["Z"] = i.Z.ToString();

				ds.Tables["item"].Rows.Add( dr );
			}

			bool ok;
			try
			{
				FileStream fs = new FileStream( "items.xml", FileMode.Create );
				ds.WriteXml( fs );
				fs.Close();
				ok = true;
			}
			catch( Exception e )
			{
				Console.WriteLine( e );
				ok = false;
			}

			if( ok )
				World.Broadcast( 0x35, false, "Items saved" );
			else
				World.Broadcast( 0x35, false, "Saving failed." );
		}
	}
}