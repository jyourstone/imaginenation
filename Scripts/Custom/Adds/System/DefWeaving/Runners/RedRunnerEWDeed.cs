namespace Server.Items
{
	public class RedRunnerEWAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new RedRunnerEWDeed(); } }

		[Constructable]
		public RedRunnerEWAddon() 
		{
			AddComponent( new AddonComponent( 0x0ACA ), -1, -1, 0 );
			AddComponent( new AddonComponent( 0x0ACE ),  0, -1, 0 );
			AddComponent( new AddonComponent( 0x0ACC ),  1, -1, 0 );
			AddComponent( new AddonComponent( 0x0ACB ), -1,  0, 0 );
			AddComponent( new AddonComponent( 0x0AD0 ),  0,  0, 0 );
			AddComponent( new AddonComponent( 0x0AC9 ),  1,  0, 0 );
		}

		public RedRunnerEWAddon( Serial serial ) : base( serial )
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

	public class RedRunnerEWDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new RedRunnerEWAddon(); } }

		[Constructable]
		public RedRunnerEWDeed()
		{
			Name = "Red Runner Deed - E/W";
		}

		public RedRunnerEWDeed( Serial serial ) : base( serial )
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
