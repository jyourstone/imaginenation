using Server.Commands;

namespace Server.Custom.PvpToolkit.Tournament
{
    public class LeaveTour
    {
        public static void Initialize()
        {
            CommandSystem.Register("LeaveTour", AccessLevel.Player, Execute);
        }

        [Usage("LeaveTour")]
        [Description("Exits the tournament, without the chance for a prize.")]
        private static void Execute(CommandEventArgs e)
        {
            TournamentCore.TryLeaveTournament(e.Mobile, false);
        }
    }
}