namespace Server.Items
{
	[Flipable( 0x35F8, 0x35F9 )]
	public class CrystalStatue : Item
	{
		[Constructable]
		public CrystalStatue() : base( 0x35F8 )
		{
			Movable = true;
			Weight = 20.0;
		}

		public CrystalStatue( Serial serial ) : base( serial )
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

	[Flipable( 0x35FA, 0x35FB )]
	public class CrystalStatueTwo : Item
	{
		[Constructable]
		public CrystalStatueTwo() : base( 0x35FA )
		{
			Movable = true;
			Weight = 20.0;
		}

		public CrystalStatueTwo( Serial serial ) : base( serial )
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

	[Flipable( 0x35FC, 0x35FD )]
	public class CrystalStatueThree : Item
	{
		[Constructable]
		public CrystalStatueThree() : base( 0x35FC )
		{
			Movable = true;
			Weight = 20.0;
		}

		public CrystalStatueThree( Serial serial ) : base( serial )
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

	[Flipable( 0x35F6, 0x35F7 )]
	public class CrystalPillar : Item
	{
		[Constructable]
		public CrystalPillar() : base( 0x35F6 )
		{
			Movable = true;
			Weight = 20.0;
		}

		public CrystalPillar( Serial serial ) : base( serial )
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

	public class CrystalAltar : Item
	{
		[Constructable]
		public CrystalAltar() : base( 0x35E9 )
		{
			Movable = true;
			Weight = 20.0;
		}

		public CrystalAltar( Serial serial ) : base( serial )
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