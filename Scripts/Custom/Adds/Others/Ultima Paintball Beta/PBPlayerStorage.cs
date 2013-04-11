/*********************************************************************************/
/*                                                                               */
/*                              Ultima Paintball 						         */
/*                        Created by Aj9251 (Disturbed)                          */         
/*                                                                               */
/*                                 Credits:                                      */
/*                   Original Idea + Some Code - A_Li_N                          */
/*                   Some Ideas + Code - Aj9251 (Disturbed)                      */
/*********************************************************************************/

using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Games.PaintBall
{
	public class PBPlayerStorage : Item
	{
		private Mobile m_Owner;
		
	//	private Item temp;

		private int Str, Dex, Int, Fame, Karma, Kills;
		private string PlayerName;
		private ArrayList StoredSkills;
	/*	private ArrayList PackItems;
		private ArrayList LayerItems; */

		[Constructable]
		public PBPlayerStorage( Mobile from ) : base ( 4626 )
		{
			m_Owner = from;

			Movable = false;
			Name = m_Owner.Name + "'s Storage Chip";
			Hue = 1152;
			
			PlayerName = m_Owner.Name;

			Str = m_Owner.Str;
			Dex = m_Owner.Dex;
			Int = m_Owner.Int;
			Fame = m_Owner.Fame;
			Karma = m_Owner.Karma;
			Kills = m_Owner.Kills;
			
	/*		PackItems = new ArrayList();
			LayerItems = new ArrayList();
			
			foreach( Item litem in m_Owner.Items )
			{
				if( litem.Layer != Layer.Bank && litem.Layer != Layer.Hair && litem.Layer != Layer.FacialHair && litem.Layer != Layer.Mount && litem.Layer != Layer.Backpack )
				{
					temp = (Item)Activator.CreateInstance( litem.GetType( ));
					LayerItems.Add( temp );
					
					litem.Delete();
				}
			}
			foreach( Item pitem in m_Owner.Backpack.Items )
			{
				temp = (Item)Activator.CreateInstance( pitem.GetType( ));
					PackItems.Add( temp );
					pitem.Delete();
					
			}
*/
			StoredSkills = new ArrayList();
			for( int i = 0; i < PowerScroll.Skills.Count; ++i )
				StoredSkills.Add( (double)m_Owner.Skills[PowerScroll.Skills[i]].Base );
		}

		public override void OnDoubleClick( Mobile from )
		{
			Restore();
			Delete();
		}
		public void Use( Mobile mob )
		{
			AutoRestore(mob);
			Delete();
		}

		public void Restore()
		{
			m_Owner.Str = Str;
			m_Owner.Dex = Dex;
			m_Owner.Int = Int;
			m_Owner.Fame = Fame;
			m_Owner.Karma = Karma;
			m_Owner.Kills = Kills;
			m_Owner.Name = PlayerName;
			

			for( int i = 0; i < StoredSkills.Count; i++ )
				m_Owner.Skills[PowerScroll.Skills[i]].Base = (double)StoredSkills[i];
		}
		public void AutoRestore( Mobile ownermob )
		{
			ownermob.Str = Str;
			ownermob.Dex = Dex;
			ownermob.Int = Int;
			ownermob.Fame = Fame;
			ownermob.Karma = Karma;
			ownermob.Kills = Kills;
			ownermob.Name = PlayerName;
			
	/*		Item toGive;
			
			for( int i = 0; i < PackItems.Count; i++ )
			{
				//toGive = (Item)Activator.CreateInstance( PackItems[i].GetType() );
				toGive = ((Item)PackItems[i]);
				toGive.Amount = ((Item)PackItems[i]).Amount;
				ownermob.AddToBackpack( toGive );
				
			}
			for( int i = 0; i < LayerItems.Count; i++ )
			{
				//toGive = (Item)Activator.CreateInstance( LayerItems[i].GetType() );
				toGive = ((Item)LayerItems[i]);
				toGive.Amount = ((Item)LayerItems[i]).Amount;
				ownermob.AddItem( toGive );
			}
			*/

			for( int i = 0; i < StoredSkills.Count; i++ )
				ownermob.Skills[PowerScroll.Skills[i]].Base = (double)StoredSkills[i];
		}

		public PBPlayerStorage( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); //version

			writer.Write( m_Owner );

			writer.Write( (int)Str );
			writer.Write( (int)Dex );
			writer.Write( (int)Int );
			writer.Write( (int)Fame );
			writer.Write( (int)Karma );
			writer.Write( (int)Kills );
			writer.Write( (string)PlayerName );
			
		/*	writer.WriteItemList( PackItems );
			writer.WriteItemList( LayerItems ); */

			writer.Write( (int)StoredSkills.Count );
			for( int i = 0; i < StoredSkills.Count; i++ )
				writer.Write( (double)StoredSkills[i] );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			StoredSkills = new ArrayList();

			m_Owner = reader.ReadMobile();

			Str = reader.ReadInt();
			Dex = reader.ReadInt();
			Int = reader.ReadInt();
			Fame = reader.ReadInt();
			Karma = reader.ReadInt();
			Kills = reader.ReadInt();
			PlayerName = reader.ReadString();
			
	/*		PackItems = reader.ReadItemList();
			LayerItems = reader.ReadItemList(); */

			int Count = reader.ReadInt();
			for( int i = 0; i < Count; i++ )
				StoredSkills.Add( reader.ReadDouble() );
		}
	}
}
