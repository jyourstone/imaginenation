/*
 * 
 * Made with love by Unixfreak August 2010
 * Redone by Shade 
 * Fixed by Taran
 * 
*/
using Server.Commands;
using Server.Network;
using Server.Gumps;

namespace Server.Quiz
{
    public class GeneralCommands
    {
        #region Gumps

        private class QuizGump : Gump
        {
            public QuizGump() : base(0, 0)
            {
                Closable = true;
                Disposable = true;
                Dragable = true;
                Resizable = false;

                AddPage(0);
                AddBackground(6, 10, 466, 186, 9200);
                AddLabel(10, 34, 0, @"Question:");
                AddLabel(12, 128, 0, @"Answer:");
                AddTextEntry(76, 34, 368, 81, 0, 0, @"");
                AddButton(379, 160, 247, 248, 0, GumpButtonType.Reply, 0);
                AddTextEntry(76, 129, 368, 20, 0, 1, @"");

            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                switch (info.ButtonID)
                {
                    case 0:
                        {
                            TextRelay entry0 = info.GetTextEntry(0);
                            string text0 = (entry0 == null ? "" : entry0.Text.Trim());
                            question = text0.ToLower();
                            TextRelay entry1 = info.GetTextEntry(1);
                            string text1 = (entry1 == null ? "" : entry1.Text.Trim());
                            answer = text1.ToLower();
                            World.Broadcast(133, true, question);
                            break;
                        }

                }
            }
        }
        #endregion

        public static string question;
        public static string answer;
        private static bool answered;
        private static string eventHolderName;
        private static bool running;
        private static readonly ScoreBoard scoreBoard=new ScoreBoard();

        public static void Initialize()
        {
            CommandSystem.Register("Qstart", AccessLevel.Counselor, Qstart_OnCommand);
            CommandSystem.Register("Qquestion", AccessLevel.Counselor, Qquestion_OnCommand);
            CommandSystem.Register("Qanswer", AccessLevel.Player, Qanswer_OnCommand);
            CommandSystem.Register("Qstats", AccessLevel.Counselor, Qstats_OnCommand);
            CommandSystem.Register("Qstop", AccessLevel.Counselor, Qstop_OnCommand);
	        CommandSystem.Register("Qhelp", AccessLevel.Player, Qhelp_OnCommand);
        }
        [Usage("Qstart")]//ok
        [Description("Starts the quiz")]
        private static void Qstart_OnCommand(CommandEventArgs e)
        {
            Mobile m = e.Mobile;
            if (running)
            {
                m.SendMessage("Another quiz is in progress and is held by: " + eventHolderName);
            }
            else
            {
                if (m != null)
                {
                    eventHolderName = m.Name;
                    running = true;
                    World.Broadcast(133, true, "Quiz is about to start in few moments, type .Qhelp for more info");
                }
            }
        }

	[Usage("Qhelp")]
	[Description("Displays quiz commands")]
	private static void Qhelp_OnCommand(CommandEventArgs e)
	{
		e.Mobile.SendMessage("Use .Qanswer YourAnswer to submit your guess");
		e.Mobile.SendMessage("Use .Qstats to display the current score");
	}


        [Usage("Qquestion")]//ok
        [Description("Sets current question")]
        private static void Qquestion_OnCommand(CommandEventArgs e)
        {
            if (running && eventHolderName==e.Mobile.Name)
            {
                Mobile m = e.Mobile;
                if (m.HasGump(typeof(QuizGump)))
                    m.CloseGump(typeof(QuizGump));
                m.SendGump(new QuizGump());
                answered = false;
            }
            else
            {
                e.Mobile.SendMessage("Quiz hasn't started yet or you are not the event holder");
            }
        }

        [Usage("Qanswer <\"your answer\">")]//ok
        [Description("Answer to the quiz question")]
        private static void Qanswer_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            if (!string.IsNullOrEmpty(question))
            {
                if (answer.ToLower() == e.ArgString.ToLower() && !answered )
                {
                    answered = true;
                    scoreBoard.Add(from);
                    World.Broadcast(133, true, "Correct answer "+ answer +" by: " + e.Mobile.Name);
                }
                else
                    e.Mobile.SendMessage(answered ? "This question has been answered already, please wait for another one." : "Your answer is incorrect! Try again");
            }
            else
            {
                e.Mobile.SendMessage("Quiz isn't running or there is no question yet.");
            }
        }

        [Usage("Qstats")]//ok
        [Description("Broadcasts to all online players scores of current quiz")]
        private static void Qstats_OnCommand(CommandEventArgs e)
        {
            if (running)
                ScoreBoard.Print();
            else
                e.Mobile.SendMessage("There is no quiz going on");
        }

        [Usage("Qstop")]//ok
        [Description("Ends current quiz and announces end scores")]
        private static void Qstop_OnCommand(CommandEventArgs e)
        {
            //make sure that Seer and above can also stop the quiz, if eventholder goes offline
            if ((running && e.Mobile.Name == eventHolderName) || (running && e.Mobile.AccessLevel >= AccessLevel.Seer))
            {
                    World.Broadcast(133, true, "Quiz has ended!");
                    Qstats_OnCommand(e);
                    question = "";
                    answer = "";
                    answered = false;
                    eventHolderName = "";
                    running = false;
                    scoreBoard.Clear();
            }
            else
            {
                e.Mobile.SendMessage("You do not have right to stop the quiz or you have to start quiz before you can stop it!");
            }
        }
        
    }
   
}
