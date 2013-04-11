namespace Server.Items
{
	public class ResStone : Item
	{
		[Constructable]
		public ResStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 0x2D1;
		}

		public ResStone( Serial serial ) : base( serial )
		{
		}

		public override string DefaultName
		{
			get { return "a resource stone"; }
		}

		public override void OnDoubleClick( Mobile from )
		{
			BagOfReagents regBag = new BagOfReagents( 50 );

			Item[] items = new Item[] { new FlamestrikeScroll( 25 ), new LightningScroll( 25 ), new GreaterHealScroll( 10 ), new MagicReflectScroll( 10 ), new Bandage( 80 ) };

			foreach( Item i in items )
			{
				i.Weight = 0.01;
				regBag.DropItem( i );
			}

			Point3D itemLocation = Point3D.Zero;
			for( int i = 0; i < 40; ++i )
			{
				GreaterHealPotion hp = new GreaterHealPotion();
				ManaPotion mana = new ManaPotion();

				regBag.DropItem( hp );
				regBag.DropItem( mana );

				if( i == 0 )
				{
					itemLocation = hp.Location;
					mana.Location = itemLocation;
				}
				else
				{
					hp.Location = itemLocation;
					mana.Location = itemLocation;
				}
			}

			if( !from.AddToBackpack( regBag ) )
				regBag.Delete();
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