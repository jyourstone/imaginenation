/* Created by Hammerhand*/

using Server.Engines.Harvest;

namespace Server.Items
{
    public class ClayShovel : BaseHarvestTool
	{
        public override HarvestSystem HarvestSystem { get { return ClayMining.System; } }

		[Constructable]
		public ClayShovel() : base( 0xF39 )
		{
            Name = "a Clay Shovel";
			Weight = 5.0;
            Hue = 1115;
			UsesRemaining = 50;
			ShowUsesRemaining = true;
		}

        public ClayShovel(Serial serial): base(serial)
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
			ShowUsesRemaining = true;
		}
	}
}