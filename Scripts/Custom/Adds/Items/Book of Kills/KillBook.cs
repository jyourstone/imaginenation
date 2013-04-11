/*----------------------------------------------------------------------------------------------------
*Script created on 25-July for Runuo
*Origional (1.0) Author plus
*Edited for [2.0] by Z3r0
*
*This kill book works only if it is in your backpack. When you double click it
*is when it truely becomes yours.
*
*Edited for IN:X
*
*-----------------------------------------------------------------------------------------------------
*/

using System;
using System.Collections;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class KillBook : Item
	{	
		
//	************************************ User Configuration **********************************
	
		private static bool m_Pointsys = false; //Set this to false if you aren't using nox's point system.

//	*******************************************************************************************		


		private ArrayList m_Entries;
		private Mobile m_BookOwner;
		private int m_TotKills;
		private int m_TotDeaths;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile BookOwner	{ get{ return m_BookOwner; }set{ m_BookOwner = value; }}
		
		public static bool Pointsys{ get{ return m_Pointsys; } set{ m_Pointsys = value; }}
		
		public int TotKills{ get{ return m_TotKills; }set{ m_TotKills = value; }}		
		
		public int TotDeaths{ get{ return m_TotDeaths; }set{ m_TotDeaths = value;}}
		
		public ArrayList Entries { get{return m_Entries; }}
		
		public override bool DisplayLootType{ get{ return false; } }
		
		[Constructable]
		public KillBook ( ) : base ( 0x2253 )
		{
			Name = "Book of kills";
			Movable = true;
			Hue = 0x4D7;
			m_Entries = new ArrayList();
			LootType = LootType.Blessed;
		}
        public override void OnSingleClick(Mobile from)
        {
            if (m_BookOwner == null)
                LabelTo(from,"Ownerless");
            else
                LabelTo(from,string.Format("{0}'s", m_BookOwner.Name));
            {
            }
           
                base.OnSingleClick(from);
        }


		public override void OnDoubleClick( Mobile from )
		{		
			from.CloseGump( typeof( KillGump  ) );
			from.CloseGump( typeof( KillIndex ) );
			
			if (from.InRange( GetWorldLocation(), 1 ))
			{	
				if (from != null && BookOwner != null )
				{
					from.SendGump( new KillIndex( from, this, m_Entries ) );
				}
				else
				{
					m_BookOwner = from;
					from.SendGump( new KillIndex( from, this, m_Entries ) );
				}
				
				if ( from != m_BookOwner && BookOwner != null)
					from.SendMessage("This is not your book, it will not work for you!");
			}
			else
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.		
		}

		public KillBook ( Serial serial ) : base ( serial )
		{
		}

		public override void Serialize ( GenericWriter writer)
		{
			base.Serialize ( writer );

			writer.Write ( 0);
			
			writer.Write( m_Entries.Count );

			for ( int i = 0; i < m_Entries.Count; ++i )
				((DeathEntry)m_Entries[i]).Serialize( writer );

			writer.Write ( m_TotKills);
			writer.Write ( m_TotDeaths);
			writer.Write ( m_BookOwner);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize ( reader );

			int version = reader.ReadInt();
			
			int count = reader.ReadInt();

			m_Entries = new ArrayList( count );

			for ( int i = 0; i < count; ++i )
				m_Entries.Add( new DeathEntry( reader ) );
			
			m_TotKills = reader.ReadInt();
			m_TotDeaths = reader.ReadInt();
			m_BookOwner = reader.ReadMobile();
		}
		
		public DeathEntry AddEntry( string name, int deaths )
		{			
			foreach( DeathEntry o in m_Entries )

			if (o.Name == name)			
			{
				o.Deaths += deaths;
				return o;
			}

			DeathEntry be = new DeathEntry( name, deaths );
		
			m_Entries.Add( be );
			return be;
		}

		public void AddEntry( DeathEntry be )
		{
			m_Entries.Add( be );			
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			
			string info = String.Format("A kill book with {0} kill{1} recorded", m_Entries.Count, m_Entries.Count != 1 ? "s":"");
			
			list.Add( info );
		}
	}
		
	public class DeathEntry
	{
		private readonly string m_Name;
		private int m_Deaths;
		
		public string Name{ get{ return m_Name; } }
		
		public int Deaths{ get{ return m_Deaths; } set{ m_Deaths = value; }	}
		
		public DeathEntry( string name, int deaths )
		{
			m_Name   = name;
			m_Deaths = deaths;
		}
		
		public void Serialize( GenericWriter writer )
		{
			writer.Write( (byte) 0 ); // version
			
			writer.Write( m_Name );

			writer.Write( m_Deaths );
		}

		public DeathEntry( GenericReader reader )
		{
			int version = reader.ReadByte();

			m_Name = reader.ReadString();
			m_Deaths = reader.ReadInt();
		}
	}
}
