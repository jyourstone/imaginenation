using Server.Commands;
using Server.Commands.Generic;
using Server.Mobiles;
using Server.Network;

namespace Server.Scripts.Custom.Adds.Commands
{
    #region

    

    #endregion

    public class RefreshCommand : BaseCommand
    {
        public RefreshCommand()
        {
            AccessLevel = AccessLevel.Counselor;
            Supports = CommandSupport.AllMobiles;
            Commands = new[] {"Refresh", "R"};
            ObjectTypes = ObjectTypes.Mobiles;
        }

        public static void Initialize()
        {
            TargetCommands.Register(new RefreshCommand());
        }

        public override void Execute(CommandEventArgs e, object obj)
        {
            Mobile mob = (Mobile) obj;

            if (mob.IsDeadBondedPet)
            {
                BaseCreature bc = mob as BaseCreature;

                if (bc != null)
                    bc.ResurrectPet();
            }
            else if (!mob.Alive && mob is PlayerMobile)
            {
                ((PlayerMobile) mob).ForceResurrect();
                CommandLogging.WriteLine(e.Mobile, "Refreshing and resurrecting " + mob.Name);
            }
            else if (!mob.Alive)
            {
                mob.Resurrect();
                CommandLogging.WriteLine(e.Mobile, "Refreshing and resurrecting " + mob.Name);
            }

            CommandLogging.WriteLine(e.Mobile, "Refreshing but not resurrecting) " + mob.Name);

            mob.PublicOverheadMessage(MessageType.Regular, mob.SpeechHue, true, "I've been refreshed.");

            mob.Hits = mob.HitsMax;
            mob.Stam = mob.StamMax;
            mob.Mana = mob.ManaMax;
            mob.CurePoison(mob);
        }
    }
}