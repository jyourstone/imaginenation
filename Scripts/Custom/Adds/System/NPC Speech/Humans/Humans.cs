using System;
using Server;

namespace Iza.NPC_Speech
{
    public class Humans : npcs
    {
        public static void Respond(SpeechEventArgs e, Mobile r, string input)
        {
            if (SpeechCheck(e, "hello", "hail", "greet"))
                res(r, true, "Hi", "Hi!", "I greet thee.", "Hello", "Hello!", "Hail to thee",
                    String.Format("Greetings {0}", e.Mobile.Female ? "ma'am." : "sir."));
            else if (SpeechCheck(e, "bye", "goodbye", "farewell"))
                res(r, true, "I'm sure we will meet again.", "Goodbye!", "Bye.");
            else if (SpeechCheck(e, "where"))
            {
                if (e.Mobile.Region.Name != null)
                    res(r, String.Format("You are in {0}.", e.Mobile.Region.Name));
                else
                    res(r, "Why, you are here!");
            }
           // else
            //    UnknownSpeech.Respond(e, r, input);
        }
    }
}