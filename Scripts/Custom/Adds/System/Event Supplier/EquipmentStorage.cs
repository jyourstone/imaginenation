using System.Collections.Generic;

namespace Server.Items
{
	public class EquipmentStorage : BaseContainer
	{
		private List<LocationStorage> m_BackpackContent = new List<LocationStorage>();
		private List<Item> m_EquippedItems = new List<Item>();
		private Mobile m_Owner;

		[Constructable]
		public EquipmentStorage( Mobile m ) : base( 0x1512 )
		{
			m_Owner = m;
			Name = string.Format( "{0}'s Equipment [Do not trade this item!]", m_Owner.Name );		
        }

		public EquipmentStorage( Serial serial ) : base( serial )
		{

		}

		public override int DefaultMaxWeight
		{
			get { return 0; }
		}

		public override int DefaultMaxItems
		{
			get { return 0; }
		}

		public void StoreEquip()
		{
			if( !m_Owner.Player || m_Owner.HasTrade )
			{
				Delete();
				return;
			}

			if( !m_Owner.Alive )
			{
				m_Owner.SendMessage( "Resurrect yourself before you attempt to autobank." );
				Delete();
				return;
			}

			//Backpack items
			if( m_Owner.Backpack != null )
				for( int i = 0; i < m_Owner.Backpack.Items.Count; ++i )
					m_BackpackContent.Add( new LocationStorage( m_Owner.Backpack.Items[i], m_Owner.Backpack.Items[i].Location ) );

			//Wearables
			foreach( Item item in m_Owner.Items )
				if( item.Layer != Layer.Bank && item.Layer != Layer.Backpack && item.Layer != Layer.Hair && item.Layer != Layer.FacialHair )
					m_EquippedItems.Add( item );

			if( m_BackpackContent.Count + m_EquippedItems.Count > 0 )
			{
				//Drop it to the "container", makes it easier to "save" the items
				for( int i = 0; i < m_BackpackContent.Count; ++i )
					DropItem( m_BackpackContent[i].ItemStored );

				for( int i = 0; i < m_EquippedItems.Count; ++i )
					DropItem( m_EquippedItems[i] );

				if( Items.Count <= 0 )
				{
					m_Owner.SendAsciiMessage( "You have nothing to bank." );
					Delete();
				}
				else
				{
					m_Owner.BankBox.DropItem( this );
					m_Owner.SendAsciiMessage( "Your equipment has been banked. To retrieve it, type bank and use the equipment statue." );
				}
			}
			else
			{
				m_Owner.SendAsciiMessage( "You have nothing to bank." );
				Delete();
			}
		}

		public override void DropItem( Item dropped )
		{
			if( !dropped.EventItem )
				base.DropItem( dropped );
			else
				dropped.Delete();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( from != m_Owner )
			{
				from.SendAsciiMessage( "Only the owner can equip this." );
				return;
			}

			if( from.Mounted )
			{
				from.SendMessage( "Can not equip while being mounted." );
				return;
			}

			if( from.HasTrade )
			{
				from.SendMessage( "Can not equip while having a trade up." );
				return;
			}

			//This is most likley not needed, but anyways.
			if( from.IsInEvent )
			{
				from.IsInEvent = false;
				from.SendAsciiMessage( "You auto supply has been removed." );

				SupplySystem.RemoveEventGear( from );
			}

			for( int i = 0; i < m_EquippedItems.Count; ++i )
			{
				if( !m_EquippedItems[i].IsChildOf( this ) )
					continue;

				if( from.FindItemOnLayer( m_EquippedItems[i].Layer ) != null )
					if( from.Backpack != null )
						from.AddToBackpack( from.FindItemOnLayer( m_EquippedItems[i].Layer ) );
					else if( m_Owner.BankBox != null )
						from.BankBox.DropItem( from.FindItemOnLayer( m_EquippedItems[i].Layer ) );

				from.EquipItem( m_EquippedItems[i] );
			}

			for( int i = 0; i < m_BackpackContent.Count; ++i )
			{
                if (m_BackpackContent[i].ItemStored != null)
                {
                    if (!m_BackpackContent[i].ItemStored.IsChildOf(this))
                        continue;

                    if (from.Backpack != null)
                        AddToPack(m_BackpackContent[i]);
                    else if (from.BankBox != null)
                        from.BankBox.DropItem(m_BackpackContent[i].ItemStored);
                }
		    }

			if( Items.Count < 1 )
				Delete();
		}

		public override bool DropToWorld( Mobile from, Point3D p )
		{
			return false;
		}

		public override bool DropToMobile( Mobile from, Mobile target, Point3D p )
		{
			return false;
		}

		private void AddToPack( LocationStorage ls )
		{
			if( m_Owner.Backpack != null )
			{
				m_Owner.Backpack.DropItem( ls.ItemStored );
				ls.ItemStored.Location = ls.Location;
			}
			else if( m_Owner.Backpack != null )
				m_Owner.AddToBackpack( ls.ItemStored );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); //version

			writer.Write( m_Owner );
			writer.WriteItemList( m_EquippedItems );

			writer.Write( m_BackpackContent.Count );
			for( int i = 0; i < m_BackpackContent.Count; ++i )
				m_BackpackContent[i].Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			m_Owner = reader.ReadMobile();
			m_EquippedItems = reader.ReadStrongItemList();

			m_BackpackContent = new List<LocationStorage>();
			int BackpackContentAmount = reader.ReadInt();

			for( int i = 0; i < BackpackContentAmount; ++i )
				m_BackpackContent.Add( LocationStorage.Deserialize( reader ) );
		}

		public static void PackItem( Mobile m, Item item )
		{
			if( !m.PlaceInBackpack( item ) )
				item.Delete();
		}

		#region Nested type: LocationStorage
		private class LocationStorage
		{
			private readonly Item m_Item;
			private readonly Point3D m_Location;

			public LocationStorage( Item item, Point3D location )
			{
				m_Item = item;
				m_Location = location;
			}

			public Item ItemStored
			{
				get { return m_Item; }
			}

			public Point3D Location
			{
				get { return m_Location; }
			}

			public void Serialize( GenericWriter writer )
			{
				writer.Write( m_Item );
				writer.Write( m_Location );
			}

			public static LocationStorage Deserialize( GenericReader reader )
			{
				return new LocationStorage( reader.ReadItem(), reader.ReadPoint3D() );
			}
		}
		#endregion
	}
}