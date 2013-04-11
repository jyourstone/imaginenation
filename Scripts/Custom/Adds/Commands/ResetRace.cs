using Server.Commands;
using Server.Commands.Generic;
using Server.Mobiles;

namespace Server.Scripts.Custom.Adds.Commands
{
    #region

    

    #endregion

    public class ResetRace : BaseCommand
    {
        public ResetRace()
        {
            AccessLevel = AccessLevel.GameMaster;
            Supports = CommandSupport.AllMobiles;
            Commands = new[] {"ResetRace"};
            ObjectTypes = ObjectTypes.Mobiles;
            Usage = "ResetRace";
            Description = "Resets players hair and hue to original values";
        }

        public static void Initialize()
        {
            TargetCommands.Register(new ResetRace());
        }

        public override void Execute(CommandEventArgs e, object obj)
        {
            Mobile from = e.Mobile;
            Mobile target = obj as Mobile;

            if (target != null)
            {
                DoResetRace(target);
            }
        }

        public static void DoResetRace(Mobile target)
        {
            if (!(target is PlayerMobile))
                return;

            PlayerMobile pm = target as PlayerMobile;
            if (pm.HasCustomRace)
            {
                if (pm.OriginalHairHue == 0 && pm.OriginalHairItemID == 0 && pm.OriginalHue == 0)
                {
                    pm.Hue = 1002;
                    pm.HairItemID = pm.Female ? 8252 : 8251;
                    pm.HairHue = 0;
                    pm.HasCustomRace = false;
                }
                else
                {
                    pm.HairItemID = pm.OriginalHairItemID;
                    pm.HairHue = pm.OriginalHairHue;
                    pm.Hue = pm.OriginalHue;
                    pm.HasCustomRace = false;
                }
            }
        }
    }
}