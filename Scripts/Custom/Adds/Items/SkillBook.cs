namespace Server.Scripts.Custom.Adds.Items
{
    #region

    using Gumps;
    using Server.Items;

    #endregion

    public class SkillBook : Item
    {
        private const string DEFAULT_NAME = "Skill book";
        public enum SkillBonusType
        {
            Relative,
            Absolute
        }

        private const AccessLevel m_RequiredAccessLevel = AccessLevel.GameMaster;

        private Mobile m_Owner;

        private SkillName m_SkillName;
        private SkillBonusType m_BonusType = SkillBonusType.Relative;
        
        private double m_BonusValue;
        private double m_MaxValue;

        private bool m_RequiresOwner;
        private bool m_CheckBaseSkill = true;
        private bool m_SetOwnerOnPickup;

        [CommandProperty(m_RequiredAccessLevel)]
        public Mobile Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }

        [CommandProperty(m_RequiredAccessLevel)]
        public SkillName SkillName
        {
            get { return m_SkillName; }
            set { m_SkillName = value; }
        }

        [CommandProperty(m_RequiredAccessLevel)]
        public SkillBonusType BonusType
        {
            get { return m_BonusType; }
            set { m_BonusType = value; }
        }

        [CommandProperty(m_RequiredAccessLevel)]
        public double Bonus
        {
            get { return m_BonusValue; }
            set { m_BonusValue = value; }
        }

        [CommandProperty(m_RequiredAccessLevel)]
        public double MaxValue
        {
            get { return m_MaxValue; }
            set { m_MaxValue = value; }
        }

        [CommandProperty(m_RequiredAccessLevel)]
        public bool RequiresOwner
        {
            get { return m_RequiresOwner; }
            set { m_RequiresOwner = value; }
        }

        [CommandProperty(m_RequiredAccessLevel)]
        public bool CheckBaseSkill
        {
            get { return m_CheckBaseSkill; }
            set { m_CheckBaseSkill = value; }
        }

        [CommandProperty(m_RequiredAccessLevel)]
        public bool SetOwnerOnPickup
        {
            get { return m_SetOwnerOnPickup; }
            set { m_SetOwnerOnPickup = value; }
        }

        [Constructable]
        public SkillBook() : this(SkillName.Alchemy, 0, 75)
        {
        }

        [Constructable]
        public SkillBook(SkillName skillName, int bonus, int maxValue)
            : base(0xFF0) //0xFF4 is open
        {
            m_SkillName = skillName;
            m_BonusValue = bonus;
            m_MaxValue = maxValue;

            Name = DEFAULT_NAME;
            LootType = LootType.Blessed;
        }

        public SkillBook(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                if (!Movable)
                    from.SendAsciiMessage("This book is frozen, you cannot use it.");
                else if (!IsChildOf(from.Backpack))
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                else if (GiveSkill(from))
                    Delete();
            }
            else if ((from.AccessLevel >= m_RequiredAccessLevel))
                from.SendGump(new PropertiesGump(from, this));
        }

        private bool GiveSkill(Mobile from)
        {
            if (m_RequiresOwner && m_Owner == null)
                from.SendMessage(string.Format("This book requires an owner, please page for a {0} to set it.", m_RequiredAccessLevel));
            else if (m_RequiresOwner && m_Owner != from)
                from.SendMessage("You are not the owner of this book.");
            else if (m_BonusValue == 0)
                from.SendMessage(string.Format("This book isn't properly set up, please page for a {0}.", m_RequiredAccessLevel));
            else if (from.Skills[m_SkillName].Lock != SkillLock.Up)
                from.SendMessage("Your skill is locked, please remove the lock before you use the book.");
            else if (m_MaxValue <= GetCurrentSkillValue(from) || GetNewSkillValue(from) <= GetCurrentSkillValue(from))
                from.SendMessage(string.Format("You have too high {0} to learn from this book.", m_SkillName));
            else
            {
                double skillValue = GetNewSkillValue(from);

                Skill playerSkill = from.Skills[m_SkillName];
                playerSkill.Base = skillValue;
                playerSkill.Update();

                from.PlaySound(0x202);
                return true;
            }

            return false;
        }

        private double GetCurrentSkillValue(Mobile from)
        {
            return m_CheckBaseSkill ? from.Skills[m_SkillName].Base : from.Skills[m_SkillName].Value;
        }

        private double GetNewSkillValue(Mobile from)
        {
            double newSkillLevel;
            if (m_BonusType == SkillBonusType.Absolute)
                newSkillLevel = m_BonusValue;
            else
            {
                newSkillLevel = GetCurrentSkillValue(from);

                newSkillLevel = newSkillLevel + m_BonusValue;
                if (newSkillLevel > m_MaxValue)
                    newSkillLevel = m_MaxValue;
            }

            return newSkillLevel;
        }

        public override void OnSingleClick(Mobile from)
        {
            if ( Name == DEFAULT_NAME || string.IsNullOrEmpty(Name))
            {
                string ownerString = m_Owner != null ? string.Format("{0}'s ", m_Owner.Name) : string.Empty;
                LabelTo(from, string.Format("{0}{1} Book.", ownerString, m_SkillName));
            }
            else
                base.OnSingleClick(from);
        }

        public override void OnAdded(object parent)
        {
            Mobile owner = null;
            if (m_SetOwnerOnPickup && m_Owner == null)
            {
                if (parent is Mobile)
                    owner = (Mobile) parent;
                else if (parent is Container)
                {
                    Container container = (Container) parent;

                    if (container.Parent is Mobile)
                        owner  = (Mobile) container.Parent;
                    else if (RootParent is Mobile)
                        owner = (Mobile)container.RootParent;
                }
            }

            if (owner != null)
            {
                m_Owner = owner;
                m_SetOwnerOnPickup = false;
            }
            else
                base.OnAdded(parent);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version

            writer.Write(m_Owner);

            writer.Write((int) m_SkillName);
            writer.Write((int) m_BonusType);

            writer.Write(m_BonusValue);
            writer.Write(m_MaxValue);

            writer.Write(m_RequiresOwner);
            writer.Write(m_CheckBaseSkill);
            writer.Write(m_SetOwnerOnPickup);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Owner = reader.ReadMobile();

            m_SkillName = (SkillName) reader.ReadInt();
            m_BonusType = (SkillBonusType) reader.ReadInt();

            m_BonusValue = reader.ReadDouble();
            m_MaxValue = reader.ReadDouble();

            m_RequiresOwner = reader.ReadBool();
            m_CheckBaseSkill = reader.ReadBool();
            m_SetOwnerOnPickup = reader.ReadBool();
        }
    }
}