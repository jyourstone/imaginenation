using System;

namespace Server.Items
{
	public class EyesOfNewt : BaseReagent, ICommodity
	{
		[Constructable]
		public EyesOfNewt() : this( 1 )
		{
		}

		[Constructable]
		public EyesOfNewt( int amount ) : base( 0x0F87, amount )
		{
			Name = "Eye of Newt";
		}

		public EyesOfNewt( Serial serial ) : base( serial )
		{
		}

        public override void OnSingleClick(Mobile from)
        {
            {
                if (Amount > 1)
                    LabelTo(from, Amount + " Eyes of Newt");
                else
                    LabelTo(from, "Eye of Newt");
            }
        }

		public override int PotionGroupIndex
		{
			get { return 8; }
		}

		//Total Mana pot
		public override int PotionIndex
		{
			get { return 1; }
		}

        int ICommodity.DescriptionNumber { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

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