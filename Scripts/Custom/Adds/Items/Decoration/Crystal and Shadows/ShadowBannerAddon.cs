namespace Server.Items
{
	public class ShadowBannerEastAddon : BaseAddon
	{
		[Constructable]
		public ShadowBannerEastAddon()
		{
			AddComponent( new AddonComponent( 13918 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 13919 ), 0, 1, 0 );
		}

		public ShadowBannerEastAddon( Serial serial ) : base( serial )
		{
		}

		public override BaseAddonDeed Deed
		{
			get { return new ShadowBannerEastAddonDeed(); }
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

	public class ShadowBannerEastAddonDeed : BaseAddonDeed
	{
		[Constructable]
		public ShadowBannerEastAddonDeed()
		{
			Name = "A deed for a Shadow Banner (Facing East)";
		}

		public ShadowBannerEastAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override BaseAddon Addon
		{
			get { return new ShadowBannerEastAddon(); }
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

	public class ShadowBannerSouthAddon : BaseAddon
	{
		[Constructable]
		public ShadowBannerSouthAddon()
		{
			AddComponent( new AddonComponent( 13916 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 13917 ), 0, 0, 0 );
		}

		public ShadowBannerSouthAddon( Serial serial ) : base( serial )
		{
		}

		public override BaseAddonDeed Deed
		{
			get { return new ShadowBannerSouthAddonDeed(); }
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

	public class ShadowBannerSouthAddonDeed : BaseAddonDeed
	{
		[Constructable]
		public ShadowBannerSouthAddonDeed()
		{
			Name = "A deed for a Shadow Banner (Facing South)";
		}

		public ShadowBannerSouthAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override BaseAddon Addon
		{
			get { return new ShadowBannerSouthAddon(); }
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