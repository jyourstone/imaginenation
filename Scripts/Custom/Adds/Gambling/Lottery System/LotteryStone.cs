// *************************************************************************************
// Lottery System v1.1 by FingersMcSteal
// Working on RunUO 1.0 & RunUO 2.0 SVN Revision 156
// 
// Some sections of this script have remarked comments to help
// shard owners setup the lottery system timers.
// By default this script is setup to allow one lottery ever 7 days with
// a lottery system message displayed every 20 minutes to all online players.
// This system, like all i create is running on my shard (Yaks UO World : Undead Realms)
// if people would like to check it out before they install it.
// This is the RE-release of the original version which had several errors in.
// *************************************************************************************

using System;
using System.Collections;
using Server.Commands;
using Server.Gumps;

// 2.0

namespace Server.Items
{
    [Flipable(0x2AED, 0x2ADD)]
	public class LotteryStone : Item
	{
		private int m_VisitsToStone = 0;
        private int m_PriceToPlay = 1000; // Price of each ticket
        private int m_HighestEverJackpot = 0;
        private int m_RollOverAmount = 0;
        private int m_NextTicketNumber = 0;
        private int m_LotteryGoldTaken = 0;
        private int m_JackpotAmount = 0;

        // *********************************************************************************
        // The 5 split figures below are how the Jackpot for a given draw is divided up over
        // the 5 types of win conditions I.E. 5, 4 ,3 ,2 or 1 matched draw number.
        // So a winning ticket with 5 matching draw numbers will recieve
        // 'Split5' or 50% of the Jackpot total.
        // These must all add up to 100.
        // This can also be accessed through [props commands by GM's.
        // *********************************************************************************
        private int m_Split5 = 50;
        private int m_Split4 = 20;
        private int m_Split3 = 15;
        private int m_Split2 = 10;
        private int m_Split1 = 5;

        // ***********************************************************************************
        // ***********************************************************************************
        // ***********************************************************************************
        // You can alter the amount of time between lotterys here...
        // This lottery is setup for one draw every 3 days... TimeSpan.FromDays (3);
        // If you do change this figure, you will need to ALSO make a change in the TIMER code
        // which is near the bottom of this file to match with the change you make here.

        private DateTime m_DateOfNextDraw = DateTime.Now + TimeSpan.FromDays (3);

        // ***********************************************************************************
        // ***********************************************************************************
        // ***********************************************************************************

        private int m_OneWinner1 = 1;
        private int m_OneWinner2 = 1;
        private int m_OneWinner3 = 2;
        private int m_OneWinner4 = 2;
        private int m_OneWinner5 = 3;

        private int m_TwoWinner1 = 5;
        private int m_TwoWinner2 = 4;
        private int m_TwoWinner3 = 3;
        private int m_TwoWinner4 = 2;
        private int m_TwoWinner5 = 1;

        private int m_ThreeWinner1 = 1;
        private int m_ThreeWinner2 = 2;
        private int m_ThreeWinner3 = 3;
        private int m_ThreeWinner4 = 4;
        private int m_ThreeWinner5 = 5;

        private int m_WinAmount5 = 0;
        private int m_WinAmount4 = 0;
        private int m_WinAmount3 = 0;
        private int m_WinAmount2 = 0;
        private int m_WinAmount1 = 0;

        private int m_LastWin1 = 0;
        private int m_LastWin2 = 0;
        private int m_LastWin3 = 0;
        private int m_LastWin4 = 0;
        private int m_LastWin5 = 0;
        private int m_LastTotalWins = 0;

        [CommandProperty(AccessLevel.Administrator)]
        public int LastTotalWins
        { get { return m_LastTotalWins; } set { m_LastTotalWins = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int LastWin5
        { get { return m_LastWin5; } set { m_LastWin5 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int LastWin4
        { get { return m_LastWin4; } set { m_LastWin4 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int LastWin3
        { get { return m_LastWin3; } set { m_LastWin3 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int LastWin2
        { get { return m_LastWin2; } set { m_LastWin2 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int LastWin1
        { get { return m_LastWin1; } set { m_LastWin1 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Split5
        { get { return m_Split5; } set { m_Split5 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Split4
        { get { return m_Split4; } set { m_Split4 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Split3
        { get { return m_Split3; } set { m_Split3 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Split2
        { get { return m_Split2; } set { m_Split2 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Split1
        { get { return m_Split1; } set { m_Split1 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int WinAmount1
        { get { return m_WinAmount1; } set { m_WinAmount1 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int WinAmount2
        { get { return m_WinAmount2; } set { m_WinAmount2 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int WinAmount3
        { get { return m_WinAmount3; } set { m_WinAmount3 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int WinAmount4
        { get { return m_WinAmount4; } set { m_WinAmount4 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int WinAmount5
        { get { return m_WinAmount5; } set { m_WinAmount5 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int ThreeWinner5
        { get { return m_ThreeWinner5; } set { m_ThreeWinner5 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int ThreeWinner4
        { get { return m_ThreeWinner4; } set { m_ThreeWinner4 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int ThreeWinner3
        { get { return m_ThreeWinner3; } set { m_ThreeWinner3 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int ThreeWinner2
        { get { return m_ThreeWinner2; } set { m_ThreeWinner2 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int ThreeWinner1
        { get { return m_ThreeWinner1; } set { m_ThreeWinner1 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int TwoWinner5
        { get { return m_TwoWinner5; } set { m_TwoWinner5 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int TwoWinner4
        { get { return m_TwoWinner4; } set { m_TwoWinner4 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int TwoWinner3
        { get { return m_TwoWinner3; } set { m_TwoWinner3 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int TwoWinner2
        { get { return m_TwoWinner2; } set { m_TwoWinner2 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int TwoWinner1
        { get { return m_TwoWinner1; } set { m_TwoWinner1 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int OneWinner5
        { get { return m_OneWinner5; } set { m_OneWinner5 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int OneWinner4
        { get { return m_OneWinner4; } set { m_OneWinner4 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int OneWinner3
        { get { return m_OneWinner3; } set { m_OneWinner3 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int OneWinner2
        { get { return m_OneWinner2; } set { m_OneWinner2 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int OneWinner1
        { get { return m_OneWinner1; } set { m_OneWinner1 = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public DateTime DateOfNextDraw
        {
            get
            {
                return m_DateOfNextDraw;
            }
            set
            {
                m_DateOfNextDraw = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int JackpotAmount
        {
            get
            {
                return m_JackpotAmount;
            }
            set
            {
                m_JackpotAmount = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int LotteryGoldTaken
        {
            get
            {
                return m_LotteryGoldTaken;
            }
            set
            {
                m_LotteryGoldTaken = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int NextTicketNumber
        {
            get
            {
                return m_NextTicketNumber;
            }
            set
            {
                m_NextTicketNumber = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int RollOverAmount
        {
            get
            {
                return m_RollOverAmount;
            }
            set
            {
                m_RollOverAmount = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int HighestEverJackpot
        {
            get
            {
                return m_HighestEverJackpot;
            }
            set
            {
                m_HighestEverJackpot = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PriceToPlay
        {
            get
            {
                return m_PriceToPlay;
            }
            set
            {
                m_PriceToPlay = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int VisitsToStone
		{
			get
			{
                return m_VisitsToStone;
			}
			set
			{
                m_VisitsToStone = value;
				InvalidateProperties();
			}
		}

		[Constructable]
        public LotteryStone() : base(0x2AED)
		{
			Movable = false;
			Hue = 1265;
			Name = "Lottery Stone";
		}

		public override void OnDoubleClick( Mobile from )
		{
            if (!from.InRange(GetWorldLocation(), 1))
            {       
                from.SendMessage(33, "Step closer to the stone please.");
                // from.PlaySound(0x1F0);
            }
            else
            {
                m_VisitsToStone = m_VisitsToStone + 1;
                from.CloseGump(typeof(LotteryGump));
                from.SendGump(new LotteryGump( this ));
                // from.PlaySound(0x421);         
            }		
		}

        public LotteryStone(Serial serial) : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // version
            writer.Write(m_VisitsToStone);
            writer.Write(m_PriceToPlay);
            writer.Write(m_HighestEverJackpot);
            writer.Write(m_RollOverAmount);
            writer.Write(m_NextTicketNumber);
            writer.Write(m_LotteryGoldTaken);
            writer.Write(m_JackpotAmount);
            writer.Write(m_Split5);
            writer.Write(m_Split4);
            writer.Write(m_Split3);
            writer.Write(m_Split2);
            writer.Write(m_Split1);

            writer.Write(m_LastWin1);
            writer.Write(m_LastWin2);
            writer.Write(m_LastWin3);
            writer.Write(m_LastWin4);
            writer.Write(m_LastWin5);
            writer.Write(m_LastTotalWins);

            writer.Write(m_DateOfNextDraw);

            writer.Write(m_OneWinner1);
            writer.Write(m_OneWinner2);
            writer.Write(m_OneWinner3);
            writer.Write(m_OneWinner4);
            writer.Write(m_OneWinner5);

            writer.Write(m_TwoWinner1);
            writer.Write(m_TwoWinner2);
            writer.Write(m_TwoWinner3);
            writer.Write(m_TwoWinner4);
            writer.Write(m_TwoWinner5);

            writer.Write(m_ThreeWinner1);
            writer.Write(m_ThreeWinner2);
            writer.Write(m_ThreeWinner3);
            writer.Write(m_ThreeWinner4);
            writer.Write(m_ThreeWinner5);

            writer.Write(m_WinAmount5);
            writer.Write(m_WinAmount4);
            writer.Write(m_WinAmount3);
            writer.Write(m_WinAmount2);
            writer.Write(m_WinAmount1);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			switch ( version )
			{
				case 0:
				{
                    m_VisitsToStone = reader.ReadInt();
                    m_PriceToPlay = reader.ReadInt();
                    m_HighestEverJackpot = reader.ReadInt();
                    m_RollOverAmount = reader.ReadInt();
                    m_NextTicketNumber = reader.ReadInt();
                    m_LotteryGoldTaken = reader.ReadInt();
                    m_JackpotAmount = reader.ReadInt();
                    m_Split5 = reader.ReadInt();
                    m_Split4 = reader.ReadInt();
                    m_Split3 = reader.ReadInt();
                    m_Split2 = reader.ReadInt();
                    m_Split1 = reader.ReadInt();

                    m_LastWin1 = reader.ReadInt();
                    m_LastWin2 = reader.ReadInt();
                    m_LastWin3 = reader.ReadInt();
                    m_LastWin4 = reader.ReadInt();
                    m_LastWin5 = reader.ReadInt();
                    m_LastTotalWins = reader.ReadInt();

                    m_DateOfNextDraw = reader.ReadDateTime();

                    m_OneWinner1 = reader.ReadInt();
                    m_OneWinner2 = reader.ReadInt();
                    m_OneWinner3 = reader.ReadInt();
                    m_OneWinner4 = reader.ReadInt();
                    m_OneWinner5 = reader.ReadInt();

                    m_TwoWinner1 = reader.ReadInt();
                    m_TwoWinner2 = reader.ReadInt();
                    m_TwoWinner3 = reader.ReadInt();
                    m_TwoWinner4 = reader.ReadInt();
                    m_TwoWinner5 = reader.ReadInt();

                    m_ThreeWinner1 = reader.ReadInt();
                    m_ThreeWinner2 = reader.ReadInt();
                    m_ThreeWinner3 = reader.ReadInt();
                    m_ThreeWinner4 = reader.ReadInt();
                    m_ThreeWinner5 = reader.ReadInt();

                    m_WinAmount5 = reader.ReadInt();
                    m_WinAmount4 = reader.ReadInt();
                    m_WinAmount3 = reader.ReadInt();
                    m_WinAmount2 = reader.ReadInt();
                    m_WinAmount1 = reader.ReadInt();
					break;
				}
			}
		}

        // ********************************************************
        // Lottery COMMANDS
        // Currently only one command is in... [lotteryticketdelete
        // To allow a clean up of the server.
        // ********************************************************
        public static void Initialize()
        {
            CommandSystem.Register("LotteryTicketDelete", AccessLevel.Administrator, LotteryTicketDelete_OnCommand);
        }

        [Usage("LotteryTicketDelete")]
        [Description("Deletes all lottery tickets.")]
        public static void LotteryTicketDelete_OnCommand(CommandEventArgs e)
        {
            DeleteAllLotteryTickets();
        }

        private static void DeleteAllLotteryTickets()
        {
            // **************************************************
            // Code to remove the lottery tickets from the server
            // **************************************************
            ArrayList ticketlist = new ArrayList();
            foreach (Item item in World.Items.Values)
            {
                if (item is LotteryTicket)
                    ticketlist.Add(item);
            }

            foreach (Item item in ticketlist)
                item.Delete();

            if (ticketlist.Count > 0)
                World.Broadcast(0x35, true, "{0} Lottery tickets removed.", ticketlist.Count);
        }

	}

    public class LotteryGump : Gump
    {
        private readonly LotteryStone m_lotterystone;

        public LotteryGump(LotteryStone m) : base(80, 80)
        {
            m_lotterystone = m;

            Closable = false;
            Disposable = false;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(0, 0, 437, 406, 83);
            AddBackground(20, 126, 245, 131, 2620);
            AddBackground(102, 274, 247, 113, 2620);
            AddLabel(86, 13, 1160, @"***** The Shard Lottery Game *****");
            AddLabel(64, 134, 1166, @"The Next Lottery Details");
            AddLabel(127, 282, 1265, @"Previous Lottery Win Numbers");
            AddLabel(31, 162, 170, @"Draw Date : " + m_lotterystone.DateOfNextDraw);
            AddLabel(31, 222, 167, @"Next Jackpot : " + m_lotterystone.LotteryGoldTaken);
            AddLabel(31, 202, 168, @"Ticket Sales So Far : " + m_lotterystone.NextTicketNumber);
            AddLabel(95, 70, 44, @"Highest Payout To Date : " + m_lotterystone.HighestEverJackpot);
            AddLabel(95, 90, 43, @"Rollover Amount Currently At : " + m_lotterystone.RollOverAmount);
            AddLabel(31, 182, 169, @"Price Per Ticket : " + m_lotterystone.PriceToPlay);
            AddLabel(116, 311, 100, @"The Last Draw : " + m_lotterystone.OneWinner1 + "," + m_lotterystone.OneWinner2 + "," + m_lotterystone.OneWinner3 + "," + m_lotterystone.OneWinner4 + "," + m_lotterystone.OneWinner5);
            AddLabel(116, 331, 99, @"2nd Last Draw: " + m_lotterystone.TwoWinner1 + "," + m_lotterystone.TwoWinner2 + "," + m_lotterystone.TwoWinner3 + "," + m_lotterystone.TwoWinner4 + "," + m_lotterystone.TwoWinner5);
            AddLabel(116, 351, 98, @"3rd Last Draw : " + m_lotterystone.ThreeWinner1 + "," + m_lotterystone.ThreeWinner2 + "," + m_lotterystone.ThreeWinner3 + "," + m_lotterystone.ThreeWinner4 + "," + m_lotterystone.ThreeWinner5);
            AddLabel(95, 50, 45, @"Visits To This Stone So Far : " + m_lotterystone.VisitsToStone); 
            AddLabel(320, 213, 85, @"Purchase Ticket");
            AddLabel(320, 235, 30, @"Cancel / Exit");
            AddButton(280, 212, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddButton(280, 234, 4017, 4019, 2, GumpButtonType.Reply, 0);
            // NEW EXTRAS
            AddLabel(320, 136, 85, @"Next Lottery");
            AddLabel(320, 154, 85, @"Details");
            AddButton(280, 135, 4005, 4007, 3, GumpButtonType.Reply, 0); // next details
            AddLabel(320, 175, 1265, @"Previous");
            AddLabel(320, 193, 1265, @"Draw Details");
            AddButton(280, 174, 4005, 4007, 4, GumpButtonType.Reply, 0); // previous details

        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            Container pack = from.Backpack;

            switch (info.ButtonID)
            {
                case 1: // Purchase
                    {
                        if (pack != null && pack.ConsumeTotal(typeof(Gold), m_lotterystone.PriceToPlay))
                        {
                            m_lotterystone.NextTicketNumber = m_lotterystone.NextTicketNumber + 1; // Increase ticket number to next ticket sold
                            m_lotterystone.LotteryGoldTaken = m_lotterystone.LotteryGoldTaken + m_lotterystone.PriceToPlay; // not finished
                            from.CloseGump(typeof(LotteryGump));
                            int blankdigit = 1; // Something to send to the first gump
                            from.SendGump(new NumberSelectionGump(m_lotterystone.NextTicketNumber, m_lotterystone.DateOfNextDraw, blankdigit, blankdigit, blankdigit, blankdigit, blankdigit, blankdigit, blankdigit, blankdigit, blankdigit, blankdigit, blankdigit, blankdigit, blankdigit, blankdigit, blankdigit, blankdigit));
                            from.SendMessage(0x35, "You pay for your ticket, select ticket numbers...");
                        }
                        else
                        {
                            from.SendMessage(0x91, "You need at least {0}gp to buy a ticket, you have to pay to play !!!", m_lotterystone.PriceToPlay);
                        }
                    }
                    break;

                case 2: // Cancel
                    {
                        from.CloseGump(typeof(LotteryGump));
                        from.SendMessage(0x35, "No Sale.");
                    }
                    break;

                case 3: // Next Lottery Details
                    {
                        from.CloseGump(typeof(NextLottery));
                        from.SendGump(new NextLottery(m_lotterystone.DateOfNextDraw, m_lotterystone.NextTicketNumber, m_lotterystone.LotteryGoldTaken, m_lotterystone.Split5, m_lotterystone.Split4, m_lotterystone.Split3, m_lotterystone.Split2, m_lotterystone.Split1, m_lotterystone.VisitsToStone, m_lotterystone.HighestEverJackpot, m_lotterystone.RollOverAmount));
                        // from.SendMessage(0x35, "No Sale.");
                    }
                    break;

                case 4: // Previous Lottery Details
                    {
                        from.CloseGump(typeof(PreviousLottery));
                        from.SendGump(new PreviousLottery(m_lotterystone.VisitsToStone, m_lotterystone.HighestEverJackpot, m_lotterystone.OneWinner1, m_lotterystone.OneWinner2, m_lotterystone.OneWinner3, m_lotterystone.OneWinner4, m_lotterystone.OneWinner5, m_lotterystone.LastWin5, m_lotterystone.LastWin4, m_lotterystone.LastWin3, m_lotterystone.LastWin2, m_lotterystone.LastWin1, m_lotterystone.LastTotalWins));
                        // from.SendMessage(0x35, "No Sale.");
                    }
                    break;
            }
        }
    }

    public class LotteryTicketGump : Gump
    {
        private readonly LotteryTicket m_lotteryticket;

        public LotteryTicketGump(LotteryTicket m) : base(50, 50)
        {
            m_lotteryticket = m;

            Closable = false;
            Disposable = false;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(0, 0, 316, 416, 9380);
            AddLabel(84, 34, 57, @"Official Lottery Ticket");
            AddLabel(40, 70, 94, @"Ticket Number : " + m_lotteryticket.TicketNumbers);
            AddLabel(40, 90, 94, @"Draw Date : " + m_lotteryticket.DateOfDrawOnTickets);
            AddLabel(40, 130, 94, @"Ticket Status : " + m_lotteryticket.TicketStatus);
            AddLabel(64, 150, 35, @"Lottery Terms and Conditions");
            AddLabel(35, 170, 47, @"The status will show if the ticket was");
            AddLabel(35, 190, 47, @"a winner or not. Winning claims must be");
            AddLabel(35, 209, 47, @"made before the next lottery is drawn.");
            AddImage(50, 314, 9004);
            AddLabel(35, 230, 35, @"Un-claimed tickets will become void.");
            AddLabel(35, 270, 47, @"Management will not be held accountable");
            AddLabel(35, 290, 47, @"for muggings, deaths or looted tickets.");
            AddButton(151, 315, 9723, 9724, 1, GumpButtonType.Reply, 0); // Exit
            AddButton(151, 350, 9723, 9724, 2, GumpButtonType.Reply, 0); // Claim Prize
            AddLabel(185, 319, 47, @"Close / Exit.");
            AddLabel(185, 354, 47, @"Claim Winnings.");
            AddLabel(40, 110, 94, @"Lottery Numbers : " + m_lotteryticket.LotteryNumber1Ticket + ", " + m_lotteryticket.LotteryNumber2Ticket + ", " + m_lotteryticket.LotteryNumber3Ticket + ", " + m_lotteryticket.LotteryNumber4Ticket + ", " + m_lotteryticket.LotteryNumber5Ticket);
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            Container pack = from.Backpack;
            
            switch (info.ButtonID)
            {
                case 1: // Exit
                    {
                        from.CloseGump(typeof(LotteryTicketGump));
                    }
                    break;
                case 2: // Claim Winnings
                    {
                        if (m_lotteryticket.TicketStatus == "Winnings Claimed")
                        {
                            from.SendMessage(0x35, "The reward from this ticket has already been paid out.");
                        }

                        if (m_lotteryticket.TicketStatus == "Void / Old Ticket")
                        {
                            from.SendMessage(0x35, "This was not a winning ticket.");
                        }

                        if (m_lotteryticket.TicketStatus == "Valid Ticket")
                        {
                            from.SendMessage(0x35, "This lottery has not been drawn yet.");
                        }

                        if (m_lotteryticket.TicketStatus == "1 Matched Number")
                        {
                            int togive = m_lotteryticket.LotteryTicketValue;
                            m_lotteryticket.TicketStatus = "Winnings Claimed";
                            m_lotteryticket.Hue = 1151;
                            from.SendMessage(0x35, "The ticket was valued at {0}gp for this lottery result, you recieve a bankcheck for your winnings.", togive);
                            from.AddToBackpack(new BankCheck(togive));
                            foreach (Item item in World.Items.Values)
                            {
                                if (item is LotteryStone)
                                {
                                    LotteryStone thelotterystone = (LotteryStone)item;
                                    if (thelotterystone.HighestEverJackpot < togive)
                                    {
                                        thelotterystone.HighestEverJackpot = togive;
                                    }
                                }
                            }
                        }

                        if (m_lotteryticket.TicketStatus == "2 Matched Numbers")
                        {
                            int togive = m_lotteryticket.LotteryTicketValue;
                            m_lotteryticket.TicketStatus = "Winnings Claimed";
                            m_lotteryticket.Hue = 1151;
                            from.SendMessage(0x35, "The ticket was valued at {0}gp for this lottery result, you recieve a bankcheck for your winnings.", togive);
                            from.AddToBackpack(new BankCheck(togive));
                            foreach (Item item in World.Items.Values)
                            {
                                if (item is LotteryStone)
                                {
                                    LotteryStone thelotterystone = (LotteryStone)item;
                                    if (thelotterystone.HighestEverJackpot < togive)
                                    {
                                        thelotterystone.HighestEverJackpot = togive;
                                    }
                                }
                            }
                        }
                        if (m_lotteryticket.TicketStatus == "3 Matched Numbers")
                        {
                            int togive = m_lotteryticket.LotteryTicketValue;
                            m_lotteryticket.TicketStatus = "Winnings Claimed";
                            m_lotteryticket.Hue = 1151;
                            from.SendMessage(0x35, "The ticket was valued at {0}gp for this lottery result, you recieve a bankcheck for your winnings.", togive);
                            from.AddToBackpack(new BankCheck(togive));
                            foreach (Item item in World.Items.Values)
                            {
                                if (item is LotteryStone)
                                {
                                    LotteryStone thelotterystone = (LotteryStone)item;
                                    if (thelotterystone.HighestEverJackpot < togive)
                                    {
                                        thelotterystone.HighestEverJackpot = togive;
                                    }
                                }
                            }
                        }
                        if (m_lotteryticket.TicketStatus == "4 Matched Numbers")
                        {
                            int togive = m_lotteryticket.LotteryTicketValue;
                            m_lotteryticket.TicketStatus = "Winnings Claimed";
                            m_lotteryticket.Hue = 1151;
                            from.SendMessage(0x35, "The ticket was valued at {0}gp for this lottery result, you recieve a bankcheck for your winnings.", togive);
                            from.AddToBackpack(new BankCheck(togive));
                            foreach (Item item in World.Items.Values)
                            {
                                if (item is LotteryStone)
                                {
                                    LotteryStone thelotterystone = (LotteryStone)item;
                                    if (thelotterystone.HighestEverJackpot < togive)
                                    {
                                        thelotterystone.HighestEverJackpot = togive;
                                    }
                                }
                            }
                        }
                        if (m_lotteryticket.TicketStatus == "5 Matched Numbers")
                        {
                            int togive = m_lotteryticket.LotteryTicketValue;
                            m_lotteryticket.TicketStatus = "Winnings Claimed";
                            m_lotteryticket.Hue = 1151;
                            from.SendMessage(0x35, "The ticket was valued at {0}gp for this lottery result, you recieve a bankcheck for your winnings.", togive);
                            from.AddToBackpack(new BankCheck(togive));
                            foreach (Item item in World.Items.Values)
                            {
                                if (item is LotteryStone)
                                {
                                    LotteryStone thelotterystone = (LotteryStone)item;
                                    if (thelotterystone.HighestEverJackpot < togive)
                                    {
                                        thelotterystone.HighestEverJackpot = togive;
                                    }
                                }
                            }
                        }
                        break;
                    }
            }
        }
    }

    [Flipable(0x14ED, 0x14EE)]
    public class LotteryTicket : Item
    {
        private DateTime m_DateOfDrawOnTickets = DateTime.Now;
        private int m_TicketNumbers = 0;
        private string m_TicketStatus = "New Ticket";

        private int m_LotteryNumber1Ticket = 0;
        private int m_LotteryNumber2Ticket = 0;
        private int m_LotteryNumber3Ticket = 0;
        private int m_LotteryNumber4Ticket = 0;
        private int m_LotteryNumber5Ticket = 0;

        private int m_LotteryTicketValue = 0;

        [CommandProperty(AccessLevel.Administrator)]
        public int LotteryTicketValue
        { get { return m_LotteryTicketValue; } set { m_LotteryTicketValue = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int LotteryNumber5Ticket
        { get { return m_LotteryNumber5Ticket; } set { m_LotteryNumber5Ticket = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int LotteryNumber4Ticket
        { get { return m_LotteryNumber4Ticket; } set { m_LotteryNumber4Ticket = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int LotteryNumber3Ticket
        { get { return m_LotteryNumber3Ticket; } set { m_LotteryNumber3Ticket = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int LotteryNumber2Ticket
        { get { return m_LotteryNumber2Ticket; } set { m_LotteryNumber2Ticket = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public int LotteryNumber1Ticket
        { get { return m_LotteryNumber1Ticket; } set { m_LotteryNumber1Ticket = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Administrator)]
        public string TicketStatus
        {
            get
            {
                return m_TicketStatus;
            }
            set
            {
                m_TicketStatus = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public DateTime DateOfDrawOnTickets
        {
            get
            {
                return m_DateOfDrawOnTickets;
            }
            set
            {
                m_DateOfDrawOnTickets = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int TicketNumbers
        {
            get
            {
                return m_TicketNumbers;
            }
            set
            {
                m_TicketNumbers = value;
                InvalidateProperties();
            }
        }

        [Constructable]
        public LotteryTicket( int ticknum, DateTime dateofdraw, int tcksel1, int tcksel2, int tcksel3, int tcksel4, int tcksel5, int tckvalue ) : base(0x14ED)
        {
            Weight = 1.0;
            Name = "Lottery Ticket : " + ticknum + " Drawn on : " + dateofdraw;
            Hue = 1172;
            m_TicketNumbers = ticknum;
            m_DateOfDrawOnTickets = dateofdraw;
            m_TicketStatus = "Valid Ticket";
            m_LotteryNumber1Ticket = tcksel1;
            m_LotteryNumber2Ticket = tcksel2;
            m_LotteryNumber3Ticket = tcksel3;
            m_LotteryNumber4Ticket = tcksel4;
            m_LotteryNumber5Ticket = tcksel5;
            m_LotteryTicketValue = tckvalue;
        }

        public LotteryTicket(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_TicketNumbers);
            writer.Write(m_TicketStatus);
            writer.Write(m_DateOfDrawOnTickets);

            writer.Write(m_LotteryNumber1Ticket);
            writer.Write(m_LotteryNumber2Ticket);
            writer.Write(m_LotteryNumber3Ticket);
            writer.Write(m_LotteryNumber4Ticket);
            writer.Write(m_LotteryNumber5Ticket);

            writer.Write(m_LotteryTicketValue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_TicketNumbers = reader.ReadInt();
            m_TicketStatus = reader.ReadString();
            m_DateOfDrawOnTickets = reader.ReadDateTime();

            m_LotteryNumber1Ticket = reader.ReadInt();
            m_LotteryNumber2Ticket = reader.ReadInt();
            m_LotteryNumber3Ticket = reader.ReadInt();
            m_LotteryNumber4Ticket = reader.ReadInt();
            m_LotteryNumber5Ticket = reader.ReadInt();

            m_LotteryTicketValue = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage(0x35, "The ticket must be in your backpack to view it.");
            }
            else
            {
                from.CloseGump(typeof(LotteryTicketGump));
                from.SendGump(new LotteryTicketGump( this ));
            }
        }
    }

    public class NumberSelectionGump : Gump
    {
        private readonly int ticknumbr = 0;
        private readonly DateTime drawdt = DateTime.Now;

        private int playerselection1 = 0;
        private int playerselection2 = 1;
        private int playerselection3 = 2;
        private int playerselection4 = 3;
        private int playerselection5 = 4;

        private int selectionchoice = 1;
        //private int reset = 1;

        private int display0 = 1;
        private int display1 = 1;
        private int display2 = 1;
        private int display3 = 1;
        private int display4 = 1;
        private int display5 = 1;
        private int display6 = 1;
        private int display7 = 1;
        private int display8 = 1;
        private int display9 = 1;

        public NumberSelectionGump(int ticknumber, DateTime draw, int pds1, int pds2, int pds3, int pds4, int pds5, int sc, int dn0, int dn1, int dn2, int dn3, int dn4, int dn5, int dn6, int dn7, int dn8, int dn9) : base(50, 50)
        {
            drawdt = draw;
            ticknumbr = ticknumber;

            Closable = false;
            Disposable = false;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(0, 0, 437, 406, 83);
            AddBackground(18, 197, 202, 185, 2620);
            AddBackground(18, 59, 399, 106, 2620);
            AddLabel(84, 13, 1160, @"***** The Shard Lottery Game *****");
            AddLabel(152, 35, 1166, @"NUMBER SELECTION");
            AddLabel(291, 335, 85, @"Continue To Ticket");
            AddLabel(291, 357, 85, @"Lucky Dip Selection");

            if (sc >= 6)
            {
                AddButton(251, 334, 4005, 4007, 12, GumpButtonType.Reply, 0); // cont to tick
            }
            AddButton(251, 356, 4017, 4019, 11, GumpButtonType.Reply, 0); // lucky D
            AddLabel(30, 70, 95, @"You are now required to select your 5 numbers for the draw.");
            AddLabel(35, 90, 94, @"The next number in the selection will be displayed in white");
            AddLabel(88, 110, 92, @"or you can try the lucky dip feature now.");
            AddLabel(30, 130, 92, @"The lucky dip will generate 5 random number for your ticket.");
            AddLabel(85, 169, 85, @"Number Selections Are :");

            if (sc == 1)
            {
                AddLabel(80, 208, 1152, @"Select Here");
                AddLabel(50, 238, 268, @"0");
                AddLabel(50, 260, 268, @"1");
                AddLabel(50, 282, 268, @"2");
                AddLabel(50, 304, 268, @"3");
                AddLabel(50, 326, 268, @"4");
                AddLabel(118, 238, 268, @"5");
                AddLabel(118, 260, 268, @"6");
                AddLabel(118, 282, 268, @"7");
                AddLabel(118, 304, 268, @"8");
                AddLabel(118, 326, 268, @"9");

                if (dn0 == 1)
                {
                    AddButton(71, 237, 4014, 4016, 0, GumpButtonType.Reply, 0); // Select 0
                }
                if (dn1 == 1)
                {
                    AddButton(71, 259, 4014, 4016, 1, GumpButtonType.Reply, 0); // Select 1
                }
                if (dn2 == 1)
                {
                    AddButton(71, 281, 4014, 4016, 2, GumpButtonType.Reply, 0); // Select 2
                }
                if (dn3 == 1)
                {
                    AddButton(71, 303, 4014, 4016, 3, GumpButtonType.Reply, 0); // Select 3
                }
                if (dn4 == 1)
                {
                    AddButton(71, 325, 4014, 4016, 4, GumpButtonType.Reply, 0); // Select 4
                }
                if (dn5 == 1)
                {
                    AddButton(139, 237, 4014, 4016, 5, GumpButtonType.Reply, 0); // Select 5
                }
                if (dn6 == 1)
                {
                    AddButton(139, 259, 4014, 4016, 6, GumpButtonType.Reply, 0); // Select 6
                }
                if (dn7 == 1)
                {
                    AddButton(139, 281, 4014, 4016, 7, GumpButtonType.Reply, 0); // Select 7
                }
                if (dn8 == 1)
                {
                    AddButton(139, 303, 4014, 4016, 8, GumpButtonType.Reply, 0); // Select 8
                }
                if (dn9 == 1)
                {
                    AddButton(139, 325, 4014, 4016, 9, GumpButtonType.Reply, 0); // Select 9
                }

                AddLabel(250, 169, 1152, pds1.ToString());
                AddLabel(270, 169, 55, pds2.ToString());
                AddLabel(290, 169, 55, pds3.ToString());
                AddLabel(310, 169, 55, pds4.ToString());
                AddLabel(330, 169, 55, pds5.ToString());
            }
            else if (sc == 2)
            {
                AddLabel(80, 208, 1152, @"Select Here");
                AddLabel(50, 238, 268, @"0");
                AddLabel(50, 260, 268, @"1");
                AddLabel(50, 282, 268, @"2");
                AddLabel(50, 304, 268, @"3");
                AddLabel(50, 326, 268, @"4");
                AddLabel(118, 238, 268, @"5");
                AddLabel(118, 260, 268, @"6");
                AddLabel(118, 282, 268, @"7");
                AddLabel(118, 304, 268, @"8");
                AddLabel(118, 326, 268, @"9");

                if (dn0 == 1)
                {
                    AddButton(71, 237, 4014, 4016, 0, GumpButtonType.Reply, 0); // Select 0
                }
                if (dn1 == 1)
                {
                    AddButton(71, 259, 4014, 4016, 1, GumpButtonType.Reply, 0); // Select 1
                }
                if (dn2 == 1)
                {
                    AddButton(71, 281, 4014, 4016, 2, GumpButtonType.Reply, 0); // Select 2
                }
                if (dn3 == 1)
                {
                    AddButton(71, 303, 4014, 4016, 3, GumpButtonType.Reply, 0); // Select 3
                }
                if (dn4 == 1)
                {
                    AddButton(71, 325, 4014, 4016, 4, GumpButtonType.Reply, 0); // Select 4
                }
                if (dn5 == 1)
                {
                    AddButton(139, 237, 4014, 4016, 5, GumpButtonType.Reply, 0); // Select 5
                }
                if (dn6 == 1)
                {
                    AddButton(139, 259, 4014, 4016, 6, GumpButtonType.Reply, 0); // Select 6
                }
                if (dn7 == 1)
                {
                    AddButton(139, 281, 4014, 4016, 7, GumpButtonType.Reply, 0); // Select 7
                }
                if (dn8 == 1)
                {
                    AddButton(139, 303, 4014, 4016, 8, GumpButtonType.Reply, 0); // Select 8
                }
                if (dn9 == 1)
                {
                    AddButton(139, 325, 4014, 4016, 9, GumpButtonType.Reply, 0); // Select 9
                }

                AddLabel(250, 169, 55, pds1.ToString());
                AddLabel(270, 169, 1152, pds2.ToString());
                AddLabel(290, 169, 55, pds3.ToString());
                AddLabel(310, 169, 55, pds4.ToString());
                AddLabel(330, 169, 55, pds5.ToString());
            }
            else if (sc == 3)
            {
                AddLabel(80, 208, 1152, @"Select Here");
                AddLabel(50, 238, 268, @"0");
                AddLabel(50, 260, 268, @"1");
                AddLabel(50, 282, 268, @"2");
                AddLabel(50, 304, 268, @"3");
                AddLabel(50, 326, 268, @"4");
                AddLabel(118, 238, 268, @"5");
                AddLabel(118, 260, 268, @"6");
                AddLabel(118, 282, 268, @"7");
                AddLabel(118, 304, 268, @"8");
                AddLabel(118, 326, 268, @"9");

                if (dn0 == 1)
                {
                    AddButton(71, 237, 4014, 4016, 0, GumpButtonType.Reply, 0); // Select 0
                }
                if (dn1 == 1)
                {
                    AddButton(71, 259, 4014, 4016, 1, GumpButtonType.Reply, 0); // Select 1
                }
                if (dn2 == 1)
                {
                    AddButton(71, 281, 4014, 4016, 2, GumpButtonType.Reply, 0); // Select 2
                }
                if (dn3 == 1)
                {
                    AddButton(71, 303, 4014, 4016, 3, GumpButtonType.Reply, 0); // Select 3
                }
                if (dn4 == 1)
                {
                    AddButton(71, 325, 4014, 4016, 4, GumpButtonType.Reply, 0); // Select 4
                }
                if (dn5 == 1)
                {
                    AddButton(139, 237, 4014, 4016, 5, GumpButtonType.Reply, 0); // Select 5
                }
                if (dn6 == 1)
                {
                    AddButton(139, 259, 4014, 4016, 6, GumpButtonType.Reply, 0); // Select 6
                }
                if (dn7 == 1)
                {
                    AddButton(139, 281, 4014, 4016, 7, GumpButtonType.Reply, 0); // Select 7
                }
                if (dn8 == 1)
                {
                    AddButton(139, 303, 4014, 4016, 8, GumpButtonType.Reply, 0); // Select 8
                }
                if (dn9 == 1)
                {
                    AddButton(139, 325, 4014, 4016, 9, GumpButtonType.Reply, 0); // Select 9
                }

                AddLabel(250, 169, 55, pds1.ToString());
                AddLabel(270, 169, 55, pds2.ToString());
                AddLabel(290, 169, 1152, pds3.ToString());
                AddLabel(310, 169, 55, pds4.ToString());
                AddLabel(330, 169, 55, pds5.ToString());
            }
            else if (sc == 4)
            {
                AddLabel(80, 208, 1152, @"Select Here");
                AddLabel(50, 238, 268, @"0");
                AddLabel(50, 260, 268, @"1");
                AddLabel(50, 282, 268, @"2");
                AddLabel(50, 304, 268, @"3");
                AddLabel(50, 326, 268, @"4");
                AddLabel(118, 238, 268, @"5");
                AddLabel(118, 260, 268, @"6");
                AddLabel(118, 282, 268, @"7");
                AddLabel(118, 304, 268, @"8");
                AddLabel(118, 326, 268, @"9");

                if (dn0 == 1)
                {
                    AddButton(71, 237, 4014, 4016, 0, GumpButtonType.Reply, 0); // Select 0
                }
                if (dn1 == 1)
                {
                    AddButton(71, 259, 4014, 4016, 1, GumpButtonType.Reply, 0); // Select 1
                }
                if (dn2 == 1)
                {
                    AddButton(71, 281, 4014, 4016, 2, GumpButtonType.Reply, 0); // Select 2
                }
                if (dn3 == 1)
                {
                    AddButton(71, 303, 4014, 4016, 3, GumpButtonType.Reply, 0); // Select 3
                }
                if (dn4 == 1)
                {
                    AddButton(71, 325, 4014, 4016, 4, GumpButtonType.Reply, 0); // Select 4
                }
                if (dn5 == 1)
                {
                    AddButton(139, 237, 4014, 4016, 5, GumpButtonType.Reply, 0); // Select 5
                }
                if (dn6 == 1)
                {
                    AddButton(139, 259, 4014, 4016, 6, GumpButtonType.Reply, 0); // Select 6
                }
                if (dn7 == 1)
                {
                    AddButton(139, 281, 4014, 4016, 7, GumpButtonType.Reply, 0); // Select 7
                }
                if (dn8 == 1)
                {
                    AddButton(139, 303, 4014, 4016, 8, GumpButtonType.Reply, 0); // Select 8
                }
                if (dn9 == 1)
                {
                    AddButton(139, 325, 4014, 4016, 9, GumpButtonType.Reply, 0); // Select 9
                }

                AddLabel(250, 169, 55, pds1.ToString());
                AddLabel(270, 169, 55, pds2.ToString());
                AddLabel(290, 169, 55, pds3.ToString());
                AddLabel(310, 169, 1152, pds4.ToString());
                AddLabel(330, 169, 55, pds5.ToString());
            }
            else if (sc == 5)
            {
                AddLabel(80, 208, 1152, @"Select Here");
                AddLabel(50, 238, 268, @"0");
                AddLabel(50, 260, 268, @"1");
                AddLabel(50, 282, 268, @"2");
                AddLabel(50, 304, 268, @"3");
                AddLabel(50, 326, 268, @"4");
                AddLabel(118, 238, 268, @"5");
                AddLabel(118, 260, 268, @"6");
                AddLabel(118, 282, 268, @"7");
                AddLabel(118, 304, 268, @"8");
                AddLabel(118, 326, 268, @"9");

                if (dn0 == 1)
                {
                    AddButton(71, 237, 4014, 4016, 0, GumpButtonType.Reply, 0); // Select 0
                }
                if (dn1 == 1)
                {
                    AddButton(71, 259, 4014, 4016, 1, GumpButtonType.Reply, 0); // Select 1
                }
                if (dn2 == 1)
                {
                    AddButton(71, 281, 4014, 4016, 2, GumpButtonType.Reply, 0); // Select 2
                }
                if (dn3 == 1)
                {
                    AddButton(71, 303, 4014, 4016, 3, GumpButtonType.Reply, 0); // Select 3
                }
                if (dn4 == 1)
                {
                    AddButton(71, 325, 4014, 4016, 4, GumpButtonType.Reply, 0); // Select 4
                }
                if (dn5 == 1)
                {
                    AddButton(139, 237, 4014, 4016, 5, GumpButtonType.Reply, 0); // Select 5
                }
                if (dn6 == 1)
                {
                    AddButton(139, 259, 4014, 4016, 6, GumpButtonType.Reply, 0); // Select 6
                }
                if (dn7 == 1)
                {
                    AddButton(139, 281, 4014, 4016, 7, GumpButtonType.Reply, 0); // Select 7
                }
                if (dn8 == 1)
                {
                    AddButton(139, 303, 4014, 4016, 8, GumpButtonType.Reply, 0); // Select 8
                }
                if (dn9 == 1)
                {
                    AddButton(139, 325, 4014, 4016, 9, GumpButtonType.Reply, 0); // Select 9
                }

                AddLabel(250, 169, 55, pds1.ToString());
                AddLabel(270, 169, 55, pds2.ToString());
                AddLabel(290, 169, 55, pds3.ToString());
                AddLabel(310, 169, 55, pds4.ToString());
                AddLabel(330, 169, 1152, pds5.ToString());
            }
            else if (sc >= 6)
            {
                AddLabel(250, 169, 1152, pds1.ToString());
                AddLabel(270, 169, 1152, pds2.ToString());
                AddLabel(290, 169, 1152, pds3.ToString());
                AddLabel(310, 169, 1152, pds4.ToString());
                AddLabel(330, 169, 1152, pds5.ToString());
            }

            AddButton(251, 312, 4029, 4031, 10, GumpButtonType.Reply, 0); // Reset            
            AddLabel(291, 313, 90, @"Reset Selection");

            playerselection1 = pds1;
            playerselection2 = pds2;
            playerselection3 = pds3;
            playerselection4 = pds4;
            playerselection5 = pds5;

            selectionchoice = sc;

            display0 = dn0;
            display1 = dn1;
            display2 = dn2;
            display3 = dn3;
            display4 = dn4;
            display5 = dn5;
            display6 = dn6;
            display7 = dn7;
            display8 = dn8;
            display9 = dn9;
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            Container pack = from.Backpack;

            if (selectionchoice == 1)
            {
                switch (info.ButtonID)
                {
                    case 0: // Pick 0
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection1 = 0;         
                            selectionchoice++;
                            display0 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 1: // Pick 1
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection1 = 1;
                            selectionchoice++;
                            display1 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 2: // Pick 2
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection1 = 2;
                            selectionchoice++;
                            display2 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 3: // Pick 3
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection1 = 3;
                            selectionchoice++;
                            display3 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 4: // Pick 4
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection1 = 4;
                            selectionchoice++;
                            display4 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 5: // Pick 5
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection1 = 5;
                            selectionchoice++;
                            display5 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 6: // Pick 6
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection1 = 6;
                            selectionchoice++;
                            display6 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 7: // Pick 7
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection1 = 7;
                            selectionchoice++;
                            display7 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 8: // Pick 8
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection1 = 8;
                            selectionchoice++;
                            display8 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 9: // Pick 9
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection1 = 9;
                            selectionchoice++;
                            display9 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 10: // Reset Number Selection Process
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            playerselection1 = 0;
                            playerselection2 = 1;
                            playerselection3 = 2;
                            playerselection4 = 3;
                            playerselection5 = 4;
                            selectionchoice = 1;
                            display0 = 1;
                            display1 = 1;
                            display2 = 1;
                            display3 = 1;
                            display4 = 1;
                            display5 = 1;
                            display6 = 1;
                            display7 = 1;
                            display8 = 1;
                            display9 = 1;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 11: // Lucky Dip Ticket Selection
                        {
                            from.CloseGump(typeof(NumberSelectionGump));

                            // *************************************************
                            // This makes sure theres never 2 of the same number
                            // *************************************************
                            playerselection1 = (Utility.RandomMinMax(0, 9));
                            do
                            {
                                playerselection2 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection1 == playerselection2);
                            do
                            {
                                playerselection3 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection2 == playerselection3 | playerselection1 == playerselection3);
                            do
                            {
                                playerselection4 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection3 == playerselection4 | playerselection2 == playerselection4 | playerselection1 == playerselection4);
                            do
                            {
                                playerselection5 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection4 == playerselection5 | playerselection3 == playerselection5 | playerselection2 == playerselection5 | playerselection1 == playerselection5);

                            selectionchoice = 6;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 12: // Give Ticket With Selected Numbers
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            from.AddToBackpack(new LotteryTicket(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, ticknumbr));
                            from.SendMessage(0x35, "You recieve your lottery ticket, Good Luck.");
                        }
                        break;
                }
            }

            else if (selectionchoice == 2)
            {
                switch (info.ButtonID)
                {
                    case 0: // Pick 0
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection2 = 0;
                            selectionchoice++;
                            display0 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 1: // Pick 1
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection2 = 1;
                            selectionchoice++;
                            display1 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 2: // Pick 2
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection2 = 2;
                            selectionchoice++;
                            display2 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 3: // Pick 3
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection2 = 3;
                            selectionchoice++;
                            display3 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 4: // Pick 4
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection2 = 4;
                            selectionchoice++;
                            display4 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 5: // Pick 5
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection2 = 5;
                            selectionchoice++;
                            display5 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 6: // Pick 6
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection2 = 6;
                            selectionchoice++;
                            display6 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 7: // Pick 7
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection2 = 7;
                            selectionchoice++;
                            display7 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 8: // Pick 8
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection2 = 8;
                            selectionchoice++;
                            display8 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 9: // Pick 9
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection2 = 9;
                            selectionchoice++;
                            display9 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 10: // Reset Number Selection Process
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            playerselection1 = 0;
                            playerselection2 = 1;
                            playerselection3 = 2;
                            playerselection4 = 3;
                            playerselection5 = 4;
                            selectionchoice = 1;
                            display0 = 1;
                            display1 = 1;
                            display2 = 1;
                            display3 = 1;
                            display4 = 1;
                            display5 = 1;
                            display6 = 1;
                            display7 = 1;
                            display8 = 1;
                            display9 = 1;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 11: // Lucky Dip Ticket Selection
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            // *************************************************
                            // This makes sure theres never 2 of the same number
                            // *************************************************
                            playerselection1 = (Utility.RandomMinMax(0, 9));
                            do
                            {
                                playerselection2 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection1 == playerselection2);
                            do
                            {
                                playerselection3 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection2 == playerselection3 | playerselection1 == playerselection3);
                            do
                            {
                                playerselection4 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection3 == playerselection4 | playerselection2 == playerselection4 | playerselection1 == playerselection4);
                            do
                            {
                                playerselection5 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection4 == playerselection5 | playerselection3 == playerselection5 | playerselection2 == playerselection5 | playerselection1 == playerselection5);
                            selectionchoice = 6;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 12: // Give Ticket With Selected Numbers
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            from.AddToBackpack(new LotteryTicket(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, ticknumbr));
                            from.SendMessage(0x35, "You recieve your lottery ticket, Good Luck.");
                        }
                        break;
                }
            }

            else if (selectionchoice == 3)
            {
                switch (info.ButtonID)
                {
                    case 0: // Pick 0
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection3 = 0;
                            selectionchoice++;
                            display0 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 1: // Pick 1
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection3 = 1;
                            selectionchoice++;
                            display1 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 2: // Pick 2
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection3 = 2;
                            selectionchoice++;
                            display2 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 3: // Pick 3
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection3 = 3;
                            selectionchoice++;
                            display3 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 4: // Pick 4
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection3 = 4;
                            selectionchoice++;
                            display4 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 5: // Pick 5
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection3 = 5;
                            selectionchoice++;
                            display5 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 6: // Pick 6
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection3 = 6;
                            selectionchoice++;
                            display6 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 7: // Pick 7
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection3 = 7;
                            selectionchoice++;
                            display7 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 8: // Pick 8
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection3 = 8;
                            selectionchoice++;
                            display8 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 9: // Pick 9
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection3 = 9;
                            selectionchoice++;
                            display9 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 10: // Reset Number Selection Process
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            playerselection1 = 0;
                            playerselection2 = 1;
                            playerselection3 = 2;
                            playerselection4 = 3;
                            playerselection5 = 4;
                            selectionchoice = 1;
                            display0 = 1;
                            display1 = 1;
                            display2 = 1;
                            display3 = 1;
                            display4 = 1;
                            display5 = 1;
                            display6 = 1;
                            display7 = 1;
                            display8 = 1;
                            display9 = 1;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 11: // Lucky Dip Ticket Selection
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            // *************************************************
                            // This makes sure theres never 2 of the same number
                            // *************************************************
                            playerselection1 = (Utility.RandomMinMax(0, 9));
                            do
                            {
                                playerselection2 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection1 == playerselection2);
                            do
                            {
                                playerselection3 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection2 == playerselection3 | playerselection1 == playerselection3);
                            do
                            {
                                playerselection4 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection3 == playerselection4 | playerselection2 == playerselection4 | playerselection1 == playerselection4);
                            do
                            {
                                playerselection5 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection4 == playerselection5 | playerselection3 == playerselection5 | playerselection2 == playerselection5 | playerselection1 == playerselection5);
                            selectionchoice = 6;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 12: // Give Ticket With Selected Numbers
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            from.AddToBackpack(new LotteryTicket(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, ticknumbr));
                            from.SendMessage(0x35, "You recieve your lottery ticket, Good Luck.");
                        }
                        break;
                }
            }

            else if (selectionchoice == 4)
            {
                switch (info.ButtonID)
                {
                    case 0: // Pick 0
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection4 = 0;
                            selectionchoice++;
                            display0 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 1: // Pick 1
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection4 = 1;
                            selectionchoice++;
                            display1 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 2: // Pick 2
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection4 = 2;
                            selectionchoice++;
                            display2 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 3: // Pick 3
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection4 = 3;
                            selectionchoice++;
                            display3 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 4: // Pick 4
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection4 = 4;
                            selectionchoice++;
                            display4 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 5: // Pick 5
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection4 = 5;
                            selectionchoice++;
                            display5 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 6: // Pick 6
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection4 = 6;
                            selectionchoice++;
                            display6 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 7: // Pick 7
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection4 = 7;
                            selectionchoice++;
                            display7 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 8: // Pick 8
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection4 = 8;
                            selectionchoice++;
                            display8 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 9: // Pick 9
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection4 = 9;
                            selectionchoice++;
                            display9 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 10: // Reset Number Selection Process
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            playerselection1 = 0;
                            playerselection2 = 1;
                            playerselection3 = 2;
                            playerselection4 = 3;
                            playerselection5 = 4;
                            selectionchoice = 1;
                            display0 = 1;
                            display1 = 1;
                            display2 = 1;
                            display3 = 1;
                            display4 = 1;
                            display5 = 1;
                            display6 = 1;
                            display7 = 1;
                            display8 = 1;
                            display9 = 1;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 11: // Lucky Dip Ticket Selection
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            // *************************************************
                            // This makes sure theres never 2 of the same number
                            // *************************************************
                            playerselection1 = (Utility.RandomMinMax(0, 9));
                            do
                            {
                                playerselection2 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection1 == playerselection2);
                            do
                            {
                                playerselection3 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection2 == playerselection3 | playerselection1 == playerselection3);
                            do
                            {
                                playerselection4 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection3 == playerselection4 | playerselection2 == playerselection4 | playerselection1 == playerselection4);
                            do
                            {
                                playerselection5 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection4 == playerselection5 | playerselection3 == playerselection5 | playerselection2 == playerselection5 | playerselection1 == playerselection5);
                            selectionchoice = 6;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 12: // Give Ticket With Selected Numbers
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            from.AddToBackpack(new LotteryTicket(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, ticknumbr));
                            from.SendMessage(0x35, "You recieve your lottery ticket, Good Luck.");
                        }
                        break;
                }
            }

            else if (selectionchoice == 5)
            {
                switch (info.ButtonID)
                {
                    case 0: // Pick 0
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection5 = 0;
                            selectionchoice++;
                            display0 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 1: // Pick 1
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection5 = 1;
                            selectionchoice++;
                            display1 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 2: // Pick 2
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection5 = 2;
                            selectionchoice++;
                            display2 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 3: // Pick 3
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection5 = 3;
                            selectionchoice++;
                            display3 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 4: // Pick 4
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection5 = 4;
                            selectionchoice++;
                            display4 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 5: // Pick 5
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection5 = 5;
                            selectionchoice++;
                            display5 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 6: // Pick 6
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection5 = 6;
                            selectionchoice++;
                            display6 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 7: // Pick 7
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection5 = 7;
                            selectionchoice++;
                            display7 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 8: // Pick 8
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection5 = 8;
                            selectionchoice++;
                            display8 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 9: // Pick 9
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            int playerselection5 = 9;
                            selectionchoice++;
                            display9 = 0;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 10: // Reset Number Selection Process
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            playerselection1 = 0;
                            playerselection2 = 1;
                            playerselection3 = 2;
                            playerselection4 = 3;
                            playerselection5 = 4;
                            selectionchoice = 1;
                            display0 = 1;
                            display1 = 1;
                            display2 = 1;
                            display3 = 1;
                            display4 = 1;
                            display5 = 1;
                            display6 = 1;
                            display7 = 1;
                            display8 = 1;
                            display9 = 1;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 11: // Lucky Dip Ticket Selection
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            // *************************************************
                            // This makes sure theres never 2 of the same number
                            // *************************************************
                            playerselection1 = (Utility.RandomMinMax(0, 9));
                            do
                            {
                                playerselection2 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection1 == playerselection2);
                            do
                            {
                                playerselection3 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection2 == playerselection3 | playerselection1 == playerselection3);
                            do
                            {
                                playerselection4 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection3 == playerselection4 | playerselection2 == playerselection4 | playerselection1 == playerselection4);
                            do
                            {
                                playerselection5 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection4 == playerselection5 | playerselection3 == playerselection5 | playerselection2 == playerselection5 | playerselection1 == playerselection5);
                            selectionchoice = 6;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 12: // Give Ticket With Selected Numbers
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            from.AddToBackpack(new LotteryTicket(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, ticknumbr));
                            from.SendMessage(0x35, "You recieve your lottery ticket, Good Luck.");
                        }
                        break;
                }
            }

            else if (selectionchoice >= 6)
            {
                switch (info.ButtonID)
                {
                    case 0: // Pick 0
                        {
                        }
                        break;
                    case 1: // Pick 1
                        {
                        }
                        break;
                    case 2: // Pick 2
                        {
                        }
                        break;
                    case 3: // Pick 3
                        {
                        }
                        break;
                    case 4: // Pick 4
                        {
                        }
                        break;
                    case 5: // Pick 5
                        {
                        }
                        break;
                    case 6: // Pick 6
                        {
                        }
                        break;
                    case 7: // Pick 7
                        {
                        }
                        break;
                    case 8: // Pick 8
                        {
                        }
                        break;
                    case 9: // Pick 9
                        {
                        }
                        break;
                    case 10: // Reset Number Selection Process
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            playerselection1 = 0;
                            playerselection2 = 1;
                            playerselection3 = 2;
                            playerselection4 = 3;
                            playerselection5 = 4;
                            selectionchoice = 1;
                            display0 = 1;
                            display1 = 1;
                            display2 = 1;
                            display3 = 1;
                            display4 = 1;
                            display5 = 1;
                            display6 = 1;
                            display7 = 1;
                            display8 = 1;
                            display9 = 1;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 11: // Lucky Dip Ticket Selection
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            // *************************************************
                            // This makes sure theres never 2 of the same number
                            // *************************************************
                            playerselection1 = (Utility.RandomMinMax(0, 9));
                            do
                            {
                                playerselection2 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection1 == playerselection2);
                            do
                            {
                                playerselection3 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection2 == playerselection3 | playerselection1 == playerselection3);
                            do
                            {
                                playerselection4 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection3 == playerselection4 | playerselection2 == playerselection4 | playerselection1 == playerselection4);
                            do
                            {
                                playerselection5 = (Utility.RandomMinMax(0, 9));
                            }
                            while (playerselection4 == playerselection5 | playerselection3 == playerselection5 | playerselection2 == playerselection5 | playerselection1 == playerselection5);
                            selectionchoice = 6;
                            from.SendGump(new NumberSelectionGump(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, selectionchoice, display0, display1, display2, display3, display4, display5, display6, display7, display8, display9));
                        }
                        break;
                    case 12: // Give Ticket With Selected Numbers
                        {
                            from.CloseGump(typeof(NumberSelectionGump));
                            from.AddToBackpack(new LotteryTicket(ticknumbr, drawdt, playerselection1, playerselection2, playerselection3, playerselection4, playerselection5, ticknumbr));
                            from.SendMessage(0x35, "You recieve your lottery ticket, Good Luck.");
                        }
                        break;
                }
            }
        }
    }

    // ***********************************************************************************************************
    // ***********************************************************************************************************
    // ***********************************************************************************************************
    // Timer code for the lottery system...
    // Once the scripts are installed to your servers script folders this code will run.
    // The code will STOP the timer for the lottery the first time this system is run, since there was
    // NO LOTTERY STONE.
    // You will have to place your 1st AND ONLY lottery stone on your shard, then you need to re-start your shard.
    // The timer code checks for Lottery Stones on a shard, IF you create MORE THAN ONE LOTTERY STONE, the timer
    // WILL BE STOPPED.
    // Your shard can ONLY HAVE ONE LOTTERY STONE.
    // if you create more than one stone the timer WILL BE STOPPED.
    // ***********************************************************************************************************
    // ***********************************************************************************************************
    // ***********************************************************************************************************

    public class LotteryTimer : Timer
    {
        public static void Initialize()
        {
            new LotteryTimer();
        }

        // *****************************************************************************************
        // *****************************************************************************************
        // *****************************************************************************************
        // This is where to set the LOTTERY MESSAGE TIMES, this is the amount of time before players
        // see the next check message... TimeSpan.FromMinutes(20)
        // This lottery is setup to display the draw messages at intervals of 20 minutes

        public LotteryTimer() : base(TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(60))

        // *****************************************************************************************
        // *****************************************************************************************
        // *****************************************************************************************
        {
            Start();
        }
        protected override void OnTick()
        {
            World.Broadcast(0, true, "Lottery System Message...");
            Console.WriteLine("Lottery System Tick.");

            ArrayList listLottoStones = new ArrayList();
            foreach (Item item in World.Items.Values)
            {
                if (item is LotteryStone)
                    listLottoStones.Add(item);
            }

            if (listLottoStones.Count > 1)
            {
                World.Broadcast(0x35, true, "Auto Shutdown, Only ONE stone allowed in the world.");
                Console.WriteLine("Lottery System Shutdown, Only ONE stone allowed in the world.");
                Stop();
            }

            if (listLottoStones.Count == 0)
            {
                World.Broadcast(0x35, true, "Auto Shutdown, No Lottery Stones in The World.");
                Console.WriteLine("Lottery System Shutdown, No Lottery Stone Found.");
                Stop();
            }

            foreach (Item item in World.Items.Values)
            {
                if (item is LotteryStone)
                {
                    LotteryStone thislotterystone = (LotteryStone)item;

                    DateTime timeofdraw = thislotterystone.DateOfNextDraw;
                    int thislotteryjackpot = thislotterystone.LotteryGoldTaken;
                    int thisrolloveramount = thislotterystone.RollOverAmount;

                    int splt5 = thislotterystone.Split5; // 50% given to a 5 matching numbers winner
                    int splt4 = thislotterystone.Split4; // 20% given to a 4 matching numbers winner
                    int splt3 = thislotterystone.Split3; // 15% given to a 3 matching numbers winner
                    int splt2 = thislotterystone.Split2; // 10% given to a 2 matching numbers winner
                    int splt1 = thislotterystone.Split1; // 5% given to a 1 matched number winner

                    World.Broadcast(0, true, "System Date/Time : {0}", DateTime.Now);
                    World.Broadcast(0, true, "Lottery Date/Time : {0}", timeofdraw);

                    if (DateTime.Now >= timeofdraw)
                    {
                        World.Broadcast(0x36, true, "Lottery Draw Time, Generating 5 Random Lottery Numbers...");

                        // *************************************************
                        // This makes sure theres never 2 of the same number
                        // *************************************************
                        int winnumber1 = 1;
                        int winnumber2 = 2;
                        int winnumber3 = 3;
                        int winnumber4 = 4;
                        int winnumber5 = 5;

                        winnumber1 = (Utility.RandomMinMax(0, 9));
                        do
                        {
                            winnumber2 = (Utility.RandomMinMax(0, 9));
                        }
                        while (winnumber1 == winnumber2);
                        do
                        {
                           winnumber3 = (Utility.RandomMinMax(0, 9));
                        }
                        while (winnumber2 == winnumber3 | winnumber1 == winnumber3);
                        do
                        {
                           winnumber4 = (Utility.RandomMinMax(0, 9));
                        }
                        while (winnumber3 == winnumber4 | winnumber2 == winnumber4 | winnumber1 == winnumber4);
                        do
                        {
                           winnumber5 = (Utility.RandomMinMax(0, 9));
                        }
                        while (winnumber4 == winnumber5 | winnumber3 == winnumber5 | winnumber2 == winnumber5 | winnumber1 == winnumber5);

                        // ***********
                        // FOR TESTING
                        // ***********
                        // int winnumber1 = 1;
                        // int winnumber2 = 2;
                        // int winnumber3 = 3;
                        // int winnumber4 = 4;
                        // int winnumber5 = 5;

                        int ticketmatches = 0;
                        int totalwinningtickets = 0;
                        int JackpotTotalGold = 0;

                        int onenumberwinners = 0;
                        int twonumberwinners = 0;
                        int threenumberwinners = 0;
                        int fournumberwinners = 0;
                        int fivenumberwinners = 0;

                        World.Broadcast(0x55, true, "Lottery Numbers Are : {0}, {1}, {2}, {3}, {4}", winnumber1, winnumber2, winnumber3, winnumber4, winnumber5);
                        World.Broadcast(0x35, true, "Checking Tickets For Winners...");

                        ArrayList listtickets = new ArrayList();
                        foreach (Item tckt in World.Items.Values)
                        {
                            if (tckt is LotteryTicket)
                            {
                                LotteryTicket thisticket = (LotteryTicket)tckt;
                                listtickets.Add(tckt);

                                if (thisticket.TicketStatus == "Valid Ticket")
                                {
                                    if (thisticket.LotteryNumber1Ticket == winnumber1 | thisticket.LotteryNumber1Ticket == winnumber2 | thisticket.LotteryNumber1Ticket == winnumber3 | thisticket.LotteryNumber1Ticket == winnumber4 | thisticket.LotteryNumber1Ticket == winnumber5)
                                    {
                                        ticketmatches++;
                                    }

                                    if (thisticket.LotteryNumber2Ticket == winnumber1 | thisticket.LotteryNumber2Ticket == winnumber2 | thisticket.LotteryNumber2Ticket == winnumber3 | thisticket.LotteryNumber2Ticket == winnumber4 | thisticket.LotteryNumber2Ticket == winnumber5)
                                    {
                                        ticketmatches++;
                                    }

                                    if (thisticket.LotteryNumber3Ticket == winnumber1 | thisticket.LotteryNumber3Ticket == winnumber2 | thisticket.LotteryNumber3Ticket == winnumber3 | thisticket.LotteryNumber3Ticket == winnumber4 | thisticket.LotteryNumber3Ticket == winnumber5)
                                    {
                                        ticketmatches++;
                                    }

                                    if (thisticket.LotteryNumber4Ticket == winnumber1 | thisticket.LotteryNumber4Ticket == winnumber2 | thisticket.LotteryNumber4Ticket == winnumber3 | thisticket.LotteryNumber4Ticket == winnumber4 | thisticket.LotteryNumber4Ticket == winnumber5)
                                    {
                                        ticketmatches++;
                                    }

                                    if (thisticket.LotteryNumber5Ticket == winnumber1 | thisticket.LotteryNumber5Ticket == winnumber2 | thisticket.LotteryNumber5Ticket == winnumber3 | thisticket.LotteryNumber5Ticket == winnumber4 | thisticket.LotteryNumber5Ticket == winnumber5)
                                    {
                                        ticketmatches++;
                                    }

                                    if (ticketmatches == 0)
                                    {
                                        thisticket.TicketStatus = "Void / Old Ticket";
                                        thisticket.Hue = 31;
                                    }
                                    if (ticketmatches == 1)
                                    {
                                        onenumberwinners++;
                                        thisticket.TicketStatus = "1 Matched Number";
                                        thisticket.Hue = 2212;
                                    }
                                    if (ticketmatches == 2)
                                    {
                                        twonumberwinners++;
                                        thisticket.TicketStatus = "2 Matched Numbers";
                                        thisticket.Hue = 2211;
                                    }
                                    if (ticketmatches == 3)
                                    {
                                        threenumberwinners++;
                                        thisticket.TicketStatus = "3 Matched Numbers";
                                        thisticket.Hue = 2210;
                                    }
                                    if (ticketmatches == 4)
                                    {
                                        fournumberwinners++;
                                        thisticket.TicketStatus = "4 Matched Numbers";
                                        thisticket.Hue = 2209;
                                    }
                                    if (ticketmatches == 5)
                                    {
                                        fivenumberwinners++;
                                        thisticket.TicketStatus = "5 Matched Numbers";
                                        thisticket.Hue = 2208;
                                    }
                                }
                                ticketmatches = 0;

                                // Void all old tickets
                                if (thisticket.TicketStatus == "1 Matched Number" && thisticket.DateOfDrawOnTickets != thislotterystone.DateOfNextDraw)
                                {
                                    thisticket.TicketStatus = "Void / Old Ticket";
                                    thisticket.Hue = 31;
                                }
                                if (thisticket.TicketStatus == "2 Matched Numbers" && thisticket.DateOfDrawOnTickets != thislotterystone.DateOfNextDraw)
                                {
                                    thisticket.TicketStatus = "Void / Old Ticket";
                                    thisticket.Hue = 31;
                                }
                                if (thisticket.TicketStatus == "3 Matched Numbers" && thisticket.DateOfDrawOnTickets != thislotterystone.DateOfNextDraw)
                                {
                                    thisticket.TicketStatus = "Void / Old Ticket";
                                    thisticket.Hue = 31;
                                }
                                if (thisticket.TicketStatus == "4 Matched Numbers" && thisticket.DateOfDrawOnTickets != thislotterystone.DateOfNextDraw)
                                {
                                    thisticket.TicketStatus = "Void / Old Ticket";
                                    thisticket.Hue = 31;
                                }
                                if (thisticket.TicketStatus == "5 Matched Numbers" && thisticket.DateOfDrawOnTickets != thislotterystone.DateOfNextDraw)
                                {
                                    thisticket.TicketStatus = "Void / Old Ticket";
                                    thisticket.Hue = 31;
                                }
                            }
                        }

                        World.Broadcast(0x35, true, "Results...");
                        World.Broadcast(0x36, true, "Five Numbers Matched : {0}", fivenumberwinners); 
                        World.Broadcast(0x36, true, "Four Numbers Matched : {0}", fournumberwinners); 
                        World.Broadcast(0x36, true, "Three Numbers Matched : {0}", threenumberwinners); 
                        World.Broadcast(0x36, true, "Two Numbers Matched : {0}", twonumberwinners); 
                        World.Broadcast(0x36, true, "One Number Matched : {0}", onenumberwinners); 
                        totalwinningtickets = fivenumberwinners + fournumberwinners + threenumberwinners + twonumberwinners + onenumberwinners;
                        World.Broadcast(0x36, true, "Total Winning Tickets : {0}", totalwinningtickets); 

                        // ********************************************************************
                        // NO ROLLOVER.
                        // No tickets had ALL 5 numbers.
                        // Add this lottery jackpot to the stones current RollOver
                        // so its avaliable for the NEXT lottery draw.
                        // The Jackpot for this lottery is only the ticket sales amount of gold
                        // ********************************************************************
                        if (fivenumberwinners == 0)
                        {
                            thislotterystone.RollOverAmount = thisrolloveramount + thislotteryjackpot;
                            JackpotTotalGold = thislotteryjackpot;
                        }
                        // ************************************************************
                        // ROLLOVER HAS BEEN WON BY...
                        // There is a 5 number match so the RollOver
                        // gets reset for the NEXT lottery draw.
                        // Because there is a 5 draw number match the Rollover is added
                        // to the ticket sales amount of gold.
                        // ************************************************************
                        if (fivenumberwinners > 0)
                        {
                            thislotterystone.RollOverAmount = 0;
                            JackpotTotalGold = thislotteryjackpot + thisrolloveramount;
                        }
                        // ******************************************
                        // Calculate 1% of the total win amount
                        // for the calculations below.
                        // ******************************************
                        int onepercent = JackpotTotalGold / 100;
                        // **************************************************
                        // This is the % split of the total win amount (100%)
                        // between the 5 winning types of tickets. The total
                        // of the 5 basesplits should ALWAYS = 100.
                        // By default this is...  
                        // 50% given to a 5 matching numbers winner
                        // 20% given to a 4 matching numbers winner
                        // 15% given to a 3 matching numbers winner
                        // 10% given to a 2 matching numbers winner
                        // 5% given to a 1 matched number winner
                        // **************************************************
                        int tempwin5 = onepercent * splt5;
                        int tempwin4 = onepercent * splt4;
                        int tempwin3 = onepercent * splt3;
                        int tempwin2 = onepercent * splt2;
                        int tempwin1 = onepercent * splt1;

                        if (fivenumberwinners > 0)
                        {
                            thislotterystone.WinAmount5 = tempwin5 / fivenumberwinners;
                        }
                        else
                        {
                            thislotterystone.WinAmount5 = tempwin5;
                        }

                        if (fournumberwinners > 0)
                        {
                            thislotterystone.WinAmount4 = tempwin4 / fournumberwinners;
                        }
                        else
                        {
                            thislotterystone.WinAmount4 = tempwin4;
                        }

                        if (threenumberwinners > 0)
                        {
                            thislotterystone.WinAmount3 = tempwin3 / threenumberwinners;
                        }
                        else
                        {
                            thislotterystone.WinAmount3 = tempwin3;
                        }

                        if (twonumberwinners > 0)
                        {
                            thislotterystone.WinAmount2 = tempwin2 / twonumberwinners;
                        }
                        else
                        {
                            thislotterystone.WinAmount2 = tempwin2;
                        }

                        if (onenumberwinners > 0)
                        {
                            thislotterystone.WinAmount1 = tempwin1 / onenumberwinners;
                        }
                        else
                        {
                            thislotterystone.WinAmount1 = tempwin1;
                        }

                        // *******************************************
                        // Transfer the winning amounts from the stone
                        // on to each winning ticket.
                        // *******************************************
                        foreach (Item tckts in World.Items.Values)
                        {
                            if (tckts is LotteryTicket)
                            {
                                LotteryTicket ticket = (LotteryTicket)tckts;
                                listtickets.Add(tckts);
                                if (ticket.TicketStatus == "1 Matched Number")
                                {
                                    ticket.LotteryTicketValue = thislotterystone.WinAmount1;
                                }
                                if (ticket.TicketStatus == "2 Matched Numbers")
                                {
                                    ticket.LotteryTicketValue = thislotterystone.WinAmount2;
                                }
                                if (ticket.TicketStatus == "3 Matched Numbers")
                                {
                                    ticket.LotteryTicketValue = thislotterystone.WinAmount3;
                                }
                                if (ticket.TicketStatus == "4 Matched Numbers")
                                {
                                    ticket.LotteryTicketValue = thislotterystone.WinAmount4;
                                }
                                if (ticket.TicketStatus == "5 Matched Numbers")
                                {
                                    ticket.LotteryTicketValue = thislotterystone.WinAmount5;
                                }
                            }
                        }

                        // Reset sales counters and gold taken for sales
                        thislotterystone.LotteryGoldTaken = 0; // Reset gold taken figure
                        thislotterystone.NextTicketNumber = 0; // Reset ticket counter for next sales

                        // reset
                        thislotterystone.WinAmount1 = 0;
                        thislotterystone.WinAmount2 = 0;
                        thislotterystone.WinAmount3 = 0;
                        thislotterystone.WinAmount4 = 0;
                        thislotterystone.WinAmount5 = 0;

                        // Store last lottery results
                        thislotterystone.LastWin1 = onenumberwinners;
                        thislotterystone.LastWin2 = twonumberwinners;
                        thislotterystone.LastWin3 = threenumberwinners;
                        thislotterystone.LastWin4 = fournumberwinners;
                        thislotterystone.LastWin5 = fivenumberwinners;

                        thislotterystone.LastTotalWins = totalwinningtickets;

                        // ***********************************************
                        // Finished Lottery, reset timers for next lottery
                        // Move last 3 draw results down a line.
                        // ***********************************************
                        thislotterystone.ThreeWinner1 = thislotterystone.TwoWinner1;
                        thislotterystone.ThreeWinner2 = thislotterystone.TwoWinner2;
                        thislotterystone.ThreeWinner3 = thislotterystone.TwoWinner3;
                        thislotterystone.ThreeWinner4 = thislotterystone.TwoWinner4;
                        thislotterystone.ThreeWinner5 = thislotterystone.TwoWinner5;

                        thislotterystone.TwoWinner1 = thislotterystone.OneWinner1;
                        thislotterystone.TwoWinner2 = thislotterystone.OneWinner2;
                        thislotterystone.TwoWinner3 = thislotterystone.OneWinner3;
                        thislotterystone.TwoWinner4 = thislotterystone.OneWinner4;
                        thislotterystone.TwoWinner5 = thislotterystone.OneWinner5;

                        thislotterystone.OneWinner1 = winnumber1;
                        thislotterystone.OneWinner2 = winnumber2;
                        thislotterystone.OneWinner3 = winnumber3;
                        thislotterystone.OneWinner4 = winnumber4;
                        thislotterystone.OneWinner5 = winnumber5;

                        // ***********************************************************************************
                        // ***********************************************************************************
                        // ***********************************************************************************
                        // You can alter the amount of time between lotterys here...
                        // This lottery is setup for one draw every 3 days... TimeSpan.FromDays (3);
                        // If you do change this figure, you will need to ALSO make a change in the ITEM code
                        // which is near the top of this file to match with the change you make here.

                        thislotterystone.DateOfNextDraw = DateTime.Now + TimeSpan.FromDays(3);

                        // ***********************************************************************************
                        // ***********************************************************************************
                        // ***********************************************************************************
                        World.Broadcast(0x35, true, "Lottery timer reset for next draw.");
                    }
                    else
                    {
                        World.Broadcast(0, true, "Lottery checked.");
                    }
                }
            }       
        }
    }

    // Extras

    public class PreviousLottery : Gump
    {
        public PreviousLottery(int visits, int highjp, int ldn1, int ldn2, int ldn3, int ldn4, int ldn5, int lwn5, int lwn4, int lwn3, int lwn2, int lwn1, int lwt) : base(80, 100)
        {
            Closable = false;
            Disposable = false;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(0, 0, 437, 406, 83);
            AddBackground(20, 126, 395, 230, 2620);
            AddLabel(86, 13, 1160, @"***** The Shard Lottery Game *****");
            AddLabel(123, 137, 1265, @"Previous Lottery Win Details");
            AddLabel(95, 70, 44, @"Highest Payout To Date : " + highjp + "gp");
            AddLabel(56, 166, 100, @"Last Lottery Draw Numbers : " + ldn1 + ", " + ldn2 + ", " + ldn3 + ", " + ldn4 + ", " + ldn5);
            AddLabel(95, 50, 45, @"Visits To This Stone So Far : " + visits);
            AddLabel(206, 366, 30, @"Cancel / Exit");
            AddButton(166, 365, 4017, 4019, 1, GumpButtonType.Reply, 0); // Exit
            AddLabel(56, 196, 100, @"Total Tickets With 5 Matching Numbers : " + lwn5);
            AddLabel(56, 216, 99, @"Total Tickets With 4 Matching Numbers : " + lwn4);
            AddLabel(56, 236, 99, @"Total Tickets With 3 Matching Numbers : " +lwn3);
            AddLabel(56, 256, 98, @"Total Tickets With 2 Matching Numbers : " + lwn2);
            AddLabel(56, 276, 98, @"Total Tickets With 1 Matching Number : " + lwn1);
            AddLabel(56, 306, 98, @"Total Amount of Winning Tickets : " + lwt);

        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            Container pack = from.Backpack;

            switch (info.ButtonID)
            {
                case 1: // Exit
                    {
                    }
                    break;
            }
        }
    }

    public class NextLottery : Gump
    {
        private readonly DateTime drawnon = DateTime.Now;
        //private int salesfigure = 0;
        private readonly int win5 = 0;
        private readonly int win4 = 0;
        private readonly int win3 = 0;
        private readonly int win2 = 0;
        private readonly int win1 = 0;

        public NextLottery(DateTime nextdrawdate, int sales, int jackpotamountsofar, int splits5, int splits4, int splits3, int splits2, int splits1, int stonevisits, int highpay, int rollo) : base(100, 80)
        {
            drawnon = nextdrawdate;

            win5 = (jackpotamountsofar / 100) * splits5;
            win4 = (jackpotamountsofar / 100) * splits4;
            win3 = (jackpotamountsofar / 100) * splits3;
            win2 = (jackpotamountsofar / 100) * splits2;
            win1 = (jackpotamountsofar / 100) * splits1;

            Closable = false;
            Disposable = false;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(0, 0, 437, 406, 83);
            AddBackground(20, 126, 395, 230, 2620);
            AddLabel(86, 13, 1160, @"***** The Shard Lottery Game *****");
            AddLabel(130, 137, 1166, @"Next Lottery Draw Details");
            AddLabel(95, 70, 44, @"Highest Payout To Date : " + highpay);
            AddLabel(95, 90, 43, @"Rollover Amount Currently At : " + rollo);
            AddLabel(56, 166, 170, @"Draw Date : " + drawnon);
            AddLabel(95, 50, 45, @"Visits To This Stone So Far : " + stonevisits);
            AddLabel(206, 366, 30, @"Cancel / Exit");
            AddButton(166, 365, 4017, 4019, 1, GumpButtonType.Reply, 0); // Exit
            AddLabel(56, 196, 169, @"Ticket Sales So Far : " + sales + ", Gold Taken : " + jackpotamountsofar + "gp");
            AddLabel(56, 216, 168, @"Winnings For 5 Draw Numbers : " + win5 + "gp @ " + splits5 + "%");
            AddLabel(56, 236, 167, @"Winnings For 4 Draw Numbers : " + win4 + "gp @ " + splits4 + "%");
            AddLabel(56, 256, 167, @"Winnings For 3 Draw Numbers : " + win3 + "gp @ " + splits3 + "%");
            AddLabel(56, 276, 166, @"Winnings For 2 Draw Numbers : " + win2 + "gp @ " + splits2 + "%");
            AddLabel(56, 296, 166, @"Winnings For 1 Draw Number : " + win1 + "gp @ " + splits1 + "%");
            AddLabel(56, 316, 190, @"Rollover Added If 5 Ticket Numbers Are Matched.");

        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            Container pack = from.Backpack;

            switch (info.ButtonID)
            {
                case 1: // Exit
                    {
                    }
                    break;
            }
        }
    }
}
