using System.Collections;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class CarpetWeaver : BaseVendor
	{
		private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public CarpetWeaver() : base( "the carpet weaver" )
		{
			SetSkill( SkillName.Tailoring, 64.0, 100.0 );
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBCarpetWeaver() );

		}

		public override VendorShoeType ShoeType { get { return VendorShoeType.Sandals; } }

		public override void InitOutfit()
		{
			base.InitOutfit();

			AddItem( new Server.Items.Robe( Utility.RandomBlueHue() ) );
			AddItem( new Server.Items.FloppyHat( Utility.RandomBlueHue() ) );
			
		}

		public CarpetWeaver( Serial serial ) : base( serial )
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
