using Server;

namespace Iza.NPC_Speech
{
    public class UnknownSpeech : npcs
    {
        public static void Respond(SpeechEventArgs e, Mobile r, string input)
        {
            res(r, true, "Huh?", "Um... um?", "What?");
        }
    }
}