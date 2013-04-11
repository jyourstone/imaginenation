using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Engines.Craft;
using Server.Gumps;
using Server.Multis;
using Server.Network;
using Solaris.CliLocHandler;

namespace Server.Items
{
	public class Runebook : Item, ISecurable, ICraftable
	{
		public static readonly TimeSpan UseDelay = TimeSpan.FromSeconds( 7.0 );

        private BookQuality m_Quality;

        [CommandProperty(AccessLevel.GameMaster)]
        public BookQuality Quality
        {
            get { return m_Quality; }
            set { m_Quality = value; InvalidateProperties(); }
        }

        private List<RunebookEntry> m_Entries;
		private string m_Description;
		private int m_CurCharges, m_MaxCharges;
		private int m_DefaultIndex;
		private SecureLevel m_Level;
		private Mobile m_Crafter;
	    private readonly Dictionary<Mobile, Point3D> m_Dictionary = new Dictionary<Mobile, Point3D>();
        private Point3D m_LastUsedLocation;

	    [CommandProperty(AccessLevel.GameMaster)]
	    public DateTime NextUse { get; set; }

	    [CommandProperty( AccessLevel.GameMaster )]
		public Mobile Crafter
		{
			get{ return m_Crafter; }
			set{ m_Crafter = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SecureLevel Level
		{
			get{ return m_Level; }
			set{ m_Level = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Description
		{
			get { return m_Description; }
			set {
				m_Description = value;
				InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int CurCharges
		{
			get { return m_CurCharges; }
			set { m_CurCharges = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxCharges
		{
			get { return m_MaxCharges; }
			set { m_MaxCharges = value; }
		}

        public Point3D LastUsedLocation
        {
            get { return m_LastUsedLocation; }
        }

        public Dictionary<Mobile, Point3D> Dictionary
        {
            get { return m_Dictionary; }
        }

		public override int LabelNumber{ get{ return 1041267; } } // runebook

		[Constructable]
		public Runebook( int maxCharges ) : base( Core.AOS ? 0x22C5 : 0xEFA )
		{
			Weight = (Core.SE ? 1.0 : 3.0);
			LootType = LootType.Regular;
			Hue = 0x461;

			Layer = (Core.AOS ? Layer.Invalid : Layer.OneHanded);

            m_Entries = new List<RunebookEntry>();

			m_MaxCharges = maxCharges;

			m_DefaultIndex = -1;

			m_Level = SecureLevel.CoOwners;
		}

		[Constructable]
		public Runebook() : this( Core.SE ? 12 : 6 )
		{
		}

        public List<RunebookEntry> Entries
		{
			get
			{
				return m_Entries;
			}
		}

		public RunebookEntry Default
		{
			get
			{
				if ( m_DefaultIndex >= 0 && m_DefaultIndex < m_Entries.Count )
                    return m_Entries[m_DefaultIndex];

				return null;
			}
			set
			{
				if ( value == null )
					m_DefaultIndex = -1;
				else
					m_DefaultIndex = m_Entries.IndexOf( value );
			}
		}

		public Runebook( Serial serial ) : base( serial )
		{
		}

		public override bool AllowEquipedCast( Mobile from )
		{
			return true;
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );
			SetSecureLevelEntry.AddTo( from, this, list );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 4 );

            //version 4
            writer.Write((byte)m_Quality);	

            //Version 3
            writer.Write( m_LastUsedLocation );

            //Version 2
			writer.Write( m_Crafter );

            //Version 1
			writer.Write( (int) m_Level );

            //Version 0
			writer.Write( m_Entries.Count );

			for ( int i = 0; i < m_Entries.Count; ++i )
                m_Entries[i].Serialize(writer);

			writer.Write( m_Description );
			writer.Write( m_CurCharges );
			writer.Write( m_MaxCharges );
			writer.Write( m_DefaultIndex );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			LootType = LootType.Regular;

			if( Core.SE && Weight == 3.0 )
				Weight = 1.0;

			int version = reader.ReadInt();

			switch ( version )
			{
                case 4:
                    {
                        m_Quality = (BookQuality)reader.ReadByte();
                        goto case 3;
                    }
                case 3:
                    {
                        m_LastUsedLocation = reader.ReadPoint3D();
                        goto case 2;
                    }
				case 2:
				    {
					    m_Crafter = reader.ReadMobile();
					    goto case 1;
				    }
				case 1:
				    {
					    m_Level = (SecureLevel)reader.ReadInt();
					    goto case 0;
				    }
				    case 0:
				    {
					    int count = reader.ReadInt();

                        m_Entries = new List<RunebookEntry>(count);

	    				for ( int i = 0; i < count; ++i )
		    				m_Entries.Add( new RunebookEntry( reader ) );

			    		m_Description = reader.ReadString();
				    	m_CurCharges = reader.ReadInt();
					    m_MaxCharges = reader.ReadInt();
					    m_DefaultIndex = reader.ReadInt();

					    break;
				    }
            }
		}

		public void DropRune( Mobile from, RunebookEntry e, int index )
		{
            if (CheckAccess(from))
            {
                if (m_DefaultIndex > index)
                    m_DefaultIndex -= 1;
                else if (m_DefaultIndex == index)
                    m_DefaultIndex = -1;

                m_Entries.RemoveAt(index);

                RecallRune rune = new RecallRune();

                rune.Target = e.Location;
                rune.TargetMap = e.Map;
                rune.Description = e.Description;
                rune.House = e.House;
                rune.Marked = true;
                rune.ChargesLeft = e.ChargesLeft;

                from.AddToBackpack(rune);

                from.SendLocalizedMessage(502421); // You have removed the rune.
            }

            else
            {
                from.SendLocalizedMessage(502413); // That cannot be done while the book is locked down.
            }
		}

		public bool IsOpen( Mobile toCheck )
		{
			NetState ns = toCheck.NetState;

			if ( ns != null ) {
				foreach ( Gump gump in ns.Gumps ) {
					RunebookGump bookGump = gump as RunebookGump;

					if ( bookGump != null && bookGump.Book == this ) {
						return true;
					}
				}
			}

			return false;
		}

		public override bool DisplayLootType{ get{ return Core.AOS; } }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

            if (m_Quality == BookQuality.Exceptional)
                list.Add(1063341); // exceptional

			if ( m_Crafter != null )
				list.Add( 1050043, m_Crafter.Name ); // crafted by ~1_NAME~

			if ( m_Description != null && m_Description.Length > 0 )
				list.Add( m_Description );
		}

		public override void OnSingleClick( Mobile from )
		{
			if ( !string.IsNullOrEmpty(m_Description) )
				LabelTo( from, m_Description );
            if (m_Crafter != null)
                LabelTo(from, "{0} crafted by {1}", CliLoc.LocToString(LabelNumber), m_Crafter.Name);
            else
                base.OnSingleClick(from);
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( GetWorldLocation(), 3 ) && from.InLOS(this) )
			{
				/*if ( DateTime.Now < NextUse )
				{
					from.SendLocalizedMessage( 502406 ); // This book needs time to recharge.
					return;
				}*/

				from.CloseGump( typeof( RunebookGump ) );
				from.SendGump( new RunebookGump( from, this ) );
                if (!m_Dictionary.ContainsKey(from))
                    m_Dictionary.Add(from, from.Location);
                else
                    m_Dictionary[from] = from.Location;
			}
            else
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
		}

		public virtual void OnTravel()
		{
			NextUse = DateTime.Now + UseDelay;
		}

        public override void OnAfterDuped( Item newItem )
        {
            Runebook book = newItem as Runebook;

            if ( book == null )
                return;

            book.m_Entries = new List<RunebookEntry>();

            for ( int i = 0; i < m_Entries.Count; i++ )
            {
                RunebookEntry entry = m_Entries[i];

                book.m_Entries.Add( new RunebookEntry( entry.Location, entry.Map, entry.Description, entry.House, entry.ChargesLeft ) );
            }
        }

		public bool CheckAccess( Mobile m )
		{
            if ((IsLockedDown || !Movable) && m.AccessLevel <= AccessLevel.Counselor)
                return false;

            else
                return true;

			/*BaseHouse house = BaseHouse.FindHouseAt( this );

			if ( house != null && house.IsAosRules && (house.Public ? house.IsBanned( m ) : !house.HasAccess( m )) )
				return false;

			return ( house != null && house.HasSecureAccess( m, m_Level ) );*/
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( dropped is RecallRune )
			{
				if ( !CheckAccess( from ) )
				{
					from.SendLocalizedMessage( 502413 ); // That cannot be done while the book is locked down.
				}
				else if ( IsOpen( from ) )
				{
					from.SendLocalizedMessage( 1005571 ); // You cannot place objects in the book while viewing the contents.
				}
				else if ( m_Entries.Count < 16 )
				{
					RecallRune rune = (RecallRune)dropped;

					if ( rune.Marked && rune.TargetMap != null )
					{
                        if (rune.ChargesLeft > 0)
                        {
                            m_Entries.Add(new RunebookEntry(rune.Target, rune.TargetMap, rune.Description, rune.House, rune.ChargesLeft));

                            dropped.Delete();

                            from.Send(new PlaySound(0x42, GetWorldLocation()));

                            string desc = rune.Description;

                            if (desc == null || (desc = desc.Trim()).Length == 0)
                                desc = "(indescript)";

                            from.SendMessage("You add a rune to {0} in the runebook.", desc);

                            return true;
                        }
                        else
                        {
                            from.SendAsciiMessage("This rune has no charges left.");
                        }
					}
					else
					{
						from.SendLocalizedMessage( 502409 ); // This rune does not have a marked location.
					}
				}
				else
				{
					from.SendLocalizedMessage( 502401 ); // This runebook is full.
				}
			}
			else if ( dropped is RecallScroll )
			{
				if ( m_CurCharges < m_MaxCharges )
				{
					from.Send( new PlaySound( 0x249, GetWorldLocation() ) );

					int amount = dropped.Amount;

					if ( amount > (m_MaxCharges - m_CurCharges) )
					{
						dropped.Consume( m_MaxCharges - m_CurCharges );
						m_CurCharges = m_MaxCharges;
					}
					else
					{
						m_CurCharges += amount;
						dropped.Delete();

						return true;
					}
				}
				else
				{
					from.SendLocalizedMessage( 502410 ); // This book already has the maximum amount of charges.
				}
			}

			return false;
		}
		#region ICraftable Members

		public int OnCraft( int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			int charges = 5 + quality + (int)(from.Skills[SkillName.Inscribe].Value / 30);

			if ( charges > 10 )
				charges = 10;

			MaxCharges = (Core.SE ? charges * 2 : charges);

			if ( makersMark )
				Crafter = from;

            m_Quality = (BookQuality)(quality - 1);

			return quality;
		}

		#endregion
	}

	public class RunebookEntry
	{
		private readonly Point3D m_Location;
		private readonly Map m_Map;
		private readonly string m_Description;
		private readonly BaseHouse m_House;
        private int m_ChargesLeft;

        public int ChargesLeft
        {
            get { return m_ChargesLeft; }
            set { m_ChargesLeft = value; }
        }

		public Point3D Location
		{
			get{ return m_Location; }
		}

		public Map Map
		{
			get{ return m_Map; }
		}

		public string Description
		{
			get{ return m_Description; }
		}

		public BaseHouse House
		{
			get{ return m_House; }
		}

		public RunebookEntry( Point3D loc, Map map, string desc, BaseHouse house, int charges)
		{
			m_Location = loc;
			m_Map = map;
			m_Description = desc;
			m_House = house;
            m_ChargesLeft = charges;
		}

		public RunebookEntry( GenericReader reader )
		{
			int version = reader.ReadByte();

			switch ( version )
			{
				case 1:
				{
					m_House = reader.ReadItem() as BaseHouse;
					goto case 0;
				}
				case 0:
				{
					m_Location = reader.ReadPoint3D();
					m_Map = reader.ReadMap();
					m_Description = reader.ReadString();
                    m_ChargesLeft = reader.ReadInt();

					break;
				}
			}
		}

		public void Serialize( GenericWriter writer )
		{
			if ( m_House != null && !m_House.Deleted )
			{
				writer.Write( (byte) 1 ); // version

				writer.Write( m_House );
			}
			else
			{
				writer.Write( (byte) 0 ); // version
			}

			writer.Write(m_Location);
			writer.Write(m_Map);
			writer.Write(m_Description);
            writer.Write(m_ChargesLeft);
		}
	}
}