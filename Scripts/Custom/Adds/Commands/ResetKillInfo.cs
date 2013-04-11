using Server.Commands;
using Server.Scripts.Custom.Adds.System.KillInfo;

namespace Server.Scripts.Custom.Adds.Commands
{
    #region

    

    #endregion

    public class ResetKillInfo
    {
        public static void Initialize()
        {
            CommandSystem.Register("ResetKillInfo", AccessLevel.Seer, Execute);
        }

        [Usage("ResetKillInfo")]
        private static void Execute(CommandEventArgs e)
        {
            KillInfo.ResetResults();
        }
    }
}