namespace Server.Scripts.Custom.Adds.Commands.DeathMatch
{
    using Server.Commands;
    using Server.Custom.PvpToolkit;
    using Targeting;

    public class KickDM
    {
        public static void Initialize()
        {
            CommandSystem.Register("KickDM", AccessLevel.Counselor, new CommandEventHandler(KickDM_Command));
        }

        [Usage("KickDM")]
        [Description("Kick a player from DM.")]
        private static void KickDM_Command(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(KickDM_Target));
            e.Mobile.SendMessage("Target the player to be kicked from DM match.");
        }

        private static void KickDM_Target(Mobile from, object o)
        {
            if (o is Mobile)
            {
                Mobile mob = (Mobile)o;
                PvpCore.TryLeaveDM(mob, true);
            }
            else
            {
                from.SendMessage("That is not a player.");
            }
        }
    }
}