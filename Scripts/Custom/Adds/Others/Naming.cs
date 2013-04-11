using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Server.Gumps;
using Server.Misc;
using Server.Accounting;
using Server.Network;
using Server.Util;
using Server.Mobiles;

namespace Server
{
    /// <summary>
    /// Summary description for Naming.
    /// </summary>
    public class Naming
    {
        private static HashSet<string> m_ReservedNames = new HashSet<string>();

        public static void Initialize()
        {
            LoadReservedNames();
            EventSink.ServerStarted += EventSink_ServerStarted;
            EventSink.Login += EventSink_Login;
            EventSink.DeleteRequest += EventSink_DeleteCharacter;
        }

        public static void EventSink_ServerStarted()
        {
            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m.Player)
                {
                    m_ReservedNames.Add(m.Name.ToLower());
                }
            }
        }

        private static void EventSink_DeleteCharacter(DeleteRequestEventArgs e)
        {
            NetState state = e.State;
            int index = e.Index;
            Account acct = state.Account as Account;

            if (acct != null && index >= 0 && index < acct.Length)
            {
                Mobile m = acct[index];
                if (m != null && m.Player)
                {
                    m_ReservedNames.Remove(m.Name.ToLower());
                }
            }
        }

        private static void EventSink_Login(LoginEventArgs e)
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;
            if (pm != null) //Taran: This can be null sometimes
            {
                if (pm.AccessLevel == AccessLevel.Player && (pm.Name == "Generic Player" || (pm.GameTime < TimeSpan.FromSeconds(30.0) && !CheckName(pm.Name))))
                {
                    e.Mobile.CantWalk = true;
                    e.Mobile.SendGump(new NamingGump(e.Mobile));
                }
            }
        }

        public static bool CheckName(string name)
        {
            if (!StringUtils.HasText(name))
                return false;

            if (!NameVerification.Validate(name, 2, 16, true, false, true, 1, NameVerification.SpaceDashPeriodQuote) || m_ReservedNames.Contains(name.ToLower()))
                return false;

            return true;
        }

        private static void LoadReservedNames()
        {
            string filename = Path.Combine(Core.BaseDirectory, @"Data\ReservedNames.txt");

            if (File.Exists(filename))
            {
                try
                {
                    string[] names = File.ReadAllLines(filename);
                    foreach (string s in names)
                    {
                        m_ReservedNames.Add(s.Trim().ToLower());
                    }
                }
                catch
                {
                }
            }
        }

        private class NamingGump : Gump
        {
            private const int GreenHue = 0x40;
            private const int LabelHue = 0x480;
            private Mobile m_Mobile;
            private static string m_Html = @"<basefont color=#FFFFFF>The name you have selected for your character isn't available. Imagine Nation ensures that all player names are unique to prevent confusion. Please select a new name for your character and press the OK button.<p>Thank you,<br>The Imagine Nation staff.";
            private static string m_Welcome = @"Welcome to Imagine Nation";
            private static string m_Message = @"Please select a new name for your character:";
            private static string m_NotAllowed = @"The name you have chosen isn't allowed. Please try again.";
            private static string m_ThankYou = @"Thank you for choosing a different name. Welcome to Imagine Nation, {0}";

            public NamingGump(Mobile m)
                : base(100, 100)
            {
                m_Mobile = m;
                m_Mobile.Name = "Generic Player";
                MakeGump();
            }

            private void MakeGump()
            {
                this.Closable = false;
                this.Disposable = true;
                this.Dragable = false;
                this.Resizable = false;

                this.AddPage(0);
                this.AddBackground(0, 0, 520, 375, 9270);
                this.AddLabel(188, 30, GreenHue, m_Welcome);
                this.AddHtml(60, 80, 395, 130, m_Html, false, false);
                this.AddLabel(120, 250, GreenHue, m_Message);
                this.AddImageTiled(170, 280, 180, 20, 5174);
                this.AddImageTiled(171, 281, 178, 18, 9274);
                this.AddTextEntry(170, 280, 180, 20, LabelHue, 0, @"");
                this.AddButton(245, 320, 4023, 4024, 1, GumpButtonType.Reply, 0);
            }

            public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
            {
                switch (info.ButtonID)
                {
                    case 1:

                        string name = info.TextEntries[0].Text;

                        if (name != null && !Naming.CheckName(name))
                            name = "Generic Player";

                        if (name == null || name == "Generic Player")
                        {
                            sender.Mobile.SendMessage(0x20, m_NotAllowed);
                            sender.Mobile.SendGump(new NamingGump(sender.Mobile));
                        }
                        else
                        {
                            sender.Mobile.Name = name;
                            sender.Mobile.CantWalk = false;
                            sender.Mobile.SendMessage(0x20, m_ThankYou, name);
                            m_ReservedNames.Add(name.ToLower());
                        }
                        break;
                    default:
                        if (sender.Mobile.NetState != null)
                            sender.Mobile.NetState.Dispose();
                        break;
                }
            }

        }
    }
}