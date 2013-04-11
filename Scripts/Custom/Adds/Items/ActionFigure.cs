using System;
using System.Collections.Generic;
using Server.Network;

namespace Server.Items
{
    public class ActionFigure : Item
    {
        private int m_Sound1;
        private int m_Sound2;
        private int m_Sound3;
        private int m_Sound4;
        private int m_Sound5;
        private string m_Phrase1;
        private string m_Phrase2;
        private string m_Phrase3;
        private string m_Phrase4;
        private string m_Phrase5;

        [Constructable]
        public ActionFigure() : base(0x25F9)
        {
            Name = "Action Figure";
			Weight = 1.0;
		}

        public ActionFigure(Serial serial) : base(serial)
        {
            m_Sound1 = -1;
            m_Sound2 = -1;
            m_Sound3 = -1;
            m_Sound4 = -1;
            m_Sound5 = -1;
        }

        # region Setters and getters
        [CommandProperty(AccessLevel.GameMaster)]
        public string Phrase1
        {
            get { return m_Phrase1; }
            set
            {
                m_Phrase1 = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Sound1
        {
            get { return m_Sound1; }
            set
            {
                m_Sound1 = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Phrase2
        {
            get { return m_Phrase2; }
            set
            {
                m_Phrase2 = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Sound2
        {
            get { return m_Sound2; }
            set
            {
                m_Sound2 = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Phrase3
        {
            get { return m_Phrase3; }
            set
            {
                m_Phrase3 = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Sound3
        {
            get { return m_Sound3; }
            set
            {
                m_Sound3 = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Phrase4
        {
            get { return m_Phrase4; }
            set
            {
                m_Phrase4 = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Sound4
        {
            get { return m_Sound4; }
            set
            {
                m_Sound4 = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Phrase5
        {
            get { return m_Phrase5; }
            set
            {
                m_Phrase5 = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Sound5
        {
            get { return m_Sound5; }
            set
            {
                m_Sound5 = value;
                InvalidateProperties();
            }
        }
        #endregion

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InLOS(this) && from.InRange(GetWorldLocation(), 5))
            {
                int max = 0;
                if (m_Phrase1 != null || m_Sound1 > 0)
                    max++;
                if (m_Phrase2 != null || m_Sound2 > 0)
                    max++;
                if (m_Phrase3 != null || m_Sound3 > 0)
                    max++;
                if (m_Phrase4 != null || m_Sound4 > 0)
                    max++;
                if (m_Phrase5 != null || m_Sound5 > 0)
                    max++;
                if (max == 0)
                    return;
                int tmpAction = Utility.Random(1, max);

                int sound = 0;
                string phrase = null;

                switch (tmpAction)
                {
                    case 1:
                        if (m_Sound1 > 0)
                            sound = Sound1;
                        if (m_Phrase1 != null)
                            phrase = Phrase1;
                        break;
                    case 2:
                        if (m_Sound2 > 0)
                            sound = Sound2;
                        if (m_Phrase2 != null)
                            phrase = Phrase2;
                        break;
                    case 3:
                        if (m_Sound3 > 0)
                            sound = Sound3;
                        if (m_Phrase3 != null)
                            phrase = Phrase3;
                        break;
                    case 4:
                        if (m_Sound4 > 0)
                            sound = Sound4;
                        if (m_Phrase4 != null)
                            phrase = Phrase4;
                        break;
                    case 5:
                        if (m_Sound5 > 0)
                            sound = Sound5;
                        if (m_Phrase5 != null)
                            phrase = Phrase5;
                        break;
                    default:
                        break;
                }

                if (RootParentEntity == null)
                {
                    Effects.PlaySound(Location, Map, sound);
                    PublicOverheadMessage(MessageType.Regular, Hue, true, phrase);
                }
                else
                {
                    from.PlaySound(sound);
                    PrivateOverheadMessage(MessageType.Regular, Hue, true, phrase, from.NetState);
                }
            }
            else
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.

        }

        public override void Serialize( GenericWriter writer )
		{
		    base.Serialize( writer );

            writer.Write( 0 );
		    writer.Write(m_Phrase1);
            writer.Write(m_Sound1);
            writer.Write(m_Phrase2);
            writer.Write(m_Sound2);
            writer.Write(m_Phrase3);
            writer.Write(m_Sound3);
            writer.Write(m_Phrase4);
            writer.Write(m_Sound4);
            writer.Write(m_Phrase5);
            writer.Write(m_Sound5);
		}

		public override void Deserialize( GenericReader reader )
		{
		    base.Deserialize( reader );
            int version = reader.ReadInt();
		    m_Phrase1 = reader.ReadString();
		    m_Sound1 = reader.ReadInt();
            m_Phrase2 = reader.ReadString();
            m_Sound2 = reader.ReadInt();
            m_Phrase3 = reader.ReadString();
            m_Sound3 = reader.ReadInt();
            m_Phrase4 = reader.ReadString();
            m_Sound4 = reader.ReadInt();
            m_Phrase5 = reader.ReadString();
            m_Sound5 = reader.ReadInt();
        }
    }
}
