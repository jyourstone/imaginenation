using System;
using System.Collections;
using Server.Commands;
using Server.Mobiles;

namespace Server.Scripts.Custom.Adds.Commands
{
    public class DisplayLocationCommand
    {
        private static readonly Hashtable m_NextUseTime = new Hashtable();

        public static void Initialize()
        {
            CommandSystem.Register("DisplayLocation", AccessLevel.Player, Execute);
            CommandSystem.Register("DL", AccessLevel.Player, Execute);
        }

        [Usage("DisplayLocation [on/off]")]
        [Aliases("DL [on/off]")]
        [Description("When enabled you will be able to see the location of other players that have this command enabled as well by typing .ShowPlayers or .SP -- NOTE: Can only be changed once every 24 hours!")]
        private static void Execute(CommandEventArgs e)
        {
            PlayerMobile pm = (PlayerMobile) e.Mobile;

            if (pm == null)
                return;

            DateTime dt = DateTime.MinValue;

            if (e.ArgString.ToLower() != "on" && e.ArgString.ToLower() != "off")
            {
                pm.SendAsciiMessage("You currently have DisplayLocation " + (pm.ShowArriveMsg ? "enabled" : "disabled"));
                pm.SendAsciiMessage("To enable/disable it type .DisplayLocation on/off");
                return;
            }

            if (m_NextUseTime[pm.Account] != null)
                dt = (DateTime) m_NextUseTime[pm.Account];

            if ( DateTime.Now < dt)
                pm.SendAsciiMessage("There is a 24 hour delay between toggling");
            else
            {
                if (pm.ShowArriveMsg)
                {
                    if (e.ArgString.ToLower() == "off")
                    {
                        pm.SendAsciiMessage("DISABLED");
                        pm.SendAsciiMessage("You will no longer be able to see the location of other players and vice versa");
                        m_NextUseTime[pm.Account] = DateTime.Now + TimeSpan.FromHours(24);
                        pm.ShowArriveMsg = false;
                    }
                    else
                        pm.SendAsciiMessage("You already have DisplayLocation enabled, if you want to disable it type .DisplayLocation off");
                }
                else
                {
                    if (e.ArgString.ToLower() == "on")
                    {
                        pm.SendAsciiMessage("ENABLED");
                        pm.SendAsciiMessage("You can now see the location of other players by typing .ShowPlayers or .SP and vice versa");
                        m_NextUseTime[pm.Account] = DateTime.Now + TimeSpan.FromHours(24);
                        pm.ShowArriveMsg = true;
                    }
                    else
                        pm.SendAsciiMessage("You already have DisplayLocation disabled, if you want to enable it type .DisplayLocation on");
                }
            }
        }
    }
}
