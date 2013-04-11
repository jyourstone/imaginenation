using System;
using System.Collections;
using Server.Gumps;

namespace Server.Commands.Generic
{
	public enum ObjectTypes
	{
		Both,
		Items,
		Mobiles,
		All
	}

	public abstract class BaseCommand
	{
	    public bool ListOptimized { get; set; }

	    public string[] Commands { get; set; }

	    public string Usage { get; set; }

	    public string Description { get; set; }

	    public AccessLevel AccessLevel { get; set; }

	    public ObjectTypes ObjectTypes { get; set; }

	    public CommandSupport Supports { get; set; }

	    public BaseCommand()
		{
			m_Responses = new ArrayList();
			m_Failures = new ArrayList();
		}

		public static bool IsAccessible( Mobile from, object obj )
		{
			if ( from.AccessLevel >= AccessLevel.Administrator || obj == null )
				return true;

			Mobile mob;

			if ( obj is Mobile )
				mob = (Mobile)obj;
			else if ( obj is Item )
				mob = ((Item)obj).RootParent as Mobile;
			else
				mob = null;

			if ( mob == null || mob == from || from.AccessLevel > mob.AccessLevel )
				return true;

			return false;
		}

		public virtual void ExecuteList( CommandEventArgs e, ArrayList list )
		{
			for ( int i = 0; i < list.Count; ++i )
				Execute( e, list[i] );
		}

		public virtual void Execute( CommandEventArgs e, object obj )
		{
		}

		public virtual bool ValidateArgs( BaseCommandImplementor impl, CommandEventArgs e )
		{
			return true;
		}

		private readonly ArrayList m_Responses;
	    private readonly ArrayList m_Failures;

	    private class MessageEntry
		{
			public readonly string m_Message;
			public int m_Count;

			public MessageEntry( string message )
			{
				m_Message = message;
				m_Count = 1;
			}

			public override string ToString()
			{
				if ( m_Count > 1 )
					return String.Format( "{0} ({1})", m_Message, m_Count );

				return m_Message;
			}
		}

		public void AddResponse( string message )
		{
			for ( int i = 0; i < m_Responses.Count; ++i )
			{
				MessageEntry entry = (MessageEntry)m_Responses[i];

				if ( entry.m_Message == message )
				{
					++entry.m_Count;
					return;
				}
			}

			if ( m_Responses.Count == 10 )
				return;

			m_Responses.Add( new MessageEntry( message ) );
		}

		public void AddResponse( Gump gump )
		{
			m_Responses.Add( gump );
		}

		public void LogFailure( string message )
		{
			for ( int i = 0; i < m_Failures.Count; ++i )
			{
				MessageEntry entry = (MessageEntry)m_Failures[i];

				if ( entry.m_Message == message )
				{
					++entry.m_Count;
					return;
				}
			}

			if ( m_Failures.Count == 10 )
				return;

			m_Failures.Add( new MessageEntry( message ) );
		}

		public void Flush( Mobile from, bool flushToLog )
		{
			if ( m_Responses.Count > 0 )
			{
				for ( int i = 0; i < m_Responses.Count; ++i )
				{
					object obj = m_Responses[i];

					if ( obj is MessageEntry )
					{
						from.SendMessage( (obj).ToString() );

						if ( flushToLog )
							CommandLogging.WriteLine( from, (obj).ToString() );
					}
					else if ( obj is Gump )
					{
						from.SendGump( (Gump) obj );
					}
				}
			}
			else
			{
				for ( int i = 0; i < m_Failures.Count; ++i )
					from.SendMessage( (m_Failures[i]).ToString() );
			}

			m_Responses.Clear();
			m_Failures.Clear();
		}
	}
}