namespace Server.Items
{
	public class Traditionaltambourine : BaseInstrument
	{
		[Constructable]
		public Traditionaltambourine() : base( 0xE9D, 0x4B7, 0x53 )
		{
			
			Name = "Tambourine of tradition";
			Hue = 0xA85;
			Weight = 1.0;
            Layer = Layer.FirstValid;
		}

		public Traditionaltambourine( Serial serial ) : base( serial )
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