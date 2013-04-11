namespace Server.Items
{
	public class GMSmithHammer : SmithHammer
	{
		private Mobile m_Owner;
		private bool UseSkillMod = false;
		private SkillMod m_BSMod;

		public GMSmithHammer( Mobile owner )
		{
			m_Owner = owner;
		}

		[Constructable]
		public GMSmithHammer()
		{
		}

		public GMSmithHammer( Serial serial ) : base( serial )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get { return m_Owner; }
			set
			{
				m_Owner = value;
				InvalidateProperties();
			}
		}

		public override bool CanEquip( Mobile m )
		{
			if( m == m_Owner )
				return true;

			return false;
		}

		public override void OnDoubleClick( Mobile m )
		{
			if( m != m_Owner )
				return;
            /*
            if (CanEquip(m))
                MoveToWorld(m.Location, m.Map); //Taran: This fixes the issue where the skillmod is added
                                                //       and then instantly removed. I have no idea why.
            */
            base.OnDoubleClick(m);
		}

		public override void OnSingleClick( Mobile m )
		{
            if (m != m_Owner)
                LabelTo(m, "{0}", Name);  //Shade: Fix for showing the name of hammer to other players
            else
			    base.OnSingleClick( m );
		}

		public override bool OnEquip( Mobile from )
		{
			if( UseSkillMod )
			{
                if (m_BSMod != null)
                    m_BSMod.Remove();

			    m_BSMod = new DefaultSkillMod( SkillName.Blacksmith, true, 20 );
                    from.AddSkillMod( m_BSMod );
			}
			return base.OnEquip( from );
		}

		public override void OnRemoved( object parent )
		{
			if( UseSkillMod && m_BSMod != null )
			{
				m_BSMod.Remove();
				m_BSMod = null;
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 );

			writer.Write( m_Owner );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Owner = reader.ReadMobile();
		}
	}
}