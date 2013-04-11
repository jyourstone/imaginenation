namespace Server.Items
{
	public class CrystalTableEastAddon : BaseAddon
	{
		[Constructable]
		public CrystalTableEastAddon()
		{
			AddComponent( new AddonComponent( 13830 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 13829 ), 0, 1, 0 );
			AddonComponent ac;
			ac = new AddonComponent( 13829 );
			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 13830 );
			AddComponent( ac, 0, 0, 0 );
		}

		public CrystalTableEastAddon( Serial serial ) : base( serial )
		{
		}

		public override BaseAddonDeed Deed
		{
			get { return new CrystalTableEastAddonDeed(); }
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

	public class CrystalTableEastAddonDeed : BaseAddonDeed
	{
		[Constructable]
		public CrystalTableEastAddonDeed()
		{
			Name = "A Deed for a Crystal Table (facing east)";
		}

		public CrystalTableEastAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override BaseAddon Addon
		{
			get { return new CrystalTableEastAddon(); }
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

	public class CrystalTableSouthAddon : BaseAddon
	{
		[Constructable]
		public CrystalTableSouthAddon()
		{
			AddComponent( new AddonComponent( 13831 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 13832 ), 0, 0, 0 );
			AddonComponent ac;
			ac = new AddonComponent( 13831 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 13832 );
			AddComponent( ac, 0, 0, 0 );
		}

		public CrystalTableSouthAddon( Serial serial ) : base( serial )
		{
		}

		public override BaseAddonDeed Deed
		{
			get { return new CrystalTableSouthAddonDeed(); }
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

	public class CrystalTableSouthAddonDeed : BaseAddonDeed
	{
		[Constructable]
		public CrystalTableSouthAddonDeed()
		{
			Name = "A Deed for a Crystal Table (facing south)";
		}

		public CrystalTableSouthAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override BaseAddon Addon
		{
			get { return new CrystalTableSouthAddon(); }
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