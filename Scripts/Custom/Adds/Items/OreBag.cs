namespace Server.Items
{
	public class OreBag : Bag
	{
		[Constructable]
		public OreBag() : this( 10 )
		{
		}

		[Constructable]
		public OreBag( int amount )
		{
			DropItem( new IronOre( amount ) );
			DropItem( new DullCopperOre( amount ) );
			DropItem( new ShadowIronOre( amount ) );
			DropItem( new CopperOre( amount ) );
			//DropItem( new BronzeOre( amount ) );
			DropItem( new GoldOre( amount ) );
			DropItem( new AgapiteOre( amount ) );
			DropItem( new VeriteOre( amount ) );
			DropItem( new ValoriteOre( amount ) );
			DropItem( new AquaOre( amount ) );
			DropItem( new RoseOre( amount ) );
			DropItem( new OceanicOre( amount ) );
			DropItem( new OldCopperOre( amount ) );
			DropItem( new MytherilOre( amount ) );
			DropItem( new BlackDiamondOre( amount ) );
			//DropItem( new AdamantiumOre( amount ) );
			DropItem( new SilverOre( amount ) );
			DropItem( new IceOre( amount ) );
			DropItem( new BloodRockOre( amount ) );
			DropItem( new OpiateOre( amount ) );
			DropItem( new SandRockOre( amount ) );
			DropItem( new BlackRockOre( amount ) );
			DropItem( new DaemonSteelOre( amount ) );
			DropItem( new SapphireOre( amount ) );
            DropItem(new AmethystOre(amount));
            DropItem(new ReactiveOre(amount));
		}

		public OreBag( Serial serial ) : base( serial )
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

	public class IngotBag : Bag
	{
		[Constructable]
		public IngotBag() : this( 10 )
		{
		}

		[Constructable]
		public IngotBag( int amount )
		{
			DropItem( new IronIngot( amount ) );
			DropItem( new DullCopperIngot( amount ) );
			DropItem( new ShadowIronIngot( amount ) );
			DropItem( new CopperIngot( amount ) );
			//DropItem( new BronzeIngot( amount ) );
			DropItem( new GoldIngot( amount ) );
			DropItem( new AgapiteIngot( amount ) );
			DropItem( new VeriteIngot( amount ) );
			DropItem( new ValoriteIngot( amount ) );
			DropItem( new AquaIngot( amount ) );
			DropItem( new RoseIngot( amount ) );
			DropItem( new OceanicIngot( amount ) );
			DropItem( new OldCopperIngot( amount ) );
			DropItem( new MytherilIngot( amount ) );
			DropItem( new BlackDiamondIngot( amount ) );
			//DropItem( new AdamantiumIngot( amount ) );
			DropItem( new SilverIngot( amount ) );
			DropItem( new IceIngot( amount ) );
			DropItem( new BloodRockIngot( amount ) );
			DropItem( new OpiateIngot( amount ) );
			DropItem( new SandRockIngot( amount ) );
			DropItem( new BlackRockIngot( amount ) );
			DropItem( new DaemonSteelIngot( amount ) );
			DropItem( new SapphireIngot( amount ) );
            DropItem(new AmethystIngot(amount));
            DropItem(new ReactiveIngot(amount));
		}

		public IngotBag( Serial serial ) : base( serial )
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

	public class GraniteBag : Bag
	{
		[Constructable]
		public GraniteBag() : this( 10 )
		{
		}

		[Constructable]
		public GraniteBag( int amount )
		{
			DropItem( new DullCopperGranite( amount ) );
			DropItem( new ShadowIronGranite( amount ) );
			DropItem( new CopperGranite( amount ) );
			DropItem( new BronzeGranite( amount ) );
			DropItem( new GoldGranite( amount ) );
			DropItem( new AgapiteGranite( amount ) );
			DropItem( new VeriteGranite( amount ) );
			DropItem( new ValoriteGranite( amount ) );
		}

		public GraniteBag( Serial serial ) : base( serial )
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