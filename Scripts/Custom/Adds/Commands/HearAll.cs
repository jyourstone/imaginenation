/*using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Commands
{
    public class HearAll
    {
        private static readonly Dictionary<Mobile, bool> m_HearAll = new Dictionary<Mobile, bool>();

        private static readonly string[] m_IgnoredSpeech = new[]
                                                      {
                                                          "sell",
                                                          "buy",
                                                          "guard",
                                                          "release",
                                                          "stay",
                                                          "bank"
                                                      };

        #region Event Handlers

        private static void OnSpeech(SpeechEventArgs args)
        {
            if (!(args.Mobile is PlayerMobile)) //Only hear player speech
                return;

            if (args.Mobile.Squelched) //Player is squelched, shouldn't hear
                return;

            string msg = !string.IsNullOrEmpty(args.Mobile.Region.Name) ? String.Format("{0} ({1}): {2}", args.Mobile.Name, args.Mobile.Region.Name, args.Speech) : String.Format("{0}: {1}", args.Mobile.Name, args.Speech);

            List<Mobile> remove = null;
            List<Mobile> mobs = new List<Mobile>();

            foreach (Mobile mob in m_HearAll.Keys)
                mobs.Add(mob);

            for (int i = 0; i < mobs.Count; i++)
            {
                if (mobs[i].NetState == null)
                {
                    if (remove == null)
                        remove = new List<Mobile>();
                    remove.Add(mobs[i]);
                }
                else
                {
                    //Don't display speech when you can actually see the player speech or if the privs are higher while hidden.
                    if (args.Mobile.InRange(mobs[i], 15) || (args.Mobile.AccessLevel > mobs[i].AccessLevel && args.Mobile.Hidden))
                        continue;
                    
                    bool filteron;
                    bool filterspeech = false;
                    m_HearAll.TryGetValue(mobs[i], out filteron);

                    if (filteron)
                    {
                        for (int j = 0; j < m_IgnoredSpeech.Length; ++j)
                        {
                            if (msg.ToLower().Contains(m_IgnoredSpeech[j]))
                                filterspeech = true;
                        }
                    }

                    if (!filterspeech)
                        mobs[i].SendMessage(args.Hue, msg);
                }
            }

            if (remove != null)
                for (int i = 0; i < remove.Count; i++)
                    m_HearAll.Remove(remove[i]);

            if (m_HearAll.Keys.Count == 0)
            {
                EventSink.Speech -= OnSpeech;
                EventSink.GuildChat -= OnGuildChat;
                EventSink.PartyChat -= OnPartyChat;
                EventSink.AllianceChat -= OnAllianceChat;
            }
        }

        private static void OnGuildChat(GuildChatEventArgs args)
        {
            string guild = args.Guild.Abbreviation == "none" ? args.Guild.Name : args.Guild.Abbreviation;
            string msg = !string.IsNullOrEmpty(args.Mobile.Region.Name) ? String.Format("Guild [{0}] [{1}] ({2}): {3}", guild, args.Mobile.Name, args.Mobile.Region.Name, args.Speech) : String.Format("Guild [{0}] [{1}]: {2}", guild, args.Mobile.Name, args.Speech);

            List<Mobile> remove = null;
            List<Mobile> mobs = new List<Mobile>();

            foreach (Mobile mob in m_HearAll.Keys)
                mobs.Add(mob);

            for (int i = 0; i < mobs.Count; i++)
            {
                if (mobs[i].NetState == null)
                {
                    if (remove == null)
                        remove = new List<Mobile>();
                    remove.Add(mobs[i]);
                }
                else
                {
                    //Don't display speech when you can actually see the player speech or if the privs are higher while hidden.
                    if (args.Mobile.InRange(mobs[i], 15) || (args.Mobile.AccessLevel > mobs[i].AccessLevel && args.Mobile.Hidden))
                        continue;

                    mobs[i].SendMessage(args.Mobile.SpeechHue, msg);
                }
            }

            if (remove != null)
                for (int i = 0; i < remove.Count; i++)
                    m_HearAll.Remove(remove[i]);

            if (m_HearAll.Keys.Count == 0)
            {
                EventSink.Speech -= OnSpeech;
                EventSink.GuildChat -= OnGuildChat;
                EventSink.PartyChat -= OnPartyChat;
                EventSink.AllianceChat -= OnAllianceChat;
            }
        }

        private static void OnPartyChat(PartyChatEventArgs args)
        {
            string msg;
            string to = args.To == null ? "" : args.To.Name;

            if (!string.IsNullOrEmpty(args.From.Region.Name))
                msg = string.IsNullOrEmpty(to) ? String.Format("Party [{0}] ({1}): {2}", args.From.Name, args.From.Region.Name, args.Speech) : String.Format("Party [{0} -> {1}] ({2}): {3}", args.From.Name, to, args.From.Region.Name, args.Speech);
            
            else
                msg = string.IsNullOrEmpty(to) ? String.Format("Party [{0}]: {1}", args.From.Name, args.Speech) : String.Format("Party [{0} -> {1}]: {2}", args.From.Name, to, args.Speech);

            List<Mobile> remove = null;
            List<Mobile> mobs = new List<Mobile>();

            foreach (Mobile mob in m_HearAll.Keys)
                mobs.Add(mob);

            for (int i = 0; i < mobs.Count; i++)
            {
                if (mobs[i].NetState == null)
                {
                    if (remove == null)
                        remove = new List<Mobile>();
                    remove.Add(mobs[i]);
                }
                else
                {
                    //Don't display speech when you can actually see the player speech or if the privs are higher while hidden.
                    if (args.From.InRange(mobs[i], 15) || (args.From.AccessLevel > mobs[i].AccessLevel && args.From.Hidden))
                        continue;

                    mobs[i].SendMessage(args.From.SpeechHue, msg);
                }
            }

            if (remove != null)
                for (int i = 0; i < remove.Count; i++)
                    m_HearAll.Remove(remove[i]);

            if (m_HearAll.Keys.Count == 0)
            {
                EventSink.Speech -= OnSpeech;
                EventSink.GuildChat -= OnGuildChat;
                EventSink.PartyChat -= OnPartyChat;
                EventSink.AllianceChat -= OnAllianceChat;
            }
        }

        private static void OnAllianceChat(AllianceChatEventArgs args)
        {
            string guild = args.Guild.Abbreviation == "none" ? args.Guild.Name : args.Guild.Abbreviation;
            string msg = !string.IsNullOrEmpty(args.Mobile.Region.Name) ? String.Format("Alliance [{0}] [{1}] ({2}): {3}", guild, args.Mobile.Name, args.Mobile.Region.Name, args.Speech) : String.Format("Alliance [{0}] [{1}]: {2}", guild, args.Mobile.Name, args.Speech);

            List<Mobile> remove = null;
            List<Mobile> mobs = new List<Mobile>();

            foreach (Mobile mob in m_HearAll.Keys)
                mobs.Add(mob);

            for (int i = 0; i < mobs.Count; i++)
            {
                if (mobs[i].NetState == null)
                {
                    if (remove == null)
                        remove = new List<Mobile>();
                    remove.Add(mobs[i]);
                }
                else
                {
                    //Don't display speech when you can actually see the player speech or if the privs are higher while hidden.
                    if (args.Mobile.InRange(mobs[i], 15) || (args.Mobile.AccessLevel > mobs[i].AccessLevel && args.Mobile.Hidden))
                        continue;

                    mobs[i].SendMessage(args.Mobile.SpeechHue, msg);
                }
            }

            if (remove != null)
                for (int i = 0; i < remove.Count; i++)
                    m_HearAll.Remove(remove[i]);

            if (m_HearAll.Keys.Count == 0)
            {
                EventSink.Speech -= OnSpeech;
                EventSink.GuildChat -= OnGuildChat;
                EventSink.PartyChat -= OnPartyChat;
                EventSink.AllianceChat -= OnAllianceChat;
            }
        }

        #endregion

        public static void Initialize()
        {
            CommandSystem.Register("HearAll", AccessLevel.GameMaster, HearAll_OnCommand);
        }

        [Usage("HearAll <filteron>")]
        [Description("Hear what everyone that is not on your screen says. Use <filteron> to turn on the automatic word filter for common macro words.")]
        private static void HearAll_OnCommand(CommandEventArgs args)
        {
            if (m_HearAll.ContainsKey(args.Mobile))
            {
                m_HearAll.Remove(args.Mobile);
                args.Mobile.SendMessage("Hear All disabled.");
                if (m_HearAll.Keys.Count == 0)
                {
                    EventSink.Speech -= OnSpeech;
                    EventSink.GuildChat -= OnGuildChat;
                    EventSink.PartyChat -= OnPartyChat;
                    EventSink.AllianceChat -= OnAllianceChat;
                }
            }
            else
            {
                if (m_HearAll.Keys.Count == 0)
                {
                    EventSink.Speech += OnSpeech;
                    EventSink.GuildChat += OnGuildChat;
                    EventSink.PartyChat += OnPartyChat;
                    EventSink.AllianceChat += OnAllianceChat;
                }
                if (args.Length == 1)
                {
                    if (args.GetString(0).ToLower() == "filteron")
                    {
                        m_HearAll.Add(args.Mobile, true);
                        args.Mobile.SendMessage("Hear All enabled with filter on, type .hearall again to disable it.");
                    }
                    else
                    {
                        m_HearAll.Add(args.Mobile, false);
                        args.Mobile.SendMessage("Hear All enabled with filter off, type .hearall again to disable it.");
                        args.Mobile.SendMessage("To enable Hear All with filter on, type '.hearall filteron'");
                    }
                }
                else
                {
                    m_HearAll.Add(args.Mobile, false);
                    args.Mobile.SendMessage("Hear All enabled with filter off, type .hearall again to disable it.");
                    args.Mobile.SendMessage("To enable Hear All with filter on, type '.hearall filteron'");
                }
            }
        }
    }
}*/