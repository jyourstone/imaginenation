namespace Server.Items
{
	public class WarDrum : BaseInstrument
	{
		[Constructable]
		public WarDrum() : base( 0xE9C, 0x2E8, 0x39 )
		{
			Hue = 0xB88;
			Name = "War drum";
			Weight = 4.0;
            Layer = Layer.TwoHanded;
		}

		public WarDrum( Serial serial ) : base( serial )
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