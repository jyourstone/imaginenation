using Server.Network;

namespace Server.Items
{
	[Flipable( 0x12AB, 0x12AC )]
	public class PokerDeck : Item
	{
		[Constructable]
		public PokerDeck() : base( 0x12AB )
		{
			Name = "Poker Deck";
		}

		public PokerDeck( Serial serial ) : base( serial )
		{
		}

		private static string GetFortune()
		{
			switch( Utility.Random( 32 ) )
			{
				default:
				case 0:
					return "Nothing";
				case 1:
					return "1 Pair, 5's high";
				case 2:
					return "Nothing";
				case 3:
					return "Nothing";
				case 4:
					return "2 Pair, 7's and 3's";
				case 5:
					return "Nothing";
				case 6:
					return "a Royal Flush";
				case 7:
					return "3 of a Kind, 8's high";
				case 8:
					return "Nothing";
				case 9:
					return "Nothing";
				case 10:
					return "a Full House, Jacks and 5's";
				case 11:
					return "Nothing";
				case 12:
					return "4 of a Kind";
				case 13:
					return "Nothing";
				case 14:
					return "Nothing";
				case 15:
					return "a Straight";
				case 16:
					return "Nothing";
				case 17:
					return "Joker's Wild! 5 of a Kind";
				case 18:
					return "a Flush, high card of 5";
				case 19:
					return "Nothing";
				case 20:
					return "2 Pair, 5's and 2's";
				case 21:
					return "Nothing";
				case 22:
					return "1 Pair Ace's high";
				case 23:
					return "Nothing";
				case 24:
					return "Nothing";
				case 25:
					return "a Flush, high card of 10";
				case 26:
					return "Nothing";
				case 27:
					return "Nothing";
				case 28:
					return "Nothing";
				case 29:
					return "a Full House, 8's and 4's";
				case 30:
					return "Nothing";
				case 31:
					return "3 of a Kind, Jacks high";
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			switch( ItemID )
			{
				case 0x12AB: // Closed north
					if( Utility.Random( 2 ) == 0 )
						ItemID = 0x12A5;
					else
						ItemID = 0x12A8;
					break;
				case 0x12AC: // Closed east
					if( Utility.Random( 2 ) == 0 )
						ItemID = 0x12A6;
					else
						ItemID = 0x12A7;
					break;
				case 0x12A5:
					from.PublicOverheadMessage( MessageType.Regular, 0, false, string.Format( "*{0} has {1} in this hand*", from.Name, GetFortune() ) );
					break;
				case 0x12A6:
					from.PublicOverheadMessage( MessageType.Regular, 0, false, string.Format( "*{0} has {1} in this hand*", from.Name, GetFortune() ) );
					break;
				case 0x12A8:
					from.PublicOverheadMessage( MessageType.Regular, 0, false, string.Format( "*{0} has {1} in this hand*", from.Name, GetFortune() ) );
					break;
				case 0x12A7:
					from.PublicOverheadMessage( MessageType.Regular, 0, false, string.Format( "*{0} has {1} in this hand*", from.Name, GetFortune() ) );
					break;
			}
		}

		public override void OnAdded( object target )
		{
			switch( ItemID )
			{
				case 0x12A5:
					ItemID = 0x12AB;
					break; // Open north
				case 0x12A6:
					ItemID = 0x12AC;
					break; // Open east
				case 0x12A8:
					ItemID = 0x12AB;
					break; // Open north
				case 0x12A7:
					ItemID = 0x12AC;
					break; // Open east
			}
		}
	}

	[Flipable( 0x12AB, 0x12AC )]
	public class DecoTarotDeck2 : Item
	{
		[Constructable]
		public DecoTarotDeck2() : base( 0x12AB )
		{
			Name = "tarot deck";
		}

		public DecoTarotDeck2( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			switch( ItemID )
			{
				case 0x12AB: // Closed north
					if( Utility.Random( 2 ) == 0 )
						ItemID = 0x12A5;
					else
						ItemID = 0x12A8;
					break;
				case 0x12AC: // Closed east
					if( Utility.Random( 2 ) == 0 )
						ItemID = 0x12A6;
					else
						ItemID = 0x12A7;
					break;
				case 0x12A5:
					ItemID = 0x12AB;
					break; // Open north
				case 0x12A6:
					ItemID = 0x12AC;
					break; // Open east
				case 0x12A8:
					ItemID = 0x12AB;
					break; // Open north
				case 0x12A7:
					ItemID = 0x12AC;
					break; // Open east
			}
		}
	}
}