namespace Server.Items
{

	public class Clay : Item
	{
		/*string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} clay" : "{0} clay", Amount );
			}
		}*/
        
		[Constructable]
		public Clay() : this(1)
		{
		}

        [Constructable]
        public Clay(int amount)
            : base(0x1BF2)
        {
            Stackable = true;
            Weight = 0.1;
            Hue = 1115;
            Name = "clay bar";
            Amount = amount;
        }
		public Clay(Serial serial) : base(serial)
		{
		}

		//public Item Dupe(int amount)
		//{
		//	return base.Dupe(new Clay(amount), amount);
		//}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
