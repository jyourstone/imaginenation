namespace Server.Items
{
	public class HarmScroll : SpellScroll
	{
        public override int ManaCost { get { return 11; } }

		[Constructable]
		public HarmScroll() : this( 1 )
		{
		}

		[Constructable]
		public HarmScroll( int amount ) : base( 11, 0x1F38, amount )
		{
		}

		public HarmScroll( Serial serial ) : base( serial )
		{
		}

        //Maka
        /*public override void OnDoubleClick(Mobile from)
        {
            from.Mana -= 4;

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