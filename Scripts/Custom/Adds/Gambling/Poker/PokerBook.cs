namespace Server.Items
{
	public class PokerBook : RedBook
	{
		[Constructable]
		public PokerBook() : base( "Rules of Poker", "Administrator", 5, false )
		{
			Hue = 0x89B;

			Pages[0].Lines = new string[]
				{
					"  This book contains",
					"the official rules",
					"of Poker.",
					"  Everyone that plays",
					"poker must abide by",
					"the rules contained",
					"inside this book.",
									};

			Pages[1].Lines = new string[]
				{
					"Before you can show",
					"your hand,",
					"You MUST place your bet.",
					"After everyone has placed",
					"a bet, the first person",
					"to bet shows their hand.",
					"then in order, the rest",
					"show theirs. High Hand wins."
				};

			Pages[2].Lines = new string[]
				{
					"  The rank of hands:",
					"lowest to highest:",
					"1 Pair, 2 Pair,",
					"3 of a Kind, Straight,",
					"Flush, Full House,",
					"4 of a Kind,",
					"Royal Flush,",
					"Joker's Wold 5 of a Kind."
				};

			Pages[3].Lines = new string[]
				{
					"When two hands are even,",
					"Like: 1 Pair & 1 Pair,",
					"the higher number after",
					"rak determines the",
					"Winning hand."
				};

			Pages[4].Lines = new string[]
				{
					"If there is a tie,",
					"like both players",
					"have 1 Pair, 5's High,",
					"the players split the pot."
				};
		}

		public PokerBook( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}