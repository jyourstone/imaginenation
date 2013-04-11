using System;
using Server.Custom.SkillBoost;
using Server.Gumps;
using Server.Mobiles;
using Server.Regions;

namespace Server.Misc
{
	public class SkillCheck
	{
		//Used to calculate the base repetition quantity.
		public const int TotalRepetitionParts = 78; //How many times may we use the same location/target for gain

		//private const bool AntiMacroCode = false; //Change this to false to disable anti-macro code

		public const int Allowance = 5; //How many times may we use the same location/target for gain
		private const int LocationSize = 1; //The size of eeach location, make this smaller so players dont have to move as far
		public static TimeSpan AntiMacroExpire = TimeSpan.FromMinutes( 5.0 ); //How long do we remember targets/locations?

        /*
		private static bool[] UseAntiMacro = new bool[]{ // true if this skill uses the anti-macro code, false if it does not
			false, // Alchemy = 0,
			false, // Anatomy = 1,
			false, // AnimalLore = 2,
			false, // ItemID = 3,
			false, // ArmsLore = 4,
			false, // Parry = 5,
			false, // Begging = 6,
			false, // Blacksmith = 7,
			false, // Fletching = 8,
			false, // Peacemaking = 9,
			false, // Camping = 10,
			false, // Carpentry = 11,
			false, // Cartography = 12,
			false, // Cooking = 13,
			false, // DetectHidden = 14,
			false, // Discordance = 15,
			false, // EvalInt = 16,
			false, // Healing = 17,
			false, // Fishing = 18,
			false, // Forensics = 19,
			false, // Herding = 20,
			false, // Hiding = 21,
			false, // Provocation = 22,
			false, // Inscribe = 23,
			false, // Lockpicking = 24,
			false, // Magery = 25,
			false, // MagicResist = 26,
			false, // Tactics = 27,
			false, // Snooping = 28,
			false, // Musicianship = 29,
			false, // Poisoning = 30,
			false, // Archery = 31,
			false, // SpiritSpeak = 32,
			false, // Stealing = 33,
			false, // Tailoring = 34,
			false, // AnimalTaming = 35,
			false, // TasteID = 36,
			false, // Tinkering = 37,
			false, // Tracking = 38,
			false, // Veterinary = 39,
			false, // Swords = 40,
			false, // Macing = 41,
			false, // Fencing = 42,
			false, // Wrestling = 43,
			false, // Lumberjacking = 44,
			false, // Mining = 45,
			false, // Meditation = 46,
			false, // Stealth = 47,
			false, // RemoveTrap = 48,
			false, // Necromancy = 49,
			false, // Focus = 50,
			false, // Chivalry = 51
		};
        */
		private static readonly TimeSpan m_StatGainDelay = TimeSpan.FromMinutes( 1.0 );

        //****************************A guide to decide the GainFactor****************************//
        //****    First off, since this is repetetive skillgain, which means that you gain    ****//
        //****    on the amount of items you make, there is a few things to keep in mind.     ****//
        //****    What item will the player do? Most players will do the item that will       ****//
        //****    provide them with the cheapest gain, some players might do the item         ****//
        //****    that will provide them with the fastest gain. Keep this in mind when        ****//
        //****    you set the gain rates. Think about speed(fast and slow macroing), cost     ****//
        //****    and not to mention the obvious, gain. Make sure that they are not           ****//
		//****    "macroing" anything they can harvest money on.                              ****//

		//****    Now for you to set the skill gain, you only have to keep in mind the things ****//
		//****    above and set the amount of repetitions you want the skill to take to GM    ****//
		//****    from 40%. 40% has been selected cause you can buy skills to 35 and and      ****//
        //****    there is no point in adepting the algo for the lower levels.                ****//



		public static void Initialize()
		{
            // Begin mod to enable XmlSpawner skill triggering
            Mobile.SkillCheckLocationHandler = XmlSpawnerSkillCheck.Mobile_SkillCheckLocation;
            Mobile.SkillCheckDirectLocationHandler = XmlSpawnerSkillCheck.Mobile_SkillCheckDirectLocation;

            Mobile.SkillCheckTargetHandler = XmlSpawnerSkillCheck.Mobile_SkillCheckTarget;
            Mobile.SkillCheckDirectTargetHandler = XmlSpawnerSkillCheck.Mobile_SkillCheckDirectTarget;
            // End mod to enable XmlSpawner skill triggering

            //Set the amount of reps the skill will require from 40-100 down below.
            //55555 is the default value.

            //Shade- Skill time estimates are no longer accurate
            SkillInfo.Table[0].GainFactor = 127000; // Alchemy = 0, 
			//Macro type: People will create nightsights, delay 8 seconds.
			//Time estimate: 12 days 24/7.

            SkillInfo.Table[1].GainFactor = 39200; // Anatomy = 1, 
			//Macro type: Target yourself, delay 1 second
			//Time estimate: 11 hours.

            SkillInfo.Table[2].GainFactor = 39200; // AnimalLore = 2, 
			//Macro type: Target animal, delay 1 second
			//Time estimate: 11 hours.

            SkillInfo.Table[3].GainFactor = 39200; // ItemID = 3, 
			//Macro type: Target weapon/armor, delay 1 second
			//Time estimate: 11 hours.

            SkillInfo.Table[4].GainFactor = 39200; // ArmsLore = 4,
			//Macro type: Target weapon/armor, delay 1 second
			//Time estimate: 11 hours.

            SkillInfo.Table[5].GainFactor = 56800; // Parry = 5, 
			//Macro type: Sparring with someone that wrestles you, delay 2 seconds
			//Time estimate: 1½ days 24/7.

            SkillInfo.Table[6].GainFactor = 39200; // Begging = 6, 
			//Macro type: On mobs, delay 3 seconds
			//Time estimate: 1½ days 24/7

            SkillInfo.Table[7].GainFactor = 223428; // Blacksmith = 7, 
			//Macro type: Make daggers, delay about 3.5 seconds in average
			//Time estimate: 9-10 days 24/7

            SkillInfo.Table[8].GainFactor = 144040; // Fletching = 8, 
			//Macro type: Make kindlings, delay about 3.5 seconds in average
			//Time estimate: 6 days 24/7

            SkillInfo.Table[9].GainFactor = 34500; // Peacemaking = 9, 
			//Macro type: Target self, delay 11 seconds when failing, 5½ seconds when success. Estimate 50/50
			//Time estimate: 3-4 days 24/7

            SkillInfo.Table[10].GainFactor = 56400; // Camping = 10,
			//Macro type: Light kindlings, delay 1 second
			//Time estimate: 16 hours

            SkillInfo.Table[11].GainFactor = 248114; // Carpentry = 11,
			//Macro type: Clubs or anything that requires 3 logs up to 70, then blank scrolls that requires 1 log, delay about 3 seconds average
			//Time estimate: 9 days 24/7          

	        SkillInfo.Table[12].GainFactor = 152000; // Cartography = 12, 
			//Macro type: Create local map, average delay of about 3 seconds
			//Time estimate: 6 days 24/7

            SkillInfo.Table[13].GainFactor = 191840; // Cooking = 13,
			//Macro type: Use raw ribs and cook them,  delay 2.5 seconds
			//Time estimate: 6 days 24/7

            SkillInfo.Table[14].GainFactor = 76400; // DetectHidden = 14, 
			//Macro type: Just press skill, delay 1 second
			//Time estimate: 1 day 24/7

            SkillInfo.Table[15].GainFactor = 60600; // Discordance = 15, 
			//Macro type: Target creature, delay 1 second
			//Time estimate: 17 hours

            SkillInfo.Table[16].GainFactor = 39200; // EvalInt = 16,
			//Macro type: Target yourself, delay 1 second
			//Time estimate: 11 hours

            SkillInfo.Table[17].GainFactor = 40200; // Healing = 17, 
			//Macro type: Hurt yourself somehow, delay 3 seconds
			//Time estimate: 1½ days 24/7

            SkillInfo.Table[18].GainFactor = 107600; // Fishing = 18, 
			//Macro type: Just fish in water, delay 3 seconds
			//Time estimate: 4 days 24/7

            SkillInfo.Table[19].GainFactor = 49200; // Forensics = 19, 
			//Macro type: Target corpse, delay 1 second
			//Time estimate: 15 hours

            SkillInfo.Table[20].GainFactor = 149600; // Herding = 20,
			//Macro type: Herd any tamable animal, delay 1 second
			//Time estimate: 2 days 24/7

            SkillInfo.Table[21].GainFactor = 48640; // Hiding = 21,
			//Macro type: Just repeat, delay 2.5 seconds
			//Time estimate: 1½ days 24/7

            SkillInfo.Table[22].GainFactor = 28000; // Provocation = 22,
			//Macro type: Target two creatures, delay 11 seconds when succeding, 5½ seconds when fail. Estimate 50/50
			//Time estimate: 2 days 24/7

            SkillInfo.Table[23].GainFactor = 164400; // Inscribe = 23,
			//Macro type: People will do just make the easiest scroll, and with meditation delay is calculated to about 7 seconds
			//Time estimate: 14 days 24/7.

            SkillInfo.Table[24].GainFactor = 49900; // Lockpicking = 24, 
			//Macro type: Repeat lockpicking, if needed relock the chest, delay 3.5 seconds, estimate 4
			//Time estimate: 2½ days 24/7

            SkillInfo.Table[25].GainFactor = 29250; // Magery = 25, 
			//Macro type: Repeat casting easy spell such as nightsight, delay with meditation calculated to about 7 seconds
			//Time estimate: 2½ days 24/7

            SkillInfo.Table[26].GainFactor = 95800; // MagicResist = 26, 
			//Macro type: Run in firefield healing yourself, train magery etc, estimated delay ½-1 second
			//Time estimate: 1 day 24/7

            SkillInfo.Table[27].GainFactor = 55800; // Tactics = 27, 
			//Macro type: Wrestle someone, delay 2 seconds
			//Time estimate: 1½ days 24/7.

            SkillInfo.Table[28].GainFactor = 50840; // Snooping = 28, 
			//Macro type: Open another mob/players backpack, delay 2.5 seconds
			//Time estimate: 1½ days 24/7.

            SkillInfo.Table[29].GainFactor = 15525; // Musicianship = 29, 
			//Macro type: Play instrument, repeat. Delay 7 seconds
			//Time estimate: 1½ days 24/7.

            SkillInfo.Table[30].GainFactor = 16280; // Poisoning = 30 
			//Macro type: Poison weapons with poison bottles. Delay 10 seconds
			//Time estimate: 2 days 24/7

            SkillInfo.Table[31].GainFactor = 36560; // Archery = 31
			//Macro type: Shoot with bow on monsters/friends. Delay 2.5 seconds
			//Time estimate: 1 day 24/7

            SkillInfo.Table[32].GainFactor = 30560; // SpiritSpeak = 32 
			//Macro type: Just repeat, delay 2.5 seconds
			//Time estimate: 1 day 24/7

            SkillInfo.Table[33].GainFactor = 116400; // Stealing = 33
            //Macro type: Just repeat, delay 4 seconds
            //Time estimate: 5-6 days 24/7

            SkillInfo.Table[34].GainFactor = 178000; // Tailoring = 34 
			//Macro type: Create bandanas, repeat. Delay about 4 seconds in average
			//Time estimate: 8-9 days 24/7

            SkillInfo.Table[35].GainFactor = 49800; // AnimalTaming = 35 
			//Macro type: Tame any animal, release, repeat. Delay about 16 seconds in average
			//Time estimate: 9-10 days 24/7

            SkillInfo.Table[36].GainFactor = 40200; // TasteID = 36 
			//Macro type: Target food, delay 1 second
			//Time estimate: 11 hours

            SkillInfo.Table[37].GainFactor = 111200; // Tinkering = 37
			//Macro type: Make clockparts, delay about 4 seconds in average
			//Time estimate: 5-6 days 24/7

            SkillInfo.Table[38].GainFactor = 93600; // Tracking = 38 
			//Macro type: Keep tracking, gain all the time. Delay 1 second
			//Time estimate: 1 day 24/7

            SkillInfo.Table[39].GainFactor = 59200; // Veterinary = 39 
			//Macro type: Hurt a mob, then heal it, delay 5 seconds
			//Time estimate: 3-4 days 24/7

            SkillInfo.Table[40].GainFactor = 28400; // Swords = 40 
			//Macro type: Hit something with a butcher's knife, delay 2 seconds
			//Time estimate: 16 hours

            SkillInfo.Table[41].GainFactor = 28600; // Macing = 41 
			//Macro type: Hit something with a club, delay 3 seconds
			//Time estimate: 16 hours

            SkillInfo.Table[42].GainFactor = 28400; // Fencing = 42 
			//Macro type: Hit something with a dagger, delay 2 seconds
			//Time estimate: 16 hours

            SkillInfo.Table[43].GainFactor = 26400; // Wrestling = 43
			//Macro type: Wrestle someone, delay 2 seconds
			//Time estimate: 15 hours

            SkillInfo.Table[44].GainFactor = 70200; // Lumberjacking = 44
			//Macro type: Chop chop wood in the foooorest, delay average about 4 seconds
			//Time estimate: 3-4 days 24/7

            SkillInfo.Table[45].GainFactor = 52500; // Mining = 45
			//Macro type: Mine all day long, delay average about 5 seconds
			//Time estimate: 3 days 24/7

            SkillInfo.Table[46].GainFactor = 8575; // Meditation = 46
			//Macro type: Meditation while macroing magery, delay 5 seconds (although longer since you have to use a spell as well)
			//Time estimate: ½-1 day 24/7

            SkillInfo.Table[47].GainFactor = 72020; // Stealth = 47 
			//Macro type: Use stealth to hide, delay 2.5 seconds
			//Time estimate: 2 days 24/7

            SkillInfo.Table[48].GainFactor = 39280; // RemoveTrap = 48 
			//Macro type: Use skill on a high level trapped chest, delay 5 seconds
			//Time estimate: 2 days 24/7

            SkillInfo.Table[49].GainFactor = 55555; // Necromancy = 49
			//Not used

            SkillInfo.Table[50].GainFactor = 55555; // Focus = 50
			//Not used

            SkillInfo.Table[51].GainFactor = 55555; // Chivalry = 51
			//Not used

            //Compute this now so that we do not have to do it on each skill check
            for (int i = 0; i < SkillInfo.Table.Length; i++)
                SkillInfo.Table[i].GainFactor /= TotalRepetitionParts;
		}

        public static bool Mobile_SkillCheckLocation(Mobile from, SkillName skillName, double minSkill, double maxSkill)
		{
			Skill skill = from.Skills[skillName];

			if( skill == null )
				return false;

			double value = skill.Value;

			if( value < minSkill )
				return false; // Too difficult

			double chance = ( value - minSkill ) / ( maxSkill - minSkill );

			Point2D loc = new Point2D( from.Location.X / LocationSize, from.Location.Y / LocationSize );
			return CheckSkill( from, skill, loc, chance );
		}

		public static bool Mobile_SkillCheckDirectLocation( Mobile from, SkillName skillName, double chance )
		{
			Skill skill = from.Skills[skillName];

			if( skill == null )
				return false;

			if( chance < 0.0 )
				return false; // Too difficult

			Point2D loc = new Point2D( from.Location.X / LocationSize, from.Location.Y / LocationSize );
			return CheckSkill( from, skill, loc, chance );
		}

        public static bool CheckSkill(Mobile from, Skill skill, object amObj, double chance)
		{
			if (from.Skills.Cap == 0)
				return false;

			bool success = (chance >= Utility.RandomDouble());

            //Repetition multiplier.
            //Calculate how many 5-percent steps the player is over 40%.
            //EG: 50%-54.99% would be 2 steps over 40%, 45%-49.99% would be 1.
            double multiplier = ((int)skill.Base / 5) - 9;

            if (multiplier >= 18)			//At 125% or higer. Multiplier never gets bigger than 18.
                multiplier = 18;
            else if (multiplier <= -5)		//At 0%-20%, 5% of the amount required at 40 and up.
                multiplier = 0.05;
            else if (multiplier <= -4)		//At 25%-30%, 10% of the amount required at 40 and up.
                multiplier = 0.1;
            else if (multiplier <= -3)		//At 30%-35%, 25% of the amount required at 40 and up.
                multiplier = 0.25;
            else if (multiplier <= -2)		//At 35%-40%. 50% of the amount required at 40 and up.
                multiplier = 0.5;
            else if (multiplier <= -1)		//At 40%-45%. 70% of the amount required at 40 and up.
                multiplier = 0.7;
            else if (multiplier <= 0)		//At 45%-50%. 90% of the amount required at 40 and up.
                multiplier = 0.9;

            //The chance to gain at your current skill level
            //Divides 50 tenths (5.0 skill%) with the repetitions required to reach next level (not from your current skill, but from the start of your level).
            double gc = 50 / (skill.Info.GainFactor * multiplier);

			if (from is BaseCreature && ((BaseCreature)from).Controlled)
				gc *= 2;

            if (SkillBoost.Running) //Taran: Skillgain boost has been enabled
                gc *= Values.SkillBoostValues[skill.Info.SkillID];

			if (from.Alive && ((gc >= Utility.RandomDouble() && AllowGain(from, skill, amObj)) || skill.Base < 10.0))
				Gain(from, skill);

			return success;
		}

		public static bool Mobile_SkillCheckTarget( Mobile from, SkillName skillName, object target, double minSkill, double maxSkill )
		{
			Skill skill = from.Skills[skillName];

            if (skill == null)
                return false;

            double value = skill.Value;

            double chance = (value - minSkill) / (maxSkill - minSkill);

            /* Taran: Not needed anymore, all skills should gain even while failed.
             * If you fail without a delay a check should be made in the script itself
            if (skillName == SkillName.AnimalTaming) //Taran - Always gain even if you fail to tame
                return CheckSkill(from, skill, target, chance);

            if (skillName == SkillName.RemoveTrap) //Taran - Always gain when using remove trap
                return CheckSkill(from, skill, target, chance);

            if (skillName == SkillName.Stealing) //Taran - Always gain when using stealing
                return CheckSkill(from, skill, target, chance);

			if( value < minSkill )
				return false; // Too difficult
            */

			return CheckSkill( from, skill, target, chance );
		}

		public static bool Mobile_SkillCheckDirectTarget( Mobile from, SkillName skillName, object target, double chance )
		{
			Skill skill = from.Skills[skillName];

			if( skill == null )
				return false;

			if( chance < 0.0 )
				return false; // Too difficult

			return CheckSkill( from, skill, target, chance );
		}

		private static bool AllowGain( Mobile from, Skill skill, object obj )
		{
            /*
            if (Core.AOS && Faction.InSkillLoss(from))	//Changed some time between the introduction of AoS and SE.
                return false;

            if (AntiMacroCode && from is PlayerMobile && UseAntiMacro[skill.Info.SkillID])
                return ((PlayerMobile)from).AntiMacroCheck(skill, obj);
            else
            */

            if (from.IsInEvent)
            {
                from.SendAsciiMessage("You cannot gain skills while in an event");
                return false;
            }

            CustomRegion cR = from.Region as CustomRegion;

		    if (cR != null && !cR.Controller.AllowSkillGain)
            {
                from.SendAsciiMessage("You cannot gain skills here");
                return false;
            }

		    return true;
		}

        public enum Stat
        {
            Str,
            Dex,
            Int
        }

		public static void Gain( Mobile from, Skill skill )
		{
			/* Rob - added to save skill before gain - GM reward gump */
			double skillBase = skill.Base;
			/* end Rob */
			if( from.Region is Jail )
				return;

			if( from is BaseCreature && ( (BaseCreature)from ).IsDeadPet )
				return;

			if( skill.SkillName == SkillName.Focus && from is BaseCreature )
				return;

			if( skill.Base < skill.Cap && skill.Lock == SkillLock.Up )
			{
				int toGain = 1;

				if( skill.Base <= 10.0 )
					toGain = Utility.Random( 4 ) + 1;

				Skills skills = from.Skills;

                if ((skills.Total / skills.Cap) >= Utility.RandomDouble()) //( skills.Total >= skills.Cap )
                {
                    for (int i = 0; i < skills.Length; ++i)
                    {
                        Skill toLower = skills[i];

                        if (toLower != skill && toLower.Lock == SkillLock.Down && toLower.BaseFixedPoint >= toGain)
                        {
                            toLower.BaseFixedPoint -= toGain;
                            break;
                        }
                    }
                }
                /*
                #region Scroll of Alacrity
                PlayerMobile pm = from as PlayerMobile;

                if (from is PlayerMobile)
                    if (pm != null && skill.SkillName == pm.AcceleratedSkill && pm.AcceleratedStart > DateTime.Now)
                        toGain *= Utility.RandomMinMax(2, 5);
                #endregion
                */
                if ( ( skills.Total + toGain ) <= skills.Cap )
					skill.BaseFixedPoint += toGain;
			}

			if( skill.Lock == SkillLock.Up )
			{
				SkillInfo info = skill.Info;

				//*****EDIT THIS LINE TO MAKE STATS GAIN FASTER, NUMBER MUST BE BETWEEN 0.00 and 1.00, THIS IS ADDED TO THE CHANCE TO GAIN IN A STAT********//
				double StatGainBonus = .60; //Extra chance to gain in stats. Left at 0 would be default runuo gains.
				//******************************************************************************************************************************************//

				if( from.StrLock == StatLockType.Up && ( ( info.StrGain / 33.3 ) + StatGainBonus ) > Utility.RandomDouble() )
				{
					if( info.StrGain != 0 )
						GainStat( from, Stat.Str );
				}
				else if( from.DexLock == StatLockType.Up && ( ( info.DexGain / 33.3 ) + StatGainBonus ) > Utility.RandomDouble() )
				{
					if( info.DexGain != 0 )
						GainStat( from, Stat.Dex );
				}
				else if( from.IntLock == StatLockType.Up && ( ( info.IntGain / 33.3 ) + StatGainBonus ) > Utility.RandomDouble() )
					if( info.IntGain != 0 )
						GainStat( from, Stat.Int );
				//following line used to show chance to gain stats ingame
				//from.SendMessage( "Str: {0} Dex: {1} Int: {2}",((info.StrGain / 33.3) + StatGainBonus),((info.DexGain / 33.3) + StatGainBonus),((info.IntGain / 33.3) + StatGainBonus) );
			}

			/* Rob - added for GM reward gump */
			if( skill.Base == 100.0 && skillBase < 100.0 )
				InvokeRewardGump( from, skill.Info );
			/* end Rob */
		}

		public static bool CanLower( Mobile from, Stat stat )
		{
			switch( stat )
			{
				case Stat.Str:
					return ( from.StrLock == StatLockType.Down && from.RawStr > 50 );
				case Stat.Dex:
					return ( from.DexLock == StatLockType.Down && from.RawDex > 50 );
				case Stat.Int:
					return ( from.IntLock == StatLockType.Down && from.RawInt > 50 );
			}

			return false;
		}

		public static bool CanRaise( Mobile from, Stat stat )
		{
			if( !( from is BaseCreature && ( (BaseCreature)from ).Controlled ) )
				if( from.RawStatTotal >= from.StatCap )
					return false;

			switch( stat )
			{
				case Stat.Str:
					return ( from.StrLock == StatLockType.Up && from.RawStr < 120 );
				case Stat.Dex:
					return ( from.DexLock == StatLockType.Up && from.RawDex < 120 );
				case Stat.Int:
					return ( from.IntLock == StatLockType.Up && from.RawInt < 120 );
			}

			return false;
		}

		public static void IncreaseStat( Mobile from, Stat stat, bool atrophy )
		{
			atrophy = atrophy || ( from.RawStatTotal >= from.StatCap );

			switch( stat )
			{
				case Stat.Str:
				{
					if( atrophy )
						if( CanLower( from, Stat.Dex ) && ( from.RawDex < from.RawInt || !CanLower( from, Stat.Int ) ) )
							--from.RawDex;
						else if( CanLower( from, Stat.Int ) )
							--from.RawInt;

					if( CanRaise( from, Stat.Str ) )
						++from.RawStr;

					break;
				}
				case Stat.Dex:
				{
					if( atrophy )
						if( CanLower( from, Stat.Str ) && ( from.RawStr < from.RawInt || !CanLower( from, Stat.Int ) ) )
							--from.RawStr;
						else if( CanLower( from, Stat.Int ) )
							--from.RawInt;

					if( CanRaise( from, Stat.Dex ) )
						++from.RawDex;

					break;
				}
				case Stat.Int:
				{
					if( atrophy )
						if( CanLower( from, Stat.Str ) && ( from.RawStr < from.RawDex || !CanLower( from, Stat.Dex ) ) )
							--from.RawStr;
						else if( CanLower( from, Stat.Dex ) )
							--from.RawDex;

					if( CanRaise( from, Stat.Int ) )
						++from.RawInt;

					break;
				}
			}
		}

		public static void GainStat( Mobile from, Stat stat )
		{
			if( ( from.LastStatGain + m_StatGainDelay ) >= DateTime.Now )
				return;

			from.LastStatGain = DateTime.Now;

			bool atrophy = ( ( from.RawStatTotal / (double)from.StatCap ) >= Utility.RandomDouble() );

			IncreaseStat( from, stat, atrophy );
		}

		private static void InvokeRewardGump( Mobile from, SkillInfo skillInfo )
		{
            Skills skills = from.Skills;
			switch( skillInfo.SkillID )
			{
				case 0: // Alchemy
				case 7: // Blacksmithy
				case 8: // Bowcraft/fletching
				case 23: // Inscription
				case 25: // Magery
				case 34: // Tailoring
				case 35: // Animal Taming
                case 44: // Lumberjack
                case 45: // Mining
					from.SendGump( new RewardGump( skillInfo, 0, "Name" ) );
					break;
                //Bardic skills, player gets to choose item
                case 9:
                case 15:
                case 22:
                case 29:
                    if (skills.Peacemaking.Base == 100.0 && skills.Discordance.Base == 100.0 && skills.Provocation.Base == 100.0 && skills.Musicianship.Base == 100.0)
			            from.SendGump(new ChooseBardRewardGump());
			        break;
                case 11: // Carpentry
                    from.SendGump(new ChooseCarpRewardGump());
                    break;
                 //"Thieving" skills
                case 21:
                case 28:
                case 33:
                case 47:
                    if (skills.Hiding.Base == 100.0 && skills.Stealth.Base == 100.0 && skills.Snooping.Base == 100.0 && skills.Stealing.Base == 100.0)
                        from.SendGump(new RewardGump(skillInfo, 0, "Name"));
			        break;
			    default:
					break;
			}
		}
	}
}