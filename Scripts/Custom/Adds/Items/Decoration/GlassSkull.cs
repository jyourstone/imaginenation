using System;

namespace Server.Items
{
	public class GlassSkull : Item
	{
		[Constructable]
		public GlassSkull() : base( 0x2203 )
		{
			Weight = 5.0;
            Name = "Glass Skull";
            Hue = 1154;
		}

		public GlassSkull( Serial serial ) : base( serial ) { }

		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }

		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
}