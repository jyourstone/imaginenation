using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Server.Accounting;
using Server.Commands;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.Targeting;

/* Jail script by Cat 9-8-2003, RunUO-Beta-36
 * started on b30.
 * Thanks to Kennie-Insane, I reused his code for handling turning the sentence 
 * length into a timeSpan. 
 * 
 * added fast movement warnig/jailing
 * added cage command
 * add holding cell region (for the cage)
 * release now verifies the existance of all mobiles on the jailing and removes 
 * any releases for mobiles that do not exisit, this will allow jailings 
 * to remove themselves when there are no more characters on the jailing
 * 
 * 
 * ENABLING PLAYER CONTEXT MENUS
 * the script will work without enabling the context menus.  This is optional.
 * add using Server.Scripts.Commands; to your playermobile file;
 * in your playermobile add "JailSystem.getcontextmenus(from,this,list);" right after 
 * base.GetContextMenuEntries( from, list );
 * should look something like this
 * public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );
			JailSystem.getcontextmenus(from,this,list);	
*/

namespace Server.Jailing
{
	public class JailSystem
	{
		private const string scriptVersion = "IN special";
		public static bool warnspeedy = false;
		public static bool timersRunning = false;
		public static string statusCommand = "jailtime";
		public static string timeCommand = "what time is it";
		public static string ooclistCommand = "ooclist";
		public static bool SingleFacetOnly = true;
		public static string JSName = "JailSystem";
		public static bool useSmokingFootGear = true;
		public static bool useLanguageFilter = false;
		public static string foulJailorName = "Language Auto Jailor";
		public static bool allowStaffBadWords = true;
		public static string oocJailorName = "OOC Jailor";
		public static bool blockOOCSpeech = false;
		public static bool useOOCFilter = false;
		public static bool AllowStaffOOC = true;
		public static int oocwarns = 2;

		public static List<Point3D> defaultRelease = new List<Point3D>();
		public static List<Point3D> cells = new List<Point3D>();
		public static List<string> badWords = new List<string>();
		public static List<string> oocWords = new List<string>();
		public static List<string> oocParts = new List<string>();
		public static List<TimeSpan> oocJailTimes = new List<TimeSpan>();
		public static List<TimeSpan> FoulMouthJailTimes = new List<TimeSpan>();

		public static Map jailMap;
		public static Map defaultReleaseFacet;

		private static Dictionary<int, JailSystem> m_jailings;
		private static Dictionary<string, Dictionary<DateTime, string>> m_warnings;
		private static Dictionary<string, Dictionary<DateTime, string>> m_fwalkWarnings;
		private static string jailDirectory = "Saves/Jailings";
		private static readonly string idxPath = Path.Combine( jailDirectory, "Jailings.idx" );
		private static readonly string binPath = Path.Combine( jailDirectory, "Jailings.bin" );
		private static int m_NextID = 0;
		private bool returnToPoint = true;
		private readonly Dictionary<int, releaseLoc> releasePoints = new Dictionary<int, releaseLoc>();
		private DateTime m_releaseTime;
		private JailingTimer autoReleasor;
		private int m_id;
		private AccessLevel m_jailorAC = AccessLevel.Counselor;
		private string m_name;
		public string jailor;
		public string reason;
		public string freedBy = JSName;

		public JailSystem()
		{
			buildJail();
		}

		public JailSystem( Mobile m ) : this( m, AccessLevel.Counselor )
		{
		}

		public JailSystem( Mobile m, AccessLevel l )
		{
			buildJail();
			Name = ( (Account)m.Account ).Username;
			m_jailorAC = l;
		}

		public JailSystem( Serial serial )
		{
			buildJail();
		}

		public static Dictionary<string, Dictionary<DateTime, string>> fWalkWarns
		{
			get
			{
				if( m_fwalkWarnings == null )
					m_fwalkWarnings = new Dictionary<string, Dictionary<DateTime, string>>();
				return m_fwalkWarnings;
			}
		}

		public static Dictionary<string, Dictionary<DateTime, string>> warns
		{
			get
			{
				if( m_warnings == null )
					m_warnings = new Dictionary<string, Dictionary<DateTime, string>>();
				return m_warnings;
			}
		}

		public static Dictionary<int, JailSystem> list
		{
			get { return m_jailings; }
		}

		public DateTime ReleaseDate
		{
			get { return m_releaseTime; }
		}

		public bool jailed
		{
			get { return ( ReleaseDate > DateTime.Now ); }
		}

		public int ID
		{
			get { return m_id; }
		}

		public Account Prisoner
		{
			get { return Accounts.GetAccount( Name ) as Account; }
		}

		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		#region Event Handlers
		public static void EventSink_Speech( SpeechEventArgs args )
		{
			if( args.Mobile is PlayerMobile )
			{
				if( useLanguageFilter && !( args.Mobile.Region is Jail ) )
					foreach( string s in badWords )
						if( args.Speech.ToLower().Trim().IndexOf( s.ToLower() ) >= 0 )
							if( ( ( (Account)args.Mobile.Account ).AccessLevel > AccessLevel.Player ) && ( allowStaffBadWords ) )
								args.Mobile.SendMessage( "If you were not staff you’d be in jail now.  Behave yourself" );
							else
							{
								Jail( args.Mobile, true, string.Format( "Foul language, \"{0}\" \"{1}\"", s, args.Speech ), true, foulJailorName );
								return;
							}
				if( ( args.Speech.ToLower().Trim() == timeCommand ) || ( args.Speech.ToLower().Trim() == timeCommand + "?" ) ) //time query
					args.Mobile.SendMessage( "It is currently {0} by the servers clock", DateTime.Now.ToString() );
				else if( args.Speech.ToLower().Trim() == "jailsystem" ) //credit speech
				{
					args.Mobile.SendMessage( "This server is running Cat's JailSystem Version:{0}", scriptVersion );
					args.Blocked = true;
					return;
				}
				else if( args.Speech.ToLower().Trim() == statusCommand ) //status command
				{
				    bool jail = false;
                    CustomRegion cR = args.Mobile.Region as CustomRegion;
                    if (cR != null && cR.Controller.RegionName.ToLower() == "jail")
                        jail = true;

				    if( args.Mobile.Region is Jail || jail ) //ignore it if they rant in jail
					{
						JailSystem js = fromMobile( args.Mobile );
						if( js != null )
						{
							args.Mobile.SendMessage( "You were jailed by: {0}", js.jailor );
							args.Mobile.SendMessage( "You were jailed for: {0}", js.reason );
							args.Mobile.SendMessage( "You are to be released at: {0}", js.ReleaseDate.ToString() );
						}
						else
							args.Mobile.SendMessage( "You are missing a jailing object, page a gm" );
					} //end status area
					args.Blocked = true;
					return;
				}
				else if( args.Speech.ToLower().Trim() == ooclistCommand ) //status command
				{
					string ooclst = "Here is the list of words that can land you in jail:";
					foreach( string s in oocWords )
						ooclst += " " + s;
					foreach( string s in oocParts )
						ooclst += " " + s;
					args.Mobile.SendMessage( ooclst );
					args.Blocked = true;
					return;
				}
				if( ( useOOCFilter ) && ( !( args.Mobile.Region is Jail ) ) )
				{
					foreach( string s in oocParts )
						if( args.Speech.ToLower().Trim().IndexOf( s.ToLower() ) >= 0 )
							if( ( ( (Account)args.Mobile.Account ).AccessLevel == AccessLevel.Player ) || ( !AllowStaffOOC ) )
							{
								oocWarn( args.Mobile, s );
								args.Blocked = blockOOCSpeech;
								return;
							}
							else
								args.Mobile.SendMessage( "Your staff, but please avoid ooc stuff as much as possible-triggered on '{0}'.  say '{1}' to see the list of ooc words", s, ooclistCommand );
					foreach( string s in oocWords )
						if( args.Speech.ToLower().Trim() == s.ToLower() )
							if( ( ( (Account)args.Mobile.Account ).AccessLevel == AccessLevel.Player ) || ( !AllowStaffOOC ) )
							{
								oocWarn( args.Mobile, s );
								args.Blocked = blockOOCSpeech;
								return;
							}
							else
								args.Mobile.SendMessage( "Your staff, but please avoid ooc stuff as much as possible-triggered on '{0}'.  say '{1}' to see the list of ooc words", s, ooclistCommand );
				}
			}
		}

		public static void onLoad()
		{
			Console.WriteLine( "Loading Jailings" );
			FileStream idxFileStream;
			FileStream binFileStream;
			//BinaryReader idxReader;
			BinaryFileReader idxReader;
			BinaryFileReader binReader;
			//GenericReader idxReader;
			long tPos;
			int tID;
			int tLength;
			JailSystem tJail;
			int JailCount = 0;
			int temp = 0;
			if( ( File.Exists( idxPath ) ) && ( File.Exists( binPath ) ) )
			{
				idxFileStream = new FileStream( idxPath, (FileMode)3, (FileAccess)1, (FileShare)1 );
				binFileStream = new FileStream( binPath, (FileMode)3, (FileAccess)1, (FileShare)1 );
				try
				{
					idxReader = new BinaryFileReader( new BinaryReader( idxFileStream ) );
					binReader = new BinaryFileReader( new BinaryReader( binFileStream ) );
					JailCount = idxReader.ReadEncodedInt();
					if( JailCount > 0 )
						for( int i = 0; i < JailCount; i++ )
						{
							temp = idxReader.ReadInt(); //catch the version number which we wont use
							tID = idxReader.ReadInt();
							tPos = idxReader.ReadLong();
							tLength = idxReader.ReadInt();
							tJail = new JailSystem( tID );
							binReader.Seek( tPos, 0 );
							try
							{
								tJail.Deserialize( binReader );
								if( binReader.Position != ( tPos + tLength ) )
									throw new Exception( String.Format( "***** Bad serialize on {0} *****", tID ) );
							}
							catch
							{
							}
						}
					else
					{
					}
					loadingameeditsettings( binReader );
				}
				finally
				{
					if( idxFileStream != null )
						idxFileStream.Close();
					if( binFileStream != null )
						binFileStream.Close();
				}
			}
			else
			{
				defaultSettings();
				Console.WriteLine( "{0}: No prior Jailsystem save, using default settings", JSName );
			}
			Console.WriteLine( "{0} Jailings Loaded:{1}", JailCount, list.Count );
		}

		public static void OnLoginJail( LoginEventArgs e )
		{
			if( !timersRunning ) //start the timers on the first user to login
			{
				timersRunning = true; //so no-one else causes the process to run
				bool loopdone = false;
				while( !loopdone )
					try
					{
						foreach( JailSystem tjs in list.Values )
							tjs.StartTimer();
						loopdone = true;
					}
					catch( Exception err )
					{
						Console.WriteLine( "Restarting the Jail timer load process:{0}", err.Message );
					}
				Console.WriteLine( "The Jail timer load process has finished" );
			}
			if( e.Mobile == null )
				return;
			JailSystem js = fromMobile( e.Mobile ); //check to see if they have a jail object
			if( js == null )
				return; //they don’t so we bail
			if( js.jailed ) //are they jailed?
				js.lockupMobile( e.Mobile ); //yup so lock them up
			else
				js.release( e.Mobile.NetState ); //no so we release them
		}

		public static void onSave( WorldSaveEventArgs e )
		{
			Console.WriteLine( "Saving Jailings" );
			if( !Directory.Exists( jailDirectory ) )
				Directory.CreateDirectory( jailDirectory );
			GenericWriter idxWriter;
			GenericWriter binWriter;
			long tPos;
			if( StandardSaveStrategy.SaveType == 0 )
			{
				idxWriter = new BinaryFileWriter( idxPath, false );
				binWriter = new BinaryFileWriter( binPath, true );
			}
			else
			{
				idxWriter = new AsyncWriter( idxPath, false );
				binWriter = new AsyncWriter( binPath, true );
			}

			idxWriter.WriteEncodedInt( m_jailings.Count );
			try
			{
				foreach( JailSystem tJail in m_jailings.Values )
				{
					tPos = binWriter.Position;
					idxWriter.Write( 0 );
					idxWriter.Write( tJail.ID );
					idxWriter.Write( tPos );
					try
					{
						tJail.Serialize( binWriter );
					}
					catch( Exception err )
					{
						Console.WriteLine( "{0}, {1} serialize", err.Message, err.TargetSite );
					}
					idxWriter.Write( (int)( binWriter.Position - tPos ) );
				}
				saveingameeditsettings( binWriter );
			}
			catch( Exception er )
			{
				Console.WriteLine( "{0}, {1}", er.Message, er.TargetSite );
			}
			idxWriter.Close();
			binWriter.Close();
			Console.WriteLine( "Jailings Saved" );
		}
		#endregion

		public static void defaultSettings()
		{
			statusCommand = "jailtime";
			timeCommand = "what time is it";
			ooclistCommand = "ooclist";
			SingleFacetOnly = true;
			JSName = "JailSystem";
			useSmokingFootGear = true;
			jailMap = Map.Trammel;
			defaultReleaseFacet = Map.Trammel;

			defaultRelease.Clear();

			defaultRelease.Add( new Point3D( 2708, 693, 0 ) );
			defaultRelease.Add( new Point3D( 4476, 1281, 0 ) );
			defaultRelease.Add( new Point3D( 1344, 1994, 0 ) );
			defaultRelease.Add( new Point3D( 1507, 3769, 0 ) );
			defaultRelease.Add( new Point3D( 780, 754, 0 ) );
			defaultRelease.Add( new Point3D( 1833, 2942, -22 ) );
			defaultRelease.Add( new Point3D( 651, 2066, 0 ) );
			defaultRelease.Add( new Point3D( 3556, 2150, 26 ) );

			useLanguageFilter = false;
			foulJailorName = "Language Auto Jailor";
			allowStaffBadWords = true;
			badWords.Clear();
			badWords.Add( "fuck" );
			badWords.Add( "cunt" );
			FoulMouthJailTimes.Clear();
			FoulMouthJailTimes.Add( new TimeSpan( 0, 0, 30, 0 ) );
			FoulMouthJailTimes.Add( new TimeSpan( 0, 1, 0, 0 ) );
			FoulMouthJailTimes.Add( new TimeSpan( 0, 1, 30, 0 ) );
			FoulMouthJailTimes.Add( new TimeSpan( 0, 2, 0, 0 ) );
			FoulMouthJailTimes.Add( new TimeSpan( 0, 2, 30, 0 ) );
			//ooc section
			oocJailorName = "OOC Jailor";
			blockOOCSpeech = false;
			useOOCFilter = false;
			oocWords.Add( "lol" );
			oocWords.Add( "ty" );
			oocWords.Add( "yw" );
			oocWords.Add( "rofl" );
			oocWords.Add( "roflmao" );
			oocWords.Add( "lmao" );
			oocWords.Add( "np" );
			oocWords.Add( "newb" );
			oocWords.Add( "brb" );
			oocWords.Add( "afk" );
			oocParts.Add( "computer" );
			oocParts.Add( "phone" );
			AllowStaffOOC = true;
			oocJailTimes.Clear();
			oocJailTimes.Add( new TimeSpan( 0, 0, 10, 0 ) );
			oocJailTimes.Add( new TimeSpan( 0, 0, 20, 0 ) );
			oocJailTimes.Add( new TimeSpan( 0, 0, 30, 0 ) );
			oocJailTimes.Add( new TimeSpan( 0, 0, 40, 0 ) );
			oocJailTimes.Add( new TimeSpan( 0, 1, 0, 0 ) );
			oocwarns = 2;
			cells.Clear();

			cells.Add( new Point3D( 1324, 1676, 30 ) ); 
			cells.Add( new Point3D( 1336, 1680, 30 ) );
			cells.Add( new Point3D( 1346, 1680, 30 ) );
            cells.Add(new Point3D(1324, 1676, 30));
            cells.Add(new Point3D(1336, 1680, 30));
            cells.Add(new Point3D(1346, 1680, 30));
            cells.Add(new Point3D(1324, 1676, 30));
            cells.Add(new Point3D(1336, 1680, 30));
            cells.Add(new Point3D(1346, 1680, 30));
			/*cells.Add( new Point3D( 5306, 1164, 0 ) );
			cells.Add( new Point3D( 5276, 1174, 0 ) );
			cells.Add( new Point3D( 5286, 1174, 0 ) );
			cells.Add( new Point3D( 5296, 1174, 0 ) );
			cells.Add( new Point3D( 5306, 1174, 0 ) );
			cells.Add( new Point3D( 5283, 1184, 0 ) );*/
		}

		public static JailSystem fromMobile( Mobile m )
		{
			return fromAccount( (Account)m.Account );
		}

		public static JailSystem fromAccount( Account a )
		{
			string un = a.Username;
			foreach( JailSystem js in list.Values )
				if( js.Name == un )
					return js;
			return null;
		}

		public static JailSystem lockup( Mobile m )
		{
			try
			{
				JailSystem js = fromMobile( m );
				if( js == null )
					js = new JailSystem( m );
				js.lockupMobile( m );
				return js;
			}
			catch
			{
				Console.WriteLine( "{0}: Lockup call failed on-{1}", JSName, m.Name );
				return null;
			}
		}

		public static void Jail( Mobile badBoy, bool foul, string reason, bool releasetoJailing, string jailedBy, AccessLevel l )
		{
			Jail( badBoy, getJailTerm( badBoy, foul ), reason, releasetoJailing, jailedBy, l );
		}

		public static void Jail( Mobile badBoy, TimeSpan ts, string reason, bool releasetoJailing, string jailedBy, AccessLevel l )
		{
			Jail( badBoy, ts, reason, releasetoJailing, jailedBy, l, true );
		}

		public static void Jail( Mobile badBoy, TimeSpan ts, string reason, bool releasetoJailing, string jailedBy, AccessLevel l, bool useBoot )
		{
			DateTime dt = DateTime.Now.Add( ts );
			Jail( badBoy, dt, reason, releasetoJailing, jailedBy, l, useBoot );
		}

		public static void Jail( Mobile badBoy, int days, int hours, int minutes, string reason, bool releasetoJailing, string jailedBy, AccessLevel l )
		{
			DateTime dt = DateTime.Now.AddDays( days ).AddHours( hours ).AddMinutes( minutes );
			Jail( badBoy, dt, reason, releasetoJailing, jailedBy, l );
		}

		public static void Jail( Mobile badBoy, DateTime dt, string reason, bool releasetoJailing, string jailedBy, AccessLevel l )
		{
			Jail( badBoy, dt, reason, releasetoJailing, jailedBy, l, true );
		}

		public static void Jail( Mobile badBoy, DateTime dt, string reason, bool releasetoJailing, string jailedBy, AccessLevel l, bool useBoot )
		{
			JailSystem js = fromMobile( badBoy );
			if( js == null )
				js = new JailSystem( badBoy, l );
			else if( js.jailed )
			{
				js.lockupMobile( badBoy, useBoot );
				return;
			}
			js.fillJailReport( badBoy, dt, reason, releasetoJailing, jailedBy );
			js.lockupMobile( badBoy, useBoot );
		}

		public static void Jail( Mobile badBoy, bool foul, string reason, bool releasetoJailing, string jailedBy )
		{
			Jail( badBoy, getJailTerm( badBoy, foul ), reason, releasetoJailing, jailedBy, AccessLevel.Counselor );
		}

		public static void Jail( Mobile badBoy, TimeSpan ts, string reason, bool releasetoJailing, string jailedBy )
		{
			DateTime dt = DateTime.Now.Add( ts );
			Jail( badBoy, dt, reason, releasetoJailing, jailedBy, AccessLevel.Counselor );
		}

		public static void Jail( Mobile badBoy, int days, int hours, int minutes, string reason, bool releasetoJailing, string jailedBy )
		{
			DateTime dt = DateTime.Now.AddDays( days ).AddHours( hours ).AddMinutes( minutes );
			Jail( badBoy, dt, reason, releasetoJailing, jailedBy, AccessLevel.Counselor );
		}

		public static void Jail( Mobile badBoy, DateTime dt, string reason, bool releasetoJailing, string jailedBy )
		{
			Jail( badBoy, dt, reason, releasetoJailing, jailedBy, AccessLevel.Counselor );
		}

		public static void Configure()
		{
			m_jailings = new Dictionary<int, JailSystem>();
			EventSink.WorldLoad += onLoad;
			EventSink.WorldSave += onSave;
			//EventSink.FastWalk += new FastWalkEventHandler( OnFastWalk );
		}

		public static bool MovementThrottle_Callback( NetState ns )
		{
			bool hotSteppen = PlayerMobile.MovementThrottle_Callback( ns );
			if( !hotSteppen )
				//Console.WriteLine( "Client: {0}: Fast movement detected (name={1})", ns, ns.Mobile.Name );
				if( warnspeedy )
					fWalkWarn( ns.Mobile );
			return hotSteppen;
		}

		public static void OnFastWalk( FastWalkEventArgs e )
		{
			e.Blocked = true; //disallow this fastwalk
			//Console.WriteLine( "Client: {0}: Fast movement detected 2(name={1}) in jail system", e.NetState, e.NetState.Mobile.Name );

			if( warnspeedy )
				fWalkWarn( e.NetState.Mobile );
		}

		public static void fWalkWarn( Mobile m )
		{
			Account acct = m.Account as Account;
			if( acct == null )
				return;
			if( !fWalkWarns.ContainsKey( acct.Username ) )
				fWalkWarns.Add( acct.Username, new Dictionary<DateTime, string>() );
			Dictionary<DateTime, string> w = fWalkWarns[acct.Username];

			if( w == null )
			{
				Jail( m, false, "Fastwalk Detected, warning system was unable to issue a warning and jailed you.", true, "Fastwalk Jailor", AccessLevel.GameMaster );
				return;
			}

			DateTime k = DateTime.Now;
			int i = 0;
			while( w.ContainsKey( k ) )
			{
				k = k.Subtract( new TimeSpan( 0, 0, 1 ) );
				if( i > 10 )
					continue;
				i++;
			}
			if( i <= 10 )
			{
				string s = "Fastwalk detection";
				w.Add( k, s );
				new warnRemover( w, k );
			}
			if( w.Count > 5 )
			{
				Jail( m, false, "Fastwalk detection after repeated warnings.", true, oocJailorName );
				fWalkWarns.Remove( acct.Username );
			}
			else
				m.SendMessage( "You have been detected using fastwalk.  If you are using a fastwalk/speed hack, stop now or go to jail." );
		}

		public static void loadingameeditsettings( GenericReader idxReader )
		{
			int version = 0;
			int temp = 0;
			try
			{
				try
				{
					version = idxReader.ReadInt();
				}
				catch
				{
					defaultSettings();
					Console.WriteLine( "{0}: settings not found in save file, using default settings.", JSName );
					return;
				}
				switch( version )
				{
					case 0:
						try
						{
							JSName = idxReader.ReadString().Trim();
							statusCommand = idxReader.ReadString().Trim();
							timeCommand = idxReader.ReadString().Trim();
							ooclistCommand = idxReader.ReadString().Trim();
							foulJailorName = idxReader.ReadString().Trim();
							oocJailorName = idxReader.ReadString().Trim();
							oocwarns = idxReader.ReadInt();
							jailMap = idxReader.ReadMap();
							defaultReleaseFacet = idxReader.ReadMap();
							SingleFacetOnly = idxReader.ReadBool();
							useSmokingFootGear = idxReader.ReadBool();
							useLanguageFilter = idxReader.ReadBool();
							allowStaffBadWords = idxReader.ReadBool();
							blockOOCSpeech = idxReader.ReadBool();
							useOOCFilter = idxReader.ReadBool();
							AllowStaffOOC = idxReader.ReadBool();

							//load default releases
							temp = idxReader.ReadEncodedInt();
							for( int i = 0; i < temp; i++ )
								defaultRelease.Add( idxReader.ReadPoint3D() );

							//load cells
							temp = idxReader.ReadEncodedInt();
							for( int i = 0; i < temp; i++ )
								cells.Add( idxReader.ReadPoint3D() );

							//load bad words
							temp = idxReader.ReadEncodedInt();
							for( int i = 0; i < temp; i++ )
								badWords.Add( idxReader.ReadString() );

							//load ooc words
							temp = idxReader.ReadEncodedInt();
							for( int i = 0; i < temp; i++ )
								oocWords.Add( idxReader.ReadString() );

							//load ooc part words
							temp = idxReader.ReadEncodedInt();
							for( int i = 0; i < temp; i++ )
								oocParts.Add( idxReader.ReadString() );

							//load oocjail times
							temp = idxReader.ReadEncodedInt();
							for( int i = 0; i < temp; i++ )
								oocJailTimes.Add( idxReader.ReadTimeSpan() );

							//load foul mouth jail times
							temp = idxReader.ReadEncodedInt();
							for( int i = 0; i < temp; i++ )
								FoulMouthJailTimes.Add( idxReader.ReadTimeSpan() );
						}
						catch
						{
							defaultSettings();
							Console.WriteLine( "{0}: settings not found in save file, using default settings.", JSName );
							return;
						}
						break;
					case -1:
						defaultSettings();
						break;
					default:
						Console.WriteLine( "{0} warning:{1}-{2}", JSName, "Loading-", "Unknown version" );
						break;
				}
			}
			catch
			{
				defaultSettings();
				Console.WriteLine( "{0}: settings not found in save file, using default settings:", JSName );
				return;
			}
		}

		public static void saveingameeditsettings( GenericWriter idxWriter )
		{
			idxWriter.Write( 0 ); //version#
			idxWriter.Write( JSName.Trim() );
			idxWriter.Write( statusCommand.Trim() );
			idxWriter.Write( timeCommand.Trim() );
			idxWriter.Write( ooclistCommand.Trim() );
			idxWriter.Write( foulJailorName.Trim() );
			idxWriter.Write( oocJailorName.Trim() );
			idxWriter.Write( oocwarns );
			idxWriter.Write( jailMap );
			idxWriter.Write( defaultReleaseFacet );
			idxWriter.Write( SingleFacetOnly );
			idxWriter.Write( useSmokingFootGear );
			idxWriter.Write( useLanguageFilter );
			idxWriter.Write( allowStaffBadWords );
			idxWriter.Write( blockOOCSpeech );
			idxWriter.Write( useOOCFilter );
			idxWriter.Write( AllowStaffOOC );

			idxWriter.WriteEncodedInt( defaultRelease.Count );
			foreach( Point3D p in defaultRelease )
				idxWriter.Write( p );

			idxWriter.WriteEncodedInt( cells.Count );
			foreach( Point3D p in cells )
				idxWriter.Write( p );

			idxWriter.WriteEncodedInt( badWords.Count );
			foreach( string s in badWords )
				idxWriter.Write( s );

			idxWriter.WriteEncodedInt( oocWords.Count );
			foreach( string s in oocWords )
				idxWriter.Write( s );

			idxWriter.WriteEncodedInt( oocParts.Count );
			foreach( string s in oocParts )
				idxWriter.Write( s );

			idxWriter.WriteEncodedInt( oocJailTimes.Count );
			foreach( TimeSpan t in oocJailTimes )
				idxWriter.Write( t );

			idxWriter.WriteEncodedInt( FoulMouthJailTimes.Count );
			foreach( TimeSpan t in FoulMouthJailTimes )
				idxWriter.Write( t );
		}

		public void buildJail()
		{
			m_id = m_NextID;
			m_releaseTime = DateTime.Now.AddDays( 1 );
			m_NextID += 1;
			m_jailings.Add( m_id, this );
			jailor = JSName;
			m_jailorAC = AccessLevel.Counselor;
			reason = "Please wait while the gm fills in a jailing report";
		}

		public void addNote( string from, string text )
		{
			Prisoner.Comments.Add( new AccountComment( JSName + "-note", text + " by: " + from ) );
		}

		public void fillJailReport( Mobile badBoy, int days, int hours, int minutes, string why, bool mreturn, string Jailor )
		{
			DateTime dt_unJail = DateTime.Now.Add( new TimeSpan( days, hours, minutes, 0 ) );
			fillJailReport( badBoy, dt_unJail, why, mreturn, Jailor );
		}

		public void fillJailReport( Mobile badBoy, DateTime dt_unJail, string why, bool mreturn, string Jailor )
		{
			Name = ( (Account)badBoy.Account ).Username;
			m_releaseTime = dt_unJail;
			reason = why;
			jailor = Jailor;
			( (Account)badBoy.Account ).Comments.Add( new AccountComment( JSName + "-jailed", "Jailed for \"" + why + "\" By:" + Jailor + " On:" + DateTime.Now + " Until:" + dt_unJail ) );
			returnToPoint = mreturn;
			StartTimer();
		}

		public virtual void Serialize( GenericWriter writer )
		{
			writer.Write( 2 );
			//new version here
			//version 2
			writer.Write( (int)m_jailorAC );
			//version 1
			writer.Write( freedBy );
			//version 0 here
			writer.Write( m_name );
			writer.Write( m_releaseTime );

			writer.WriteEncodedInt( releasePoints.Count );
			foreach( releaseLoc rl in releasePoints.Values )
			{
				writer.Write( rl.map );
				writer.Write( rl.location );
				writer.Write( rl.mobile );
				writer.Write( rl.returnToPoint );
			}
			writer.Write( jailor );
			writer.Write( reason );
		}

		public virtual void Deserialize( GenericReader reader )
		{
			int imax = 0;
			int version = reader.ReadInt();
			switch( version )
			{
				case 2:
					m_jailorAC = (AccessLevel)reader.ReadInt();
					goto case 1;
				case 1:
					freedBy = reader.ReadString().Trim();
					goto case 0;
				case 0:
					m_name = reader.ReadString().Trim();
					m_releaseTime = reader.ReadDateTime();

					imax = reader.ReadEncodedInt();
					for( int i = 0; i < imax; i++ )
					{
						releaseLoc rl = new releaseLoc();
						rl.map = reader.ReadMap();
						rl.location = reader.ReadPoint3D();
						rl.mobile = reader.ReadInt();
						rl.returnToPoint = reader.ReadBool();
						releasePoints.Add( rl.mobile, rl );
					}
					jailor = reader.ReadString().Trim();
					reason = reader.ReadString().Trim();
					break;
				default:
					break;
			}
			//Console.WriteLine( "Loaded Jail object:{0} releases:{1}", m_name, imax );
		}

		public override string ToString()
		{
			return Name;
		}

		public void TimerRelease()
		{
			if( m_releaseTime <= DateTime.Now )
				release();
			else
				Console.WriteLine( "JailSystem: A Jail Timer fired but the timer was incorrect so the release was not honored." );
		}

		public void forceRelease( Mobile releasor )
		{
			try
			{
				if( m_jailorAC > releasor.AccessLevel )
				{
					releasor.SendLocalizedMessage( 1061637 );
					return;
				}
			}
			catch( Exception err )
			{
				Console.WriteLine( "{0}: access level error, resume release-{1}", JSName, err );
			}
			freedBy = releasor.Name + " (At:" + DateTime.Now + ")";
			try
			{
				if( autoReleasor != null )
					autoReleasor.Stop();
			}
			catch
			{
			}
			m_releaseTime = DateTime.Now.Subtract( new TimeSpan( 1, 0, 0, 0, 0 ) );
			release();
		}

		public void release( NetState ns )
		{
			try
			{
				if( autoReleasor != null )
				{
					if( autoReleasor.Running )
						autoReleasor.Stop();
					autoReleasor = null;
				}
				try
				{
					if( !( ns.Mobile.Region is Jail ) )
						return;
					ns.Mobile.SendLocalizedMessage( 501659 );
				}
				catch( Exception err )
				{
					Console.WriteLine( "{0}: {1} Mobile not released", JSName, err );
					return;
				}
				releaseLoc rl;
				try
				{
					rl = releasePoints[ns.Mobile.Serial.Value];
				}
				catch
				{
					rl = new releaseLoc();
					rl.mobile = ns.Mobile.Serial.Value;
					releasePoints.Add( ns.Mobile.Serial.Value, rl );
				}
				if( rl.release( freedBy ) )
					releasePoints.Remove( ns.Mobile.Serial.Value );
			}
			catch( Exception err )
			{
				Console.WriteLine( "{0}: {1}", JSName, err );
			}
			if( releasePoints.Count == 0 )
			{
				Console.WriteLine( "Jailing removed for account {0}", Name );
				try
				{
					list.Remove( ID );
				}
				catch
				{
				}
			}
		}

		public void ban( Mobile from )
		{
			try
			{
				Prisoner.Comments.Add( new AccountComment( JSName + "-jailed", string.Format( "{0} banned this account on {1}.", from.Name, DateTime.Now ) ) );
				Prisoner.Banned = true;
				CommandLogging.WriteLine( from, "{0} {1} {3} account {2}", from.AccessLevel, CommandLogging.Format( from ), Prisoner.Username, Prisoner.Banned ? "banning" : "unbanning" );
				list.Remove( ID );
			}
			catch
			{
				from.SendMessage( "Banning Failed.  If you are trying to remove the jailing release the person, or use 'killjail {0}'", ID );
				from.SendMessage( "Using killjail on an unbanned account can cause problems for that account." );
			}
		}

		public static void killJailing( int tID )
		{
			list.Remove( tID );
		}

		public void release()
		{
			try
			{
				if( !( autoReleasor == null ) )
					if( autoReleasor.Running )
						autoReleasor.Stop();
					//autoReleasor=null;
			}
			catch( Exception err )
			{
				Console.WriteLine( "{0}: auto releasor not found-{1}", JSName, err );
			}
			try
			{
				verifyMobs();
			}
			catch( Exception err )
			{
				Console.WriteLine( "{0}: Verify Mobiles failed-{1}", JSName, err );
			}
			try
			{
				foreach( NetState ns in NetState.Instances )
					if( ( (Account)ns.Account ).Username == m_name )
						release( ns );
			}
			catch( Exception err )
			{
				Console.WriteLine( "{0}: Release failed-{1} **The most common occurance of this is when an account has been deleted while in jail ***Use the adminjail command to cycle through the jailings and automaticly remove them.", JSName, err );
			}
			if( releasePoints.Count == 0 )
				try
				{
					list.Remove( ID );
					Console.WriteLine( "Jailing removed for account {0}", Name );
				}
				catch
				{
				}
		}

		public void verifyMobs()
		{
			ArrayList temp = new ArrayList();
			foreach( releaseLoc r in releasePoints.Values )
				try
				{
					Mobile m = World.FindMobile( r.mobile );
					if( m == null )
						temp.Add( r );
				}
				catch
				{
					temp.Add( r );
				}
			foreach( releaseLoc r in temp )
				releasePoints.Remove( r.mobile );
		}

		public void killJail()
		{
			if( Prisoner == null )
				m_jailings.Remove( m_id );
		}

		public void lockupMobile( Mobile m )
		{
			lockupMobile( m, true );
		}

		public void lockupMobile( Mobile m, bool useFootWear )
		{
			if( !releasePoints.ContainsKey( m.Serial.Value ) )
				releasePoints.Add( m.Serial.Value, new releaseLoc( m.Location, m.Map, m.Serial.Value, returnToPoint ) );
			m.SendMessage( "While in jail, you can say \"{0}\" at any time to check your jail status", statusCommand );
			m.SendMessage( "You can say \"{0}\" at any time to check the time according to the server", timeCommand );
			if( useOOCFilter )
				m.SendMessage( "You can say \"{0}\" at any time to see the list of words marked as being out of character", ooclistCommand );
			if( m.Region is Jail )
				return;
			//if they are already in jail there is no need to do this
			Point3D cell;
			cell = cells[( ( new Random() ).Next( 0, cells.Count - 1 ) )];
			if( ( useSmokingFootGear ) && ( useFootWear ) )
				new SmokingBoots( m );
			m.Location = cell;
			m.Map = jailMap;
		}

		public void StartTimer()
		{
			if( autoReleasor != null )
			{
				if( autoReleasor.Running )
					autoReleasor.Stop();
				autoReleasor = null;
			}
			if( !jailed )
			{
				release();
				return;
			}
			autoReleasor = new JailingTimer( this );
			autoReleasor.Start();
		}

		public void resetReleaseDateOneDay()
		{
			m_releaseTime = DateTime.Now.AddDays( 1 );
			StartTimer();
		}

		public void resetReleaseDateNow()
		{
			m_releaseTime = DateTime.Now;
		}

		public void AddDays( int days )
		{
			m_releaseTime = m_releaseTime.AddDays( days );
			StartTimer();
		}

		public void AddMonths( int months )
		{
			m_releaseTime = m_releaseTime.AddMonths( months );
			StartTimer();
		}

		public void AddHours( int hours )
		{
			m_releaseTime = m_releaseTime.AddHours( hours );
			StartTimer();
		}

		public void AddMinutes( int minutes )
		{
			m_releaseTime = m_releaseTime.AddMinutes( minutes );
			StartTimer();
		}

		public void subtractDays( int days )
		{
			removeTime( days, 0, 0 );
		}

		public void subtractHours( int hours )
		{
			removeTime( 0, hours, 0 );
		}

		public void subtractMinutes( int minutes )
		{
			removeTime( 0, 0, minutes );
		}

		public void removeTime( int days, int hours, int minutes )
		{
			m_releaseTime = m_releaseTime.Subtract( new TimeSpan( days, hours, minutes, 0, 0 ) );
			StartTimer();
		}

		public static void oocWarn( Mobile m, string s )
		{
			Account acct = m.Account as Account;

			if( acct == null )
				return;

			if( !warns.ContainsKey( acct.Username ) )
				warns.Add( acct.Username, new Dictionary<DateTime, string>() );

			Dictionary<DateTime, string> w = warns[acct.Username];

			if( w == null )
			{
				Jail( m, false, "Going OOC, warning system was unable to issue a warning and jailed you.", true, "OOC Jailor" );
				return;
			}

			DateTime k = DateTime.Now;
			int i = 0;
			while( w.ContainsKey( k ) )
			{
				k = k.Subtract( new TimeSpan( 0, 0, 1 ) );
				if( i > 10 )
					continue;
				i++;
			}
			if( i <= 10 )
			{
				w.Add( k, s );
				new warnRemover( w, k );
			}
			if( w.Count > oocwarns )
			{
				Jail( m, false, "Going OOC after repeated warnings.", true, oocJailorName );
				warns.Remove( acct.Username );
			}
			else
			{
				m.SendMessage( k.ToString() );
				m.SendMessage( "'{0}' is an out of character term.  Going ooc too much can land you in Jail.  For a list of ooc words say '{1}'", s, ooclistCommand );
			}
		}

		public static TimeSpan getJailTerm( Mobile m, bool foul )
		{
			int visits = countJailings( m, foul );
			if( foul )
				return getFoulJailTerm( visits );
			else
				return getOOCJailTerm( visits );
		}

		public static TimeSpan getOOCJailTerm( int visits )
		{
			oocJailTimes.Sort();
			if( visits >= oocJailTimes.Count )
				visits = oocJailTimes.Count - 1;
			if( visits < 0 )
				visits = 0;
			return oocJailTimes[visits];
		}

		public static TimeSpan getFoulJailTerm( int visits )
		{
			FoulMouthJailTimes.Sort();
			if( visits >= FoulMouthJailTimes.Count )
				visits = FoulMouthJailTimes.Count - 1;
			if( visits < 0 )
				visits = 0;
			return FoulMouthJailTimes[visits];
		}

		public static int countJailings( Mobile m, bool foul )
		{
			return countJailings( ( (Account)m.Account ), foul );
		}

		public static int countJailings( Account a, bool foul )
		{
			int foulCt = 0;
			int oocCt = 0;
			foreach( AccountComment note in a.Comments )
			{
				if( note.Content.IndexOf( " By:" + oocJailorName ) >= 0 )
					oocCt++;
				if( note.Content.IndexOf( " By:" + foulJailorName ) >= 0 )
					foulCt++;
			}
			if( foul )
				return foulCt;
			else
				return oocCt;
		}

		public static void Initialize()
		{
			EventSink.Login += OnLoginJail;
			EventSink.Speech += EventSink_Speech;
			CommandSystem.Register( "unjail", AccessLevel.GameMaster, unjail_OnCommand );
			CommandSystem.Register( "release", AccessLevel.GameMaster, unjail_OnCommand );
			CommandSystem.Register( "jail", AccessLevel.Counselor, jail_OnCommand );
			CommandSystem.Register( "review", AccessLevel.Counselor, review_OnCommand );
			CommandSystem.Register( "warn", AccessLevel.Counselor, warn_OnCommand );
			CommandSystem.Register( "macro", AccessLevel.Counselor, macroCheck_OnCommand );
			CommandSystem.Register( "adminJail", AccessLevel.Administrator, adminJail_OnCommand );
			CommandSystem.Register( "killJail", AccessLevel.Administrator, KillJail_OnCommand );
			CommandSystem.Register( "cage", AccessLevel.Administrator, cage_OnCommand );

			//PacketHandler ph = PacketHandlers.GetHandler( 0x02 );
			//ph.ThrottleCallback = new ThrottlePacketCallback( MovementThrottle_Callback );
		}

		public static void getcontextmenus( Mobile from, Mobile player, ArrayList list )
		{
			if( from.AccessLevel >= AccessLevel.Counselor )
				list.Add( new JailEntry( from, player ) );
			if( from.AccessLevel >= AccessLevel.GameMaster )
				list.Add( new unJailEntry( from, player ) );
			if( from.AccessLevel >= AccessLevel.Counselor )
				list.Add( new ReviewEntry( from, player ) );
			if( from.AccessLevel >= AccessLevel.Counselor )
				list.Add( new macroerEntry( from, player ) );
		}

		[Usage( "AdminJail" )]
		[Description( "Displays the jail sentence gump." )]
		public static void adminJail_OnCommand( CommandEventArgs e )
		{
			if( e.Mobile is PlayerMobile )
				e.Mobile.SendGump( new JailAdminGump() );
		}

		[Usage( "killJail <ID>" )]
		[Description( "Deletes the jailing with the specified ID.  Used only to recover from deadly errors" )]
		public static void KillJail_OnCommand( CommandEventArgs e )
		{
			try
			{
				int tID = Convert.ToInt32( e.ArgString.Trim() );
				killJailing( tID );
			}
			catch( Exception err )
			{
				e.Mobile.SendMessage( "Kill jailing failed: {0}", err.ToString() );
			}
		}

		[Usage( "UnJail" )]
		[Description( "Releases the selected player from jail." )]
		public static void unjail_OnCommand( CommandEventArgs e )
		{
			if( e.Mobile is PlayerMobile )
			{
				e.Mobile.Target = new JailTarget( true );
				e.Mobile.SendLocalizedMessage( 3000218 );
			}
		}

		[Usage( "Cage" )]
		[Description( "places a cage around the target." )]
		public static void cage_OnCommand( CommandEventArgs e )
		{
			if( e.Mobile is PlayerMobile )
			{
				e.Mobile.Target = new CageTarget();
				e.Mobile.SendMessage( "Who would you like to cage?" );
			}
		}

		[Usage( "Macro" )]
		[Description( "Issues a macroing check dialog." )]
		public static void macroCheck_OnCommand( CommandEventArgs e )
		{
			if( e.Mobile is PlayerMobile )
			{
				e.Mobile.Target = new MacroTarget();
				e.Mobile.SendLocalizedMessage( 3000218 );
			}
		}

		[Usage( "Jail" )]
		[Description( "Places the selected player in jail." )]
		public static void jail_OnCommand( CommandEventArgs e )
		{
			if( e.Mobile is PlayerMobile )
			{
				e.Mobile.Target = new JailTarget( false );
				e.Mobile.SendLocalizedMessage( 3000218 );
			}
		}

		[Usage( "Warn" )]
		[Description( "Issues a warning to the player." )]
		public static void warn_OnCommand( CommandEventArgs e )
		{
			if( e.Mobile is PlayerMobile )
			{
				e.Mobile.Target = new WarnTarget();
				e.Mobile.SendLocalizedMessage( 3000218 );
			}
		}

		[Usage( "Review" )]
		[Description( "Reviews the jailings, GM notes and warnings of the selected player." )]
		public static void review_OnCommand( CommandEventArgs e )
		{
			if( e.Mobile is PlayerMobile )
			{
				e.Mobile.Target = new WarnTarget( false );
				e.Mobile.SendLocalizedMessage( 3000218 );
			}
		}

		public static void macroTest( Mobile from, Mobile badBoy )
		{
			if( badBoy.NetState == null )
			{
				from.SendMessage( "They are not online." );
				return;
			}
			badBoy.SendGump( new UnattendedMacroGump( from, badBoy ) );
		}

		public static void newJailingFromGMandPlayer( Mobile from, Mobile m )
		{
			JailSystem js = fromMobile( m );
			if( js == null )
				js = new JailSystem();
			else if( js.jailed )
			{
				from.SendMessage( "{0} is already jailed", m.Name );
				js.lockupMobile( m );
				return;
			}
			js.resetReleaseDateOneDay();
			js.lockupMobile( m );
			js.jailor = from.Name;
			if( m.Equals( from ) )
				m.SendLocalizedMessage( 503315 );
			else
				m.SendLocalizedMessage( 503316 );
			from.SendGump( ( new JailGump( js, from, m, 0, "", "" ) ) );
		}

		public class warnRemover : Timer
		{
			public DateTime key;
			public Dictionary<DateTime, string> issuedWarnings;

			public warnRemover( Dictionary<DateTime, string> w, DateTime k ) : base( new TimeSpan( 1, 0, 0, 0 ) )
			{
				key = k;
				issuedWarnings = w;
				Start();
			}

			protected override void OnTick()
			{
				if( issuedWarnings.ContainsKey( key ) )
					issuedWarnings.Remove( key );
			}
		}

		public class releaseLoc
		{
			public Point3D location;
			public Map map;
			public int mobile;
			public bool returnToPoint;

			public releaseLoc()
			{
				returnToPoint = false;
			}

			public releaseLoc( bool rel2JailPoint )
			{
				returnToPoint = rel2JailPoint;
			}

			public releaseLoc( Point3D loc, Map m, int mob, bool rel2JailPoint )
			{
				location = loc;
				map = m;
				mobile = mob;
				returnToPoint = rel2JailPoint;
			}

			public bool release( string releasor )
			{
				Mobile m = World.FindMobile( mobile );
				if( m == null )
				{
					Console.WriteLine( "release location error, Mobile not found." );
					return false;
				}
				if( !returnToPoint )
				{
					//not returning to the jailing point so rewrites this release point info
					if( SingleFacetOnly )
						map = defaultReleaseFacet;
					else if( m.Kills >= 5 )
						map = Map.Felucca;
					else
						map = Map.Trammel;
					location = defaultRelease[( new Random() ).Next( 0, defaultRelease.Count - 1 )];
				}
				try
				{
					m.Location = location;
					m.Map = map;
				}
				catch
				{
				}
				if( m.Region is Jail )
				{
					try
					{
						( (Account)m.Account ).Comments.Add( new AccountComment( JSName, releasor + "'s release Failed for " + m.Name + "(" + ( (Account)m.Account ).Username + ") at " + DateTime.Now + " to " + location + "(" + map + ")" ) );
					}
					catch
					{
					}
					return false;
				}
				else
				{
					try
					{
						( (Account)m.Account ).Comments.Add( new AccountComment( JSName, releasor + " released " + m.Name + "(" + ( (Account)m.Account ).Username + ") at " + DateTime.Now + " to " + location + "(" + map + ")" ) );
					}
					catch
					{
					}
					return true;
				}
			}
		}

		public class JailingTimer : Timer
		{
			public JailSystem Prisoner;

			public JailingTimer( JailSystem js ) : base( js.ReleaseDate.Subtract( DateTime.Now ) )
			{
				Prisoner = js;
			}

			protected override void OnTick()
			{
				Prisoner.TimerRelease();
			}
		}
	}
}