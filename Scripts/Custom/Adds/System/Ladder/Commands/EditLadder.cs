using Server.Ladder;
using Server.Mobiles;

namespace Server.Commands
{ 
    public class EditLadder 
    { 
        //private static int usercount;
        private static PlayerMobile pm;

        public static void Initialize() 
        {
            CommandSystem.Register("EditLadder", AccessLevel.Counselor, EditLadder_OnCommand); 
        } 

        [Usage( "EditLadder" )] 
        [Description( "Edit the PvP ladder." )]
        private static void EditLadder_OnCommand(CommandEventArgs e) 
        {
            pm = e.Mobile as PlayerMobile;

            if (pm != null && pm is PlayerMobile)
                ExecuteCommand();

        }
        private static void ExecuteCommand()
        {
            if (pm.LadderGump != null)
            {
                if (pm.LadderGump is InitiateEventGump)
                {
                    InitiateEventGump ieg = pm.LadderGump as InitiateEventGump;
                    pm.SendGump(new InitiateEventGump(pm, ieg.mobilesJoined, ieg.eventType, ieg.current_SWG));
                }
                else
                {
                    SelectWinnerGump swg = pm.LadderGump as SelectWinnerGump;
                    pm.SendGump(new SelectWinnerGump(pm, swg.IEG, swg.current_SWG));
                }

                pm.SendAsciiMessage("Loading saved event...");
            }
            else
                pm.SendGump(new InitiateEventGump(pm, null, null, null));
        }
   } 
} 
