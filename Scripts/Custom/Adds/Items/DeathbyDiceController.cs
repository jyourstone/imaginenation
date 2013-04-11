using System;
using System.Collections.Generic;
using Server.Network;

namespace Server.Items
{
    [DynamicFliping]
    [Flipable(0xEDE, 0xEDD)]
    public class DeathbyDiceController : Item
    {
        private int m_Boxes = 3;
        private int m_Count = 11;
        private Point3D m_FirstNW;
        private Point3D m_FirstSE;
        private Point3D m_FourthNW;
        private Point3D m_FourthSE;
        private Map m_MapDest;
        private bool m_Running;
        private Point3D m_SecondNW;
        private Point3D m_SecondSE;
        private Point3D m_ThirdNW;
        private Point3D m_ThirdSE;

        [Constructable]
        public DeathbyDiceController() : base(0x1174)
        {
            Name = "Death by Dice Controller";
            Movable = false;
            Hue = 1171;
        }

        public DeathbyDiceController(Serial serial)
            : base(serial)
        {
        }

        #region Setters and Getters

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D FirstNW
        {
            get { return m_FirstNW; }
            set
            {
                m_FirstNW = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D FirstSE
        {
            get { return m_FirstSE; }
            set
            {
                m_FirstSE = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D SecondNW
        {
            get { return m_SecondNW; }
            set
            {
                m_SecondNW = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D SecondSE
        {
            get { return m_SecondSE; }
            set
            {
                m_SecondSE = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D ThirdNW
        {
            get { return m_ThirdNW; }
            set
            {
                m_ThirdNW = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D ThirdSE
        {
            get { return m_ThirdSE; }
            set
            {
                m_ThirdSE = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D FourthNW
        {
            get { return m_FourthNW; }
            set
            {
                m_FourthNW = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D FourthSE
        {
            get { return m_FourthSE; }
            set
            {
                m_FourthSE = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map MapDest
        {
            get { return m_MapDest; }
            set
            {
                m_MapDest = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Count
        {
            get { return m_Count; }
            set
            {
                m_Count = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Boxes
        {
            get { return m_Boxes; }
            set
            {
                m_Boxes = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Running
        {
            get { return m_Running; }
        }

        #endregion

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel > AccessLevel.Player)
            {
                if (m_MapDest != null)
                {
                    if (!m_Running)
                    {
                        from.PlaySound(545);
                        new InternalTimer(from, this, m_Count, m_Boxes).Start();
                        m_Running = true;
                    }
                    else
                    {
                        from.SendAsciiMessage("You stop the timer!");
                        m_Running = false;
                    }
                }
                else
                    from.SendAsciiMessage("You have not selected a map!");
            }
            else
                from.SendAsciiMessage("Only staff members can use this!");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 

            writer.Write(m_FirstNW);
            writer.Write(m_FirstSE);
            writer.Write(m_SecondNW);
            writer.Write(m_SecondSE);
            writer.Write(m_ThirdNW);
            writer.Write(m_ThirdSE);
            writer.Write(m_FourthNW);
            writer.Write(m_FourthSE);
            writer.Write(m_MapDest);
            writer.Write(m_Count);
            writer.Write(m_Running);
            writer.Write(m_Boxes);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_FirstNW = reader.ReadPoint3D();
            m_FirstSE = reader.ReadPoint3D();
            m_SecondNW = reader.ReadPoint3D();
            m_SecondSE = reader.ReadPoint3D();
            m_ThirdNW = reader.ReadPoint3D();
            m_ThirdSE = reader.ReadPoint3D();
            m_FourthNW = reader.ReadPoint3D();
            m_FourthSE = reader.ReadPoint3D();
            m_MapDest = reader.ReadMap();
            m_Count = reader.ReadInt();
            m_Running = reader.ReadBool();
            m_Boxes = reader.ReadInt();
        }

        #region Nested type: InternalTimer

        private class InternalTimer : Timer
        {
            private readonly int m_Boxes;
            private readonly DeathbyDiceController m_Item;
            private int m_Count;
            private readonly Mobile m_From;

            public InternalTimer(Mobile from, DeathbyDiceController item, int count, int boxes)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_From = from;
                m_Item = item;
                m_Count = count;
                m_Boxes = boxes;
            }

            protected override void OnTick()
            {
                m_Count--;

                if (m_Count >= 1)
                {
                    if (m_Item.m_Running)
                        m_Item.PublicOverheadMessage(MessageType.Regular, 906, true, (m_Count).ToString());
                    else
                        Stop();
                }

                if (m_Count == 0) // Complete
                {
                    Rectangle2D rect1 = new Rectangle2D(m_Item.m_FirstNW.X, m_Item.m_FirstNW.Y,
                                                        m_Item.m_FirstSE.X - m_Item.m_FirstNW.X + 1,
                                                        m_Item.m_FirstSE.Y - m_Item.m_FirstNW.Y + 1);
                    Rectangle2D rect2 = new Rectangle2D(m_Item.m_SecondNW.X, m_Item.m_SecondNW.Y,
                                                        m_Item.m_SecondSE.X - m_Item.m_SecondNW.X + 1,
                                                        m_Item.m_SecondSE.Y - m_Item.m_SecondNW.Y + 1);
                    Rectangle2D rect3 = new Rectangle2D(m_Item.m_ThirdNW.X, m_Item.m_ThirdNW.Y,
                                                        m_Item.m_ThirdSE.X - m_Item.m_ThirdNW.X + 1,
                                                        m_Item.m_ThirdSE.Y - m_Item.m_ThirdNW.Y + 1);
                    Rectangle2D rect4 = new Rectangle2D(m_Item.m_FourthNW.X, m_Item.m_FourthNW.Y,
                                                        m_Item.m_FourthSE.X - m_Item.m_FourthNW.X + 1,
                                                        m_Item.m_FourthSE.Y - m_Item.m_FourthNW.Y + 1);

                    #region Amount of boxes

                    int boxtodie;

                    switch (m_Boxes)
                    {
                        case 1:
                            boxtodie = 1;
                            break;
                        case 2:
                            boxtodie = Utility.Random(1, 2);
                            break;
                        case 3:
                            boxtodie = Utility.Random(1, 3);
                            break;
                        case 4:
                            boxtodie = Utility.Random(1, 4);
                            break;
                        default:
                            boxtodie = Utility.Random(1, 4);
                            break;
                    }

                    #endregion

                    #region Box to die

                    Rectangle2D rect;

                    switch (boxtodie)
                    {
                        case 1:
                            rect = rect1;
                            break;
                        case 2:
                            rect = rect2;
                            break;
                        case 3:
                            rect = rect3;
                            break;
                        case 4:
                            rect = rect4;
                            break;
                        default:
                            rect = rect1;
                            break;
                    }

                    #endregion

                    List<Mobile> toKill = new List<Mobile>();
                    IPooledEnumerable eable = m_Item.m_MapDest.GetMobilesInBounds(rect);

                    foreach (Mobile m in eable)
                    {
                        toKill.Add(m);
                    }

                    eable.Free();

                    if (toKill.Count == 0)
                    {
                        m_From.PlaySound(1050);
                        m_Item.PublicOverheadMessage(MessageType.Regular, 906, true, string.Format("Noone was killed!"));
                        m_Item.m_Running = false;
                        return;
                    }

                    #region Deathtype

                    int deathtype = Utility.Random(5);
                    int deathsoundmale = Utility.RandomList(1059, 1060, 1061, 1063, 346, 347, 348, 349);
                    int deathsoundfemale = Utility.RandomList(788, 789, 790, 791, 336, 337, 338, 339);

                    try
                    {
                        foreach (Mobile m in toKill)
                        {
                            if (m == null || !m.Alive)
                                continue;

                            //Add effects here
                            switch (deathtype)
                            {
                                case 0: //Flamestrike death
                                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                                    m.PlaySound(0x208);
                                    break;

                                case 1: //Explosion death
                                    m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                                    m.PlaySound(519);
                                    break;

                                case 2: //Electrocution
                                    m.FixedParticles(14276, 40, 50, 5044, EffectLayer.Head);
                                    m.PlaySound(756);
                                    new KillTimer(m).Start();
                                    break;

                                case 3: //Saw trap
                                    m.FixedParticles(0x11AD, 40, 30, 5044, EffectLayer.CenterFeet);
                                    m.PlaySound(0x21C);
                                    break;

                                case 4: //Lightning
                                    m.BoltEffect(0);
                                    break;
                            }

                            if (deathtype != 2)
                            {
                                if (m.CanBeDamaged())
                                {
                                    if (m.BodyValue == 0x190)
                                        m.PlaySound(deathsoundmale);
                                    else if (m.BodyValue == 0x191)
                                        m.PlaySound(deathsoundfemale);
                                }

                                m.Kill();
                            }
                        }
                    }
                    catch
                    {
                        m_Item.PublicOverheadMessage(MessageType.Regular, 906, true, string.Format("An error occured"));
                    }

                    #endregion

                    toKill.Clear();
                    m_Item.m_Running = false;
                }
            }
        }

        #endregion

        #region Nested type: KillTimer

        private class KillTimer : Timer
        {
            private readonly Mobile m_Mobile;

            public KillTimer(Mobile m) : base(TimeSpan.FromSeconds(3))
            {
                m_Mobile = m;
            }

            protected override void OnTick()
            {
                if (m_Mobile != null && m_Mobile.Alive)
                {
                    int deathsoundmale = Utility.RandomList(1059, 1060, 1061, 1063, 346, 347, 348, 349);
                    int deathsoundfemale = Utility.RandomList(788, 789, 790, 791, 336, 337, 338, 339);

                    if (m_Mobile.CanBeDamaged())
                    {
                        if (m_Mobile.BodyValue == 0x190)
                            m_Mobile.PlaySound(deathsoundmale);
                        else if (m_Mobile.BodyValue == 0x190)
                            m_Mobile.PlaySound(deathsoundfemale);
                    }

                    m_Mobile.Kill();
                }
            }
        }

        #endregion
    }
}