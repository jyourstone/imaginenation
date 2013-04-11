namespace Server.Gumps
{
	public class LotteryInfoGump : Gump
	{
        public LotteryInfoGump() : base(0, 0)
		{
            string lottohelp01 = "To purchase a ticket players will require the correct amount of gold in there backpacks. Once you have selected the purchase ticket option you have paid for your ticket, you will then be required to make your number selections.";
            string lottohelp02 = "<br><br>Number Selection...<br><br>You can at any time select the numbers you have in front of you by moving to the continue to ticket button, this will place the ticket into your backpack.<br>You can also select the lucky dip button to let the server issue 5 random numbers. You can keep selecting the lucky dip feature until you are happy with the numbers you see.";
            string lottohelp03 = "<br><br>Lottery Draw...<br><br>When the lottery takes place the server will choose 5 random numbers, these will be the winning numbers. The server will then scan through all lottery tickets and change the ticket status to show if your ticket had any of the winning numbers. If your ticket had a matching number it will be displayed on the ticket.";
            string lottohelp04 = "<br><br>Claiming Winnings...<br><br>If you have a ticket thats showing as having a number or numbers that matched then you can claim the winnings for that ticket, this is done by using the button on the lottery ticket, once claimed the tickets status is changed and you will recieve a bank check in your backpack. You can only make one claim off a winning ticket.";
            string lottohelp05 = " You also have UNTIL the next lottery draw to claim your winning prize. If you happen to miss the claim and the next lottery draw takes place the ticket from the previous draw will become void.";
            string lottohelp06 = "<br><br>Good Luck with your lottery game.";

            Closable = false;
            Disposable = false;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(50, 37, 422, 420, 9200);
            AddImage(50, 37, 10462);
            AddImage(234, 37, 10462);
            AddImage(326, 37, 10462);
            AddImage(50, 427, 10462);
            AddImage(142, 427, 10462);
            AddImage(442, 97, 10460);
            AddImage(142, 37, 10462);
            AddImage(50, 187, 10460);
            AddImage(418, 37, 10462);
            AddImage(418, 427, 10462);
            AddImage(326, 427, 10462);
            AddImage(442, 127, 10460);
            AddImage(442, 67, 10460);
            AddImage(50, 97, 10460);
            AddImage(50, 67, 10460);
            AddImage(50, 217, 10460);
            AddImage(50, 247, 10460);
            AddImage(50, 127, 10460);
            AddImage(50, 157, 10460);
            AddImage(50, 277, 10460);
            AddImage(50, 307, 10460);
            AddImage(442, 157, 10460);
            AddImage(442, 187, 10460);
            AddImage(442, 217, 10460);
            AddImage(442, 247, 10460);
            AddImage(442, 277, 10460);
            AddImage(442, 307, 10460);
            AddImage(234, 427, 10462);
            AddImage(442, 337, 10460);
            AddImage(442, 367, 10460);
            AddImage(442, 397, 10460);
            AddImage(50, 337, 10460);
            AddImage(50, 367, 10460);
            AddImage(50, 397, 10460);
            AddImage(440, 0, 10441);
            AddImage(0, 0, 10440);
            AddAlphaRegion(78, 66, 365, 363);
            AddImage(288, 37, 10450);
            AddImage(196, 37, 10450);
            AddImage(104, 37, 10450);
            AddImage(380, 37, 10450);
            AddImage(104, 407, 10450);
            AddImage(196, 407, 10450);
            AddImage(288, 407, 10450);
            AddImage(380, 407, 10450);
            AddImage(385, 50, 1417);
            // this.AddImage(371, 370, 249);
            AddButton(371, 370, 0xF9, 0xF8, 0, GumpButtonType.Reply, 0);
            AddImage(394, 59, 5581);
            AddLabel(165, 92, 87, @"Lottery Information");
            AddHtml(92, 117, 268, 276, @"Buying a ticket...<br><br>" + lottohelp01 + lottohelp02 + lottohelp03 + lottohelp04 + lottohelp05 + lottohelp06, false, true);     
		}
    }
}