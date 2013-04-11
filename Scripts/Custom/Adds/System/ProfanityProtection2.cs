#region

using System.Text.RegularExpressions;
using Server.Mobiles;

#endregion

namespace Server.Scripts.Custom.Adds.System
{
    public class ProfanityProtection2
    {
        public static bool Enabled;

        public static Regex m_Regex;

        #region Disallowed Words

        private static readonly string[] m_Disallowed = new[]
                                                            {
                                                                "jigaboo",
                                                                "chigaboo",
                                                                "kyke",
                                                                "kike",
                                                                "puta",
                                                                "spic",
                                                                "piss off",
                                                                "lezbo",
                                                                "lesbo",
                                                                "felatio",
                                                                "dyke",
                                                                "dildo",
                                                                "chinc",
                                                                "chink",
                                                                "cunnilingus",
                                                                "cum",
                                                                "cocksucker",
                                                                "cock",
                                                                "clitoris",
                                                                "clit",
                                                                "hitler",
                                                                "penis",
                                                                "nigga",
                                                                "nigger",
                                                                "klit",
                                                                "kunt",
                                                                "jiz",
                                                                "jism",
                                                                "jerkoff",
                                                                "jackoff",
                                                                "fag",
                                                                "faggot",
                                                                "fucken",
                                                                "fucking",
                                                                "fuckin",
                                                                "whore",
                                                                "blowjob",
                                                                "bitch",
                                                                "asshole",
                                                                "dick",
                                                                "pussy",
                                                                "snatch",
                                                                "cunt",
                                                                "stfu",
                                                                "fuck"
                                                            };

        #endregion

        public static void Initialize()
        {
            if (Enabled)
            {
                SetRegex();
                EventSink.Speech += EventSink_Speech;
            }
        }

        private static void EventSink_Speech(SpeechEventArgs e)
        {
            if (e.Mobile != null)
            {
                Mobile from = e.Mobile;

                if (from is PlayerMobile)
                {
                    e.Speech = GetFilteredSpeech(from, e.Speech);
                }
            }
        }

        public static void SetRegex()
        {
            string output = string.Empty;
            foreach (string word in m_Disallowed)
            {
                string newWord = string.Empty;
                for (int i = 0; i < word.Length; i++)
                {
                    newWord += word[i] + @"[^\w]?";
                }
                newWord += "|";
                output += newWord;
            }

            char[] trimArray = {'|'};
            output.TrimEnd(trimArray);

            m_Regex = new Regex(output, RegexOptions.IgnoreCase);
        }

        private static string GetFilteredSpeech(Mobile from, string speech)
        {
            //TODO: Make this work? :/
            //if (m_Regex.Matches(speech).Count >= 1)
            //{
            //    if (from.AccessLevel == AccessLevel.Player)
            //    {
            //        from.SendAsciiMessage("Harassing and cursing is not allowed! Repeated offences will get you jailed!");
            //        return m_Regex.Replace(speech, string.Empty);
            //    }
            //    from.SendAsciiMessage("You are staff so you can swear, but please try not to.");
            //}

            if (from.AccessLevel == AccessLevel.Player)
                return m_Regex.Replace(speech, string.Empty);

            return speech;
        }
    }
}