namespace Server.Items
{
	public class SacrificeSymbolAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new SacrificeSymbolDeed(); } }

		[Constructable]
		public SacrificeSymbolAddon()
		{
			AddComponent( new AddonComponent( 0x150E ), 0, 0, 0 ); //N
			AddComponent( new AddonComponent( 0x1511 ),  1, 0, 0 ); //NE
			AddComponent( new AddonComponent( 0x1510 ),  1, 1, 0 ); //SE
			AddComponent( new AddonComponent( 0x150F ), 0,  1, 0 ); //S
		}

        public SacrificeSymbolAddon(Serial serial)
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

	public class SacrificeSymbolDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new SacrificeSymbolAddon(); } }

		[Constructable]
		public SacrificeSymbolDeed()
		{
            Name = "Sacrifice symbol deed";
		}

        public SacrificeSymbolDeed(Serial serial)
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
