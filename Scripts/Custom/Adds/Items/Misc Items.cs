using Server.Gumps;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Items
{
    [DynamicFliping]
    [Flipable(0x9AB, 0xE7C)]
    public class Bank : Item
    {
        private int m_Guild = -1;

        [Constructable]
        public Bank() : base(0x9AB)
        {
            Name = "Bank box";
            Movable = false;
        }

        public Bank(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int GuildID
        {
            get { return m_Guild; }
            set
            {
                m_Guild = value;
                InvalidateProperties();
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            /*Erzse - 5 tiles away instead of two. */
            if (from.AccessLevel == AccessLevel.Player && (!from.InLOS(this) || !from.InRange(GetWorldLocation(), 5)))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                return;
            }

            #region Erzse - Guild Check
            if (GuildID != -1 && from.Guild != null)
                if (from.Guild.Id != GuildID)
                {
                    from.SendMessage("This chest is specially locked by its guild.");
                    return;
                }
                else
                    from.BankBox.Open();
            else
                from.BankBox.Open();
            #endregion
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 

            writer.WriteEncodedInt(m_Guild);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Guild = reader.ReadEncodedInt();
        }
    }

	public class KarmaBong : RedRibbedFlask
	{
		[Constructable]
		public KarmaBong()
		{
			Name = "Karma device";
		}

		public KarmaBong( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( from.InRange( GetWorldLocation(), 5 ) && from.InLOS(this) )
			{
				from.PlaySound( 32 );
				from.SendMessage( "Your fame is: {0}", from.Fame );
				from.SendMessage( "Your karma is: {0}", from.Karma );
				from.SendMessage( "And your killcount is: {0}", from.Kills );
			}
			else
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
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

    public class TeleportDevice : Item
    {
        private Mobile m_Owner;

        [Constructable]
        public TeleportDevice()
        {
            ItemID = 7955;
            Name = "Teleport device";
            Hue = 1175;
            Movable = true;
            LootType = LootType.Blessed;
        }

        public TeleportDevice(Serial serial)
            : base(serial)
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

        public override void OnDoubleClick(Mobile from)
        {
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}

            else if (from != m_Owner)
            {
                from.SendAsciiMessage("You are not the owner of this");
            }

            else
            {
                from.Target = new InternalTarget(from);
            }
        }

        private class InternalTarget : Target
        {
            public InternalTarget(Mobile owner) : base(12, true, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object o)
            {
                IPoint3D p = o as IPoint3D;
                Mobile mobileTarget = o as Mobile;

                IPoint3D orig = p;
                Map map = from.Map;

                SpellHelper.GetSurfaceTop(ref p);

                if (o != null && from.InRange(new Point2D(p.X, p.Y), 14))
                {
                    Mobile m = from;

                    Point3D to = new Point3D(p);

                    m.Location = to;
                    m.ProcessDelta();

                }
                else
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
            }

            protected override void OnTargetFinish(Mobile from)
            {
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version 

            //ver 1
            writer.Write(m_Owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    m_Owner = reader.ReadMobile();
                    goto case 0;
                case 0:
                    break;
            }
        }
    }

    public class DeathShroudOfHidden : BaseSuit
    {
        private Mobile m_Owner;

        [Constructable]
        public DeathShroudOfHidden() : base(AccessLevel.Player, 0x0, 0x204E)
        {
            Weight = 0;
            LootType = LootType.Blessed;
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

        public override void OnDoubleClick(Mobile from)
        {
            if (from != m_Owner)
            {
                from.SendAsciiMessage("You are not the owner of this");
            }

            else if (from.Hidden)
            {
                from.Hidden = false;
                from.AllowedStealthSteps = 0;
            }
            else
            {
                from.Hidden = true;
                from.AllowedStealthSteps = 99999;
            }
        }

        public override bool OnEquip(Mobile from)
        {
            if (from != m_Owner)
            {
                from.SendAsciiMessage("You are not the owner of this");
                return false;
            }
            else
                return base.OnEquip(from);
        }

        public DeathShroudOfHidden(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version 

            //ver 1
            writer.Write(m_Owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    m_Owner = reader.ReadMobile();
                    goto case 0;
                case 0:
                    break;
            }
        }
    }

    public class WhoDevice : Item
    {
        private Mobile m_Owner;

        [Constructable]
        public WhoDevice()
        {
            ItemID = 7955;
            Name = "Gem of insight";
            Hue = 1172;
            Movable = true;
            LootType = LootType.Blessed;
        }

        public WhoDevice(Serial serial)
            : base(serial)
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

        public override void OnDoubleClick(Mobile from)
        {
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.

            else if (from != m_Owner)
                from.SendAsciiMessage("You are not the owner of this");

            else
                from.SendGump(new WhoGump(from, ""));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version 

            //ver 1
            writer.Write(m_Owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    m_Owner = reader.ReadMobile();
                    goto case 0;
                case 0:
                    break;
            }
        }
    }


}