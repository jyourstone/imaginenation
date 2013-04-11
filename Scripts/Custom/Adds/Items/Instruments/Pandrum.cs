namespace Server.Items
{
	public class Pandrum : BaseInstrument
	{
		[Constructable]
		public Pandrum() : base( 0x03AA, 0x2E9, 0x39 )
		{
			Name = "Pan drum";
			Hue = 0xA06;
			Weight = 4.0;
		}

		public Pandrum( Serial serial ) : base( serial )
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
				Weight = 4.0;
		}
	}
}