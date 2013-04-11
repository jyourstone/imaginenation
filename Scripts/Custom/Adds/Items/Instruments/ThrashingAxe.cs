namespace Server.Items
{
	public class Thrashingaxe : BaseInstrument
	{
		[Constructable]
		public Thrashingaxe() : base( 0x2D34, 0x5a5, 0x5C3 )
		{
			Name = "The Axe of Thrashing";
			Hue = 0xBAF;
			Weight = 2.0;
		}

		public Thrashingaxe( Serial serial ) : base( serial )

{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Weight == 3.0 )
				Weight = 2.0;
		}
	}
}