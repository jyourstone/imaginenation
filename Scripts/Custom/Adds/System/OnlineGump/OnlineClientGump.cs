using Server.Network;

namespace Server.Gumps
{
	public class OnlineClientGump : Gump
	{
		private readonly NetState m_State;

		public override void OnResponse( NetState state, RelayInfo info )
		{
			if ( m_State == null || state == null )
				return;

			Mobile focus = m_State.Mobile;
			Mobile from = state.Mobile;

		    switch ( info.ButtonID )
		    {
		        case 1: // Tell
		            if (focus == null)
		            {
		                from.SendMessage("That character is no longer online.");
		                return;
		            }
		            if (focus.Deleted)
		            {
		                from.SendMessage("That character no longer exists.");
		                return;
		            }
		            if (from != focus && focus.Hidden && from.AccessLevel < focus.AccessLevel)
		            {
		                from.SendMessage("That character is no longer visible.");
		                return;
		            }

		            TextRelay text = info.GetTextEntry(0);

		            if (text != null && !focus.HasFilter)
		            {
		                focus.SendMessage(134, "[{0}] {1}", from.Name, text.Text);
		                from.SendAsciiMessage("You sent a message to {0}", focus.Name);
		            }
                    else
                        from.SendAsciiMessage("Your message to {0} was not sent", focus.Name);

		            from.SendGump(new OnlineClientGump(from, m_State));

		            break;
		    }
		}

	    public OnlineClientGump( Mobile from, NetState state ) : base( 30, 20 )
		{
			if ( from == null || state == null )
				return;

			m_State = state;

            AddPage(0);
            AddBackground(68, 134, 352, 153, 9200);
            AddBackground(76, 161, 335, 119, 3000);
            AddTextEntry(81, 166, 325, 110, 0, 0, @"", 300);
            AddButton(373, 253, 1155, 1153, 1, GumpButtonType.Reply, 0);
            AddButton(337, 253, 1152, 1150, 0, GumpButtonType.Reply, 0);
            AddHtml(157, 140, 210, 25, @"<FONT SIZE=5>Enter your message below:</FONT>", false, false);  
		}
	}
}