using Server.Gumps;
using Server.Jailing;
using Server.Network;

namespace Server.ContextMenus
{
	public class ReviewEntry : ContextMenuEntry
	{
		private readonly Mobile m_gm;
		private readonly Mobile m_player;

		public ReviewEntry( Mobile gm, Mobile player ) : base( 10004, 200 )
		{
			m_gm = gm;
			m_player = player;
		}

		public override void OnClick()
		{
			m_gm.SendGump( new JailReviewGump( m_gm, m_player, 0, null ) );
		}
	}

	public class JailEntry : ContextMenuEntry
	{
		private readonly Mobile m_gm;
		private readonly Mobile m_player;

		public JailEntry( Mobile gm, Mobile player ) : base( 5008, 200 )
		{
			m_gm = gm;
			m_player = player;
		}

		public override void OnClick()
		{
			JailSystem.newJailingFromGMandPlayer( m_gm, m_player );
			//this is where we jail them
		}
	}

	public class unJailEntry : ContextMenuEntry
	{
		private readonly Mobile m_gm;
		private readonly Mobile m_player;
		private readonly JailSystem js;

		public unJailEntry( Mobile gm, Mobile player ) : base( 5135, 200 )
		{
			m_gm = gm;
			m_player = player;
			js = JailSystem.fromMobile( m_player );
			if( js == null )
				Flags |= CMEFlags.Disabled;
			else if( !js.jailed )
				Flags |= CMEFlags.Disabled;
		}

		public override void OnClick()
		{
			if( js == null )
				m_gm.SendMessage( "They are not jailed" );
			else if( js.jailed )
				js.forceRelease( m_gm );
		}
	}

	public class macroerEntry : ContextMenuEntry
	{
		private readonly Mobile m_gm;
		private readonly Mobile m_player;
		private readonly JailSystem js;

		public macroerEntry( Mobile gm, Mobile player ) : base( 394, 200 )
		{
			m_gm = gm;
			m_player = player;
			js = JailSystem.fromMobile( m_player );
			if( js == null )
			{
			}
			else if( js.jailed )
				Flags |= CMEFlags.Disabled;
		}

		public override void OnClick()
		{
			if( js == null )
				JailSystem.macroTest( m_gm, m_player );
			else if( !js.jailed )
				JailSystem.macroTest( m_gm, m_player );
			else
				m_gm.SendMessage( "They are already in jail." );
		}
	}
}