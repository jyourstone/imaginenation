using Server.Network;

namespace Server.Gumps
{
    public class ChooseBardRewardGump : Gump
    {
        public ChooseBardRewardGump()
            : base(100, 100)
        {
            Closable = false;
            Disposable = false;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddImageTiled(127, 124, 257, 366, 3504);
            AddImageTiled(125, 102, 260, 24, 3501);
            AddImageTiled(105, 127, 23, 362, 3503);
            AddImageTiled(130, 482, 263, 26, 3507);
            AddImage(105, 102, 3500);
            AddImage(381, 102, 3502);
            AddImage(105, 482, 3506);
            AddImage(381, 482, 3508);
            AddLabel(164, 133, 347, @"Congrats! You GM'd all bardic");
            AddLabel(163, 154, 347, @"skills. Choose an instrument.");
            AddImage(123, 120, 2530);
            AddImage(376, 473, 2530);
            AddImageTiled(381, 126, 24, 362, 3505);

            AddLabel(155, 225, 347, @"Drums");
            AddLabel(156, 443, 347, @"Lap Harp");
            AddLabel(155, 339, 347, @"Harp");
            AddLabel(264, 225, 347, @"Tambourine");
            AddLabel(264, 339, 347, @"Tambourine Tassel");
            AddLabel(264, 444, 347, @"Lute");

            AddButton(135, 228, 2117, 2118, 3740, GumpButtonType.Reply, 0); //Drums
            AddButton(135, 341, 2117, 2118, 3761, GumpButtonType.Reply, 0); //Harp
            AddButton(135, 447, 2117, 2118, 3762, GumpButtonType.Reply, 0); //Lap Harp
            AddButton(244, 228, 2117, 2118, 3741, GumpButtonType.Reply, 0); //Tambourine
            AddButton(243, 342, 2117, 2118, 3742, GumpButtonType.Reply, 0); //Tambourine Tassel
            AddButton(245, 447, 2117, 2118, 3763, GumpButtonType.Reply, 0); //Lute
            
            AddItem(137, 192, 3740);
            AddItem(110, 278, 3761);
            AddItem(126, 406, 3762);
            AddItem(241, 199, 3741);
            AddItem(235, 414, 3763);  
            AddItem(249, 305, 3742);

        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            sender.Mobile.SendGump(new RewardGump(sender.Mobile.Skills.Musicianship.Info, 0, "Name", info.ButtonID));
        }
    }
}