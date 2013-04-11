namespace Server.Items
{
	public class HonestySymbolAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new HonestySymbolDeed(); } }

		[Constructable]
		public HonestySymbolAddon()
		{
			AddComponent( new AddonComponent( 0x14A3 ), 0, 0, 0 ); //N
			AddComponent( new AddonComponent( 0x14A6 ),  1, 0, 0 ); //NE
			AddComponent( new AddonComponent( 0x14A5 ),  1, 1, 0 ); //SE
			AddComponent( new AddonComponent( 0x14A4 ), 0,  1, 0 ); //S
		}

        public HonestySymbolAddon(Serial serial)
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

	public class HonestySymbolDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new HonestySymbolAddon(); } }

		[Constructable]
		public HonestySymbolDeed()
		{
            Name = "Honesty symbol deed";
		}

        public HonestySymbolDeed(Serial serial)
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
