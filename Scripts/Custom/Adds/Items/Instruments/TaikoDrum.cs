namespace Server.Items
{
	public class Taikodrum : BaseInstrument
	{
		[Constructable]
		public Taikodrum() : base( 0x0E9C, 0x2EC, 0x39 )
		{
			Hue = 0x909;
			Name = "Taiko drum";
			Weight = 4.0;
            Layer = Layer.TwoHanded;
		}

		public Taikodrum( Serial serial ) : base( serial )
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
				Weight = 4.0;
		}
	}
}