namespace Server.Items
{
	public class CinnamonRunnerNSAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new CinnamonRunnerNSDeed(); } }

		[Constructable]
		public CinnamonRunnerNSAddon()
		{
			AddComponent( new AddonComponent( 0x0AE4 ),  0, -1, 0 );
			AddComponent( new AddonComponent( 0x0AE6 ),  1, -1, 0 );
			AddComponent( new AddonComponent( 0x0AE7 ),  0,  0, 0 );
			AddComponent( new AddonComponent( 0x0AE9 ),  1,  0, 0 );
			AddComponent( new AddonComponent( 0x0AE5 ),  0,  1, 0 );
			AddComponent( new AddonComponent( 0x0AE3 ),  1,  1, 0 );
		}

		public CinnamonRunnerNSAddon( Serial serial ) : base( serial )
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

	public class CinnamonRunnerNSDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new CinnamonRunnerNSAddon(); } }

		[Constructable]
		public CinnamonRunnerNSDeed()
		{
			Name = "Cinnamon Runner Deed - N/S";
		}

		public CinnamonRunnerNSDeed( Serial serial ) : base( serial )
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
