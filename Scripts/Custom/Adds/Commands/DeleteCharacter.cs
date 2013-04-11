using System.Collections.Generic;
using Server.Commands;
using Server.Commands.Generic;
using Server.Mobiles;
using Server.Multis;

namespace Server.Scripts.Custom.Adds.Commands
{
    #region

    

    #endregion

    public class DeleteCharacter : BaseCommand
    {
        public DeleteCharacter()
        {
            AccessLevel = AccessLevel.Administrator;
            Supports = CommandSupport.AllMobiles;
            Commands = new[] {"DeleteCharacter"};
            ObjectTypes = ObjectTypes.Mobiles;
            Usage = "DeleteCharacter";
            Description = "Deletes character from account.";
        }

        public static void Initialize()
        {
            TargetCommands.Register(new DeleteCharacter());
        }

        public override void Execute(CommandEventArgs e, object obj)
        {
            Mobile from = e.Mobile;
            Mobile target = (Mobile) obj;

            if (target != null)
                DoDelete(from, target);
        }

        public static void DoDelete(Mobile from, Mobile target)
        {
            if (!(target is PlayerMobile))
                return;

            if (target.NetState != null)
            {
                from.SendAsciiMessage("The target character must be logged out!");
                return;
            }

            List<BaseHouse> houses = BaseHouse.GetHouses(target);

            for (int h = 0; h < houses.Count; ++h)
                houses[h].Delete();

            CommandLogging.WriteLine(from, "{0} {1} deleting {2} ({3})  from account {4} ", from.AccessLevel, CommandLogging.Format(from), target.Name, target.Serial, target.Account.Username);

            target.Delete();
        }
    }
}