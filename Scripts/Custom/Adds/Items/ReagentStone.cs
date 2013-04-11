namespace Server.Items
{
	public class ReagentStone : Item
	{
        private int m_Price = 4600;

        [CommandProperty(AccessLevel.Seer)]
        public int Price
        {
            get { return m_Price; }
            set { m_Price = value; }
        }

		[Constructable]
		public ReagentStone() : base( 0xED5 )
		{
			Name = "Reagent Stone";
			Movable = false;
			Hue = 1159;
		}

		public ReagentStone( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
            if (Mobiles.BaseVendor.ConsumeBackpackAndBankGold(from, m_Price))
            {
                Bag regBag = new Bag();
                regBag.Hue = 1159;

                regBag.DropItem(new Bottle(12));
                regBag.DropItem(new EyesOfNewt(30));
                regBag.DropItem(new BlackPearl(50));
                regBag.DropItem(new Bloodmoss(50));
                regBag.DropItem(new Nightshade(50));
                regBag.DropItem(new Bandage(50));
                regBag.DropItem(new SulfurousAsh(60));
                regBag.DropItem(new Garlic(70));
                regBag.DropItem(new Ginseng(70));
                regBag.DropItem(new SpidersSilk(75));
                regBag.DropItem(new MandrakeRoot(85));

                from.Backpack.DropItem(regBag);
                from.SendAsciiMessage("You've bought a bag of reagents.");
                from.PlaySound(247);
            }
            else
                from.SendAsciiMessage("You do not have enough money!");
		}

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from,string.Format("{0} [{1}]",Name, m_Price));
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version 

            writer.Write(m_Price);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version == 1)
                m_Price = reader.ReadInt();
		}
	}
}