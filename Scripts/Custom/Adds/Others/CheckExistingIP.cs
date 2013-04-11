using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Server.Accounting;
using Server.Mobiles;
using Server.Network;

namespace Server
{
    public class CheckExistingIP
    {
        private static Timer warningTimer;

        public static void Initialize()
        {
            EventSink.Login += OnLogin;
        }

        private static void OnLogin(LoginEventArgs e)
        {
            if (e.Mobile == null || e.Mobile.NetState == null || e.Mobile.AccessLevel > AccessLevel.Player)
                return;

            Account a = e.Mobile.Account as Account;
            bool allowMulti = false;

            //Check the connecting player's account for chars with AllowMulti true
            if (a != null)
            {
                for (int i = 0; i < a.Length; ++i)
                {
                    Mobile mob = a[i];
                    if (mob is PlayerMobile && ((PlayerMobile) mob).AllowMulti)
                    {
                        allowMulti = true;
                        break;
                    }
                }
            }

            //If AllowMulti is set on a char, dont need to go further.
            if (allowMulti)
                return;

            List<NetState> states = NetState.Instances;
            List<IAccount> accs = new List<IAccount>();

            for (int i = 0; i < states.Count; ++i)
            {
                IPAddress ip = states[i].Address;
                Mobile m = states[i].Mobile;

                if (e.Mobile.NetState.Address == ip)
                {
                    if (m == null || m == e.Mobile || m.AccessLevel > AccessLevel.Player)
                        continue;

                    //Add all accounts currently connected with the same IP to a list for checking
                    accs.Add(states[i].Account);
                }
            }

            if (accs.Count > 0)
                CheckChars(e.Mobile, accs);
        }

        public static void CheckChars(Mobile m, List<IAccount> acclist)
        {
            bool allowMulti = false;

            //Check all the accounts currently connected for AllowMulti on a char);
            for (int i = 0; i < acclist.Count; ++i)
            {
                Account acct = acclist[i] as Account;
                
                //Check the chars for AllowMulti
                if (acct != null)
                {
                    for (int j = 0; j < acct.Length; ++j)
                    {
                        Mobile mob = acct[j];
                        if (mob is PlayerMobile && ((PlayerMobile) mob).AllowMulti)
                        {
                            allowMulti = true;
                            break;
                        }

                    }
                }
            }

            //If AllowMulti isn't found, start the warning timer
            if (!allowMulti)
            {
                //*****Logging attempt*****
                try
                {
                    Stream fileStream = File.Open("Logs/MultiAccount.log", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    StreamWriter writeAdapter = new StreamWriter(fileStream);
                    writeAdapter.WriteLine("{0}\t{1}\t[{2}]{3}\talready has another account connected from the same IP and AllowMulti is not set", DateTime.Now, m.NetState.Address, m.NetState.Account, m.NetState.Mobile.Name);
                    writeAdapter.Close();
                }
                catch
                {
                }
                //**************************

                if (warningTimer == null || !warningTimer.Running)
                {
                    warningTimer = new WarningTimer();
                    warningTimer.Start();
                }
            }
        }

        public class WarningTimer : Timer
        {
            public WarningTimer() : base(TimeSpan.FromSeconds(3), TimeSpan.FromMinutes(10))
            {
            }

            protected override void OnTick()
            {
                Dictionary<IPAddress, List<Mobile>> ipDictionary = new Dictionary<IPAddress, List<Mobile>>();
                bool stopTimer = true;

                List<NetState> states = NetState.Instances;
                for (int i = 0; i < states.Count; ++i)
                {
                    Mobile m = states[i].Mobile;

                    if (m == null)
                        continue;

                    if (m.AccessLevel > AccessLevel.Player)
                        continue;

                    if (m is PlayerMobile && ((PlayerMobile)m).AllowMulti)
                        continue;

                    IPAddress ipAddress = states[i].Address;

                    List<Mobile> mobileList;
                    if (ipDictionary.TryGetValue(ipAddress, out mobileList))
                        mobileList.Add(m);
                    else
                    {
                        mobileList = new List<Mobile> {m};
                        ipDictionary.Add(ipAddress, mobileList);
                    }
                }

                foreach (KeyValuePair<IPAddress, List<Mobile>> keyValuePair in ipDictionary)
                {
                    List<Mobile> mobileList = keyValuePair.Value;

                    if (mobileList.Count > 1)
                    {
                        stopTimer = false;
                        string multiEntry = string.Format("Staff warning message: {0}", mobileList[0].Name);

                        for (int i = 0; i < mobileList.Count; i++)
                            mobileList[i].SendAsciiMessage(34, "There is currently another account connected with your IP. You are only allowed to own one account per person. Staff has been notified of this and will come see you soon to verify that you are not just one person.");

                        for (int i = 1; i < mobileList.Count; i++)
                            multiEntry += string.Format(" and {0}", mobileList[i].Name);

                        multiEntry += " are connected with the same IP and AllowMulti is not set, please verify that they are different persons.";
                        Commands.CommandHandlers.BroadcastMessage(AccessLevel.GameMaster, 34, multiEntry);
                    }
                }

                if (stopTimer)
                    Stop();
            }
        }
    }
}
