using Server.Mobiles;

namespace Server.Items
{
    public class HealPotion : BaseHealPotion
	{
        /* Loki edit: Fixed values to be consistent and correctly below greater heal
        public override int MinHeal { get { return 10; } }
        public override int MaxHeal { get { return 25; } }
         */
        public override int MinHeal { get { return 20; } }
        public override int MaxHeal { get { return 20; } }
		//public override double Delay{ get{ return (Core.AOS ? 8.0 : 10.0); } }

		[Constructable]
		public HealPotion() : this(1)
		{
		}

        [Constructable]
        public HealPotion(int amount): base(PotionEffect.Heal)
        {
            Amount = amount;
            Name = "heal potion";
        }

		public HealPotion( Serial serial ) : base( serial )
		{
		}

        //public override double PotionDelay { get { return 15.0; } }
        /*
        //Maka - unstacking pots on death, stacking on resurrection
        public override void OnAdded(object parent)
        {
            int amount = Amount;
            if (amount != 1 && parent is Corpse)
            {
                Corpse corpse = (Corpse)parent;
                if (corpse.Owner is PlayerMobile)
                {
                    Delete();
                    for (int i = 0; i < amount; i++)
                        corpse.DropItem(new HealPotion());
                }
                else
                    base.OnAdded(parent);
            }
            else
                base.OnAdded(parent);
        }
        */
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