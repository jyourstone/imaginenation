using System;

namespace Server.Items
{
	public class StarWarToken : Item
	{

		[Constructable]
		public StarWarToken() : this( 1 )
		{
		}

		[Constructable]
		public StarWarToken( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
		{
		}

		[Constructable]
		public StarWarToken( int amount ) : base( 0xE73 )
		{
			Stackable = true;
			Amount = amount;
			Hue = 1175;
			Name = "StarWar Token";
			LootType = LootType.Blessed;
			Weight = 0;
		}

		public StarWarToken( Serial serial ) : base( serial )
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