namespace Server.Items
{
	public class BagOfReagents : Bag
	{
		[Constructable]
		public BagOfReagents() : this( 50, false )
		{
		}

		[Constructable]
		public BagOfReagents( int amount ) : this( amount, false )
		{
		}

		[Constructable]
		public BagOfReagents( int amount, bool isEventBag )
		{
			BaseReagent[] reagents = new BaseReagent[]
				{
					new BlackPearl( amount ),
					new Bloodmoss( amount ),
					new Garlic( amount ),
					new Ginseng( amount ),
					new MandrakeRoot( amount ),
					new Nightshade( amount ),
					new SulfurousAsh( amount ),
					new SpidersSilk( amount )
		};

			if( isEventBag )
			{
				EventItem = true;
				for( int i = 0; i < reagents.Length; ++i )
				{
					reagents[i].EventItem = true;
					DropItem( reagents[i] );
				}
			}
			else
				for( int i = 0; i < reagents.Length; ++i )
					DropItem( reagents[i] );
		}

		public BagOfReagents( Serial serial ) : base( serial )
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