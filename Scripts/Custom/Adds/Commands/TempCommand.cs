using System;
using System.Collections.Generic;
using System.IO;
using Server.Items;
using Server.Mobiles;
using Solaris.CliLocHandler;

namespace Server.Commands
{
    public class ChangeBanks
    {
        public static void Initialize()
        {
            CommandSystem.Register("TempCommand", AccessLevel.Owner, TempCommand_OnCommand);
        }

        private static void TempCommand_OnCommand(CommandEventArgs e)
        {
            (e.Mobile as BaseCreature).MinTameSkill = 100;
            List<Item> items = new List<Item>();
            int vendors = 0;
            using (StreamWriter op = new StreamWriter("vendorlist.txt"))
            {
                try
                {
                    foreach (Mobile m in World.Mobiles.Values)
                    {
                        if (!(m is PlayerVendor))
                            continue;

                        PlayerVendor vendor = m as PlayerVendor;
                        if (vendor.Owner != null)
                        {
                            vendors++;
                            foreach (Item item in vendor.Backpack.Items)
                                items.Add(item);
                        }
                    }
                    for (int i = 0; i < items.Count; ++i)
                    {
                        Item item = items[i];
                        PlayerVendor pv = item.RootParent as PlayerVendor;
                        if (pv != null)
                        {
                            VendorItem vi = pv.GetVendorItem(item);
                            op.Write(item.ItemID.ToString());
                            op.Write(",");
                            op.Write(item.Hue);
                            op.Write(",");
                            op.Write(item.Name ?? CliLoc.LocToString(item.LabelNumber));
                            op.Write(",");
                            op.Write(item.Amount);
                            op.Write(",");
                            if (vi != null)
                            {
                                op.Write(vi.Description);
                                op.Write(",");
                                op.Write(vi.Created);
                                op.Write(",");
                                op.Write(vi.FormattedPrice);
                            }
                            op.WriteLine();

                            if (item is BaseContainer)
                            {
                                foreach (Item it in item.Items)
                                {
                                    items.Add(it);
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    //throw new ArgumentException();
                }
            }
            World.Broadcast(29, true, vendors + "vendors");
            World.Broadcast(29, true, items.Count + "items");
        }
    }
}
