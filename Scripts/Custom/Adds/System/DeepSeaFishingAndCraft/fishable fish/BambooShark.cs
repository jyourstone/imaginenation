using System;
using Server;
using Server.Misc;
using System.Collections;
using Server.Network;


namespace Server.Items
{
	public class BambooShark : Food
	{
		[Constructable]
		public BambooShark() : base( 15113 )
		{
			Weight = 0.1;
            Stackable = false;
			FillFactor = 3;
			Name = "Bamboo shark";
		}

		public BambooShark( Serial serial ) : base( serial )
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