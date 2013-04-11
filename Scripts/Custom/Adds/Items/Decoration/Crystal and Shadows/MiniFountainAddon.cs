namespace Server.Items
{
	public class MiniFountainAddon : BaseAddon
	{
		[Constructable]
		public MiniFountainAddon()
		{
			AddComponent( new AddonComponent( 16144 ), 0, 0, 5 );
			AddComponent( new AddonComponent( 1801 ), 0, 0, 0 );
			AddonComponent ac;
			ac = new AddonComponent( 16144 );
			AddComponent( ac, 0, 0, 5 );
			ac = new AddonComponent( 1801 );
			AddComponent( ac, 0, 0, 0 );
		}

		public MiniFountainAddon( Serial serial ) : base( serial )
		{
		}

		public override BaseAddonDeed Deed
		{
			get { return new MiniFountainAddonDeed(); }
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class MiniFountainAddonDeed : BaseAddonDeed
	{
		[Constructable]
		public MiniFountainAddonDeed()
		{
			Name = "A Small Fountain Deed";
		}

		public MiniFountainAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override BaseAddon Addon
		{
			get { return new MiniFountainAddon(); }
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}