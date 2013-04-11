using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class RaresVendor : BaseVendor
	{
		private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
		protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public RaresVendor() : base( "the custom provisioner" )
		{
            Item temp;
            temp = new Shoes();
            temp.Hue = 1953;
            temp.Movable = false;
            AddItem(temp);
            temp = new LongPants();
            temp.Hue = 01;
            temp.Movable = false;
            AddItem(temp);
            temp = new Doublet();
            temp.Hue = 1953;
            temp.Movable = false;
            AddItem(temp);
            temp = new FancyShirt();
            temp.Hue = 01;
            temp.Movable = false;
            AddItem(temp);
            temp = new FloppyHat();
            temp.Hue = 1953;
            temp.Movable = false;
            AddItem(temp);
            temp = new Cloak();
            temp.Hue = 1953;
            temp.Movable = false;
            AddItem(temp);
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBRaresVendor() );

			//if ( IsTokunoVendor )
			//	m_SBInfos.Add( new SBSEHats() );
		}

		public RaresVendor( Serial serial ) : base( serial )
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