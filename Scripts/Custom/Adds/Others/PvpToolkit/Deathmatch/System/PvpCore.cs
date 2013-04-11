using System.Collections.Generic;
using Server.Commands;
using Server.Custom.PvpToolkit.DMatch.Items;
using Server.Custom.PvpToolkit.Gumps;
using Server.Mobiles;
using System;

namespace Server.Custom.PvpToolkit
{
    public class PvpCore
    {
        public static string DeathmatchVersion { get { return "1.01"; } }

        private static List<DMStone> m_DMStones = new List<DMStone>();

        public static List<DMStone> DMStones { get { return m_DMStones; } set { m_DMStones = value; } }

        private static int AmountOfRunningDeathMatchGames = 0;

        public static void Initialize()
        {
            OnLoad();
        }

        public static void AddRunningDM()
        {
            if (AmountOfRunningDeathMatchGames < 0) //This can be negative when server is shut down or crashes when a DM is running
                AmountOfRunningDeathMatchGames = 0;

            if (++AmountOfRunningDeathMatchGames == 1)
                EventSink.PlayerDeath += EventSink_PlayerDeath;
        }

        public static void RemoveRunningDM()
        {
            if (--AmountOfRunningDeathMatchGames == 0)
                EventSink.PlayerDeath -= EventSink_PlayerDeath;

            if (AmountOfRunningDeathMatchGames < 0)
                AmountOfRunningDeathMatchGames = 0;
        }

        public static bool TryJoinDM(Mobile from)
        {
            return TryJoinDM(from, FindDMStone());
        }

        public static bool TryJoinDM(Mobile from, DMStone dmStone)
        {
            if (Factions.Sigil.ExistsOn(from))
                from.SendLocalizedMessage(1061632); // You can't do that while carrying the sigil.
            else if (IsInDeathmatch(from))
                from.SendMessage("You are already in a deathmatch. Say \"i wish to leave the deathmatch\" to leave.");
            else if (from.IsInEvent)
                from.SendMessage("You cannot join a tournament while in an event");
            else if (from.Spell != null)
                from.SendLocalizedMessage(1049616); // You are too busy to do that at the moment.
            else if (!from.Alive)
                from.SendMessage("You cannot join a deathmatch while dead.");
            else if (from.IsBodyMod && !from.Body.IsHuman)
                from.SendMessage("You cannot join a deathmatch while polymorphed.");
            else if (DMStones.Count < 1)
                from.SendMessage("No deathmatch system exists on the server.");
            else if (from.AccessLevel > AccessLevel.Player)
                from.SendMessage("You cannot join a deathmatch as staff");
            else
                return AllowDMJoin(from, dmStone);

            return false;
        }

        public static void TryLeaveDM(Mobile from, bool kicked)
        {
            if (IsInDeathmatch(from))
            {
                DMStone s = GetPlayerStone(from);
                from.Frozen = false;
                if (!from.Alive)
                    from.Resurrect();
                if (s != null)
                {
                    if (kicked)
                        s.RemovePlayer(from, true);
                    else
                        s.RemovePlayer(from, false);
                }
            }
        }

        public static void SendMessage(List<Mobile> mobiles, string message)
        {
            SendMessage(mobiles, message, true);
        }

        public static void SendMessage(List<Mobile> mobiles, string message, bool toStaff)
        {
            for (int i = 0; i < mobiles.Count; i++)
                mobiles[i].SendMessage(38, message);

            if (toStaff)
                CommandHandlers.BroadcastMessage(AccessLevel.Counselor, 38, string.Format("[Deathmatch] {0}", message));
        }

        public static void TryShowScoreBoard(Mobile from)
        {
            if (IsInDeathmatch(from))
            {
                DMStone s = GetPlayerStone(from);

                if (s != null)
                    s.ShowScore(from);
            }
        }

        private static void AllowDMJoin( Mobile m )
        {
            AllowDMJoin(m, FindDMStone());
        }

        private static bool AllowDMJoin(Mobile from, DMStone dmStone)
        {
            if (dmStone != null)
            {
                if (dmStone.UseSphereRules)
                {
                    if (dmStone.EventSupplier != null)
                    {
                        dmStone.AddPlayer(from);
                        return true;
                    }
                    
                    from.SendAsciiMessage("There is no event supplier connected to the death match");
                }
                else
                {
                    from.SendGump(new PvpAcceptGump(dmStone));
                    return true;
                }
            }
            else
                from.SendMessage("Either a deathmatch has not been started or is full and not accepting players.");

            return false;
        }

        public static DMStone FindDMStone()
        {
            foreach (DMStone dmStone in DMStones)
                if (dmStone != null && dmStone.Started && dmStone.AcceptingContestants)
                    return dmStone;

            return null;
        }

        private static void OnLoad()
        {
            
        }

        private static void EventSink_PlayerDeath( PlayerDeathEventArgs e )
        {
            Mobile m = e.Mobile;

            if( m == null )
                return;

            if( IsInDeathmatch( m ) )
            {
                DMStone stone = GetPlayerStone( m );

                if( stone != null )
                    stone.HandleDeath( m );
            }
            
        }

        public static DMStone GetPlayerStone( Mobile m )
        {
 	        foreach( DMStone stone in DMStones )
                if( stone != null && stone.Contestants.Contains( m ) )
                    return stone;

            return null;
        }

        public static bool IsInDeathmatch( Mobile m )
        {
            foreach( DMStone stone in DMStones )
                if( stone != null && stone.Contestants.Contains( m ) )
                    return true;
            
            return false;
        }

        public static Layer[] EquipmentLayers = new Layer[]
		{
			Layer.Cloak,
			Layer.Bracelet,
			Layer.Ring,
			Layer.Shirt,
			Layer.Pants,
			Layer.InnerLegs,
			Layer.Shoes,
			Layer.Arms,
			Layer.InnerTorso,
			Layer.MiddleTorso,
			Layer.OuterLegs,
			Layer.Neck,
			Layer.Waist,
			Layer.Gloves,
			Layer.OuterTorso,
			Layer.OneHanded,
			Layer.TwoHanded,
			Layer.FacialHair,
			Layer.Hair,
			Layer.Helm
		}; 
    }
}