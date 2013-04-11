using Server.Items;

namespace Server.Commands.Generic
{
    #region

    

    #endregion

    public class SendToCommand : BaseCommand
    {
        public SendToCommand()
        {
            AccessLevel = AccessLevel.GameMaster;
            Supports = CommandSupport.AllMobiles;
            Commands = new[] {"SendTo", "ST"};
            ObjectTypes = ObjectTypes.Mobiles;
            Usage = "SendTo <x y> | <x y z> | <yLat yMins (N/S) ySount xLong (E/W)>";
            Description = "Sends target mobile to provided location.";
        }

        public static void Initialize()
        {
            TargetCommands.Register(new SendToCommand());
        }

        public override void Execute(CommandEventArgs e, object obj)
        {
            Mobile from = e.Mobile;
            Mobile target = obj as Mobile;
            Map map = from.Map;
            Point3D location = Point3D.Zero;

            if (target == null || map == Map.Internal || map == null)
            {
                from.SendAsciiMessage("Invalid target.");
                return;
            }
            else if (e.Length == 0)
            {
                from.SendAsciiMessage("Format:");
                from.SendAsciiMessage("SendTo int x int y");
                from.SendAsciiMessage("SendTo int x int y int z");
                from.SendAsciiMessage("SendTo int yLat int yMins (N|S) ySount int xLong int xMins (E|W) xEast");
                return;
            }
            else if (e.Length == 2)
            {
                int x = e.GetInt32(0);
                int y = e.GetInt32(1);

                location = new Point3D(x, y, map.GetAverageZ(x, y));
            }
            else if (e.Length == 3)
                location = new Point3D(e.GetInt32(0), e.GetInt32(1), e.GetInt32(2));
            else if (e.Length == 6)
            {
                location = Sextant.ReverseLookup(map, e.GetInt32(3), e.GetInt32(0), e.GetInt32(4), e.GetInt32(1), Insensitive.Equals(e.GetString(5), "E"), Insensitive.Equals(e.GetString(2), "S"));

                if (location == Point3D.Zero)
                    from.SendMessage("Sextant reverse lookup failed.");
            }

            if (location != Point3D.Zero)
            {
                target.MoveToWorld(location, map);
                from.SendAsciiMessage(string.Format("Sent {0} to {1}.", target.Name, location));
            }
            else
                from.SendAsciiMessage("Invalid location.");
        }
    }
}