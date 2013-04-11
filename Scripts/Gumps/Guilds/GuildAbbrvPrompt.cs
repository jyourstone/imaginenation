using Server.Guilds;
using Server.Prompts;

namespace Server.Gumps
{
	public class GuildAbbrvPrompt : Prompt
	{
		private readonly Mobile m_Mobile;
		private readonly Guild m_Guild;

		public GuildAbbrvPrompt( Mobile m, Guild g )
		{
			m_Mobile = m;
			m_Guild = g;
		}

		public override void OnCancel( Mobile from )
		{
			if ( GuildGump.BadLeader( m_Mobile, m_Guild ) )
				return;

			GuildGump.EnsureClosed( m_Mobile );
			m_Mobile.SendGump( new GuildmasterGump( m_Mobile, m_Guild ) );
		}

		public override void OnResponse( Mobile from, string text )
		{
			if ( GuildGump.BadLeader( m_Mobile, m_Guild ) )
				return;

			text = text.Trim();

			if ( text.Length > 20 )
				text = text.Substring( 0, 20 );

			if ( text.Length > 0 )
			{
				if ( BaseGuild.FindByAbbrev( text ) != null )
				{
					m_Mobile.SendMessage( "{0} conflicts with the abbreviation of an existing guild.", text );
				}
				else
				{
                    if (text.Length > 20)
                    {
                        m_Mobile.SendMessage("Your Guild Abrevation is too long.");
                    }
                    else
                    {
                        m_Guild.Abbreviation = text;
                        m_Guild.GuildTextMessage("Your guild abbreviation has changed to: " + m_Guild.Abbreviation);
                        //m_Guild.GuildMessage(1018025, true, "2Your guild abbreviation has changed to: " + m_Guild.Abbreviation); // Your guild abbreviation has changed:

                    }
				}
			}

			GuildGump.EnsureClosed( m_Mobile );
			m_Mobile.SendGump( new GuildmasterGump( m_Mobile, m_Guild ) );
		}
	}
}