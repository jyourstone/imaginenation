/*
 * Damien's Bounty System
 * 
 * 1.4
 * Expire time now shows in the Bounty Board Gump
 * Expire time now shows in the Edit Bounty Gump
 * People that you can claim bounties on are Noto-Attackable (gray).
 * If you have been accepted to claim a bounty it is now shown in the bounty gump.
 * Fixed issue with wrong string shown in message when a bounty is removed/expired.
 * Owners are now notified when bounties are claimed.
 * People that can perform the hunt are now notified when it is over.
 * 
 * 1.3
 * Added DefaultMinBounty method to BountyBoardEntry, by default its 1000
 * Fixed gump error with gump not closing on right click.
 * Fixed Crash when dropping a head 
 * 
 * 1.2
 * Added ascending sort feature for names and descending sort feature for 
 * price (press button in label header).
 * Players can no longer request bounties for other players on their account.
 * Bounty hunts are no long criminal actions.
 * 
 * 1.1
 * Added DefaultDecayTime to retrieve the default bounty decay time.
 * Fixed Bounty Reward of 0gp bug.
 * Heads created before a bounty is placed can't be turned in.
 * Fixed crash bug on request bounty by a badly formated string.
 * If players have a bounty on them and are killed by a bounty collect they 
 * cannot report a murder.
 * Now when a bounty is claimed if there are multiple bounties claimable the 
 * oldest bounty is used.
 * Fixed bug of owner being refunded multiple times when a bounty expires.
 *
 * 1.0
 * To view the bounty gump type [bounties
 * Heads can only be turned into Order Guards.
 * Bounties decay after two weeks.
 * Bounties can only be created if a murder is credited.
 * To avoid OSI exploits the bounty Owner must approve the Bounty
 * hunter.
 * Gold is removed from the owner's bank on bounty creation.  Returned to
 * the bank if the bounty decays.
 * 
 * BountySchema.xsd must be in the .\Data\Bounty System directory.
 * Active bounties are stored in Bounties.xml
 * The request tag are people who request the bounty.
 * The accept tag are people who can claim the bounty.
 * 
 */

using System;
using System.Text;
using System.Collections;
using System.Data;
using System.IO;
using Server;
using Server.Prompts;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Commands;

namespace Server.BountySystem
{
	public class BountyBoardEntry
	{
		public static TimeSpan DefaultDecayRate { get { return TimeSpan.FromDays( 14.0 ); } }
		public static int DefaultMinBounty{ get { return 1000; } }

		private Mobile m_Owner;
		private Mobile m_Wanted;
		private ArrayList m_Requested;
		private ArrayList m_Accepted;
		private int m_Price;
		private DateTime m_ExpireTime;

		public Mobile Owner{ get{ return m_Owner; } }
		public Mobile Wanted{ get{ return m_Wanted; } }

		public DateTime ExpireTime
		{ 
			get{ return m_ExpireTime; } 
			set{ m_ExpireTime = value; }
		}

		public int Price
		{ 
			get{ return m_Price; } 
			set{ m_Price = value; }
		}

		public bool Expired{ get{ return ( DateTime.Now >= m_ExpireTime ); } }
		public ArrayList Requested{ get{ return m_Requested; } }
		public ArrayList Accepted{ get{ return m_Accepted; } }

		public BountyBoardEntry( Mobile Owner, Mobile Wanted, int price, DateTime expireTime )
		{
			m_Owner = Owner;
			m_Wanted = Wanted;
			m_Price = price;
			m_ExpireTime = expireTime;

			m_Requested = new ArrayList();
			m_Accepted = new ArrayList();
		}
	}

	[Flipable( 0x1E5E, 0x1E5F )]
	public class BountyBoard : Item
	{
		private static string xmlSchema = ".\\data\\bounty system\\BountySchema.xsd";
		private static string xmlFile = ".\\data\\bounty system\\Bounties.xml";
		private static string xmlFileBackup = ".\\data\\bounty system\\Bounties.bak";

		private static ArrayList m_Entries;

		public override int LabelNumber{ get{ return (m_Entries.Count > 0 ? 1042679 : 1042680); } }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if(m_Entries.Count > 0)
			{
				string text = String.Format( "A bounty board with {0} posted bounties", m_Entries.Count );
				list.Add( text );
			}
			else
				list.Add( 1042679 );
		}

		public static void Initialize()
		{
			CommandSystem.Register( "Bounties", AccessLevel.GameMaster, new CommandEventHandler( Bounties_OnCommand ) );
			m_Entries = new ArrayList();

			EventSink.WorldSave += new WorldSaveEventHandler( saveGlobalList );

			loadGlobalList();
		}

		[Usage( "Bounties" )]
		[Description( "Manages the global bounty list." )]
		public static void Bounties_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendGump( new BountyBoardGump( e.Mobile, null ) );
		}

		public static void saveGlobalList( WorldSaveEventArgs e )
		{
			if( !System.IO.File.Exists( xmlSchema ) )
			{
				Console.WriteLine( "Could not open {0}.", xmlSchema );
				Console.WriteLine( "{0} must be in .\\Data\\Bounty System", xmlSchema );
				Console.WriteLine( "Cannot save bounties." );
				return;
			}

			if( System.IO.File.Exists(xmlFileBackup) )
				System.IO.File.Delete(xmlFileBackup);

			if( System.IO.File.Exists( xmlFile ) )
			{
				System.IO.File.Move(xmlFile, xmlFileBackup);
			}

			DataSet ds = new DataSet();
			ds.ReadXmlSchema( xmlSchema );
			DataRow bountyRow, OwnerRow, WantedRow, requestedRow, acceptedRow;
            try
            {
                var entries = new BountyBoardEntry[m_Entries.Count];
                m_Entries.CopyTo(entries);

                foreach (BountyBoardEntry entry in m_Entries)
                {
                    if (entry == null)
                        continue;

                    if (entry.Owner == null || entry.Owner.Deleted)
                        continue;

                    if (entry.Wanted == null || entry.Wanted.Deleted)
                    {
                        BountyBoard.RemoveEntry(entry, true);
                        continue;
                    }

                    bountyRow = ds.Tables["Bounty"].NewRow();
                    bountyRow["Price"] = entry.Price;
                    bountyRow["ExpireTime"] = entry.ExpireTime;
                    ds.Tables["Bounty"].Rows.Add(bountyRow);

                    OwnerRow = ds.Tables["Owner"].NewRow();
                    OwnerRow["Name"] = entry.Owner.Name;
                    OwnerRow["Serial"] = entry.Owner.Serial.Value;
                    OwnerRow.SetParentRow(bountyRow, ds.Relations["Bounty_Owner"]);
                    ds.Tables["Owner"].Rows.Add(OwnerRow);

                    WantedRow = ds.Tables["Wanted"].NewRow();
                    WantedRow["Name"] = entry.Wanted.Name;
                    WantedRow["Serial"] = entry.Wanted.Serial.Value;
                    WantedRow.SetParentRow(bountyRow, ds.Relations["Bounty_Wanted"]);
                    ds.Tables["Wanted"].Rows.Add(WantedRow);

                    foreach (Mobile requested in entry.Requested)
                    {
                        if (requested == null || requested.Deleted)
                            continue;

                        requestedRow = ds.Tables["Requested"].NewRow();
                        requestedRow["Name"] = requested.Name;
                        requestedRow["Serial"] = requested.Serial.Value;
                        requestedRow.SetParentRow(bountyRow, ds.Relations["Bounty_Requested"]);
                        ds.Tables["Requested"].Rows.Add(requestedRow);
                    }

                    foreach (Mobile accepted in entry.Accepted)
                    {
                        if (accepted == null || accepted.Deleted)
                            continue;

                        acceptedRow = ds.Tables["Accepted"].NewRow();
                        acceptedRow["Name"] = accepted.Name;
                        acceptedRow["Serial"] = accepted.Serial.Value;
                        acceptedRow.SetParentRow(bountyRow, ds.Relations["Bounty_Accepted"]);
                        ds.Tables["Accepted"].Rows.Add(acceptedRow);
                    }

                    ds.WriteXml(xmlFile);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
		}

		public static void loadGlobalList()
		{
			if( !System.IO.File.Exists( xmlSchema ) )
			{
				Console.WriteLine( "Could not open {0}.", xmlSchema );
				Console.WriteLine( "{0} must be in .\\Data\\Bounty System", xmlSchema );
				Console.WriteLine( "Cannot save bounties." );
				return;
			}

			if( !System.IO.File.Exists( xmlFile ) )
			{
				Console.WriteLine( "Could not open {0}.", xmlFile );
				Console.WriteLine( "{0} must be in .\\Data\\Bounty System", xmlFile );
				Console.WriteLine( "This is okay if this is the first run after installation of the Bounty system." );
				return;
			}

			DataSet ds = new DataSet();
			try
			{
				ds.ReadXmlSchema( xmlSchema );
				ds.ReadXml( xmlFile );
			}
			catch
			{
				Console.WriteLine( "Error reading {0}.  File may be corrupt.", xmlFile);
				return;
			}

			Mobile Owner = null;
			Mobile Wanted = null;
			Mobile requested = null;
			Mobile accepted = null;
			int price;
			DateTime expireTime;
			BountyBoardEntry entry;

			foreach( DataRow bountyRow in ds.Tables["Bounty"].Rows )
			{
				foreach( DataRow childRow in bountyRow.GetChildRows( "Bounty_Owner" ) )
					Owner = World.FindMobile( (int) childRow["Serial"] );

				if(Owner == null || Owner.Deleted  || !(Owner is PlayerMobile) )
					continue;

				foreach( DataRow childRow in bountyRow.GetChildRows( "Bounty_Wanted" ) )
					Wanted = World.FindMobile( (int) childRow["Serial"] );

				price = (int) bountyRow["Price"];
				expireTime = (DateTime) bountyRow["ExpireTime"];

				entry = new BountyBoardEntry( Owner, Wanted, price, expireTime );

				foreach( DataRow childRow in bountyRow.GetChildRows( "Bounty_requested" ) )
				{
					requested = World.FindMobile( (int) childRow["Serial"] );
					if( requested != null && !requested.Deleted && requested is PlayerMobile )
						entry.Requested.Add( requested );
				}

				foreach( DataRow childRow in bountyRow.GetChildRows( "Bounty_accepted" ) )
				{
					accepted = World.FindMobile( (int) childRow["Serial"] );
					if( accepted != null && !accepted.Deleted && accepted is PlayerMobile )
						entry.Accepted.Add( accepted );
				}

				if( !entry.Expired )
					BountyBoard.AddEntry( entry );
				else
				{
					NotifyBountyEnd( entry );
					if ( !Banker.Deposit( entry.Owner, price ) )
						entry.Owner.AddToBackpack( new Gold( price ) );
				}

				if( Wanted == null || Wanted.Deleted || !(Wanted is PlayerMobile) )
					BountyBoard.RemoveEntry( entry, true );
			}
		}

		public static ArrayList Entries
		{
			get{ return m_Entries; }
		}

		public static BountyBoardEntry AddEntry( Mobile Owner, Mobile Wanted, int price, DateTime expireTime )
		{
			foreach( BountyBoardEntry entry in m_Entries )
			{
				if(entry.Owner == Owner && entry.Wanted == Wanted)
				{
					entry.Price += price;
					entry.ExpireTime = expireTime;
					return entry;
				}
			}

			BountyBoardEntry be = new BountyBoardEntry( Owner, Wanted, price, expireTime );

			m_Entries.Add( be );

			ArrayList instances = BountyBoard.Instances;

			for ( int i = 0; i < instances.Count; ++i )
				((BountyBoard)instances[i]).InvalidateProperties();

			return be;
		}

		public static void AddEntry( BountyBoardEntry be )
		{
			m_Entries.Add( be );

			ArrayList instances = BountyBoard.Instances;

			for ( int i = 0; i < instances.Count; ++i )
				((BountyBoard)instances[i]).InvalidateProperties();
		}

		public static void RemoveEntry( BountyBoardEntry be, bool refund )
		{
			m_Entries.Remove( be );
			
			ArrayList instances = BountyBoard.Instances;

			for ( int i = 0; i < instances.Count; ++i )
				((BountyBoard)instances[i]).InvalidateProperties();

			if( refund && be != null && be.Owner != null && be.Wanted != null)
			{
				string msg = String.Format( "Your bounty in the amount of {0} on {1}'s head has ended.", be.Price, be.Wanted.Name );
				if( NetState.Instances.Contains( be.Owner.NetState ) )
				{
					be.Owner.SendMessage( msg );
				}
				else
				{
					((PlayerMobile)be.Owner).ShowBountyUpdate = true;
					((PlayerMobile)be.Owner).BountyUpdateList.Add( msg );
				}

				if ( Banker.Deposit( be.Owner, be.Price ) )
					be.Owner.SendLocalizedMessage( 1060397, be.Price.ToString() ); // ~1_AMOUNT~ gold has been deposited into your bank box.
				else
				{
					be.Owner.AddToBackpack( new Gold( be.Price ) );
					be.Owner.SendMessage( "The bounty of {0} has been added to your backpack.", be.Price );
				}
			}
			else if( be != null && be.Owner != null && be.Wanted != null)
			{
				string msg = String.Format( "Your bounty in the amount of {0} on {1}'s head has been claimed.", be.Price,
					be.Wanted.Name );
				if( NetState.Instances.Contains( be.Owner.NetState ) )
				{
					be.Owner.SendMessage( msg );
				}
				else
				{
					((PlayerMobile)be.Owner).ShowBountyUpdate = true;
					((PlayerMobile)be.Owner).BountyUpdateList.Add( msg );
				}
			}

			NotifyBountyEnd( be );
		}

		public static void NotifyBountyEnd( BountyBoardEntry be )
		{
			foreach ( PlayerMobile player in be.Accepted )
			{
				string msg = String.Format( "The bounty hunt on {0}'s head is over.",
					be.Wanted.Name );
				if( NetState.Instances.Contains( player.NetState ) )
				{
					player.SendMessage( msg );
				}
				else
				{
					player.ShowBountyUpdate = true;
					player.BountyUpdateList.Add( msg );
				}
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendGump( new BountyBoardGump( from, this ) );
		}

		private static ArrayList m_Instances = new ArrayList();

		public static ArrayList Instances
		{
			get{ return m_Instances; }
		}

		[Constructable]
		public BountyBoard()  : base( 0x1E5E )
		{
			m_Instances.Add( this );
			Name = "Bounty Board";
			Movable = false;
		}

		public override void OnDelete()
		{
			m_Instances.Remove( this );
			base.OnDelete();
		}

		public BountyBoard( Serial serial ) : base( serial )
		{
			m_Instances.Add( this );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public static bool hasBounty( Mobile claimer, Mobile killer, out BountyBoardEntry bountyEntry, 
			out bool canClaim)
		{
			bountyEntry = null;
			canClaim = false;
			DateTime expireTime = DateTime.MaxValue;
			bool hasBounty = false;

			foreach( BountyBoardEntry entry in m_Entries )
			{
				if(entry.Wanted == killer )
				{
					hasBounty = true;
					canClaim = entry.Accepted.Contains( claimer );

					if( /*canClaim &&*/ entry.ExpireTime < expireTime ) // crash bugfix
					{
						bountyEntry = entry;
						expireTime = entry.ExpireTime;
					}
				}
			}
			return hasBounty;
		}

		public static bool Attackable( Mobile attacker, Mobile attackee )
		{
			foreach( BountyBoardEntry entry in m_Entries )
			{
				if( entry.Wanted == attackee && entry.Accepted.Contains( attacker ) )
					return true;
			}

			return false;
		}
	}
}