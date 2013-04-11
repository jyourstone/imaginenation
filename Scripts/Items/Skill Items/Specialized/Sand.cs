using System;

namespace Server.Items
{
	[Flipable( 0x11EA, 0x11EB )]
	public class Sand : Item, ICommodity
	{
        int ICommodity.DescriptionNumber { get { return LabelNumber; } }

		public override int LabelNumber{ get{ return 1044626; } } // sand
        bool ICommodity.IsDeedable { get { return true; } }

		[Constructable]
		public Sand() : this( 1 )
		{
		}

		[Constructable]
		public Sand( int amount ) : base( 0x11EA )
		{
			Stackable = true;
			Weight = 1.0;
		    Amount = amount;
		}

		public Sand( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version == 0 && Name == "sand" )
				Name = null;
		}
	}
}