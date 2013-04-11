using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public class Dices : Item, ITelekinesisable
    {
        private static readonly Hashtable m_Table = new Hashtable();

        [CommandProperty(AccessLevel.Counselor)]
        public override bool EventItem
        {
            get { return base.EventItem; }
            set { base.EventItem = value; }
        }

		[Constructable]
		public Dices() : base( 0xFA7 )
		{
			Weight = 1.0;
		}

		public Dices( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
            if (from.InRange(GetWorldLocation(), 2) && from.InLOS(this))
                Roll(from);
            else
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
		}

        public void OnTelekinesis(Mobile from)
        {
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5022);
            Effects.PlaySound(Location, Map, 0x1F5);

            Roll(from);
        }

        public void Roll(Mobile from)
        {
            if (!m_Table.Contains(from))
            {
                PublicOverheadMessage(MessageType.Emote, 34, false,
                                      string.Format("*{0} rolls {1}, {2}*", from.Name, Utility.Random(1, 6),
                                                    Utility.Random(1, 6)));
                Timer t = new DelayTimer(from);
                m_Table[from] = t;
                t.Start();
            }
            else
                from.SendAsciiMessage("You cannot use this again so soon");
        }
        
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
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
                RemoveTimer( m_From );
            }
        }
	}


}