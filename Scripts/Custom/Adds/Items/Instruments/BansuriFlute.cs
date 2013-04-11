namespace Server.Items
{
	public class Bansuriflute : BaseInstrument
	{
		[Constructable]
		public Bansuriflute() : base( 0x2807, 0x58B, 0x503 )
		{
			Hue = 0xBB3;
			Name = "Bansuri flute";
			Weight = 2.0;
		}

		public Bansuriflute( Serial serial ) : base( serial )
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