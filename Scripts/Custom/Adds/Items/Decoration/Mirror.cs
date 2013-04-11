using System;

namespace Server.Items
{
    [FlipableAttribute(0x2A7B, 0x2A7D)]
	public class Mirror : Item
	{
		[Constructable]
		public Mirror() : base( 0x2A7B )
		{
			Weight = 10.0;
            Name = "Mirror";
		}

		public Mirror( Serial serial ) : base( serial ) { }

		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }

		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
}