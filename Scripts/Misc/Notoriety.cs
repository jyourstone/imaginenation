using System.Collections.Generic;
using Server.Custom.Games;
using Server.Factions;
using Server.Guilds;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Regions;
using Server.SkillHandlers;
//Bounty System Start
using Server.BountySystem;
//Bounty System End

namespace Server.Misc
{
	public class NotorietyHandlers
	{
		public static void Initialize()
		{
            Notoriety.Hues[Notoriety.Innocent] = 99;
            Notoriety.Hues[Notoriety.Ally] = 0x3F;
            Notoriety.Hues[Notoriety.CanBeAttacked] = 0x3B2;
            Notoriety.Hues[Notoriety.Criminal] = 0x3B2;
            Notoriety.Hues[Notoriety.Enemy] = 0x90;
            Notoriety.Hues[Notoriety.Murderer] = 33;
            Notoriety.Hues[Notoriety.Invulnerable] = 0x35;

			Notoriety.Handler = MobileNotoriety;

			Mobile.AllowBeneficialHandler = Mobile_AllowBeneficial;
			Mobile.AllowHarmfulHandler = Mobile_AllowHarmful;
		}

        public static int KILLS_FOR_MURDER = 5;

        public static int NPC_KARMA_RED = -800;
        public static int NPC_KARMA_GREY = 100;

        public static int PLAYER_KARMA_RED = -9000;
        public static int PLAYER_KARMA_GREY = -2500;

		private enum GuildStatus { None, Peaceful, Waring }

		private static GuildStatus GetGuildStatus( Mobile m )
		{
			if( m.Guild == null )
				return GuildStatus.None;
		    if( ((Guild)m.Guild).Enemies.Count == 0 && m.Guild.Type == GuildType.Regular )
		        return GuildStatus.Peaceful;

		    return GuildStatus.Waring;
		}

		private static bool CheckBeneficialStatus( GuildStatus from, GuildStatus target )
		{
			if( from == GuildStatus.Waring || target == GuildStatus.Waring )
				return false;

			return true;
		}

		public static bool Mobile_AllowBeneficial( Mobile from, Mobile target )
		{
			if( from == null || target == null || from.AccessLevel > AccessLevel.Player || target.AccessLevel > AccessLevel.Player )
				return true;

            Map map = from.Map;

			#region Factions
			Faction targetFaction = Faction.Find( target, true );

            if ((!Core.ML || map == Faction.Facet) && targetFaction != null)
			{
				if( Faction.Find( from, true ) != targetFaction )
					return false;
			}
			#endregion

            if (Custom.PvpToolkit.PvpCore.IsInDeathmatch(from) || Custom.PvpToolkit.PvpCore.IsInDeathmatch(target))
                return false;

			if( map != null && (map.Rules & MapRules.BeneficialRestrictions) == 0 )
				return true; // In felucca, anything goes

			if( !from.Player )
				return true; // NPCs have no restrictions

			if( target is BaseCreature && !((BaseCreature)target).Controlled )
				return true; // Players can heal uncontrolled mobiles

            if (from is PlayerMobile && !from.IsInEvent &&((PlayerMobile)from).Young && (!(target is PlayerMobile) || !((PlayerMobile)target).Young))
				return false; // Young players cannot perform beneficial actions towards older players unless in event

			Guild fromGuild = from.Guild as Guild;
			Guild targetGuild = target.Guild as Guild;

			if( fromGuild != null && targetGuild != null && (targetGuild == fromGuild || fromGuild.IsAlly( targetGuild )) )
				return true; // Guild members can be beneficial

			return CheckBeneficialStatus( GetGuildStatus( from ), GetGuildStatus( target ) );
		}

		public static bool Mobile_AllowHarmful( Mobile from, Mobile target )
		{
		    if( from == null || target == null || from.AccessLevel > AccessLevel.Player || target.AccessLevel > AccessLevel.Player )
				return true;

            CustomRegion cR = target.Region as CustomRegion;

            if (from.IsInEvent && target.IsInEvent)
                return true;

            if ((from.IsInEvent && !target.IsInEvent) && (cR == null || cR.Controller.HasAttackPenalty))
                return false;

            if ((!from.IsInEvent && target.IsInEvent) && (cR == null || cR.Controller.HasAttackPenalty))
                return false;

			//Map map = from.Map;

			//if( map != null && (map.Rules & MapRules.HarmfulRestrictions) == 0 )
			//	return true; // In felucca, anything goes

            BaseCreature bc = from as BaseCreature;

            if (!from.Player && !(bc != null && bc.GetMaster() != null && bc.GetMaster().AccessLevel == AccessLevel.Player))
			{
				if( !CheckAggressor( from.Aggressors, target ) && !CheckAggressed( from.Aggressed, target ) && target is PlayerMobile && ((PlayerMobile)target).CheckYoungProtection( from ) )
					return false;

				return true; // Uncontrolled NPCs are only restricted by the young system
			}
            /*
			Guild fromGuild = GetGuildFor( from.Guild as Guild, from );
			Guild targetGuild = GetGuildFor( target.Guild as Guild, target );

			if( fromGuild != null && targetGuild != null && (fromGuild == targetGuild || fromGuild.IsAlly( targetGuild ) || fromGuild.IsEnemy( targetGuild )) )
				return true; // Guild allies or enemies can be harmful
            */

            if (from is PlayerMobile && ((PlayerMobile)from).CheckYoungProtection(target) || (target is PlayerMobile && ((PlayerMobile)target).CheckYoungProtection(from)))
            {
                if (target is BaseCreature && (((BaseCreature) target).Controlled || (((BaseCreature) target).Summoned && from != ((BaseCreature) target).SummonMaster)))
                    return false; // Cannot harm other controlled mobiles

                if (target.Player)
                    return false; // Cannot harm other players

                if (!(target is BaseCreature && ((BaseCreature) target).InitialInnocent))
                {
                    if (Notoriety.Compute(from, target) == Notoriety.Innocent)
                        return false; // Cannot harm innocent mobiles
                }
            }

		    return true;
		}

		public static Guild GetGuildFor( Guild def, Mobile m )
		{
			Guild g = def;

			BaseCreature c = m as BaseCreature;

			if( c != null && c.Controlled && c.ControlMaster != null )
			{
				c.DisplayGuildTitle = false;

				if( c.Map != Map.Internal && (Core.AOS || Guild.NewGuildSystem || c.ControlOrder == OrderType.Attack || c.ControlOrder == OrderType.Guard) )
					g = (Guild)(c.Guild = c.ControlMaster.Guild);
				else if( c.Map == Map.Internal || c.ControlMaster.Guild == null )
					g = (Guild)(c.Guild = null);
			}

			return g;
		}

		public static int CorpseNotoriety( Mobile source, Corpse target )
		{
			if( target.AccessLevel > AccessLevel.Player )
				return Notoriety.CanBeAttacked;

            // MODIFICATIONS FOR Capture the Flag and Color Wars
            if (target.Owner != null && source is PlayerMobile && target.Owner is PlayerMobile)
            {
                PlayerMobile src = source as PlayerMobile;
                PlayerMobile trg = target.Owner as PlayerMobile;
                BaseTeamGame srcevent = src.CurrentEvent as BaseTeamGame;
                BaseTeamGame trgevent = trg.CurrentEvent as BaseTeamGame;
                if(srcevent != null && trgevent != null)
                {
                    BaseGameTeam srcteam = srcevent.GetTeam(source);
                    if (srcteam != null)
                    {
                        BaseGameTeam trgteam = srcevent.GetTeam(target.Owner);
                        if (trgteam != null)
                            return srcteam == trgteam ? Notoriety.Ally : Notoriety.Enemy;
                    }
                }
            }

		    //Is it really this simple...?
            if (target.Owner is PlayerMobile)
            {
                PlayerMobile pmTarget = (PlayerMobile)target.Owner;

                //Carved players turn grey
                if (target.Carved)
                    return Notoriety.CanBeAttacked;

                if (target.Kills >= KILLS_FOR_MURDER)
                    return Notoriety.Murderer;

                //Player mobile notoriety hierarchy
                if (pmTarget.Karma <= PLAYER_KARMA_RED)
                    return Notoriety.Murderer;
                if (target.Criminal)
                    return Notoriety.Criminal;
                if (pmTarget.Karma <= PLAYER_KARMA_GREY)
                    return Notoriety.CanBeAttacked;

                Guild sourceGuild = GetGuildFor(source.Guild as Guild, source);
                Guild targetGuild = GetGuildFor(target.Owner.Guild as Guild, target.Owner);

                if (sourceGuild != null && targetGuild != null)
                {
                    if (sourceGuild == targetGuild || sourceGuild.IsAlly(targetGuild))
                        return Notoriety.Ally;
                    if (sourceGuild.IsEnemy(targetGuild))
                        return Notoriety.Enemy;
                }

                Faction srcFaction = Faction.Find(source, true, true);
                Faction trgFaction = Faction.Find(target.Owner, true, true);

                if (srcFaction != null && trgFaction != null && srcFaction != trgFaction && source.Map == Faction.Facet)
                {
                    List<Mobile> secondList = target.Aggressors;

                    for (int i = 0; i < secondList.Count; ++i)
                    {
                        if (secondList[i] == source || secondList[i] is BaseFactionGuard)
                            return Notoriety.Enemy;
                    }
                }

                List<Mobile> list = target.Aggressors;

                for (int i = 0; i < list.Count; ++i)
                {
                    if (list[i] == source)
                        return Notoriety.CanBeAttacked;
                }

                return Notoriety.Innocent;
            }

		    return Notoriety.CanBeAttacked;

		    #region New style checking
            /*
            BaseCreature cretOwner = target.Owner as BaseCreature;

			if( cretOwner != null )
			{
                if (cretOwner.AlwaysMurderer || cretOwner.IsAnimatedDead)
                    return Notoriety.CanBeAttacked;
    
                if (cretOwner.Karma <= NPC_KARMA_RED || cretOwner.Criminal || cretOwner.Karma <= NPC_KARMA_GREY)
                    return Notoriety.CanBeAttacked;

				Guild sourceGuild = GetGuildFor( source.Guild as Guild, source );
				Guild targetGuild = GetGuildFor( target.Guild as Guild, target.Owner );

				if( sourceGuild != null && targetGuild != null )
				{
					if( sourceGuild == targetGuild || sourceGuild.IsAlly( targetGuild ) )
						return Notoriety.Ally;
					else if( sourceGuild.IsEnemy( targetGuild ) )
						return Notoriety.Enemy;
				}

				Faction srcFaction = Faction.Find( source, true, true );
				Faction trgFaction = Faction.Find( target.Owner, true, true );

				if( srcFaction != null && trgFaction != null && srcFaction != trgFaction && source.Map == Faction.Facet )
					return Notoriety.Enemy;

				if( CheckHouseFlag( source, target.Owner, target.Location, target.Map ) )
					return Notoriety.CanBeAttacked;

				int actual = Notoriety.CanBeAttacked;

				if( DateTime.Now >= (target.TimeOfDeath + Corpse.MonsterLootRightSacrifice) )
					return actual;

				Party sourceParty = Party.Get( source );

				List<Mobile> list = target.Aggressors;

				for( int i = 0; i < list.Count; ++i )
				{
					if( list[i] == source || (sourceParty != null && Party.Get( list[i] ) == sourceParty) )
						return actual;
				}

				return Notoriety.Innocent;
			}
			else
			{
                //Special player mobile cases
                if (target.Owner is PlayerMobile)
                {
                    PlayerMobile pmTarget = (PlayerMobile)target.Owner;

                    //Carved players turn grey
                    if (target.Carved)
                        return Notoriety.CanBeAttacked;

                    if (target.Kills >= KILLS_FOR_MURDER)
                        return Notoriety.Murderer;
            
                    //Player mobile notoriety hierarchy
                    if (pmTarget.Karma <= PLAYER_KARMA_RED)
                        return Notoriety.Murderer;
                    else if (target.Criminal)
                        return Notoriety.Criminal;
                    else if (pmTarget.Karma <= PLAYER_KARMA_GREY)
                        return Notoriety.CanBeAttacked;
                }
                else if (target.Owner is Mobile)//Special mobile cases
                {
                    Mobile mobileTarget = (Mobile)target.Owner;

                    if (target.Kills >= KILLS_FOR_MURDER)
                        return Notoriety.CanBeAttacked;
           
                    if (mobileTarget.Karma <= NPC_KARMA_RED || target.Criminal || mobileTarget.Karma <= NPC_KARMA_GREY)
                        return Notoriety.CanBeAttacked;
                }

                Guild sourceGuild = GetGuildFor(source.Guild as Guild, source);
                Guild targetGuild = GetGuildFor(target.Guild as Guild, target.Owner);

                if (sourceGuild != null && targetGuild != null)
                {
                    if (sourceGuild == targetGuild || sourceGuild.IsAlly(targetGuild))
                        return Notoriety.Ally;
                    else if (sourceGuild.IsEnemy(targetGuild))
                        return Notoriety.Enemy;
                }

                Faction srcFaction = Faction.Find(source, true, true);
                Faction trgFaction = Faction.Find(target.Owner, true, true);

                if (srcFaction != null && trgFaction != null && srcFaction != trgFaction && source.Map == Faction.Facet)
                {
                    List<Mobile> secondList = target.Aggressors;

                    for (int i = 0; i < secondList.Count; ++i)
                    {
                        if (secondList[i] == source || secondList[i] is BaseFactionGuard)
                            return Notoriety.Enemy;
                    }
                }

                if (target.Owner != null && target.Owner is BaseCreature && ((BaseCreature)target.Owner).AlwaysAttackable)
                    return Notoriety.CanBeAttacked;

                if (CheckHouseFlag(source, target.Owner, target.Location, target.Map))
                    return Notoriety.CanBeAttacked;

                if (!(target.Owner is PlayerMobile) && !IsPet(target.Owner as BaseCreature))
                    return Notoriety.CanBeAttacked;

                List<Mobile> list = target.Aggressors;

                for (int i = 0; i < list.Count; ++i)
                {
                    if (list[i] == source)
                        return Notoriety.CanBeAttacked;
                }

                return Notoriety.Innocent;
			}
            */
            #endregion
        }

		public static int MobileNotoriety( Mobile source, Mobile target )
		{
		    CustomRegion cR = target.Region as CustomRegion;

            #region Event Notorieties

            //Check event prop first for better use of resources
            if ((source.IsInEvent && !target.IsInEvent) && (cR == null || cR.Controller.HasAttackPenalty))
                return Notoriety.Invulnerable;
            if ((!source.IsInEvent && target.IsInEvent) && (cR == null || cR.Controller.HasAttackPenalty))
                return Notoriety.Invulnerable;

            //Deathmatch
            if (Custom.PvpToolkit.PvpCore.IsInDeathmatch(source) && Custom.PvpToolkit.PvpCore.IsInDeathmatch(target))
            {
                Custom.PvpToolkit.DMatch.Items.DMStone dm1 = Custom.PvpToolkit.PvpCore.GetPlayerStone(source);
                Custom.PvpToolkit.DMatch.Items.DMStone dm2 = Custom.PvpToolkit.PvpCore.GetPlayerStone(target);

                return dm1 == dm2 ? Notoriety.Enemy : Notoriety.Invulnerable;
            }

            //Tournament
            if (Custom.PvpToolkit.Tournament.TournamentCore.IsInTournament(source) && Custom.PvpToolkit.Tournament.TournamentCore.IsInTournament(target))
		    {
                Custom.PvpToolkit.Tournament.TournamentStone stone1 = Custom.PvpToolkit.Tournament.TournamentCore.GetPlayerStone(source);
                Custom.PvpToolkit.Tournament.TournamentStone stone2 = Custom.PvpToolkit.Tournament.TournamentCore.GetPlayerStone(target);

                return stone1 == stone2 ? Notoriety.Enemy : Notoriety.Invulnerable;
		    }

		    //Capture the Flag / Color Wars / Double Dom games
            if (source is PlayerMobile && target is PlayerMobile)
            {
                PlayerMobile src = source as PlayerMobile;
                PlayerMobile trg = target as PlayerMobile;
                BaseTeamGame srcevent = src.CurrentEvent as BaseTeamGame;
                BaseTeamGame trgevent = trg.CurrentEvent as BaseTeamGame;
                if (srcevent != null && trgevent != null)
                {
                    BaseGameTeam srcteam = srcevent.GetTeam(source);
                    if (srcteam != null)
                    {
                        BaseGameTeam trgteam = srcevent.GetTeam(target);
                        if (trgteam != null)
                            return srcteam == trgteam ? Notoriety.Ally : Notoriety.Enemy;
                    }
                }
            }

            #endregion

            if (Core.AOS && (target.Blessed || (target is BaseVendor && ((BaseVendor)target).IsInvulnerable) || target is PlayerVendor || target is TownCrier))
                return Notoriety.Invulnerable;

            if (target is BaseCreature && (((BaseCreature)target).AlwaysMurderer || ((BaseCreature)target).IsAnimatedDead))
                return Notoriety.Murderer;

            if ( target is PlayerMobile && (((PlayerMobile)target).AlwaysMurderer))
                return Notoriety.Murderer;

            //All mobiles have same murder rules.
            if (target.Kills >= KILLS_FOR_MURDER)
                return Notoriety.Murderer;

            //Target should be karma red before guild notoriety
            if (target is PlayerMobile)
            {
                if (target.Karma <= PLAYER_KARMA_RED)
                    return Notoriety.Murderer;
            }

		    Guild sourceGuild = GetGuildFor(source.Guild as Guild, source);
            Guild targetGuild = GetGuildFor(target.Guild as Guild, target);

            if (sourceGuild != null && targetGuild != null)
            {
                if (sourceGuild == targetGuild || sourceGuild.IsAlly(targetGuild) || (sourceGuild.Type !=
                    GuildType.Regular && sourceGuild.Type == targetGuild.Type))
                    return Notoriety.Ally;
                if (sourceGuild.IsEnemy(targetGuild))
                    return Notoriety.Enemy;
            }

		    if (cR != null && !cR.Controller.HasAttackPenalty)
                return Notoriety.CanBeAttacked;

            if (target.AccessLevel > AccessLevel.Player)
                return Notoriety.CanBeAttacked;

            if (target is PlayerMobile) //Player mobile notoriety hierarchy
            {
                if (target.Criminal)
                    return Notoriety.Criminal;
                if (target.Karma <= PLAYER_KARMA_GREY)
                    return Notoriety.CanBeAttacked;
            }
            else //Mobile notoriety hierarchy
            {
                if (target.Karma <= NPC_KARMA_RED)
                    return Notoriety.Murderer;
                if (target.Criminal)
                    return Notoriety.Criminal;
                if (target.Karma <= NPC_KARMA_GREY)
                    return Notoriety.CanBeAttacked;
            }

			if( source.Player && !target.Player && source is PlayerMobile && target is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)target;

                Mobile master = bc.GetMaster();

                if (master != null && master.AccessLevel > AccessLevel.Player)
                    return Notoriety.CanBeAttacked;

				if( !bc.Summoned && !bc.Controlled && ((PlayerMobile)source).EnemyOfOneType == target.GetType() )
					return Notoriety.Enemy;

                if (bc.ControlMaster == source)
                    return Notoriety.CanBeAttacked;

                master = bc.ControlMaster;

                if (Core.ML && master != null)
                {
                    if (source == master && CheckAggressor(target.Aggressors, source))
                        return Notoriety.CanBeAttacked;

                    return MobileNotoriety(source, master);
                }
			}


			Faction srcFaction = Faction.Find( source, true, true );
			Faction trgFaction = Faction.Find( target, true, true );

			if( srcFaction != null && trgFaction != null && srcFaction != trgFaction && source.Map == Faction.Facet )
				return Notoriety.Enemy;

            if (Stealing.ClassicMode && target is PlayerMobile && ((PlayerMobile)target).PermaFlags.Contains(source))
                return Notoriety.CanBeAttacked;

            if (target is BaseCreature && ((BaseCreature)target).AlwaysAttackable)
                return Notoriety.CanBeAttacked;

            if (!IsGuardCandidate(target) && (target is BaseGuard))
                return Notoriety.Innocent;

            //Maka - Repo update, might mess stuff upp
            //Taran - Yeah it did, I had to uncomment it :P
            //if (!(target is BaseCreature && ((BaseCreature)target).InitialInnocent))
            //{
            //  if( !target.Body.IsHuman && !target.Body.IsGhost && !IsPet( target as BaseCreature ) && !TransformationSpellHelper.UnderTransformation( target ) && !AnimalForm.UnderTransformation( target ) )
            //      return Notoriety.CanBeAttacked;
            //}

            if (CheckAggressor(source.Aggressors, target))
                return Notoriety.CanBeAttacked;

            if (CheckAggressed(source.Aggressed, target))
                return Notoriety.CanBeAttacked;

			if( target is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)target;

                if (bc.Controlled && bc.ControlOrder == OrderType.Guard && bc.ControlTarget == source)
                    return Notoriety.CanBeAttacked;
			}

			if( source is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)source;

				Mobile master = bc.GetMaster();
                if (master != null && CheckAggressor(master.Aggressors, target))
                    return Notoriety.CanBeAttacked;
			}

            //bounty system
            if (BountyBoard.Attackable(source, target))
                return Notoriety.CanBeAttacked;
            //end bounty system

			return Notoriety.Innocent;
		}

		public static bool CheckHouseFlag( Mobile from, Mobile m, Point3D p, Map map )
		{
			BaseHouse house = BaseHouse.FindHouseAt( p, map, 16 );

			if( house == null || house.Public || !house.IsFriend( from ) )
				return false;

			if( m != null && house.IsFriend( m ) )
				return false;

			BaseCreature c = m as BaseCreature;

			if( c != null && !c.Deleted && c.Controlled && c.ControlMaster != null )
				return !house.IsFriend( c.ControlMaster );

			return true;
		}

		public static bool IsPet( BaseCreature c )
		{
			return (c != null && c.Controlled);
		}

        public static bool IsGuardCandidate(Mobile m)
        {
            if (m == null)
                return false;

            if (!m.Alive || m.Blessed)
                return false;

            if (m.Kills >= KILLS_FOR_MURDER)
                return true;

            if (m is PlayerMobile)
            {
                if ( m.Karma <= PLAYER_KARMA_RED || m.Criminal )
                    return true;
            }
            else if ( m.Karma <= NPC_KARMA_RED || m.Criminal )
                return true;

            if (m is BaseCreature && ((BaseCreature)m).AlwaysMurderer)
                return true;

            if (m is PlayerMobile && ((PlayerMobile)m).AlwaysMurderer)
                return true;

            return false;
        }

		public static bool IsSummoned( BaseCreature c )
		{
			return (c != null && /*c.Controlled &&*/ c.Summoned);
		}

		public static bool CheckAggressor( List<AggressorInfo> list, Mobile target )
		{
			for( int i = 0; i < list.Count; ++i )
				if( list[i].Attacker == target )
					return true;

			return false;
		}

		public static bool CheckAggressed( List<AggressorInfo> list, Mobile target )
		{
			for( int i = 0; i < list.Count; ++i )
			{
				AggressorInfo info = list[i];

				if( !info.CriminalAggression && info.Defender == target )
					return true;
			}

			return false;
		}
	}
}