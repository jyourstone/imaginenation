namespace Server.Items
{
	public class FireballScroll : SpellScroll
	{
        //Loki edit: Mana matched to spell
        public override int ManaCost { get { return 9; } }

		[Constructable]
		public FireballScroll() : this( 1 )
		{
		}

		[Constructable]
		public FireballScroll( int amount ) : base( 17, 0x1F3E, amount )
		{
		}

		public FireballScroll( Serial serial ) : base( serial )
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