namespace Server.Items
{
	public class Seductionlute : BaseInstrument
	{
		[Constructable]
		public Seductionlute() : base( 0xEB3, 0x418, 0x4D )
		{
			
			Name = "Lute of seduction";
			Hue = 0x9FB;
			Weight = 5.0;
            Layer = Layer.FirstValid;
		}

		public Seductionlute( Serial serial ) : base( serial )
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