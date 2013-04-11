namespace Server.Items
{
    public class LayeredBodySash : BaseOuterTorso
    {
        [CommandProperty(AccessLevel.Counselor)]
        public override bool EventItem
        {
            get { return base.EventItem; }
            set { base.EventItem = value; }
        }

        [Constructable]
        public LayeredBodySash(): base(0x1541)
        {
            Weight = 1.0;
			Name = "Layered body sash";
			LootType = LootType.Blessed;
			Layer = Layer.Earrings;
        }

        public LayeredBodySash(Serial serial)
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