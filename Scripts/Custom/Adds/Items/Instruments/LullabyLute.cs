namespace Server.Items
{
	public class Lullabylute : BaseInstrument
	{
		[Constructable]
		public Lullabylute() : base( 0xEB4, 0x40B, 0x4D )
		{
			
			Name = "Lute of lullabies";
			Hue = 0x9DF;
			Weight = 5.0;
		    Layer = Layer.FirstValid;
		}

		public Lullabylute( Serial serial ) : base( serial )
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
				Weight = 5.0;
		}
	}
}