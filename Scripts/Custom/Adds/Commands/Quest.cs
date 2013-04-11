using System;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Scripts.Custom.Adds.Commands
{
    public class Quest
    {
        public static void Initialize()
        {
            CommandSystem.Register("Quest", AccessLevel.Player, Execute);
        }

        [Usage("Quest")]
        [Description("Will display some options to any quests you are currently in.")]

        private static void Execute(CommandEventArgs e)
        {
            PlayerMobile pm = (PlayerMobile)e.Mobile;

            if (pm == null)
                return;

            if (pm.Quest != null)
            {
                pm.CloseGump(typeof(QuestGump));
                pm.SendGump(new QuestGump(pm));
            }
            else
            {
                pm.SendAsciiMessage("You have no active quests");
                return;
            }
        }
    }

    public class QuestGump : Gump
    {
        private readonly PlayerMobile pm;
        private const int White = 0xFFFFFF;

        public QuestGump(PlayerMobile from): base(50, 50)
        {
            pm = from;
           
            Closable = true;
            Disposable = true;
            Dragable = true;

            AddPage(0);
            AddBackground(10, 10, 242, 242, 9270);
            AddBlackAlpha(20, 20, 220, 220);
            AddHtml(82, 23, 47, 20, Color("Quest", White), false, false);
            
            AddImage(177, 20, 1261);

            AddImage(0, 0, 10460);
            AddImage(228, 227, 10460);
            AddImage(229, 0, 10460);
            AddImage(0, 229, 10460);
            
            if (pm.Quest.Objectives.Count > 0)
            {
                AddButton(33, 79, 2103, 2104, 1, GumpButtonType.Reply, 0);
                AddHtml(50, 75, 102, 20, Color("View Quest Log", White), false, false);
            }

            if (pm.Quest.Conversations.Count > 0)
            {
                AddHtml(50, 124, 125, 20, Color("Quest Conversation", White), false, false);
                AddButton(33, 129, 2103, 2104, 2, GumpButtonType.Reply, 0);
            }

            AddHtml(50, 175, 86, 20, Color("Cancel Quest", White), false, false);
            AddButton(33, 179, 2103, 2104, 3, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(Network.NetState sender, RelayInfo info)
        {
            PlayerMobile p = (PlayerMobile) sender.Mobile;
            
            if (p == null)
                return;

            if (info.ButtonID == 1)
                p.Quest.ShowQuestLog();
            else if (info.ButtonID == 2)
                p.Quest.ShowQuestConversation();
            else if (info.ButtonID == 3)
                p.Quest.BeginCancelQuest();
            else
                p.CloseGump(typeof(QuestGump));
        }

        public string Color(string text, int color)
        {
            return String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text);
        }

        public void AddBlackAlpha(int x, int y, int width, int height)
        {
            AddImageTiled(x, y, width, height, 2624);
            AddAlphaRegion(x, y, width, height);
        }
    }
}
