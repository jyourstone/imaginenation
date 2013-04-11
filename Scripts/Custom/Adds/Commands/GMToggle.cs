namespace Server.Commands
{
    public class GMToggleCommand
    {
        public static AccessLevel Minimum_Accesslevel = AccessLevel.Counselor;

        public static void Initialize()
        {
            //Exluded from help menu in Handlers.cs - line 1298
            CommandSystem.Register("GM", AccessLevel.Player, Execute);
        }

        public static void Execute(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            //Code from Maka's PrivSwitcher
            if (from.Account.AccessLevel >= Minimum_Accesslevel)
                if (from.AccessLevel >= Minimum_Accesslevel)
                    from.AccessLevel = AccessLevel.Player;
                else
                    from.AccessLevel = from.Account.AccessLevel;
        }
    }
}