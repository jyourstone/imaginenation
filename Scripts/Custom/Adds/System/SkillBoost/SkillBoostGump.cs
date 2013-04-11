using System;
using Server.Commands;
using Server.Custom.SkillBoost;
using Server.Network;

namespace Server.Gumps
{
    public class SkillBoostGump : Gump
    {
        private readonly Mobile m_From;

        public SkillBoostGump(Mobile from) : base(50, 40)
        {
            m_From = from;

            Closable = true;
            Disposable = true;
            Dragable = true;

            AddPage(0);

            AddBackground(0, 0, 481, 411, 9200);
            AddImageTiled(12, 12, 454, 358, 2624);

            //*******************Begin first column**************************
            AddImageTiled(143, 13, 20, 20, 3604);
            AddImageTiled(13, 13, 98, 20, 3004);
            AddImageTiled(112, 13, 30, 20, 3004);
            AddHtml(15, 13, 98, 20, @"Alchemy", false, false);
            AddTextEntry(114, 13, 25, 20, 0, 0, Values.GetValue(0).ToString());

            AddImageTiled(143, 34, 20, 20, 3604);
            AddImageTiled(13, 34, 98, 20, 3004);
            AddImageTiled(112, 34, 30, 20, 3004);
            AddHtml(15, 34, 98, 20, @"Anatomy", false, false);
            AddTextEntry(114, 34, 25, 20, 0, 1, Values.GetValue(1).ToString());

            AddImageTiled(143, 55, 20, 20, 3604);
            AddImageTiled(13, 55, 98, 20, 3004);
            AddImageTiled(112, 55, 30, 20, 3004);
            AddHtml(15, 55, 98, 20, @"AnimalLore", false, false);
            AddTextEntry(114, 55, 25, 20, 0, 2, Values.GetValue(2).ToString());

            AddImageTiled(143, 76, 20, 20, 3604);
            AddImageTiled(13, 76, 98, 20, 3004);
            AddImageTiled(112, 76, 30, 20, 3004);
            AddHtml(15, 76, 98, 20, @"ItemID", false, false);
            AddTextEntry(114, 76, 25, 20, 0, 3, Values.GetValue(3).ToString());

            AddImageTiled(143, 97, 20, 20, 3604);
            AddImageTiled(13, 97, 98, 20, 3004);
            AddImageTiled(112, 97, 30, 20, 3004);
            AddHtml(15, 97, 98, 20, @"ArmsLore", false, false);
            AddTextEntry(114, 97, 25, 20, 0, 4, Values.GetValue(4).ToString());

            AddImageTiled(143, 118, 20, 20, 3604);
            AddImageTiled(13, 118, 98, 20, 3004);
            AddImageTiled(112, 118, 30, 20, 3004);
            AddHtml(15, 118, 98, 20, @"Parrying", false, false);
            AddTextEntry(114, 118, 25, 20, 0, 5, Values.GetValue(5).ToString());

            AddImageTiled(143, 139, 20, 20, 3604);
            AddImageTiled(13, 139, 98, 20, 3004);
            AddImageTiled(112, 139, 30, 20, 3004);
            AddHtml(15, 139, 98, 20, @"Begging", false, false);
            AddTextEntry(114, 139, 25, 20, 0, 6, Values.GetValue(6).ToString());

            AddImageTiled(143, 160, 20, 20, 3604);
            AddImageTiled(13, 160, 98, 20, 3004);
            AddImageTiled(112, 160, 30, 20, 3004);
            AddHtml(15, 160, 98, 20, @"Blacksmith", false, false);
            AddTextEntry(114, 160, 25, 20, 0, 7, Values.GetValue(7).ToString());

            AddImageTiled(143, 181, 20, 20, 3604);
            AddImageTiled(13, 181, 98, 20, 3004);
            AddImageTiled(112, 181, 30, 20, 3004);
            AddHtml(15, 181, 98, 20, @"Fletching", false, false);
            AddTextEntry(114, 181, 25, 20, 0, 8, Values.GetValue(8).ToString());

            AddImageTiled(143, 202, 20, 20, 3604);
            AddImageTiled(13, 202, 98, 20, 3004);
            AddImageTiled(112, 202, 30, 20, 3004);
            AddHtml(15, 202, 98, 20, @"Peacemaking", false, false);
            AddTextEntry(114, 202, 25, 20, 0, 9, Values.GetValue(9).ToString());

            AddImageTiled(143, 223, 20, 20, 3604);
            AddImageTiled(13, 223, 98, 20, 3004);
            AddImageTiled(112, 223, 30, 20, 3004);
            AddHtml(15, 223, 98, 20, @"Camping", false, false);
            AddTextEntry(114, 223, 25, 20, 0, 10, Values.GetValue(10).ToString());

            AddImageTiled(143, 244, 20, 20, 3604);
            AddImageTiled(13, 244, 98, 20, 3004);
            AddImageTiled(112, 244, 30, 20, 3004);
            AddHtml(15, 244, 98, 20, @"Carpentry", false, false);
            AddTextEntry(114, 244, 25, 20, 0, 11, Values.GetValue(11).ToString());

            AddImageTiled(143, 265, 20, 20, 3604);
            AddImageTiled(13, 265, 98, 20, 3004);
            AddImageTiled(112, 265, 30, 20, 3004);
            AddHtml(15, 265, 98, 20, @"Cartography", false, false);
            AddTextEntry(114, 265, 25, 20, 0, 12, Values.GetValue(12).ToString());

            AddImageTiled(143, 286, 20, 20, 3604);
            AddImageTiled(13, 286, 98, 20, 3004);
            AddImageTiled(112, 286, 30, 20, 3004);
            AddHtml(15, 286, 98, 20, @"Cooking", false, false);
            AddTextEntry(114, 286, 25, 20, 0, 13, Values.GetValue(13).ToString());

            AddImageTiled(143, 307, 20, 20, 3604);
            AddImageTiled(13, 307, 98, 20, 3004);
            AddImageTiled(112, 307, 30, 20, 3004);
            AddHtml(15, 307, 98, 20, @"DetectHidden", false, false);
            AddTextEntry(114, 307, 25, 20, 0, 14, Values.GetValue(14).ToString());

            AddImageTiled(143, 328, 20, 20, 3604);
            AddImageTiled(13, 328, 98, 20, 3004);
            AddImageTiled(112, 328, 30, 20, 3004);
            AddHtml(15, 328, 98, 20, @"Discordance", false, false);
            AddTextEntry(114, 328, 25, 20, 0, 15, Values.GetValue(15).ToString());

            AddImageTiled(143, 349, 20, 20, 3604);
            AddImageTiled(13, 349, 98, 20, 3004);
            AddImageTiled(112, 349, 30, 20, 3004);
            AddHtml(15, 349, 98, 20, @"EvalInt", false, false);
            AddTextEntry(114, 349, 25, 20, 0, 16, Values.GetValue(16).ToString());
            //*********************End first column***************************

            //*******************Begin second column**************************
            AddImageTiled(294, 13, 20, 20, 3604);
            AddImageTiled(164, 13, 98, 20, 3004);
            AddImageTiled(263, 13, 30, 20, 3004);
            AddHtml(166, 13, 98, 20, @"Healing", false, false);
            AddTextEntry(265, 13, 25, 20, 0, 17, Values.GetValue(17).ToString());

            AddImageTiled(294, 34, 20, 20, 3604);
            AddImageTiled(164, 34, 98, 20, 3004);
            AddImageTiled(263, 34, 30, 20, 3004);
            AddHtml(166, 34, 98, 20, @"Fishing", false, false);
            AddTextEntry(265, 34, 25, 20, 0, 18, Values.GetValue(18).ToString());

            AddImageTiled(294, 55, 20, 20, 3604);
            AddImageTiled(164, 55, 98, 20, 3004);
            AddImageTiled(263, 55, 30, 20, 3004);
            AddHtml(166, 55, 98, 20, @"Forensics", false, false);
            AddTextEntry(265, 55, 25, 20, 0, 19, Values.GetValue(19).ToString());

            AddImageTiled(294, 76, 20, 20, 3604);
            AddImageTiled(164, 76, 98, 20, 3004);
            AddImageTiled(263, 76, 30, 20, 3004);
            AddHtml(166, 76, 98, 20, @"Herding", false, false);
            AddTextEntry(265, 76, 25, 20, 0, 20, Values.GetValue(20).ToString());

            AddImageTiled(294, 97, 20, 20, 3604);
            AddImageTiled(164, 97, 98, 20, 3004);
            AddImageTiled(263, 97, 30, 20, 3004);
            AddHtml(166, 97, 98, 20, @"Hiding", false, false);
            AddTextEntry(265, 97, 25, 20, 0, 21, Values.GetValue(21).ToString());

            AddImageTiled(294, 118, 20, 20, 3604);
            AddImageTiled(164, 118, 98, 20, 3004);
            AddImageTiled(263, 118, 30, 20, 3004);
            AddHtml(166, 118, 98, 20, @"Provocation", false, false);
            AddTextEntry(265, 118, 25, 20, 0, 22, Values.GetValue(22).ToString());

            AddImageTiled(294, 139, 20, 20, 3604);
            AddImageTiled(164, 139, 98, 20, 3004);
            AddImageTiled(263, 139, 30, 20, 3004);
            AddHtml(166, 139, 98, 20, @"Inscribe", false, false);
            AddTextEntry(265, 139, 25, 20, 0, 23, Values.GetValue(23).ToString());

            AddImageTiled(294, 160, 20, 20, 3604);
            AddImageTiled(164, 160, 98, 20, 3004);
            AddImageTiled(263, 160, 30, 20, 3004);
            AddHtml(166, 160, 98, 20, @"Lockpicking", false, false);
            AddTextEntry(265, 160, 25, 20, 0, 24, Values.GetValue(24).ToString());

            AddImageTiled(294, 181, 20, 20, 3604);
            AddImageTiled(164, 181, 98, 20, 3004);
            AddImageTiled(263, 181, 30, 20, 3004);
            AddHtml(166, 181, 98, 20, @"Magery", false, false);
            AddTextEntry(265, 181, 25, 20, 0, 25, Values.GetValue(25).ToString());

            AddImageTiled(294, 202, 20, 20, 3604);
            AddImageTiled(164, 202, 98, 20, 3004);
            AddImageTiled(263, 202, 30, 20, 3004);
            AddHtml(166, 202, 98, 20, @"MagicResist", false, false);
            AddTextEntry(265, 202, 25, 20, 0, 26, Values.GetValue(26).ToString());

            AddImageTiled(294, 223, 20, 20, 3604);
            AddImageTiled(164, 223, 98, 20, 3004);
            AddImageTiled(263, 223, 30, 20, 3004);
            AddHtml(166, 223, 98, 20, @"Tactics", false, false);
            AddTextEntry(265, 223, 25, 20, 0, 27, Values.GetValue(27).ToString());

            AddImageTiled(294, 244, 20, 20, 3604);
            AddImageTiled(164, 244, 98, 20, 3004);
            AddImageTiled(263, 244, 30, 20, 3004);
            AddHtml(166, 244, 98, 20, @"Snooping", false, false);
            AddTextEntry(265, 244, 25, 20, 0, 28, Values.GetValue(28).ToString());

            AddImageTiled(294, 265, 20, 20, 3604);
            AddImageTiled(164, 265, 98, 20, 3004);
            AddImageTiled(263, 265, 30, 20, 3004);
            AddHtml(166, 265, 98, 20, @"Musicianship", false, false);
            AddTextEntry(265, 265, 25, 20, 0, 29, Values.GetValue(29).ToString());

            AddImageTiled(294, 286, 20, 20, 3604);
            AddImageTiled(164, 286, 98, 20, 3004);
            AddImageTiled(263, 286, 30, 20, 3004);
            AddHtml(166, 286, 98, 20, @"Poisoning", false, false);
            AddTextEntry(265, 286, 25, 20, 0, 30, Values.GetValue(30).ToString());

            AddImageTiled(294, 307, 20, 20, 3604);
            AddImageTiled(164, 307, 98, 20, 3004);
            AddImageTiled(263, 307, 30, 20, 3004);
            AddHtml(166, 307, 98, 20, @"Archery", false, false);
            AddTextEntry(265, 307, 25, 20, 0, 31, Values.GetValue(31).ToString());

            AddImageTiled(294, 328, 20, 20, 3604);
            AddImageTiled(164, 328, 98, 20, 3004);
            AddImageTiled(263, 328, 30, 20, 3004);
            AddHtml(166, 328, 98, 20, @"SpiritSpeak", false, false);
            AddTextEntry(265, 328, 25, 20, 0, 32, Values.GetValue(32).ToString());

            AddImageTiled(294, 349, 20, 20, 3604);
            AddImageTiled(164, 349, 98, 20, 3004);
            AddImageTiled(263, 349, 30, 20, 3004);
            AddHtml(166, 349, 98, 20, @"Stealing", false, false);
            AddTextEntry(265, 349, 25, 20, 0, 33, Values.GetValue(33).ToString());
            //*********************End second column***************************

            //********************Begin third column***************************
            AddImageTiled(445, 13, 20, 20, 3604);
            AddImageTiled(315, 13, 98, 20, 3004);
            AddImageTiled(414, 13, 30, 20, 3004);
            AddHtml(317, 13, 98, 20, @"Tailoring", false, false);
            AddTextEntry(416, 13, 25, 20, 0, 34, Values.GetValue(34).ToString());

            AddImageTiled(445, 34, 20, 20, 3604);
            AddImageTiled(315, 34, 98, 20, 3004);
            AddImageTiled(414, 34, 30, 20, 3004);
            AddHtml(317, 34, 98, 20, @"AnimalTaming", false, false);
            AddTextEntry(416, 34, 25, 20, 0, 35, Values.GetValue(35).ToString());

            AddImageTiled(445, 55, 20, 20, 3604);
            AddImageTiled(315, 55, 98, 20, 3004);
            AddImageTiled(414, 55, 30, 20, 3004);
            AddHtml(317, 55, 98, 20, @"TasteID", false, false);
            AddTextEntry(416, 55, 25, 20, 0, 36, Values.GetValue(36).ToString());

            AddImageTiled(445, 76, 20, 20, 3604);
            AddImageTiled(315, 76, 98, 20, 3004);
            AddImageTiled(414, 76, 30, 20, 3004);
            AddHtml(317, 76, 98, 20, @"Tinkering", false, false);
            AddTextEntry(416, 76, 25, 20, 0, 37, Values.GetValue(37).ToString());

            AddImageTiled(445, 97, 20, 20, 3604);
            AddImageTiled(315, 97, 98, 20, 3004);
            AddImageTiled(414, 97, 30, 20, 3004);
            AddHtml(317, 97, 98, 20, @"Tracking", false, false);
            AddTextEntry(416, 97, 25, 20, 0, 38, Values.GetValue(38).ToString());

            AddImageTiled(445, 118, 20, 20, 3604);
            AddImageTiled(315, 118, 98, 20, 3004);
            AddImageTiled(414, 118, 30, 20, 3004);
            AddHtml(317, 118, 98, 20, @"Veterinary", false, false);
            AddTextEntry(416, 118, 25, 20, 0, 39, Values.GetValue(39).ToString());

            AddImageTiled(445, 139, 20, 20, 3604);
            AddImageTiled(315, 139, 98, 20, 3004);
            AddImageTiled(414, 139, 30, 20, 3004);
            AddHtml(317, 139, 98, 20, @"Swords", false, false);
            AddTextEntry(416, 139, 25, 20, 0, 40, Values.GetValue(40).ToString());

            AddImageTiled(445, 160, 20, 20, 3604);
            AddImageTiled(315, 160, 98, 20, 3004);
            AddImageTiled(414, 160, 30, 20, 3004);
            AddHtml(317, 160, 98, 20, @"Macing", false, false);
            AddTextEntry(416, 160, 25, 20, 0, 41, Values.GetValue(41).ToString());

            AddImageTiled(445, 181, 20, 20, 3604);
            AddImageTiled(315, 181, 98, 20, 3004);
            AddImageTiled(414, 181, 30, 20, 3004);
            AddHtml(317, 181, 98, 20, @"Fencing", false, false);
            AddTextEntry(416, 181, 25, 20, 0, 42, Values.GetValue(42).ToString());

            AddImageTiled(445, 202, 20, 20, 3604);
            AddImageTiled(315, 202, 98, 20, 3004);
            AddImageTiled(414, 202, 30, 20, 3004);
            AddHtml(317, 202, 98, 20, @"Wrestling", false, false);
            AddTextEntry(416, 202, 25, 20, 0, 43, Values.GetValue(43).ToString());

            AddImageTiled(445, 223, 20, 20, 3604);
            AddImageTiled(315, 223, 98, 20, 3004);
            AddImageTiled(414, 223, 30, 20, 3004);
            AddHtml(317, 223, 98, 20, @"Lumberjacking", false, false);
            AddTextEntry(416, 223, 25, 20, 0, 44, Values.GetValue(44).ToString());

            AddImageTiled(445, 244, 20, 20, 3604);
            AddImageTiled(315, 244, 98, 20, 3004);
            AddImageTiled(414, 244, 30, 20, 3004);
            AddHtml(317, 244, 98, 20, @"Mining", false, false);
            AddTextEntry(416, 244, 25, 20, 0, 45, Values.GetValue(45).ToString());

            AddImageTiled(445, 265, 20, 20, 3604);
            AddImageTiled(315, 265, 98, 20, 3004);
            AddImageTiled(414, 265, 30, 20, 3004);
            AddHtml(317, 265, 98, 20, @"Meditation", false, false);
            AddTextEntry(416, 265, 25, 20, 0, 46, Values.GetValue(46).ToString());

            AddImageTiled(445, 286, 20, 20, 3604);
            AddImageTiled(315, 286, 98, 20, 3004);
            AddImageTiled(414, 286, 30, 20, 3004);
            AddHtml(317, 286, 98, 20, @"Stealth", false, false);
            AddTextEntry(416, 286, 25, 20, 0, 47, Values.GetValue(47).ToString());

            AddImageTiled(445, 307, 20, 20, 3604);
            AddImageTiled(315, 307, 98, 20, 3004);
            AddImageTiled(414, 307, 30, 20, 3004);
            AddHtml(317, 307, 98, 20, @"RemoveTrap", false, false);
            AddTextEntry(416, 307, 25, 20, 0, 48, Values.GetValue(48).ToString());

            AddImageTiled(445, 349, 20, 20, 3604);
            AddImageTiled(315, 349, 98, 20, 3004);
            AddImageTiled(414, 349, 30, 20, 3004);
            AddTextEntry(416, 349, 25, 20, 0, 100, GetTimeLeft(SkillBoost.ResetValuesTimer));
            AddHtml(317, 349, 98, 20, @"Hours to run", false, false);
            //*********************End third column***************************

            AddButton(19, 383, 1209, 1210, 3, GumpButtonType.Reply, 0);
            AddHtml(36, 381, 144, 20, @"Recommended", false, false);

            AddHtml(150, 381, 144, 20, @"Running:", false, false);
            AddHtml(200, 381, 144, 20, !SkillBoost.Running ? @"<BASEFONT COLOR=#FF0000>False</BASEFONT>" : @"<BASEFONT COLOR=#00FF00>True</BASEFONT>", false, false);

            AddButton(320, 380, 246, 244, 2, GumpButtonType.Reply, 0); //Default
            AddButton(400, 380, 238, 240, 1, GumpButtonType.Reply, 0); //Apply
        }

        private static string GetTimeLeft(Timer timer)
        {
            if (timer == null || !timer.Running) 
                return "12";
            
            int timeLeft = (int)(TimeSpan.FromHours(timer.Delay.TotalHours) - (DateTime.Now - SkillBoost.StartedOn)).TotalHours;

            if (timeLeft < 0)
                timeLeft = 0;

            return timeLeft.ToString();
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 1) //Apply
            {
                bool m_Success = true;
                double m_Value = 0;

                try
                {
                    TextRelay text = info.GetTextEntry(100);
                    m_Value = Convert.ToInt16(text.Text);

                    if (SkillBoost.ResetValuesTimer != null)
                    {
                        SkillBoost.ResetValuesTimer.Stop();
                        SkillBoost.ResetValuesTimer = null;
                    }
                }
                catch
                {
                    m_Success = false;
                    m_From.SendMessage("Bad time format. ## expected.");
                }

                try
                {
                    TextRelay text;

                    for (int i = 0; i < 49; i++)
                    {
                        text = info.GetTextEntry(i);
                        double newValue = Convert.ToDouble(text.Text);

                        if (newValue >= 1 && newValue <= 10)
                            Values.SkillBoostValues[i] = newValue;
                        else
                        {
                            m_Success = false;
                            m_From.SendAsciiMessage("Values need to be between 1 and 10");
                            break;
                        }
                    }
                }
                catch
                {
                    m_Success = false;
                    m_From.SendMessage("Bad format. ###,## expected.");
                }

                if (m_Success)
                {
                    SkillBoost.StartedOn = DateTime.Now;
                    SkillBoost.ResetValuesTimer = Timer.DelayCall(TimeSpan.FromHours(m_Value), new TimerCallback(SkillBoost.Stop));
                    SkillBoost.Running = true;
                    CommandLogging.WriteLine(m_From, "{0} {1} enabled skillboost for {2} hours", m_From.AccessLevel, CommandLogging.Format(m_From), SkillBoost.ResetValuesTimer.Delay.TotalHours);
                }
                else
                {
                    if (SkillBoost.ResetValuesTimer != null)
                    {
                        SkillBoost.ResetValuesTimer.Stop();
                        SkillBoost.ResetValuesTimer = null;
                    }

                    SkillBoost.Stop();
                }

            }
            else if (info.ButtonID == 2) //Default
            {
                if (SkillBoost.ResetValuesTimer != null)
                {
                    SkillBoost.ResetValuesTimer.Stop();
                    SkillBoost.ResetValuesTimer = null;
                }

                SkillBoost.Stop();
                Values.DefaultValues();
            }
            else if (info.ButtonID == 3) //Recommended
            {
                Values.RecommendedValues();
            }

            if (info.ButtonID > 0)
                m_From.SendGump(new SkillBoostGump(m_From));
        }
    }
}