namespace Server.Items
{
	public class Madroneharp : BaseInstrument
	{
		[Constructable]
		public Madroneharp() : base( 0xEB2, 0x393, 0x45 )
		{
			Name = "Madrone harp" ;
			Hue = 0x29;
			Weight = 10.0;
            Layer = Layer.FirstValid;
		}

		public Madroneharp( Serial serial ) : base( serial )
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
				Weight = 10.0;
		}
	}
}