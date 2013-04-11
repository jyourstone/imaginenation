using System;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Threading;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Ladder
{
    public class InitiateEventGump : Gump
    {

        private readonly Mobile m_From;
        public ArrayList mobilesJoined;
        public string eventType = "1vs1 No Loot";
        public InitiateEventGump current_IEG;
        public SelectWinnerGump current_SWG;

        #region GumpRegion
        public InitiateEventGump(Mobile from, ArrayList playersJoined, string EventType, SelectWinnerGump swg)
            : base(50, 50)
        {
            #region InitiateVars
            m_From = from;

            current_IEG = this;
            if (from is PlayerMobile)
                (from as PlayerMobile).LadderGump = current_IEG;

            if (swg != null)
                current_SWG = swg;

            if (playersJoined != null)
                mobilesJoined = playersJoined;
            else
                mobilesJoined = new ArrayList();

            if (EventType != null)
                eventType = EventType;

            AddBackground(50, 50, 400, 250, 9200);

            #endregion

            //Event Type
            AddLabel(160, 100, 0x22, "Event Type:");
            AddButton(330, 90, 250, 250, 1, GumpButtonType.Reply, 0);//Up
            AddButton(330, 110, 253, 253, 2, GumpButtonType.Reply, 0);//Down
            AddLabel(235, 100, 0, eventType);
            //Event Type end

            //Participents
            AddLabel(170, 170, 0x22, "Participants:");
            AddLabel(250, 170, 0, mobilesJoined.Count.ToString());
            AddButton(270, 160, 4016, 4016, 3, GumpButtonType.Reply, 0);//Set
            AddLabel(300, 160, 0x22, "Set");
            AddButton(270, 180, 4016, 4016, 4, GumpButtonType.Reply, 0);//Reset
            AddLabel(300, 180, 0x22, "Reset");
            //Participants end


            AddButton(370, 260, 2471, 2471, 10, GumpButtonType.Reply, 0);//NextPage

        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {

            if (m_From is PlayerMobile)
            {
                switch (info.ButtonID)
                {
                    case 0: return;
                    case 1://Up
                        {
                            switch (eventType)
                            {
                                case "1vs1 No Loot": eventType = "FFA Loot"; break;
                                case "1vs1 Loot": eventType = "1vs1 No Loot"; break;
                                case "2vs2 No Loot": eventType = "1vs1 Loot"; break;
                                case "2vs2 Loot": eventType = "2vs2 No Loot"; break;
                                case "FFA No Loot": eventType = "2vs2 Loot"; break;
                                case "FFA Loot": eventType = "FFA No Loot"; break;
                                default: eventType = "Error In Event Type."; break;
                            }
                            m_From.CloseGump(typeof(InitiateEventGump));
                            m_From.SendGump(new InitiateEventGump(m_From, mobilesJoined, eventType, current_SWG));
                            break;
                        }
                    case 2://Down
                        {
                            switch (eventType)
                            {
                                case "1vs1 No Loot": eventType = "1vs1 Loot"; break;
                                case "1vs1 Loot": eventType = "2vs2 No Loot"; break;
                                case "2vs2 No Loot": eventType = "2vs2 Loot"; break;
                                case "2vs2 Loot": eventType = "FFA No Loot"; break;
                                case "FFA No Loot": eventType = "FFA Loot"; break;
                                case "FFA Loot": eventType = "1vs1 No Loot"; break;
                                default: eventType = "Error In Event Type."; break;
                            }
                            m_From.CloseGump(typeof(InitiateEventGump));
                            m_From.SendGump(new InitiateEventGump(m_From, mobilesJoined, eventType, current_SWG));
                            break;
                        }
                    case 3://Set
                        {
                            SetMobileCount(m_From);
                            break;
                        }
                    case 4://Reset
                        {
                            ResetMobileCount(m_From);
                            break;
                        }
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                        {
                            if (mobilesJoined.Count >= 0)
                            {
                                m_From.SendGump(new SelectWinnerGump(m_From, current_IEG, null));
                                m_From.CloseGump(typeof(InitiateEventGump));
                            }
                            else
                            {
                                m_From.SendAsciiMessage("Not enough players joined!");
                                m_From.SendGump(current_IEG);
                            }
                            break;
                        }
                    default: m_From.SendAsciiMessage("Error in gump"); break;

                }
            }
        }
        #endregion

        public void SetMobileCount(Mobile from)
        {
            if( mobilesJoined == null)
                mobilesJoined = new ArrayList();

            foreach (Mobile m in from.Region.GetMobiles())
            {
                if (m is PlayerMobile && m.AccessLevel == AccessLevel.Player && !(mobilesJoined.Contains(m)))
                    mobilesJoined.Add(m);
            }

            from.SendAsciiMessage("Mobiles have updated.");
            from.CloseGump(typeof(InitiateEventGump));
            from.SendGump(new InitiateEventGump(from, mobilesJoined, eventType, current_SWG));
        }

        public void ResetMobileCount(Mobile from)
        {
            mobilesJoined = new ArrayList();

            foreach (Mobile m in from.Region.GetMobiles())
            {
                if (m is PlayerMobile && m.AccessLevel == AccessLevel.Player)
                    mobilesJoined.Add(m);
            }

            from.SendAsciiMessage("Mobiles have been reset and updated!");
            from.CloseGump(typeof(InitiateEventGump));
            from.SendGump(new InitiateEventGump(from, mobilesJoined, eventType, current_SWG));
        }
    }

    public class SelectWinnerGump : Gump
    {

        public Mobile[] m_First = new Mobile[2], m_Second = new Mobile[2], m_Third = new Mobile[2];
        public Mobile m_From;

        public ArrayList mobilesJoined;
        public SelectWinnerGump current_SWG;
        public InitiateEventGump IEG;
        public string eventType = "1vs1 No Loot";

        #region GumpRegion
        public SelectWinnerGump(Mobile from, InitiateEventGump ieg, SelectWinnerGump swg)
            : base(50, 50)
        {
            #region InitiateVars
            if (swg == null)
            {
                current_SWG = this;
                IEG = ieg;
            }
            else
            {
                current_SWG = swg;
                IEG = current_SWG.IEG;
            }

            m_From = from;

            AddBackground(50, 50, 400, 250, 9200);

            if (IEG.mobilesJoined != null)
                mobilesJoined = IEG.mobilesJoined;
            else
                mobilesJoined = new ArrayList();

            if (IEG.eventType != null)
                eventType = IEG.eventType;

            if (from is PlayerMobile)
                (from as PlayerMobile).LadderGump = current_SWG;

            #endregion


            #region InitiateTextAndButtons
            //Event Type
            AddLabel(105, 80, 0x22, "Event Type:");
            AddLabel(180, 80, 0, eventType);
            //Event Type end

            //Participents
            AddLabel(290, 80, 0x22, "Participants:");
            AddLabel(375, 80, 0, mobilesJoined.Count.ToString());
            //Participants end

            AddButton(370, 260, 2471, 2471, 9, GumpButtonType.Reply, 0);//NextPage
            AddButton(80, 260, 2468, 2468, 10, GumpButtonType.Reply, 0);//PreviousPage
            #endregion


            #region AddWinners
            AddLabel(120, 140, 0, "First:");
            AddButton(90, 140, 4005, 4005, 1, GumpButtonType.Reply, 0);
            if (current_SWG != null && current_SWG.m_First[0] != null)
                AddLabel(170, 140, 0x22, current_SWG.m_First[0].Name);
            else
                AddLabel(170, 140, 0x22, "Select Player!");


            AddLabel(120, 170, 0, "Second:");
            AddButton(90, 170, 4005, 4005, 2, GumpButtonType.Reply, 0);
            if (current_SWG != null && current_SWG.m_Second[0] != null)
                AddLabel(170, 170, 0x22, current_SWG.m_Second[0].Name);
            else
                AddLabel(170, 170, 0x22, "Select Player!");



            if (current_SWG != null && current_SWG.mobilesJoined != null && current_SWG.mobilesJoined.Count >= 34)
            {
                AddButton(90, 190, 4005, 4005, 3, GumpButtonType.Reply, 0);
                AddLabel(120, 190, 0, "Third:");
                if (current_SWG != null && current_SWG.m_Third[0] != null)
                    AddLabel(170, 190, 0x22, current_SWG.m_Third[0].Name);
                else
                    AddLabel(170, 190, 0x22, "Select Player!");
            }

            //Add 2v2 partner
            if (current_SWG.eventType.Contains("2vs2"))
            {
                AddLabel(270, 140, 0, "&");
                if (current_SWG != null && current_SWG.m_First[1] != null)
                    AddLabel(290, 140, 0x22, current_SWG.m_First[1].Name);
                else
                    AddLabel(290, 140, 0x22, "Select Player!");

                AddLabel(270, 170, 0, "&");
                if (current_SWG != null && current_SWG.m_Second[1] != null)
                    AddLabel(290, 170, 0x22, current_SWG.m_Second[1].Name);
                else
                    AddLabel(290, 170, 0x22, "Select Player!");

                if (current_SWG != null && current_SWG.mobilesJoined != null && current_SWG.mobilesJoined.Count >= 34)
                {
                    AddLabel(270, 190, 0, "&");
                    if (current_SWG != null && current_SWG.m_Third[1] != null)
                        AddLabel(290, 190, 0x22, current_SWG.m_Third[1].Name);
                    else
                        AddLabel(290, 190, 0x22, "Select Player!");
                }
            }

            #endregion

        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {

            if (m_From is PlayerMobile)
            {
                switch (info.ButtonID)
                {
                    case 0: return;
                    case 1://Add First
                        {
                            if (current_SWG.eventType.Contains("2vs2") && (current_SWG.m_First[0] != null && current_SWG.m_First[1] != null))
                            {
                                current_SWG.m_First[0] = null;
                                current_SWG.m_First[1] = null;
                                m_From.SendGump(new SelectWinnerGump(m_From, current_SWG.IEG, current_SWG));
                            }
                            else
                                m_From.Target = new SelectMobile(current_SWG, info.ButtonID);
                            break;
                        }
                    case 2://Add Second
                        {
                            if (current_SWG.eventType.Contains("2vs2") && (current_SWG.m_Second[0] != null && current_SWG.m_Second[1] != null))
                            {
                                current_SWG.m_Second[0] = null;
                                current_SWG.m_Second[1] = null;
                                m_From.SendGump(new SelectWinnerGump(m_From, current_SWG.IEG, current_SWG));
                            }
                            else
                                m_From.Target = new SelectMobile(current_SWG, info.ButtonID);

                            break;
                        }
                    case 3://Add Third
                        {
                            if (current_SWG.eventType.Contains("2vs2") && (current_SWG.m_Third[0] != null && current_SWG.m_Third[1] != null))
                            {
                                current_SWG.m_Third[0] = null;
                                current_SWG.m_Third[1] = null;
                                m_From.SendGump(new SelectWinnerGump(m_From, current_SWG.IEG, current_SWG));
                            }
                            else
                                m_From.Target = new SelectMobile(current_SWG, info.ButtonID);
                            break;
                        }
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        {
                            if (current_SWG.m_First == current_SWG.m_Second || (current_SWG.m_Third != null && current_SWG.m_Second == current_SWG.m_Third) || (current_SWG.m_Third != null && current_SWG.m_First == current_SWG.m_Third))
                            {
                                m_From.SendGump(new SelectWinnerGump(m_From, current_SWG.IEG, current_SWG));
                                m_From.SendAsciiMessage("You have the same player in more than one position");
                            }
                            else
                                if (current_SWG.m_First == null || current_SWG.m_Second == null || (current_SWG.mobilesJoined.Count >= 34 && current_SWG.m_Third == null))
                                {
                                    m_From.SendGump(new SelectWinnerGump(m_From, current_SWG.IEG, current_SWG));
                                    m_From.SendAsciiMessage("Not all winners have been set, please reset them.");
                                }
                                else
                                {
                                    m_From.SendGump(new ComfirmGump(m_From, current_SWG));
                                    m_From.SendAsciiMessage("Re read everything befor you update the ladder!");
                                }

                            break;
                        }
                    case 10:
                        {
                            m_From.SendGump(IEG);
                            m_From.CloseGump(typeof(SelectWinnerGump));
                            break;
                        }
                    default: m_From.SendAsciiMessage("Error in gump"); break;

                }
            }
        }
        #endregion

        public class SelectMobile : Target
        {
            //Mobile m_Player;
            readonly SelectWinnerGump current_SWG;
            readonly int m_Place;
            public SelectMobile(SelectWinnerGump SWG, int place)
                : base(12, false, TargetFlags.None)
            {
                current_SWG = SWG;
                m_Place = place;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is PlayerMobile && (o as PlayerMobile).AccessLevel == AccessLevel.Player)
                {
                    if (current_SWG.eventType.Contains("2vs2"))
                    {
                        if (m_Place == 1)
                        {
                            if (current_SWG.m_First[0] == null)
                                current_SWG.m_First[0] = (o as Mobile);
                            else if (current_SWG.m_First[1] == null)
                                current_SWG.m_First[1] = (o as Mobile);
                        }
                        else if (m_Place == 2)
                        {
                            if (current_SWG.m_Second[0] == null)
                                current_SWG.m_Second[0] = (o as Mobile);
                            else if (current_SWG.m_Second[1] == null)
                                current_SWG.m_Second[1] = (o as Mobile);
                        }
                        else if (m_Place == 3)
                        {
                            if (current_SWG.m_Third[0] == null)
                                current_SWG.m_Third[0] = (o as Mobile);
                            else if (current_SWG.m_Third[1] == null)
                                current_SWG.m_Third[1] = (o as Mobile);
                        }
                        else
                            from.SendAsciiMessage("Error in selecting target!");
                    }
                    else
                    {
                        if (m_Place == 1)
                            current_SWG.m_First[0] = (o as Mobile);
                        else if (m_Place == 2)
                            current_SWG.m_Second[0] = (o as Mobile);
                        else if (m_Place == 3)
                            current_SWG.m_Third[0] = (o as Mobile);
                        else
                            from.SendAsciiMessage("Error in selecting target!");

                        from.SendAsciiMessage((o as Mobile).Name + " Targeted!");
                    }
                }
                else
                    from.SendAsciiMessage("You can only target players!");
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (from is PlayerMobile)
                {
                    from.SendGump(new SelectWinnerGump(from, current_SWG.IEG, current_SWG));
                }
            }
        }
    }

    public class ComfirmGump : Gump
    {

        private readonly Mobile[] m_First = new Mobile[2];
        private readonly Mobile[] m_Second = new Mobile[2];
        private readonly Mobile[] m_Third = new Mobile[2];
        private readonly Mobile m_From;

        public ArrayList mobilesJoined;
        public SelectWinnerGump current_SWG;
        public InitiateEventGump IEG;
        public string eventType = "1vs1 No Loot";

        #region GumpRegion

        public ComfirmGump(Mobile from, SelectWinnerGump swg)
            : base(50, 50)
        {
            if (swg != null)
                current_SWG = swg;

            m_First[0] = current_SWG.m_First[0];
            m_First[1] = current_SWG.m_First[1];

            m_Second[0] = current_SWG.m_Second[0];
            m_Second[1] = current_SWG.m_Second[1];

            m_Third[0] = current_SWG.m_Third[0];
            m_Third[1] = current_SWG.m_Third[1];

            AddBackground(50, 50, 400, 250, 9200);

            mobilesJoined = current_SWG.mobilesJoined;

            m_From = from;

            #region InitiateTextAndButtons
            //Event Type
            AddLabel(105, 80, 0x22, "Event Type:");
            AddLabel(180, 80, 0, current_SWG.eventType);
            //Event Type end

            //Participents
            AddLabel(290, 80, 0x22, "Participants:");
            AddLabel(375, 80, 0, current_SWG.mobilesJoined.Count.ToString());
            //Participants end

            AddButton(370, 260, 2462, 2462, 1, GumpButtonType.Reply, 0);//NextPage
            AddButton(80, 260, 2468, 2468, 2, GumpButtonType.Reply, 0);//PreviousPage
            #endregion

            #region AddWinners
            AddLabel(120, 140, 0, "First:");
            if (current_SWG != null && current_SWG.m_First[0] != null)
                AddLabel(170, 140, 0x22, current_SWG.m_First[0].Name);
            else
                AddLabel(170, 140, 0x22, "Select Player!");

            if (current_SWG.eventType.Contains("2vs2"))
            {
                AddLabel(250, 140, 0, "&");
                if (current_SWG != null && current_SWG.m_First[1] != null)
                    AddLabel(170, 140, 0x22, current_SWG.m_First[1].Name);
                else
                    AddLabel(170, 140, 0x22, "Select Player!");
            }


            AddLabel(120, 170, 0, "Second:");
            if (current_SWG != null && current_SWG.m_Second[0] != null)
                AddLabel(170, 170, 0x22, current_SWG.m_Second[0].Name);
            else
                AddLabel(170, 170, 0x22, "Select Player!");



            if (current_SWG != null && current_SWG.mobilesJoined != null && current_SWG.mobilesJoined.Count >= 34)
            {
                AddLabel(120, 190, 0, "Third:");
                if (current_SWG != null && current_SWG.m_Third[0] != null)
                    AddLabel(170, 190, 0x22, current_SWG.m_Third[0].Name);
                else
                    AddLabel(170, 190, 0x22, "Select Player!");
            }

            #endregion


        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {

            if (m_From is PlayerMobile)
            {
                switch (info.ButtonID)
                {
                    case 0: return;
                    case 1://Update
                        {
                            Thread updateLadderThread = new Thread(ConnectToDb);
                            updateLadderThread.Start();
                            break;
                        }
                    case 2://Back
                        {
                            m_From.SendGump(new SelectWinnerGump(m_From, current_SWG.IEG, current_SWG));
                            m_From.CloseGump(typeof(ComfirmGump));
                            break;
                        }
                    default: m_From.SendAsciiMessage("Error in gump"); break;

                }
            }
        }
        #endregion

        public DataTable participantTable, eventTable;
        public ArrayList mobilesAdded;

        public void ConnectToDb()
        {
            participantTable = new DataTable();
            eventTable = new DataTable();

            if (MySQLConnection.OpenConnection() == false)
            {
                m_From.SendAsciiMessage("Cannot establish connection, try again later");
                return;
            }

            OdbcConnection dbConnection = MySQLConnection.GetmySQLConnection;

            //Initiate select strings
            string getParticipants = "SELECT * FROM participant";
            string getEvents = "SELECT * FROM event";

            //Set participant adapter
            OdbcDataAdapter participants = new OdbcDataAdapter();
            OdbcCommand selectP = dbConnection.CreateCommand();
            selectP.CommandText = getParticipants;
            participants.SelectCommand = selectP;

            //Set participant adapter
            OdbcDataAdapter events = new OdbcDataAdapter();
            OdbcCommand selectE = dbConnection.CreateCommand();
            selectE.CommandText = getEvents;
            events.SelectCommand = selectE;

            //Set players joined
            participants.Fill(participantTable);
            SetPlayersJoined(participantTable, dbConnection, current_SWG);

            //Set winners
            participants.Fill(participantTable);
            SetWinners(participantTable, dbConnection, current_SWG);

            //Add event
            events.Fill(eventTable);
            AddEvent(eventTable, dbConnection);

            //Update ingame ladder and closes connections.
            UpdateIngameLadder();

            if (dbConnection != null && dbConnection.State == ConnectionState.Open)
                dbConnection.Close();
        
        }

        public void AddEvent(DataTable eventTable, OdbcConnection connection)
        {
            //Event 	Type 	Winner 	Second 	Third 	Hoster 	Participants 	Date
            int first = 0, second = 0, third = 0;
            int first2 = 0, second2 = 0, third2 = 0;
            string time = "'" + DateTime.Now.Year + "-" + DateTime.Now.Month.ToString("00") + "-" + DateTime.Now.Day.ToString("00") + " " + DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + "'";

            if (current_SWG.m_First[0] != null)
                first = m_First[0].Serial;
            if (current_SWG.m_Second[0] != null)
                second = m_Second[0].Serial;
            if (current_SWG.m_Third[0] != null)
                third = m_Third[0].Serial;

            if (current_SWG.eventType.Contains("2vs2"))
            {
                if (current_SWG.m_First[1] != null)
                    first2 = m_First[1].Serial;
                if (current_SWG.m_Second[1] != null)
                    second2 = m_Second[1].Serial;
                if (current_SWG.m_Third[1] != null)
                    third2 = m_Third[1].Serial;
            }

            string addEventString = "INSERT INTO event ("
            + " Type, Winner, Winner2, Second, Second2, Third, Third2, Hoster, Participants, DateHosted)"
            + " VALUES ( '" + current_SWG.eventType + "', '" + first + "', '" + first2 + "', '" + second + "', '" + second2 + "', '" + third + "', '" + third2 + "', '" + m_From.Name + "', " + current_SWG.mobilesJoined.Count + ", " + time + ")";

            OdbcCommand addEventCommand = connection.CreateCommand();
            addEventCommand.CommandText = addEventString;

            addEventCommand.ExecuteNonQuery();
        }


        public void SetPlayersJoined(DataTable ladder, OdbcConnection dbConnection, SelectWinnerGump swg)
        {
            mobilesAdded = new ArrayList();
            DataTable tempTable = new DataTable();
            tempTable = ladder.Copy();

            //Set Primary key
            DataColumn[] primaryKey = new DataColumn[1];
            primaryKey[0] = ladder.Columns[0];
            ladder.PrimaryKey = primaryKey;

            foreach (DataRow dr in tempTable.Rows)
            {
                Mobile player = null;
                DataRow existingRow = null;

                foreach (PlayerMobile pm in swg.mobilesJoined)
                {
                    player = pm;
                    if (ladder.Rows.Count < 0)
                        AddPlayerToLadder(dbConnection, player);
                    else
                    {
                        if (ladder.Rows.Contains(((int)player.Serial).ToString()))
                        {
                            existingRow = ladder.Rows.Find((int)pm.Serial);
                            UpdatePlayerStats(dbConnection, existingRow, player);
                        }
                        else if (mobilesAdded.Contains(pm) == false)
                        {
                            AddPlayerToLadder(dbConnection, player);
                        }
                    }
                }
            }
        }

        public void UpdatePlayerStats(OdbcConnection connection, DataRow oldRow, Mobile player)
        {
            string addString = "UPDATE participant SET " +
            "Name = '" + player.Name + "', " +
            "Joined = " + (int.Parse(oldRow.ItemArray[5].ToString()) + 1) + " " +
            "WHERE Mobile = " + ((int)player.Serial);

            OdbcCommand updateCommand = connection.CreateCommand();
            updateCommand.CommandText = addString;

            updateCommand.ExecuteNonQuery();
        }

        public void AddPlayerToLadder(OdbcConnection connection, Mobile player)
        {
            mobilesAdded.Add((player as PlayerMobile));

            string addString = "INSERT INTO participant ("
            + " Mobile,  Name, Won, Second, Third, Joined, TotalPoints, 1v1Points, 2v2Points, OtherPoints)"
            + " VALUES ( " + ((int)player.Serial) + ", '" + player.Name + "', 0 , 0, 0, 1, 0, 0, 0, 0)";

            OdbcCommand addCommand = connection.CreateCommand();
            addCommand.CommandText = addString;

            addCommand.ExecuteNonQuery();
        }


        public void SetWinners(DataTable ladder, OdbcConnection dbConnection, SelectWinnerGump swg)
        {
            if (m_First[0] != null)
                AddWinnerToLadder(ladder, dbConnection, swg, m_First[0]);
            if (m_Second[0] != null)
                AddWinnerToLadder(ladder, dbConnection, swg, m_Second[0]);
            if (m_Third[0] != null)
                AddWinnerToLadder(ladder, dbConnection, swg, m_Third[0]);

            if (current_SWG.eventType.Contains("2vs2"))
            {
                if (m_First[1] != null)
                    AddWinnerToLadder(ladder, dbConnection, swg, m_First[1]);
                if (m_Second[1] != null)
                    AddWinnerToLadder(ladder, dbConnection, swg, m_Second[1]);
                if (m_Third[1] != null)
                    AddWinnerToLadder(ladder, dbConnection, swg, m_Third[1]);
            }
        }

        public void AddWinnerToLadder(DataTable ladder, OdbcConnection dbConnection, SelectWinnerGump swg, Mobile winnerToAdd)
        {
            if (ladder.Rows.Contains(((int)winnerToAdd.Serial).ToString()) == false && mobilesAdded.Contains((winnerToAdd as PlayerMobile)) == false)
                AddPlayerToLadder(dbConnection, winnerToAdd);

            DataRow existingRow = ladder.Rows.Find((int)winnerToAdd.Serial);

            if (existingRow != null && winnerToAdd != null)
                UpdateWinnerStats(dbConnection, existingRow, winnerToAdd, swg);
            else
                AddWinnerToLadder(ladder, dbConnection, swg, winnerToAdd);
        }

        public void UpdateWinnerStats(OdbcConnection connection, DataRow oldRow, Mobile player, SelectWinnerGump swg)
        {
            string addWinnerString = string.Empty;
            int fpoints = 0, spoints = 0, tpoints = 0;

            if (swg.eventType == "1vs1 No Loot" || swg.eventType == "1vs1 Loot")
            {
                if (swg.mobilesJoined.Count >= 45)
                {
                    fpoints = 5;
                    spoints = 3;
                    tpoints = 2;
                }
                else if (swg.mobilesJoined.Count >= 34)
                {
                    fpoints = 4;
                    spoints = 2;
                    tpoints = 1;
                }
                else if (swg.mobilesJoined.Count >= 20)
                {
                    fpoints = 3;
                    spoints = 1;
                    tpoints = 0;
                }
                else if (swg.mobilesJoined.Count >= 14)
                {
                    fpoints = 2;
                    spoints = 1;
                    tpoints = 0;
                }
                else
                {
                    fpoints = 1;
                    spoints = 0;
                    tpoints = 0;
                }
            }
            else if (swg.eventType == "2vs2 No Loot" || swg.eventType == "2vs2 Loot")
            {
                if (swg.mobilesJoined.Count >= 34)
                {
                    fpoints = 3;
                    spoints = 2;
                    tpoints = 1;
                }
                else if (swg.mobilesJoined.Count >= 24)
                {
                    fpoints = 2;
                    spoints = 1;
                    tpoints = 0;
                }
                else
                {
                    fpoints = 1;
                    spoints = 0;
                    tpoints = 0;
                }
            }
            else
            {
                if (swg.mobilesJoined.Count >= 30)
                {
                    fpoints = 2;
                    spoints = 0;
                    tpoints = 0;
                }
                else
                {
                    fpoints = 1;
                    spoints = 0;
                    tpoints = 0;
                }
            }

            if (swg.eventType == "1vs1 No Loot" || swg.eventType == "1vs1 Loot")
            {
                if (player == m_First[0])
                {
                    addWinnerString = "UPDATE participant SET " +
                    "Name = '" + player.Name + "', " +
                    "Won = " + (int.Parse(oldRow.ItemArray[2].ToString()) + 1) + ", " +
                    "TotalPoints = " + (int.Parse(oldRow.ItemArray[6].ToString()) + fpoints) + ", " +
                    "1v1Points = " + (int.Parse(oldRow.ItemArray[7].ToString()) + fpoints) +
                    " WHERE Mobile = " + ((int)player.Serial);
                    //Misc.Guilds.GuildLadderUpdate.UpdateTourPoints(player, swg.mobilesJoined.Count);
                }
                else if (player == m_Second[0])
                {
                    addWinnerString = "UPDATE participant SET " +
                    "Name = '" + player.Name + "', " +
                    "Second = " + (int.Parse(oldRow.ItemArray[3].ToString()) + 1) + ", " +
                    "TotalPoints = " + (int.Parse(oldRow.ItemArray[6].ToString()) + spoints) + ", " +
                    "1v1Points = " + (int.Parse(oldRow.ItemArray[7].ToString()) + spoints) +
                    " WHERE Mobile = " + ((int)player.Serial);

                    //Misc.Guilds.GuildLadderUpdate.UpdateTourPoints(player, (int)(swg.mobilesJoined.Count/2));
                }
                else if (player == m_Third[0])
                {
                    addWinnerString = "UPDATE participant SET " +
                    "Name = '" + player.Name + "', " +
                    "Third = " + (int.Parse(oldRow.ItemArray[4].ToString()) + 1) + ", " +
                    "TotalPoints = " + (int.Parse(oldRow.ItemArray[6].ToString()) + tpoints) + ", " +
                    "1v1Points = " + (int.Parse(oldRow.ItemArray[7].ToString()) + tpoints) +
                    " WHERE Mobile = " + ((int)player.Serial);

                    //Misc.Guilds.GuildLadderUpdate.UpdateTourPoints(player, (int)(swg.mobilesJoined.Count / 3));

                }
            }
            else if (swg.eventType == "2vs2 No Loot" || swg.eventType == "2vs2 Loot")
            {
                if (player == m_First[0] || player == m_First[1])
                {
                    addWinnerString = "UPDATE participant SET " +
                    "Name = '" + player.Name + "', " +
                    "Won = " + (int.Parse(oldRow.ItemArray[2].ToString()) + 1) + ", " +
                    "TotalPoints = " + (int.Parse(oldRow.ItemArray[6].ToString()) + fpoints) + ", " +
                    "2v2Points = " + (int.Parse(oldRow.ItemArray[8].ToString()) + fpoints) +
                    " WHERE Mobile = " + ((int)player.Serial);

                    //Misc.Guilds.GuildLadderUpdate.UpdateTourPoints(player, (int)(swg.mobilesJoined.Count/2));

                }
                else if (player == m_Second[0] || player == m_Second[1])
                {
                    addWinnerString = "UPDATE participant SET " +
                    "Name = '" + player.Name + "', " +
                    "Second = " + (int.Parse(oldRow.ItemArray[3].ToString()) + 1) + ", " +
                    "TotalPoints = " + (int.Parse(oldRow.ItemArray[6].ToString()) + spoints) + ", " +
                    "2v2Points = " + (int.Parse(oldRow.ItemArray[8].ToString()) + spoints) +
                    " WHERE Mobile = " + ((int)player.Serial);
                    //Misc.Guilds.GuildLadderUpdate.UpdateTourPoints(player, (int)(swg.mobilesJoined.Count / 4));

                }
                else if (player == m_Third[0] || player == m_Third[1])
                {
                    addWinnerString = "UPDATE participant SET " +
                    "Name = '" + player.Name + "', " +
                    "Third = " + (int.Parse(oldRow.ItemArray[4].ToString()) + 1) + ", " +
                    "TotalPoints = " + (int.Parse(oldRow.ItemArray[6].ToString()) + tpoints) + ", " +
                    "2v2Points = " + (int.Parse(oldRow.ItemArray[9].ToString()) + tpoints) +
                    " WHERE Mobile = " + ((int)player.Serial);

                    //Misc.Guilds.GuildLadderUpdate.UpdateTourPoints(player, (int)(swg.mobilesJoined.Count / 6));

                }
            }
            else
            {
                if (player == m_First[0])
                {
                    addWinnerString = "UPDATE participant SET " +
                    "Name = '" + player.Name + "', " +
                    "Won = " + (int.Parse(oldRow.ItemArray[2].ToString()) + 1) + ", " +
                    "TotalPoints = " + (int.Parse(oldRow.ItemArray[6].ToString()) + fpoints) + ", " +
                    "OtherPoints = " + (int.Parse(oldRow.ItemArray[9].ToString()) + fpoints) +
                    " WHERE Mobile = " + ((int)player.Serial);
                }
                else if (player == m_Second[0])
                {
                    addWinnerString = "UPDATE participant SET " +
                    "Name = '" + player.Name + "', " +
                    "Second = " + (int.Parse(oldRow.ItemArray[3].ToString()) + 1) + ", " +
                    "TotalPoints = " + (int.Parse(oldRow.ItemArray[6].ToString()) + spoints) + ", " +
                    "OtherPoints = " + (int.Parse(oldRow.ItemArray[9].ToString()) + spoints) +
                    " WHERE Mobile = " + ((int)player.Serial);
                }
                else if (player == m_Third[0])
                {
                    addWinnerString = "UPDATE participant SET " +
                    "Name = '" + player.Name + "', " +
                    "Third = " + (int.Parse(oldRow.ItemArray[4].ToString()) + 1) + ", " +
                    "TotalPoints = " + (int.Parse(oldRow.ItemArray[6].ToString()) + tpoints) + ", " +
                    "OtherPoints = " + (int.Parse(oldRow.ItemArray[9].ToString()) + tpoints) +
                    " WHERE Mobile = " + ((int)player.Serial);
                }
            }

            OdbcCommand addWinnerCommand = connection.CreateCommand();
            addWinnerCommand.CommandText = addWinnerString;

            addWinnerCommand.ExecuteNonQuery();
        }


        public static void UpdateIngameLadder()
        {
            if (MySQLConnection.OpenConnection() == false)
                return;

            Fill1vs1Ladder();
            Fill2vs2Ladder();
            FillTotalLadder();

            MySQLConnection.GetmySQLConnection.Close();
            MySQLConnection.GetmySQLConnection = null;
        }


        public static void Fill1vs1Ladder()
        {
            MySQLConnection.Get1vs1Ladder = new DataTable();

            string getParticipants = "SELECT Mobile, Name, 1v1Points FROM Participant ORDER BY 1v1Points DESC, Won DESC, Second DESC, Third DESC, Joined LIMIT 0 , 10";
            OdbcDataAdapter participants = new OdbcDataAdapter();
            OdbcCommand selectP = MySQLConnection.GetmySQLConnection.CreateCommand();
            selectP.CommandText = getParticipants;
            participants.SelectCommand = selectP;

            participants.Fill(MySQLConnection.Get1vs1Ladder);
        }

        public static void Fill2vs2Ladder()
        {
            MySQLConnection.Get2vs2Ladder = new DataTable();

            string getParticipants = "SELECT Mobile, Name, 2v2Points FROM Participant ORDER BY 2v2Points DESC, Won DESC, Second DESC, Third DESC, Joined LIMIT 0 , 10";
            OdbcDataAdapter participants = new OdbcDataAdapter();
            OdbcCommand selectP = MySQLConnection.GetmySQLConnection.CreateCommand();
            selectP.CommandText = getParticipants;
            participants.SelectCommand = selectP;

            participants.Fill(MySQLConnection.Get2vs2Ladder);
        }

        public static void FillTotalLadder()
        {
            MySQLConnection.GetTotalLadder = new DataTable();

            string getParticipants = "SELECT Mobile, Name, TotalPoints FROM Participant ORDER BY TotalPoints DESC, Won DESC, Second DESC, Third DESC, Joined LIMIT 0 , 10";
            OdbcDataAdapter participants = new OdbcDataAdapter();
            OdbcCommand selectP = MySQLConnection.GetmySQLConnection.CreateCommand();
            selectP.CommandText = getParticipants;
            participants.SelectCommand = selectP;

            participants.Fill(MySQLConnection.GetTotalLadder);
        }
    }
}