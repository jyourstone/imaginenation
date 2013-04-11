using Server.Targeting;

namespace Server.Items
{
    public class BlackDyeTarget : Target 
    {
        private readonly BlackDyeDeed m_Deed;

        public BlackDyeTarget(BlackDyeDeed deed)
            : base(1, false, TargetFlags.None)
        {
            m_Deed = deed;
        }

        protected override void OnTarget(Mobile from, object target) 
        {
            if (m_Deed.Deleted || m_Deed.RootParent != from)
                return;

            if (target is BaseClothing)
            {
                BaseClothing item = (BaseClothing)target;

                if (item.Hue == 1) 
                {
                    from.SendAsciiMessage("That is already pure black");
                }
                else if (!item.Movable)
                {
                    from.SendAsciiMessage("You cannot dye an item that is locked down");
                }
                else if (item.RootParent != from)
                {
                    from.SendAsciiMessage("You cannot dye that");
                }
                else
                {
                    item.Hue = 1;
                    from.SendAsciiMessage("You dye the item");
                    m_Deed.Delete(); 
                }
            }
            else
            {
                from.SendAsciiMessage("You cannot dye that");
            }
        }
    }

    public class BlackDyeDeed : Item 
    {
        public override string DefaultName
        {
            get { return "Clothing black dye deed"; }
        }

        [Constructable]
        public BlackDyeDeed()
            : base(0x14F0)
        {
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public BlackDyeDeed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            LootType = LootType.Blessed;

            int version = reader.ReadInt();
        }

        public override bool DisplayLootType { get { return false; } }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001);
            }
            else
            {
                from.SendAsciiMessage("What article of clothing would you like to dye?"); 
                from.Target = new BlackDyeTarget(this); 
            }
        }
    }
}