namespace Server.Items
{
	public class GoldenRunnerNSAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new GoldenRunnerNSDeed(); } }

		[Constructable]
		public GoldenRunnerNSAddon()
		{
			AddComponent( new AddonComponent( 0x0ADC ),  0, -1, 0 );
			AddComponent( new AddonComponent( 0x0ADE ),  1, -1, 0 );
			AddComponent( new AddonComponent( 0x0ADF ),  0,  0, 0 );
			AddComponent( new AddonComponent( 0x0AE1 ),  1,  0, 0 );
			AddComponent( new AddonComponent( 0x0ADD ),  0,  1, 0 );
			AddComponent( new AddonComponent( 0x0ADB ),  1,  1, 0 );
		}

		public GoldenRunnerNSAddon( Serial serial ) : base( serial )
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
		}
	}

	public class GoldenRunnerNSDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new GoldenRunnerNSAddon(); } }

		[Constructable]
		public GoldenRunnerNSDeed()
		{
			Name = "Golden Runner Deed - N/S";
		}

		public GoldenRunnerNSDeed( Serial serial ) : base( serial )
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
		}
	}
}
