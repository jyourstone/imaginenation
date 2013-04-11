using System.Data;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Ladder
{
    public class Show1vs1LadderLadder : Gump
    {
        public enum Buttons
        {
            Show1vs1Ladder = 1,
            Show2vs2Ladder = 2,
            ShowTotalLadder = 3,
            GoToWeb = 4,
        }

        public static DataTable ladderTable;
        public Gump thisGump;

        private readonly Mobile m_From;

        public Show1vs1LadderLadder(Mobile from)
            : base(50, 50)
        {
            m_From = from;
            ladderTable = new DataTable();

            if (MySQLConnection.Get1vs1Ladder == null)
               ComfirmGump.UpdateIngameLadder();

            if (MySQLConnection.Get1vs1Ladder == null)
                from.SendAsciiMessage("Ladder is currently not available at this time.");

            ladderTable = MySQLConnection.Get1vs1Ladder;


            #region InitiateVarsAndText
            Closable = true;
            Disposable = true;
            Dragable = false;
            Resizable = false;
            AddPage(0);
            AddImageTiled(9, 10, 482, 411, 2170);
            AddButton(201, 46, 2445, 2445, (int)Buttons.GoToWeb, GumpButtonType.Reply, 0);
            AddLabel(217, 47, 0, @"Web Ladder");
            AddLabel(214, 101, 34, @"1vs1 Ladder");

            AddLabel(315, 127, 0, @"Points");
            AddLabel(117, 127, 0, @"Rank");
            AddLabel(156, 127, 0, @"Name");

            AddLabel(120, 164, 34, @"#1:");
            AddLabel(118, 181, 34, @"#2:");
            AddLabel(118, 198, 34, @"#3:");
            AddLabel(118, 215, 0, @"#4:");
            AddLabel(118, 233, 0, @"#5:");
            AddLabel(118, 250, 0, @"#6:");
            AddLabel(118, 267, 0, @"#7:");
            AddLabel(118, 285, 0, @"#8:");
            AddLabel(118, 301, 0, @"#9:");
            AddLabel(116, 317, 0, @"#10:");

            AddButton(112, 384, 30, 30, (int)Buttons.Show1vs1Ladder, GumpButtonType.Reply, 0);
            AddButton(222, 384, 30, 30, (int)Buttons.Show2vs2Ladder, GumpButtonType.Reply, 0);
            AddButton(332, 384, 30, 30, (int)Buttons.ShowTotalLadder, GumpButtonType.Reply, 0);

            AddLabel(135, 386, 34, @"1vs1");
            AddLabel(242, 386, 34, @"2vs2");
            AddLabel(351, 386, 34, @"Total");

            #endregion
            if (ladderTable == null)
            {
                m_From.SendAsciiMessage("Cannot display ladder at this point");
                return;
            }

            int hue = 0;
            for (int i = 0; i < ladderTable.Rows.Count && i < 10; i++)
            {
                Mobile mobileFound = World.FindMobile(int.Parse(ladderTable.Rows[i].ItemArray[0].ToString()));

                if (mobileFound == null)
                    hue = 0;
                else if (mobileFound.Kills >= 5)
                    hue = 33;
                else
                    hue = 99;

                AddLabel(156, 164 + (i * 17), hue, ladderTable.Rows[i].ItemArray[1].ToString());
                AddLabel(315, 164 + (i * 17), 0, ladderTable.Rows[i].ItemArray[2].ToString());
            }

        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {

            if (m_From is PlayerMobile)
            {
                switch (info.ButtonID)
                {
                    case 0: return;
                    case 1://1vs1
                        {
                            m_From.CloseGump(typeof(Show2vs2LadderLadder));
                            m_From.CloseGump(typeof(ShowTotalLadderLadder));
                            m_From.SendGump(new Show1vs1LadderLadder(m_From));
                            break;
                        }
                    case 2://2vs2
                        {
                            m_From.CloseGump(typeof(Show1vs1LadderLadder));
                            m_From.CloseGump(typeof(ShowTotalLadderLadder));
                            m_From.SendGump(new Show2vs2LadderLadder(m_From));
                            break;
                        }
                    case 3://total
                        {
                            m_From.CloseGump(typeof(Show1vs1LadderLadder));
                            m_From.CloseGump(typeof(Show2vs2LadderLadder));
                            m_From.SendGump(new ShowTotalLadderLadder(m_From));
                            break;
                        }
                    case 4://Web ladder
                        {
                            m_From.LaunchBrowser(MySQLConnection.ladderUrl);
                            break;
                        }
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    default: m_From.SendAsciiMessage("Error in gump"); break;

                }
            }
        }
    }

    public class Show2vs2LadderLadder : Gump
    {
        public enum Buttons
        {
            Show1vs1Ladder = 1,
            Show2vs2Ladder,
            ShowTotalLadder,
            GoToWeb,
        }

        public static DataTable ladderTable;
        public Gump thisGump;

        private readonly Mobile m_From;
        public Show2vs2LadderLadder(Mobile from)
            : base(50, 50)
        {
            m_From = from;
            ladderTable = new DataTable();
            ladderTable = MySQLConnection.Get2vs2Ladder;


            #region InitiateVarsAndText
            Closable = true;
            Disposable = true;
            Dragable = false;
            Resizable = false;
            AddPage(0);
            AddImageTiled(9, 10, 482, 411, 2170);
            AddButton(201, 46, 2445, 2445, (int)Buttons.GoToWeb, GumpButtonType.Reply, 0);
            AddLabel(217, 47, 0, @"Web Ladder");
            AddLabel(214, 101, 34, @"2vs2 Ladder");

            AddLabel(315, 127, 0, @"Points");
            AddLabel(117, 127, 0, @"Rank");
            AddLabel(156, 127, 0, @"Name");

            AddLabel(120, 164, 34, @"#1:");
            AddLabel(118, 181, 34, @"#2:");
            AddLabel(118, 198, 34, @"#3:");
            AddLabel(118, 215, 0, @"#4:");
            AddLabel(118, 233, 0, @"#5:");
            AddLabel(118, 250, 0, @"#6:");
            AddLabel(118, 267, 0, @"#7:");
            AddLabel(118, 285, 0, @"#8:");
            AddLabel(118, 301, 0, @"#9:");
            AddLabel(116, 317, 0, @"#10:");

            AddButton(112, 384, 30, 30, (int)Buttons.Show1vs1Ladder, GumpButtonType.Reply, 0);
            AddButton(222, 384, 30, 30, (int)Buttons.Show2vs2Ladder, GumpButtonType.Reply, 0);
            AddButton(332, 384, 30, 30, (int)Buttons.ShowTotalLadder, GumpButtonType.Reply, 0);

            AddLabel(135, 386, 34, @"1vs1");
            AddLabel(242, 386, 34, @"2vs2");
            AddLabel(351, 386, 34, @"Total");

            #endregion
            if (ladderTable == null)
            {
                m_From.SendAsciiMessage("Cannot display ladder at this point");
                return;
            }

            int hue = 0;
            for (int i = 0; i < ladderTable.Rows.Count && i < 10; i++)
            {
                Mobile mobileFound = World.FindMobile(int.Parse(ladderTable.Rows[i].ItemArray[0].ToString()));

                if (mobileFound == null)
                    hue = 0;
                else if (mobileFound.Kills >= 5)
                    hue = 33;
                else
                    hue = 99;

                AddLabel(156, 164 + (i * 17), hue, ladderTable.Rows[i].ItemArray[1].ToString());
                AddLabel(315, 164 + (i * 17), 0, ladderTable.Rows[i].ItemArray[2].ToString());
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_From is PlayerMobile)
            {
                switch (info.ButtonID)
                {
                    case 0: return;
                    case 1://1vs1
                        {
                            m_From.CloseGump(typeof(Show2vs2LadderLadder));
                            m_From.CloseGump(typeof(ShowTotalLadderLadder));
                            m_From.SendGump(new Show1vs1LadderLadder(m_From));
                            break;
                        }
                    case 2://2vs2
                        {
                            m_From.CloseGump(typeof(Show1vs1LadderLadder));
                            m_From.CloseGump(typeof(ShowTotalLadderLadder));
                            m_From.SendGump(new Show2vs2LadderLadder(m_From));
                            break;
                        }
                    case 3://total
                        {
                            m_From.CloseGump(typeof(Show1vs1LadderLadder));
                            m_From.CloseGump(typeof(Show2vs2LadderLadder));
                            m_From.SendGump(new ShowTotalLadderLadder(m_From));
                            break;
                        }
                    case 4://Web ladder
                        {
                            m_From.LaunchBrowser(MySQLConnection.ladderUrl);
                            break;
                        }
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    default: m_From.SendAsciiMessage("Error in gump"); break;

                }
            }
        }
    }


    public class ShowTotalLadderLadder : Gump
    {
        public enum Buttons
        {
            Show1vs1Ladder = 1,
            Show2vs2Ladder,
            ShowTotalLadder,
            GoToWeb,
        }

        public static DataTable ladderTable;
        public Gump thisGump;

        private readonly Mobile m_From;
        public ShowTotalLadderLadder(Mobile from)
            : base(50, 50)
        {
            m_From = from;
            ladderTable = new DataTable();
            ladderTable = MySQLConnection.GetTotalLadder;


            #region InitiateVarsAndText
            Closable = true;
            Disposable = true;
            Dragable = false;
            Resizable = false;
            AddPage(0);
            AddImageTiled(9, 10, 482, 411, 2170);
            AddButton(201, 46, 2445, 2445, (int)Buttons.GoToWeb, GumpButtonType.Reply, 0);
            AddLabel(217, 47, 0, @"Web Ladder");
            AddLabel(214, 101, 34, @"Total Ladder");

            AddLabel(315, 127, 0, @"Points");
            AddLabel(117, 127, 0, @"Rank");
            AddLabel(156, 127, 0, @"Name");

            AddLabel(120, 164, 34, @"#1:");
            AddLabel(118, 181, 34, @"#2:");
            AddLabel(118, 198, 34, @"#3:");
            AddLabel(118, 215, 0, @"#4:");
            AddLabel(118, 233, 0, @"#5:");
            AddLabel(118, 250, 0, @"#6:");
            AddLabel(118, 267, 0, @"#7:");
            AddLabel(118, 285, 0, @"#8:");
            AddLabel(118, 301, 0, @"#9:");
            AddLabel(116, 317, 0, @"#10:");

            AddButton(112, 384, 30, 30, (int)Buttons.Show1vs1Ladder, GumpButtonType.Reply, 0);
            AddButton(222, 384, 30, 30, (int)Buttons.Show2vs2Ladder, GumpButtonType.Reply, 0);
            AddButton(332, 384, 30, 30, (int)Buttons.ShowTotalLadder, GumpButtonType.Reply, 0);

            AddLabel(135, 386, 34, @"1vs1");
            AddLabel(242, 386, 34, @"2vs2");
            AddLabel(351, 386, 34, @"Total");

            #endregion

            int hue = 0;
            if (ladderTable == null)
            {
                m_From.SendAsciiMessage("Cannot display ladder at this point");
                return;
            }

            for (int i = 0; i < ladderTable.Rows.Count && i < 10; i++)
            {
                Mobile mobileFound = World.FindMobile(int.Parse(ladderTable.Rows[i].ItemArray[0].ToString()));

                if (mobileFound == null)
                    hue = 0;
                else if (mobileFound.Kills >= 5)
                    hue = 33;
                else
                    hue = 99;

                AddLabel(156, 164 + (i * 17), hue, ladderTable.Rows[i].ItemArray[1].ToString());
                AddLabel(315, 164 + (i * 17), 0, ladderTable.Rows[i].ItemArray[2].ToString());
            }

        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {

            if (m_From is PlayerMobile)
            {
                switch (info.ButtonID)
                {
                    case 0: return;
                    case 1://1vs1
                        {
                            m_From.CloseGump(typeof(Show2vs2LadderLadder));
                            m_From.CloseGump(typeof(ShowTotalLadderLadder));
                            m_From.SendGump(new Show1vs1LadderLadder(m_From));
                            break;
                        }
                    case 2://2vs2
                        {
                            m_From.CloseGump(typeof(Show1vs1LadderLadder));
                            m_From.CloseGump(typeof(ShowTotalLadderLadder));
                            m_From.SendGump(new Show2vs2LadderLadder(m_From));
                            break;
                        }
                    case 3://total
                        {
                            m_From.CloseGump(typeof(Show1vs1LadderLadder));
                            m_From.CloseGump(typeof(Show2vs2LadderLadder));
                            m_From.SendGump(new ShowTotalLadderLadder(m_From));
                            break;
                        }
                    case 4://Web ladder
                        {
                            m_From.LaunchBrowser(MySQLConnection.ladderUrl);
                            break;
                        }
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    default: m_From.SendAsciiMessage("Error in gump"); break;

                }
            }
        }
    }


}