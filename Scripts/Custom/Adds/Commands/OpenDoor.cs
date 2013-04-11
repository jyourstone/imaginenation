using Server.Commands;
using Server.Items;

namespace Server.Scripts.Custom.Adds.Commands
{
    #region

    

    #endregion

    public class DoorCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("OpenDoor", AccessLevel.Player, Door_OnCommand);
        }

        [Usage("OpenDoor")]
        [Description("Opens the first close by door.")]
        private static void Door_OnCommand(CommandEventArgs e)
        {
            if (!e.Mobile.Alive)
            {
                e.Mobile.SendMessage("You can't use this command while being dead!");
                return;
            }

            BaseDoor doorToOpen = null;

            IPooledEnumerable eable = e.Mobile.GetObjectsInRange(3);
            foreach (object o in eable)
                if (o is BaseDoor && ((BaseDoor)o).GuildID <= 0)
                {
                    //if (e.Mobile.InLOS(o))
                    //{
                        doorToOpen = (BaseDoor) o;
                        break;
                    //}
                }
            eable.Free();

            if (doorToOpen == null)
                return;

            doorToOpen.Use(e.Mobile);
        }
    }
}