namespace Server.Items
{
	public class JusticeSymbolAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new JusticeSymbolDeed(); } }

		[Constructable]
		public JusticeSymbolAddon()
		{
			AddComponent( new AddonComponent( 0x14B3 ), 0, 0, 0 ); //N
			AddComponent( new AddonComponent( 0x14B6 ),  1, 0, 0 ); //NE
			AddComponent( new AddonComponent( 0x14B5 ),  1, 1, 0 ); //SE
			AddComponent( new AddonComponent( 0x14B4 ), 0,  1, 0 ); //S
		}

        public JusticeSymbolAddon(Serial serial)
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

	public class JusticeSymbolDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new JusticeSymbolAddon(); } }

		[Constructable]
		public JusticeSymbolDeed()
		{
            Name = "Justice symbol deed";
		}

        public JusticeSymbolDeed(Serial serial)
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
