using Server.Misc;
using Server.Mobiles;
using Server.Regions;
using Server.SkillHandlers;

namespace Server.Gumps
{
    public class FameKillSystem
    {
        public static void Initialize()
        {
            EventSink.PlayerDeath += EventSink_PlayerDeath;
        }

        public static void EventSink_PlayerDeath(PlayerDeathEventArgs e)
        {
            Mobile killed = e.Mobile;
            Mobile killer = e.Mobile.LastKiller;

            //Ignore non-players
            if (!(killed is PlayerMobile))
                return;

            //Ignore players in events
            if ((killed).IsInEvent || ((killer != null) && killer.IsInEvent))
                return;

            //Ignore players in a NoFameLoss region
            if (killed.Region is CustomRegion)
            {
                CustomRegion cR = (CustomRegion)killed.Region;
                if (cR.Controller.NoFameLoss)
                    return;
            }

            //Remove 1/10th of the fame when a player dies
            killed.Fame -= (killed.Fame / 10);

            if (killer == null || killer.Combatant != killed )
                return;

            if (!killer.InRange(killed.Location, 15))
                return;

            int n = Notoriety.Compute(killer, killed);

            bool innocent = (n == Notoriety.Innocent);
            bool faction = (n == Notoriety.Enemy || n == Notoriety.Ally);
            bool criminal = (n == Notoriety.Criminal || n == Notoriety.Murderer);

            int fameAward = killed.Fame/10;
            int karmaAward = -killed.Karma/10;

            if (innocent && karmaAward > 0)
                karmaAward = -(karmaAward);
            else if (criminal && karmaAward < 0)
                karmaAward = -(karmaAward);

            //Reward the killer karma only if he attacked first and it's not a faction kill
            if (!faction && (NotorietyHandlers.CheckAggressor(killed.Aggressors, killer) || NotorietyHandlers.CheckAggressed(killed.Aggressors, killer)))
                Titles.AwardKarma(killer, karmaAward, true);

            Titles.AwardFame(killer, fameAward, true);

            if (innocent)
            {
                ++killer.Kills;

                if (killer.Kills == 5)
                    killer.SendLocalizedMessage(502134); //You are now known as a murderer!
            }
            else if (Stealing.SuspendOnMurder && killer.Kills >= 1 && killer is PlayerMobile && ((PlayerMobile)killer).NpcGuild == NpcGuild.ThievesGuild)
                killer.SendLocalizedMessage(501562); // You have been suspended by the Thieves Guild.
        }
    }
}
