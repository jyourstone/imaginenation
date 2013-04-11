using System;

namespace Server.Items
{
	public class Coral : Item
	{

		[Constructable]
		public Coral() : base( 15098 )
		{
			Name = "Coral";
			Weight = 1.0;
		}

		public Coral( Serial serial ) : base( serial )
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