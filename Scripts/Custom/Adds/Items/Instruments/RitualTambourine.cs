namespace Server.Items
{
	public class RitualTambourine : BaseInstrument
	{
		[Constructable]
		public RitualTambourine() : base( 0xE9E, 0x4B5, 0x53 )
		{
			Name = "Tambourine of ritual";
			Weight = 1.0;
            Hue = 0xAB1;
            Layer = Layer.FirstValid;
		}

		public RitualTambourine( Serial serial ) : base( serial )
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