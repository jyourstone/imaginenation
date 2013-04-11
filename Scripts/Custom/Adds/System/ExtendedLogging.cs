using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Server.Misc
{
	public class LogRecorder
	{
        private static readonly List<string> speechlist = new List<string>();
        //private static readonly List<string> guildchatlist = new List<string>();
        //private static readonly List<string> partychatlist = new List<string>();
        //private static readonly List<string> alliancechatlist = new List<string>();

		public static void Initialize()
		{
			EventSink.Login += EventSink_Login;
			EventSink.Logout += EventSink_Logout;
			EventSink.Speech += OnSpeech;
            //EventSink.GuildChat += OnGuildChat;
            //EventSink.PartyChat += OnPartyChat;
		    //EventSink.AllianceChat += OnAllianceChat;
		    EventSink.WorldSave += OnSave;
            //EventSink.Crashed += OnCrashed;

			if( !Directory.Exists( "Logs" ) )
				Directory.CreateDirectory( "Logs" );
			if( !Directory.Exists( "Logs/LoginLogout" ) )
				Directory.CreateDirectory( "Logs/LoginLogout" );
			if( !Directory.Exists( "Logs/Speech" ) )
				Directory.CreateDirectory( "Logs/Speech" );
            if (!Directory.Exists("Logs/Speech/RegularSpeech"))
                Directory.CreateDirectory("Logs/Speech/RegularSpeech");
            /*if (!Directory.Exists("Logs/Speech/GuildChat"))
                Directory.CreateDirectory("Logs/Speech/GuildChat");
            if (!Directory.Exists("Logs/Speech/PartyChat"))
                Directory.CreateDirectory("Logs/Speech/PartyChat");
            if (!Directory.Exists("Logs/Speech/AllianceChat"))
                Directory.CreateDirectory("Logs/Speech/AllianceChat");*/
		}

		private static void EventSink_Login( LoginEventArgs args )
		{
			Mobile m = args.Mobile;
			try
			{
				Stream fileStream = File.Open( "Logs/LoginLogout/" + m.Account + ".log", FileMode.Append, FileAccess.Write, FileShare.ReadWrite );
				StreamWriter writeAdapter = new StreamWriter( fileStream );
		                writeAdapter.WriteLine(String.Format("{0} {1} logged in as {2} on {3}", m.NetState, m.Account, m.Name, DateTime.Now));
				writeAdapter.Close();
			}
			catch
			{
				Console.WriteLine( "Record Error... {0} Login", m.Account );
			}
		}

		private static void EventSink_Logout( LogoutEventArgs args )
		{
			Mobile m = args.Mobile;
			try
			{
				Stream fileStream = File.Open( "Logs/LoginLogout/" + m.Account + ".log", FileMode.Append, FileAccess.Write, FileShare.ReadWrite );
				StreamWriter writeAdapter = new StreamWriter( fileStream );

				writeAdapter.WriteLine( String.Format( "Disconnected: {0} logged out as {1} on {2}", m.Account, m.Name, DateTime.Now ) );
				writeAdapter.Close();
			}
			catch
			{
				Console.WriteLine( "Record Error... {0} Logout", m.Account );
			}
		}
        
		public static void OnSpeech( SpeechEventArgs e )
		{
		    string speech = e.Speech;
		    string name = e.Mobile.Name;
		    string account = e.Mobile.Account != null ? e.Mobile.Account.ToString() : "[No account]";
		    string region = e.Mobile.Region.Name;
		    string datetime = DateTime.Now.ToString();

            string text = string.Format("{0} [{1}] [{2}]: <{3}> {4}", datetime, account, region, name, speech );

            speechlist.Add(text);
		}
        /*
        public static void OnGuildChat(GuildChatEventArgs e)
        {
            string speech = e.Speech;
            string name = e.Mobile.Name;
            string account = e.Mobile.Account != null ? e.Mobile.Account.ToString() : "[No account]";
            string region = e.Mobile.Region.Name;
            string guild = e.Mobile.Guild.Abbreviation == "none" ? e.Mobile.Guild.Name : e.Mobile.Guild.Abbreviation;
            string datetime = DateTime.Now.ToString();

            string text = string.Format("{0} [{1}] [{2}]: Guild[{3}] <{4}> {5}", datetime, account, region, guild, name, speech);

            guildchatlist.Add(text);

            if (guildchatlist.Count > 1000)
                SaveGuildChat();
        }
        
        public static void OnPartyChat(PartyChatEventArgs e)
        {
            string speech = e.Speech;
            string from = e.From.Name;
            string to = e.To == null ? "" : e.To.Name;
            string account = e.From.Account != null ? e.From.Account.ToString() : "[No account]";
            string region = e.From.Region.Name;
            string datetime = DateTime.Now.ToString();

            string text;

            if (string.IsNullOrEmpty(to))
                text = string.Format("{0} [{1}] [{2}]: Party[{3}] {4}", datetime, account, region, from, speech);
            else
                text = string.Format("{0} [{1}] [{2}]: Party[{3} -> {4}] {5}", datetime, account, region, from, to, speech);
            
            partychatlist.Add(text);

            if (partychatlist.Count > 1000)
                SavePartyChat();
        }
        
        public static void OnAllianceChat(AllianceChatEventArgs e)
        {
            string speech = e.Speech;
            string name = e.Mobile.Name;
            string account = e.Mobile.Account != null ? e.Mobile.Account.ToString() : "[No account]";
            string region = e.Mobile.Region.Name;
            string guild = e.Mobile.Guild.Abbreviation == "none" ? e.Mobile.Guild.Name : e.Mobile.Guild.Abbreviation;
            string datetime = DateTime.Now.ToString();

            string text = string.Format("{0} [{1}] [{2}]: Guild[{3}] <{4}> {5}", datetime, account, region, guild, name, speech);

            alliancechatlist.Add(text);

            if (alliancechatlist.Count > 1000)
                SaveAllianceChat();
        }
        */
        public static void OnSave(WorldSaveEventArgs w)
        {
            SaveSpeech();
            //SaveGuildChat();
            //SavePartyChat();
            //SaveAllianceChat();
        }
        /*
        public static void OnCrashed(CrashedEventArgs c)
        {
            try
            {
                SaveSpeech();
                //SaveGuildChat();
                //SavePartyChat();
                //SaveAllianceChat();
            }
            catch
            {
            }
        }
        */
        public static void SaveSpeech()
        {
            try
            {
                Stream fileStream = File.Open("Logs/Speech/RegularSpeech/" + DateTime.Now.ToLongDateString() + ".log", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter writeAdapter = new StreamWriter(fileStream);

                for (int i = 0; i < speechlist.Count; ++i)
                    writeAdapter.WriteLine(speechlist[i]);

                writeAdapter.Close();
                speechlist.Clear();
            }
            catch
            {
                Console.WriteLine("Record Error..." );
            }
        }
        /*
        public static void SaveGuildChat()
        {
            try
            {
                Stream fileStream = File.Open("Logs/Speech/GuildChat/" + DateTime.Now.ToLongDateString() + ".log", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter writeAdapter = new StreamWriter(fileStream);

                for (int i = 0; i < guildchatlist.Count; ++i)
                        writeAdapter.WriteLine(guildchatlist[i]);

                writeAdapter.Close();
                guildchatlist.Clear();
            }
            catch
            {
                Console.WriteLine("Record Error...");
            }
        }
        
        public static void SavePartyChat()
        {
            try
            {
                Stream fileStream = File.Open("Logs/Speech/PartyChat/" + DateTime.Now.ToLongDateString() + ".log", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter writeAdapter = new StreamWriter(fileStream);

                for (int i = 0; i < partychatlist.Count; ++i)
                    writeAdapter.WriteLine(partychatlist[i]);

                writeAdapter.Close();
                partychatlist.Clear();
            }
            catch
            {
                Console.WriteLine("Record Error...");
            }
        }
        
        public static void SaveAllianceChat()
        {
            try
            {
                Stream fileStream = File.Open("Logs/Speech/AllianceChat/" + DateTime.Now.ToLongDateString() + ".log", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter writeAdapter = new StreamWriter(fileStream);

                for (int i = 0; i < alliancechatlist.Count; ++i)
                    writeAdapter.WriteLine(alliancechatlist[i]);

                writeAdapter.Close();
                alliancechatlist.Clear();
            }
            catch
            {
                Console.WriteLine("Record Error...");
            }
        }
        */
	}
}