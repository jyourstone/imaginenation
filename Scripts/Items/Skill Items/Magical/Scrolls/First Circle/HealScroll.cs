namespace Server.Items
{
	public class HealScroll : SpellScroll
	{
        public override int ManaCost { get { return 4; } } //Loki edit

		[Constructable]
		public HealScroll() : this( 1 )
		{
		}

		[Constructable]
		public HealScroll( int amount ) : base( 3, 0x1F31, amount )
		{
		}

		public HealScroll( Serial serial ) : base( serial )
		{
		}

        //Maka
        /*public override void OnDoubleClick(Mobile from)
        {
            from.Mana -= 2;

            base.OnDoubleClick(from);
        }*/

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