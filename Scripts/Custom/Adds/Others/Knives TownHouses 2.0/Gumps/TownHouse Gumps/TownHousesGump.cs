using System.Collections;
using Server;
using Server.Gumps;
using Server.Multis;

namespace Knives.TownHouses
{
    public class TownHousesGump : GumpPlusLight
    {
        public static void Initialize()
        {
            RUOVersion.AddCommand("TownHouses", AccessLevel.Counselor, OnHouses);
        }

        private static void OnHouses(CommandInfo info)
        {
            new TownHousesGump(info.Mobile);
        }

        private int c_Page;

        public TownHousesGump(Mobile m)
            : base(m, 100, 100)
        {
            m.CloseGump(typeof(TownHousesGump));
        }

        protected override void BuildGump()
        {
            int width = 400;
            int y = 0;

            AddHtml(0, y += 10, width, "<CENTER>TownHouses Main Menu");
            AddImage(width / 2 - 120, y + 2, 0x39);
            AddImage(width / 2 + 90, y + 2, 0x3B);

            int pp = 10;

            if (c_Page != 0)
                AddButton(width / 2 - 10, y += 25, 0x25E4, 0x25E5, "Page Down", PageDown);

            ArrayList list = new ArrayList(TownHouseSign.AllSigns);

            list.Sort(new InternalSort());

            TownHouseSign sign = null;

            y += 5;

            for (int i = c_Page * pp; i < (c_Page + 1) * pp && i < list.Count; ++i)
            {
                sign = (TownHouseSign)list[i];

                AddHtml(30, y += 20, width/2-20, "<DIV ALIGN=LEFT>" + sign.Name);
                AddButton(15, y + 3, 0x2716, "TownHouse Menu", TownHouseMenu, sign);

                if (sign.House != null && sign.House.Owner != null)
                {
                    AddHtml(width/2, y, width/2 - 40, "<DIV ALIGN=RIGHT>" + sign.House.Owner.RawName);
                    AddButton(width-30, y+3, 0x2716, "House Menu", HouseMenu, sign.House);
                }
            }

            if (pp * (c_Page + 1) < list.Count)
                AddButton(width / 2 - 10, y += 25, 0x25E8, 0x25E9, "Page Up", PageUp);

            AddHtml(0, y+=35, width, "<CENTER>Add New TownHouse");
            AddButton(width / 2 - 80, y + 3, 0x2716, "New", New);
            AddButton(width / 2 + 70, y + 3, 0x2716, "New", New);

            AddBackgroundZero(0, 0, width, y + 40, 0x13BE);
        }

        private void TownHouseMenu(object obj)
        {
            if (!(obj is TownHouseSign))
                return;

            NewGump();

            new TownHouseSetupGump(Owner, (TownHouseSign)obj);
        }

        private void HouseMenu(object obj)
        {
            if (!(obj is BaseHouse))
                return;

            NewGump();

            Owner.SendGump(new HouseGumpAOS(0, Owner, (BaseHouse)obj));
        }

        private void New()
        {
            TownHouseSign sign = new TownHouseSign();
            Owner.AddToBackpack(sign);
            Owner.SendMessage("A new sign is now in your backpack.  It will move on it's own during setup, but if you don't complete setup you may want to delete it.");

            NewGump();

            new TownHouseSetupGump(Owner, sign);
        }

        private void PageUp()
        {
            c_Page++;

            NewGump();
        }

        private void PageDown()
        {
            c_Page--;

            NewGump();
        }


        private class InternalSort : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                    return 0;
                if (x == null || !(x is TownHouseSign))
                    return -1;
                if (y == null || !(y is TownHouseSign))
                    return 1;

                TownHouseSign a = (TownHouseSign)x;
                TownHouseSign b = (TownHouseSign)y;

                return Insensitive.Compare(a.Name, b.Name);
            }
        }
    }
}