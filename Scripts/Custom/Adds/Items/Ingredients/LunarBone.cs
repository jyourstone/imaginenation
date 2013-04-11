namespace Server.Items
{
	public class LunarBone : Item
	{
		[Constructable]
		public LunarBone() : this( 1 )
		{
		}

		[Constructable]
        public LunarBone(int amount): base(0xF8A)
		{
			Name = "Lunar bone";
            Hue = 53;
            Stackable = true;
            Amount = amount;
		}

        public LunarBone(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            return;
        }

	    public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}