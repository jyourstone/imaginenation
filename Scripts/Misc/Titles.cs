using System;
using System.Text;
using Server.Engines.CannedEvil;
using Server.Mobiles;
using Server.Regions;

namespace Server.Misc
{
	public class Titles
	{
		public const int MinFame = 0;
		public const int MaxFame = 10000;

		public static void AwardFame( Mobile m, int offset, bool message )
		{
            //No fame award when in a NoFameLoss region
            if (m.Region is CustomRegion)
            {
                CustomRegion cR = (CustomRegion)m.Region;
                if (cR.Controller.NoFameLoss)
                    return;
            }

			if ( offset > 0 )
			{
				if ( m.Fame >= MaxFame )
					return;

				if ( offset < 0 )
					offset = 0;
			}
			else if ( offset < 0 )
			{
				if ( m.Fame <= MinFame )
					return;

				if ( offset > 0 )
					offset = 0;
			}

			if ( (m.Fame + offset) > MaxFame )
				offset = MaxFame - m.Fame;
			else if ( (m.Fame + offset) < MinFame )
				offset = MinFame - m.Fame;

			m.Fame += offset;

			if ( message )
			{
				if ( offset > 29 )
					m.SendMessage( "You have gained huge amounts of fame" );
				else if ( offset > 24 )
					m.SendMessage( "You have gained large amounts of fame" );
				else if ( offset > 19 )
					m.SendMessage( "You have gained alot of fame" );
				else if ( offset > 14 )
					m.SendMessage( "You have gained a moderate amount of fame" );
				else if ( offset > 9 )
					m.SendMessage( "You have gained a little fame" );
				else if ( offset > 4 )
					m.SendMessage( "You have gained a small amount of fame" );
				else if ( offset > 0 )
					m.SendMessage( "You have gained a bit of fame" );
				else if ( offset < -29 )
					m.SendMessage( "You have lost huge amounts of fame" );
				else if ( offset < -24 )
					m.SendMessage( "You have lost large amounts of fame" );
				else if ( offset < -19 )
					m.SendMessage( "You have lost alot of fame" );
				else if ( offset < -14 )
					m.SendMessage( "You have lost a moderate amount of fame" );
				else if ( offset < -9 )
					m.SendMessage( "You have lost a little fame" );
				else if ( offset < -4 )
					m.SendMessage( "You have lost a small amount of fame" );
				else if ( offset < 0 )
					m.SendMessage( "You have lost a bit of fame" );
			}
		}

		public const int MinKarma = -10000;
		public const int MaxKarma =  10000;

		public static void AwardKarma( Mobile m, int offset, bool message )
		{
            //No karma award when in a NoFameLoss region
            if (m.Region is CustomRegion)
            {
                CustomRegion cR = (CustomRegion)m.Region;
                if (cR.Controller.NoFameLoss)
                    return;
            }

			if ( offset > 0 )
			{
                if (m is PlayerMobile && ((PlayerMobile)m).KarmaLocked)
                    return;

				if ( m.Karma >= MaxKarma )
					return;
			}
			else if ( offset < 0 )
			{
				if ( m.Karma <= MinKarma )
					return;
			}

			if ( (m.Karma + offset) > MaxKarma )
				offset = MaxKarma - m.Karma;
			else if ( (m.Karma + offset) < MinKarma )
				offset = MinKarma - m.Karma;

			m.Karma += offset;

			if ( message )
			{
				if ( offset > 29 )
					m.SendMessage( "You have gained huge amounts of karma" );
				else if ( offset > 24 )
					m.SendMessage( "You have gained large amounts of karma" );
				else if ( offset > 19 )
					m.SendMessage( "You have gained alot of karma" );
				else if ( offset > 14 )
					m.SendMessage( "You have gained a moderate amount of karma" );
				else if ( offset > 9 )
					m.SendMessage( "You have gained a little karma" );
				else if ( offset > 4 )
					m.SendMessage( "You have gained a small amount of karma" );
				else if ( offset > 0 )
					m.SendMessage( "You have gained a bit of karma" );
				else if ( offset < -29 )
					m.SendMessage( "You have lost huge amounts of karma" );
				else if ( offset < -24 )
					m.SendMessage( "You have lost large amounts of karma" );
				else if ( offset < -19 )
					m.SendMessage( "You have lost alot of karma" );
				else if ( offset < -14 )
					m.SendMessage( "You have lost a moderate amount of karma" );
				else if ( offset < -9 )
					m.SendMessage( "You have lost a little karma" );
				else if ( offset < -4 )
					m.SendMessage( "You have lost a small amount of karma" );
				else if ( offset < 0 )
					m.SendMessage( "You have lost a bit of karma" );
			}

            //if ( !Core.AOS && wasPositiveKarma && m.Karma < 0 && m is PlayerMobile && !((PlayerMobile)m).KarmaLocked )
            //{
            //    ((PlayerMobile)m).KarmaLocked = true;
            //    m.SendLocalizedMessage( 1042511, "", 0x22 ); // Karma is locked.  A mantra spoken at a shrine will unlock it again.
            //}
		}

        public static string[] HarrowerTitles = new string[] { "Spite", "Opponent", "Hunter", "Venom", "Executioner", "Annihilator", "Champion", "Assailant", "Purifier", "Nullifier" };

		public static string ComputeTitle( Mobile beholder, Mobile beheld )
		{
			StringBuilder title = new StringBuilder();

			int fame = beheld.Fame;
			int karma = beheld.Karma;

            if (beheld.AccessLevel > AccessLevel.Player)
            {
                string staffTitle;

                if (beheld.AccessLevel == AccessLevel.GameMaster)
                    staffTitle = "Game Master";
                else
                    staffTitle = beheld.AccessLevel.ToString();

                title.AppendFormat("{0} {1}", staffTitle, beheld.Name);
            }
			else if ( beheld.Kills >= 5 )
				title.AppendFormat( beheld.Fame >= 10000 ? "The Murderer {1} {0}" : "The Murderer {0}", beheld.Name, beheld.Female ? "Lady" : "Lord" );
            else if (beheld.Criminal)
                title.AppendFormat(beheld.Fame >= 10000 ? "The Criminal {1} {0}" : "The Criminal {0}", beheld.Name, beheld.Female ? "Lady" : "Lord");
			else
			{
				for ( int i = 0; i < m_FameEntries.Length; ++i )
				{
					FameEntry fe = m_FameEntries[i];

					if ( fame <= fe.m_Fame || i == (m_FameEntries.Length - 1) )
					{
						KarmaEntry[] karmaEntries = fe.m_Karma;

						for ( int j = 0; j < karmaEntries.Length; ++j )
						{
							KarmaEntry ke = karmaEntries[j];

							if ( karma <= ke.m_Karma || j == (karmaEntries.Length - 1) )
							{
								title.AppendFormat( ke.m_Title, beheld.Name, beheld.Female ? "Lady" : "Lord" );
								break;
							}
						}

						break;
					}
				}
            }

            if (beheld.Guild != null && !beheld.Guild.Disbanded && !string.IsNullOrEmpty(beheld.Guild.Abbreviation))
            {
                if  (beheld.DisplayGuildTitle)
                    title.AppendFormat( " [{0}]", beheld.Guild.Abbreviation );
            }

			if( beheld is PlayerMobile && ((PlayerMobile)beheld).DisplayChampionTitle )
			{
				PlayerMobile.ChampionTitleInfo info = ((PlayerMobile)beheld).ChampionTitles;

				if( info.Harrower > 0 )
					title.AppendFormat( ": {0} of Evil", HarrowerTitles[Math.Min( HarrowerTitles.Length, info.Harrower )-1] );
				else
				{
					int highestValue = 0, highestType = 0;
					for( int i = 0; i < ChampionSpawnInfo.Table.Length; i++ )
					{
						int v = info.GetValue( i );

						if( v > highestValue )
						{
							highestValue = v;
							highestType = i;
						}
					}

					int offset = 0;
					if( highestValue > 800 )
						offset = 3;
					else if( highestValue > 300 )
						offset = (highestValue/300);

					if( offset > 0 )
					{
						ChampionSpawnInfo champInfo = ChampionSpawnInfo.GetInfo( (ChampionSpawnType)highestType );
						title.AppendFormat( ": {0} of the {1}", champInfo.LevelNames[Math.Min( offset, champInfo.LevelNames.Length ) -1], champInfo.Name );
					}
				}
			}

			string customTitle = beheld.Title;

			if ( customTitle != null && (customTitle = customTitle.Trim()).Length > 0 )
				title.AppendFormat( " {0}", customTitle );
            else if (beheld.Guild != null && !beheld.Guild.Disbanded && beheld.DisplayGuildTitle && !string.IsNullOrEmpty(beheld.GuildTitle) )
                    title.AppendFormat(", {0}", beheld.GuildTitle);
            else if (beheld.Player && (beheld.AccessLevel == AccessLevel.Player || beholder == beheld))
            {
                string skillTitle = GetSkillTitle(beheld);

                if (skillTitle != null)
                {
                    title.Append(", ").Append(skillTitle);
                }
            }

			return title.ToString();
		}

        public static string GetSkillTitle(Mobile mob)
        {
            Skill highest = GetHighestSkill(mob);// beheld.Skills.Highest;

            if (highest != null && highest.BaseFixedPoint >= 300)
            {
                string skillLevel = GetSkillLevel(highest);
                string skillTitle = highest.Info.Title;

                if (mob.Female && skillTitle.EndsWith("man"))
                    skillTitle = skillTitle.Substring(0, skillTitle.Length - 3) + "woman";

                return String.Concat(skillLevel, " ", skillTitle);
            }

            return null;
        }

		private static Skill GetHighestSkill( Mobile m )
		{
			Skills skills = m.Skills;

			/*if ( !Core.AOS )
				return skills.Highest;*/

			Skill highest = null;

			for ( int i = 0; i < m.Skills.Length; ++i )
			{
				Skill check = m.Skills[i];

				if ( highest == null || check.BaseFixedPoint > highest.BaseFixedPoint )
					highest = check;
				else if ( highest != null && highest.Lock != SkillLock.Up && check.Lock == SkillLock.Up && check.BaseFixedPoint == highest.BaseFixedPoint )
					highest = check;
			}

			return highest;
		}

		private static readonly string[,] m_Levels = new string[,]
			{
				{ "Neophyte",		"Neophyte",		"Neophyte"		},
				{ "Novice",			"Novice",		"Novice"		},
				{ "Apprentice",		"Apprentice",	"Apprentice"	},
				{ "Journeyman",		"Journeyman",	"Journeyman"	},
				{ "Expert",			"Expert",		"Expert"		},
				{ "Adept",			"Adept",		"Adept"			},
				{ "Master",			"Master",		"Master"		},
				{ "Grandmaster",	"Grandmaster",	"Grandmaster"	},
				{ "Elder",			"Tatsujin",		"Shinobi"		},
				{ "Legendary",		"Kengo",		"Ka-ge"			}
			};

		private static string GetSkillLevel( Skill skill )
		{
			return m_Levels[GetTableIndex( skill ), GetTableType( skill )];
		}

		private static int GetTableType( Skill skill )
		{
			switch ( skill.SkillName )
			{
				default: return 0;
				case SkillName.Bushido: return 1;
				case SkillName.Ninjitsu: return 2;
			}
		}

		private static int GetTableIndex( Skill skill )
		{
			int fp = Math.Min( skill.BaseFixedPoint, 1200 );

			return (fp - 300) / 100;
		}

		private static readonly FameEntry[] m_FameEntries = new FameEntry[]
			{
				new FameEntry( 1249, new KarmaEntry[]
				{
					new KarmaEntry( -10000, "The Outcast {0}" ),
					new KarmaEntry( -5000, "The Despicable {0}" ),
					new KarmaEntry( -2500, "The Scoundrel {0}" ),
					new KarmaEntry( -1250, "The Unsavory {0}" ),
					new KarmaEntry( -625, "The Rude {0}" ),
					new KarmaEntry( 624, "{0}" ),
					new KarmaEntry( 1249, "The Fair {0}" ),
					new KarmaEntry( 2499, "The Kind {0}" ),
					new KarmaEntry( 4999, "The Good {0}" ),
					new KarmaEntry( 9999, "The Honest {0}" ),
					new KarmaEntry( 10000, "The Trustworthy {0}" )
				} ),
				new FameEntry( 2499, new KarmaEntry[]
				{
					new KarmaEntry( -10000, "The Wretched {0}" ),
					new KarmaEntry( -5000, "The Dastardly {0}" ),
					new KarmaEntry( -2500, "The Malicious {0}" ),
					new KarmaEntry( -1250, "The Dishonorable {0}" ),
					new KarmaEntry( -625, "The Disreputable {0}" ),
					new KarmaEntry( 624, "The Notable {0}" ),
					new KarmaEntry( 1249, "The Upstanding {0}" ),
					new KarmaEntry( 2499, "The Respectable {0}" ),
					new KarmaEntry( 4999, "The Honorable {0}" ),
					new KarmaEntry( 9999, "The Commendable {0}" ),
					new KarmaEntry( 10000, "The Estimable {0}" )
				} ),
				new FameEntry( 4999, new KarmaEntry[]
				{
					new KarmaEntry( -10000, "The Nefarious {0}" ),
					new KarmaEntry( -5000, "The Wicked {0}" ),
					new KarmaEntry( -2500, "The Vile {0}" ),
					new KarmaEntry( -1250, "The Ignoble {0}" ),
					new KarmaEntry( -625, "The Notorious {0}" ),
					new KarmaEntry( 624, "The Prominent {0}" ),
					new KarmaEntry( 1249, "The Reputable {0}" ),
					new KarmaEntry( 2499, "The Proper {0}" ),
					new KarmaEntry( 4999, "The Admirable {0}" ),
					new KarmaEntry( 9999, "The Famed {0}" ),
					new KarmaEntry( 10000, "The Great {0}" )
				} ),
				new FameEntry( 9999, new KarmaEntry[]
				{
					new KarmaEntry( -10000, "The Dread {0}" ),
					new KarmaEntry( -5000, "The Evil {0}" ),
					new KarmaEntry( -2500, "The Villainous {0}" ),
					new KarmaEntry( -1250, "The Sinister {0}" ),
					new KarmaEntry( -625, "The Infamous {0}" ),
					new KarmaEntry( 624, "The Renowned {0}" ),
					new KarmaEntry( 1249, "The Distinguished {0}" ),
					new KarmaEntry( 2499, "The Eminent {0}" ),
					new KarmaEntry( 4999, "The Noble {0}" ),
					new KarmaEntry( 9999, "The Illustrious {0}" ),
					new KarmaEntry( 10000, "The Glorious {0}" )
				} ),
				new FameEntry( 10000, new KarmaEntry[]
				{
					new KarmaEntry( -10000, "The Dread {1} {0}" ),
					new KarmaEntry( -5000, "The Evil {1} {0}" ),
					new KarmaEntry( -2500, "The Dark {1} {0}" ),
					new KarmaEntry( -1250, "The Sinister {1} {0}" ),
					new KarmaEntry( -625, "The Dishonored {1} {0}" ),
					new KarmaEntry( 624, "{1} {0}" ),
					new KarmaEntry( 1249, "The Distinguished {1} {0}" ),
					new KarmaEntry( 2499, "The Eminent {1} {0}" ),
					new KarmaEntry( 4999, "The Noble {1} {0}" ),
					new KarmaEntry( 9999, "The Illustrious {1} {0}" ),
					new KarmaEntry( 10000, "The Glorious {1} {0}" )
				} )
			};
	}

	public class FameEntry
	{
		public int m_Fame;
		public KarmaEntry[] m_Karma;

		public FameEntry( int fame, KarmaEntry[] karma )
		{
			m_Fame = fame;
			m_Karma = karma;
		}
	}

	public class KarmaEntry
	{
		public int m_Karma;
		public string m_Title;

		public KarmaEntry( int karma, string title )
		{
			m_Karma = karma;
			m_Title = title;
		}
	}
}