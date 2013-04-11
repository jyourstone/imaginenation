using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class MysteryStone : Item
	{
		[Constructable]
		public MysteryStone() : base( 3800 )
		{
			Movable = false;
			Name = "a gravestone";
		}

		public MysteryStone( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( from is PlayerMobile )
				if( from.InRange( GetWorldLocation(), 3 ) && from.InLOS(this) )
				{
					from.CloseGump( typeof( MysteryStoneGump ) );
					from.SendGump( new MysteryStoneGump() );
					Effects.PlaySound( from.Location, from.Map, 0x5C7 );
				}
				else
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
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