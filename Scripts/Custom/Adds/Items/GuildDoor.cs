using Server.Network;

namespace Server.Items
{
    [Flipable(0x6C5, 0x6C6, 0x6C7, 0x6C8, 0x6C9, 0x6CA, 0x6CB, 0x6CC, 0x6CD, 0x6CE, 0x6CF, 0x6D0, 0x6D1, 0x6D2, 0x6D3, 0x6D4)]
    public class GuildDoor : Item
    {
        private int m_Guild = -1;

        [Constructable]
        public GuildDoor() : base(0x6C5)
        {
            Name = "Guild door";
            Movable = false;
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
            if (from.InRange(GetWorldLocation(), 2) && from.InLOS(this))
            {
                if (GuildID != -1 && from.Guild != null && !(from.Guild.Disbanded))
                {
                    if (from.Guild.Id == GuildID)
                    {
                        from.MoveToWorld(this.Location, this.Map);
                        Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                        from.PlaySound(510);
                        return;
                    }
                }
                else
                from.SendAsciiMessage("You cannot use that!");
            }
            else
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
        }

        public GuildDoor(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer) // Default Serialize method
        {
            base.Serialize(writer);

            writer.Write(0); // version
            writer.WriteEncodedInt(m_Guild);
        }

        public override void Deserialize(GenericReader reader) // Default Deserialize method
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Guild = reader.ReadEncodedInt();
        }
    }
}