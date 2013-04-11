using Server.Spells.First;

namespace Server.Items
{
    public class WeakenWand : BaseWand
	{
		[Constructable]
		public WeakenWand() : base( 17 )
		{
            ItemID = 0xdf5;
            Name = "Weaken";
		}

		public WeakenWand( Serial serial ) : base( serial )
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
			Cast( new WeakenSpell( from, this ) );
		}
	}
}