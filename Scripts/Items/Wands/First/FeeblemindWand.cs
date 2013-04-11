using Server.Spells.First;

namespace Server.Items
{
	public class FeeblemindWand : BaseWand
	{
		[Constructable]
		public FeeblemindWand() : base( 17 )
		{
            ItemID = 0xdf4;
            Name = "Feeblemind";
		}

		public FeeblemindWand( Serial serial ) : base( serial )
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
			Cast( new FeeblemindSpell( from, this ) );
		}
	}
}