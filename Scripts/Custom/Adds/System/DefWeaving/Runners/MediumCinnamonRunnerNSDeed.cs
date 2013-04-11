namespace Server.Items
{
	public class MediumCinnamonRunnerNSAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new MediumCinnamonRunnerNSDeed(); } }

		[Constructable]
		public MediumCinnamonRunnerNSAddon()
		{
			AddComponent( new AddonComponent( 0x0AE4 ),  0, -2, 0 );
			AddComponent( new AddonComponent( 0x0AE6 ),  1, -2, 0 );
			AddComponent( new AddonComponent( 0x0AE7 ),  0,  0, 0 );
            AddComponent(new AddonComponent(0x0AE7), 0, -1, 0);
            AddComponent(new AddonComponent(0x0AE7), 0, 1, 0);
			AddComponent( new AddonComponent( 0x0AE9 ),  1,  0, 0 );
            AddComponent(new AddonComponent(0x0AE9), 1, 1, 0);
            AddComponent(new AddonComponent(0x0AE9), 1, -1, 0);
			AddComponent( new AddonComponent( 0x0AE5 ),  0,  2, 0 );
			AddComponent( new AddonComponent( 0x0AE3 ),  1,  2, 0 );
		}

		public MediumCinnamonRunnerNSAddon( Serial serial ) : base( serial )
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

	public class MediumCinnamonRunnerNSDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new MediumCinnamonRunnerNSAddon(); } }

		[Constructable]
		public MediumCinnamonRunnerNSDeed()
		{
			Name = "Medium Cinnamon Runner Deed - N/S";
		}

		public MediumCinnamonRunnerNSDeed( Serial serial ) : base( serial )
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
