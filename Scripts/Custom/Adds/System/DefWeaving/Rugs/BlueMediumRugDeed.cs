namespace Server.Items
{
	public class BlueMediumRugAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new BlueMediumRugDeed(); } }

		[Constructable]
		public BlueMediumRugAddon()
		{
            AddComponent(new AddonComponent(0x0ABE), 0, 0, 0); //center
            AddComponent(new AddonComponent(0x0ABE), -1, 0, 0);
            AddComponent(new AddonComponent(0x0ABE), -1, 1, 0);
            AddComponent(new AddonComponent(0x0ABE), 0, 1, 0);
            AddComponent(new AddonComponent(0x0AC3), -2, -1, 0); //NW corner
            AddComponent(new AddonComponent(0x0AF7), -1, -1, 0); //top frill
            AddComponent(new AddonComponent(0x0AF7), 0, -1, 0);
            AddComponent(new AddonComponent(0x0AC5), 1, -1, 0); //NE corner
            AddComponent(new AddonComponent(0x0AF6), -2, 0, 0); //left frill
            AddComponent(new AddonComponent(0x0AF6), -2, 1, 0);
            AddComponent(new AddonComponent(0x0AF8), 1, 0, 0); //right frill
            AddComponent(new AddonComponent(0x0AF8), 1, 1, 0);
            AddComponent(new AddonComponent(0x0AC4), -2, 2, 0); //SW corner
            AddComponent(new AddonComponent(0x0AF9), -1, 2, 0); //bottom frill
            AddComponent(new AddonComponent(0x0AF9), 0, 2, 0);
            AddComponent(new AddonComponent(0x0AC2), 1, 2, 0); //SE corner
		}

        public BlueMediumRugAddon(Serial serial)
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

	public class BlueMediumRugDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new BlueMediumRugAddon(); } }

		[Constructable]
		public BlueMediumRugDeed()
		{
            Name = "Blue medium rug";
		}

        public BlueMediumRugDeed(Serial serial)
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
