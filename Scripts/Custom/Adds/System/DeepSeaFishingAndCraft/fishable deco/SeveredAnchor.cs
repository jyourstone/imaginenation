using System;

namespace Server.Items
{
	public class SeveredAnchor : Item
	{

		[Constructable]
		public SeveredAnchor() : base( 5367 )
		{
			Name = "Severed Anchor";
			Weight = 1.0;
		}

		public SeveredAnchor( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}