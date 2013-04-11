namespace Server.Items
{
	public class FullRoomCinnamonRugAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new FullRoomCinnamonRugDeed(); } }

		[Constructable]
		public FullRoomCinnamonRugAddon()
		{
			AddComponent( new AddonComponent( 0x0AAE ), -2, -2, 0 );
			AddComponent( new AddonComponent( 0x0AAA ),  0, -2, 0 );
            AddComponent(new AddonComponent(0x0AAA), 1, -2, 0);
            AddComponent(new AddonComponent(0x0AAA), -1, -2, 0);
			AddComponent( new AddonComponent( 0x0AAF ),  2, -2, 0 );
			AddComponent( new AddonComponent( 0x0AAD ), -2,  0, 0 );
            AddComponent(new AddonComponent(0x0AAD), -2, 1, 0);//
            AddComponent(new AddonComponent(0x0AAD), -2, -1, 0);//
			AddComponent( new AddonComponent( 0x0AA9 ),  0,  0, 0 );
            AddComponent(new AddonComponent(0x0AA9), -1, -1, 0);
            AddComponent(new AddonComponent(0x0AA9), 0, -1, 0);
            AddComponent(new AddonComponent(0x0AA9), 1, -1, 0);
            AddComponent(new AddonComponent(0x0AA9), -1, 0, 0);
            AddComponent(new AddonComponent(0x0AA9), 1, 0, 0);
            AddComponent(new AddonComponent(0x0AA9), -1, 1, 0);
            AddComponent(new AddonComponent(0x0AA9), 0, 1, 0);
            AddComponent(new AddonComponent(0x0AA9), 1, 1, 0);
			AddComponent( new AddonComponent( 0x0AAB ),  2,  0, 0 );
            AddComponent(new AddonComponent(0x0AAB), 2, 1, 0);//
            AddComponent(new AddonComponent(0x0AAB), 2, -1, 0);//
			AddComponent( new AddonComponent( 0x0AB1 ), -2,  2, 0 );
			AddComponent( new AddonComponent( 0x0AAC ),  0,  2, 0 );
            AddComponent(new AddonComponent(0x0AAC), -1, 2, 0);
            AddComponent(new AddonComponent(0x0AAC), 1, 2, 0);
			AddComponent( new AddonComponent( 0x0AB0 ),  2,  2, 0 );
		}

		public FullRoomCinnamonRugAddon( Serial serial ) : base( serial )
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

	public class FullRoomCinnamonRugDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new FullRoomCinnamonRugAddon(); } }

		[Constructable]
		public FullRoomCinnamonRugDeed()
		{
			Name = "Full room cinnamon rug (5x5)";
		}

		public FullRoomCinnamonRugDeed( Serial serial ) : base( serial )
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
