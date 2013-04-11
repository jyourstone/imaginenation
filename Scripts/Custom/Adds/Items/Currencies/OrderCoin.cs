using System;

namespace Server.Items
{
	public class OrderCoin : Item
	{

		[Constructable]
		public OrderCoin() : this( 1 )
		{
		}

		[Constructable]
		public OrderCoin( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
		{
		}

		[Constructable]
		public OrderCoin( int amount ) : base( 0xEF0 )
		{
			Stackable = true;
			Amount = amount;
			Hue = 1325;
			Name = "Order Signet";
			LootType = LootType.Blessed;
			Weight = 0;
		}

		public OrderCoin( Serial serial ) : base( serial )
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