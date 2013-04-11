using System;

namespace Server.Items
{
	public class SunkenShip : Item
	{

		[Constructable]
		public SunkenShip() : base( 5363 )
		{
			Name = "H.M.S Destiny";
			Hue = 1162;
			Weight = 1.0;
		}

		public SunkenShip( Serial serial ) : base( serial )
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