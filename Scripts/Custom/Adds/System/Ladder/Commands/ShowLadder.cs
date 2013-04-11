/*using Server.Ladder;

namespace Server.Commands
{
   public class Ladder
    {
        public static Mobile m_From;

        public static void Initialize()
        {
            CommandSystem.Register("Ladder", AccessLevel.Player, ShowLadder_OnCommand);
        }

        [Usage("Ladder")]
        [Description("Shows you the current status of our pvp ladder")]
       private static void ShowLadder_OnCommand(CommandEventArgs e)
        {
            if (MySQLConnection.Get1vs1Ladder == null)
                ComfirmGump.UpdateIngameLadder();        
  
            if (MySQLConnection.Get1vs1Ladder == null)
            {
                e.Mobile.SendAsciiMessage("Cannot establish connection, try again later.");
                return;
            }

            m_From = e.Mobile;
            m_From.SendGump(new ShowTotalLadderLadder(m_From));
        }
    }
  
}*/