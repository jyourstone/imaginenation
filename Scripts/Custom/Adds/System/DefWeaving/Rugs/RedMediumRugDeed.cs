namespace Server.Items
{
	public class RedMediumRugAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new RedMediumRugDeed(); } }

		[Constructable]
		public RedMediumRugAddon()
		{
            AddComponent(new AddonComponent(0x0AC8), 0, 0, 0); //center
            AddComponent(new AddonComponent(0x0AC8), -1, 0, 0);
            AddComponent(new AddonComponent(0x0AC8), -1, 1, 0);
            AddComponent(new AddonComponent(0x0AC8), 0, 1, 0);
            AddComponent(new AddonComponent(0x0ACA), -2, -1, 0); //NW corner
            AddComponent(new AddonComponent(0x0ACE), -1, -1, 0); //top frill
            AddComponent(new AddonComponent(0x0ACE), 0, -1, 0);
            AddComponent(new AddonComponent(0x0ACC), 1, -1, 0); //NE corner
            AddComponent(new AddonComponent(0x0ACD), -2, 0, 0); //left frill
            AddComponent(new AddonComponent(0x0ACD), -2, 1, 0);
            AddComponent(new AddonComponent(0x0ACF), 1, 0, 0); //right frill
            AddComponent(new AddonComponent(0x0ACF), 1, 1, 0);
            AddComponent(new AddonComponent(0x0ACB), -2, 2, 0); //SW corner
            AddComponent(new AddonComponent(0x0AD0), -1, 2, 0); //bottom frill
            AddComponent(new AddonComponent(0x0AD0), 0, 2, 0);
            AddComponent(new AddonComponent(0x0AC9), 1, 2, 0); //SE corner
		}

        public RedMediumRugAddon(Serial serial)
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

	public class RedMediumRugDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new RedMediumRugAddon(); } }

		[Constructable]
		public RedMediumRugDeed()
		{
            Name = "Red medium rug";
		}

        public RedMediumRugDeed(Serial serial)
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
