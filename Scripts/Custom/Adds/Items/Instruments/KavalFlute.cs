namespace Server.Items
{
	public class Kavalflute : BaseInstrument
	{
		
		[Constructable]
		public Kavalflute() : base( 0x2711, 0x58A, 0x58C )
		{
			Name = "Kaval flute";
			Hue = 0x7E0;
			Weight = 2.0;
		}

		public Kavalflute( Serial serial ) : base( serial )

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