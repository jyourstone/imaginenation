using Server.Gumps;

namespace Server.Items
{
	public class LotteryInfoStone : Item
	{
		[Constructable]
		public LotteryInfoStone() : base( 0x2AED )
		{
			Movable = false;
			Hue = 1154;
			Name = "*** Lottery Information Stone ***";
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 1))
            {
                from.SendMessage( 33, "Step Closer To The Stone, You Are Too Far Away");
                from.PlaySound(0x1F0);
            }
            else
            {
                from.CloseGump(typeof(LotteryInfoGump));
                from.SendGump(new LotteryInfoGump());
                from.PlaySound(0x1ED);
            }
        }

        public LotteryInfoStone(Serial serial) : base(serial)
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