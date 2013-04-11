using Server.Guilds;
using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands.Generic
{
    public class ViewGuildCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("ViewGuild", AccessLevel.GameMaster, ViewGuild_OnCommand);
        }

        [Usage("ViewGuild")]
        [Description("Displays guild props of the targeted player. This menu also contains options to go to guildstone and open guildstone menu.")]
        public static void ViewGuild_OnCommand(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, ViewGuild_OnTarget);
        }

        public static void ViewGuild_OnTarget(Mobile from, object targeted)
        {
            Mobile m_From = from;
            PlayerMobile target = targeted as PlayerMobile;

            if (target != null)
            {
                if (target.Guild != null && !target.Guild.Disbanded && target.Guild is Guild)
                    m_From.SendGump(new ViewGuildGump(m_From, target.Guild as Guild));
                else
                    m_From.SendAsciiMessage("That player is not in a guild.");
            }
            else
                m_From.SendAsciiMessage("This only works on players!");
        }
    }
}