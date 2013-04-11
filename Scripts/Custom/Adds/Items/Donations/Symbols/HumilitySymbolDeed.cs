namespace Server.Items
{
	public class HumilitySymbolAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new HumilitySymbolDeed(); } }

		[Constructable]
		public HumilitySymbolAddon()
		{
			AddComponent( new AddonComponent( 0x14D3 ), 0, 0, 0 ); //N
			AddComponent( new AddonComponent( 0x14D6 ),  1, 0, 0 ); //NE
			AddComponent( new AddonComponent( 0x14D5 ),  1, 1, 0 ); //SE
			AddComponent( new AddonComponent( 0x14D4 ), 0,  1, 0 ); //S
		}

		public HumilitySymbolAddon( Serial serial ) : base( serial )
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

	public class HumilitySymbolDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new HumilitySymbolAddon(); } }

		[Constructable]
		public HumilitySymbolDeed()
		{
            Name = "Humility symbol deed";
		}

		public HumilitySymbolDeed( Serial serial ) : base( serial )
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
