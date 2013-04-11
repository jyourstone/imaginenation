using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Server.Accounting;
using Server.Items;
using Server.Mobiles;
using Server.Multis;

namespace Server.Commands
{
    #region

    

    #endregion

    public class PlayerWealth
    {
        public static void Initialize()
        {
            CommandSystem.Register("PlayerWealth", AccessLevel.Administrator, PlayerWealth_OnCommand);
        }

        [Usage("PlayerWealth")]
        [Description("Creates the file playerwealth.html, which is a report of all gold/checks for players on shard.")]
        public static void PlayerWealth_OnCommand(CommandEventArgs e)
        {
            string filename = "playerwealth.html";
            List<AccountInfo> AccountWealth = new List<AccountInfo>();
            AccountInfo A;
            A.GrandTotal = 0;
            WealthInfo backpack;
            WealthInfo bank;
            WealthInfo home;
            Mobile m = null;
            if (e != null)
                m = e.Mobile;
            long shardtotal = 0;
            long totalnotreportedamt = 0;
            uint totalhomes = 0;
            uint totalnotreported = 0;
            uint totalaccounts = 0;
            uint totalchars = 0;
            uint totalvendors = 0;
            double percent = 0;

            foreach (Account a in Accounts.GetAccounts())
            {
                if (a != null && a.Username != null && a.AccessLevel == AccessLevel.Player)
                {
                    totalaccounts++;
                    A.acct = a;
                    A.wealth.TotalGold = 0;
                    A.wealth.TotalChecks = 0;
                    A.wealth.VendorGold = 0;
                    backpack.TotalGold = 0;
                    backpack.TotalChecks = 0;
                    bank.TotalGold = 0;
                    bank.TotalChecks = 0;
                    home.TotalGold = 0;
                    home.TotalChecks = 0;
                    home.VendorGold = 0;
                    int money = 0;
                    for (int i = 0; i < a.Length; i++) // First record gold in player's bank/backpack
                    {
                        Mobile cm = a[i];

                        if (cm == null)
                            continue;
                        totalchars++;
                        if (cm.Backpack != null)
                            backpack = SearchContainer(cm.Backpack);
                        if (cm.BankBox != null)
                            bank = SearchContainer(cm.BankBox);
                        A.wealth.TotalGold += backpack.TotalGold;
                        A.wealth.TotalChecks += backpack.TotalChecks;
                        A.wealth.TotalGold += bank.TotalGold;
                        A.wealth.TotalChecks += bank.TotalChecks;

                        List<PlayerVendor> vendors = new List<PlayerVendor>();
                        
                        foreach (Mobile mob in World.Mobiles.Values)
                        {
                            if (mob is PlayerVendor)
                                vendors.Add(mob as PlayerVendor);
                        }

                        for (int j = 0; j < vendors.Count; ++j)
                        {
                            if (vendors[j].Owner == cm)
                            {
                                money += vendors[j].HoldGold;
                                totalvendors ++;
                            }
                        }
                    }
                    ArrayList allHouses = new ArrayList(2);
                    for (int i = 0; i < a.Length; ++i) // Now houses they own
                    {
                        Mobile mob = a[i];

                        if (mob != null)
                            allHouses.AddRange(BaseHouse.GetHouses(mob));
                    }
                    for (int i = 0; i < allHouses.Count; ++i)
                    {
                        BaseHouse house = (BaseHouse) allHouses[i];

                        totalhomes++;
                        foreach (IEntity entity in house.GetHouseEntities())
                        {
                            if (entity is Item && !((Item) entity).Deleted)
                            {
                                Item item = (Item) entity;
                                if (item is Gold)
                                    home.TotalGold += item.Amount;
                                else if (item is BankCheck)
                                    home.TotalChecks += ((BankCheck) item).Worth;
                                else if (item is Container)
                                    home = SearchContainer((Container) item, home);
                            }
                            /*else  // Vendors gold belongs to???? Let's skip this...
							{
							} */
                        }
                    }
                    A.wealth.TotalGold += home.TotalGold;
                    A.wealth.TotalChecks += home.TotalChecks;
                    A.wealth.VendorGold += money;
                    A.GrandTotal = A.wealth.TotalGold + A.wealth.TotalChecks + A.wealth.VendorGold;
                    shardtotal += A.GrandTotal;
                    if (A.GrandTotal < 10000)
                    {
                        totalnotreportedamt += A.GrandTotal;
                        totalnotreported++;
                    }
                    else
                        AccountWealth.Add(A);
                }
            }
            //AccountWealth.Sort(new SortArray());
            AccountWealth.Sort(delegate(AccountInfo a1, AccountInfo a2) { return a2.GrandTotal.CompareTo(a1.GrandTotal); });
            using (StreamWriter op = new StreamWriter(filename))
            {
                op.WriteLine("<html><body><strong>Player Wealth report generated on {0}</strong>", DateTime.Now);
                op.WriteLine("<br/><strong>Total Accounts: {0}</strong>", totalaccounts);
                op.WriteLine("<br/><strong>Total Characters: {0}</strong>", totalchars);
                op.WriteLine("<br/><strong>Total Houses: {0}</strong>", totalhomes);
                op.WriteLine("<br/><strong>Total Vendors: {0}</strong>", totalvendors);
                op.WriteLine("<br/><strong>Total Gold/Checks/Vendors: {0}</strong>", shardtotal.ToString("#,##0", new CultureInfo("en-US")));
                op.WriteLine("<br/><br/>");
                op.WriteLine("<table width=\"500\"  border=\"2\" bordercolor=\"#FFFFFF\" bgcolor=\"#DEB887\"<td colspan=\"5\"><div align=\"left\"><font color=\"#8B0000\" size=\"+2\"><strong>Player Wealth</strong></font></div></td>");
                op.WriteLine("<tr bgcolor=\"#667C3F\"><font color=\"#FFFFFF\" size=\"+1\"><td align=\"center\">Account</td><td width=\"100\" align=\"right\">Gold</td><td width=\"100\" align=\"right\">Checks</td><td width=\"100\" align=\"right\">Vendor(s)</td><td width=\"100\" align=\"right\">Total</td><td width=\"100\" align=\"right\">% of shard</td></tr></font>");
                int count = 0;
                foreach (AccountInfo ai in AccountWealth)
                {
                    count++;
                    percent = ai.GrandTotal/(double) shardtotal*100.00f;
                    op.WriteLine("<tr bgcolor=\"#{6}\"><td align=\"left\"><pre>{7,-3} {0}</pre></td><td align=\"Right\">{1}</td><td align=\"Right\">{2}</td><td align=\"Right\">{3}</td><td align=\"Right\">{4}</td><td align=\"Right\">{5: ##0.00}%</td></tr></div>", ai.acct.Username, ai.wealth.TotalGold.ToString("#,##0", new CultureInfo("en-US")), ai.wealth.TotalChecks.ToString("#,##0", new CultureInfo("en-US")), ai.wealth.VendorGold.ToString("#,##0", new CultureInfo("en-US")), ai.GrandTotal.ToString("#,##0", new CultureInfo("en-US")), percent, percent > 0.70 ? (percent > 3.5 ? "DC143C" : (percent > 1.0 ? "B22222" : "F08080")) : (percent < 0.25 ? (percent < 0.01 ? "7FFFD4" : "90EE90") : "F0E68C"), count > 50 ? "   " : count + ".");
                }
                percent = totalnotreportedamt/(double) shardtotal*100.00f;
                op.WriteLine("<tr bgcolor=\"#D3D3D3\"><td align=\"center\" colspan=\"3\">{0} accounts < 10000 not reported.</td><td align=\"Right\">{1: ##,###,###,##0}</td><td align=\"Right\">{2: ##0.00}%</td></tr>", totalnotreported, totalnotreportedamt, percent);
                op.WriteLine("</table></body></html>");
            }
            if (m != null)
                m.SendMessage("Total accounts processed: {0}", totalaccounts);
            else
                Console.WriteLine("Player wealth report generated. Total accounts: {0}", totalaccounts);
        }

        public static WealthInfo SearchContainer(Container pack, WealthInfo w)
        {
            WealthInfo w1;
            w1.TotalGold = 0;
            w1.TotalChecks = 0;
            w1 = SearchContainer(pack);
            w.TotalGold += w1.TotalGold;
            w.TotalChecks += w1.TotalChecks;
            return w;
        }

        public static WealthInfo SearchContainer(Container pack)
        {
            WealthInfo w;
            w.TotalGold = 0;
            w.TotalChecks = 0;
            w.VendorGold = 0;
            if (pack == null)
                return w;
            List<Item> packlist = pack.Items;
            for (int i = 0; i < packlist.Count; ++i)
            {
                Item item = packlist[i];

                if (item != null && !item.Deleted)
                {
                    if (item is Container)
                        w = SearchContainer((Container) item, w);
                    else if (item is Gold)
                        w.TotalGold += item.Amount;
                    else if (item is BankCheck)
                        w.TotalChecks += ((BankCheck) item).Worth;
                }
            }
            return w;
        }

        #region Nested type: AccountInfo

        public struct AccountInfo
        {
            public Account acct;
            public long GrandTotal;
            public WealthInfo wealth;
        }

        #endregion

        #region Nested type: WealthInfo

        public struct WealthInfo
        {
            public long TotalChecks;
            public long TotalGold;
            public long VendorGold;
        }

        #endregion
    }
}