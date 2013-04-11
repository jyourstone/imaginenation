namespace Server.Items
{
	public class Bubenclacker : BaseInstrument
	{
		[Constructable]
		public Bubenclacker() : base( 0xE9E, 0x4B6, 0x53 )
		{
			
			Name = "Buben clacker";
			Hue = 0xAB4;
			Weight = 1.0;
		    Layer = Layer.FirstValid;
		}

		public Bubenclacker( Serial serial ) : base( serial )
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

			if ( Weight == 2.0 )
				Weight = 1.0;
		}
	}
}