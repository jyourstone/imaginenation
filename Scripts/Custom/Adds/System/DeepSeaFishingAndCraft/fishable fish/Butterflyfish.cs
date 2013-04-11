using System;
using Server;
using Server.Misc;
using System.Collections;
using Server.Network;


namespace Server.Items
{
	public class Butterflyfish : Food
	{
		[Constructable]
		public Butterflyfish() : base( 15103 )
		{
			Weight = 0.1;
            Stackable = false;
			FillFactor = 3;
			Name = "Butterflyfish";
		}

		public Butterflyfish( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}