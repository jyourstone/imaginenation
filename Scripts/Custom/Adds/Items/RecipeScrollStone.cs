using System.Collections.Generic;
using Server.Network;

namespace Server.Items
{
    public class RecipeScrollStone : Item
    {
        private int m_Recipe = -1;
        private Dictionary<Serial, Mobile> m_MobileList = new Dictionary<Serial, Mobile>();

        [Constructable]
        public RecipeScrollStone() : base(0x2a94)
        {
            Name = "Recipe Scroll Stone";
            Movable = false;
            Hue = 2708;
        }

        public RecipeScrollStone(Serial serial)
            : base(serial)
        {
        }


        [CommandProperty(AccessLevel.GameMaster)]
        public int Recipe
        {
            get { return m_Recipe; }
            set
            {
                m_Recipe = value;
                InvalidateProperties();
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InLOS(this) && from.InRange(Location, 2))
            {
                if (m_Recipe > 0)
                {
                    if (!m_MobileList.ContainsKey(from.Serial))
                    {
                        Item scroll = new RecipeScroll(m_Recipe);
                        from.AddToBackpack(scroll);
                        m_MobileList.Add(from.Serial, from);
                        from.SendAsciiMessage("You put the recipe scroll in your backpack");
                    }
                    else
                        from.SendAsciiMessage("You have already gotten the recipe!");
                }
                else
                    from.SendAsciiMessage("This stone is not activated");
            }
            else
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1); // version 
            writer.Write(m_Recipe);
            writer.Write(m_MobileList.Count);
            foreach (KeyValuePair<Serial, Mobile> keyValuePair in m_MobileList)
                writer.WriteMobile(keyValuePair.Value);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            switch (version)
            {
                case 1:
                        m_Recipe = reader.ReadInt();

                        m_MobileList = new Dictionary<Serial, Mobile>();    
                        int count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                        {
                            Mobile toAdd = reader.ReadMobile();
                            if (toAdd != null)
                                m_MobileList.Add(toAdd.Serial, toAdd);
                        }

                        goto case 0;
                case 0:
                        break;
            }
        }
    }
}