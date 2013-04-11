using Server;
using Server.Mobiles;

namespace Iza.NPC_Speech
{
    public class npcs
    {
        public static void SmartRespond(SpeechEventArgs e, Mobile r, string speech)
        {
            if (r is BaseVendor)
            {
                if (r.Combatant != null)
                {
                    if (r.Combatant != e.Mobile)
                        res(r, false, "I am too busy fighting to deal with thee!");
                    else if (r.Combatant == e.Mobile)
                        res(r, true, "Quit trying to talk your way out!",
                                     "Quit speaking and fight, scum!");
                }
                else
                {
                    if (r is Mage)
                        MageShop.Respond(e, r, speech);

                    else if (r.BodyValue == 0x191 || r.BodyValue == 0x190)
                        Humans.Respond(e, r, speech);
                }
            }
        }

        public static void res(Mobile responder, params string[] response)
        {
            foreach (string str in response)
                responder.Say(str);
        }

        public static void res(Mobile responder, bool random, params string[] response)
        {
            if ( response.Length > 1 && random )
                responder.Say(response[Utility.Random(response.Length)]);
        }

        public static bool SpeechCheck(SpeechEventArgs e, params string[] input)
        {
            foreach (string str in input)
                if (e.Speech.Contains(str))
                    return true;
            return false;
        }
    }
}
