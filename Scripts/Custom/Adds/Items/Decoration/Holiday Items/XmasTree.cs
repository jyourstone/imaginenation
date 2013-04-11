using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Items
{
    public class INXChristmasTree : Item
    {
        private int m_XmasList = -1;
        private static Dictionary<Serial, Mobile> m_MobileList = new Dictionary<Serial, Mobile>();

        [CommandProperty(AccessLevel.Seer)]
        public int XmasList
        {
            get { return m_XmasList; }
            set
            {
                m_XmasList = value;
                InvalidateProperties();
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            PlayerMobile pl = (PlayerMobile)from;
            
            if (from.InLOS(this) && from.InRange(Location, 2))
            {
                if (m_XmasList > 0)
                {

                    if (!m_MobileList.ContainsKey(from.Serial))
                    {
                        GiftBox gb = new GiftBox();
                        gb.Hue = 2881;
                        gb.Name = "Happy Holidays!";
                        switch (Utility.Random(5))

                            #region Gifts
                        {
                            case 0:

                                gb.DropItem(new GreenCandyCane());
                                gb.DropItem(new SnowCoveredTree());
                                gb.DropItem(new WhitePoinsettia());
                                gb.DropItem(new RandomReindeer());
                                gb.DropItem(new BlueSnowflake());
                                break;
                            case 1:

                                gb.DropItem(new HolidayWreath());
                                gb.DropItem(new WhiteSnowflake());
                                gb.DropItem(new Snowman());
                                gb.DropItem(new MiniXmasTree());
                                gb.DropItem(new RandomReindeer());
                                break;
                            case 2:

                                gb.DropItem(new HolidayCactus());
                                gb.DropItem(new GreenStocking());
                                gb.DropItem(new LumpOfCoal());
                                gb.DropItem(new RedCandyCane());
                                gb.DropItem(new RandomReindeer());
                                break;
                            case 3:

                                gb.DropItem(new GingerBreadMan());
                                gb.DropItem(new FestiveShrub());
                                gb.DropItem(new DecorativeMistletoe());
                                gb.DropItem(new HolidayCandle());
                                gb.DropItem(new RandomReindeer());
                                break;
                            case 4:
                                gb.DropItem(new RedStocking());
                                gb.DropItem(new RedPoinsettia());
                                gb.DropItem(new SnowGlobe());
                                gb.DropItem(new Snowball());
                                gb.DropItem(new RandomReindeer());
                                break;
                        }

                        #endregion

                        pl.Backpack.DropItem(gb);

                        m_MobileList.Add(from.Serial, from);
                        pl.SendAsciiMessage("Happy Holidays from INX Staff!");
                    }
                    else
                        pl.SendAsciiMessage("You already received the gift that Santa left for you!");
                }
                else
                    pl.SendAsciiMessage("There are no gifts for you under the tree right now.");
            }
            else
                from.SendAsciiMessage("You can't reach that");
        }

        [Constructable]
        public INXChristmasTree() : base(0x1B7E)
        {
            Name = "Christmas Tree";
            Movable = false;
        }

        public INXChristmasTree(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1); // version 
            writer.Write(m_XmasList);
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
                        m_XmasList = reader.ReadInt();

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