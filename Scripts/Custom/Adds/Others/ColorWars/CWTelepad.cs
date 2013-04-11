using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Custom.Games
{
    public class CWTelepad : Item
    {
        private CWGame m_Game;
        private int m_TeamID = -1;
        private bool m_NewbieAllItems = true;
        private Point3D m_TeamHome;
        private Point3D m_TeamHome2;
        private Map m_TeamHomeMap;
        private string m_TeamName;


        [Constructable]
        public CWTelepad()
            : base(0x17e5)
        {
            Movable = false;
            Name = "Color Wars Telepad";
            Light = LightType.Circle300;
        }

        public CWTelepad(Serial serial)
            : base(serial)
        {
        }

        #region Setters & Getters
        [CommandProperty(AccessLevel.GameMaster)]
        public bool NewbieAllItems
        {
            get { return m_NewbieAllItems; }
            set { m_NewbieAllItems = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ClothHue { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int GearHue { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public string TeamName
        {
            get { return m_TeamName; }
            set
            {
                if (m_Game != null && !m_Game.Deleted)
                {
                    if (TeamID != -1)
                    {
                        CWTeam cwteam = (CWTeam)m_Game.Teams[m_TeamID-1];
                        cwteam.Name = value;
                        m_TeamName = value;
                    }
                    else
                        PublicOverheadMessage(MessageType.Regular, 906, true,
                                              "You must enter a team ID first");
                }
                else
                    PublicOverheadMessage(MessageType.Regular, 906, true,
                                      "You must link the telepad to a CW control stone first");

            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public CWGame Game
        {
            get { return m_Game;}
            set
            {
                if (m_Game != value)
                {
                    m_TeamID = -1;
                    m_TeamHome = new Point3D(0,0,0);
                    m_TeamHome2 = new Point3D(0, 0, 0);
                    m_TeamHomeMap = null;
                    m_TeamName = null;
                }
                m_Game = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D TeamHome
        {
            get { return m_TeamHome;}
            set
            {
                if (m_Game != null && !m_Game.Deleted)
                {
                    if (TeamID != -1)
                    {
                        CWTeam cwteam = (CWTeam)m_Game.Teams[m_TeamID-1];
                        cwteam.Home = value;

                        m_TeamHome = value;
                    }
                    else
                        PublicOverheadMessage(MessageType.Regular, 906, true,
                                              "You must enter a team ID first");
                }
                else
                    PublicOverheadMessage(MessageType.Regular, 906, true,
                                      "You must link the telepad to a CW control stone first");
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D TeamHome2
        {
            get { return m_TeamHome2; }
            set
            {
                if (m_Game != null && !m_Game.Deleted)
                {
                    if (TeamID != -1)
                    {
                        CWTeam cwteam = (CWTeam)m_Game.Teams[m_TeamID - 1];
                        cwteam.Home2 = value;

                        m_TeamHome2 = value;
                    }
                    else
                        PublicOverheadMessage(MessageType.Regular, 906, true,
                                              "You must enter a team ID first");
                }
                else
                    PublicOverheadMessage(MessageType.Regular, 906, true,
                                      "You must link the telepad to a CW control stone first");
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map TeamHomeMap
        {
            get { return m_TeamHomeMap;}
            set
            {
                if (m_Game != null && !m_Game.Deleted)
                {
                    if (TeamID != -1)
                    {
                        CWTeam cwteam = (CWTeam) m_Game.Teams[m_TeamID-1];
                        cwteam.HomeMap = value;
                        m_TeamHomeMap = value;
                    }
                    else
                        PublicOverheadMessage(MessageType.Regular, 906, true,
                                              "You must enter a team ID first");
                }
                else
                    PublicOverheadMessage(MessageType.Regular, 906, true,
                                          "You must link the telepad to a CW control stone first");
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TeamID
        {
            get { return m_TeamID;}
            set
            {
                if (m_Game != null && !m_Game.Deleted)
                {
                    if (value > 0 && value <= m_Game.TeamCount)
                    {
                        if (value != m_TeamID && m_TeamID != -1)
                        {
                            m_TeamName = null;
                            m_TeamHome = new Point3D(0,0,0);
                            m_TeamHome2 = new Point3D(0, 0, 0);
                            m_TeamHomeMap = null;
                            m_TeamName = null;
                        }
                        CWTeam cwteam = (CWTeam) m_Game.Teams[value-1];
                        cwteam.Name = TeamName;
                        m_TeamID = value;
                    }
                    else
                        PublicOverheadMessage(MessageType.Regular, 906, true,
                                              "That many teams does not exist in the game");
                }
                else
                    PublicOverheadMessage(MessageType.Regular, 906, true,
                                          "You must link the telepad to a CW control stone first");
            }
        }
        #endregion

        public override bool OnMoveOver(Mobile m)
        {
            if (m.AccessLevel >= AccessLevel.Counselor)
            {
                m.SendAsciiMessage("This is not for staff members.");
                return true;
            }

            if (m is BaseCreature)
                return Teleport(m);
            if (m is PlayerMobile)
            {
                if (!m.Alive)
                {
                    ((PlayerMobile)m).ForceResurrect();
                    return false;
                }

                if (!m.IsInEvent || m_Game != null)
                {
                    if (m_Game != null && m_TeamID != -1)
                    {
                        //Store our equipment
                        EquipmentStorage playerGear = new EquipmentStorage(m);
                        playerGear.StoreEquip();

                        //Supply the right type of gear
                        SupplySystem.SupplyGear(m, this);

                        CWTeam team = (CWTeam)m_Game.Teams[m_TeamID-1];
                        //m_Game.SwitchTeams(m, team);

                        m.SendMessage("You have joined {0}!", team.Name);

                        //Tag the mobile to be in the event and display a message
                        m.IsInEvent = true;
                        m.SendAsciiMessage("You have been auto supplied.");

                        if (m_Game != null && m_Game.Running)
                            return Teleport(m);
                    }
                    else
                    {
                        m.SendMessage("The game has not started yet.");
                        return false;
                    }
                }
                else
                {
                    m.IsInEvent = false;
                    m.SendAsciiMessage("Your auto supply has been removed.");

                    SupplySystem.RemoveEventGear(m);
                    return Teleport(m);
                }
            }
            return true;
        }
        
        public bool Teleport(Mobile m)
        {
            if (m_TeamHomeMap != null && m_TeamHomeMap != Map.Internal)
            {
                if (m_TeamHome != Point3D.Zero && m_TeamHome2 != Point3D.Zero)
                {
                    if (Utility.RandomDouble() > 0.5)
                        m.MoveToWorld(m_TeamHome, m_TeamHomeMap);
                    else
                        m.MoveToWorld(m_TeamHome2, m_TeamHomeMap);

                    return false;
                }

                if (m_TeamHome != Point3D.Zero)
                {
                    m.MoveToWorld(m_TeamHome, m_TeamHomeMap);
                    return false;
                }

                if (m_TeamHome2 != Point3D.Zero)
                {
                    m.MoveToWorld(m_TeamHome2, m_TeamHomeMap);
                    return false;
                }

                return true;
            }
            return true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);

            writer.Write(NewbieAllItems);
            writer.Write(ClothHue);
            writer.Write(GearHue);
            writer.Write(m_TeamName);
            writer.Write(m_Game);
            writer.Write(m_TeamHome);
            writer.Write(m_TeamHome2);
            writer.Write(m_TeamHomeMap);
            writer.Write(m_TeamID);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                {
                    NewbieAllItems = reader.ReadBool();
                    ClothHue = reader.ReadInt();
                    GearHue = reader.ReadInt();
                    m_TeamName = reader.ReadString();
                    m_Game = reader.ReadItem() as CWGame;
                    m_TeamHome = reader.ReadPoint3D();
                    m_TeamHome2 = reader.ReadPoint3D();
                    m_TeamHomeMap = reader.ReadMap();
                    m_TeamID = reader.ReadInt();

                    break;
                }
            }
        }
    }
}
