namespace Server.Items
{
	public class BlueRunnerNSAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new BlueRunnerNSDeed(); } }

		[Constructable]
		public BlueRunnerNSAddon()
		{
			AddComponent( new AddonComponent( 0x0AC3 ),  0, -1, 0 );
			AddComponent( new AddonComponent( 0x0AC5 ),  1, -1, 0 );
			AddComponent( new AddonComponent( 0x0AF6 ),  0,  0, 0 );
			AddComponent( new AddonComponent( 0x0AF8 ),  1,  0, 0 );
			AddComponent( new AddonComponent( 0x0AC4 ),  0,  1, 0 );
			AddComponent( new AddonComponent( 0x0AC2 ),  1,  1, 0 );
		}

		public BlueRunnerNSAddon( Serial serial ) : base( serial )
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

	public class BlueRunnerNSDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new BlueRunnerNSAddon(); } }

		[Constructable]
		public BlueRunnerNSDeed() 
		{
			Name = "Blue Runner Deed - N/S";
		}

		public BlueRunnerNSDeed( Serial serial ) : base( serial )
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
