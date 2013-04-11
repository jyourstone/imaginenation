namespace Server.Items
{
	public class DragonStatue : Item
	{
		[Constructable]
		public DragonStatue() : base( 0x20D6 )
		{
			Weight = 1.0;
            Name = "Dragon";
		}

		public DragonStatue( Serial serial ) : base( serial ) { }

		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( 0 ); }

		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
}