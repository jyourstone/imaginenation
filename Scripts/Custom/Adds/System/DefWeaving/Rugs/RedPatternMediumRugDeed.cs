namespace Server.Items
{
	public class RedPatternMediumRugAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new RedPatternMediumRugDeed(); } }

		[Constructable]
		public RedPatternMediumRugAddon()
		{
            AddComponent(new AddonComponent(0x0AC7), 0, 0, 0); //center
            AddComponent(new AddonComponent(0x0AC7), -1, 0, 0);
            AddComponent(new AddonComponent(0x0AC7), -1, 1, 0);
            AddComponent(new AddonComponent(0x0AC7), 0, 1, 0);
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

        public RedPatternMediumRugAddon(Serial serial)
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

	public class RedPatternMediumRugDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new RedPatternMediumRugAddon(); } }

		[Constructable]
		public RedPatternMediumRugDeed()
		{
            Name = "Red pattern medium rug";
		}

        public RedPatternMediumRugDeed(Serial serial)
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
