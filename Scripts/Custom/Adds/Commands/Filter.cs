using Server.Network;

namespace Server.Commands
{
    #region

    

    #endregion

    public class FilterCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("BattleFocus", AccessLevel.Player, BF_OnCommand);
            CommandSystem.Register("BF", AccessLevel.Player, BF_OnCommand);
        }

        [Usage("BattleFocus")]
        [Description("Filters away packets that are out of LOS.")]
        private static void BF_OnCommand(CommandEventArgs e)
        {
            e.Mobile.HasFilter = !e.Mobile.HasFilter;

            if (!e.Mobile.HasFilter)
                e.Mobile.SendEverything();
            else
            {
                Mobile from = e.Mobile;
                NetState ns = from.NetState;

                if (from.Map != null && ns != null)
                {
                    IPooledEnumerable eable = from.Map.GetObjectsInRange(from.Location, Core.GlobalMaxUpdateRange);

                    foreach (object o in eable)
                        if (o is Mobile)
                        {
                            Mobile m = (Mobile) o;

                            if (m != from && Utility.InUpdateRange(from.Location, m.Location) && !from.InLOS(o))
                                ns.Send(m.RemovePacket);
                        }
                        else if (o is Item)
                        {
                            Item item = (Item) o;

                            if (from.InRange(item.Location, item.GetUpdateRange(from)) && !from.InLOS(o))
                                ns.Send(item.RemovePacket);
                        }

                    eable.Free();
                }
            }

            string message, status;
            status = e.Mobile.HasFilter ? "on" : "off";
            message = e.Mobile.HasFilter ? "you no longer see out of sight" : "you can now see everything again";

            e.Mobile.SendAsciiMessage(string.Format("You have turned {0} the PvP filter, {1}.", status, message));
        }
    }
}