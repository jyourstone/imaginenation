using Server.Commands;
using Server.Commands.Generic;
using Server.Mobiles;

namespace Server.AntiMacro
{
    #region

    

    #endregion

    public class AntiMacroCommand : BaseCommand
    {
        public AntiMacroCommand()
        {
            AccessLevel = AccessLevel.GameMaster;
            Supports = CommandSupport.AllMobiles;
            Commands = new[] {"AntiMacro", "am"};
            ObjectTypes = ObjectTypes.Mobiles;
            Usage = string.Empty;
            Description = "Sends the anti web based anti macro gump to a player";
        }

        public static void Initialize()
        {
            TargetCommands.Register(new AntiMacroCommand());
        }

        public override void Execute(CommandEventArgs e, object obj)
        {
            Mobile from = e.Mobile;
            PlayerMobile target = obj as PlayerMobile;

            if (target != null)
            {
                if (MySQLManager.SQLEnabled)
                    AntiMacroGump.SendGumpThreaded(target);
                else
                    from.SendAsciiMessage("The new anti macro gump is disabled, please use OldAntiMacro/OAM.");
            }
            else
                from.SendAsciiMessage("This only works on players!");
        }
    }
}