using System;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Custom.SkillBoost
{
    public class SkillBoost
    {
        public static Timer ResetValuesTimer { get; set; }
        public static DateTime StartedOn { get; set; }
        public static bool Running { get; set; }

        public static void Initialize()
        {
            CommandSystem.Register("SkillBoost", AccessLevel.Administrator, On_OnCommand);
        }

        [Usage("SkillBoost")]
        [Description("To boost skillgains type in a multiplier for each skill and for how many hours you want to boost them.")]
        private static void On_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile is PlayerMobile)
            {
                e.Mobile.CloseGump(typeof(SkillBoostGump));
                e.Mobile.SendGump(new SkillBoostGump(e.Mobile));
            }
        }

        public static void Stop()
        {
            Running = false;
        }
    }
}