using System;

namespace Server.Items
{
	public class Pearl : Item
	{

		[Constructable]
		public Pearl() : base( 12694 )
		{
			Name = "Pearl";
			Weight = 1.0;
		}

		public Pearl( Serial serial ) : base( serial )
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