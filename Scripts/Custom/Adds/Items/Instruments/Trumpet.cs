namespace Server.Items
{
	public class Trumpet : BaseInstrument
	{
		
		[Constructable]
		public Trumpet() : base( 0x2D30, 0x5A7, 0x5B3 )
		{
			Name = "Trumpet";
			Hue = 0x35;
			Weight = 2.0;
		}

		public Trumpet( Serial serial ) : base( serial )

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