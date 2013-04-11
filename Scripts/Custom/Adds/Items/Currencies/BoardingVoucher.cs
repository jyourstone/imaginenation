using System;

namespace Server.Items
{
	public class BoardingVoucher : Item
	{

		[Constructable]
		public BoardingVoucher() : this( 1 )
		{
		}

		[Constructable]
		public BoardingVoucher( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
		{
		}

		[Constructable]
		public BoardingVoucher( int amount ) : base( 0x2275 )
		{
			Stackable = true;
			Amount = amount;
			Hue = 0;
			Name = "boarding voucher";
			LootType = LootType.Blessed;
			Weight = 0;
		}

		public BoardingVoucher( Serial serial ) : base( serial )
		{
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