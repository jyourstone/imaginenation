using System.Collections;
using Server.Accounting;
using Server.Jailing;
using Server.Network;

namespace Server.Gumps
{
	public class JailReviewGump : Gump
	{
		private Mobile badBoy;
		private Mobile jailor;
		//private string m_reason = "Breaking Shard Rules";
		private ArrayList m_warn;
		private int m_id;
		private bool displayReleases = false;

		public JailReviewGump( Mobile from, Mobile m ) : base( 1, 30 )
		{
			buildit( from, m, 0, null, "" );
		}

		public JailReviewGump( Mobile from, Mobile m, int id, ArrayList warnings ) : base( 1, 30 )
		{
			buildit( from, m, id, warnings, "" );
		}

		public JailReviewGump( Mobile from, Mobile m, int id, ArrayList warnings, string note ) : base( 1, 30 )
		{
			buildit( from, m, id, warnings, note );
		}

		public JailReviewGump( Mobile from, Mobile m, int id, ArrayList warnings, bool showRelease ) : base( 1, 30 )
		{
			buildit( from, m, id, warnings, "", true, showRelease );
		}

		public JailReviewGump( Mobile from, Mobile m, int id, ArrayList warnings, string note, bool showRelease ) : base( 1, 30 )
		{
            // Handle exception here
			buildit( from, m, id, warnings, note, true, showRelease );
		}

		public void buildit( Mobile from, Mobile m, int id, ArrayList warnings, string aNote )
		{
			buildit( from, m, id, warnings, aNote, true, false );
		}

		public void buildit( Mobile from, Mobile m, int id, ArrayList warnings, string aNote, bool tGo, bool showRelease )
		{
            try
            {
                displayReleases = showRelease;
                from.CloseGump(typeof(JailReviewGump));
                jailor = from;
                badBoy = m;
                m_id = id;
                Closable = true;
                Dragable = true;
                AddPage(0);
                AddBackground(0, 0, 326, 230, 5054);
                AddLabel(12, 4, 200, "Reviewing: " + badBoy.Name + " (" + ((Account)badBoy.Account).Username + ")");
                if (tGo)
                {
                    AddLabel(300, 17, 200, "GO");
                    AddButton(280, 20, 2223, 2224, 2, GumpButtonType.Reply, 0);
                }
                AddLabel(12, 200, 200, "Note");
                AddBackground(42, 198, 268, 24, 0x2486);
                AddTextEntry(46, 200, 250, 20, 200, 0, aNote);
                //add button
                AddButton(70, 150, 2460, 2461, 1, GumpButtonType.Reply, 0);
                //previous button
                AddButton(120, 150, 2466, 2467, 20, GumpButtonType.Reply, 0);
                //next Button
                AddButton(200, 150, 2469, 2470, 21, GumpButtonType.Reply, 0);
                //release toggle
                AddButton(115, 167, displayReleases ? 2154 : 2151, displayReleases ? 2151 : 2154, 22, GumpButtonType.Reply, 0);
                AddLabel(147, 171, 200, "Show Releases");
                if (warnings == null)
                {
                    m_warn = new ArrayList();
                    foreach (AccountComment note in ((Account)m.Account).Comments)
                        if ((note.AddedBy == JailSystem.JSName + "-warning") || (note.AddedBy == JailSystem.JSName + "-jailed") || (note.AddedBy == JailSystem.JSName + "-note") || ((displayReleases) && ((note.AddedBy == JailSystem.JSName))))
                            m_warn.Add(note);
                    m_id = m_warn.Count - 1;
                }
                else
                    m_warn = warnings;
                AddImageTiled(9, 36, 308, 110, 2624);
                AddAlphaRegion(9, 36, 308, 110);
                string temp = "No prior warnings.";
                if (m_warn.Count > 0)
                {
                    if (m_id < 0)
                        m_id = m_warn.Count - 1;
                    if (m_id >= m_warn.Count)
                        m_id = 0;
                    temp = ((AccountComment)m_warn[m_id]).Content;
                    if (((AccountComment)m_warn[m_id]).AddedBy == JailSystem.JSName + "-warning")
                        AddLabel(12, 40, 53, "Warned");
                    else if (((AccountComment)m_warn[m_id]).AddedBy == JailSystem.JSName + "-jailed")
                        AddLabel(12, 40, 38, "Jailed");
                    else if (((AccountComment)m_warn[m_id]).AddedBy == JailSystem.JSName + "-note")
                        AddLabel(12, 40, 2, "Note");
                    else
                        AddLabel(12, 40, 2, "Release");
                    AddLabel(60, 40, 200, "Issued: " + ((AccountComment)m_warn[m_id]).LastModified);
                }
                else
                    //no prior warning
                    m_id = -1;
                AddLabel(12, 60, 200, "Event " + (m_id + 1) + " of " + m_warn.Count);
                //AddLabel( 12, 230, 200, temp );
                AddHtml(12, 80, 300, 62, temp, true, true);
            }
            catch (System.NullReferenceException)
            {
                // Malik: Couldn't find enough information on this in the crash logs to find out a reason why it crashed.
                // Need more information on this and on the conditions why it happens. Exception handler here until then.
                from.SendAsciiMessage("Error creating review gump. Please report this to devs and give as much details as possible on what you were doing!");
            }
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			switch( info.ButtonID )
			{
				case 22:
					displayReleases = ( !displayReleases );
					from.SendGump( new JailReviewGump( from, badBoy, 0, null, info.GetTextEntry( 0 ).Text, displayReleases ) );
					break;
				case 20:
					//previous button
					m_id--;
					if( m_id < 0 )
						m_id = m_warn.Count - 1;
					from.SendGump( new JailReviewGump( from, badBoy, m_id, m_warn, info.GetTextEntry( 0 ).Text, displayReleases ) );
					break;
				case 21:
					//next button
					m_id++;
					if( m_id >= m_warn.Count )
						m_id = 0;
					from.SendGump( new JailReviewGump( from, badBoy, m_id, m_warn, info.GetTextEntry( 0 ).Text, displayReleases ) );
					break;
					//reason buttons
				case 2:
					from.SendGump( new JailReviewGump( from, badBoy, m_id, m_warn, info.GetTextEntry( 0 ).Text, displayReleases ) );
					from.Hidden = true;
					from.Location = badBoy.Location;
					from.Map = badBoy.Map;
					break;
				case 1:
					if( info.GetTextEntry( 0 ).Text != "note added" )
					{
						( (Account)badBoy.Account ).Comments.Add( new AccountComment( JailSystem.JSName + "-note", info.GetTextEntry( 0 ).Text + " by: " + from.Name ) );
						from.SendGump( new JailReviewGump( from, badBoy, 0, null, "note added", displayReleases ) );
					}
					else
						from.SendGump( new JailReviewGump( from, badBoy, 0, null, "", displayReleases ) );
					break;
				default:
					break;
			}
		}
	}
}