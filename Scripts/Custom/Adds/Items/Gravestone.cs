using Server.Gumps;
using Server.Network;
using Server.Targeting;

namespace Server.Items 
{
    [Flipable(0x1167, 0x1168)]
    public class Gravestone : Item
    {

        private bool m_Filled;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool Filled { get { return m_Filled; } set { m_Filled = value; InvalidateProperties(); } }

        [Constructable]
        public Gravestone()
            : base(0x1167)
        {
            Name = "a Gravestone";
            Weight = 10;
            Stackable = false;
            Hue = 2558;
        }

        public Gravestone(Serial serial)
            : base(serial)
        {
        }

        public class GraveGump : Gump
        {

            public GraveGump()
                : base(60, 60)
            {
                Closable = true;
                Disposable = true;
                Dragable = true;
                Resizable = false;
                AddPage(0);
                AddPage(0);
                AddImage(0, 0, 101);
                AddLabel(87, 119, 0, @"R . I . P");
                AddLabel(53, 146, 0, @"Here rests the soul");
                AddLabel(55, 163, 0, @"Of a broken soldier");
                AddItem(173, 95, 3811, 0);
                AddItem(9, 172, 3812, 0);
                AddItem(139, 199, 7573, 0);
                AddItem(28, 260, 6920, 0);
                AddItem(64, 253, 6883, 0);
                AddItem(100, 247, 6935, 0);
            }

        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Filled)
            {
                if (from.InRange(this, 3) && from.InLOS(this))
                {
                    from.SendAsciiMessage("You look at the Tombstone");
                    from.PlaySound(47);
                    from.SendGump(new GraveGump());
                }
                else if (IsChildOf(from.Backpack))
                {
                    from.SendAsciiMessage("You look at the Tombstone");
                    from.PlaySound(47);
                    from.SendGump(new GraveGump());
                }
                else
                {
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                }
            }

            if (!(Filled))
            {
                if (!IsChildOf(from.Backpack))
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                else
                {
                    from.Target = new InternalTarget(this);
                    from.SendAsciiMessage("This tombstone is blank!");
                    from.SendAsciiMessage("Target a players head to bury them.");
                }
            }
        }

        private class InternalTarget : Target
        {
            private readonly Gravestone m_Gravestone;

            public InternalTarget(Gravestone Gravestone)
                : base(1, false, TargetFlags.None)
            {
                m_Gravestone = Gravestone;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item)
                {
                    Item item = (Item)targeted;
                    if (item is Head)
                    {
                        Head head = item as Head;
                        if (head.Owner != null)
                        {
                            string tempstring = head.Owner.ToString();
                            // set up your split delimiter
                            string[] split = tempstring.Split('"');
                            m_Gravestone.Name = string.Format("the Tombstone of {0}", split[1]);
                            m_Gravestone.Filled = true;
                            from.PlaySound(80);
                            from.SendAsciiMessage("You buried the player!");
                            item.Delete();
                        }
                        else
                            from.SendAsciiMessage("You cannot use that head for the tombstone.");
                    }
                    else
                        from.SendAsciiMessage("That is not a head.");
                }
                else
                    from.SendAsciiMessage("That is not an item");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1); // version 
            writer.Write(m_Filled);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            switch (version)
            {
                case 1:
                    {
                        m_Filled = reader.ReadBool();
                        goto case 0;
                    }
                case 0:
                    {
                        break;
                    }
            }
        }
    } 
}