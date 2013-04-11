namespace Server.Items
{
	public class GlobeOfSosariaAddon : BaseAddon
	{
		[Constructable]
		public GlobeOfSosariaAddon()
		{
			AddComponent( new AddonComponent( 13911 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 13920 ), 0, 0, 0 );
			AddonComponent ac;
			ac = new AddonComponent( 13911 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 13920 );
			AddComponent( ac, 0, 0, 0 );
		}

		public GlobeOfSosariaAddon( Serial serial ) : base( serial )
		{
		}

		public override BaseAddonDeed Deed
		{
			get { return new GlobeOfSosariaAddonDeed(); }
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

	public class GlobeOfSosariaAddonDeed : BaseAddonDeed
	{
		[Constructable]
		public GlobeOfSosariaAddonDeed()
		{
			Name = "A Deed for a Globe Of Sosaria";
		}

		public GlobeOfSosariaAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override BaseAddon Addon
		{
			get { return new GlobeOfSosariaAddon(); }
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