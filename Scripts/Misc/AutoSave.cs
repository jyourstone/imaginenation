/* AutoSave.cs - Modified by Alari (alarihyena@gmail.com)
8:31 PM Thursday, July 05, 2007
For use with RunUO 2.0 (SVN 186)

Features:  (All features are optional.)

- Creates protected Archive backups in a seperate directory.

- Displays your custom AutoSave gump before a save, and closes it after.

- Checks for enough free disk space before Archive and Save.


Installation:

Rename RunUO\Scripts\Misc\AutoSave.cs to AutoSave.cs.original

Copy this AutoSave.cs to RunUO\Scripts\Misc\AutoSave.cs
 for compatibility with SVN Update - it will merge automatically.


What is an Archive backup? :

A copy of your Saves directory, made before the Save is allowed to
occur. Unlike the Automatic backups, these Archive backups are not
overwritten by the AutoSave.cs script.

The default is to make one Archive backup every day. You can have the
script save more than one Archive backup per day, see the section on
GetArchiveTimeStamp() below for details.


Why is this useful? :

Imagine a hacker or disgruntled GM wipes your shard, then does '[save'
4-5 times to overwrite the Automatic backups, and you don't have any
other backup. Absurd? This has actually happened, a post on the RunUO
forum inspired me to make this modification.

PLEASE NOTE: This is NOT a replacement for doing regular REAL backups!
Please please PLEASE consider investing in a real backup device! CD/DVD
burners from NewEgg.com are cheap. (No, I don't get a kickback for
mentioning them. ;) However even if you do have a way of doing real
backups, this feature can help save your shard in case a backup was
missed or is corrupt.


What you need to do to use this:

These instructions list each setting that you can customize to change
how this AutoSave works. This header explains the basics of the
settings, and more details may be found at the locations in the file
where the settings are found. (Also given in this header)

I tried to move as many possible settings to the top of the script for
easier customization, but there are a LOT of possible changes that you
can make to the way this script works.


Settings:

At the beginning of the script:

Comment out the USESGUMP define if you do not wish to use a save gump.

Comment out the CHECKFREEDISK define if you do not wish to check for
 enough free disk space before doing a save. (Or possibly if you use
 Mono... Can someone using Mono please test?)

m_Delay - The delay between world saves.

m_Warning - How long before a world save are players warned?

m_Archive - The location to store Archive backups, or null to disable.

m_Notify - The minimum access level notified when the disk is full. 

NoIOHour - No saves will occur during this hour of the day.


Additional modifications:


In Save():

Change the name of 'SaveGump' if yours is called something else.

If you wish your save gump to remain up even after the save has
finished, comment out the code in the second USESGUMP section. 


In m_Backups:
Modify to taste for number of Automatic backups to keep.
Note: Automatic backups are not Archive backups!
Archive backups are not overwritten. Automatic backups are.


In GetArchiveTimeStamp():
This controls how Archives are created. There are two versions of
GetArchiveTimeStamp() included, the default one will Archive once a day
while the other (commented out) example one will Archive twice a day.



------------------------------------------------------------------------
Monitor your server disk space usage and clean out your Archive
directory of old archived backups that you no longer want or need to
keep.

These backups will not be automatically overwritten like Automatic
backups are.

And let me tell you nothing sucks like running out of disk space on your
server. (Even with the code that checks for free disk space...)
------------------------------------------------------------------------



That's it! At least, those are the main things to modify in order to
configure this script. You may want to tweak other parts of the script
to customize it.



Note on free disk space checking:

The way the free disk space checks are done, the server should stop
making Archive backups before it stops saving. (Higher 'fudge factor'
setting.) This was done so that there would hopefully be time for admins
to clean out the disk on the server and still have current saves and
automatic backups.

Also, the code to check for free space when doing an Automatic backup
has been commented out. This is because Automatic backups should be
overwriting themselves, thus not really ever increasing the amount of
disk space being used. You can remove the comment marks from the code
to check disk space before an Automatic backup.


*/


// --- SETTINGS ---


// Uncomment this define to use a save gump. If your save gump is not
// called "SaveGump", search and replace that with the name of yours.

//#define USESGUMP


// Comment out this define if you do not wish to check for free disk
// space before a save, or possibly if you are running your server on
// Mono (Can anyone test if kernel32.dll calls work in Mono?)

#define CHECKFREEDISK


// --- END SETTINGS ---


using System;
using System.IO;
using Server.Commands;

namespace Server.Misc
{
    public class AutoSave : Timer
    {

        // --- SETTINGS ---

        //   Sets the delay between world saves.
        private static readonly TimeSpan m_Delay = TimeSpan.FromMinutes(30.0);


        // Use one of the following m_Warning settings:

        //   Sets the warning before a save to 30 seconds.
        //private static TimeSpan m_Warning = TimeSpan.FromSeconds(15.0);

        //   Disables the warning before a save.
         private static TimeSpan m_Warning = TimeSpan.Zero;


        // Use one of the following m_Archive settings:
        /*
        I recommend changing the following m_Archive setting to point somewhere
        outside the RunUO directory, and making whatever adjustments are
        necessary to the number of Automatic backups kept (in m_Backups, below
        Save() ) so that your RunUO directory will always fit nicely onto a
        CD-R/RW... Of course, if you use DVD-Rs or tape or some such like that,
        you don't have to worry about that. ;)
        */
        //   Saves Archive backups to RunUO\Backups\Archive
        private static readonly string m_Archive = Path.Combine( Core.BaseDirectory, "Backups\\Archive" );

        //   Saves Archive backups to a specific location.
        //private static string m_Archive = @"C:\Backups";

        //   Disables Archive backups.
        // private static string m_Archive = null;


        // This setting controls the minimum access level to be notified
        // if the disk is full.
#if CHECKFREEDISK
        private const AccessLevel m_Notify = AccessLevel.GameMaster;
#endif

        /*
        NoIOHour - If datetime.now.hour == NoIOHour, then the saves will be
        skipped. This is so that backup processes have a window of time where
        there will not be any disk IO done by AutoSave. 24 hour format. (0=12am,
        12=12pm, 23=11pm) It's like happy hour for backup programs, except
        without any booze. :>  -1 to disable. (or a fictional hour. =)
        */
        //   No saving will be allowed during this hour.
        //public static int NoIOHour = 8;	// 8 am

        //   '-1' disables
         public static int NoIOHour = -1;	// disabled


        // --- END SETTINGS ---


        /*
        This is about as far down as you should have to modify for casual
        adjustment of settings. Of course, feel free to change anything below
        here to suit your needs.
        */



        public static void Initialize()
        {
            new AutoSave().Start();
            CommandSystem.Register("SetSaves", AccessLevel.Administrator, SetSaves_OnCommand);
        }

        private static bool m_SavesEnabled = true;

        public static bool SavesEnabled
        {
            get { return m_SavesEnabled; }
            set { m_SavesEnabled = value; }
        }

        [Usage("SetSaves <true | false>")]
        [Description("Enables or disables automatic shard saving.")]
        public static void SetSaves_OnCommand(CommandEventArgs e)
        {
            if (e.Length == 1)
            {
                m_SavesEnabled = e.GetBoolean(0);
                e.Mobile.SendMessage("Saves have been {0}.", m_SavesEnabled ? "enabled" : "disabled");
            }
            else
            {
                e.Mobile.SendMessage("Format: SetSaves <true | false>");
                e.Mobile.SendMessage("Saves are currently: {0}", m_SavesEnabled ? "enabled" : "disabled");
            }
        }

        public AutoSave()
            : base(m_Delay - m_Warning, m_Delay)
        {
            Priority = TimerPriority.OneMinute;
        }

        protected override void OnTick()
        {
            if (!m_SavesEnabled || AutoRestart.Restarting)
                return;

            if (m_Warning == TimeSpan.Zero)
            {
                Save(true);
            }
            else
            {
                int s = (int)m_Warning.TotalSeconds;
                int m = s / 60;
                s %= 60;

                if (m > 0 && s > 0)
                    World.Broadcast(0x3b2, true, "The world will save in {0} minute{1} and {2} second{3}.", m, m != 1 ? "s" : "", s, s != 1 ? "s" : "");
                else if (m > 0)
                    World.Broadcast(0x3b2, true, "The world will save in {0} minute{1}.", m, m != 1 ? "s" : "");
                else
                    World.Broadcast(0x3b2, true, "The world will save in {0} second{1}.", s, s != 1 ? "s" : "");

                DelayCall(m_Warning, new TimerCallback(Save));
            }
        }

        public static void Save()
        {
            Save(true);
        }

        public static void Save(bool permitBackgroundWrite)
        {
            if (AutoRestart.Restarting)
                return;

            World.WaitForWriteCompletion();

            // should world.save be allowed to occur at this time?
            if (NoIOHour != -1)
            {
                DateTime now = DateTime.Now;

                if (now.Hour == NoIOHour)
                {
                    // DEBUG
                    Console.WriteLine("AutoSave.cs : NoIOHour : Saving not allowed during hour {0}.", NoIOHour);

                    return;
                }
            }

#if USESGUMP
            ArrayList mobs = new ArrayList(World.Mobiles.Values);
            foreach (Mobile m in mobs)
            {
                if (m != null && m is PlayerMobile)
                    m.SendGump(new SaveGump());   // change to the name of your custom save gump if you have one
            }
#endif

#if CHECKFREEDISK
            string[] rootdriv = Core.BaseDirectory.Split('\\');
            string rootdrive = rootdriv[0] + "\\";

            // get size of Saves directory.
            long savesize = directorySize(Path.Combine(Core.BaseDirectory, "Saves"));

            long freeBytesAvailable = 0;
            bool worked = false;


            DriveInfo r = new DriveInfo(rootdrive);
            if (r.IsReady)
            {
                freeBytesAvailable = r.AvailableFreeSpace;

                worked = true;
            }


            long totalBytes = 0;

            if (worked)
                totalBytes = freeBytesAvailable;
            else
                totalBytes = 1000000000;  // 1 gig

            // DEBUG
            //Console.WriteLine( "\n\nrootdrive: {0}", rootdrive );
            //Console.WriteLine( "freeBytesAvailable: {0:n}", freeBytesAvailable );
            //Console.WriteLine( "savesize: {0:n}\n\n", savesize );


            // m_Archive may point to a different drive than the one RunUO is on...


            long atotalBytes = 0;
            string arootdrive = "";

            if (m_Archive != null)
            {
                string[] arootdriv = m_Archive.Split('\\');
                arootdrive = arootdriv[0] + "\\";

                if (arootdrive == rootdrive)
                {
                    atotalBytes = totalBytes;
                }
                else
                {
                    worked = false;  // reset

                    DriveInfo a = new DriveInfo(rootdrive);
                    if (a.IsReady)
                    {
                        freeBytesAvailable = a.AvailableFreeSpace;

                        worked = true;
                    }

                    if (worked)
                        atotalBytes = freeBytesAvailable;
                    else
                        atotalBytes = 1000000000;  // 1 gig - fudge it

                }

                // DEBUG
                //Console.WriteLine( "arootdrive: {0}", arootdrive );
                //Console.WriteLine( "atotalBytes: {0:n}\n\n", atotalBytes );
            }


            // ARCHIVE
            // * 4 for fudge factor
            if (m_Archive != null && atotalBytes > (savesize * 4))
            {
#endif
                if (m_Archive != null)
                {
                    try { Archive(); }
                    catch { Console.WriteLine("AutoSave.cs : Save() : try Archive() :\n Archive attempt failed!"); }
                }
#if CHECKFREEDISK
            }
            else if (m_Archive != null)
            {
                Console.WriteLine("Error! Not enough free disk space left on {0} to Archive!", arootdrive);
                CommandHandlers.BroadcastMessage(m_Notify, 33, String.Format("[AutoSave.cs] Error! Not enough free disk space left on {0} to Archive!", arootdrive));
            }


            /*			
            Something tells me I should not be checking for free disk space during a
            backup, since in the majority of cases, backups will be overwriting
            existing backups and thus not using any additional space. Also, removing
            the old Saves directory may be necessary, and it is Backup() which does
            that. Leaving this feature commented out for now. There is a try command
            wrapping it, so that should prevent a crash...
            */
            // BACKUP			
            // * for fudge factor
            //	if ( totalBytes > ( savesize * 2 ) )
            //	{
#endif
            try { Backup(); }
            catch { Console.WriteLine("AutoSave.cs : Save() : try Backup() :\n Backup attempt failed!"); }
#if CHECKFREEDISK
            //	}
            //	else
            //	{
            //		Console.WriteLine( "Error! Not enough free disk space left on {0} to Backup!", rootdrive );
            //		CommandHandlers.BroadcastMessage( m_Notify, 33, String.Format( "[AutoSave.cs] Error! Not enough free disk space left on {0} to Backup!", rootdrive ) );
            //	}


            // SAVE
            // * 2 for fudge factor
            if (totalBytes > (savesize * 2))
            {
#endif
            World.Save(true, permitBackgroundWrite);
#if CHECKFREEDISK
            }
            else
            {
                Console.WriteLine("Error! Not enough free disk space left on {0} to Save!", rootdrive);
                CommandHandlers.BroadcastMessage(m_Notify, 33, String.Format("[AutoSave.cs] Error! Not enough free disk space left on {0} to Save!", rootdrive));
            }
#endif

#if USESGUMP
            // Comment these lines out to keep your save
            // gump up after the save has completed.
            foreach (Mobile m in mobs)
            {
                if (m != null && m is PlayerMobile)
                    m.CloseGump(typeof(SaveGump));
            }
#endif
        }

        // Original Automatic backup settings.
        // Each line equals one backup.
        /*
        private static string[] m_Backups = new string[]
        {
            "Third Backup",
            "Second Backup",
            "Most Recent"
        };
        */

        // I prefer to retain more Automatic backups.
        // Edit this section to fit your needs.

        private static readonly string[] m_Backups = new[]
		{
            "48 Backup - Oldest",
            "47 Backup",
            "46 Backup",
            "45 Backup",
            "44 Backup",
            "43 Backup",
            "42 Backup",
            "41 Backup",
            "40 Backup",
            "39 Backup",
            "38 Backup",
            "37 Backup",
            "36 Backup",
            "35 Backup",
            "34 Backup",
            "33 Backup",
            "32 Backup",
            "31 Backup",
            "30 Backup",
            "29 Backup",
            "28 Backup",
            "27 Backup",
            "26 Backup",
			"25 Backup",
			"24 Backup",
			"23 Backup",
			"22 Backup",
			"21 Backup",
			"20 Backup",
			"19 Backup",
			"18 Backup",
			"17 Backup",
			"16 Backup",
			"15 Backup",
			"14 Backup",
			"13 Backup",
			"12 Backup",
			"11 Backup",
			"10 Backup",
			"09 Backup",
			"08 Backup",
			"07 Backup",
			"06 Backup",
			"05 Backup",
			"04 Backup",
			"03 Backup",
			"02 Backup",
			"01 Backup - Most Recent"
		};

        private static void Backup()
        {
            if (m_Backups.Length == 0)
                return;

            string root = Path.Combine(Core.BaseDirectory, "Backups\\Automatic");

            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            string[] existing = Directory.GetDirectories(root);

            for (int i = 0; i < m_Backups.Length; ++i)
            {
                DirectoryInfo dir = Match(existing, m_Backups[i]);

                if (dir == null)
                    continue;

                if (i > 0)
                {
                    string timeStamp = FindTimeStamp(dir.Name);

                    if (timeStamp != null)
                    {
                        try { dir.MoveTo(FormatDirectory(root, m_Backups[i - 1], timeStamp)); }
                        catch { Console.WriteLine("AutoSave.cs : Backup() : try dir.MoveTo failed!"); }
                    }
                }
                else
                {
                    try { dir.Delete(true); }
                    catch { Console.WriteLine("AutoSave.cs : Backup() : try dir.Delete failed!"); }
                }
            }

            string saves = Path.Combine(Core.BaseDirectory, "Saves");

            if (Directory.Exists(saves))
                Directory.Move(saves, FormatDirectory(root, m_Backups[m_Backups.Length - 1], GetTimeStamp()));
        }

        private static DirectoryInfo Match(string[] paths, string match)
        {
            for (int i = 0; i < paths.Length; ++i)
            {
                DirectoryInfo info = new DirectoryInfo(paths[i]);

                if (info.Name.StartsWith(match))
                    return info;
            }

            return null;
        }

        private static string FormatDirectory(string root, string name, string timeStamp)
        {
            return Path.Combine(root, String.Format("{0} ({1})", name, timeStamp));
        }

        private static string FindTimeStamp(string input)
        {
            int start = input.IndexOf('(');

            if (start >= 0)
            {
                int end = input.IndexOf(')', ++start);

                if (end >= start)
                    return input.Substring(start, end - start);
            }

            return null;
        }

        private static string GetTimeStamp()
        {
            DateTime now = DateTime.Now;

            return String.Format("{0}-{1}-{2} {3}-{4:D2}-{5:D2}",
            now.Day,
            now.Month,
            now.Year,
            now.Hour,
            now.Minute,
            now.Second
            );
        }

        /*
        GetArchiveTimeStamp controls how many Archive backups are made. The
        basics of how it works is that Archive will copy the Saves directory to
        a directory of the name returned by GetArchiveTimeStamp(), unless that
        directory already exists. So changing what GetArchiveTimeStamp() returns
        will change how many Archive backups are created.
        */

        // One Archive backup per day
        private static string GetArchiveTimeStamp()
        {
            DateTime now = DateTime.Now;

            return String.Format("{0}-{1}-{2}",
            now.Year, now.Month, now.Day);
        }

        /*
            // you could use this instead if you want to make
            //  two Archive backups per day.
		
            // Two Archive backups per day
            private static string GetArchiveTimeStamp()
            {
                DateTime now = DateTime.Now;
			
                string ampm;
                if ( now.Hour < 12 )
                    ampm = "AM";
                else
                    ampm = "PM";
			
                return String.Format( "{0}-{1}-{2}-{3}",
                now.Year, now.Month, now.Day, ampm);
            }
        */

        public static void Archive()
        {
            string root = m_Archive;

            if (root == null)   // shouldn't get this far but what the heck...
                return;

            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);


            string archiveDir = Path.Combine(root, GetArchiveTimeStamp());

            string saves = Path.Combine(Core.BaseDirectory, "Saves");


            if (!Directory.Exists(archiveDir))
            {
                if (Directory.Exists(saves))
                {
                    try { copyDirectory(saves, archiveDir); }
                    catch
                    {
                        Console.WriteLine("AutoSave.cs : Archive() : try copyDirectory :\n Archive copyDirectory failed!");
                    }
                }
                else
                {
                    Console.WriteLine("AutoSave.cs : Archive() : Directory.Exists( saves ) :\n Error! Archive did not occur because Saves directory did not exist!");
                }
            }
        }


        // copyDirectory from:
        // http://dotnetjunkies.com/WebLog/sajay/archive/2004/12/03/34763.aspx

        // Copy directory structure recursively
        public static void copyDirectory(string src, string dst)
        {
            String[] files;
            if (dst[dst.Length - 1] != Path.DirectorySeparatorChar)
                dst += Path.DirectorySeparatorChar;
            if (!Directory.Exists(dst)) Directory.CreateDirectory(dst);
            files = Directory.GetFileSystemEntries(src);
            foreach (string element in files)
            {
                // Sub directories
                if (Directory.Exists(element))
                    copyDirectory(element, dst + Path.GetFileName(element));
                // files in directory
                else
                    File.Copy(element, dst + Path.GetFileName(element), true);
            }
        }

        // returns real size of directory in bytes.
        public static long directorySize(string dir)
        {
            long size = 0;
            DirectoryInfo directory = new DirectoryInfo(dir);

            foreach (FileInfo file in directory.GetFiles())
            {
                size = size + file.Length;
            }

            foreach (DirectoryInfo subdir in directory.GetDirectories())
            {
                size = size + directorySize(subdir.FullName);
            }

            return (size);
        }
    }
}
