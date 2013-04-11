using Server.Network;

namespace Server.Gumps
{
    public class ChooseCarpRewardGump : Gump
    {
        public ChooseCarpRewardGump()
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
            AddLabel(164, 133, 347, @"Congrats! You GM'd carpentry");
            AddLabel(163, 154, 347, @"Choose your reward.");
            AddImage(123, 120, 2530);
            AddImage(376, 473, 2530);
            AddImageTiled(381, 126, 24, 362, 3505);

            AddLabel(155, 225, 347, @"Hammer"); 
            AddLabel(156, 443, 347, @"Smoothing Plane"); 
            AddLabel(155, 339, 347, @"Saw"); 
            AddLabel(272, 225, 347, @"Inshave"); 
            AddLabel(272, 339, 347, @"Draw Knife"); 
            AddLabel(272, 444, 347, @"Froe");

            AddButton(135, 228, 2117, 2118, 4138, GumpButtonType.Reply, 0); //hammer
            AddButton(135, 341, 2117, 2118, 4148, GumpButtonType.Reply, 0); //saw
            AddButton(135, 447, 2117, 2118, 4146, GumpButtonType.Reply, 0); //smoothingplane
            AddButton(255, 228, 2117, 2118, 4325, GumpButtonType.Reply, 0); //froe
            AddButton(255, 342, 2117, 2118, 4324, GumpButtonType.Reply, 0); //drawknife
            AddButton(255, 447, 2117, 2118, 4326, GumpButtonType.Reply, 0); //inshave

            AddItem(137, 192, 4138); //hammer
            AddItem(136, 281, 4148); //saw
            AddItem(142, 406, 4146); //smoothingplane
            AddItem(258, 199, 4325); //froe
            AddItem(261, 414, 4326); //inshave
            AddItem(260, 305, 4324); //drawknife

        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            sender.Mobile.SendGump(new RewardGump(sender.Mobile.Skills.Carpentry.Info, 0, "Name", info.ButtonID));
        }
    }
}