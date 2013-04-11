using Server.Mobiles;

namespace Server.Commands.Generic
{
    #region

    

    #endregion

    public class StoneCommand : BaseCommand
    {
        public StoneCommand()
        {
            AccessLevel = AccessLevel.GameMaster;
            Supports = CommandSupport.AllMobiles;
            Commands = new[] {"Stone"};
            ObjectTypes = ObjectTypes.Mobiles;
            Usage = "Stone";
            Description = "Stones targeted player.";
        }

        public static void Initialize()
        {
            TargetCommands.Register(new StoneCommand());
        }

        public override void Execute(CommandEventArgs e, object obj)
        {
            Mobile from = e.Mobile;
            PlayerMobile target = obj as PlayerMobile;

            if (target != null)
            {
                if (!target.Stoned)
                {
                    target.Stoned = true;
                    target.Squelched = true;
                    target.PagingSquelched = true;
                    target.Frozen = true;
                    target.SolidHueOverride = 901;
                    target.SendAsciiMessage("You have been stoned!");
                }
                else
                    from.SendAsciiMessage("That player is already stoned.");
            }
            else
                from.SendAsciiMessage("This only works on players!");
        }
    }

    public class UnStoneCommand : BaseCommand
    {
        public UnStoneCommand()
        {
            AccessLevel = AccessLevel.GameMaster;
            Supports = CommandSupport.AllMobiles;
            Commands = new[] {"UnStone"};
            ObjectTypes = ObjectTypes.Mobiles;
            Usage = "UnStone";
            Description = "UnStones targeted player.";
        }

        public static void Initialize()
        {
            TargetCommands.Register(new UnStoneCommand());
        }

        public override void Execute(CommandEventArgs e, object obj)
        {
            Mobile from = e.Mobile;
            PlayerMobile target = obj as PlayerMobile;

            if (target != null)
            {
                if (target.Stoned)
                {
                    target.Stoned = false;
                    target.Squelched = false;
                    target.PagingSquelched = false;
                    target.Frozen = false;
                    target.SolidHueOverride = -1;
                    target.SendAsciiMessage("You have been unstoned.");
                }
                else
                    from.SendAsciiMessage("That player is not stoned.");
            }
            else
                from.SendAsciiMessage("This only works on players!");
        }
    }
}