namespace Server.Items
{
	public abstract class BaseManaPotion : BasePotion
	{
		public BaseManaPotion( PotionEffect effect ) : base( 0xF09, effect )
		{
            Stackable = true;
           	Hue = 0x388;
		}

		public BaseManaPotion( Serial serial ) : base( serial )
		{
		}

		public abstract int Mana { get; }

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

        private void GiveMana(Mobile from)
        {
            //Loki edit: Not necessary: int overMaxMana;
            int currentMana = from.Mana;
            int maxMana = from.ManaMax;
            
            if (currentMana + Mana > maxMana)
            {
                /* Loki edit:
                overMaxMana = (currentMana + Mana) - maxMana;
                if ((overMaxMana / 2) + currentMana > (maxMana + (Mana / 2)))
                    from.Mana = maxMana + (Mana / 2);
                else
                    from.Mana = maxMana + overMaxMana/2;
                 */
                from.Mana = maxMana + (int)(0.5 * (currentMana + Mana - maxMana));
            }
            else
                from.Mana += Mana;
        }

        public override void Drink(Mobile from)
        {
            if (CanDrink(from))
            {
                GiveMana(from);
                PlayDrinkEffect(from, this);
            }
        }
	}
}