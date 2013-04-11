using System;
using System.Collections.Generic;
using System.Text;
using Server.Mobiles;
using Server.Network;
using Server.Custom.Games;

namespace Server.Items
{
    public class CustomGameEventSupplier : EventSupplier
    {
        private Point3D m_TargetLocation = Point3D.Zero;
        private Map m_TargetMap = null;
        private EventType m_EventType = EventType.NoEvent;

        [Constructable]
        public CustomGameEventSupplier()
            : base()
        {

        }

        public CustomGameEventSupplier(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2);

            //2
            writer.Write((byte)m_EventType);

            //1
            writer.Write(m_TargetLocation);
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
                        m_EventType = (EventType)reader.ReadByte();
                        goto case 1;
                    }
                case 1:
                    {
                        m_TargetLocation = reader.ReadPoint3D();
                        m_TargetMap = reader.ReadMap();
                        break;
                    }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Target
        {
            get { return m_TargetLocation; }
            set { m_TargetLocation = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map TargetMap
        {
            get { return m_TargetMap; }
            set { m_TargetMap = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public EventType EventType
        {
            get { return m_EventType; }
            set { m_EventType = value; }
        }

        public bool Teleport(Mobile m)
        {
            if (m_TargetMap != null && m_TargetMap != Map.Internal && m_TargetLocation != Point3D.Zero)
            {
                m.MoveToWorld(m_TargetLocation, m_TargetMap);
                return false;
            }

            return true;
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m.Alive == false)
            {
                m.SendAsciiMessage("You cannot use that while dead.");
                return true;
            }

            if (m.AccessLevel >= AccessLevel.Counselor)
            {
                m.SendAsciiMessage("This is not for staff members.");
                return true;
            }

            if (m is BaseCreature)
                return Teleport(m);
            else if (m is PlayerMobile)
            {
                if (!m.IsInEvent)
                {
                    if (SupplyType == SupplyType.Custom && !CanUseCustomGear)
                    {
                        PublicOverheadMessage(MessageType.Regular, 906, true, "Custom setup error.");
                        return false;
                    }

                    //Store our equipment
                    EquipmentStorage playerGear = new EquipmentStorage(m);
                    playerGear.StoreEquip();

                    //Supply the right type of gear
                    SupplySystem.SupplyGear(m, this);

                    //Tag the mobile to be in the event and display a message
                    
                    m.IsInEvent = true;
                    ((PlayerMobile)m).EventType = m_EventType;
                    m.SendAsciiMessage("You have been auto supplied.");
                    return Teleport(m);
                }
                else
                {
                    m.IsInEvent = false;
                    ((PlayerMobile)m).EventType = EventType.NoEvent;
                    m.SendAsciiMessage("Your auto supply has been removed.");

                    SupplySystem.RemoveEventGear(m);
                    return Teleport(m);
                }
            }
            return true;
        }
    }
}
