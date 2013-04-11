using System;
using Server.Network;
using Server.Regions;

namespace Server.Items
{
    public class Waystone : Item
    {
        private Point3D m_Origin;
        private static TeleportTimer m_TeleportTimer;

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D PointDest { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map MapDest { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Delay { get; set; }

        [Constructable]
        public Waystone() : base(7955)
        {
            Name = "Waystone";
            Hue = 2473;
            LootType = LootType.Blessed;
            Delay = 30;
        }

        public Waystone(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (PointDest == Point3D.Zero && MapDest == null && RootParentEntity is Mobile)
            {
                Mobile m = RootParentEntity as Mobile;
                PointDest = m.Location;
                MapDest = m.Map;
                if (!String.IsNullOrEmpty(m.Region.Name))
                    Name = "Waystone to " + m.Region.Name;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            CustomRegion cR = from.Region as CustomRegion;

            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1060640); // "This item must be in your backpack to use it"
                return;
            }

            if (PointDest == Point3D.Zero || MapDest == null || MapDest == Map.Internal)
            {
                from.SendAsciiMessage("This waystone does not lead anywhere");
                return;
            }

            if (m_TeleportTimer != null && m_TeleportTimer.Running)
            {
                from.SendAsciiMessage("You stop the timer");
                m_TeleportTimer.Stop();
                return;
            }

            if (cR != null && !cR.Controller.CanUseStuckMenu)
            {
                from.SendAsciiMessage("You cannot use a waystone in this region");
                return;
            }

            if (from.Region.IsPartOf(typeof(Jail)))
            {
                from.SendLocalizedMessage(1114345, "", 0x35); // You'll need a better jailbreak plan than that!
                return;
            }

            if (from.Hits < from.HitsMax)
            {
                from.SendAsciiMessage("You must be fully healed to use this");
                return;
            }

            m_Origin = from.Location;
            from.SendAsciiMessage(string.Format("You will be teleported in {0} seconds", Delay));

            m_TeleportTimer = new TeleportTimer(this, from);
            m_TeleportTimer.Start();

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);

            writer.Write(m_Origin);
            writer.Write(PointDest);
            writer.Write(MapDest);
            writer.Write(Delay);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Origin = reader.ReadPoint3D();
                        PointDest = reader.ReadPoint3D();
                        MapDest = reader.ReadMap();
                        Delay = reader.ReadInt();
                        break;
                    }
            }
        }

        public class TeleportTimer : Timer
        {
            private readonly Waystone m_Waystone;
            private readonly Mobile m_Mobile;
            private int m_State;

            public TeleportTimer(Waystone ws, Mobile m) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_Waystone = ws;
                m_State = ws.Delay;
                m_Mobile = m;
            }

            protected override void OnTick()
            {
                m_State--;

                if (m_Waystone.Deleted || m_Waystone == null)
                    Stop();

                else if (!m_Waystone.IsChildOf(m_Mobile.Backpack))
                {
                    m_Mobile.SendAsciiMessage("Canceled, your waystone must be in your backpack to use it");
                    Stop();
                }

                else if (m_Mobile.Hits < m_Mobile.HitsMax)
                {
                    m_Mobile.SendAsciiMessage("Canceled, you must be fully healed to use your waystone");
                    Stop();
                }

                else if (m_Mobile.Location != m_Waystone.m_Origin)
                {
                    m_Mobile.SendAsciiMessage("Canceled, you must stand still to use your waystone");
                    Stop();
                }

                else if (m_State == 0) // Complete
                {
                    Map map = m_Waystone.MapDest;

                    if (map == null || map == Map.Internal)
                        map = m_Mobile.Map;

                    Point3D p = m_Waystone.PointDest;

                    if (p == Point3D.Zero)
                        p = m_Mobile.Location;

                    Effects.SendLocationEffect(m_Mobile.Location, m_Mobile.Map, 0x3728, 10, 10);

                    m_Mobile.MoveToWorld(p, map);
                    m_Mobile.RevealingAction();

                    Effects.SendLocationEffect(m_Mobile.Location, m_Mobile.Map, 0x3728, 10, 10);
                    Effects.PlaySound(m_Mobile.Location, m_Mobile.Map, 0x1FE);

                    m_Waystone.Delete();

                    Stop();
                }
            }
        }
    }
}