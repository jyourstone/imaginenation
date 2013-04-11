namespace Server.Items
{
	public class Gold : Item
	{
		public override double DefaultWeight
		{
            get { return (Core.ML ? (0.02 / 3) : 0); }
		}

		[Constructable]
		public Gold() : this( 1 )
		{
		}

		[Constructable]
		public Gold( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
		{
		}

		[Constructable]
		public Gold( int amount ) : base( 0xEED )
		{
			Stackable = true;
			Amount = amount;
		}

		public Gold( Serial serial ) : base( serial )
		{
		}

		public override int GetDropSound()
		{
			if ( Amount == 1 )
                return 53;
			else if ( Amount == 2 )
                return 50;
            else if (Amount == 3)
                return 54;
			else
                return 55;
		}

		protected override void OnAmountChange( int oldValue )
		{
			int newValue = Amount;

			UpdateTotal( this, TotalType.Gold, newValue - oldValue );
		}

		public override int GetTotal( TotalType type )
		{
			int baseTotal = base.GetTotal( type );

			if ( type == TotalType.Gold )
				baseTotal += Amount;

			return baseTotal;
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