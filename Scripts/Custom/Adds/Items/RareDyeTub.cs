namespace Server.Items
{
    public class RareDyeTub : DyeTub
    {
        private Mobile m_TubOwner;
        private bool m_HasColor;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile TubOwner
        {
            get { return m_TubOwner; }
            set { m_TubOwner = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool HasColor
        {
            get { return m_HasColor; }
            set
            {
                if (!value)
                {
                    Redyable = true;
                    Hue = DyedHue = 0;
                    Redyable = false;
                }

                m_HasColor = value;
            }
        }
        
        [Constructable]
        public RareDyeTub()
        {
            Hue = DyedHue = 0;
            Redyable = false;
            Name = "Rare dye tub";
            LootType = LootType.Blessed;
        }

        public RareDyeTub(Serial serial)
            : base(serial)
        {
        }

        public override bool AllowRunebooks
        {
            get { return true; }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!HasColor)
            {
                m_TubOwner = from;
                HasColor = true;

                Redyable = true;
                Hue = DyedHue = Utility.RandomList(Sphere.RareHues);
                Redyable = false;
                from.SendMessage("The dye tub magicaly colors itself");
                Name = string.Format("{0}'s rare dye tub [{1}]", from.Name, Hue);
                from.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                from.PlaySound(0x202);
            }
            else //if (from == m_TubOwner)
                base.OnDoubleClick(from);
            //else
            //    from.SendAsciiMessage("This is not your Rare DyeTub!");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version

            // ver 1
            writer.Write(m_HasColor);

            // ver 0
            writer.Write(m_TubOwner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_HasColor = reader.ReadBool();
                        goto case 0;
                    }
                case 0:
                    {
                        m_TubOwner = reader.ReadMobile();
                        break;
                    }
            }

            if (version == 0) //Set all existing tubs to HasColor so they don't get recolored
                m_HasColor = true;
        }
    }
}