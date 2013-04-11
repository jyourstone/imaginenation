namespace Server.Items
{
	public class Walnutharp : BaseInstrument
	{
		[Constructable]
		public Walnutharp() : base( 0xEB2, 0x391, 0x45 )
		{
			Name = "Walnut harp";
			Hue = 0x72C;
			Weight = 10.0;
            Layer = Layer.FirstValid;
		}

		public Walnutharp( Serial serial ) : base( serial )
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