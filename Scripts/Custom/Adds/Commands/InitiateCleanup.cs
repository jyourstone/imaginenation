using Server.Commands;
using Server.Scripts.Custom.Adds.System;

namespace Server.Scripts.Custom.Adds.Commands
{
    #region

    

    #endregion

    public class InitiateCleanup
    {
        public static void Initialize()
        {
            CommandSystem.Register("IC", AccessLevel.Administrator, Execute);
        }

        [Usage("ResetKillInfo")]
        private static void Execute(CommandEventArgs e)
        {
            SystemCleanup.DoCleanup();
        }
    }
}