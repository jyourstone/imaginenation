namespace Server.Items
{
    public class WeaponDeed : Item
    {
        [CommandProperty(AccessLevel.Counselor)]
        public override bool EventItem
        {
            get { return base.EventItem; }
            set { base.EventItem = value; }
        }

        [Constructable]
        public WeaponDeed(): base(0x14F0)
        {
            Weight = 1.0;
			Name = "A R/R Weapon Deed - Page a staff member to help you convert it";
			LootType = LootType.Blessed;
			Hue = 2531;
        }

        public WeaponDeed(Serial serial)
            : base(serial)
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
    }
}