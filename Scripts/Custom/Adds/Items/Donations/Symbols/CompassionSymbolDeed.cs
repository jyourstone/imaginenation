namespace Server.Items
{
	public class CompassionSymbolAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new CompassionSymbolDeed(); } }

		[Constructable]
		public CompassionSymbolAddon()
		{
			AddComponent( new AddonComponent( 0x14AB ), 0, 0, 0 ); //N
			AddComponent( new AddonComponent( 0x14AE ),  1, 0, 0 ); //NE
			AddComponent( new AddonComponent( 0x14AD ),  1, 1, 0 ); //SE
			AddComponent( new AddonComponent( 0x14AC ), 0,  1, 0 ); //S
		}

        public CompassionSymbolAddon(Serial serial)
            : base(serial)
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

	public class CompassionSymbolDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new CompassionSymbolAddon(); } }

		[Constructable]
		public CompassionSymbolDeed()
		{
            Name = "Compassion symbol deed";
		}

        public CompassionSymbolDeed(Serial serial)
            : base(serial)
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
