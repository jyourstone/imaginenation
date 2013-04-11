using Server.Network;

namespace Server.Items
{
	[Flipable( 0x9A8, 0xE80 )]
	public class StaffBox : Container
	{
		public override int DefaultMaxWeight{ get{ return 0; } } 

		public override bool IsDecoContainer
		{
			get{ return false; }
		}

		[Constructable]
		public StaffBox() : base( 0x9A8 )
		{
			Movable = false;
            Name = "Staff Box";
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(GetWorldLocation(), 8))
            {
                if (from.AccessLevel > AccessLevel.Player)
                    DisplayTo(from);

                else
                    from.SendAsciiMessage("Only staff members can open this container");
            }
            else
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that
        }

		public StaffBox( Serial serial ) : base( serial )
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