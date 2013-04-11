using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Server.Accounting;
using Server.Engines.Help;
using Server.Network;

namespace Server.Misc
{
	internal class ServerConsole
	{
		private static PageEntry[] m_List;
		public static bool paging;

		#region Event Handlers
		public static void EventSink_ServerStarted()
		{
			ThreadPool.QueueUserWorkItem( new WaitCallback( ConsoleListen ) );
			Console.WriteLine( "CC initialized..." );
		}

		#endregion

		public static void Initialize()
		{
			EventSink.ServerStarted += new ServerStartedEventHandler( EventSink_ServerStarted );
		}

		public static void ConsoleListen( Object stateInfo )
		{
			if( !paging )
			{
				string input = Console.ReadLine();
				Next( input );
			}
		}

		public static void PageResp( object obj )
		{
			paging = true;
			object[] objects = (object[])obj;
			int w = (int)objects[0];
			int pag = (int)objects[1];
			int paG;
			if( w == 1 )
			{
				up:
				try
				{
					paG = Convert.ToInt32( Console.ReadLine() );
				}
				catch
				{
					Console.WriteLine( "Thats not a number,try again." );
					goto up;
				}
				Console.WriteLine( "Type your response" );
				object[] ob = new object[] { 2, paG };
				ThreadPool.QueueUserWorkItem( new WaitCallback( PageResp ), ob );
			}
			else
			{
				string resp = Console.ReadLine();
				ArrayList list = PageQueue.List;
				m_List = (PageEntry[])list.ToArray( typeof( PageEntry ) );
				if( m_List.Length > 0 )
					if( pag > m_List.Length )
						Console.WriteLine( "Error: Not a valid page number" );
					else
						for( int i = 0; i < m_List.Length; ++i )
						{
							PageEntry e = m_List[i];
							if( i == pag )
							{
								e.Sender.SendGump( new MessageSentGump( e.Sender, "Admin", resp, true ) );
								PageQueue.Remove( e );
								Console.WriteLine( "Message Sent..." );
							}
						}
				else
					Console.WriteLine( "There are no pages to display." );
			}
			paging = false;
			ThreadPool.QueueUserWorkItem( new WaitCallback( ConsoleListen ) );
		}

		public static void BroadcastMessage( AccessLevel ac, int hue, string message )
		{
			foreach( NetState state in NetState.Instances )
			{
				Mobile m = state.Mobile;
				if( m != null && m.AccessLevel >= ac )
					m.SendMessage( hue, message );
			}
		}

		public static void Next( string nput )
		{
			string input = nput.ToLower();
			if( input.StartsWith( "bc" ) )
			{
				string imput = input.Replace( "bc", "" );
				BroadcastMessage( AccessLevel.Player, 0x35, String.Format( "[Admin] {0}", imput ) );
				Console.WriteLine( "Players will see: {0}", imput );
			}
			else if( input.StartsWith( "sc" ) )
			{
				string imput = input.Replace( "staff", "" );
				BroadcastMessage( AccessLevel.Counselor, 0x32, String.Format( "[Admin] {0}", imput ) );
				Console.WriteLine( "Staff will see: {0}", imput );
			}
			else if( input.StartsWith( "ban" ) )
			{
				string imput = input.Replace( "ban", "" );
				List<NetState> states = NetState.Instances;
				if( states.Count == 0 )
					Console.WriteLine( "There are no players online." );
				for( int i = 0; i < states.Count; ++i )
				{
					Account a = states[i].Account as Account;
					if( a == null )
						continue;
					Mobile m = states[i].Mobile;
					if( m == null )
						continue;
					string innput = imput.ToLower();
					if( m.Name.ToLower() == innput.Trim() )
					{
						NetState m_ns = m.NetState;
						Console.WriteLine( "Mobile name: '{0}' Account name: '{1}'", m.Name, a.Username );
						a.Banned = true;
						m_ns.Dispose();
						Console.WriteLine( "Banning complete." );
					}
				}
			}
			else if( input.StartsWith( "kick" ) )
			{
				string imput = input.Replace( "kick", "" );
				List<NetState> states = NetState.Instances;
				if( states.Count == 0 )
					Console.WriteLine( "There are no players online." );
				for( int i = 0; i < states.Count; ++i )
				{
					Account a = states[i].Account as Account;
					if( a == null )
						continue;
					Mobile m = states[i].Mobile;
					if( m == null )
						continue;
					string innput = imput.ToLower();
					if( m.Name.ToLower() == innput.Trim() )
					{
						NetState m_ns = m.NetState;
						Console.WriteLine( "Mobile name: '{0}' Account name: '{1}'", m.Name, a.Username );
						m_ns.Dispose();
						Console.WriteLine( "Kicking complete." );
					}
				}
			}
			else
				switch( input.Trim() )
				{
					case "shutdown nosave":
						Core.Process.Kill();
						break;
					case "restart nosave":
						Process.Start( Core.ExePath, Core.Arguments );
						Core.Process.Kill();
						break;
					case "online":
					{
						List<NetState> states = NetState.Instances;
						if( states.Count == 0 )
							Console.WriteLine( "There are no users online at this time." );
						for( int i = 0; i < states.Count; ++i )
						{
							Account a = states[i].Account as Account;
							if( a == null )
								continue;
							Mobile m = states[i].Mobile;
							if( m != null )
								Console.WriteLine( "- Account: {0}, Name: {1}, IP: {2}", a.Username, m.Name, states[i] );
						}
						break;
					}
					case "pages":
					{
						paging = true;
						ArrayList list = PageQueue.List;
						PageEntry e;
						for( int i = 0; i < list.Count; )
						{
							e = (PageEntry)list[i];
							if( e.Sender.Deleted || e.Sender.NetState == null )
							{
								e.AddResponse( e.Sender, "[Logout]" );
								PageQueue.Remove( e );
							}
							else
								++i;
						}
						m_List = (PageEntry[])list.ToArray( typeof( PageEntry ) );
						if( m_List.Length > 0 )
						{
							for( int i = 0; i < m_List.Length; ++i )
							{
								e = m_List[i];
								string type = PageQueue.GetPageTypeName( e.Type );
								Console.WriteLine( "--------------Page Number: " + i + " --------------------" );
								Console.WriteLine( "Player   :" + e.Sender.Name );
								Console.WriteLine( "Catagory :" + type );
								Console.WriteLine( "Message  :" + e.Message );
							}
							Console.WriteLine( "Type the number of the page to respond to." );
							object[] oj = new object[] { 1, 2 };
							ThreadPool.QueueUserWorkItem( new WaitCallback( PageResp ), oj );
						}
						else
						{
							Console.WriteLine( "No pages to display." );
							paging = false;
						}
						break;
					}
					case "help":
					case "list": //Credit to HomeDaddy for this wonderful list!
					default:
					{
						Console.WriteLine( " " );
						Console.WriteLine( "Commands:" );
						Console.WriteLine( "shutdown nosave - Shuts down the server without saving." );
						Console.WriteLine( "restart nosave  - Restarts the server without saving." );
						Console.WriteLine( "online          - Shows a list of every person online:" );
						Console.WriteLine( "                      Account, Char Name, IP." );
						Console.WriteLine( "bc <message>    - Type this command and your message after it. It will then be" );
						Console.WriteLine( "                      sent to all players." );
						Console.WriteLine( "sc <message>    - Type this command and your message after it.It will then be " );
						Console.WriteLine( "                      sent to all staff." );
						Console.WriteLine( "pages           - Shows all the pages in the page queue,you type the page" );
						Console.WriteLine( "                      number ,then you type your response to the player." );
						Console.WriteLine( "ban <playername>- Kicks and bans the users account." );
						Console.WriteLine( "kick <playername>- Kicks the user." );
						Console.WriteLine( "list or help    - Shows this list." );
						Console.WriteLine( " " );
						break;
					}
				}
			if( !paging )
				ThreadPool.QueueUserWorkItem( new WaitCallback( ConsoleListen ) );
		}
	}
}