using Server.Engines.Craft;
using Server.Targeting;

namespace Server.Items
{
    public class SewingKit : BaseTool
    {
       // public override CraftSystem CraftSystem { get { return DefTailoring.CraftSystem; } }

        [Constructable]
        public SewingKit() : base(0xF9D)
        {
            Weight = 2.0;
        }

		[Constructable]
        public SewingKit(int uses) : base(uses, 0xF9D)
		{
			Weight = 2.0;
		}

        public SewingKit(Serial serial) : base(serial)
        {
        }

        public override CraftSystem CraftSystem
        {
            get { return DefTailoring.CraftSystem; }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
                from.Target = new SewingKitTarget(this, CraftSystem);
            else
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize( writer );

            writer.Write( 0 ); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }

        public class SewingKitTarget : Target
        {
            private readonly BaseTool m_Tool;
            private CraftSystem m_CraftSystem;

            public SewingKitTarget(BaseTool tool, CraftSystem craftSystem) : base(4, false, TargetFlags.None)
            {
                m_Tool = tool;
                m_CraftSystem = craftSystem;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Tool != null && (targeted is Cloth || targeted is BoltOfCloth || targeted is BaseLeather))
                    from.SendGump(new CraftGump(from, DefTailoring.CraftSystem, m_Tool, null));
                else
                    from.SendAsciiMessage("You cant use this on that!");
            }
        }
    }
}