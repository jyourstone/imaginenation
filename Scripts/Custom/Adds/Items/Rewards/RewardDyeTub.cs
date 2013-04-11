namespace Server.Items
{
    public class RewardDyeTub : DyeTub
    {
        private Mobile m_Owner;

        public RewardDyeTub(Mobile owner)
        {
            m_Owner = owner;
        }

        public override bool AllowRunebooks // so you can dye runes.
        {
            get { return true; }
        }

        [Constructable]
        public RewardDyeTub()
        {
            LootType = LootType.Blessed;
        }

        public RewardDyeTub(Serial serial)
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
            if (m_Owner != null)
            {
                if (m_Owner.Deleted)
                    from.SendAsciiMessage("This tub seems to have dried up!");
                else if (from.Guild != null && m_Owner.Guild != null)
                {
                    if (from.Guild == m_Owner.Guild)
                        base.OnDoubleClick(from);
                }
                else if (from.Account != m_Owner.Account)
                    from.SendAsciiMessage("This is not your tub!");
                else
                    base.OnDoubleClick(from);
            }
        }

        public override void OnItemLifted(Mobile from, Item item)
        {
            if (m_Owner != null)
                if (from.AccessLevel >= AccessLevel.Counselor || from.Account == m_Owner.Account)
                    base.OnItemLifted(from, item);

            return;
        }

        public override bool OnDragLift(Mobile from)
        {
            if (m_Owner != null)
            {
                if (from.AccessLevel >= AccessLevel.Counselor || from.Account == m_Owner.Account)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);

            writer.Write(m_Owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Owner = reader.ReadMobile();
        }
    }
}