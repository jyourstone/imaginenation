namespace Server.Items
{
	public class CinnamonRunnerEWAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new CinnamonRunnerEWDeed(); } }

		[Constructable]
		public CinnamonRunnerEWAddon()
		{
			AddComponent( new AddonComponent( 0x0AE4 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 0x0AE8 ),  0, -1, 0 );
			AddComponent( new AddonComponent( 0x0AE6 ),  1, -1, 0 );
			AddComponent( new AddonComponent( 0x0AE5 ), -1,  0, 0 );
			AddComponent( new AddonComponent( 0x0AEA ),  0,  0, 0 );
			AddComponent( new AddonComponent( 0x0AE3 ),  1,  0, 0 );
		}

		public CinnamonRunnerEWAddon( Serial serial ) : base( serial )
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

	public class CinnamonRunnerEWDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new CinnamonRunnerEWAddon(); } }

		[Constructable]
		public CinnamonRunnerEWDeed()
		{
			Name = "Cinnamon Runner Deed - E/W";
		}

		public CinnamonRunnerEWDeed( Serial serial ) : base( serial )
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
