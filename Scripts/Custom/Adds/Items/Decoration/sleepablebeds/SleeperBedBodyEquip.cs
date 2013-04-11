// Script Package: Sleepable Beds
// Version: 1.0
// Author: Oak
// Servers: RunUO 2.0
// Date: 7/7/2006
// History: 
//  Written for RunUO 1.0 shard, Sylvan Dreams,  in February 2005. Based largely on work by David on his Sleepable NPCs scripts.
//  Modified for RunUO 2.0, removed shard specific customizations (wing layers, etc.)
using System;
using System.IO;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Network
{
	public sealed class SleeperBedBodyEquip : Packet
	{
		public SleeperBedBodyEquip( Mobile beholder, SleeperBedBody beheld ) : base( 0x89 )
		{
			ArrayList list = beheld.EquipItems;

			EnsureCapacity( 8 + (list.Count * 5) );

			m_Stream.Write( (int) beheld.Serial );

			for ( int i = 0; i < list.Count; ++i )
			{
				Item item = (Item)list[i];

				if ( !item.Deleted && beholder.CanSee( item ) && item.Parent == beheld )
				{
					m_Stream.Write( (byte) (item.Layer + 1) );
					m_Stream.Write( (int) item.Serial );
				}
			}

			m_Stream.Write( (byte) Layer.Invalid );
		}
	}
	public sealed class SleeperBedBodyContent : Packet
	{
		public SleeperBedBodyContent( Mobile beholder, SleeperBedBody beheld ) : base( 0x3C )
		{
			ArrayList items = beheld.EquipItems;
			int count = items.Count;

			EnsureCapacity( 5 + (count * 19) );

			long pos = m_Stream.Position;

			int written = 0;

			m_Stream.Write( (ushort) 0 );

			for ( int i = 0; i < count; ++i )
			{
				Item child = (Item)items[i];

				if ( !child.Deleted && child.Parent == beheld && beholder.CanSee( child ) )
				{
					m_Stream.Write( (int) child.Serial );
					m_Stream.Write( (ushort) child.ItemID );
					m_Stream.Write( (byte) 0 ); // signed, itemID offset
					m_Stream.Write( (ushort) child.Amount );
					m_Stream.Write( (short) child.X );
					m_Stream.Write( (short) child.Y );
					m_Stream.Write( (int) beheld.Serial );
					m_Stream.Write( (ushort) child.Hue );

					++written;
				}
			}

			m_Stream.Seek( pos, SeekOrigin.Begin );
			m_Stream.Write( (ushort) written );
		}
	}
}
