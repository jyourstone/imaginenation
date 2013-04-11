namespace Server.Items
{
	public abstract class BaseHealPotion : BasePotion
	{
		public BaseHealPotion( PotionEffect effect ) : base( 0xF0C, effect )
		{
		}

		public BaseHealPotion( Serial serial ) : base( serial )
		{
		}

		public abstract int MinHeal { get; }
		public abstract int MaxHeal { get; }
		//public abstract double Delay { get; }

        //Loki/Rob edit
        public override double PotionDelay { get { return 10.0; } }

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

		public bool DoHeal( Mobile from )
		{
			int min = Scale( from, MinHeal );
			int max = Scale( from, MaxHeal );
		    int Hits = Utility.RandomMinMax(min, max);

            if (!from.Region.OnHeal(from, ref Hits))
            {
                from.SendMessage("You cannot be healed here.");
                return false;
            }

            //Loki edit: Not required: int overMaxHits;
            int currentHits = from.Hits;
            int maxHits = from.HitsMax;


            if (currentHits + Hits > maxHits)
            {
                /* Loki edit: This formula is wrong and too long
                overMaxHits = (currentHits + Hits) - maxHits;
                if ((overMaxHits / 2) + currentHits > (maxHits + (Hits / 2)))
                    from.Hits = maxHits + (Hits / 2);
                else
                    from.Hits = maxHits + overMaxHits / 2;
                */
                from.Hits = maxHits + (int)(0.5 * (currentHits + Hits - maxHits));
            }
            else
                from.Hits += Hits;

            //Loki edit: All heal potions will do the heal animation
            from.FixedParticles(0x376A, 5, 15, 5005, EffectLayer.Waist);

            return true;

		    //from.Heal( Utility.RandomMinMax( min, max ) );
		}

		public override void Drink( Mobile from )
		{
            if (CanDrink(from))
            {
                if (DoHeal(from))
                    PlayDrinkEffect(from, this);
            }
		}
	}
}