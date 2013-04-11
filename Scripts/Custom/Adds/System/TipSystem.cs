using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Util;

namespace Server.Scripts.Custom.Adds.System
{
    class TipSystem
    {
        public class TipCommands
        {
            private static string m_NoTips = @"There are no tips to display";

            public static void Initialize()
            {
                CommandSystem.Register("refreshtips", AccessLevel.Developer, Execute_Refresh);
                CommandSystem.Register("savetips", AccessLevel.Developer, Execute_Save);
                CommandSystem.Register("tips", AccessLevel.Player, Execute_Tips);
                CommandSystem.Register("addtip", AccessLevel.GameMaster, Execute_Add);
            }

            public static void Execute_Save(CommandEventArgs e)
            {
                Mobile from = e.Mobile;
                if (from is PlayerMobile)
                {
                    SaveTips();
                    from.SendMessage("Tips have been saved successfully.");
                }
            }

            public static void Execute_Add(CommandEventArgs e)
            {
                Mobile from = e.Mobile;
                if (from is PlayerMobile)
                {
                    from.SendGump(new AddTipGump());
                }
            }

            public static void Execute_Refresh(CommandEventArgs e)
            {
                Mobile from = e.Mobile;
                if (from is PlayerMobile)
                {
                    TipSystem.LoadTips();
                }
            }

            public static void Execute_Tips(CommandEventArgs e)
            {
                Mobile from = e.Mobile;
                if (!TipSystem.Enabled)
                {
                    from.SendMessage(m_NoTips);
                }
                else if (from is PlayerMobile)
                {
                    from.CloseGump(typeof(TipsGump));
                    from.SendGump(new TipsGump(from as PlayerMobile));
                }
            }
        }

        public class Tip
        {
            public string Text { get; set; }
            public string Url { get; set; }
            public string From { get; set; }
            public string CreateDate { get; set; }
        }

        private static string mFileName = @"Data\tips.xml";

        public static List<Tip> Tips { get; set; }
        public static Boolean Enabled { get; set; }

        public static void Initialize()
        {
            LoadTips();
            EventSink.Login += EventSink_Login;
            EventSink.WorldSave += EventSink_Save;

        }

        private static void EventSink_Save(WorldSaveEventArgs e)
        {
            SaveTips();
        }

        private static void EventSink_Login(LoginEventArgs e)
        {
            Mobile mob = e.Mobile;
            if (mob != null && mob is PlayerMobile && Enabled)
            {
                PlayerMobile pm = mob as PlayerMobile;
                if(pm.ShowTipsOnLogin)
                    mob.SendGump(new TipsGump(pm));
            }
        }

        private static void SaveTips()
        {
            List<Tip> tips = new List<Tip>(Tips);

            if (tips.Count == 0)
                return;

            string filename = Path.Combine(Core.BaseDirectory, mFileName);

            XDocument doc = new XDocument();
            XElement root = new XElement("Tips");
            foreach (Tip t in tips)
            {
                XElement tip = new XElement("Tip");
                tip.Add(new XElement("Text", t.Text));
                tip.Add(new XElement("From", t.From));
                tip.Add(new XElement("CreateDate", t.CreateDate));
                if (StringUtils.HasText(t.Url))
                    tip.Add(new XElement("Url", t.Url));
                root.Add(tip);
            }
            doc.Add(root);
            doc.Save(filename);
        }

        private static void LoadTips()
        {
            Tips = new List<Tip>();
            string filename = Path.Combine(Core.BaseDirectory, mFileName);
            try
            {
                XDocument xdoc = XDocument.Load(filename);
                List<Tip> tips = (from xnode in xdoc.Element("Tips").Elements("Tip")
                                  select new Tip
                                  {
                                      Text = xnode.Element("Text").Value,
                                      From = xnode.Element("From").Value,
                                      Url = xnode.Element("Url") != null ? xnode.Element("Url").Value : null,
                                      CreateDate = xnode.Element("CreateDate") != null ? xnode.Element("CreateDate").Value : null,
                                  }).ToList();
                Tips.AddRange(tips);
            }
            catch
            {
                Console.WriteLine("Error Loading tips.xml, Tips will not be enabled");
            }

            if (Tips.Count > 0)
                Enabled = true;
        }

        private static void AddTip(Tip tip)
        {
            Tips.Add(tip);
        }

        public class AddTipGump : Gump
        {
            private static int m_LabelHue = 0x480;

            public enum Buttons
            {
                Ok = 1,
                Cancel = 2,
                Tip = 3,
                From = 4,
                Url = 5
            }

            public AddTipGump() : base(100, 100)
            {
                MakeGump();
            }

            private void MakeGump()
            {
                Closable = true;
                Disposable = true;
                Dragable = true;
                Resizable = false;
                AddPage(0);
                AddBackground(0, 0, 400, 285, 2600);
                AddLabel(50, 20, m_LabelHue, @"Add Tip");
                AddButton(130, 242, 247, 249, (int)Buttons.Ok, GumpButtonType.Reply, 0);
                AddButton(210, 242, 241, 243, (int)Buttons.Cancel, GumpButtonType.Reply, 0);
                AddTextEntry(24, 51, 353, 107, 0, (int)Buttons.Tip, @"");
                AddTextEntry(94, 170, 283, 20, 0, (int)Buttons.From, @"");
                AddLabel(43, 171, m_LabelHue, @"From");
                AddLabel(41, 205, m_LabelHue, @"Url");
                AddTextEntry(94, 205, 283, 20, 0, (int)Buttons.Url, @"");
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Mobile from = sender.Mobile;
                if (from == null || info.ButtonID == (int) Buttons.Cancel)
                {
                    return;
                }

                string tip = info.GetTextEntry((int)Buttons.Tip).Text.Trim();

                if(tip.Length == 0)
                {
                    from.SendMessage("You must enter a valid tip.");
                    from.SendGump(new AddTipGump());
                    return;
                }

                string creator = info.GetTextEntry((int)Buttons.From).Text;
                string url = info.GetTextEntry((int)Buttons.Url).Text;

                Tip t = new Tip();
                t.CreateDate = DateTime.Now.ToShortDateString();
                t.From = StringUtils.GetString(creator.Trim(), from.Name);
                t.Text = tip;
                t.Url = url.Trim();

                TipSystem.AddTip(t);
            }
        }

        public class TipsGump : Gump
        {
            private static int m_LabelHue = 0x480;
            private static string m_Title = @"Did you know?", m_CheckboxLabel = @"Show Tips on Login", m_NextLabel = @"Next Tip",
                m_GoLabel = @"Go", m_FromLabel = @"Tip provided by:";
            private static Random m_Random = new Random();

            private Tip CurrentTip { get; set; }
            private PlayerMobile m_Mobile;

            public enum Buttons
            {
                Next = 1,
                Ok = 2,
                Go = 3
            }

            public TipsGump(PlayerMobile m)
                : this(m, TipSystem.Tips[m_Random.Next(0, TipSystem.Tips.Count - 1)])
            {
            }

            public TipsGump(PlayerMobile m, Tip tip) : base(100, 100)
            {           
                if (m != null)
                {
                    m_Mobile = m;
                    CurrentTip = tip;
                    MakeGump();
                }
            }

            private void MakeGump()
            {
                AddBackground(0, 0, 400, 300, 2600);
                AddLabel(50, 20, m_LabelHue, m_Title);
                AddCheck(40, 235, 210, 211, m_Mobile.ShowTipsOnLogin, 0);
                AddLabel(65, 235, m_LabelHue, m_CheckboxLabel);
                AddHtml(25, 50, 350, 150, CurrentTip.Text, (bool)true, (bool)true);
                AddButton(290, 260, 247, 249, (int)Buttons.Ok, GumpButtonType.Reply, 0);
                AddButton(65, 260, 4005, 4007, (int)Buttons.Next, GumpButtonType.Reply, 0);
                AddLabel(105, 261, m_LabelHue, m_NextLabel);
                AddLabel(143, 205, m_LabelHue, m_FromLabel);
                AddLabel(255, 205, m_LabelHue, CurrentTip.From);
                if (StringUtils.HasText(CurrentTip.Url))
                {
                    AddButton(290, 235, 22153, 22154, (int)Buttons.Go, GumpButtonType.Reply, 0);
                    AddLabel(310, 235, m_LabelHue, m_GoLabel);
                }
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Mobile from = sender.Mobile;
                if (from == null || !(from is PlayerMobile))
                {
                    return;
                }

                PlayerMobile pm = from as PlayerMobile;
                pm.ShowTipsOnLogin = info.IsSwitched(0);

                switch (info.ButtonID)
                {
                    case (int)Buttons.Go:
                        if (StringUtils.HasText(CurrentTip.Url))
                        {
                            pm.LaunchBrowser(CurrentTip.Url);
                        }
                        break;
                    case (int)Buttons.Next:
                        ShowNextTip(pm);
                        break;
                }
            }

            private void ShowNextTip(PlayerMobile to)
            {
                List<Tip> tips = TipSystem.Tips;
                int nextIndex = tips.IndexOf(CurrentTip) +1;
                if (nextIndex == tips.Count)
                {
                    nextIndex = 0;
                }
                Tip nextTip = tips[nextIndex];
                to.SendGump(new TipsGump(to, nextTip));
            }
        }
    }
}