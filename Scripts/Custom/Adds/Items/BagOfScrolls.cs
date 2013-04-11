namespace Server.Items
{
	public class BagOfScrolls : Bag
	{
		[Constructable]
		public BagOfScrolls() : this( 50, false )
		{
		}

		[Constructable]
		public BagOfScrolls( int amount ) : this( amount, false )
		{
		}

		[Constructable]
		public BagOfScrolls( int amount, bool isEventBag )
		{
			SpellScroll[] scrolls = new SpellScroll[] { new FlamestrikeScroll( amount ), new LightningScroll( amount ), new GreaterHealScroll( amount ), new MagicReflectScroll( amount ) };

			if( isEventBag )
			{
				EventItem = true;
				for( int i = 0; i < scrolls.Length; ++i )
				{
					scrolls[i].EventItem = true;
					DropItem( scrolls[i] );
				}
			}
			else
				for( int i = 0; i < scrolls.Length; ++i )
					DropItem( scrolls[i] );
		}

		public BagOfScrolls( Serial serial ) : base( serial )
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