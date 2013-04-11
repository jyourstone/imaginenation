using Server.Commands;
using Server.Scripts.Custom.Adds.System.KillInfo;

namespace Server.Scripts.Custom.Adds.Commands
{
    #region

    

    #endregion

    public class PrintKillInfo
    {
        public static void Initialize()
        {
            CommandSystem.Register("PrintKillInfo", AccessLevel.Seer, Execute);
        }

        [Usage("PrintKillInfo")]
        private static void Execute(CommandEventArgs e)
        {
            KillInfo.PrintResults();
        }
    }
}