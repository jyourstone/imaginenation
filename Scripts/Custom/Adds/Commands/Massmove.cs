using System;
using Server.Mobiles;

namespace Server.Commands
{
    #region

    

    #endregion

    public class MassmoveCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("Massmove", AccessLevel.Player, Massmove_OnCommand);
        }

        [Usage("Massmove")]
        [Description("Moves a set of potions in your backpack")]
        private static void Massmove_OnCommand(CommandEventArgs e)
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;

            if (pm != null)
            {
                if (pm.IsMassmoving)
                {
                    pm.IsMassmoving = false;
                    pm.SendAsciiMessage("Massmove disabled...");
                }
                else
                {
                    pm.IsMassmoving = true;
                    pm.SendAsciiMessage("Massmoving enabled. Move the potion you want to massmove.");
                    Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerStateCallback(DisableMassmove), pm);
                }
            }
        }

        public static void DisableMassmove(object state)
        {
            PlayerMobile pm = state as PlayerMobile;

            if (pm != null && pm.IsMassmoving)
            {
                pm.IsMassmoving = false;
                pm.SendAsciiMessage("Massmove disabled...");
            }
        }
    }
}