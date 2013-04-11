using Server.Commands.Generic;
using Server.Items;
using Server.Network;

namespace Server.Commands
{
    #region

    

    #endregion

    public class UnlinkCommand : BaseCommand
    {
        public UnlinkCommand()
        {
            AccessLevel = AccessLevel.Counselor;
            Supports = CommandSupport.Area | CommandSupport.Region | CommandSupport.Global | CommandSupport.Multi | CommandSupport.Single;
            Commands = new[] {"unlink"};
            ObjectTypes = ObjectTypes.Items;
            Usage = "unlink <target door>";
            Description = "Unlinks any door";
        }

        public static void Initialize()
        {
            TargetCommands.Register(new UnlinkCommand());
        }

        public override void Execute(CommandEventArgs e, object obj)
        {
            BaseDoor m = obj as BaseDoor;

            if (m != null)
            {
                m.Link = null;
                m.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "This door is no longer linked.");
            }
        }
    }
}