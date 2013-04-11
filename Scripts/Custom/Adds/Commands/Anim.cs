namespace Server.Scripts.Custom.Adds.Commands
{
    #region

    using Server.Commands;

    #endregion

    public class Animate
    {
        public static int animation;

        public static void Initialize()
        {
            CommandSystem.Register("Anim", AccessLevel.Counselor, Animate_OnCommand);
        }

        [Usage("Animate <int>")]
        [Description("Display the specific animation")]
        private static void Animate_OnCommand(CommandEventArgs e)
        {
            if (e.Arguments.Length >= 1)
                int.TryParse(e.Arguments[0], out animation);

            try
            {
                e.Mobile.Animate(animation, 5, 1, true, false, 0);
            }
            catch
            {
            }
        }
    }
}