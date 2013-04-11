using System;

namespace Server.Items
{
	public class SeaGrass : Item
	{

		[Constructable]
		public SeaGrass() : base( 3219 )
		{
			Name = "Sea Grass";
			Weight = 1.0;
		}

		public SeaGrass( Serial serial ) : base( serial )
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