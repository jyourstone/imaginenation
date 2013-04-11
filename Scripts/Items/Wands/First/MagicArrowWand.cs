using Server.Spells.First;

namespace Server.Items
{
	public class MagicArrowWand : BaseWand
	{
		[Constructable]
		public MagicArrowWand() : base( 17 )
		{
            ItemID = 0xdf2;
            Name = "Magic Arrow";
		}

		public MagicArrowWand( Serial serial ) : base( serial )
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

		public override void OnWandUse( Mobile from )
		{
			Cast( new MagicArrowSpell( from, this ) );
		}
	}
}