using System;
using System.Collections.Generic;
using System.Threading;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.AntiMacro
{
    public class AntiMacroGump : Gump
    {
        private static readonly Dictionary<Mobile, AntiMacroGump> m_GumpDictionary = new Dictionary<Mobile, AntiMacroGump>();
        private const int c_MaxTriesBeforeDeath = 5;
        private const int c_MaxAllowedTries = 20;
        private const string c_CaptchaURLFormat = @"http://game.in-uo.net/am/index.php?id={0}&s={1}";
        private static readonly DateTime m_LinuxTime = new DateTime(1970, 1, 1, 0, 0, 0);
        private static TimeSpan c_MaxTime = TimeSpan.FromSeconds(180);

        private int m_TriesLeft;
        private int m_TotalTriesLeft;
        private readonly PlayerMobile m_Owner;
        private NewMacroTimer m_MacroTimer;
        private string m_Answer = string.Empty;
        private readonly int m_Seed;

        private enum Replies
        {
            Submit = 100,
            OpenWebSite
        }

        public static void SendGumpThreaded(PlayerMobile to)
        {
            if (to.AntiMacroGump && m_GumpDictionary.ContainsKey(to))
            {
                AntiMacroGump oldGump = m_GumpDictionary[to];
                m_GumpDictionary.Remove(to);

                //Close existing gump if it exists
                if (to.HasGump(typeof(AntiMacroGump)))
                    to.CloseGump(typeof(AntiMacroGump));

                to.SendGump(new AntiMacroGump(oldGump) );

                return;
            }

            if ( !MySQLManager.SQLEnabled)
            {
                to.SendGump(new OldAntiMacroGump(to));
                return;
            }

            AntiMacroGump gump = new AntiMacroGump(to);
            new Thread(gump.ThreadedGump).Start();
        }

        private void ThreadedGump()
        {
            //Don't show the gump if we cant conncet to the SQL - Might be an idea
            //to insert the old anti macro code here, though i doubt this will happend a lot
            if (!MySQLManager.InsertToSQL(m_Owner, m_Seed, DateTime.Now.Subtract(m_LinuxTime).TotalSeconds + c_MaxTime.TotalSeconds))
            {
                m_Owner.SendGump(new OldAntiMacroGump(m_Owner));
                return;
            }

            m_Owner.SendGump(this, false);
        }

        private AntiMacroGump(PlayerMobile owner) : base( 0, 0 )
        {
            m_Owner = owner;
            m_Owner.AntiMacroGump = true;

            m_Seed = Utility.Random(9999);
            m_TriesLeft = c_MaxTriesBeforeDeath;
            m_TotalTriesLeft = c_MaxAllowedTries;

            //Start the macro check timer. If the user doesn't respond within c_MaxTime seconds, he will be penalized.
            m_MacroTimer = new NewMacroTimer(this, c_MaxTime);
            m_MacroTimer.Start();

            InitiateGump();

            if (m_GumpDictionary.ContainsKey(m_Owner))
                m_GumpDictionary[m_Owner] = this;
            else
                m_GumpDictionary.Add(m_Owner, this);
        }

        private AntiMacroGump(AntiMacroGump oldGump) : base(0, 0)
        {
            m_Owner = oldGump.m_Owner;

            m_Seed = oldGump.m_Seed;
            m_TriesLeft = oldGump.m_TriesLeft;
            m_TotalTriesLeft = oldGump.m_TotalTriesLeft;

            m_MacroTimer = oldGump.m_MacroTimer;

            InitiateGump();

            if (m_GumpDictionary.ContainsKey(m_Owner))
                m_GumpDictionary[m_Owner] = this;
            else
                m_GumpDictionary.Add(m_Owner, this);
        }

        private void InitiateGump()
        {
            Closable = false;
            Disposable = false;
            Dragable = true;
            Resizable = false;

            string triesLeftString = string.Format("{0} {1} left before dying!", m_TriesLeft, (m_TriesLeft == 1 ? "try" : "tries"));

            if (m_TriesLeft > 0)
                m_Owner.SendAsciiMessage("You have " + triesLeftString);

            AddPage(0);
            AddBackground(179, 43, 353, 116, 9200);

            AddLabel(183, 48, 0, @"Please click on the question mark (?) to the right.");
            AddLabel(183, 68, 0, @"Then enter the code from the website and press ok.");
            AddButton(505, 59, 22153, 22154, (int)Replies.OpenWebSite, GumpButtonType.Reply, 0);
            if (m_TriesLeft > 0)
                AddLabel(340, 95, 0, triesLeftString);
            AddLabel(183, 117, 0, @"If we cannot open your browser, please go to:");
            AddLabel(183, 138, 0, string.Format(c_CaptchaURLFormat, m_Owner.Serial.GetHashCode(), m_Seed));
            AddImage(185, 93, 2445);
            AddTextEntry(191, 94, 106, 20, 0, 0, @"");
            AddButton(305, 93, 4023, 4024, (int)Replies.Submit, GumpButtonType.Reply, 0);
        }

        private void StartThreadedValidation()
        {
            new Thread(ThreadedValidation).Start();
        }

        private void ThreadedValidation()
        {
            if (!MySQLManager.ValidateInput(m_Owner, m_Answer, m_TriesLeft))
            {
                if (m_TriesLeft == 1)
                    new KillTimer(m_Owner).Start(); //Dirty fix to fix any possible threaded issues.

                if (m_TotalTriesLeft <= 0) //So people can't spam and eventually enter the correct answer
                {
                    m_Owner.SendAsciiMessage("You have entered the incorrect answer too many times and you will now need to page staff to get unfrozen");
                    m_Owner.AntiMacroGump = false;
                    return;
                }

                if (m_TriesLeft > 0)
                    m_TriesLeft--;

                m_TotalTriesLeft--;

                m_Owner.SendAsciiMessage("Wrong answer!");
                AntiMacroGump newGump = new AntiMacroGump(this);
                m_Owner.SendGump(newGump);

                //We dont want to end the timer and free the owner
                return;
            }

            //End the timer
            if (m_MacroTimer != null)
            {
                m_MacroTimer.Stop();
                m_MacroTimer = null;
            }

            FreeOwner();

            m_Owner.SendAsciiMessage(string.Format("Thank you {0} for validating your presence. Have a nice day!", m_Owner.Name));
            m_Owner.CantWalk = false;
            if (m_Owner.Frozen)
                m_Owner.Frozen = false;
        }

        private void StartThreadedRemoveFromDataBase()
        {
            new Thread(TheadedRemoveFromDataBase).Start();
        }

        private void TheadedRemoveFromDataBase()
        {
            MySQLManager.RemoveFromDataBase(m_Owner.Serial.GetHashCode());
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            //Set the answering string
            if (info == null || info.GetTextEntry(0) == null || info.GetTextEntry(0).Text == null )
            {
                m_Answer = string.Empty; //Crash fix. Think that the player somehow managed to respond when the gump had expired
                StartThreadedValidation();
                return;
            }
            else
                m_Answer = info.GetTextEntry(0).Text.Trim();

            switch(info.ButtonID)
            {
                case 0:
				{
					break;
				}
                case (int)Replies.Submit:
                {
                    StartThreadedValidation();
                    break;
                }
                case (int)Replies.OpenWebSite:
                {
                    LaunchBrowser();
                    m_Owner.SendGump(new AntiMacroGump(this));
                    break;
                }
            }
        }

        private void LaunchBrowser()
        {
            m_Owner.LaunchBrowser(string.Format(c_CaptchaURLFormat, m_Owner.Serial.GetHashCode(), m_Seed));
        }

        private void FreeOwner()
        {
            m_Owner.CloseGump(typeof(AntiMacroGump));
            m_Owner.AntiMacroGump = false;

            if (m_GumpDictionary.ContainsKey(m_Owner))
                m_GumpDictionary.Remove(m_Owner);

        }

        private class NewMacroTimer : Timer
        {
            private readonly PlayerMobile m_From;

            public NewMacroTimer(AntiMacroGump starterGump, TimeSpan duration) : base(duration)
            {
                m_From = starterGump.m_Owner;

                m_From.AntiMacroGump = true;
                m_From.SendAsciiMessage(33, string.Format("You now have {0} seconds to respond before you are killed.", duration.TotalSeconds));

                m_From.Frozen = true;
                m_From.CantWalk = true;
            }

            protected override void OnTick()
            {
                m_From.Kill();
                m_From.Frozen = true; //Needs to reset since it's removed when killed
            }
        }

        private class KillTimer : Timer
        {
            private readonly PlayerMobile m_From;

            public KillTimer(PlayerMobile mobile) : base(TimeSpan.FromMilliseconds(0.0))
            {
                m_From = mobile;
            }

            protected override void OnTick()
            {
                m_From.CriminalAction(false);
                m_From.Kill();
                m_From.Frozen = true; //Needs to reset since it's removed when killed
            }
        }
    }
}