using Server.Commands;
using Server.Regions;
using Server.Spells;

namespace Server.Custom.PvpToolkit.Tournament
{
    public class JoinTour
    {
        public static void Initialize()
        {
            CommandSystem.Register("JoinTour", AccessLevel.Player, Execute);
        }

        [Usage("JoinTour")]
        [Description("Join an existing automated tournament.")]
        private static void Execute(CommandEventArgs e)
        {
            CustomRegion cR = e.Mobile.Region as CustomRegion;
            Mobile m = e.Mobile;

            if (cR != null && !cR.Controller.CanUseStuckMenu)
            {
                m.SendAsciiMessage("You cannot join an event from where you are right now");
                return;
            }

            if (m.HasTrade)
            {
                m.SendLocalizedMessage(1004041);
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

            if (m.SolidHueOverride != -1)
            {
                m.SendAsciiMessage("You cannot join an event while using a teleporter");
                return;
            }

            if (m.Hits < m.HitsMax)
            {
                m.SendAsciiMessage("You must be fully healed to join the tournament!");
                return;
            }

            TournamentCore.TryJoinTournament(m);
        }
    }
}