using System;
using Server;

namespace Server.Items
{
	public class TrTokens : Item
	{
		[Constructable]
		public TrTokens() : this( 1 )
		{
		}

		[Constructable]
		public TrTokens( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
		{
		}

		[Constructable]
		public TrTokens( int amount ) : base( 0xEED )
		{
			Stackable = true;
			Name = "silver coin";
			Hue = 2101;
			Weight = 0;
			Amount = amount;
		}

		public TrTokens( Serial serial ) : base( serial )
		{
		}

		public override int GetDropSound()
		{
			if ( Amount <= 1 )
				return 0x2E4;
			else if ( Amount <= 5 )
				return 0x2E5;
			else
				return 0x2E6;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}