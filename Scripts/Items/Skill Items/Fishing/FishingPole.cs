using System.Collections.Generic;
using Server.ContextMenus;
using Server.Engines.Harvest;

namespace Server.Items
{
	public class FishingPole : Item
	{
		[Constructable]
		public FishingPole() : base( 0x0DC0 )
		{
			Layer = Layer.OneHanded;
			Weight = 8.0;
		}

        public override void OnDoubleClick(Mobile from)
        {
                Fishing.System.BeginHarvesting(from, this);
                
                if (!Sphere.EquipOnDouble(from, this))
                    return;

                base.OnDoubleClick(from);
                
        }

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			BaseHarvestTool.AddContextMenuEntries( from, this, list, Fishing.System );
		}

		public FishingPole( Serial serial ) : base( serial )
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