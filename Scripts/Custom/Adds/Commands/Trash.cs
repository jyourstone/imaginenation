using Server.Commands;
using Server.Commands.Generic;
using Server.Items;

namespace Server.Scripts.Custom.Adds.Commands
{
    public class Trash : BaseCommand
    {
        private static string mNotInBackpack = "The targeted item must be in your backpack.";
        private static string mNoTrashCan = "You must have a reward trash can in your backpack to use this command.";

        public Trash()
        {
            AccessLevel = AccessLevel.Player;
            Supports = CommandSupport.AllItems;
            Commands = new[] { "Trash" };
            ObjectTypes = ObjectTypes.Items;
            Usage = "Trash";
            Description = "Trashes a targeted item.";
        }

        public static void Initialize()
        {
            TargetCommands.Register(new Trash());
        }

        public override void Execute(CommandEventArgs e, object obj)
        {
            Mobile from = e.Mobile;
            Item target = (Item)obj;

            if (target != null)
                DoTrash(from, target);
        }

        public static void DoTrash(Mobile from, Item target)
        {
            Item item = from.Backpack.FindItemByType(typeof (MiniRewardCan), true);

            if (!target.IsChildOf(from.Backpack))
            {
                from.SendAsciiMessage(mNotInBackpack);
            }
            else if (item == null || !(item is MiniRewardCan))
            {
                from.SendAsciiMessage(mNoTrashCan);
            }
            else
            {
                item.OnDragDrop(from, target);
            }
        }
    }
}
