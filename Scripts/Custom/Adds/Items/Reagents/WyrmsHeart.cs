using System;

namespace Server.Items
{
	public class WyrmsHeart : BaseReagent, ICommodity
	{
        int ICommodity.DescriptionNumber { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

        //Misc pots
        public override int PotionGroupIndex
        {
            get { return 8; }
        }

        //Shrinks
        public override int PotionIndex
        {
            get { return 3; }
        }

		[Constructable]
		public WyrmsHeart() : this( 1 )
		{
		}

		[Constructable]
		public WyrmsHeart( int amount ) : base( 0xF91, amount )
		{
			Name = "Wyrm's Heart";
		}

		public WyrmsHeart( Serial serial ) : base( serial )
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