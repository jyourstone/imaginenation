using Server.Engines.Craft;

namespace Server.Items
{
	public class ClayCraftTool : BaseTool
	{
		public override CraftSystem CraftSystem { get { return ClayCraft.CraftSystem; } }


		[Constructable]
		public ClayCraftTool() : base( 0x12B3 )
		{
			Weight = 1.0;
			Name = "Clay Crafting Tool";
			Hue = 1174;
		}

		[Constructable]
		public ClayCraftTool( int uses ) : base( uses, 0x12B3 )
		{
			Weight = 1.0;
			Hue = 1174;
		}

		public ClayCraftTool( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Weight == 2.0 )
				Weight = 1.0;
		}
	}
}
