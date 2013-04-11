using Server.Engines.VeteranRewards;

namespace Server.Items
{
    public class EvilMageStatue : Item, IRewardItem
	{
        private bool m_IsRewardItem = true;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsRewardItem
        {
            get { return m_IsRewardItem; }
            set { m_IsRewardItem = value; }
        }
		[Constructable]
		public EvilMageStatue() : base( 0x258A )
		{
			Weight = 1.0;
		}

		public EvilMageStatue( Serial serial ) : base( serial ) { }

		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( 0 ); }

		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
}