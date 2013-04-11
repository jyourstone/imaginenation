namespace Server.Items
{
	public class BlueRunnerEWAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new BlueRunnerEWDeed(); } }

		[Constructable]
		public BlueRunnerEWAddon()
		{
			AddComponent( new AddonComponent( 0x0AC3 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 0x0AF7 ),  0, -1, 0 );
			AddComponent( new AddonComponent( 0x0AC5 ),  1, -1, 0 );
			AddComponent( new AddonComponent( 0x0AC4 ), -1,  0, 0 );
			AddComponent( new AddonComponent( 0x0AF9 ),  0,  0, 0 );
			AddComponent( new AddonComponent( 0x0AC2 ),  1,  0, 0 );
		}

		public BlueRunnerEWAddon( Serial serial ) : base( serial )
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

	public class BlueRunnerEWDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new BlueRunnerEWAddon(); } }

		[Constructable]
		public BlueRunnerEWDeed()
		{
			Name = "Blue Runner Deed - E/W";
		}

		public BlueRunnerEWDeed( Serial serial ) : base( serial )
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
