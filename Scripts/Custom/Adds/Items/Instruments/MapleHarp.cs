namespace Server.Items
{
	public class MapleHarp : BaseInstrument
	{
		[Constructable]
		public MapleHarp() : base( 0xEB2, 0x392, 0x46 )
		{
			Name = "Maple harp";
			Hue = 0x6D2;
			Weight = 10.0;
            Layer = Layer.FirstValid;
		}

		public MapleHarp( Serial serial ) : base( serial )
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