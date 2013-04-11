using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Server.Commands
{
    public class CommandLogging
    {
        private static StreamWriter m_Output;
        private static bool m_Enabled = true;

        private static readonly List<string> commandlist = new List<string>();
        private static readonly List<string> commandnamelist = new List<string>();
        private static readonly List<string> commandaccesslevellist = new List<string>();

        public static bool Enabled { get { return m_Enabled; } set { m_Enabled = value; } }

        public static StreamWriter Output { get { return m_Output; } }

        public static void Initialize()
        {
            EventSink.Command += EventSink_Command;
            EventSink.WorldSave += OnSave;
            EventSink.Crashed += OnCrashed;

            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");

            string directory = "Logs/Commands";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            try
            {
                m_Output = new StreamWriter(Path.Combine(directory, String.Format("{0}.log", DateTime.Now.ToLongDateString())), true) { AutoFlush = true };

                m_Output.WriteLine("##############################");
                m_Output.WriteLine("Log started on {0}", DateTime.Now);
                m_Output.WriteLine();
            }
            catch
            {
            }
        }

        public static object Format(object o)
        {
            if (o is Mobile)
            {
                Mobile m = (Mobile)o;

                if (m.Account == null)
                    return String.Format("{0} (no account)", m);
                return String.Format("{0} ('{1}')", m, m.Account.Username);
            }
            if (o is Item)
            {
                Item item = (Item)o;

                return String.Format("0x{0:X} ({1})", item.Serial.Value, item.GetType().Name);
            }

            return o;
        }

        public static void WriteLine(Mobile from, string format, params object[] args)
        {
            WriteLine(from, String.Format(format, args));
        }

        public static void WriteLine(Mobile from, string text)
        {
            //Display commands in game if .hearcommands is enabled
            HearCommands.DisplayCommand(from, text);

            if (!m_Enabled)
                return;

            string netstate = "(deleted)";

            if (from.NetState != null)
            {
                netstate = from.NetState.ToString();
            }

            string datetime = DateTime.Now.ToString();

            string msg = string.Format("{0}: {1}: {2}", datetime, netstate, text);
            string name = (from.Account == null ? from.Name : from.Account.Username);
            string accesslevel = from.AccessLevel.ToString();

            commandlist.Add(msg);
            commandnamelist.Add(name);
            commandaccesslevellist.Add(accesslevel);
        }

        private static readonly char[] m_NotSafe = new char[] { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };

        public static void AppendPath(ref string path, string toAppend)
        {
            path = Path.Combine(path, toAppend);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static string Safe(string ip)
        {
            if (ip == null)
                return "null";

            ip = ip.Trim();

            if (ip.Length == 0)
                return "empty";

            bool isSafe = true;

            for (int i = 0; isSafe && i < m_NotSafe.Length; ++i)
                isSafe = (ip.IndexOf(m_NotSafe[i]) == -1);

            if (isSafe)
                return ip;

            StringBuilder sb = new StringBuilder(ip);

            for (int i = 0; i < m_NotSafe.Length; ++i)
                sb.Replace(m_NotSafe[i], '_');

            return sb.ToString();
        }

        private static readonly string[] m_IgnoredCommands = new string[]
                                                      {
                                                          "wop",
                                                          "g",
                                                          "tele",
                                                          "opendoor",
                                                          "s",
                                                          "a"
                                                      };

        public static void EventSink_Command(CommandEventArgs e)
        {
            for (int i = 0; i < m_IgnoredCommands.Length; ++i)
            {
                if (e.Command.ToLowerInvariant() == m_IgnoredCommands[i])
                    return;
            }
            WriteLine(e.Mobile, "{0} {1} used command '{2} {3}'", e.Mobile.AccessLevel, Format(e.Mobile), e.Command, e.ArgString);
        }

        public static void LogChangeProperty(Mobile from, object o, string name, string value)
        {
            WriteLine(from, "{0} {1} set property '{2}' of {3} to '{4}'", from.AccessLevel, Format(from), name, Format(o), value);
        }

        public static void OnSave(WorldSaveEventArgs w)
        {
            SaveCommands();
        }

        public static void OnCrashed(CrashedEventArgs c)
        {
            try
            {
                SaveCommands();
            }
            catch
            {
            }
        }

        public static void SaveCommands()
        {
            if (!Enabled)
                return;

            try
            {
                for (int i = 0; i < commandlist.Count; ++i)
                {
                    m_Output.WriteLine(commandlist[i]);

                    string path = Core.BaseDirectory;

                    AppendPath(ref path, "Logs");
                    AppendPath(ref path, "Commands");
                    AppendPath(ref path, commandaccesslevellist[i]);
                    path = Path.Combine(path, String.Format("{0}.log", commandnamelist[i]));

                    using (StreamWriter sw = new StreamWriter(path, true))
                        sw.WriteLine(commandlist[i]);
                }

                commandlist.Clear();
                commandnamelist.Clear();
                commandaccesslevellist.Clear();
            }
            catch
            {
            }

        }
    }
}