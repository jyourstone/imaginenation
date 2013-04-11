namespace Server.Items
{
	public class BlueLargeRugAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new BlueLargeRugDeed(); } }

		[Constructable]
		public BlueLargeRugAddon()
		{
            AddComponent(new AddonComponent(0x0ABE), 0, 0, 0); //center
            AddComponent(new AddonComponent(0x0ABE), -1, 0, 0);
            AddComponent(new AddonComponent(0x0ABE), -1, 1, 0);
            AddComponent(new AddonComponent(0x0ABE), 0, 1, 0);
            AddComponent(new AddonComponent(0x0ABE), -2, 0, 0);
            AddComponent(new AddonComponent(0x0ABE), -2, 1, 0);
            AddComponent(new AddonComponent(0x0ABE), -2, 2, 0);
            AddComponent(new AddonComponent(0x0ABE), -1, 2, 0);
            AddComponent(new AddonComponent(0x0ABE), 0, 2, 0);
            AddComponent(new AddonComponent(0x0AF8), 1, 2, 0); //right
            AddComponent(new AddonComponent(0x0AF8), 1, 0, 0); 
            AddComponent(new AddonComponent(0x0AF8), 1, 1, 0);
            AddComponent(new AddonComponent(0x0AF7), -2, -1, 0); //top
            AddComponent(new AddonComponent(0x0AF7), -1, -1, 0); 
            AddComponent(new AddonComponent(0x0AF7), 0, -1, 0);
            AddComponent(new AddonComponent(0x0AC5), 1, -1, 0); //NE corner
            AddComponent(new AddonComponent(0x0AC3), -3, -1, 0); //NW corner
            AddComponent(new AddonComponent(0x0AC2), 1, 3, 0); //SE corner
            AddComponent(new AddonComponent(0x0AC4), -3, 3, 0); //SW corner
            AddComponent(new AddonComponent(0x0AF6), -3, 0, 0); //left
            AddComponent(new AddonComponent(0x0AF6), -3, 1, 0);
            AddComponent(new AddonComponent(0x0AF6), -3, 2, 0);
            AddComponent(new AddonComponent(0x0AF9), -1, 3, 0); //bottom
            AddComponent(new AddonComponent(0x0AF9), 0, 3, 0);
            AddComponent(new AddonComponent(0x0AF9), -2, 3, 0);
            
		}

        public BlueLargeRugAddon(Serial serial)
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

	public class BlueLargeRugDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new BlueLargeRugAddon(); } }

		[Constructable]
		public BlueLargeRugDeed()
		{
            Name = "Blue large rug";
		}

        public BlueLargeRugDeed(Serial serial)
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
