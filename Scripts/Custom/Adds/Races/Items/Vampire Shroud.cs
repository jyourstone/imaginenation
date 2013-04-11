namespace Server.Items
{
	[Flipable( 0x2684, 0x2683 )]
	public class VampireShroud : BaseOuterTorso
	{
		private Mobile m_Owner;
		private bool m_OwnerOnly;

		[Constructable]
		public VampireShroud() : this( 0x455 )
		{
		}

		[Constructable]
		public VampireShroud( int hue ) : base( 0x2684, hue )
		{
			Name = "Vampire Robe";
			LootType = LootType.Blessed;
			Weight = 0.0;
		}

		public VampireShroud( Serial serial ) : base( serial )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public new Mobile Owner
		{
			get { return m_Owner; }
			set
			{
				m_Owner = value;
				InvalidateProperties();
			}
		}

		public override bool DisplayLootType
		{
			get { return false; }
		}
        
		public override bool OnEquip( Mobile from )
		{
			if( from == Owner )
				return base.OnEquip( from );

            from.SendAsciiMessage("This is not your clothing!");
		    return false;
		}
        
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version

			writer.Write( m_Owner );
			writer.Write( m_OwnerOnly );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Owner = reader.ReadMobile();
			m_OwnerOnly = reader.ReadBool();
		}
	}
}