using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class FishtankKit : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefAquarium.CraftSystem; } }

		[Constructable]
		public FishtankKit() : base( 0x1EB8 )
		{
			Name = "Fishtank kit";
			Hue = 1031;
			Weight = 1.0;
		}

		[Constructable]
		public FishtankKit( int uses ) : base( uses, 50 )
		{
			Weight = 1.0;
		}

		public FishtankKit( Serial serial ) : base( serial )
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