using System.Collections.Generic;
using Server.Guilds;
using Server.Gumps;

namespace Server.Commands
{
    public class ViewGuilds
    {
        public static void Initialize()
        {
            CommandSystem.Register("ViewGuilds", AccessLevel.GameMaster, ViewGuilds_OnCommand);
        }

        [Usage("ViewGuilds")]
        [Description("Shows a list of all existing guilds. This menu also allows you to view props of a selected guild. Optionally filters results by name or abbrevation")]
        private static void ViewGuilds_OnCommand(CommandEventArgs e)
        {
            string filter = e.ArgString;

            if (filter != null && (filter = filter.Trim()).Length == 0)
                filter = null;
            else if (filter != null)
                filter = filter.ToLower();

            Mobile m_From = e.Mobile;
            List<Guild> guilds = new List<Guild>();

            foreach (KeyValuePair<int, BaseGuild> keyValuePair in BaseGuild.List)
            {
                if (!(keyValuePair.Value is Guild))
                    continue;

                Guild g = keyValuePair.Value as Guild;

                if (g.Disbanded)
                    continue;

                if ( filter != null && ( (g.Name == null || g.Name.ToLower().IndexOf(filter) < 0) && (g.Abbreviation == null || g.Abbreviation.ToLower().IndexOf(filter) < 0) ) )
                    continue;

                guilds.Add(g);
            }

            if (guilds.Count > 0)
                m_From.SendGump(new ViewGuildsGump(m_From, guilds));
        }
    }
}