using Server.Spells.First;

namespace Server.Items
{
	public class ClumsyWand : BaseWand
	{
		[Constructable]
		public ClumsyWand() : base( 17 )
		{
            ItemID = 0xdf2;
            Name = "Clumsy";
		}

		public ClumsyWand( Serial serial ) : base( serial )
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
			Cast( new ClumsySpell( from, this ) );
		}
	}
}