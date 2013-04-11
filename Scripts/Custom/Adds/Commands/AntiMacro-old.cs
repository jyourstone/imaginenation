using Server.AntiMacro;
using Server.Commands;
using Server.Commands.Generic;
using Server.Mobiles;

namespace Server.Scripts.Custom.Adds.Commands
{
    #region

    

    #endregion

    public class OldAntiMacroCommand : BaseCommand
    {
        public OldAntiMacroCommand()
        {
            AccessLevel = AccessLevel.GameMaster;
            Supports = CommandSupport.AllMobiles;
            Commands = new[] {"OldAntiMacro", "oam"};
            ObjectTypes = ObjectTypes.Mobiles;
            Usage = string.Empty;
            Description = "Sends the old anti macro gump to a player";
        }

        public static void Initialize()
        {
            TargetCommands.Register(new OldAntiMacroCommand());
        }

        public override void Execute(CommandEventArgs e, object obj)
        {
            PlayerMobile target = obj as PlayerMobile;

            if (target != null)
                target.SendGump(new OldAntiMacroGump(target));
            else
                e.Mobile.SendAsciiMessage("This only works on players!");
        }
    }
}