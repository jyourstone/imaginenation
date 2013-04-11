using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Items;

namespace Server.Scripts.Custom.Adds.Commands
{
    public class Use
    {
        private static Dictionary<string, Type> m_SupportedTypes;
        private static string m_Usage = "You must specify the item to use. Type .UseHelp for a list with supported item types.";
        private static string m_UnknownType = "{0} is not a supported item type. Type .UseHelp for a list with supported item types.";
        private static string m_NotFound = "This item type was not found in your backpack: {0}";

        private enum MessageType
        {
            Usage,
            UnknownType,
            NotFound
        }

        public static void Initialize()
        {
            m_SupportedTypes = new Dictionary<string, Type>();

            //If you add more types here, please sort them alphabetically
            m_SupportedTypes["fsscroll"] = typeof(FlamestrikeScroll);
            m_SupportedTypes["ghscroll"] = typeof(GreaterHealScroll);
            m_SupportedTypes["greaterexplosionpot"] = typeof(GreaterExplosionPotion);
            m_SupportedTypes["greaterhealpot"] = typeof(GreaterHealPotion);
            m_SupportedTypes["greatermanapot"] = typeof(TotalManaPotion);
            m_SupportedTypes["invispot"] = typeof(InvisibilityPotion);
            m_SupportedTypes["lightningscroll"] = typeof(LightningScroll);
            m_SupportedTypes["manapot"] = typeof(ManaPotion);
            m_SupportedTypes["mrscroll"] = typeof(MagicReflectScroll);
            m_SupportedTypes["shrinkpot"] = typeof(ShrinkPotion);
            m_SupportedTypes["totalrefreshpot"] = typeof(TotalRefreshPotion);

            CommandSystem.Register("Use", AccessLevel.Player, Use_OnCommand);
            CommandSystem.Register("UseHelp", AccessLevel.Player, UseHelp_OnCommand);
        }

        [Usage("UseHelp")]
        [Description("Displays a list of the items that can be used with .Use")]
        private static void UseHelp_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendAsciiMessage("The supported items to use with .Use are:");
            foreach (KeyValuePair<string, Type> kvp in m_SupportedTypes)
                e.Mobile.SendAsciiMessage(kvp.Key);
        }

        [Usage("Use <string>")]
        [Description("Use a specified item")]
        private static void Use_OnCommand(CommandEventArgs e)
        {
            Mobile mob = e.Mobile; 
            if (e.Arguments.Length >= 1)
            {
                string key = e.Arguments[0];
                Type t;
                if (!m_SupportedTypes.TryGetValue(key.ToLower(), out t))
                {
                    SendMessage(mob, MessageType.UnknownType, key);
                    //SendMessage(mob, MessageType.Usage);
                }
                else 
                {
                    Item item = mob.Backpack.FindItemByType(t);
                    if(item != null)
                    {
                        mob.Use(item);
                    }
                    else
                    {
                        SendMessage(mob, MessageType.NotFound, key);
                    }
                }
            }
            else
            {
                SendMessage(mob, MessageType.Usage);
            }
        }

        private static void SendMessage(Mobile m, MessageType type, params string[] args)
        {
            string message = "";
            switch (type)
            {
                case MessageType.Usage:
                    message = m_Usage;
                    break;
                case MessageType.NotFound:
                    message = String.Format(m_NotFound, args[0]);
                    break;
                case MessageType.UnknownType:
                    message = String.Format(m_UnknownType, args[0]);
                    break;
            }
            m.SendMessage(message);
        }
    }
}
