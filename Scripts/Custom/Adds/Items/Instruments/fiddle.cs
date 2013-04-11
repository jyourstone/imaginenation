namespace Server.Items
{
	public class fiddle : BaseInstrument
	{
		[Constructable]
		public fiddle() : base( 0x1E29, 0x5B1, 0x5B0 )
		{
			Name = "Fiddle";
			Weight = 2.0;
		}

		public fiddle( Serial serial ) : base( serial )

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