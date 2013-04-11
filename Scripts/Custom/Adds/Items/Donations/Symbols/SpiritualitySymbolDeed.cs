namespace Server.Items
{
	public class SpiritualitySymbolAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new SpiritualitySymbolDeed(); } }

		[Constructable]
		public SpiritualitySymbolAddon()
		{
			AddComponent( new AddonComponent( 0x14C3 ), 0, 0, 0 ); //N
			AddComponent( new AddonComponent( 0x14C6 ),  1, 0, 0 ); //NE
			AddComponent( new AddonComponent( 0x14C5 ),  1, 1, 0 ); //SE
			AddComponent( new AddonComponent( 0x14C4 ), 0,  1, 0 ); //S
		}

        public SpiritualitySymbolAddon(Serial serial)
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

	public class SpiritualitySymbolDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new SpiritualitySymbolAddon(); } }

		[Constructable]
		public SpiritualitySymbolDeed()
		{
            Name = "Spirituality symbol deed";
		}

        public SpiritualitySymbolDeed(Serial serial)
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
