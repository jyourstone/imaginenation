using Server.Network;

namespace Server.Items
{
	public class D20 : Item
	{
		[Constructable]
		public D20() : base( 0x1F19 )
		{
			Weight = 1.0;
			Name = "Guess the Number";
			Hue = 1153;
		}

		public D20( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( GetWorldLocation(), 2 ) )
				return;

			PublicOverheadMessage( MessageType.Regular, 0, false, string.Format( "*{0} rolls {1}*", from.Name, Utility.Random( 1, 20 ) ) );
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
	}
}
