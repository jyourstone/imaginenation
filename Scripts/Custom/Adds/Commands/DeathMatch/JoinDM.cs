#define ENABLED

#region

using Server.Commands;
using Server.Custom.PvpToolkit;
using Server.Regions;
using Server.Spells;

#endregion

namespace Server.Scripts.Custom.Adds.Commands.DeathMatch
{
    public class JoinDM
    {
        public static void Initialize()
        {
#if ENABLED
                CommandSystem.Register("JoinDM", AccessLevel.Player, Execute);
#endif
        }

        [Usage("JoinDM")]
        [Description("Join an existing death match.")]
        private static void Execute(CommandEventArgs e)
        {
            Mobile m = e.Mobile;
            CustomRegion cR = m.Region as CustomRegion;

            if (cR != null && !cR.Controller.CanUseStuckMenu)
            {
                m.SendAsciiMessage("You cannot join the deathmatch from where you are right now");
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
                m.SendAsciiMessage("You must be fully healed to join the deatmatch!");
                return;
            }

            PvpCore.TryJoinDM(m);
        }
    }
}