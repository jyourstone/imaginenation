namespace Server.Items
{
	public class MediumCinnamonRunnerEWAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new MediumCinnamonRunnerEWDeed(); } }

		[Constructable]
		public MediumCinnamonRunnerEWAddon()
		{
			AddComponent( new AddonComponent( 0x0AE4 ), -2, -1, 0 );
			AddComponent( new AddonComponent( 0x0AE8 ),  0, -1, 0 );
            AddComponent(new AddonComponent(0x0AE8), -1, -1, 0);
            AddComponent(new AddonComponent(0x0AE8), 1, -1, 0);
			AddComponent( new AddonComponent( 0x0AE6 ),  2, -1, 0 );
			AddComponent( new AddonComponent( 0x0AE5 ), -2,  0, 0 );
			AddComponent( new AddonComponent( 0x0AEA ),  0,  0, 0 );
            AddComponent(new AddonComponent(0x0AEA), -1, 0, 0);
            AddComponent(new AddonComponent(0x0AEA), 1, 0, 0);
			AddComponent( new AddonComponent( 0x0AE3 ),  2,  0, 0 );
		}

		public MediumCinnamonRunnerEWAddon( Serial serial ) : base( serial )
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

	public class MediumCinnamonRunnerEWDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new MediumCinnamonRunnerEWAddon(); } }

		[Constructable]
		public MediumCinnamonRunnerEWDeed()
		{
			Name = "Medium Cinnamon Runner Deed - E/W";
		}

		public MediumCinnamonRunnerEWDeed( Serial serial ) : base( serial )
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
