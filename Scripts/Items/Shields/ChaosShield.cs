using Server.Guilds;

namespace Server.Items
{
	public class ChaosShield : BaseShield, IDyable
	{
        private Mobile m_Owner;
		public override int BasePhysicalResistance{ get{ return 1; } }
		public override int BaseFireResistance{ get{ return 0; } }
		public override int BaseColdResistance{ get{ return 0; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 100; } }
		public override int InitMaxHits{ get{ return 125; } }

		public override int AosStrReq{ get{ return 95; } }

		//public override int ArmorBase{ get{ return 30; } }

		[Constructable]
		public ChaosShield() : base( 0x1BC3 )
		{
			if ( !Core.AOS )
				LootType = LootType.Newbied;

		    Durability = ArmorDurabilityLevel.Indestructible;
			Weight = 5.0;
            BaseArmorRating = 24;
		}

		public ChaosShield( Serial serial ) : base(serial)
		{
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get { return m_Owner; }
            set
            {
                m_Owner = value;
                InvalidateProperties();
            }
        }

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (BaseArmorRating == 28)
                BaseArmorRating = 24;

            switch (version)
            {
                case 1:
                    m_Owner = reader.ReadMobile();
                    goto case 0;
                case 0:
                    break;
            }
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 1 );//version

            //ver 1
            writer.Write(m_Owner);
		}

        public override bool OnEquip(Mobile from)
        {
            if (from.AccessLevel > AccessLevel.Player)
                return true;

            if (from.Guild == null || from.Guild.Disbanded || from.Guild.Type == GuildType.Regular)
            {
                from.SendAsciiMessage("You must be in a chaos or order guild to use this");
                return false;
            }

            if (from != m_Owner)
            {
                if (from.Fame < 10000)
                {
                    from.SendAsciiMessage("Since you are not the owner of this shield, you must get 10k fame to be able to equip it for the first time and become the new owner.");
                    return false;
                }
                from.SendAsciiMessage("You are now the owner of this shield");
                m_Owner = from;
            }

            return true;
        }

        public override void OnSingleClick( Mobile from )
        {
            base.OnSingleClick(from);
        }

        public bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;

            Hue = sender.DyedHue;
            return true;
        }
	}
}