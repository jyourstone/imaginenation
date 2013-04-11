using Server;

namespace Iza.NPC_Speech
{
    public class MageShop : npcs
    {
        public static string RandomString = "Magic is the greatest gift to our world.  Do not abuse it.";

        public static void Respond(SpeechEventArgs e, Mobile r, string input)
        {
            if (SpeechCheck(e, "job", "what do you do"))
                res(r, "I am a living library of the arcane arts.");
            else if (SpeechCheck(e, "abbey"))
                res(r, "Oh, I haven't been to my abbey in ages.");
            else if (SpeechCheck(e, "ability", "abilities"))
                res(r, "One must have born propensity for magic, a natural ability if you will.");
            else if (SpeechCheck(e, "arcane", "art"))
                res(r, "The arcane art is one of keeping mystical secrets.");
            else if (SpeechCheck(e, "cast", "casting"))
                res(r, "Just close your eyes, say the words, and visualize.  You may need your Spellbook for assistance.");
            else if (SpeechCheck(e, "component", "supplies"))
                res(r, "Reagents and a heavy spellbook are all the components one needs.");
            else if (SpeechCheck(e, "craft"))
                res(r, "The craft of a mage takes many years to learn.");
            else if (SpeechCheck(e, "des mani"))
                res(r, "The words to weaken an opponent.");
            else if (SpeechCheck(e, "empath"))
                res(r, "I know not what someone feels nor do I wish to.");
            else if (SpeechCheck(e, "ether"))
                res(r, "I have heard rumors of ether, but have never seen it myself.");
            else if (SpeechCheck(e, "guild"))
                res(r, "Join the Mage's Guild, and you'll receive discounts on reagents.");
            else if (SpeechCheck(e, "in lor"))
                res(r, "Night Sight is very handy when reagent hunting.");
            else if (SpeechCheck(e, "in mani"))
                res(r, "You shall hear these words quite often on a battlefield.");
            else if (SpeechCheck(e, "in mani ylem"))
                res(r, "No, I am not very hungry.");
            else if (SpeechCheck(e, "in por ylem"))
                res(r, "Beginning mages use this spell quite often.");
            else if (SpeechCheck(e, "ingredients"))
                res(r, "Which reagent are you referring to?");
            else if (SpeechCheck(e, "reagent"))
                res(r, "Reagents are the basis for all magic.");
            else if (SpeechCheck(e, "res wis"))
                res(r, "The Feeblemind spell truly isn't that useful.");
            else if (SpeechCheck(e, "relvinian"))
                res(r, "I think you have been in the sun too long.");
            else if (SpeechCheck(e, "scroll"))
                res(r, "Oh yes, always carry a few extra scrolls.  One never knows.");
            else if (SpeechCheck(e, "skill"))
                res(r, "One needs to dedicate himself to the arcane arts to acquire great skill.");
            else if (SpeechCheck(e, "spell"))
                res(r, "One can never practice any spell enough.");
            else if (SpeechCheck(e, "spellbook", "spell book", "spells"))
                res(r, "Never leave town without your spellbook.");
            else if (SpeechCheck(e, "talent"))
                res(r, "I feel that one must have a natural talent when it comes to casting spells.");
            else if (SpeechCheck(e, "uus jux"))
                res(r, "Clumsy is one of the first spells that one learns.  Quite easy to cast.");

            else
                Humans.Respond(e, r, input);
        }
    }
}