using System;
using Server.Multis;
using Server.Prompts;
using Server.Regions;

namespace Server.Items
{
    [Flipable(0x1f14, 0x1f15, 0x1f16, 0x1f17)]
    public class RecallRune : Item
    {
        private string m_Description;
        private bool m_Marked;
        private Point3D m_Target;
        private Map m_TargetMap;
        private BaseHouse m_House;
        private int m_ChargesLeft = 0;
        private const string RuneFormat = "Rune to: {0}";
        private const string FadedRuneFormat = "Faded Rune to: {0}";

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public BaseHouse House
        {
            get
            {
                if (m_House != null && m_House.Deleted)
                    House = null;

                return m_House;
            }
            set { m_House = value; CalculateHue(); InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public string Description
        {
            get
            {
                return m_Description;
            }
            set
            {
                m_Description = value;
                InvalidateProperties();
            }
        }


        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public int ChargesLeft
        {
            get
            {
                return m_ChargesLeft;
            }
            set
            {
                m_ChargesLeft = value;
            }
        }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public bool Marked
        {
            get
            {
                return m_Marked;
            }
            set
            {
                if (m_Marked != value)
                {
                    m_Marked = value;
                    CalculateHue();
                    InvalidateProperties();
                }
            }
        }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public Point3D Target
        {
            get
            {
                return m_Target;
            }
            set
            {
                m_Target = value;
            }
        }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public Map TargetMap
        {
            get
            {
                return m_TargetMap;
            }
            set
            {
                if (m_TargetMap != value)
                {
                    m_TargetMap = value;
                    CalculateHue();
                    InvalidateProperties();
                }
            }
        }

        public virtual void Mark(Mobile m)
        {
            m_Marked = true;
            LootType = LootType.Regular;
            m_House = null;
            m_Target = m.Location;
            m_TargetMap = m.Map;

            m_Description = BaseRegion.GetRuneNameFor(Region.Find(m_Target, m_TargetMap));
            m_ChargesLeft = (int)m.Skills.Magery.Value;

            //CalculateHue();
            InvalidateProperties();
        }

        private void CalculateHue()
        {
            Hue = Hue;
            /*if (!m_Marked)
                Hue = 0;
            else if (m_TargetMap == Map.Trammel)
                Hue = (House != null ? 0x47F : 50);
            else if (m_TargetMap == Map.Felucca)
                Hue = (House != null ? 0x66D : 0);
            else if (m_TargetMap == Map.Ilshenar)
                Hue = (House != null ? 0x55F : 1102);
            else if (m_TargetMap == Map.Malas)
                Hue = (House != null ? 0x55F : 1102);
            else if (m_TargetMap == Map.Tokuno)
                Hue = (House != null ? 0x47F : 1154);*/
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_Marked)
            {
                string desc;

                if ((desc = m_Description) == null || (desc = desc.Trim()).Length == 0)
                    desc = "an unknown location";

                if (m_TargetMap == Map.Tokuno)
                    list.Add((House != null ? 1063260 : 1063259), RuneFormat, desc); // ~1_val~ (Tokuno Islands)[(House)]
                else if (m_TargetMap == Map.Malas)
                    list.Add((House != null ? 1062454 : 1060804), RuneFormat, desc); // ~1_val~ (Malas)[(House)]
                else if (m_TargetMap == Map.Felucca)
                    list.Add((House != null ? 1062452 : 1060805), RuneFormat, desc); // ~1_val~ (Felucca)[(House)]
                else if (m_TargetMap == Map.Trammel)
                    list.Add((House != null ? 1062453 : 1060806), RuneFormat, desc); // ~1_val~ (Trammel)[(House)]
                else
                    list.Add((House != null ? "{0} ({1})(House)" : "{0} ({1})"), String.Format(RuneFormat, desc), m_TargetMap);
            }
        }

        public override void OnSingleClick(Mobile from)
        {
            if (m_Marked)
            {
                string desc;

                if ((desc = m_Description) == null || (desc = desc.Trim()).Length == 0)
                    desc = "unknown location";

                if (m_ChargesLeft == 0)
                    LabelTo(from, string.Format(FadedRuneFormat, desc));
                else if (m_ChargesLeft == 0)
                    LabelTo(from, string.Format(RuneFormat, desc));
                else
                    LabelTo(from, string.Format(RuneFormat, desc));
            }
            else
                LabelTo(from, "a blank recall rune");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            else if (m_Marked)
            {
                from.SendAsciiMessage("What is the new name of the rune?");

                from.Prompt = new RenamePrompt(this);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            if (m_House != null && !m_House.Deleted)
            {
                writer.Write(1); // version

                writer.Write(m_House);
            }
            else
            {
                writer.Write(2); // version
            }

            //Version 2
            writer.Write(m_ChargesLeft);

            writer.Write(m_Description);
            writer.Write(m_Marked);
            writer.Write(m_Target);
            writer.Write(m_TargetMap);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        m_ChargesLeft = reader.ReadInt();
                        goto case 0;
                    }
                case 1:
                    {
                        m_House = reader.ReadItem() as BaseHouse;
                        goto case 0;
                    }
                case 0:
                    {
                        m_Description = reader.ReadString();
                        m_Marked = reader.ReadBool();
                        m_Target = reader.ReadPoint3D();
                        m_TargetMap = reader.ReadMap();

                        CalculateHue();

                        break;
                    }
            }
        }

        private class RenamePrompt : Prompt
        {
            private readonly RecallRune m_Rune;

            public RenamePrompt(RecallRune rune)
            {
                m_Rune = rune;
            }

            public override void OnResponse(Mobile from, string text)
            {
                if (m_Rune.House == null && m_Rune.Marked)
                {
                    m_Rune.Description = text;
                    //Iza - "rune to:" to "Rune to"
                    from.SendAsciiMessage(string.Format("Rune renamed: Rune to {0}.",text));
                }
            }
        }

        [Constructable]
        public RecallRune()
            : base(0x1F14)
        {
            Weight = 1.0;
            CalculateHue();
        }

        public RecallRune(Serial serial) : base(serial)
        {
        }
    }
}