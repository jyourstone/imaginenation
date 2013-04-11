using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Server.BountySystem;
using Server.Guilds;
using Server.Mobiles;
using Server.Network;
using Server.Regions;

namespace Server.Misc
{
    public class StatusPage : Timer
    {
        private const string c_StatusDir = "web/serverstatus/";
        private const string c_FullPath = c_StatusDir + @"status.html";
        public static bool Enabled = true;

        public static void Initialize()
        {
            if (Enabled)
                new StatusPage().Start();
        }

        public StatusPage()
            : base(TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(60.0))
        {
            Priority = TimerPriority.FiveSeconds;
        }

        private static string Encode(string input)
        {
            StringBuilder sb = new StringBuilder(input);

            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("\"", "&quot;");
            sb.Replace("'", "&apos;");

            return sb.ToString();
        }

        protected override void OnTick()
        {
            List<Mobile> list = new List<Mobile>();
            List<NetState> states = NetState.Instances;

            for (int i = 0; i < states.Count; ++i)
            {
                Mobile m = states[i].Mobile;

                if (m != null && (m.AccessLevel == AccessLevel.Player || !m.Hidden))
                    list.Add(m);
            }

            if (!Directory.Exists(c_StatusDir))
                Directory.CreateDirectory(c_StatusDir);

            string time = DateTime.Now.Year + "-" + DateTime.Now.Month.ToString("00") + "-" + DateTime.Now.Day.ToString("00") + " " + DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00");

            using (StreamWriter op = new StreamWriter(c_FullPath))
            {
                op.WriteLine("<html>");
                op.WriteLine("   <head>");
                op.WriteLine("      <link rel=\"stylesheet\" type=\"text/css\" href=\"servstat.css\" />");
                op.WriteLine("      <script src=\"TextFilter.js\" type=\"text/javascript\"></script>");
                op.WriteLine("      <script src=\"sorttable.js\" type=\"text/javascript\"></script>");
                op.WriteLine("      <title>Imagine Nation Status</title>");
                op.WriteLine("   </head>");
                op.WriteLine("      <big><b>Online: " + list.Count + "</big></b>");
                op.WriteLine("      <br />");
                op.WriteLine("      <i>Updated: " + time + " (GMT+1)</i>");
                op.WriteLine("      <br />");
                op.WriteLine("      <br />");
                op.WriteLine("      To sort, click on the column you want to sort by.");
                op.WriteLine("      <br />");
                op.WriteLine("      Free text search: <input name=\"filter\" onkeyup=\"filter(this, 'clientTable')\" type=\"text\">");
                op.WriteLine("      <NOSCRIPT>Greatly enhance functionality by enabling JavaScript in your browser.</NOSCRIPT>");
                op.WriteLine("      <br />");
                op.WriteLine("      <table width=\"100%\" class=\"sortable\" id=\"clientTable\" align=\"left\">");
                op.WriteLine("         <thead>");
                op.WriteLine("            <tr>");
                op.WriteLine("               <th width=\"19%\" class=\"sorttable_alpha\">Name</th>");
                op.WriteLine("               <th width=\"17%\" class=\"sorttable_alpha\">Guild</th>");
                op.WriteLine("               <th width=\"19%\" class=\"sorttable_alpha\">Region</th>");
                op.WriteLine("               <th width=\"7%\" class=\"sorttable_numeric\">Skills</th>");
                op.WriteLine("               <th width=\"5%\" class=\"sorttable_numeric\">Kills</th>");
                op.WriteLine("               <th width=\"7%\" class=\"sorttable_numeric\">Fame</th>");
                op.WriteLine("               <th width=\"7%\" class=\"sorttable_numeric\">Karma</th>");
                op.WriteLine("               <th width=\"8%\" class=\"sorttable_numeric\">Bounty</th>");
                op.WriteLine("               <th width=\"9%\" abbr=\"gametime\" class=\"sorttable_numeric\">Gametime</th>");
                op.WriteLine("            </tr>");
                op.WriteLine("         </thead>");
                op.WriteLine("         <tbody>");

                for (int i = 0; i < list.Count; ++i)
                {
                    Mobile m = list[i];

                    if (m != null)
                    {
                        //Begin writing player row
                        op.WriteLine("            <tr>");

                        #region Name
                        op.Write("               <td>");

                        //Add the font and apply the name string
                        string name = m.Name;
                        string formatString;
                        switch (m.AccessLevel)
                        {
                            case AccessLevel.Player:
                                if (m.Kills >= NotorietyHandlers.KILLS_FOR_MURDER || m.Karma <= NotorietyHandlers.PLAYER_KARMA_RED)
                                    formatString = "<div class=\"PlayerName_Evil\">{0}</div>";
                                else if (m.Criminal || m.Karma <= NotorietyHandlers.PLAYER_KARMA_GREY)
                                    formatString = "<div class=\"PlayerName_Grey\">{0}</div>";
                                else
                                    formatString = "<div class=\"PlayerName_Good\">{0}</div>";
                                break;

                            case AccessLevel.Counselor:
                                formatString = "<div class=\"CounselorName\"><b>Counselor {0}</b></div>";
                                break;

                            case AccessLevel.GameMaster:
                                formatString = "<div class=\"GMName\"><b>GM {0}</b></div>";
                                break;

                            case AccessLevel.Developer:
                                formatString = "<div class=\"DevName\"><b>Developer {0}</b></div>";
                                break;

                            case AccessLevel.Seer:
                                formatString = "<div class=\"SeerName\"><b>Seer {0}</b></div>";
                                break;

                            case AccessLevel.Administrator:
                                formatString = "<div class=\"AdminName\"><b>Admin {0}</b></div>";
                                break;

                            case AccessLevel.Owner:
                                formatString = "<div class=\"OwnerName\"><b>Owner {0}</b></div>";
                                break;

                            default:
                                formatString = "<div class=\"DefaultName\">{0}</div>";
                                break;
                        }

                        if (m is PlayerMobile && ((PlayerMobile)m).Young)
                            name = name + " [Young]";

                        //End the column
                        op.WriteLine(string.Format(formatString, Encode(name)) + "</td>");
                        #endregion

                        #region Guild

                        //Start the column
                        op.Write("               <td>");

                        Guild g = m.Guild as Guild;
                        string guildString;

                        //Add the font
                        if (g != null && m.DisplayGuildTitle)
                        {
                            switch (g.Type)
                            {
                                case GuildType.Chaos:
                                    guildString = "<div class=\"PlayerGuild_Chaos\">{0}</div>";
                                    break;
                                case GuildType.Order:
                                    guildString = "<div class=\"PlayerGuild_Order\">{0}</div>";
                                    break;
                                default:
                                    guildString = "<div class=\"PlayerGuild_Neutral\">{0}</div>";
                                    break;
                            }

                            op.Write(string.Format(guildString, Encode(g.Abbreviation)));
                        }
                        else
                            op.Write("-");

                        //End the column
                        op.WriteLine("</td>");

                        #endregion

                        #region Region

                        //Start the column
                        op.Write("               <td>");

                        op.Write(GetRegion(m));

                        //End the column
                        op.WriteLine("</td>");

                        #endregion

                        #region SumSkills

                        //Start the column
                        op.Write("               <td>");

                        op.Write(m.SkillsTotal / 10);

                        //End the column
                        op.WriteLine("</td>");

                        #endregion

                        #region Kills

                        //Start the column
                        op.Write("               <td>");

                        op.Write(m.Kills);

                        //End the column
                        op.WriteLine("</td>");

                        #endregion

                        #region Fame

                        //Start the column
                        op.Write("               <td>");

                        op.Write(m.Fame);

                        //End the column
                        op.WriteLine("</td>");

                        #endregion

                        #region Karma

                        //Start the column
                        op.Write("               <td>");

                        op.Write(m.Karma);

                        //End the column
                        op.WriteLine("</td>");

                        #endregion

                        #region Bounty

                        //Start the column
                        op.Write("               <td>");

                        ArrayList m_Entries = BountyBoard.Entries;
                        int price = 0;

                        foreach (BountyBoardEntry entry in m_Entries)
                        {
                            if (entry.Wanted == m)
                                price += entry.Price;
                        }

                        op.Write(price.ToString("#,##0", new CultureInfo("en-US")));

                        #endregion

                        #region GameTime

                        //Start the column
                        op.Write("               <td>");

                        PlayerMobile pm = m as PlayerMobile;

                        if (pm != null)
                            op.Write(pm.GameTime.Days + (pm.GameTime.Days != 1 ? " days" : " day"));

                        //End the column
                        op.WriteLine("</td>");

                        #endregion

                        //End writing player row
                        op.WriteLine("            </tr>");
                    }
                }

                op.WriteLine("         </tbody>");
                op.WriteLine("      </table>");
                op.WriteLine("   </body>");
                op.WriteLine("</html>");
            }
        }

        private static string GetRegion(Mobile m)
        {
            string regionString;
            GuardedRegion reg = (GuardedRegion)m.Region.GetRegion(typeof(GuardedRegion));
            CustomRegion cR = (CustomRegion) m.Region.GetRegion(typeof (CustomRegion));
            bool blueText = false;

            if (m.Hidden)
                regionString = "-";
            else if (string.IsNullOrEmpty(m.Region.Name))
                regionString = "-";
            else if (m.Region is DungeonRegion || m.Region.Name.ToLower().Contains("dungeon"))
                regionString = "Dungeon";
            else if (m.Region is TownRegion)
            {
                if (reg != null && !reg.Disabled)
                    blueText = true;

                regionString = m.Region.Name;
            }
            else if (reg != null && !reg.Disabled)
            {
                regionString = m.Region.Name;
                blueText = true;
            }
            else if (cR != null && (!cR.Controller.AllowPvP || !cR.Controller.CanBeDamaged || !cR.Controller.AllowHarmPlayer))
            {
                regionString = m.Region.Name;
                blueText = true;
            }
            else
                regionString = "-";

            if (regionString != "-")
            {
                regionString = string.Format(blueText ? "<div class=\"PlayerRegion_Guarded\">{0}</div>" : "<div class=\"PlayerRegion_Unguarded\">{0}</div>", regionString);
            }

            return regionString;
        }
    }
}