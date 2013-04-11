namespace Server.Items
{
	public class EvilShrineAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new EvilShrineDeed(); } }

		[Constructable]
		public EvilShrineAddon()
		{
			AddComponent( new AddonComponent( 0x2A9B ), 0, 0, 0 ); //N
			AddComponent( new AddonComponent( 0x2A9A ),  1, 0, 0 ); //NE
		}

        public EvilShrineAddon(Serial serial)
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

	public class EvilShrineDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new EvilShrineAddon(); } }

		[Constructable]
		public EvilShrineDeed()
		{
            Name = "Evil shrine deed";
		}

        public EvilShrineDeed(Serial serial)
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
