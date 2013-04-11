using Server.Mobiles;

namespace Server.Items
{
    public class LesserHealPotion : BaseHealPotion
	{
        /* Loki edit: Fixed to reduce randomness and set below regular heal
        public override int MinHeal { get { return 5; } }
        public override int MaxHeal { get { return 20; } }
         */
        public override int MinHeal { get { return 15; } }
        public override int MaxHeal { get { return 15; } }
		//public override double Delay{ get{ return (Core.AOS ? 3.0 : 10.0); } }

		[Constructable]
		public LesserHealPotion() : this(1)
		{
		}

        [Constructable]
        public LesserHealPotion(int amount)  : base(PotionEffect.HealLesser)
        {
            Name = "lesser heal potion";
            Amount = amount;
        }

		public LesserHealPotion( Serial serial ) : base( serial )
		{
		}

        //public override double PotionDelay { get { return 13.0; } }
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
                        corpse.DropItem(new LesserHealPotion());
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