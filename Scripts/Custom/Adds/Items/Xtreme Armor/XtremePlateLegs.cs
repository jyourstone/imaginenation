namespace Server.Items
{
	[Flipable( 0x1411, 0x141a )]
	public class XtremePlateLegs : BaseArmor
	{
		public override int BasePhysicalResistance{ get{ return 5; } }
		public override int BaseFireResistance{ get{ return 3; } }
		public override int BaseColdResistance{ get{ return 2; } }
		public override int BasePoisonResistance{ get{ return 3; } }
		public override int BaseEnergyResistance{ get{ return 2; } }

		public override int InitMinHits{ get{ return 100; } }
		public override int InitMaxHits{ get{ return 115; } }

		public override int AosStrReq{ get{ return 90; } }

		public override int OldStrReq{ get{ return 60; } }
		public override int ArmorBase{ get{ return 53; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }

		[Constructable]
		public XtremePlateLegs() : base( 0x1411 )
		{

			Weight = 7.0;
            Hue = Utility.RandomList(2535, 2534);
            Name = "Xtreme plate legs";
            BaseArmorRating = 53;
            IsRenamed = true;
		}

		public XtremePlateLegs( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}