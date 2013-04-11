namespace Server.Items
{
	public class ValorSymbolAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new ValorSymbolDeed(); } }

		[Constructable]
		public ValorSymbolAddon()
		{
			AddComponent( new AddonComponent( 0x14BB ), 0, 0, 0 ); //N
			AddComponent( new AddonComponent( 0x14BE ),  1, 0, 0 ); //NE
			AddComponent( new AddonComponent( 0x14BD ),  1, 1, 0 ); //SE
			AddComponent( new AddonComponent( 0x14BC ), 0,  1, 0 ); //S
		}

        public ValorSymbolAddon(Serial serial)
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

	public class ValorSymbolDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new ValorSymbolAddon(); } }

		[Constructable]
		public ValorSymbolDeed()
		{
            Name = "Valor symbol deed";
		}

        public ValorSymbolDeed(Serial serial)
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
