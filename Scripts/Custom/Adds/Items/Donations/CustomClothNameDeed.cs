using Server.Guilds;
using Server.Prompts;
using Server.Targeting;

namespace Server.Items
{
    public class CustomClothNameDeed : Item
    {
        [Constructable]
        public CustomClothNameDeed() : base(5359)
        {
            Name = "Custom Clothing Name Deed";
            Hue = 1947;
        }

        public CustomClothNameDeed(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.Target = new InternalTarget(this);
                from.SendAsciiMessage("Please target the clothing you want to change the name on.");
            }
            else
                from.SendLocalizedMessage(1045156); // You must have the deed in your backpack to use it.
        }

        private class InternalTarget : Target
        {
            private readonly Item m_Deed;

            public InternalTarget(Item item) : base(3, false, TargetFlags.None)
            {
                m_Deed = item;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (!(target is BaseClothing))
                {
                    from.SendAsciiMessage("That is not clothing!");
                    return;
                }

                Item item = target as Item;

                if (!item.IsChildOf(from.Backpack))
                {
                    from.SendAsciiMessage("The clothing must be in your backpack.");
                    return;
                }

                from.SendAsciiMessage("Please enter the desired name or press ESC to cancel.");
                from.Prompt = new CustomTitlePrompt(m_Deed, item);
            }
        }

        private class CustomTitlePrompt : Prompt
        {
            private readonly Item m_Deed;
            private readonly Item m_Cloth;

            public CustomTitlePrompt(Item deed, Item cloth)
            {
                m_Deed = deed;
                m_Cloth = cloth;
            }

            public override void OnResponse(Mobile from, string text)
            {
                if (m_Deed == null || m_Deed.Deleted || m_Cloth == null || m_Cloth.Deleted)
                    return;
                
                if (!m_Deed.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(1045156); // You must have the deed in your backpack to use it.
                    return;
                }

                if (!m_Cloth.IsChildOf(from.Backpack))
                {
                    from.SendAsciiMessage("The clothing must be in your backpack.");
                    return;
                }

                string name = Utility.FixHtml(text.Trim());

                if (name.Length > 20)
                    from.SendAsciiMessage("That name is too long.");
                else if (!BaseGuildGump.CheckProfanity(name))
                    from.SendAsciiMessage("That name is disallowed.");
                else
                {
                    m_Cloth.Name = name;
                    from.SendAsciiMessage("The name on your clothing has been changed!");
                    Effects.SendLocationEffect(from.Location, from.Map, 14170, 20, 1946, 0);
                    from.PlaySound(0x064E);
                    m_Deed.Consume();
                }
            }

            public override void OnCancel(Mobile from)
            {
                from.SendAsciiMessage("Canceled...");
                base.OnCancel(from);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}