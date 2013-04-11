namespace Server.Items
{
	public class RedRunnerNSAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new RedRunnerNSDeed(); } }

		[Constructable]
		public RedRunnerNSAddon() 
		{
			AddComponent( new AddonComponent( 0x0ACA ),  0, -1, 0 );
			AddComponent( new AddonComponent( 0x0ACC ),  1, -1, 0 );
			AddComponent( new AddonComponent( 0x0ACD ),  0,  0, 0 );
			AddComponent( new AddonComponent( 0x0ACF ),  1,  0, 0 );
			AddComponent( new AddonComponent( 0x0ACB ),  0,  1, 0 );
			AddComponent( new AddonComponent( 0x0AC9 ),  1,  1, 0 );
		}

		public RedRunnerNSAddon( Serial serial ) : base( serial )
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

	public class RedRunnerNSDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new RedRunnerNSAddon(); } }

		[Constructable]
		public RedRunnerNSDeed()
		{
			Name = "Red Runner Deed - N/S";
		}

		public RedRunnerNSDeed( Serial serial ) : base( serial )
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
