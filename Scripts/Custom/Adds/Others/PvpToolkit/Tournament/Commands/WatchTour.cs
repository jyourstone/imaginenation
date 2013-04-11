using Server.Commands;
using Server.Regions;
using Server.Spells;

namespace Server.Custom.PvpToolkit.Tournament
{
    public class WatchTour
    {
        public static void Initialize()
        {
            CommandSystem.Register("WatchTour", AccessLevel.Player, Execute);
        }

        [Usage("WatchTour")]
        [Description("Watch an existing automated tournament.")]
        private static void Execute(CommandEventArgs e)
        {
            CustomRegion cR = e.Mobile.Region as CustomRegion;
            Mobile m = e.Mobile;

            if (cR != null && !cR.Controller.CanUseStuckMenu)
            {
                m.SendAsciiMessage("You cannot watch the tournament from where you are right now");
                return;
            }
            if (m.Region.IsPartOf(typeof(Jail)))
            {
                m.SendLocalizedMessage(1114345, "", 0x35); // You'll need a better jailbreak plan than that!
                return;
            }
            if (SpellHelper.CheckCombat(m))
            {
                m.SendAsciiMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??
                return;
            }
            if (m.Hits < m.HitsMax)
            {
                m.SendAsciiMessage("You must be fully healed to watch the tournament!");
                return;
            }
            foreach (TournamentStone tStone in TournamentCore.TournamentStones)
            {
                if (tStone != null && tStone.Started)
                {
                    if (!TournamentCore.IsInTournament(m))
                        m.MoveToWorld(tStone.LeaveLocation, tStone.LeaveMap);
                    else
                        m.SendAsciiMessage("You are already in a tournament");
                    return;
                }
            }

            m.SendMessage("There is no automated tournament running to watch");
        }
    }
}