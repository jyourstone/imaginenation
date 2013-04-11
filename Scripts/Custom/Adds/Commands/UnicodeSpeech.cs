using Server.Mobiles;

namespace Server.Commands
{
    #region

    

    #endregion

    public class UnicodeSpeechCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("Unicode", AccessLevel.Player, UnicodeSpeech_OnCommand);
        }

        [Usage("Unicode")]
        [Description("Enable or disable unicode speech")]
        private static void UnicodeSpeech_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile) e.Mobile;
                pm.UseUnicodeSpeech = !pm.UseUnicodeSpeech;

                string currentSpeech, oldSpeech;

                currentSpeech = pm.UseUnicodeSpeech ? "UNICODE" : "ASCII";
                oldSpeech = pm.UseUnicodeSpeech ? "ASCII" : "UNICODE";

                pm.SendAsciiMessage(string.Format("You turned on {0} speech, use .unicode to switch back to {1}.", currentSpeech, oldSpeech));
            }
        }
    }
}