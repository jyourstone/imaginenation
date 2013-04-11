namespace Server.Items
{
	public class RandomLanternDeed : Item
	{
		[Constructable]
		public RandomLanternDeed() : base( 0x14F0 )
		{
			Weight = 1.0;
			Name = "Random lantern deed";
		}

        public RandomLanternDeed(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( IsChildOf( from.Backpack ) )
            {
                Item w = new Lantern();
                w.Hue = Utility.RandomList(Sphere.RareHues);

                from.AddToBackpack(w);
                Delete();
            }
			else
				from.SendAsciiMessage( "That must be in your pack for you to use it." );
		}
	}
}