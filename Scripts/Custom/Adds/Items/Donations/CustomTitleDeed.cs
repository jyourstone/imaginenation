using Server.Guilds;
using Server.Prompts;

namespace Server.Items
{
    public class CustomTitleDeed : Item
    {
        [Constructable]
        public CustomTitleDeed() : base(5359)
        {
            Name = "Custom Title Deed";
            Hue = 1945;
        }

        public CustomTitleDeed(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendAsciiMessage("Please enter your desired title or press ESC to cancel.");
                from.Prompt = new CustomTitlePrompt(this);
            }
            else
                from.SendLocalizedMessage(1045156); // You must have the deed in your backpack to use it.
        }

        private class CustomTitlePrompt : Prompt
        {
            private readonly Item m_Item;

            public CustomTitlePrompt(Item item)
            {
                m_Item = item;
            }

            public override void OnResponse(Mobile from, string text)
            {
                if (m_Item == null || m_Item.Deleted)
                    return;

                if (m_Item.IsChildOf(from.Backpack))
                {
                    string title = Utility.FixHtml(text.Trim());

                    if (title.Length > 20)
                        from.SendLocalizedMessage(501178); // That title is too long.
                    else if (!BaseGuildGump.CheckProfanity(title))
                        from.SendLocalizedMessage(501179); // That title is disallowed.
                    else
                    {
                        from.Title = title;
                        from.SendAsciiMessage("Your title has been changed!");
                        Effects.SendLocationEffect(from.Location, from.Map, 14170, 20, 1944, 0);
                        from.PlaySound(0x064C);
                        m_Item.Consume();
                    }
                }
                else
                    from.SendLocalizedMessage(1045156); // You must have the deed in your backpack to use it.
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