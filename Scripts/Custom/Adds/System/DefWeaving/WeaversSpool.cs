using Server.Engines.Craft;

namespace Server.Items
{
	[Flipable( 0x1420, 0x1421 )]
	public class WeaversSpool : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefWeaving.CraftSystem; } }

		[Constructable]
		public WeaversSpool() : base( 0x1420 )
		{
			Name = "Weaver's spool";
            UsesRemaining = 8;
            ShowUsesRemaining = true;
			Weight = 4.0;
		}
        

		[Constructable]
		public WeaversSpool( int uses ) : base( uses, 0x1420 )
		{
			Weight = 4.0;
		}

        public override void OnSingleClick(Mobile from)
        {
            if (UsesRemaining != 0)
            {
                LabelTo(from, "Weaver's spool with {0} uses remaining", UsesRemaining);
            }
        }

		public WeaversSpool( Serial serial ) : base( serial )
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
		}
	}
}
