namespace Server.Items
{
	public abstract class BaseRefreshPotion : BasePotion
	{
        //Loki/Rob edit
        public override double PotionDelay { get { return 8.0; } }

		public abstract int Refresh{ get; }

		public BaseRefreshPotion( PotionEffect effect ) : base( 0xF0B, effect )
		{
		}

		public BaseRefreshPotion( Serial serial ) : base( serial )
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

        private void GiveStam(Mobile from)
        {
            //Loki edit: Not required: int overMaxStam;
            int currentStam = from.Stam;
            int maxStam = from.StamMax;

            if (currentStam + Refresh > maxStam)
            {
                /* Loki edit: Formula fix
                 * overMaxStam = (currentStam + Refresh) - maxStam;
                if ((overMaxStam / 2) + currentStam > (maxStam + (Refresh / 2)))
                    from.Stam = maxStam + (Refresh / 2);
                else
                    from.Stam = maxStam + overMaxStam / 2; */
                from.Stam = maxStam + (int)(0.5 * (currentStam + Refresh - maxStam));
            }
            else
                from.Stam += Refresh;

            //Loki edit: All refresh potions will do the heal animation
            from.FixedParticles(0x376A, 5, 15, 5005, EffectLayer.Waist);
        }

		public override void Drink( Mobile from )
		{
            if (CanDrink(from))
            {
                GiveStam(from);
                //from.Stam += Scale(from, (int)(Refresh * from.StamMax));
                PlayDrinkEffect(from, this);
            }
		}
	}
}