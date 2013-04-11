using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public class Dice1to3 : Item
    {
        private static readonly Hashtable m_Table = new Hashtable();

        [CommandProperty(AccessLevel.Counselor)]
        public override bool EventItem
        {
            get { return base.EventItem; }
            set { base.EventItem = value; }
        }

        [Constructable]
        public Dice1to3()
            : base(0xFA7)
        {
            Weight = 1.0;
        }

        public Dice1to3(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(GetWorldLocation(), 2))
            {
                if (!m_Table.Contains(from))
                {
                    PublicOverheadMessage(MessageType.Emote, 34, false,
                                          string.Format("*{0} rolls a {1}*", from.Name, Utility.Random(1, 3)));
                    Timer t = new DelayTimer(from);
                    m_Table[from] = t;
                    t.Start();
                }
                else
                    from.SendAsciiMessage("You cannot use this again so soon");
            }
            else
                from.SendAsciiMessage("You are to far away to use that!");

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public static void RemoveTimer(Mobile m)
        {
            Timer t = (Timer)m_Table[m];

            if (t != null)
            {
                t.Stop();
                m_Table.Remove(m);
            }
        }

        private class DelayTimer : Timer
        {
            private readonly Mobile m_From;

            public DelayTimer(Mobile from)
                : base(TimeSpan.FromSeconds(5))
            {
                m_From = from;
                Priority = TimerPriority.FiftyMS;
            }

            protected override void OnTick()
            {
                RemoveTimer(m_From);
            }
        }
    }
}