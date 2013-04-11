namespace Server.Items
{
	public class RedPatternLargeRugAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new RedPatternLargeRugDeed(); } }

		[Constructable]
		public RedPatternLargeRugAddon()
		{
            AddComponent(new AddonComponent(0x0AC7), 0, 0, 0); //center
            AddComponent(new AddonComponent(0x0AC7), -1, 0, 0);
            AddComponent(new AddonComponent(0x0AC7), -1, 1, 0);
            AddComponent(new AddonComponent(0x0AC7), 0, 1, 0);
            AddComponent(new AddonComponent(0x0AC7), -2, 0, 0);
            AddComponent(new AddonComponent(0x0AC7), -2, 1, 0);
            AddComponent(new AddonComponent(0x0AC7), -2, 2, 0);
            AddComponent(new AddonComponent(0x0AC7), -1, 2, 0);
            AddComponent(new AddonComponent(0x0AC7), 0, 2, 0);
            AddComponent(new AddonComponent(0x0ACF), 1, 2, 0); //right
            AddComponent(new AddonComponent(0x0ACF), 1, 0, 0);
            AddComponent(new AddonComponent(0x0ACF), 1, 1, 0);
            AddComponent(new AddonComponent(0x0ACE), -2, -1, 0); //top
            AddComponent(new AddonComponent(0x0ACE), -1, -1, 0);
            AddComponent(new AddonComponent(0x0ACE), 0, -1, 0);
            AddComponent(new AddonComponent(0x0ACC), 1, -1, 0); //NE corner
            AddComponent(new AddonComponent(0x0ACA), -3, -1, 0); //NW corner
            AddComponent(new AddonComponent(0x0AC9), 1, 3, 0); //SE corner
            AddComponent(new AddonComponent(0x0ACB), -3, 3, 0); //SW corner
            AddComponent(new AddonComponent(0x0ACD), -3, 0, 0); //left
            AddComponent(new AddonComponent(0x0ACD), -3, 1, 0);
            AddComponent(new AddonComponent(0x0ACD), -3, 2, 0);
            AddComponent(new AddonComponent(0x0AD0), -1, 3, 0); //bottom
            AddComponent(new AddonComponent(0x0AD0), 0, 3, 0);
            AddComponent(new AddonComponent(0x0AD0), -2, 3, 0);
            
		}

        public RedPatternLargeRugAddon(Serial serial)
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

	public class RedPatternLargeRugDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new RedPatternLargeRugAddon(); } }

		[Constructable]
		public RedPatternLargeRugDeed()
		{
            Name = "Red pattern large rug";
		}

        public RedPatternLargeRugDeed(Serial serial)
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
