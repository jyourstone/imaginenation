using Server.Commands;
using Server.Targeting;

namespace Server.Custom.PvpToolkit.Tournament
{
    public class KickDM
    {
        public static void Initialize()
        {
            CommandSystem.Register("KickTour", AccessLevel.Counselor, KickDM_Command);
        }

        [Usage("KickTour")]
        [Description("Kick a player from tournament.")]
        private static void KickDM_Command(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, KickTour_Target);
            e.Mobile.SendMessage("Target the player to be kicked from tournament.");
        }

        private static void KickTour_Target(Mobile from, object o)
        {
            if (o is Mobile)
            {
                Mobile mob = (Mobile)o;
                TournamentCore.TryLeaveTournament(mob, true);
            }
            else
            {
                from.SendMessage("That is not a player.");
            }
        }
    }
}