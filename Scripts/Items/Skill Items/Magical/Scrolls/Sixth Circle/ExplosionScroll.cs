namespace Server.Items
{
	public class ExplosionScroll : SpellScroll
	{
		[Constructable]
		public ExplosionScroll() : this( 1 )
		{
		}

		[Constructable]
		public ExplosionScroll( int amount ) : base( 42, 0x1F57, amount )
		{
		}

		public ExplosionScroll( Serial serial ) : base( serial )
		{
		}

        public override int ManaCost { get { return 15; } }

        /*public override void OnDoubleClick(Mobile from)
        {
            if (!Sphere.CanUse(from, this))
                return;

            if (from.Hits < 16)
                from.Kill();
            else
            {
                from.Hits -= 15;
                from.PlaySound(from.GetHurtSound());

                if (!from.Mounted)
                    from.Animate(20, 5, 1, true, false, 0);
                else
                    from.Animate(29, 5, 1, true, false, 0);

                base.OnDoubleClick(from);
            }
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