using System;
using System.Collections.Generic;
using Server.Network;

namespace Server.Items
{
    public class MiniTrashCan : Container
	{

        public override int DefaultMaxWeight { get { return 0; } } // A value of 0 signals unlimited weight

        public override bool IsDecoContainer
        {
            get { return false; }
        }
		[Constructable]
		public MiniTrashCan() : base( 0xE7A )
		{
			Weight = 0.1;
			Hue = 2523;
			Name = "Mini trash can";
			LootType = LootType.Blessed;
		}

		public MiniTrashCan( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (Items.Count > 0)
            {
                m_Timer = new EmptyTimer(this);
                m_Timer.Start();
            }
		}

        public override void OnDoubleClick(Mobile from)
        {
            return;
        }

		public override bool OnDragDrop( Mobile from, Item item )
		{

			if ( item.LootType == LootType.Newbied || item.LootType == LootType.Blessed )
			{
                from.LocalOverheadMessage(MessageType.Regular, 906, true, "You cannot trash blessed or newbied items");
                return false;
			}
            if (!base.OnDragDrop(from, item))
                return false;

            
            if (TotalItems >= 50)
            {
                Empty(501478); // The trash is full!  Emptying!
            }
            
            else
            {
                /*
                from.LocalOverheadMessage(MessageType.Regular, 906, true, string.Format("The item will be deleted in 20 seconds"));
                
                if (m_Timer != null)
                    m_Timer.Stop();
                else
                    m_Timer = new EmptyTimer(this);

                m_Timer.Start();
                */
                from.LocalOverheadMessage(MessageType.Regular, 906, true, "You trash the item");
                item.Delete();

			}
            return true;
		}
        public override bool OnDragDropInto(Mobile from, Item item, Point3D p)
        {
            if (item.LootType == LootType.Newbied || item.LootType == LootType.Blessed)
            {
                from.LocalOverheadMessage(MessageType.Regular, 906, true, "You cannot trash blessed or newbied items");
                return false;
            }

            if (!base.OnDragDropInto(from, item, p))
                return false;

            
            if (TotalItems >= 50)
            {
                Empty(501478); // The trash is full!  Emptying!
            }

            else
            {
                /*
                from.LocalOverheadMessage(MessageType.Regular, 906, true, string.Format("The item will be deleted in 20 seconds"));

                if (m_Timer != null)
                    m_Timer.Stop();
                else
                    m_Timer = new EmptyTimer(this);

                m_Timer.Start();
                */

                from.LocalOverheadMessage(MessageType.Regular, 906, true, "You trash the item");
                item.Delete();
            }

            return true;
        }
        public void Empty(int message)
        {
            List<Item> items = Items;

            if (items.Count > 0)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, message, "");

                for (int i = items.Count - 1; i >= 0; --i)
                {
                    if (i >= items.Count)
                        continue;

                    items[i].Delete();
                }
            }

            if (m_Timer != null)
                m_Timer.Stop();

            m_Timer = null;
        }

        private Timer m_Timer;

        private class EmptyTimer : Timer
        {
            private readonly MiniTrashCan m_Minitrashcan;

            public EmptyTimer(MiniTrashCan minitrashcan) : base(TimeSpan.FromSeconds(20.0))
            {
                m_Minitrashcan = minitrashcan;
                Priority = TimerPriority.FiveSeconds;
            }

            protected override void OnTick()
            {
                m_Minitrashcan.Empty(501479); // Emptying the trashcan!
            }
        }

    }
}