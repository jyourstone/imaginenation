using System;
using Server;
using Server.Misc;
using System.Collections;
using Server.Network;


namespace Server.Items
{
	public class FantailGoldfish : Food
	{
		[Constructable]
		public FantailGoldfish() : base( 15107 )
		{
			Weight = 0.1;
            Stackable = false;
			FillFactor = 3;
			Name = "Fantail goldfish";
		}

		public FantailGoldfish( Serial serial ) : base( serial )
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