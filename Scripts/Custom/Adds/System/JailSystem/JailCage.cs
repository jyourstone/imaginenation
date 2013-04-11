using System.Collections.Generic;
using Server.Regions;

namespace Server.Items
{
	public class JailCage : Item
	{
		private List<Item> m_Components;

		private HoldingCell cellblock;

		public JailCage( Mobile from ) : base( 1180 )
		{
			Movable = false;
			MoveToWorld( from.Location, from.Map );
			m_Components = new List<Item>();
			cellblock = new HoldingCell( from.Location.X, from.Location.Y, from.Map );
			AddItem( from, 1, 0, 0, new CagePart( 1180, this ), true ); // right upper
			AddItem( from, 1, 1, 0, new CagePart( 1180, this ), true ); // right lower
			AddItem( from, 0, 1, 0, new CagePart( 1180, this ), true ); // left lower

			AddItem( from, 1, 1, 0, new CagePart( 2082, this ), true ); // right lower
			AddItem( from, 1, 0, 0, new CagePart( 2081, this ), true ); //right center
			AddItem( from, 1, -1, 0, new CagePart( 2083, this ), true ); //right upper
			AddItem( from, -1, 1, 0, new CagePart( 2081, this ), true ); //left lower 
			AddItem( from, -1, 0, 0, new CagePart( 2081, this ), true ); //left center
			//AddItem( from, -1, -1, 0, new cagePart( 2083 ),true );//left upper
			AddItem( from, 0, 1, 0, new CagePart( 2083, this ), true ); //center lower
			AddItem( from, 0, -1, 0, new CagePart( 2083, this ), true ); //center upper
		}

		public JailCage( Serial serial ) : base( serial )
		{
		}

		private void AddItem( Mobile from, int x, int y, int z, Item item, bool vis )
		{
			item.Visible = vis;
			item.MoveToWorld( new Point3D( from.Location.X + x, from.Location.Y + y, from.Location.Z + z ), from.Map );

			m_Components.Add( item );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version

			writer.Write( m_Components.Count );

			for( int i = 0; i < m_Components.Count; ++i )
				writer.Write( m_Components[i] );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch( version )
			{
				default:
				{
					int count = reader.ReadInt();

					m_Components = new List<Item>( count );

					for( int i = 0; i < count; ++i )
					{
						Item item = reader.ReadItem();

						if( item != null )
							m_Components.Add( item );
					}

					break;
				}
			}
			cellblock = new HoldingCell( X, Y, Map );
		}

		public override void OnAfterDelete()
		{
			foreach( Item item in m_Components )
				if( ( item != null ) && ( !item.Deleted ) )
					item.Delete();
			//Region.RemoveRegion(cellblock);
		}

		private class CagePart : Item
		{
			private JailCage m_parent;
			// a tree ornament

			public CagePart( int itemID, JailCage parent ) : base( itemID )
			{
				m_parent = parent;
				Movable = false;
			}

			public CagePart( Serial serial ) : base( serial )
			{
			}

			public override int LabelNumber
			{
				get { return 1016152; }
			}

			public override void OnDelete()
			{
				if( m_parent != null )
					if( !m_parent.Deleted )
						m_parent.Delete();
				base.OnDelete();
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( 1 ); // version
				writer.Write( m_parent );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();
				switch( version )
				{
					case 1:
						m_parent = (JailCage)reader.ReadItem();
						break;
					default:
						break;
				}
			}
		}
	}
}