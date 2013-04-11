namespace Server.Items
{
	public class Suspenselute : BaseInstrument
	{
		[Constructable]
		public Suspenselute() : base( 0xEB4, 0x403, 0x4D )
		{
			
			Name = "Lute of suspense";
			Hue = 0xBA4;
			Weight = 5.0;
            Layer = Layer.FirstValid;
		}

		public Suspenselute( Serial serial ) : base( serial )
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