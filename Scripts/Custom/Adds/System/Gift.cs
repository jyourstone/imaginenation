using System;
using System.Collections.Generic;
using System.IO;
using Server.Accounting;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Gumps
{
    public class GiftGump : Gump
    {
        private static readonly List<string> addresses = new List<string>();
        private static readonly List<string> accounts = new List<string>();
        private static readonly DateTime validuntil = new DateTime(2010, 02, 02, 23, 59, 59);

        public static void Initialize()
        {
            //EventSink.Login += OnLogin;
            //CommandSystem.Register("CheckGift", AccessLevel.GameMaster, On_CheckGiftCommand);
            CommandSystem.Register("SendGift", AccessLevel.GameMaster, On_SendGiftCommand);
            //CommandSystem.Register("Gift", AccessLevel.Player, On_GiftCommand);

            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");
        }

        [Usage("CheckGift")]
        [Description("Checks if the targeted player has recieved their gift, checks for account and IP")]
        private static void On_CheckGiftCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new CheckGiftTarget();
        }

        [Usage("SendGift")]
        [Description("Sends the gift gump to a player")]
        private static void On_SendGiftCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new SendGiftTarget();
        }

        [Usage("Gift")]
        [Description("Brings the gift gump up")]
        private static void On_GiftCommand(CommandEventArgs e)
        {
            NetState state = e.Mobile.NetState;
            Account a = (Account)e.Mobile.Account;

            if (state == null || a == null)
                return;

            if (accounts.Contains(a.ToString()))
            {
                e.Mobile.SendAsciiMessage("You have already received your gift!");
                return;
            }

            if (addresses.Contains(state.Address.ToString()))
            {
                e.Mobile.SendAsciiMessage("This IP has already received a gift, if you are on a shared connection pleage page a staff member for assistance!");
                return;
            }

            if (e.Mobile is PlayerMobile)
            {
                if (((PlayerMobile)e.Mobile).TempCheck)
                {
                    e.Mobile.SendAsciiMessage("You have already received your gift!");
                    return;
                }
            }

            if (DateTime.Now < validuntil)
            {
                e.Mobile.CloseGump(typeof (GiftGump));
                e.Mobile.SendGump(new GiftGump());
            }
            else
                e.Mobile.SendAsciiMessage("Sorry, you could only receive your gift until " + validuntil);
        }

        public class CheckGiftTarget : Target
        {
            public CheckGiftTarget() : base(-1, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is PlayerMobile)
                {
                    PlayerMobile pm = (PlayerMobile) targeted;
                    NetState state = pm.NetState;
                    Account a = (Account)pm.Account;

                    if (state == null || a == null)
                        return;

                    from.SendAsciiMessage("Has this IP received a gift: " + (addresses.Contains(state.Address.ToString()) ? "Yes" : "No"));
                    from.SendAsciiMessage("Has this account received a gift: " + (accounts.Contains(a.ToString()) ? "Yes" : "No"));
                }
            }
        }

        public class SendGiftTarget : Target
        {
            public SendGiftTarget() : base(-1, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is PlayerMobile)
                {
                    PlayerMobile pm = (PlayerMobile)targeted;
                    pm.CloseGump(typeof (GiftGump));
                    pm.SendGump(new GiftGump());

                    CommandLogging.WriteLine(from, "{0} sent a gift package gump to {0}", from.Name, pm.Name);
                }
            }
        }

        private static void OnLogin(LoginEventArgs e)
        {
            NetState state = e.Mobile.NetState;
            Account a = (Account) e.Mobile.Account;

            if (DateTime.Now > validuntil)
                return;

            if (state == null || a == null)
                return;

            if (!e.Mobile.Alive || addresses.Contains(state.Address.ToString()) || accounts.Contains(a.ToString()))
                return;

            if (e.Mobile is PlayerMobile)
            {
                if (((PlayerMobile) e.Mobile).TempCheck)
                    return;
            }

            e.Mobile.CloseGump(typeof (GiftGump));
            e.Mobile.SendGump(new GiftGump());

        }

        public GiftGump() : base(50,50)
        {
            Closable = true;
            Disposable = true;
            Dragable = true;
            AddPage(0);
            AddBackground(112, 136, 480, 473, 9200);
            AddLabel(300, 149, 1948, @"Congratulations!");
            AddHtml(131, 178, 442, 147, @"<center>We have finally opened our new server and as a gift to everyone for waiting so long, you get to choose one of the following gifts. This offer only stands until 2010-02-02 and is limited to 1 gift per account and IP.<br><br> Thank you for playing on Imagine Nation: Xtreme, we wouldn't be here without you. <br><br>You can open this gump again by typing .gift", false, false);

            AddHtml(145, 340, 150, 210, @"<center><u>PvP Package</u><br>200 XUO FS scrolls<br>200 NOS FS scrolls<br>500 mana pots<br>500 GH pots<br>2 random magic weapons<br>3 blood rock sets exceptional<br>5 kill remove balls<br>1 pure black robe<br>80k", false, false);
            AddHtml(315, 340, 150, 220, @"<center><u>PvM Package</u><br>200 BS scrolls<br>100 EV scrolls<br>100 mana pots<br>400 each reg<br>10000 arrows<br>1 random mount<br>1 random magic weapon<br>1 blood rock set exceptional<br>1 sapphire robe<br>125k", false, false);
            AddHtml(440, 340, 150, 210, @"<center><u>Just gold</u><br>250k", false, false);
            
            AddButton(210, 545, 2117, 2118, 1, GumpButtonType.Reply, 0);
            AddButton(380, 562, 2117, 2118, 2, GumpButtonType.Reply, 0);
            AddButton(505, 380, 2117, 2118, 3, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            NetState ns = state;
            Account a = (Account)state.Mobile.Account;
            string package = "none";

            if (ns == null || a == null)
                return;

            switch (info.ButtonID)
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        state.Mobile.SendAsciiMessage("A PvP package crystal has been placed in your bankbox!");
                        state.Mobile.BankBox.DropItem(new PVPPackage{Owner = state.Mobile});
                        package = "PvP";
                        goto case 99;
                    }
                case 2:
                    {
                        state.Mobile.SendAsciiMessage("A PvM package crystal has been placed in your bankbox!");
                        state.Mobile.BankBox.DropItem(new PVMPackage {Owner = state.Mobile});
                        package = "PvM";
                        goto case 99;
                    }
                case 3:
                    {
                        state.Mobile.SendAsciiMessage("A bankcheck worth 250k has been placed in your bankbox");
                        state.Mobile.BankBox.DropItem(new BankCheck(250000) {Name = "(gift)"});
                        package = "Gold";
                        goto case 99;
                    }
                case 99:
                    {
                        state.Mobile.PlaySound(491);
                        
                        if (state.Mobile is PlayerMobile)
                        {
                            PlayerMobile pm = (PlayerMobile)state.Mobile;
                            pm.TempCheck = true;
                        }

                        addresses.Add(state.Address.ToString());
                        accounts.Add(state.Account.ToString());

                        Console.WriteLine("{0} received a gift", state.Mobile.Name);

                        //*****Logging attempt*****
                        try
                        {
                            Stream fileStream = File.Open("Logs/ReceivedGifts.log", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                            StreamWriter writeAdapter = new StreamWriter(fileStream);
                            writeAdapter.WriteLine("[{0}]{1}\t{2}\t{3}\t{4}", state.Account, state.Mobile.Name, state.Address, package, DateTime.Now);
                            writeAdapter.Close();
                        }
                        catch
                        {
                        }
                        //**************************

                        break;
                    }
            }
        }
    }
}





