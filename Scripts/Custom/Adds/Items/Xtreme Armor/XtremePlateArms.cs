namespace Server.Items
{
	[Flipable( 0x1410, 0x1417 )]
	public class XtremePlateArms : BaseArmor
	{
		public override int BasePhysicalResistance{ get{ return 5; } }
		public override int BaseFireResistance{ get{ return 3; } }
		public override int BaseColdResistance{ get{ return 2; } }
		public override int BasePoisonResistance{ get{ return 3; } }
		public override int BaseEnergyResistance{ get{ return 2; } }

		public override int InitMinHits{ get{ return 100; } }
		public override int InitMaxHits{ get{ return 115; } }

		public override int AosStrReq{ get{ return 80; } }
		public override int OldStrReq{ get{ return 40; } }


		public override int ArmorBase{ get{ return 53; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }

		[Constructable]
		public XtremePlateArms() : base( 0x1410 )
		{
			Weight = 5.0;
            Hue = Utility.RandomList(2535, 2534);
            Name = "Xtreme plate arms";
            BaseArmorRating = 53;
            IsRenamed = true;
		}

		public XtremePlateArms( Serial serial ) : base( serial )
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

			if ( Weight == 1.0 )
				Weight = 5.0;
		}
	}
}