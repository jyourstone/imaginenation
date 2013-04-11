using Server.Guilds;
using Server.Multis;
using Server.Network;

namespace Server.Items
{
    [DynamicFliping]
    [Flipable(0x9AB, 0xE7C)]
    public class GuildContainer : MetalChest
    {
        private int m_Guild = -1;

        [Constructable]
        public GuildContainer()
        {
            Name = "Guild container";
            Movable = false;
            LiftOverride = true;
        }

        public GuildContainer(Serial serial)
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
            if (from.InRange(GetWorldLocation(), 6) && from.InLOS(this))
            {
                if (from.AccessLevel > AccessLevel.Player)
                    DisplayTo(from);

                else if (GuildID != -1 && from.Guild != null && !(from.Guild.Disbanded))
                {
                    if (from.Guild.Id == GuildID)
                    {
                        from.SendAsciiMessage("The container opens for you, guildmember");
                        DisplayTo(from);
                        return;
                    }
                }
                from.SendAsciiMessage("Only guildmembers can open this container");
            }
            else
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
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

	public class GuildContainerDeed : Item
	{
		[Constructable]
		public GuildContainerDeed()
		{
		    ItemID = 5360;
		    Name = "a guild container deed";
		}

        public GuildContainerDeed(Serial serial) : base(serial)
		{
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendAsciiMessage("That must be in your pack for you to use it.");
                return;
            }

            Guild guild = from.Guild as Guild;
            if (guild == null || guild.Disbanded)
            {
                from.SendAsciiMessage("You need to be in a guild to place a guild container");
                return;
            }
            
            Mobile leader = guild.Leader;
            if (from != leader)
            {
                from.SendAsciiMessage("You must be the leader of the guild to place a guild container");
                return;
            }

            BaseHouse house = BaseHouse.FindHouseAt(from);
            if (house != null && house.IsOwner(from))
            {
                Delete();

                GuildContainer gc = new GuildContainer();
                gc.MoveToWorld(from.Location, from.Map);
                gc.GuildID = from.Guild.Id;
            }
            else
                from.SendAsciiMessage("You must place this in a house you own");

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