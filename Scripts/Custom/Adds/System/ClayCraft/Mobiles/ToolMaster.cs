using System.Collections;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class ToolMaster : BaseVendor
	{
        public override bool ClickTitle { get { return true; } }
		private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
		protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public ToolMaster() : base( "the clay crafter" )
		{
            Title = "the clay crafter";
			SetSkill( SkillName.ItemID, 64.0, 100.0 );
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBToolMaster () );
		}

		public ToolMaster( Serial serial ) : base( serial )
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
