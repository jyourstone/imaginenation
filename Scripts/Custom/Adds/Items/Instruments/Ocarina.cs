namespace Server.Items
{
	public class Ocarina : BaseInstrument
	{
		
		[Constructable]
		public Ocarina() : base( 0x1421, 0x5B8, 0x5B7 )
		{
			Name = "Ocarina";
			Hue = 0x9DC;
			Weight = 2.0;
		}

		public Ocarina( Serial serial ) : base( serial )

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