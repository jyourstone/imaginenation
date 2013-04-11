namespace Server.Items
{
	public abstract class BaseNightSightPotion : BasePotion
	{
		public BaseNightSightPotion( PotionEffect effect ) : base( 0xF06, effect )
		{
		}

		public BaseNightSightPotion( Serial serial ) : base( serial )
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

        public override void Drink(Mobile from)
        {
            if (from.LightLevel <= 25)
            {
                if (CanDrink(from))
                {
                    new LightCycle.NightSightTimer(from).Start();
                    from.LightLevel = 25;

                    PlayDrinkEffect(from, this);
                }
            }
		}
	}
}