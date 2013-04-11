using System;

namespace Server.Items
{
	public class ImagineNickel : Item
	{

		[Constructable]
		public ImagineNickel() : this( 1 )
		{
		}

		[Constructable]
		public ImagineNickel( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
		{
		}

		[Constructable]
		public ImagineNickel( int amount ) : base( 0xEF0 )
		{
			Stackable = true;
			Amount = amount;
			Hue = 2635;
			Name = "Imagine Nickel";
			LootType = LootType.Regular;
			Weight = 0;
		}

		public ImagineNickel( Serial serial ) : base( serial )
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