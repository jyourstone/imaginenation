namespace Server.Items
{
	[Flipable( 0x364B, 0x369B )]
	public class ShadowStatue : Item
	{
		[Constructable]
		public ShadowStatue() : base( 0x364B )
		{
			Movable = true;
			Weight = 20.0;
		}

		public ShadowStatue( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if( Weight == 6.0 )
				Weight = 20.0;
		}
	}

	public class ShadowStatueTwo : Item
	{
		[Constructable]
		public ShadowStatueTwo() : base( 0x364C )
		{
			Movable = true;
			Weight = 20.0;
		}

		public ShadowStatueTwo( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if( Weight == 6.0 )
				Weight = 20.0;
		}
	}

	[Flipable( 0x364D, 0x369C )]
	public class ShadowStatueThree : Item
	{
		[Constructable]
		public ShadowStatueThree() : base( 0x364D )
		{
			Movable = true;
			Weight = 20.0;
		}

		public ShadowStatueThree( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if( Weight == 6.0 )
				Weight = 20.0;
		}
	}

	public class ObsidianRock : Item
	{
		[Constructable]
		public ObsidianRock() : base( 0x364E )
		{
			Movable = true;
			Weight = 20.0;
		}

		public ObsidianRock( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if( Weight == 6.0 )
				Weight = 20.0;
		}
	}

	public class ShadowStatueFour : Item
	{
		[Constructable]
		public ShadowStatueFour() : base( 0x364F )
		{
			Movable = true;
			Weight = 20.0;
		}

		public ShadowStatueFour( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if( Weight == 6.0 )
				Weight = 20.0;
		}
	}

	public class ShadowStatueFive : Item
	{
		[Constructable]
		public ShadowStatueFive() : base( 0x3650 )
		{
			Movable = true;
			Weight = 20.0;
		}

		public ShadowStatueFive( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if( Weight == 6.0 )
				Weight = 20.0;
		}
	}

	public class ShadowFirepit : Item
	{
		[Constructable]
		public ShadowFirepit() : base( 0x3651 )
		{
			Movable = true;
			Weight = 20.0;
		}

		public ShadowFirepit( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if( Weight == 6.0 )
				Weight = 20.0;
		}
	}

	public class ShadowFirepitTwo : Item
	{
		[Constructable]
		public ShadowFirepitTwo() : base( 0x3654 )
		{
			Movable = true;
			Weight = 20.0;
		}

		public ShadowFirepitTwo( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if( Weight == 6.0 )
				Weight = 20.0;
		}
	}
}