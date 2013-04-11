namespace Server.Items
{
	public class ChaosSymbolAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new ChaosSymbolDeed(); } }

		[Constructable]
		public ChaosSymbolAddon()
		{
			AddComponent( new AddonComponent( 0x14E3 ), 0, 0, 0 ); //N
			AddComponent( new AddonComponent( 0x14E6 ),  1, 0, 0 ); //NE
			AddComponent( new AddonComponent( 0x14E5 ),  1, 1, 0 ); //SE
			AddComponent( new AddonComponent( 0x14E4 ), 0,  1, 0 ); //S
		}

		public ChaosSymbolAddon( Serial serial ) : base( serial )
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

	public class ChaosSymbolDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new ChaosSymbolAddon(); } }

		[Constructable]
		public ChaosSymbolDeed()
		{
            Name = "Chaos symbol deed";
		}

		public ChaosSymbolDeed( Serial serial ) : base( serial )
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
