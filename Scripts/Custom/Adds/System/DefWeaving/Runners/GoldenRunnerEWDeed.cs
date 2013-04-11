namespace Server.Items
{
	public class GoldenRunnerEWAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new GoldenRunnerEWDeed(); } }

		[Constructable]
		public GoldenRunnerEWAddon()
		{
			AddComponent( new AddonComponent( 0x0ADC ), -1, -1, 0 );
			AddComponent( new AddonComponent( 0x0AE0 ),  0, -1, 0 );
			AddComponent( new AddonComponent( 0x0ADE ),  1, -1, 0 );
			AddComponent( new AddonComponent( 0x0ADD ), -1,  0, 0 );
			AddComponent( new AddonComponent( 0x0AE2 ),  0,  0, 0 );
			AddComponent( new AddonComponent( 0x0ADB ),  1,  0, 0 );
		}

		public GoldenRunnerEWAddon( Serial serial ) : base( serial )
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

	public class GoldenRunnerEWDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new GoldenRunnerEWAddon(); } }

		[Constructable]
		public GoldenRunnerEWDeed()
		{
			Name = "Golden Runner Deed - E/W";
		}

		public GoldenRunnerEWDeed( Serial serial ) : base( serial )
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
