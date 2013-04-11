using System;
using System.IO;
using Server.Accounting;

namespace Server.IN.Logging
{
	public class RewardLogging
	{
		private static StreamWriter m_Output;
		private static bool m_Enabled = true;

		public static bool Enabled
		{
			get { return m_Enabled; }
			set { m_Enabled = value; }
		}

		public static StreamWriter Output
		{
			get { return m_Output; }
		}

		public static void Initialize()
		{
			if( !Directory.Exists( "Logs" ) )
				Directory.CreateDirectory( "Logs" );

			string directory = "Logs/Rewards";

			if( !Directory.Exists( directory ) )
				Directory.CreateDirectory( directory );

			try
			{
				m_Output = new StreamWriter( Path.Combine( directory, String.Format( "{0}.log", DateTime.Now.ToLongDateString() ) ), true );

				m_Output.AutoFlush = true;

				m_Output.WriteLine( "##############################" );
				m_Output.WriteLine( "Log started on {0}", DateTime.Now );
				m_Output.WriteLine();
			}
			catch
			{
			}
		}

		public static void WriteLine( Mobile from, SkillInfo theSkill, string theName, int theHue )
		{
			m_Output.WriteLine( "{0}: Account - {1}: Character - {2} : Skill - {3} : Name - {4} : Hue - {5}", DateTime.Now, ( (Account)from.Account ).Username, from.Name, theSkill.Name, theName, theHue );
		}

		/*
		private static char[] m_NotSafe = new char[]{ '\\', '/', ':', '*', '?', '"', '<', '>', '|' };

		public static void AppendPath( ref string path, string toAppend )
		{
			path = Path.Combine( path, toAppend );

			if ( !Directory.Exists( path ) )
				Directory.CreateDirectory( path );
		}

		public static string Safe( string ip )
		{
			if ( ip == null )
				return "null";

			ip = ip.Trim();

			if ( ip.Length == 0 )
				return "empty";

			bool isSafe = true;

			for ( int i = 0; isSafe && i < m_NotSafe.Length; ++i )
				isSafe = ( ip.IndexOf( m_NotSafe[i] ) == -1 );

			if ( isSafe )
				return ip;

			System.Text.StringBuilder sb = new System.Text.StringBuilder( ip );

			for ( int i = 0; i < m_NotSafe.Length; ++i )
				sb.Replace( m_NotSafe[i], '_' );

			return sb.ToString();
		}
*/
	}
}