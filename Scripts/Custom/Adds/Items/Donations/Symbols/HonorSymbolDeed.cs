namespace Server.Items
{
	public class HonorSymbolAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new HonorSymbolDeed(); } }

		[Constructable]
		public HonorSymbolAddon()
		{
			AddComponent( new AddonComponent( 0x14CB ), 0, 0, 0 ); //N
			AddComponent( new AddonComponent( 0x14CE ),  1, 0, 0 ); //NE
			AddComponent( new AddonComponent( 0x14CD ),  1, 1, 0 ); //SE
			AddComponent( new AddonComponent( 0x14CC ), 0,  1, 0 ); //S
		}

        public HonorSymbolAddon(Serial serial)
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

	public class HonorSymbolDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new HonorSymbolAddon(); } }

		[Constructable]
		public HonorSymbolDeed()
		{
            Name = "Honor symbol deed";
		}

        public HonorSymbolDeed(Serial serial)
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
