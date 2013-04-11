using System;
using System.Collections;
using System.Collections.Generic;
using Server.Accounting;
using Server.ContextMenus;
using Server.Custom.PvpToolkit;
using Server.Custom.PvpToolkit.Tournament;
using Server.DuelSystem;
using Server.Engines.BulkOrders;
using Server.Engines.CannedEvil;
using Server.Engines.Craft;
using Server.Engines.Help;
using Server.Engines.MyRunUO;
using Server.Engines.Quests;
using Server.Ethics;
using Server.Factions;
using Server.Guilds;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Movement;
using Server.Multis;
using Server.Network;
using Server.Regions;
using Server.SkillHandlers;
using Server.Spells;
using Server.Spells.Bushido;
using Server.Spells.Fifth;
using Server.Spells.Fourth;
using Server.Spells.Necromancy;
using Server.Spells.Ninjitsu;
using Server.Spells.Seventh;
using Server.Targeting;
using RankDefinition=Server.Guilds.RankDefinition;
using Server.Spells.Spellweaving;
using Server.Custom.Games;
using Server.Poker;
using Server.Engines.PartySystem;
//bounty system
using Server.BountySystem;
//end bounty system

namespace Server.Mobiles
{
	#region Enums
	[Flags]
	public enum PlayerFlag // First 16 bits are reserved for default-distro use, start custom flags at 0x00010000
	{
        None				= 0x00000000,
		Glassblowing		= 0x00000001,
		Masonry				= 0x00000002,
		SandMining			= 0x00000004,
		StoneMining			= 0x00000008,
		ToggleMiningStone	= 0x00000010,
		KarmaLocked			= 0x00000020,
		AutoRenewInsurance	= 0x00000040,
		UseOwnFilter		= 0x00000080,
		PublicMyRunUO		= 0x00000100,
		PagingSquelched		= 0x00000200,
		Young				= 0x00000400,
		AcceptGuildInvites	= 0x00000800,
		DisplayChampionTitle= 0x00001000,
        HasStatReward		= 0x00002000,
        ClayMining          = 0x00010005,    //clay
	}

	public enum NpcGuild
	{
		None,
		MagesGuild,
		WarriorsGuild,
		ThievesGuild,
		RangersGuild,
		HealersGuild,
		MinersGuild,
		MerchantsGuild,
		TinkersGuild,
		TailorsGuild,
		FishermensGuild,
		BardsGuild,
		BlacksmithsGuild
	}

	public enum SolenFriendship
	{
		None,
		Red,
		Black
	}
	#endregion

	public partial class PlayerMobile : Mobile, IHonorTarget, IDyable, Participant
    {
        #region Tip System - Rob
        [CommandProperty(AccessLevel.GameMaster)]
        public Boolean ShowTipsOnLogin { get; set; }
        #endregion

        #region HitChanceModifier - maka
        public enum SwingAction
        {
            Hit,
            Miss
        }

        private SwingAction m_LastSwingActionResult = SwingAction.Miss;
        private int m_SwingActionCount = 1;

	    public SwingAction LastSwingActionResult
	    {
            get { return m_LastSwingActionResult; }
            set { m_LastSwingActionResult = value; }
	    }

        public int SwingCount
        {
            get { return m_SwingActionCount; }
            set { m_SwingActionCount = value; }
        }
        #endregion

        // bounty system
	    private ArrayList m_BountyUpdateList = new ArrayList();

	    public bool ShowBountyUpdate { get; set; }

	    public ArrayList BountyUpdateList
        {
            get { return m_BountyUpdateList; }
            set { m_BountyUpdateList = value; }
        }
        //end bounty system

        private class CountAndTimeStamp
		{
			private int m_Count;
			private DateTime m_Stamp;

		    public DateTime TimeStamp { get{ return m_Stamp; } }
			public int Count 
			{ 
				get { return m_Count; } 
				set	{ m_Count = value; m_Stamp = DateTime.Now; } 
			}
		}

		private DesignContext m_DesignContext;

        private List<Mobile> m_AutoStabled;
        private List<Mobile> m_AllFollowers;

        private PokerGame m_PokerGame; //Edit for Poker System
        public PokerGame PokerGame
        {
            get { return m_PokerGame; }
            set { m_PokerGame = value; }
        }

	    private TimeSpan m_PlayerGuildGameTime;
		private PlayerFlag m_Flags;

	    //Maka
	    private int m_OldMana = 0, m_OldStam = 0;

	    private RankDefinition m_GuildRank;

	    public object LadderGump { get; set; }

	    #region imported character
        private bool m_Imported;
        public bool Imported
        {
            get { return m_Imported; }
            set { m_Imported = value; }
        }
        #endregion

        #region always murderer
        private bool m_AlwaysMurderer;
        [CommandProperty (AccessLevel.GameMaster)]
        public bool AlwaysMurderer
        {
            get { return m_AlwaysMurderer; }
            set { m_AlwaysMurderer = value; }
        }
        #endregion

        #region Malik "AntiMacroGump"

	    [CommandProperty(AccessLevel.GameMaster)]
	    public bool AntiMacroGump { get; set; }

	    #endregion

        #region Malik - Skill boost
        private bool m_HasStartingSkillBoost;
        // Just in case... admin and above only
        [CommandProperty(AccessLevel.Administrator)]
        public bool HasStartingSkillBoost
        {
            get { return m_HasStartingSkillBoost; }
            set { m_HasStartingSkillBoost = value; }
        }
        #endregion

        #region Nasir - Has Custom Race

        private bool m_HasCustomRace;
        [CommandProperty(AccessLevel.GameMaster)]
	    public bool HasCustomRace
	    {
            get { return m_HasCustomRace; }
            set { m_HasCustomRace = value; }
        }

        #endregion
        
        #region Nasir - Original Character Variables

        private int m_OriginalHairHue;
	    private int m_OriginalHairItemID;
	    private int m_OriginalHue;

        public int OriginalHairHue
        {
            get { return m_OriginalHairHue; }
            set { m_OriginalHairHue = value; }
        }
	    public int OriginalHairItemID
	    {
            get { return m_OriginalHairItemID; }
            set { m_OriginalHairItemID = value; }
	    }
	    public int OriginalHue
	    {
            get { return m_OriginalHue; }
            set { m_OriginalHue = value; }
	    }

        #endregion

        #region Nasir - Stoned
        private bool m_Stoned;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Stoned
		{
			get { return m_Stoned; }
			set { m_Stoned = value; }
        }

        

        public override bool CanBeDamaged()
		{
			if( m_Stoned )
				return false;

			return base.CanBeDamaged();
		}

		public override bool CanUseStuckMenu()
		{
			if( m_Stoned )
				return false;

			return base.CanUseStuckMenu();
		}
		#endregion

        #region Yankovic - Rating & Events
        private int m_Rating = 1300;
        private int m_TournamentRating = 1300;

        private EventType m_EventType = EventType.NoEvent;
        private BaseGame m_CurrentEvent;
        private int m_Score;
        private GameInfoGumpType m_ShowGameGump = GameInfoGumpType.Extended;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Rating
        {
            get { return m_Rating; }
        }

        public void SetRating(int rating)
        {
            if (rating < 0)
                rating = 0;
            m_Rating = rating;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TournamentRating
        {
            get { return m_TournamentRating; }
        }

        public void SetTournamentRating(int rating)
        {
            if (rating < 0)
                rating = 0;
            m_TournamentRating = rating;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public EventType EventType
        {
            set
            {
                if (CurrentEvent == null)
                    m_EventType = value;
                else
                    m_EventType = EventType.NoEvent;
            }
            get {
                if (CurrentEvent != null)
                    return CurrentEvent.EventType;
                else
                    return m_EventType;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public BaseGame CurrentEvent
        {
            get { return m_CurrentEvent; }
            set 
            { 
                m_CurrentEvent = value;
                if (value == null)
                    m_EventType = EventType.NoEvent;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Score
        {
            get
            {
                return m_Score;
            }
            set
            {
                if (CurrentEvent == null)
                {
                    m_Score = 0;
                    return;
                }
                m_Score = value;
                if (m_Score >= CurrentEvent.MaxScore)
                    CurrentEvent.EndGameCommand();
            }
        }

        public int CompareTo(Participant p)
        {
            return Score.CompareTo(p.Score);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public GameInfoGumpType GameInfoGumpType
        {
            set
            {
                m_ShowGameGump = value;
                if (value == GameInfoGumpType.Disabled)
                    SendAsciiMessage(33, "You have chosen to disable the gameinfogump. To enable it again type .gamegump!");
            }
            get
            {
                return m_ShowGameGump;
            }
        }
        #endregion

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ShowArriveMsg { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowMulti { get; set; }

        #region Temporary bool check for temporary scripts
        [CommandProperty(AccessLevel.GameMaster)]
        public bool TempCheck { get; set; }
        #endregion

        #region Taran - Mods for polymorph
	    public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int SwingSpeed { get; set; }
        #endregion

	    public List<string> OfflineMessagesText = new List<string>();
	    public List<string> OfflineMessagesFrom = new List<string>();

	    public int AntiMacroFailed = 0;
	    public DateTime AntiMacroTimeCheck = DateTime.MinValue;

        public bool IsMassmoving { get; set; }

        #region Getters & Setters

        public List<Mobile> AutoStabled { get { return m_AutoStabled; } }

        public List<Mobile> AllFollowers
        {
            get
            {
                if (m_AllFollowers == null)
                    m_AllFollowers = new List<Mobile>(); ;
                return m_AllFollowers;
            }
        }

		#region Maka

	    [CommandProperty(AccessLevel.GameMaster)]
	    public DuelStone DuelStone { get; set; }

	    public DateTime LastAttackTime { get; set; }

	    public IAction CurrentAction { get; set; }

	    public Timer CurrentSwingTimer { get; set; }

        public Timer CurrentSquelchTimer { get; set; }

        public Timer CurrentLogoutMoveTimer { get; set; }

	    public bool HiddenWithSpell { get; set; }

	    public bool UseUnicodeSpeech { get; set; }

	    public WopLock WopLock { get; set; }

	    #endregion

	    //[CommandProperty(AccessLevel.GameMaster)]
	    public bool AllowDelete { get; set; }

        private Dictionary<Item, Point2D> m_ItemLocations;

	    public RankDefinition GuildRank
		{
			get
			{
			    if( AccessLevel >= AccessLevel.GameMaster )
					return RankDefinition.Leader;
			    return m_GuildRank;
			}
	        set{ m_GuildRank = value; }
		}

	    [CommandProperty( AccessLevel.GameMaster )]
	    public int GuildMessageHue { get; set; }

	    [CommandProperty( AccessLevel.GameMaster )]
	    public int AllianceMessageHue { get; set; }

	    [CommandProperty( AccessLevel.GameMaster )]
	    public int Profession { get; set; }

	    public int StepsTaken { get; set; }

	    [CommandProperty( AccessLevel.GameMaster )]
	    public NpcGuild NpcGuild { get; set; }

	    [CommandProperty( AccessLevel.GameMaster )]
	    public DateTime NpcGuildJoinTime { get; set; }

	    [CommandProperty(AccessLevel.GameMaster)]
	    public DateTime NextBODTurnInTime { get; set; }

	    [CommandProperty( AccessLevel.GameMaster )]
	    public DateTime LastOnline { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastMoved
        {
            get { return LastMoveTime; }
        }

	    [CommandProperty( AccessLevel.GameMaster )]
	    public TimeSpan NpcGuildGameTime { get; set; }

	    [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan PlayerGuildGameTime
        {
            get { return m_PlayerGuildGameTime; }
            set { m_PlayerGuildGameTime = value; }
        }

		private int m_ToTItemsTurnedIn;

		[CommandProperty( AccessLevel.GameMaster )]
		public int ToTItemsTurnedIn
		{
			get { return m_ToTItemsTurnedIn; }
			set { m_ToTItemsTurnedIn = value; }
		}

		private int m_ToTTotalMonsterFame;

		[CommandProperty( AccessLevel.GameMaster )]
		public int ToTTotalMonsterFame
		{
			get { return m_ToTTotalMonsterFame; }
			set { m_ToTTotalMonsterFame = value; }
		}

		#endregion

		#region PlayerFlags
		public PlayerFlag Flags
		{
			get{ return m_Flags; }
			set{ m_Flags = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool PagingSquelched
		{
			get{ return GetFlag( PlayerFlag.PagingSquelched ); }
			set{ SetFlag( PlayerFlag.PagingSquelched, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Glassblowing
		{
			get{ return GetFlag( PlayerFlag.Glassblowing ); }
			set{ SetFlag( PlayerFlag.Glassblowing, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Masonry
		{
			get{ return GetFlag( PlayerFlag.Masonry ); }
			set{ SetFlag( PlayerFlag.Masonry, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool SandMining
		{
			get{ return GetFlag( PlayerFlag.SandMining ); }
			set{ SetFlag( PlayerFlag.SandMining, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool StoneMining
		{
			get{ return GetFlag( PlayerFlag.StoneMining ); }
			set{ SetFlag( PlayerFlag.StoneMining, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ToggleMiningStone
		{
			get{ return GetFlag( PlayerFlag.ToggleMiningStone ); }
			set{ SetFlag( PlayerFlag.ToggleMiningStone, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool KarmaLocked
		{
			get{ return GetFlag( PlayerFlag.KarmaLocked ); }
			set{ SetFlag( PlayerFlag.KarmaLocked, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AutoRenewInsurance
		{
			get{ return GetFlag( PlayerFlag.AutoRenewInsurance ); }
			set{ SetFlag( PlayerFlag.AutoRenewInsurance, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool UseOwnFilter
		{
			get{ return GetFlag( PlayerFlag.UseOwnFilter ); }
			set{ SetFlag( PlayerFlag.UseOwnFilter, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool PublicMyRunUO
		{
			get{ return GetFlag( PlayerFlag.PublicMyRunUO ); }
			set{ SetFlag( PlayerFlag.PublicMyRunUO, value ); InvalidateMyRunUO(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AcceptGuildInvites
		{
			get{ return GetFlag( PlayerFlag.AcceptGuildInvites ); }
			set{ SetFlag( PlayerFlag.AcceptGuildInvites, value ); }
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public bool HasStatReward
        {
            get { return GetFlag(PlayerFlag.HasStatReward); }
            set { SetFlag(PlayerFlag.HasStatReward, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]//clay
        public bool ClayMining
        {
            get { return GetFlag(PlayerFlag.ClayMining); }
            set { SetFlag(PlayerFlag.ClayMining, value); }
        }

        #region Auto Arrow Recovery
        private Dictionary<Type, int> m_RecoverableAmmo = new Dictionary<Type, int>();

        public Dictionary<Type, int> RecoverableAmmo
        {
            get { return m_RecoverableAmmo; }
        }

        public void RecoverAmmo()
        {
            if (Core.SE && Alive)
            {
                foreach (KeyValuePair<Type, int> kvp in m_RecoverableAmmo)
                {
                    if (kvp.Value > 0)
                    {
                        Item ammo = null;

                        try
                        {
                            ammo = Activator.CreateInstance(kvp.Key) as Item;
                        }
                        catch
                        {
                        }

                        if (ammo != null)
                        {
                            string name = ammo.Name;
                            ammo.Amount = kvp.Value;

                            if (name == null)
                            {
                                if (ammo is Arrow)
                                    name = "arrow";
                                else if (ammo is Bolt)
                                    name = "bolt";
                            }

                            if (name != null && ammo.Amount > 1)
                                name = String.Format("{0}s", name);

                            if (name == null)
                                name = String.Format("#{0}", ammo.LabelNumber);

                            PlaceInBackpack(ammo);
                            SendLocalizedMessage(1073504, String.Format("{0}\t{1}", ammo.Amount, name)); // You recover ~1_NUM~ ~2_AMMO~.
                        }
                    }
                }

                m_RecoverableAmmo.Clear();
            }
        }
        #endregion

        private DateTime m_AnkhNextUse;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime AnkhNextUse
        {
            get { return m_AnkhNextUse; }
            set { m_AnkhNextUse = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan DisguiseTimeLeft
        {
            get { return DisguiseTimers.TimeRemaining(this); }
        }

        private DateTime m_PeacedUntil;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime PeacedUntil
        {
            get { return m_PeacedUntil; }
            set { m_PeacedUntil = value; }
        }

        #region Scroll of Alacrity
        private DateTime m_AcceleratedStart;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime AcceleratedStart
        {
            get { return m_AcceleratedStart; }
            set { m_AcceleratedStart = value; }
        }

        private SkillName m_AcceleratedSkill;

        [CommandProperty(AccessLevel.GameMaster)]
        public SkillName AcceleratedSkill
        {
            get { return m_AcceleratedSkill; }
            set { m_AcceleratedSkill = value; }
        }
        #endregion

        private bool m_IsStealthing; // IsStealthing should be moved to Server.Mobiles

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsStealthing // IsStealthing should be moved to Server.Mobiles
        {
            get { return m_IsStealthing; }
            set { m_IsStealthing = value; }
        }
        
        private bool m_IgnoreMobiles; // IgnoreMobiles should be moved to Server.Mobiles

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IgnoreMobiles // IgnoreMobiles should be moved to Server.Mobiles
        {
            get
            {
                return m_IgnoreMobiles;
            }
            set
            {
                if (m_IgnoreMobiles != value)
                {
                    m_IgnoreMobiles = value;
                    Delta(MobileDelta.Flags);
                }
            }
        }

        private int m_NonAutoreinsuredItems; // number of items that could not be automaitically reinsured because gold in bank was not enough

        /* 
         * a value of zero means, that the mobile is not executing the spell. Otherwise,
         * the value should match the BaseMana required 
        */

	    [CommandProperty(AccessLevel.GameMaster)]
	    public int ExecutesLightningStrike { get; set; }

	    public bool NinjaWepCooldown { get; set; }

        #endregion

        public static Direction GetDirection4( Point3D from, Point3D to )
		{
			int dx = from.X - to.X;
			int dy = from.Y - to.Y;

			int rx = dx - dy;
			int ry = dx + dy;

			Direction ret;

			if ( rx >= 0 && ry >= 0 )
				ret = Direction.West;
			else if ( rx >= 0 && ry < 0 )
				ret = Direction.South;
			else if ( rx < 0 && ry < 0 )
				ret = Direction.East;
			else
				ret = Direction.North;

			return ret;
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.Owner )]
		public override Mobile Combatant
		{
			get { return base.Combatant; }
			set
			{
				if( value != null )
					LastAttackTime = DateTime.Now;

				base.Combatant = value;
			}
		}

        public override void OnWarmodeChanged()
        {
            //Abort action
            AbortCurrentPlayerAction();

            if (!Warmode)
            {
                Combatant = null;
                Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(RecoverAmmo));
            }

            if (Spell != null && Spell.IsCasting)
                Spell.OnCasterKilled();

            BandageCheck();
            WeaponTimerCheck();

            base.OnWarmodeChanged();
        }

		public override bool OnDroppedItemToWorld( Item item, Point3D location )
		{
			if ( !base.OnDroppedItemToWorld( item, location ) )
				return false;

            /* Taran: On OSI, items cannot be dropped on the same tile as a mobile
            IPooledEnumerable mobiles = Map.GetMobilesInRange(location, 0);

			foreach ( Mobile m in mobiles ) 
			{ 
				if ( m.Z >= location.Z && m.Z < location.Z + 16 )
				{
					mobiles.Free();
					return false;
				}
			}
			
			mobiles.Free();
            */

            BounceInfo bi = item.GetBounce();

			if ( bi != null )
			{
				Type type = item.GetType();

				if ( type.IsDefined( typeof( FurnitureAttribute ), true ) || type.IsDefined( typeof( DynamicFlipingAttribute ), true ) )
				{
					object[] objs = type.GetCustomAttributes( typeof( FlipableAttribute ), true );

					if ( objs.Length > 0 )
					{
						FlipableAttribute fp = objs[0] as FlipableAttribute;

						if ( fp != null )
						{
							int[] itemIDs = fp.ItemIDs;

							Point3D oldWorldLoc = bi.m_WorldLoc;
							Point3D newWorldLoc = location;

							if ( oldWorldLoc.X != newWorldLoc.X || oldWorldLoc.Y != newWorldLoc.Y )
							{
								Direction dir = GetDirection4( oldWorldLoc, newWorldLoc );

								if ( itemIDs.Length == 2 )
								{
									switch ( dir )
									{
										case Direction.North:
										case Direction.South: item.ItemID = itemIDs[0]; break;
										case Direction.East:
										case Direction.West: item.ItemID = itemIDs[1]; break;
									}
								}
								else if ( itemIDs.Length == 4 )
								{
									switch ( dir )
									{
										case Direction.South: item.ItemID = itemIDs[0]; break;
										case Direction.East: item.ItemID = itemIDs[1]; break;
										case Direction.North: item.ItemID = itemIDs[2]; break;
										case Direction.West: item.ItemID = itemIDs[3]; break;
									}
								}
							}
						}
					}
				}
			}

			return true;
		}

        public override int GetPacketFlags()
        {
            int flags = base.GetPacketFlags();

            if (m_IgnoreMobiles)
                flags |= 0x10;

            return flags;
        }

        public override int GetOldPacketFlags()
        {
            int flags = base.GetOldPacketFlags();

            if (m_IgnoreMobiles)
                flags |= 0x10;

            return flags;
        }

		public bool GetFlag( PlayerFlag flag )
		{
			return ( (m_Flags & flag) != 0 );
		}

		public void SetFlag( PlayerFlag flag, bool value )
		{
			if ( value )
				m_Flags |= flag;
			else
				m_Flags &= ~flag;
		}

		public DesignContext DesignContext
		{
			get{ return m_DesignContext; }
			set{ m_DesignContext = value; }
		}

		public static void Initialize()
		{
            //Crim timer
		    ExpireCriminalDelay = TimeSpan.FromMinutes(5);

            //Attack last code // Can we avoid it through a method? 
            PacketHandlers.RegisterThrottler(0x05, OnAttackRequest);

            if (FastwalkPrevention)
                PacketHandlers.RegisterThrottler(0x02, MovementThrottle_Callback);

			EventSink.Login += OnLogin;
			EventSink.Logout += OnLogout;
			EventSink.Connected += EventSink_Connected;
			EventSink.Disconnected += EventSink_Disconnected;

            if (Core.SE)
            {
                Timer.DelayCall(TimeSpan.Zero, new TimerCallback(CheckPets));
            }
		}

        private static void CheckPets()
        {
            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m is PlayerMobile)
                {
                    PlayerMobile pm = (PlayerMobile)m;

                    if (((!pm.Mounted || (pm.Mount != null && pm.Mount is EtherealMount)) && (pm.AllFollowers.Count > pm.AutoStabled.Count)) ||
                        (pm.Mounted && (pm.AllFollowers.Count > (pm.AutoStabled.Count + 1))))
                    {
                        pm.AutoStablePets(); /* autostable checks summons, et al: no need here */
                    }
                }
            }
        }

		public override void OnSkillInvalidated( Skill skill )
		{
			if ( Core.AOS && skill.SkillName == SkillName.MagicResist )
				UpdateResistances();
		}

		public override int GetMaxResistance( ResistanceType type )
		{
            if (AccessLevel > AccessLevel.Player)
                return int.MaxValue;

			int max = base.GetMaxResistance( type );

			if ( type != ResistanceType.Physical && 60 < max && CurseSpell.UnderEffect( this ) )
				max = 60;

			if( Core.ML && Race == Race.Elf && type == ResistanceType.Energy )
				max += 5; //Intended to go after the 60 max from curse

			return max;
		}

		protected override void OnRaceChange( Race oldRace )
		{
			ValidateEquipment();
			UpdateResistances();
		}

		public override int MaxWeight { get { return (((Core.ML && Race == Race.Human) ? 100 : 47) + (int)(3.5 * Str)); } }

		private int m_LastGlobalLight = -1, m_LastPersonalLight = -1;

		public override void OnNetStateChanged()
		{
			m_LastGlobalLight = -1;
			m_LastPersonalLight = -1;
		}

		public override void ComputeBaseLightLevels( out int global, out int personal )
		{
			global = LightCycle.ComputeLevelFor( this );

			bool racialNightSight = (Core.ML && Race == Race.Elf);

			if ( LightLevel < 21 && ( AosAttributes.GetValue( this, AosAttribute.NightSight ) > 0 || racialNightSight ))
				personal = 21;
			else
				personal = LightLevel;
		}

		public override void CheckLightLevels( bool forceResend )
		{
			NetState ns = NetState;

			if ( ns == null )
				return;

			int global, personal;

			ComputeLightLevels( out global, out personal );

			if ( !forceResend )
				forceResend = ( global != m_LastGlobalLight || personal != m_LastPersonalLight );

			if ( !forceResend )
				return;

			m_LastGlobalLight = global;
			m_LastPersonalLight = personal;

			ns.Send( GlobalLightLevel.Instantiate( global ) );
			ns.Send( new PersonalLightLevel( this, personal ) );
		}

		public override int GetMinResistance( ResistanceType type )
		{
			int magicResist = (int)(Skills[SkillName.MagicResist].Value * 10);
			int min = int.MinValue;

			if ( magicResist >= 1000 )
				min = 40 + ((magicResist - 1000) / 50);
			else if ( magicResist >= 400 )
				min = (magicResist - 400) / 15;

			if ( min > MaxPlayerResistance )
				min = MaxPlayerResistance;

			int baseMin = base.GetMinResistance( type );

			if ( min < baseMin )
				min = baseMin;

			return min;
		}

        public override void OnManaChange(int oldValue)
        {
            base.OnManaChange(oldValue);
            if (ExecutesLightningStrike > 0)
            {
                if (Mana < ExecutesLightningStrike)
                {
                    SpecialMove.ClearCurrentMove(this);
                }
            }
        }

		private static void OnLogin( LoginEventArgs e )
		{
			Mobile from = e.Mobile;

			CheckAtrophies( from );

			if ( AccountHandler.LockdownLevel > AccessLevel.Player )
			{
				string notice;

				Account acct = from.Account as Account;

				if ( acct == null || !acct.HasAccess( from.NetState ) )
				{
					if ( from.AccessLevel == AccessLevel.Player )
						notice = "The server is currently under lockdown. No players are allowed to log in at this time.";
					else
						notice = "The server is currently under lockdown. You do not have sufficient access level to connect.";

					Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( Disconnect ), from );
				}
				else if ( from.AccessLevel >= AccessLevel.Administrator )
				{
					notice = "The server is currently under lockdown. As you are an administrator, you may change this from the [Admin gump.";
				}
				else
				{
					notice = "The server is currently under lockdown. You have sufficient access level to connect.";
				}
				from.SendGump( new NoticeGump( 1060637, 30720, notice, 0xFFC000, 300, 140, null, null ) );
			}

            if (!((PlayerMobile)from).HasStartingSkillBoost)
            {
                if (from.Skills.Total == 0)
                    from.SendGump(new SkillBonusStoneInfoGump());
            }

            if (((PlayerMobile)from).AntiMacroGump)
            {
                AntiMacro.AntiMacroGump.SendGumpThreaded((PlayerMobile)from);
            }

            ((PlayerMobile)from).ClaimAutoStabledPets();

            if (((PlayerMobile)from).OfflineMessagesText.Count > 0) //Player has offline page response messages to read
            {
                for (int i = 0; i < ((PlayerMobile) from).OfflineMessagesText.Count; ++i)
                {
                    from.SendGump(new PageResponseGump(from, ((PlayerMobile) from).OfflineMessagesFrom[i], ((PlayerMobile) from).OfflineMessagesText[i]));
                }
                ((PlayerMobile)from).OfflineMessagesText.Clear();
                ((PlayerMobile)from).OfflineMessagesFrom.Clear();
            }

            //bounty system
            PlayerMobile player = (PlayerMobile)from;

            if (player.ShowBountyUpdate)
            {
                from.SendGump(new BountyStatusGump(from, player.BountyUpdateList));
                player.BountyUpdateList.Clear();
                player.ShowBountyUpdate = false;
            }
            //end bounty system
		}

		private bool m_NoDeltaRecursion;

		public void ValidateEquipment()
		{
			if ( m_NoDeltaRecursion || Map == null || Map == Map.Internal )
				return;

			if ( Items == null )
				return;

			m_NoDeltaRecursion = true;
			Timer.DelayCall( TimeSpan.Zero, new TimerCallback( ValidateEquipment_Sandbox ) );
		}

		private void ValidateEquipment_Sandbox()
		{
			try
			{
				if ( Map == null || Map == Map.Internal )
					return;

				List<Item> items = Items;

				if ( items == null )
					return;

				bool moved = false;

				int str = Str;
				int dex = Dex;
				int intel = Int;

				#region Factions
				int factionItemCount = 0;
				#endregion

				PlayerMobile from = this;

				#region Ethics
				Ethic ethic = Ethic.Find( from );
				#endregion

				for ( int i = items.Count - 1; i >= 0; --i )
				{
					if ( i >= items.Count )
						continue;

					Item item = items[i];

					#region Ethics
					if ( ( item.SavedFlags & 0x100 ) != 0 )
					{
						if ( item.Hue != Ethic.Hero.Definition.PrimaryHue )
						{
							item.SavedFlags &= ~0x100;
						}
						else if ( ethic != Ethic.Hero )
						{
							from.AddToBackpack( item );
							moved = true;
							continue;
						}
					}
					else if ( ( item.SavedFlags & 0x200 ) != 0 )
					{
						if ( item.Hue != Ethic.Evil.Definition.PrimaryHue )
						{
							item.SavedFlags &= ~0x200;
						}
						else if ( ethic != Ethic.Evil )
						{
							from.AddToBackpack( item );
							moved = true;
							continue;
						}
					}
					#endregion

					if ( item is BaseWeapon )
					{
						BaseWeapon weapon = (BaseWeapon)item;

						bool drop = false;

						if( dex < weapon.DexRequirement )
							drop = true;
						else if( str < AOS.Scale( weapon.StrRequirement, 100 - weapon.GetLowerStatReq() ) )
							drop = true;
						else if( intel < weapon.IntRequirement )
							drop = true;
						//else if( weapon.RequiredRace != null && weapon.RequiredRace != Race )
						//	drop = true;

						if ( drop )
						{
							string name = weapon.Name;

							if ( name == null )
								name = String.Format( "#{0}", weapon.LabelNumber );

							from.SendLocalizedMessage( 1062001, name ); // You can no longer wield your ~1_WEAPON~
							from.AddToBackpack( weapon );
							moved = true;
						}
					}
					else if ( item is BaseArmor )
					{
						BaseArmor armor = (BaseArmor)item;

						bool drop = false;

						if ( !armor.AllowMaleWearer && !from.Female && from.AccessLevel < AccessLevel.GameMaster )
						{
							drop = true;
						}
						else if ( !armor.AllowFemaleWearer && from.Female && from.AccessLevel < AccessLevel.GameMaster )
						{
							drop = true;
						}
						//else if( armor.RequiredRace != null && armor.RequiredRace != Race )
						//{
						//	drop = true;
						//}
						else
						{
							int strBonus = armor.ComputeStatBonus( StatType.Str ), strReq = armor.ComputeStatReq( StatType.Str );
							int dexBonus = armor.ComputeStatBonus( StatType.Dex ), dexReq = armor.ComputeStatReq( StatType.Dex );
							int intBonus = armor.ComputeStatBonus( StatType.Int ), intReq = armor.ComputeStatReq( StatType.Int );

							if( dex < dexReq || (dex + dexBonus) < 1 )
								drop = true;
							else if( str < strReq || (str + strBonus) < 1 )
								drop = true;
							else if( intel < intReq || (intel + intBonus) < 1 )
								drop = true;
						}

						if ( drop )
						{
							string name = armor.Name;

							if ( name == null )
								name = String.Format( "#{0}", armor.LabelNumber );

							if ( armor is BaseShield )
								from.SendLocalizedMessage( 1062003, name ); // You can no longer equip your ~1_SHIELD~
							else
								from.SendLocalizedMessage( 1062002, name ); // You can no longer wear your ~1_ARMOR~

							from.AddToBackpack( armor );
							moved = true;
						}
                    }/* Taran: No resreictions for clothes
					else if ( item is BaseClothing )
					{
						BaseClothing clothing = (BaseClothing)item;

						bool drop = false;

						if ( !clothing.AllowMaleWearer && !from.Female && from.AccessLevel < AccessLevel.GameMaster )
						{
							drop = true;
						}
						else if ( !clothing.AllowFemaleWearer && from.Female && from.AccessLevel < AccessLevel.GameMaster )
						{
							drop = true;
						}
						else if( clothing.RequiredRace != null && clothing.RequiredRace != Race )
						{
							drop = true;
						}
						else
						{
							int strBonus = clothing.ComputeStatBonus( StatType.Str );
							int strReq = clothing.ComputeStatReq( StatType.Str );

							if( str < strReq || (str + strBonus) < 1 )
								drop = true;
						}

						if ( drop )
						{
							string name = clothing.Name;

							if ( name == null )
								name = String.Format( "#{0}", clothing.LabelNumber );

							from.SendLocalizedMessage( 1062002, name ); // You can no longer wear your ~1_ARMOR~

							from.AddToBackpack( clothing );
							moved = true;
						}
					}*/

					FactionItem factionItem = FactionItem.Find( item );

					if ( factionItem != null )
					{
						bool drop = false;

						Faction ourFaction = Faction.Find( this );

						if ( ourFaction == null || ourFaction != factionItem.Faction )
							drop = true;
						else if ( ++factionItemCount > FactionItem.GetMaxWearables( this ) )
							drop = true;

						if ( drop )
						{
							from.AddToBackpack( item );
							moved = true;
						}
					}
				}

				if ( moved )
					from.SendLocalizedMessage( 500647 ); // Some equipment has been moved to your backpack.
			}
			catch ( Exception e )
			{
				Console.WriteLine( e );
			}
			finally
			{
				m_NoDeltaRecursion = false;
			}
		}

		public override void Delta( MobileDelta flag )
		{
			base.Delta( flag );

			if ( (flag & MobileDelta.Stat) != 0 )
				ValidateEquipment();

			if ( (flag & (MobileDelta.Name | MobileDelta.Hue)) != 0 )
				InvalidateMyRunUO();
		}

		private static void Disconnect( object state )
		{
			NetState ns = ((Mobile)state).NetState;

			if ( ns != null )
				ns.Dispose();
		}

		private static void OnLogout( LogoutEventArgs e )
		{
            PlayerMobile pm = e.Mobile as PlayerMobile;

            if (pm != null)
            {
                pm.AbortCurrentPlayerAction();
                ((PlayerMobile) e.Mobile).AutoStablePets();
            }                
		}

		private static void EventSink_Connected( ConnectedEventArgs e )
		{
			PlayerMobile pm = e.Mobile as PlayerMobile;

			if ( pm != null )
			{
				pm.m_SessionStart = DateTime.Now;

				if ( pm.m_Quest != null )
					pm.m_Quest.StartTimer();

				pm.BedrollLogout = false;
				pm.LastOnline = DateTime.Now;
			}

            DisguiseTimers.StartTimer(e.Mobile);

			Timer.DelayCall( TimeSpan.Zero, new TimerStateCallback( ClearSpecialMovesCallback ), e.Mobile );
		}

		private static void ClearSpecialMovesCallback( object state )
		{
			Mobile from = (Mobile)state;

			SpecialMove.ClearAllMoves( from );
		}

		private static void EventSink_Disconnected( DisconnectedEventArgs e )
		{
			Mobile from = e.Mobile;
			DesignContext context = DesignContext.Find( from );

			if ( context != null )
			{
				/* Client disconnected
				 *  - Remove design context
				 *  - Eject all from house
				 *  - Restore relocated entities
				 */

				// Remove design context
				DesignContext.Remove( from );

				// Eject all from house
				from.RevealingAction();

				foreach ( Item item in context.Foundation.GetItems() )
					item.Location = context.Foundation.BanLocation;

				foreach ( Mobile mobile in context.Foundation.GetMobiles() )
					mobile.Location = context.Foundation.BanLocation;

				// Restore relocated entities
				context.Foundation.RestoreRelocatedEntities();
			}

			PlayerMobile pm = e.Mobile as PlayerMobile;

			if ( pm != null )
			{
                pm.AbortCurrentPlayerAction();

				pm.m_GameTime += (DateTime.Now - pm.m_SessionStart);

                if (pm.Guild != null && !pm.Guild.Disbanded)
                    pm.PlayerGuildGameTime += (DateTime.Now - pm.m_SessionStart);

				if ( pm.m_Quest != null )
					pm.m_Quest.StopTimer();

				pm.m_SpeechLog = null;
				pm.LastOnline = DateTime.Now;
			}

            DisguiseTimers.StopTimer(from);
		}

		public override void RevealingAction()
		{
			if ( m_DesignContext != null )
				return;

		    HiddenWithSpell = false;

			base.RevealingAction();

            m_IsStealthing = false; // IsStealthing should be moved to Server.Mobiles
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override bool Hidden
		{
			get
			{
				return base.Hidden;
			}
			set
			{
				base.Hidden = value;

				RemoveBuff( BuffIcon.Invisibility );	//Always remove, default to the hiding icon EXCEPT in the invis spell where it's explicitly set

				if( !Hidden )
				{
					RemoveBuff( BuffIcon.HidingAndOrStealth );
				}
				else// if( !InvisibilitySpell.HasTimer( this ) )
				{
					BuffInfo.AddBuff( this, new BuffInfo( BuffIcon.HidingAndOrStealth, 1075655 ) );	//Hidden/Stealthing & You Are Hidden
				}
			}
		}

		public override void OnSubItemAdded( Item item )
		{
			if ( AccessLevel < AccessLevel.GameMaster && item.IsChildOf( Backpack ) )
			{
				int maxWeight = WeightOverloading.GetMaxWeight( this );
				int curWeight = BodyWeight + TotalWeight;

				if ( curWeight > maxWeight )
					SendLocalizedMessage( 1019035, true, String.Format( " : {0} / {1}", curWeight, maxWeight ) );
			}
		}

        public override bool CanBeHarmful( Mobile target, bool message, bool ignoreOurBlessedness )
		{
			if ( m_DesignContext != null || (target is PlayerMobile && ((PlayerMobile)target).m_DesignContext != null) )
				return false;

			if ( (target is BaseVendor && ((BaseVendor)target).IsInvulnerable) || target is PlayerVendor || target is TownCrier )
			{
				if ( message )
				{
					if ( target.Title == null )
						SendMessage( "{0} the vendor cannot be harmed.", target.Name );
					else
						SendMessage( "{0} {1} cannot be harmed.", target.Name, target.Title );
				}

				return false;
			}

            if (!target.Player)
            {
                BaseHouse house = BaseHouse.FindHouseAt(this);
                BaseHouse house2 = BaseHouse.FindHouseAt(target);
                if (house != null && house != house2)
                {
                    SendAsciiMessage("You cannot do this!");
                    return false;
                }
            }
            
			return base.CanBeHarmful( target, message, ignoreOurBlessedness );
		}

		public override bool CanBeBeneficial( Mobile target, bool message, bool allowDead )
		{
			if ( m_DesignContext != null || (target is PlayerMobile && ((PlayerMobile)target).m_DesignContext != null) )
				return false;

            if (!target.Player)
            {
                BaseHouse house = BaseHouse.FindHouseAt(this);
                BaseHouse house2 = BaseHouse.FindHouseAt(target);

                if (house != null && house != house2)
                {
                    SendAsciiMessage("You cannot do this!");
                    return false;
                }
            }

			return base.CanBeBeneficial( target, message, allowDead );
		}

		public override bool CheckContextMenuDisplay( IEntity target )
		{
			return ( m_DesignContext == null );
		}

		public override void OnItemAdded( Item item )
		{
			base.OnItemAdded( item );

			if ( item is BaseArmor || item is BaseWeapon )
			{
				Hits=Hits; Stam=Stam; Mana=Mana;
			}

			if ( NetState != null )
				CheckLightLevels( false );

			InvalidateMyRunUO();
		}

		public override void OnItemRemoved( Item item )
		{
			base.OnItemRemoved( item );

			if ( item is BaseArmor || item is BaseWeapon )
			{
				Hits=Hits; Stam=Stam; Mana=Mana;
			}

			if ( NetState != null )
				CheckLightLevels( false );

			InvalidateMyRunUO();
		}

		public override double ArmorRating
		{
			get
			{
				//BaseArmor ar;
				double rating = 0.0;

				AddArmorRating( ref rating, NeckArmor );
				AddArmorRating( ref rating, HandArmor );
				AddArmorRating( ref rating, HeadArmor );
				AddArmorRating( ref rating, ArmsArmor );
				AddArmorRating( ref rating, LegsArmor );
				AddArmorRating( ref rating, ChestArmor );
				AddArmorRating( ref rating, ShieldArmor );

				return VirtualArmor + VirtualArmorMod + rating;
			}
		}

		private static void AddArmorRating( ref double rating, Item armor )
		{
			BaseArmor ar = armor as BaseArmor;

			if( ar != null && ( !Core.AOS || ar.ArmorAttributes.MageArmor == 0 ))
				rating += ar.ArmorRatingScaled;
		}

        public double BaseArmorRatingSpells
        {
            get
            {
                double rating = 0.0;

                AddBaseArmorRatingSpells(ref rating, NeckArmor);
                AddBaseArmorRatingSpells(ref rating, HandArmor);
                AddBaseArmorRatingSpells(ref rating, HeadArmor);
                AddBaseArmorRatingSpells(ref rating, ArmsArmor);
                AddBaseArmorRatingSpells(ref rating, LegsArmor);
                AddBaseArmorRatingSpells(ref rating, ChestArmor);

                return rating;
            }
        }

        private static void AddBaseArmorRatingSpells(ref double rating, Item armor)
        {
            BaseArmor ar = armor as BaseArmor;

            if (ar != null)
                rating += ar.ArmorRatingSpellsScaled;
        }

		#region [Stats]Max
		[CommandProperty( AccessLevel.GameMaster )]
		public override int HitsMax
		{
			get
			{
				int strBase;
				int strOffs = GetStatOffset( StatType.Str );

				if ( Core.AOS )
				{
					strBase = Str;	//this.Str already includes GetStatOffset/str
					strOffs = AosAttributes.GetValue( this, AosAttribute.BonusHits );

                    if (Core.ML && strOffs > 25 && AccessLevel <= AccessLevel.Player)
                        strOffs = 25;

					if ( AnimalForm.UnderTransformation( this, typeof( BakeKitsune ) ) || AnimalForm.UnderTransformation( this, typeof( GreyWolf ) ) )
						strOffs += 20;
				}
				else
				{
					strBase = RawStr;
				}

                //return (strBase);
                return (strBase / 2) + 50 + strOffs;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int StamMax
		{
			get
			{
                int dexOffs = GetStatOffset(StatType.Dex);

                return (RawDex) + dexOffs; //return base.StamMax + AosAttributes.GetValue( this, AosAttribute.BonusStam );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int ManaMax
		{
			get
			{
                int intOffs = GetStatOffset(StatType.Int);

                return (RawInt) + intOffs; //return base.ManaMax + AosAttributes.GetValue( this, AosAttribute.BonusMana ) + ((Core.ML && Race == Race.Elf) ? 20 : 0);
			}
		}
		#endregion

		#region Stat Getters/Setters

		[CommandProperty( AccessLevel.GameMaster )]
		public override int Str
		{
			get
			{
				if( Core.ML && AccessLevel == AccessLevel.Player )
					return Math.Min( base.Str, 150 );

				return base.Str;
			}
			set
			{
				base.Str = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int Int
		{
			get
			{
				if( Core.ML && AccessLevel == AccessLevel.Player )
					return Math.Min( base.Int, 150 );

				return base.Int;
			}
			set
			{
				base.Int = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int Dex
		{
			get
			{
				if( Core.ML && AccessLevel == AccessLevel.Player )
					return Math.Min( base.Dex, 150 );

				return base.Dex;
			}
			set
			{
				base.Dex = value;
			}
		}

        #endregion

		public override bool Move( Direction d )
		{
            /*
			NetState ns = this.NetState;

			if ( ns != null )
			{
				if ( HasGump( typeof( ResurrectGump ) ) ) {
					if ( Alive ) {
						CloseGump( typeof( ResurrectGump ) );
					} else {
						SendLocalizedMessage( 500111 ); // You are frozen and cannot move.
						return false;
					}
				}
			}*/

			TimeSpan speed = ComputeMovementSpeed( d );

		    if ( !Alive )
				MovementImpl.IgnoreMovableImpassables = true;

			bool res = base.Move( d );

			MovementImpl.IgnoreMovableImpassables = false;

			if ( !res )
				return false;

			m_NextMovementTime += speed;

			return true;
		}

		public override bool CheckMovement( Direction d, out int newZ )
		{
			DesignContext context = m_DesignContext;

			if ( context == null )
				return base.CheckMovement( d, out newZ );

			HouseFoundation foundation = context.Foundation;

			newZ = foundation.Z + HouseFoundation.GetLevelZ( context.Level, context.Foundation );

			int newX = X, newY = Y;
			Movement.Movement.Offset( d, ref newX, ref newY );

			int startX = foundation.X + foundation.Components.Min.X + 1;
			int startY = foundation.Y + foundation.Components.Min.Y + 1;
			int endX = startX + foundation.Components.Width - 1;
			int endY = startY + foundation.Components.Height - 2;

			return ( newX >= startX && newY >= startY && newX < endX && newY < endY && Map == foundation.Map );
		}

		public override bool AllowItemUse( Item item )
		{
			return DesignContext.Check( this );
		}

		public SkillName[] AnimalFormRestrictedSkills{ get{ return m_AnimalFormRestrictedSkills; } }

		private readonly SkillName[] m_AnimalFormRestrictedSkills = new SkillName[]
		{
			SkillName.ArmsLore,	SkillName.Begging, SkillName.Discordance, SkillName.Forensics,
			SkillName.Inscribe, SkillName.ItemID, SkillName.Meditation, SkillName.Peacemaking,
			SkillName.Provocation, SkillName.RemoveTrap, SkillName.SpiritSpeak, SkillName.Stealing,	
			SkillName.TasteID
		};

		public override bool AllowSkillUse( SkillName skill )
		{
			if ( AnimalForm.UnderTransformation( this ) )
			{
				for( int i = 0; i < m_AnimalFormRestrictedSkills.Length; i++ )
				{
					if( m_AnimalFormRestrictedSkills[i] == skill )
					{
						SendLocalizedMessage( 1070771 ); // You cannot use that skill in this form.
						return false;
					}
				}
			}

			return DesignContext.Check( this );
		}

		private bool m_LastProtectedMessage;
		private int m_NextProtectionCheck = 1;

		public virtual void RecheckTownProtection()
		{
			m_NextProtectionCheck = 1;

			GuardedRegion reg = (GuardedRegion) Region.GetRegion( typeof( GuardedRegion ) );
			bool protectionTimer = ( reg != null && !reg.IsDisabled() );

			if ( protectionTimer != m_LastProtectedMessage )
			{
				if ( protectionTimer )
					SendLocalizedMessage( 500112 ); // You are now under the protection of the town guards.
				else
					SendLocalizedMessage( 500113 ); // You have left the protection of the town guards.

				m_LastProtectedMessage = protectionTimer;
			}
		}

		public override void MoveToWorld( Point3D loc, Map map )
		{
			base.MoveToWorld( loc, map );

			RecheckTownProtection();
		}

		public override void SetLocation( Point3D loc, bool isTeleport )
		{
			if ( !isTeleport && AccessLevel == AccessLevel.Player )
			{
				// moving, not teleporting
				int zDrop = ( Location.Z - loc.Z );

				if ( zDrop > 20 ) // we fell more than one story
					Hits -= ((zDrop / 20) * 10) - 5; // deal some damage; does not kill, disrupt, etc
			}

			base.SetLocation( loc, isTeleport );

			if ( isTeleport || --m_NextProtectionCheck == 0 )
				RecheckTownProtection();
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( from == this )
			{
				if ( m_Quest != null )
					m_Quest.GetContextMenuEntries( list );
                
				if ( Alive && InsuranceEnabled )
				{
					list.Add( new CallbackEntry( 6201, new ContextCallback( ToggleItemInsurance ) ) );

					if ( AutoRenewInsurance )
						list.Add( new CallbackEntry( 6202, new ContextCallback( CancelRenewInventoryInsurance ) ) );
					else
						list.Add( new CallbackEntry( 6200, new ContextCallback( AutoRenewInventoryInsurance ) ) );
				}
                
				BaseHouse house = BaseHouse.FindHouseAt( this );

				if ( house != null )
				{
					if ( Alive && house.InternalizedVendors.Count > 0 && house.IsOwner( this ) )
						list.Add( new CallbackEntry( 6204, new ContextCallback( GetVendor ) ) );

					if ( house.IsAosRules )
						list.Add( new CallbackEntry( 6207, new ContextCallback( LeaveHouse ) ) );
				}

				if ( m_JusticeProtectors.Count > 0 )
					list.Add( new CallbackEntry( 6157, new ContextCallback( CancelProtection ) ) );

				if( Alive )
					list.Add( new CallbackEntry( 6210, new ContextCallback( ToggleChampionTitleDisplay ) ) );

                if (from != this)
                {
                    if (Alive && Core.Expansion >= Expansion.AOS)
                    {
                        Party theirParty = from.Party as Party;
                        Party ourParty = this.Party as Party;

                        if (theirParty == null && ourParty == null)
                        {
                            list.Add(new AddToPartyEntry(from, this));
                        }
                        else if (theirParty != null && theirParty.Leader == from)
                        {
                            if (ourParty == null)
                            {
                                list.Add(new AddToPartyEntry(from, this));
                            }
                            else if (ourParty == theirParty)
                            {
                                list.Add(new RemoveFromPartyEntry(from, this));
                            }
                        }
                    }

                    BaseHouse curhouse = BaseHouse.FindHouseAt(this);

                    if (curhouse != null)
                    {
                        if (Alive && Core.Expansion >= Expansion.AOS && curhouse.IsAosRules && curhouse.IsFriend(from))
                            list.Add(new EjectPlayerEntry(from, this));
                    }
                }
			}
		}

        
		private void CancelProtection()
		{
			for ( int i = 0; i < m_JusticeProtectors.Count; ++i )
			{
				Mobile prot = m_JusticeProtectors[i];

				string args = String.Format( "{0}\t{1}", Name, prot.Name );

				prot.SendLocalizedMessage( 1049371, args ); // The protective relationship between ~1_PLAYER1~ and ~2_PLAYER2~ has been ended.
				SendLocalizedMessage( 1049371, args ); // The protective relationship between ~1_PLAYER1~ and ~2_PLAYER2~ has been ended.
			}

			m_JusticeProtectors.Clear();
		}
        

		#region Insurance

        
		private void ToggleItemInsurance()
		{
			if ( !CheckAlive() )
				return;

			BeginTarget( -1, false, TargetFlags.None, ToggleItemInsurance_Callback );
			SendLocalizedMessage( 1060868 ); // Target the item you wish to toggle insurance status on <ESC> to cancel
		}
        

		private static bool CanInsure( Item item )
		{
			if ( item is Container || item is BagOfSending || item is KeyRing )
				return false;

			if ( (item is Spellbook && item.LootType == LootType.Blessed)|| item is Runebook || item is PotionKeg || item is Sigil )
				return false;

			if ( item.Stackable )
				return false;

			if ( item.LootType == LootType.Cursed )
				return false;

			if ( item.ItemID == 0x204E ) // death shroud
				return false;

			return true;
		}

		private void ToggleItemInsurance_Callback( Mobile from, object obj )
		{
			if ( !CheckAlive() )
				return;

			Item item = obj as Item;

			if ( item == null || !item.IsChildOf( this ) )
			{
				BeginTarget( -1, false, TargetFlags.None, ToggleItemInsurance_Callback );
				SendLocalizedMessage( 1060871, "", 0x23 ); // You can only insure items that you have equipped or that are in your backpack
			}
			else if ( item.Insured )
			{
				item.Insured = false;

				SendLocalizedMessage( 1060874, "", 0x35 ); // You cancel the insurance on the item

				BeginTarget( -1, false, TargetFlags.None, ToggleItemInsurance_Callback );
				SendLocalizedMessage( 1060868, "", 0x23 ); // Target the item you wish to toggle insurance status on <ESC> to cancel
			}
			else if ( !CanInsure( item ) )
			{
				BeginTarget( -1, false, TargetFlags.None, ToggleItemInsurance_Callback );
				SendLocalizedMessage( 1060869, "", 0x23 ); // You cannot insure that
			}
			else if ( item.LootType == LootType.Blessed || item.LootType == LootType.Newbied || item.BlessedFor == from )
			{
				BeginTarget( -1, false, TargetFlags.None, ToggleItemInsurance_Callback );
				SendLocalizedMessage( 1060870, "", 0x23 ); // That item is blessed and does not need to be insured
				SendLocalizedMessage( 1060869, "", 0x23 ); // You cannot insure that
			}
			else
			{
				if ( !item.PayedInsurance )
				{
					if ( Banker.Withdraw( from, 600 ) )
					{
						SendLocalizedMessage( 1060398, "600" ); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
						item.PayedInsurance = true;
					}
					else
					{
						SendLocalizedMessage( 1061079, "", 0x23 ); // You lack the funds to purchase the insurance
						return;
					}
				}

				item.Insured = true;

				SendLocalizedMessage( 1060873, "", 0x23 ); // You have insured the item

				BeginTarget( -1, false, TargetFlags.None, ToggleItemInsurance_Callback );
				SendLocalizedMessage( 1060868, "", 0x23 ); // Target the item you wish to toggle insurance status on <ESC> to cancel
			}
		}
        

		private void AutoRenewInventoryInsurance()
		{
			if ( !CheckAlive() )
				return;

			SendLocalizedMessage( 1060881, "", 0x23 ); // You have selected to automatically reinsure all insured items upon death
			AutoRenewInsurance = true;
		}

		private void CancelRenewInventoryInsurance()
		{
			if ( !CheckAlive() )
				return;

			if( Core.SE )
			{
				if( !HasGump( typeof( CancelRenewInventoryInsuranceGump ) ) )
					SendGump( new CancelRenewInventoryInsuranceGump( this ) );
			}
			else
			{
				SendLocalizedMessage( 1061075, "", 0x23 ); // You have cancelled automatically reinsuring all insured items upon death
				AutoRenewInsurance = false;
			}
		}
        

        public bool BeginPlayerAction(IAction newAction)
        {
            return BeginPlayerAction(newAction, false);
        }

        public bool BeginPlayerAction(IAction newAction, bool abortPreviousAction)
        {
            if (abortPreviousAction)
            {
                if (CurrentAction != null)
                    CurrentAction.AbortAction(this);

                EndAction(typeof(IAction));
            }
        
            bool toReturn = BeginAction(typeof(IAction));

            if (toReturn)
                CurrentAction = newAction;

            return toReturn;
        }

        public void ResetPlayerAction(IAction newAction)
        {
            ResetPlayerAction(newAction, false);
        }

        public void ResetPlayerAction(IAction newAction, bool abortPreviousAction)
        {
            if (abortPreviousAction && CurrentAction != null)
                CurrentAction.AbortAction(this);

            BeginAction(typeof(IAction));
            CurrentAction = newAction;
        }

        public void EndPlayerAction()
        {
            CurrentAction = null;
            EndAction(typeof(IAction));
        }

        public void AbortCurrentPlayerAction()
        {
            if (CurrentAction != null)
                CurrentAction.AbortAction(this);

            EndPlayerAction();
        }

    	private class CancelRenewInventoryInsuranceGump : Gump
		{
			private readonly PlayerMobile m_Player;

			public CancelRenewInventoryInsuranceGump( PlayerMobile player ) : base( 250, 200 )
			{
				m_Player = player;

				AddBackground( 0, 0, 240, 142, 0x13BE );
				AddImageTiled( 6, 6, 228, 100, 0xA40 );
				AddImageTiled( 6, 116, 228, 20, 0xA40 );
				AddAlphaRegion( 6, 6, 228, 142 );

				AddHtmlLocalized( 8, 8, 228, 100, 1071021, 0x7FFF, false, false ); // You are about to disable inventory insurance auto-renewal.

				AddButton( 6, 116, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 40, 118, 450, 20, 1060051, 0x7FFF, false, false ); // CANCEL

				AddButton( 114, 116, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 148, 118, 450, 20, 1071022, 0x7FFF, false, false ); // DISABLE IT!
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				if ( !m_Player.CheckAlive() )
					return;

				if ( info.ButtonID == 1 )
				{
					m_Player.SendLocalizedMessage( 1061075, "", 0x23 ); // You have cancelled automatically reinsuring all insured items upon death
					m_Player.AutoRenewInsurance = false;
				}
				else
				{
					m_Player.SendLocalizedMessage( 1042021 ); // Cancelled.
				}
			}
		}

		#endregion

		public void GetVendor()
		{
			BaseHouse house = BaseHouse.FindHouseAt( this );

			if ( CheckAlive() && house != null && house.IsOwner( this ) && house.InternalizedVendors.Count > 0 )
			{
				CloseGump( typeof( ReclaimVendorGump ) );
				SendGump( new ReclaimVendorGump( house ) );
			}
		}

        
		private void LeaveHouse()
		{
			BaseHouse house = BaseHouse.FindHouseAt( this );

			if ( house != null )
				Location = house.BanLocation;
		}

		private delegate void ContextCallback();

        
		private class CallbackEntry : ContextMenuEntry
		{
			private readonly ContextCallback m_Callback;

			public CallbackEntry( int number, ContextCallback callback ) : this( number, -1, callback )
			{
			}

			public CallbackEntry( int number, int range, ContextCallback callback ) : base( number, range )
			{
				m_Callback = callback;
			}

			public override void OnClick()
			{
				if ( m_Callback != null )
					m_Callback();
			}
		}
        

		public override void DisruptiveAction()
		{
			if( Meditating )
			{
				RemoveBuff( BuffIcon.ActiveMeditation );
			}

			base.DisruptiveAction();
		}
		public override void OnDoubleClick( Mobile from )
		{
			if ( this != from )
				SpellHelper.Turn( from, this );

			if ( this == from && !Warmode )
			{
				IMount mount = Mount;

				if ( mount != null && !DesignContext.Check( this ) )
					return;
			}

			base.OnDoubleClick( from );
		}

        public override void DisplayPaperdollTo(Mobile to)
        {
            if (DesignContext.Check(this) && (BodyMod < 1 || this == to || to.AccessLevel >= AccessLevel.GameMaster))
                base.DisplayPaperdollTo(to);
        }

	    private static bool m_NoRecursion;

		public override bool CheckEquip( Item item )
		{
			if ( !base.CheckEquip( item ) )
				return false;

			#region Factions
			FactionItem factionItem = FactionItem.Find( item );

			if ( factionItem != null )
			{
				Faction faction = Faction.Find( this );

				if ( faction == null )
				{
					SendLocalizedMessage( 1010371 ); // You cannot equip a faction item!
					return false;
				}
			    if ( faction != factionItem.Faction )
			    {
			        SendLocalizedMessage( 1010372 ); // You cannot equip an opposing faction's item!
			        return false;
			    }
			    int maxWearables = FactionItem.GetMaxWearables( this );

			    for ( int i = 0; i < Items.Count; ++i )
			    {
			        Item equiped = Items[i];

			        if ( item != equiped && FactionItem.Find( equiped ) != null )
			        {
			            if ( --maxWearables == 0 )
			            {
			                SendLocalizedMessage( 1010373 ); // You do not have enough rank to equip more faction items!
			                return false;
			            }
			        }
			    }
			}
			#endregion

			if ( AccessLevel < AccessLevel.GameMaster && item.Layer != Layer.Mount && HasTrade )
			{
				BounceInfo bounce = item.GetBounce();

				if ( bounce != null )
				{
					if ( bounce.m_Parent is Item )
					{
						Item parent = (Item) bounce.m_Parent;

						if ( parent == Backpack || parent.IsChildOf( Backpack ) )
							return true;
					}
					else if ( bounce.m_Parent == this )
					{
						return true;
					}
				}

				SendLocalizedMessage( 1004042 ); // You can only equip what you are already carrying while you have a trade pending.
				return false;
			}

			return true;
		}

		public override bool CheckTrade( Mobile to, Item item, SecureTradeContainer cont, bool message, bool checkItems, int plusItems, int plusWeight )
		{
			int msgNum = 0;

			if ( cont == null )
			{
				if ( to.Holding != null )
					msgNum = 1062727; // You cannot trade with someone who is dragging something.
				else if ( HasTrade )
					msgNum = 1062781; // You are already trading with someone else!
				else if ( to.HasTrade )
					msgNum = 1062779; // That person is already involved in a trade
			}

			if ( msgNum == 0 )
			{
				if ( cont != null )
				{
					plusItems += cont.TotalItems;
					plusWeight += cont.TotalWeight;
				}

				if ( Backpack == null || !Backpack.CheckHold( this, item, false, checkItems, plusItems, plusWeight ) )
					msgNum = 1004040; // You would not be able to hold this if the trade failed.
				else if ( to.Backpack == null || !to.Backpack.CheckHold( to, item, false, checkItems, plusItems, plusWeight ) )
					msgNum = 1004039; // The recipient of this trade would not be able to carry this.
				else
					msgNum = CheckContentForTrade( item );
			}

			if ( msgNum != 0 )
			{
				if ( message )
					SendLocalizedMessage( msgNum );

				return false;
			}

			return true;
		}

		private static int CheckContentForTrade( Item item )
		{
			if ( item is TrapableContainer && ((TrapableContainer)item).TrapType != TrapType.None )
				return 1004044; // You may not trade trapped items.

			if ( StolenItem.IsStolen( item ) )
				return 1004043; // You may not trade recently stolen items.

			if ( item is Container )
			{
				foreach ( Item subItem in item.Items )
				{
					int msg = CheckContentForTrade( subItem );

					if ( msg != 0 )
						return msg;
				}
			}

			return 0;
		}

		public override bool CheckNonlocalDrop( Mobile from, Item item, Item target )
		{
			if ( !base.CheckNonlocalDrop( from, item, target ) )
				return false;

			if ( from.AccessLevel >= AccessLevel.GameMaster )
				return true;

			Container pack = Backpack;
			if ( from == this && HasTrade && ( target == pack || target.IsChildOf( pack ) ) )
			{
				BounceInfo bounce = item.GetBounce();

				if ( bounce != null && bounce.m_Parent is Item )
				{
					Item parent = (Item) bounce.m_Parent;

					if ( parent == pack || parent.IsChildOf( pack ) )
						return true;
				}

				SendLocalizedMessage( 1004041 ); // You can't do that while you have a trade pending.
				return false;
			}

			return true;
		}

		protected override void OnLocationChange( Point3D oldLocation )
		{
			CheckLightLevels( false );

			DesignContext context = m_DesignContext;

			if ( context == null || m_NoRecursion )
				return;

			m_NoRecursion = true;

			HouseFoundation foundation = context.Foundation;

            int newX = X, newY = Y;
            int newZ = foundation.Z + HouseFoundation.GetLevelZ(context.Level, context.Foundation);

            int startX = foundation.X + foundation.Components.Min.X + 1;
            int startY = foundation.Y + foundation.Components.Min.Y + 1;
            int endX = startX + foundation.Components.Width - 1;
            int endY = startY + foundation.Components.Height - 2;

			if ( newX >= startX && newY >= startY && newX < endX && newY < endY && Map == foundation.Map )
			{
				if ( Z != newZ )
					Location = new Point3D( X, Y, newZ );

				m_NoRecursion = false;
				return;
			}

			Location = new Point3D( foundation.X, foundation.Y, newZ );
			Map = foundation.Map;

			m_NoRecursion = false;
		}

		public override bool OnMoveOver( Mobile m )
		{
		    if (AccessLevel != AccessLevel.Player || m.AccessLevel != AccessLevel.Player)
				return true;
            if (m is BaseCreature && !((BaseCreature)m).Controlled)
                return (!Alive || !m.Alive || IsDeadBondedPet || m.IsDeadBondedPet) || (Hidden && AccessLevel > AccessLevel.Player);
            if (HiddenWithSpell && Hidden)
		        return true;
		    if (Hidden && !HiddenWithSpell && m.Alive )
		    {
		        RevealingAction();
		        m.SendAsciiMessage("You stumble upon {0}.", Name);
		        return true;
		    }
		    if (!Alive || !m.Alive || IsDeadBondedPet || m.IsDeadBondedPet)
		    {
		        if (!Alive && !Warmode)
		            m.SendAsciiMessage("You feel a tingling sensation.");

		        return true;
		    }
		    if (m.Stam >= m.StamMax * 0.1)
		    {
		        m.Stam -= 2;
		        if (m.AccessLevel == AccessLevel.Player)
		            m.SendAsciiMessage("Being perfectly rested, you shove them out of the way.");

		        return true;
		    }
		    return false;
		}

	    public override bool CheckShove( Mobile shoved )
	    {
            if (m_IgnoreMobiles || TransformationSpellHelper.UnderTransformation(shoved, typeof(WraithFormSpell)))
                return true;
	        return base.CheckShove( shoved );
	    }


	    protected override void OnMapChange( Map oldMap )
		{
			if ( (Map != Faction.Facet && oldMap == Faction.Facet) || (Map == Faction.Facet && oldMap != Faction.Facet) )
				InvalidateProperties();

			DesignContext context = m_DesignContext;

			if ( context == null || m_NoRecursion )
				return;

			m_NoRecursion = true;

			HouseFoundation foundation = context.Foundation;

			if ( Map != foundation.Map )
				Map = foundation.Map;

			m_NoRecursion = false;
		}

		public override void OnBeneficialAction( Mobile target, bool isCriminal )
		{
			if ( m_SentHonorContext != null )
				m_SentHonorContext.OnSourceBeneficialAction( target );

			base.OnBeneficialAction( target, isCriminal );
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			/*int disruptThreshold;

			if ( !Core.AOS )
				disruptThreshold = 0;
			else if ( from != null && from.Player )
				disruptThreshold = 18;
			else
				disruptThreshold = 25;*/

			if( Confidence.IsRegenerating( this ) )
				Confidence.StopRegenerating( this );

			WeightOverloading.FatigueOnDamage( this, amount );

			if ( m_ReceivedHonorContext != null )
				m_ReceivedHonorContext.OnTargetDamaged( from, amount );
			if ( m_SentHonorContext != null )
				m_SentHonorContext.OnSourceDamaged( from, amount );

            if (willKill && from is PlayerMobile)
                Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(((PlayerMobile)from).RecoverAmmo));

			base.OnDamage( amount, from, willKill );
		}

        public void ForceResurrect()
        {
            bool wasAlive = Alive;
            base.Resurrect();

            Corpse corpse = null;

            //Taran: How the hell can this be null sometimes? Crashed because of that. 
            //Hmm could be if a custom region tries to res and tele someone when they
            //are alive, fixed that but kept this just in case.
            if (Map != null)
            {
                IPooledEnumerable eable = Map.GetItemsInRange(Location, 2);

                foreach (Item item in eable)
                {
                    if (item is Corpse && item.ItemID != 0xECA && item.ItemID != 9)
                    {
                        Corpse tempCorpse = item as Corpse;
                        int tempCorpseCount = tempCorpse.Items.Count;
                        int OutNumber = -1;

                        if ((tempCorpse.Owner == this) && (tempCorpseCount > OutNumber))
                            corpse = tempCorpse;
                    }
                }
            }

            bool hascorpse = ((corpse != null) && (corpse.ItemID != 0xECA) && (corpse.ItemID != 91) && !corpse.Carved &&
                              InRange(corpse, 2));

            if (hascorpse)
            {
                List<Item> itemList = new List<Item>(corpse.Items);

                foreach (Item item in itemList)
                {
                    if (corpse.EquipItems.Contains(item))
                        EquipItem(item);
                    else
                        AddToBackpack(item);
                }
                corpse.Delete();
            }
            else
            {
                ResRobe rr = new ResRobe();

                if (FindItemOnLayer(rr.Layer) == null && !IsInEvent)
                    EquipItem(rr);
                else
                    rr.Delete();
            }

            SendLocalizedMessage(500116);

            if (Alive && !wasAlive)
            {
                if (hascorpse)
                {
                    Animate(21, 6, 1, false, false, 0);
                    Effects.SendLocationEffect(Location, Map, 0x376A, 16, 10);
                    PlaySound(0x214);
                }
            }
            Mana = m_OldMana;
            Stam = m_OldStam;

            if (CurrentEvent != null)
                CurrentEvent.OnPlayerResurrect(this);

            //Put the newbied items in the same place as they were before you died
            if (m_ItemLocations != null)
            {
                foreach (Item item in m_ItemLocations.Keys)
                {
                    item.X = m_ItemLocations[item].X;
                    item.Y = m_ItemLocations[item].Y;
                }
            }
        }

        public override void Resurrect()
        {
            CustomRegion cR;

            if ((cR = Region as CustomRegion) != null && AccessLevel == AccessLevel.Player && !cR.Controller.CanRessurect)
            {
                SendAsciiMessage("You cannot be resurrected here!");
                return;
            }

            bool wasAlive = Alive;
            base.Resurrect();

            Corpse corpse = null;

            //Taran: How the hell can this be null sometimes? Crashed because of that. 
            //Hmm could be if a custom region tries to res and tele someone when they
            //are alive, fixed that but kept this just in case.
            if (Map != null) 
            {
                IPooledEnumerable eable = Map.GetItemsInRange(Location, 2);

                foreach (Item item in eable)
                {
                    if (item is Corpse && item.ItemID != 0xECA && item.ItemID != 9)
                    {
                        Corpse tempCorpse = item as Corpse;
                        int tempCorpseCount = tempCorpse.Items.Count;
                        int OutNumber = -1;

                        if ((tempCorpse.Owner == this) && (tempCorpseCount > OutNumber))
                        {
                            corpse = tempCorpse;
                            //OutNumber = tempCorpseCount;
                        } //break;
                    }
                }
            }

            bool hascorpse = ((corpse != null) && (corpse.ItemID != 0xECA) && (corpse.ItemID != 91) && !corpse.Carved &&
                              InRange(corpse, 2));

            if (hascorpse)
            {
                List<Item> itemList = new List<Item>(corpse.Items);

                foreach (Item item in itemList)
                {
                    if (corpse.EquipItems.Contains(item))
                        EquipItem(item);
                    else
                        AddToBackpack(item);
                }
                corpse.Delete();
            }
            else
            {
                ResRobe rr = new ResRobe();

                if (FindItemOnLayer(rr.Layer) == null && !IsInEvent)
                    EquipItem(rr);
                else
                    rr.Delete();
            }

            SendLocalizedMessage(500116);

            if (Alive && !wasAlive)
            {
                if (hascorpse)
                {
                    Animate(21, 6, 1, false, false, 0);
                    Effects.SendLocationEffect(Location, Map, 0x376A, 16, 10);
                    PlaySound(0x214);
                }
            }


            Mana = m_OldMana;
            Stam = m_OldStam;

            if (CurrentEvent != null)
                CurrentEvent.OnPlayerResurrect(this);

            //Put the newbied items in the same place as they were before you died
            if (m_ItemLocations != null)
            {
                foreach (Item item in m_ItemLocations.Keys)
                {
                    item.X = m_ItemLocations[item].X;
                    item.Y = m_ItemLocations[item].Y;
                }
            }
        }


		public override double RacialSkillBonus
		{
			get
			{
				if( Core.ML && Race == Race.Human )
					return 20.0;

				return 0;
			}
		}

		public override bool EquipItem( Item item )
		{
			if( item == null || item.Deleted || !item.CanEquip( this ) )
				return false;

			if( CheckEquip( item ) && OnEquip( item ) && item.OnEquip( this ) )
			{
				if( FindItemOnLayer( item.Layer ) != null )
					AddToBackpack( FindItemOnLayer( item.Layer ) );

				AddItem( item );
				return true;
			}

			return false;
		}

		private Mobile m_InsuranceAward;
		private int m_InsuranceCost;
		private int m_InsuranceBonus;

        private List<Item> m_EquipSnapshot;

        public List<Item> EquipSnapshot
        {
            get { return m_EquipSnapshot; }
        }

        private bool FindItems_Callback(Item item)
        {
            if (!item.Deleted && (item.LootType == LootType.Blessed || item.LootType == LootType.Newbied || item.Insured ))
            {
                if (Backpack != item.ParentEntity)
                {
                    if (item.ParentEntity is Container)
                    {
                        Container cont = item.ParentEntity as Container;
                        if (cont.LootType != LootType.Blessed && cont.LootType != LootType.Newbied && !cont.Insured)
                            return true;
                    }
                }
            }
            return false;
        }

		public override bool OnBeforeDeath()
		{
		    PolymorphSpell.StopTimer(this);

            m_OldMana = Mana;
            m_OldStam = Stam;

            NetState state = NetState;

            if (state != null)
                state.CancelAllTrades();

            DropHolding();

            if (Backpack != null && !Backpack.Deleted)
            {
                List<Item> ilist = Backpack.FindItemsByType<Item>(FindItems_Callback);
                for (int i = 0; i < ilist.Count; i++)
                {
                    Backpack.AddItem(ilist[i]);
                }

                m_ItemLocations = new Dictionary<Item, Point2D>();
                for (int i = 0; i < Backpack.Items.Count; i++)
                {
                    Item item = Backpack.Items[i];

                    //Taran: Empty reward can on death
                    if (item is MiniRewardCan)
                    {
                        MiniRewardCan mrc = item as MiniRewardCan;
                        if (mrc.trashedItem != null)
                            mrc.trashedItem.Delete();
                    }

                    //Save location of the item if it's newbied
                    if (item.LootType == LootType.Blessed || item.LootType == LootType.Newbied || AccessLevel > AccessLevel.Player)
                        m_ItemLocations[item] = new Point2D(item.X, item.Y);
                }
            }

            m_EquipSnapshot = new List<Item>(this.Items);

            m_NonAutoreinsuredItems = 0;
			m_InsuranceCost = 0;
			m_InsuranceAward = FindMostRecentDamager( false );

			if ( m_InsuranceAward is BaseCreature )
			{
				Mobile master = ((BaseCreature)m_InsuranceAward).GetMaster();

				if ( master != null )
					m_InsuranceAward = master;
			}

			if ( m_InsuranceAward != null && (!m_InsuranceAward.Player || m_InsuranceAward == this) )
				m_InsuranceAward = null;

			if ( m_InsuranceAward is PlayerMobile )
				((PlayerMobile)m_InsuranceAward).m_InsuranceBonus = 0;

			if ( m_ReceivedHonorContext != null )
				m_ReceivedHonorContext.OnTargetKilled();
			if ( m_SentHonorContext != null )
				m_SentHonorContext.OnSourceKilled();

            RecoverAmmo();

			return base.OnBeforeDeath();
		}

		private bool CheckInsuranceOnDeath( Item item )
		{
			if ( InsuranceEnabled && item.Insured )
			{
				if ( AutoRenewInsurance )
				{
					int cost = ( m_InsuranceAward == null ? 600 : 300 );

					if ( Banker.Withdraw( this, cost ) )
					{
						m_InsuranceCost += cost;
						item.PayedInsurance = true;
                        SendLocalizedMessage(1060398, cost.ToString()); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
					}
					else
					{
						SendLocalizedMessage( 1061079, "", 0x23 ); // You lack the funds to purchase the insurance
						item.PayedInsurance = false;
						item.Insured = false;
                        m_NonAutoreinsuredItems++;
					}
				}
				else
				{
					item.PayedInsurance = false;
					item.Insured = false;
				}

				if ( m_InsuranceAward != null )
				{
					if ( Banker.Deposit( m_InsuranceAward, 300 ) )
					{
						if ( m_InsuranceAward is PlayerMobile )
							((PlayerMobile)m_InsuranceAward).m_InsuranceBonus += 300;
					}
				}

				return true;
			}

			return false;
		}

        public override DeathMoveResult GetParentMoveResultFor(Item item)
        {
            if (CheckInsuranceOnDeath(item))
                return DeathMoveResult.MoveToBackpack;

            DeathMoveResult res = base.GetParentMoveResultFor(item);

            if (res == DeathMoveResult.MoveToCorpse && item.Movable && Young)
                res = DeathMoveResult.MoveToBackpack;

            return res;
        }

		public override DeathMoveResult GetInventoryMoveResultFor( Item item )
		{
			if( CheckInsuranceOnDeath( item ) )
				return DeathMoveResult.MoveToBackpack;

			DeathMoveResult res = base.GetInventoryMoveResultFor( item );

			if( res == DeathMoveResult.MoveToCorpse && item.Movable && Young )
				res = DeathMoveResult.MoveToBackpack;

			return res;
		}

		public override void OnDeath( Container c )
		{
            if (m_NonAutoreinsuredItems > 0)
            {
                SendLocalizedMessage(1061115);
            }

			base.OnDeath( c );

            m_EquipSnapshot = null;

			HueMod = -1;
			NameMod = null;
			SavagePaintExpiration = TimeSpan.Zero;

			SetHairMods( -1, -1 );

			PolymorphSpell.StopTimer( this );
			IncognitoSpell.StopTimer( this );
            DisguiseTimers.RemoveTimer(this);

			EndAction( typeof( PolymorphSpell ) );
			EndAction( typeof( IncognitoSpell ) );
            EndAction( typeof(IAction) );

			MeerMage.StopEffect( this, false );

			StolenItem.ReturnOnDeath( this, c );

			if ( m_PermaFlags.Count > 0 )
			{
				m_PermaFlags.Clear();

				if ( c is Corpse )
					((Corpse)c).Criminal = true;

				if ( Stealing.ClassicMode )
					Criminal = true;
			}
            /*
			if ( Kills >= 5 && DateTime.Now >= m_NextJustAward )
			{
				var m = FindMostRecentDamager( false );

				if( m is BaseCreature )
					m = ((BaseCreature)m).GetMaster();

				if ( m != null && m is PlayerMobile && m != this )
				{
					var gainedPath = false;

					var pointsToGain = 0;

					pointsToGain += (int) Math.Sqrt( GameTime.TotalSeconds * 4 );
					pointsToGain *= 5;
					pointsToGain += (int) Math.Pow( Skills.Total / 250, 2 );

					if ( VirtueHelper.Award( m, VirtueName.Justice, pointsToGain, ref gainedPath ) )
					{
						if ( gainedPath )
							m.SendLocalizedMessage( 1049367 ); // You have gained a path in Justice!
						else
							m.SendLocalizedMessage( 1049363 ); // You have gained in Justice.

						m.FixedParticles( 0x375A, 9, 20, 5027, EffectLayer.Waist );
						m.PlaySound( 0x1F7 );

						m_NextJustAward = DateTime.Now + TimeSpan.FromMinutes( pointsToGain / 3 );
					}
				}
			}
            
			if ( m_InsuranceAward is PlayerMobile )
			{
				var pm = (PlayerMobile)m_InsuranceAward;

				if ( pm.m_InsuranceBonus > 0 )
					pm.SendLocalizedMessage( 1060397, pm.m_InsuranceBonus.ToString() ); // ~1_AMOUNT~ gold has been deposited into your bank box.
			}
            */

            Mobile killer = FindMostRecentDamager( true );

			if ( killer is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)killer;

				Mobile master = bc.GetMaster();
				if( master != null )
					killer = master;
			}

			if ( Young )
			{
                Timer.DelayCall(TimeSpan.FromSeconds(2.5), new TimerCallback(SendYoungDeathNotice));
                Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(SendYoungTeleportGump));
			}

			Faction.HandleDeath( this, killer );

			Guilds.Guild.HandleDeath( this, killer );

			if( m_BuffTable != null )
			{
				List<BuffInfo> list = new List<BuffInfo>();

				foreach( BuffInfo buff in m_BuffTable.Values )
				{
					if( !buff.RetainThroughDeath )
					{
						list.Add( buff );
					}
				}

				for( int i = 0; i < list.Count; i++ )
				{
					RemoveBuff( list[i] );
				}
			}

            //Auto cut "item id" fix when a player is not in an event
            if (m_CurrentEvent == null && !PvpCore.IsInDeathmatch(this) && !TournamentCore.IsInTournament(this))
            {
                if (c != null)
                {
                    if (c is Corpse && !((Corpse) c).Carved)
                    {
                        FakeCorpse.CreateCorpses(this);

                        Timer.DelayCall(TimeSpan.FromSeconds(3.0), TimeSpan.FromSeconds(3.0), 1, new TimerStateCallback(ResetItemID), c);
                        c.ItemID = (0xED4 + Utility.Random(2000));
                    }
                }
            }

		    // Yankovic - Calculate new rating
            if (killer is PlayerMobile && killer != this)
            {
                Events.CalculateRating((PlayerMobile)killer, this);
            }

            if (m_CurrentEvent != null)
                m_CurrentEvent.OnPlayerDeath(this);

            if (IsInEvent && c is Corpse)
                c.EventItem = true; //So that other players in events can loot your body if configured that way
		}

        private static void ResetItemID(object state)
        {
            Corpse c = (Corpse)state;

            if (c == null || c.Carved)
                return;

            //Send the proper body ID and and displays proper image.
            c.ProcessDelta();
            c.SendRemovePacket();
            c.ItemID = 0x2006;
            c.ProcessDelta();
        }

		private List<Mobile> m_PermaFlags;
		private readonly List<Mobile> m_VisList;
		private readonly Hashtable m_AntiMacroTable;
	    private List<Mobile> m_Snoopers;
		private TimeSpan m_GameTime;
		private TimeSpan m_ShortTermElapse;
		private TimeSpan m_LongTermElapse;
		private DateTime m_SessionStart;
        private DateTime m_LastPetBallTime;
	    private DateTime m_NextSmithBulkOrder;
		private DateTime m_NextTailorBulkOrder;
		private DateTime m_SavagePaintExpiration;
		private SkillName m_Learning = (SkillName)(-1);

		public SkillName Learning
		{
			get{ return m_Learning; }
			set{ m_Learning = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan SavagePaintExpiration
		{
			get
			{
				TimeSpan ts = m_SavagePaintExpiration - DateTime.Now;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				m_SavagePaintExpiration = DateTime.Now + value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NextSmithBulkOrder
		{
			get
			{
				TimeSpan ts = m_NextSmithBulkOrder - DateTime.Now;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				try{ m_NextSmithBulkOrder = DateTime.Now + value; }
				catch{}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NextTailorBulkOrder
		{
			get
			{
				TimeSpan ts = m_NextTailorBulkOrder - DateTime.Now;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				try{ m_NextTailorBulkOrder = DateTime.Now + value; }
				catch{}
			}
		}

	    [CommandProperty(AccessLevel.GameMaster)]
	    public DateTime LastEscortTime { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastPetBallTime
        {
            get { return m_LastPetBallTime; }
            set { m_LastPetBallTime = value; }
        }

	    public PlayerMobile()
		{
            m_AutoStabled = new List<Mobile>();

			m_VisList = new List<Mobile>();
			m_PermaFlags = new List<Mobile>();
			m_AntiMacroTable = new Hashtable();
            m_Snoopers = new List<Mobile>();

			m_BOBFilter = new BOBFilter();

			m_GameTime = TimeSpan.Zero;
			m_ShortTermElapse = TimeSpan.FromHours( 8.0 );
			m_LongTermElapse = TimeSpan.FromHours( 40.0 );

			m_JusticeProtectors = new List<Mobile>();
			m_GuildRank = RankDefinition.Lowest;

			m_ChampionTitles = new ChampionTitleInfo();

			InvalidateMyRunUO();
		}

		public override bool MutateSpeech( List<Mobile> hears, ref string text, ref object context )
		{
            if (Alive)
                return false;

            if (Skills[SkillName.SpiritSpeak].Value >= 100.0)
                return false;

			if ( Core.AOS )
			{
				for ( int i = 0; i < hears.Count; ++i )
				{
					Mobile m = hears[i];

					if ( m != this && m.Skills[SkillName.SpiritSpeak].Value >= 100.0 )
						return false;
				}
			}

			return base.MutateSpeech( hears, ref text, ref context );
		}

        public override void  OnSaid(SpeechEventArgs e)
        {
			if( Squelched )
			{
			    SendMessage( "You can not say anything, you have been squelched." ); //Cliloc ITSELF changed during ML.
				e.Blocked = true;
			}

			if( !e.Blocked && !HiddenWithSpell )
				RevealingAction();
        }

        public override void DoSpeech(string text, int[] keywords, MessageType type, int hue)
        {
            /*if (Guilds.Guild.NewGuildSystem && (type == MessageType.Guild || type == MessageType.Alliance))
            {
                var g = Guild as Guild;
                if (g == null)
                {
                    SendLocalizedMessage(1063142); // You are not in a guild!
                }
                else if (type == MessageType.Alliance)
                {
                    if (g.Alliance != null && g.Alliance.IsMember(g))
                    {
                        //g.Alliance.AllianceTextMessage( hue, "[Alliance][{0}]: {1}", this.Name, text );
                        g.Alliance.AllianceChat(this, text);
                        SendToStaffMessage(this, "[Alliance]: {0}", text);

                        AllianceMessageHue = hue;
                    }
                    else
                    {
                        SendLocalizedMessage(1071020); // You are not in an alliance!
                    }
                }
                else //Type == MessageType.Guild
                {
                    GuildMessageHue = hue;

                    g.GuildChat(this, text);
                    SendToStaffMessage(this, "[Guild]: {0}", text);
                }
            }*/
            if (Map != null)
            {
                if (Deleted || Commands.CommandSystem.Handle(this, text, type))
                    return;

                if (WopLock == null)
                    WopLock = new WopLock();

                if (!WopLock.CanWop(this, false))
                    return;

                int range = 15;

                switch (type)
                {
                    case MessageType.Regular:
                        SpeechHue = hue;
                        break;
                    case MessageType.Emote:
                        EmoteHue = hue;
                        break;
                    case MessageType.Whisper:
                        WhisperHue = hue;
                        range = 1;
                        break;
                    case MessageType.Yell:
                        YellHue = hue;
                        range = 18;
                        break;
                    default:
                        type = MessageType.Regular;
                        break;
                }

                SpeechEventArgs regArgs = new SpeechEventArgs(this, text, type, hue, keywords);

                EventSink.InvokeSpeech(regArgs);
                Region.OnSpeech(regArgs);
                OnSaid(regArgs);

                if (regArgs.Blocked)
                    return;

                text = regArgs.Speech;

                if (string.IsNullOrEmpty(text))
                    return;

                if (m_Hears == null)
                    m_Hears = new List<Mobile>();
                else if (m_Hears.Count > 0)
                    m_Hears.Clear();

                if (m_OnSpeech == null)
                    m_OnSpeech = new ArrayList();
                else if (m_OnSpeech.Count > 0)
                    m_OnSpeech.Clear();

                List<Mobile> hears = m_Hears;
                ArrayList onSpeech = m_OnSpeech;

                IPooledEnumerable eable = Map.GetObjectsInRange(Location, range);

                foreach (object o in eable)
                {
                    if (o is Mobile)
                    {
                        Mobile heard = (Mobile)o;

                        if ( (heard.CanSee(this) || ((!Alive || HiddenWithSpell))) && (!heard.HasFilter || InLOS(heard)))
                        {
                            if (heard.NetState != null)
                                hears.Add(heard);

                            if (heard.HandlesOnSpeech(this))
                                onSpeech.Add(heard);

                            for (int i = 0; i < heard.Items.Count; ++i)
                            {
                                Item item = heard.Items[i];

                                if (item.HandlesOnSpeech)
                                    onSpeech.Add(item);

                                if (item is Container)
                                    AddSpeechItemsFrom(onSpeech, (Container)item);
                            }
                        }
                    }
                    else if (o is Item)
                    {
                        if (((Item)o).HandlesOnSpeech)
                            onSpeech.Add(o);

                        if (o is Container)
                            AddSpeechItemsFrom(onSpeech, (Container)o);
                    }
                }

                eable.Free();

                object mutateContext = null;
                string mutatedText = text;
                SpeechEventArgs mutatedArgs = null;

                if (MutateSpeech(hears, ref mutatedText, ref mutateContext))
                    mutatedArgs = new SpeechEventArgs(this, mutatedText, type, hue, new int[0]);

                //Don't go to warmode when speaking as a ghost
                //CheckSpeechManifest();

                ProcessDelta();

                Packet regp = null;
                Packet mutp = null;
                Packet regp2 = null;
                Packet mutp2 = null;

                if (hue > 670 && hue <= 1001) //Taran: Block speechhue that can look like powerwords
                    hue = 902;

                //Can be checked in the for loop, but uses a lot more resources
                if (UseUnicodeSpeech)
                {
                    for (int i = 0; i < hears.Count; ++i)
                    {
                        Mobile heard = hears[i];
                        NetState ns = heard.NetState;

                        if (ns == null)
                            continue;

                        if (mutatedArgs == null || !CheckHearsMutatedSpeech(heard, mutateContext) || heard.Skills.SpiritSpeak.Value >= 100.0)
                        {
                            heard.OnSpeech(regArgs);

                            if (heard.CanSee(this))
                            {
                                if (regp == null)
                                    regp = Packet.Acquire(new UnicodeMessage(Serial, Body, type, hue, 3, Language, Name, text));

                                ns.Send(regp);
                            }
                            else
                            {
                                if (regp2 == null)
                                    regp2 = Packet.Acquire(new UnicodeMessage(Serial, Body, type, hue, 3, Language, Name, Name + ": " + text));

                                ns.Send(regp2);
                            }
                        }
                        else
                        {
                            heard.OnSpeech(mutatedArgs);

                            if (heard.CanSee(this))
                            {
                                if (mutp == null)
                                    mutp = Packet.Acquire(new UnicodeMessage(Serial, Body, type, hue, 3, Language, Name, mutatedText));

                                ns.Send(mutp);
                            }
                            else
                            {
                                if (mutp2 == null)
                                    mutp2 = Packet.Acquire((new UnicodeMessage(Serial, Body, type, hue, 3, Language, Name, Name + ": " + mutatedText)));

                                ns.Send(mutp2);
                            }

                            heard.SendSound(Utility.RandomMinMax(382, 385));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < hears.Count; ++i)
                    {
                        Mobile heard = hears[i];
                        NetState ns = heard.NetState;

                        if (ns == null)
                            continue;

                        if (mutatedArgs == null || !CheckHearsMutatedSpeech(heard, mutateContext) || heard.Skills.SpiritSpeak.Value >= 100.0)
                        {
                            heard.OnSpeech(regArgs);

                            if (heard.CanSee(this))
                            {
                                if (regp == null)
                                    regp = Packet.Acquire(new AsciiMessage(Serial, Body, type, hue, 3, Name, text));

                                ns.Send(regp);
                            }
                            else
                            {
                                if (regp2 == null)
                                    regp2 = Packet.Acquire(new AsciiMessage(Serial, Body, type, hue, 3, Name, Name + ": " + text));

                                ns.Send(regp2);
                            }
                        }
                        else
                        {
                            heard.OnSpeech(mutatedArgs);

                            if (heard.CanSee(this))
                            {
                                if (mutp == null)
                                    mutp = Packet.Acquire(new AsciiMessage(Serial, Body, type, hue, 3, Name, mutatedText));

                                ns.Send(mutp);
                            }
                            else
                            {
                                if (mutp2 == null)
                                    mutp2 = Packet.Acquire((new AsciiMessage(Serial, Body, type, hue, 3, Name, Name + ": " + mutatedText)));

                                ns.Send(mutp2);
                            }

                            heard.SendSound(Utility.RandomMinMax(382, 385));
                        }
                    }
                }

                Packet.Release(regp);
                Packet.Release(mutp);

                if (onSpeech.Count > 1)
                    onSpeech.Sort(LocationComparer.GetInstance(this));

                for (int i = 0; i < onSpeech.Count; ++i)
                {
                    object obj = onSpeech[i];

                    if (obj is Mobile)
                    {
                        Mobile heard = (Mobile)obj;

                        if (mutatedArgs == null || !CheckHearsMutatedSpeech(heard, mutateContext) || heard.Skills.SpiritSpeak.Value >= 100.0)
                            heard.OnSpeech(regArgs);
                        else
                            heard.OnSpeech(mutatedArgs);
                    }
                    else
                    {
                        Item item = (Item)obj;

                        item.OnSpeech(regArgs);
                    }
                }             
            }
        }

		private static List<Mobile> m_Hears;
		private static ArrayList m_OnSpeech;

		private static void SendToStaffMessage( Mobile from, string text )
		{
			Packet p = null;

			foreach( NetState ns in from.GetClientsInRange( 8 ) )
			{
				Mobile mob = ns.Mobile;

				if( mob != null && mob.AccessLevel >= AccessLevel.GameMaster && mob.AccessLevel > from.AccessLevel )
				{
					if( p == null )
						p = Packet.Acquire( new UnicodeMessage( from.Serial, from.Body, MessageType.Regular, from.SpeechHue, 3, from.Language, from.Name, text ) );

					ns.Send( p );
				}
			}

			Packet.Release( p );
		}
		private static void SendToStaffMessage( Mobile from, string format, params object[] args )
		{
			SendToStaffMessage( from, String.Format( format, args ) );
		}

		public override void Damage( int amount, Mobile from )
		{
            if (EvilOmenSpell.TryEndEffect(this))
                amount = (int)(amount * 1.25);

			Mobile oath = BloodOathSpell.GetBloodOath( from );

                /* Per EA's UO Herald Pub48 (ML): 
                * ((resist spellsx10)/20 + 10=percentage of damage resisted)
                */

			if ( oath == this )
			{
				amount = (int)(amount * 1.1);

                if (amount > 35 && from is PlayerMobile)  /* capped @ 35, seems no expansion */
                {
                    amount = 35;
                }

                if (Core.ML)
                {
                    from.Damage((int)(amount * (1 - (((from.Skills.MagicResist.Value * .5) + 10) / 100))), this);
                }
                else
                {
                    from.Damage(amount, this);
                }
            }

            if (from != null && Talisman is BaseTalisman)
            {
                BaseTalisman talisman = (BaseTalisman)Talisman;

                if (talisman.Protection != null && talisman.Protection.Type != null)
                {
                    Type type = talisman.Protection.Type;

                    if (type == from.GetType())
                        amount *= 1 - (int)(((double)talisman.Protection.Amount) / 100);
                }
            }

			base.Damage( amount, from );
		}

		#region Poison
		public override ApplyPoisonResult ApplyPoison( Mobile from, Poison poison )
		{
			if ( !Alive )
				return ApplyPoisonResult.Immune;

            if (Spells.Necromancy.EvilOmenSpell.TryEndEffect(this))
                poison = PoisonImpl.IncreaseLevel(poison);

			ApplyPoisonResult result = base.ApplyPoison( from, poison );

			if ( from != null && result == ApplyPoisonResult.Poisoned && PoisonTimer is PoisonImpl.PoisonTimer )
				( (PoisonImpl.PoisonTimer)PoisonTimer ).From = from;

			return result;
		}

		public override bool CheckPoisonImmunity( Mobile from, Poison poison )
		{
			if ( Young )
				return true;

			return base.CheckPoisonImmunity( from, poison );
		}

		public override void OnPoisonImmunity( Mobile from, Poison poison )
		{
			if ( Young )
				SendLocalizedMessage( 502808 ); // You would have been poisoned, were you not new to the land of Britannia. Be careful in the future.
			else
				base.OnPoisonImmunity( from, poison );
		}
		#endregion

		public PlayerMobile( Serial s ) : base( s )
		{
			m_VisList = new List<Mobile>();
			m_AntiMacroTable = new Hashtable();
			InvalidateMyRunUO();
		}

		public List<Mobile> VisibilityList
		{
			get{ return m_VisList; }
		}

		public List<Mobile> PermaFlags
		{
			get{ return m_PermaFlags; }
		}

	    public List<Mobile> Snoopers
	    {
            get { return m_Snoopers; }
	    }

		public override int Luck{ get{ return AosAttributes.GetValue( this, AosAttribute.Luck ); } }

		public override bool IsHarmfulCriminal( Mobile target )
		{
			if ( Stealing.ClassicMode && target is PlayerMobile && ((PlayerMobile)target).m_PermaFlags.Count > 0 )
			{
				int noto = Notoriety.Compute( this, target );

				if ( noto == Notoriety.Innocent )
					target.Delta( MobileDelta.Noto );

				return false;
			}

			if ( target is BaseCreature && ((BaseCreature)target).InitialInnocent && !((BaseCreature)target).Controlled )
				return false;

            if (Core.ML && target is BaseCreature && ((BaseCreature)target).Controlled && this == ((BaseCreature)target).ControlMaster)
                return false;

            //bounty system
			if( BountyBoard.Attackable( this, target ) )
				return false;
			//end bounty system

			return base.IsHarmfulCriminal( target );
		}

		public bool AntiMacroCheck( Skill skill, object obj )
		{
			if ( obj == null || m_AntiMacroTable == null || AccessLevel != AccessLevel.Player )
				return true;

			Hashtable tbl = (Hashtable)m_AntiMacroTable[skill];
			if ( tbl == null )
				m_AntiMacroTable[skill] = tbl = new Hashtable();

			CountAndTimeStamp count = (CountAndTimeStamp)tbl[obj];
			if ( count != null )
			{
				if ( count.TimeStamp + SkillCheck.AntiMacroExpire <= DateTime.Now )
				{
					count.Count = 1;
					return true;
				}
			    ++count.Count;
			    if ( count.Count <= SkillCheck.Allowance )
			        return true;
			    return false;
			}
		    tbl[obj] = count = new CountAndTimeStamp();
		    count.Count = 1;

		    return true;
		}

		private void RevertHair()
		{
			SetHairMods( -1, -1 );
		}

		private BOBFilter m_BOBFilter;

		public BOBFilter BOBFilter
		{
			get{ return m_BOBFilter; }
		}
        
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			switch ( version )
			{
                case 44:
			        {
			            AntiMacroGump = reader.ReadBool();
			            goto case 43;
			        }
                case 43:
                    {
                        ShowTipsOnLogin = reader.ReadBool();
                        goto case 42;
                    }
                case 42:
			        {
			            AllowMulti = reader.ReadBool();
			            goto case 41;
			        }
                case 41:
                    {
                        GameInfoGumpType = (GameInfoGumpType)reader.ReadInt();
                        goto case 40;
                    }
                case 40:
                    {
                        m_EventType = (EventType)reader.ReadByte();
                        m_CurrentEvent = reader.ReadItem() as BaseGame;
                        goto case 39;
                    }
                case 39:
			        {
			            TempCheck = reader.ReadBool();
			            goto case 38;
			        }
                case 38:
                    {
                        m_AutoStabled = reader.ReadStrongMobileList();
                        m_AnkhNextUse = reader.ReadDateTime();
                        m_PeacedUntil = reader.ReadDateTime();
                        goto case 37;
                    }
                case 37:
			        {
			            ShowArriveMsg = reader.ReadBool();
			            goto case 36;
			        }
                case 36:
                    {
                        m_Rating = reader.ReadInt();
                        m_TournamentRating = reader.ReadInt();
                        m_CurrentEvent = null;
                        goto case 35;
                    }
                case 35:
                    {
                        m_Snoopers = reader.ReadStrongMobileList();
                        goto case 34;
                    }
                case 34:
                    m_HasStartingSkillBoost = reader.ReadBool();
                    goto case 33;
                case 33:
			        {
			            m_OriginalHairHue = reader.ReadInt();
			            m_OriginalHairItemID = reader.ReadInt();
			            m_OriginalHue = reader.ReadInt();
			            m_HasCustomRace = reader.ReadBool();
			            goto case 32;
			        }
                case 32:
                    VirtualArmor = reader.ReadInt();
                    goto case 31;
                case 31:
                    m_PlayerGuildGameTime = reader.ReadTimeSpan();
                    goto case 30;
                case 30:
                    m_AlwaysMurderer = reader.ReadBool();
                    goto case 29;
                case 29:
                    m_Imported = reader.ReadBool();
                    goto case 28;
                case 28:
                    /*Item nullItem = reader.ReadItem();
                    goto case 27;*/
                case 27:
                    m_Stoned = reader.ReadBool();
                    goto case 26;
				case 26:
					UseUnicodeSpeech = reader.ReadBool();
					goto case 25;
				case 25:
				{
					int recipeCount = reader.ReadInt();

					if( recipeCount > 0 )
					{
						m_AcquiredRecipes = new Dictionary<int, bool>();

						for( int i = 0; i < recipeCount; i++ )
						{
							int r = reader.ReadInt();
							if( reader.ReadBool() )	//Don't add in recipies which we haven't gotten or have been removed
								m_AcquiredRecipes.Add( r, true );
						}
					}
					goto case 24;
				}
				case 24:
				{
					m_LastHonorLoss = reader.ReadDeltaTime();
					goto case 23;
				}
				case 23:
				{
					m_ChampionTitles = new ChampionTitleInfo( reader );
					goto case 22;
				}
				case 22:
				{
					m_LastValorLoss = reader.ReadDateTime();
					goto case 21;
				}
				case 21:
				{
					m_ToTItemsTurnedIn = reader.ReadEncodedInt();
					m_ToTTotalMonsterFame = reader.ReadInt();
					goto case 20;
				}
				case 20:
				{
					AllianceMessageHue = reader.ReadEncodedInt();
					GuildMessageHue = reader.ReadEncodedInt();

					goto case 19;
				}
				case 19:
				{
					int rank = reader.ReadEncodedInt();
					int maxRank = RankDefinition.Ranks.Length -1;
					if( rank > maxRank )
						rank = maxRank;

					m_GuildRank = RankDefinition.Ranks[rank];
					LastOnline = reader.ReadDateTime();
					goto case 18;
				}
				case 18:
				{
					m_SolenFriendship = (SolenFriendship) reader.ReadEncodedInt();

					goto case 17;
				}
				case 17: // changed how DoneQuests is serialized
				case 16:
				{
					m_Quest = QuestSerializer.DeserializeQuest( reader );

					if ( m_Quest != null )
						m_Quest.From = this;

					int count = reader.ReadEncodedInt();

					if ( count > 0 )
					{
						m_DoneQuests = new List<QuestRestartInfo>();

						for ( int i = 0; i < count; ++i )
						{
							Type questType = QuestSerializer.ReadType( QuestSystem.QuestTypes, reader );
							DateTime restartTime;

							if ( version < 17 )
								restartTime = DateTime.MaxValue;
							else
								restartTime = reader.ReadDateTime();

							m_DoneQuests.Add( new QuestRestartInfo( questType, restartTime ) );
						}
					}

					Profession = reader.ReadEncodedInt();
					goto case 15;
				}
				case 15:
				{
					m_LastCompassionLoss = reader.ReadDeltaTime();
					goto case 14;
				}
				case 14:
				{
					m_CompassionGains = reader.ReadEncodedInt();

					if ( m_CompassionGains > 0 )
						m_NextCompassionDay = reader.ReadDeltaTime();

					goto case 13;
				}
				case 13: // just removed m_PayedInsurance list
				case 12:
				{
					m_BOBFilter = new BOBFilter( reader );
					goto case 11;
				}
				case 11:
				{
					if ( version < 13 )
					{
						List<Item> payed = reader.ReadStrongItemList();

						for ( int i = 0; i < payed.Count; ++i )
							payed[i].PayedInsurance = true;
					}

					goto case 10;
				}
				case 10:
				{
					if ( reader.ReadBool() )
					{
						m_HairModID = reader.ReadInt();
						m_HairModHue = reader.ReadInt();
						m_BeardModID = reader.ReadInt();
						m_BeardModHue = reader.ReadInt();
					}

					goto case 9;
				}
				case 9:
				{
					SavagePaintExpiration = reader.ReadTimeSpan();

					if ( SavagePaintExpiration > TimeSpan.Zero )
					{
						BodyMod = ( Female ? 184 : 183 );
						HueMod = 0;
					}

					goto case 8;
				}
				case 8:
				{
					NpcGuild = (NpcGuild)reader.ReadInt();
					NpcGuildJoinTime = reader.ReadDateTime();
					NpcGuildGameTime = reader.ReadTimeSpan();
					goto case 7;
				}
				case 7:
				{
					m_PermaFlags = reader.ReadStrongMobileList();
					goto case 6;
				}
				case 6:
				{
					NextTailorBulkOrder = reader.ReadTimeSpan();
					goto case 5;
				}
				case 5:
				{
					NextSmithBulkOrder = reader.ReadTimeSpan();
					goto case 4;
				}
				case 4:
				{
					m_LastJusticeLoss = reader.ReadDeltaTime();
					m_JusticeProtectors = reader.ReadStrongMobileList();
					goto case 3;
				}
				case 3:
				{
					m_LastSacrificeGain = reader.ReadDeltaTime();
					m_LastSacrificeLoss = reader.ReadDeltaTime();
					m_AvailableResurrects = reader.ReadInt();
					goto case 2;
				}
				case 2:
				{
					m_Flags = (PlayerFlag)reader.ReadInt();
					goto case 1;
				}
				case 1:
				{
					m_LongTermElapse = reader.ReadTimeSpan();
					m_ShortTermElapse = reader.ReadTimeSpan();
					m_GameTime = reader.ReadTimeSpan();
					goto case 0;
				}
				case 0:
				{
                    if (version < 43)
                        ShowTipsOnLogin = true;

                    if (version < 38)
                        m_AutoStabled = new List<Mobile>();
					break;
				}
			}

			// Professions weren't verified on 1.0 RC0
			if ( !CharacterCreation.VerifyProfession( Profession ) )
				Profession = 0;

			if ( m_PermaFlags == null )
				m_PermaFlags = new List<Mobile>();

            if ( m_Snoopers == null)
                m_Snoopers = new List<Mobile>();

			if ( m_JusticeProtectors == null )
				m_JusticeProtectors = new List<Mobile>();

			if ( m_BOBFilter == null )
				m_BOBFilter = new BOBFilter();

			if( m_GuildRank == null )
				m_GuildRank = RankDefinition.Member;	//Default to member if going from older verstion to new version (only time it should be null)

			if( LastOnline == DateTime.MinValue && Account != null )
				LastOnline = ((Account)Account).LastLogin;

			if( m_ChampionTitles == null )
				m_ChampionTitles = new ChampionTitleInfo();

            if (AccessLevel > AccessLevel.Player)
                m_IgnoreMobiles = true;

			List<Mobile> list = Stabled;

			for ( int i = 0; i < list.Count; ++i )
			{
				BaseCreature bc = list[i] as BaseCreature;

				if ( bc != null )
					bc.IsStabled = true;
			}

			CheckAtrophies( this );


			if( Hidden )	//Hiding is the only buff where it has an effect that's serialized.
				AddBuff( new BuffInfo( BuffIcon.HidingAndOrStealth, 1075655 ) );
		}
		
		public override void Serialize( GenericWriter writer )
		{
			//cleanup our anti-macro table 
			foreach ( Hashtable t in m_AntiMacroTable.Values )
			{
				ArrayList remove = new ArrayList();
				foreach ( CountAndTimeStamp time in t.Values )
				{
					if ( time.TimeStamp + SkillCheck.AntiMacroExpire <= DateTime.Now )
						remove.Add( time );
				}

				for (int i=0;i<remove.Count;++i)
					t.Remove( remove[i] );
			}

            CheckKillDecay();

			CheckAtrophies( this );

			base.Serialize( writer );

            writer.Write( 44 ); // version

            //ver 44
            writer.Write(AntiMacroGump);

            //ver 43
            writer.Write(ShowTipsOnLogin);

            //ver 42
            writer.Write(AllowMulti);

            //ver 41
            writer.Write((int)GameInfoGumpType);

            //ver 40
            writer.Write((byte)m_EventType);
            writer.Write(m_CurrentEvent);

            //ver 39
            writer.Write(TempCheck);
            //ver 38
            writer.Write(m_AutoStabled, true);
            writer.Write((DateTime)m_AnkhNextUse);
            writer.Write((DateTime)m_PeacedUntil);
            //ver 37
            writer.Write(ShowArriveMsg);
            //ver 36
            writer.Write(m_Rating);
            writer.Write(m_TournamentRating);
            //ver 35
            writer.Write(m_Snoopers, true);
            //ver 34
            writer.Write(m_HasStartingSkillBoost);
            //ver 33
            writer.Write(m_OriginalHairHue);
		    writer.Write(m_OriginalHairItemID);
		    writer.Write(m_OriginalHue);
		    writer.Write(m_HasCustomRace);
            //ver 32
            writer.Write( VirtualArmor = 0 );
            //ver 31
            writer.Write(m_PlayerGuildGameTime);
            //ver 30
            writer.Write(m_AlwaysMurderer);
            //ver 29
            writer.Write(m_Imported);

            //ver 28
            //writer.Write((TownMemberState)m_TownMemberState);

            //ver 27
            writer.Write(m_Stoned);
			//ver 26
			writer.Write( UseUnicodeSpeech );

			if( m_AcquiredRecipes == null )
			{
				writer.Write( 0 );
			}
			else
			{
				writer.Write( m_AcquiredRecipes.Count );

				foreach( KeyValuePair<int, bool> kvp in m_AcquiredRecipes )
				{
					writer.Write( kvp.Key );
					writer.Write( kvp.Value );
				}
			}

			writer.WriteDeltaTime( m_LastHonorLoss );

			ChampionTitleInfo.Serialize( writer, m_ChampionTitles );

			writer.Write( m_LastValorLoss );
			writer.WriteEncodedInt( m_ToTItemsTurnedIn );
			writer.Write( m_ToTTotalMonsterFame );	//This ain't going to be a small #.

			writer.WriteEncodedInt( AllianceMessageHue );
			writer.WriteEncodedInt( GuildMessageHue );

			writer.WriteEncodedInt( m_GuildRank.Rank );
			writer.Write( LastOnline );

			writer.WriteEncodedInt( (int) m_SolenFriendship );

			QuestSerializer.Serialize( m_Quest, writer );

			if ( m_DoneQuests == null )
			{
				writer.WriteEncodedInt( 0 );
			}
			else
			{
				writer.WriteEncodedInt( m_DoneQuests.Count );

				for ( int i = 0; i < m_DoneQuests.Count; ++i )
				{
					QuestRestartInfo restartInfo = m_DoneQuests[i];

					QuestSerializer.Write( restartInfo.QuestType, QuestSystem.QuestTypes, writer );
					writer.Write( restartInfo.RestartTime );
				}
			}

			writer.WriteEncodedInt( Profession );

			writer.WriteDeltaTime( m_LastCompassionLoss );

			writer.WriteEncodedInt( m_CompassionGains );

			if ( m_CompassionGains > 0 )
				writer.WriteDeltaTime( m_NextCompassionDay );

			m_BOBFilter.Serialize( writer );

			bool useMods = ( m_HairModID != -1 || m_BeardModID != -1 );

			writer.Write( useMods );

			if ( useMods )
			{
				writer.Write( m_HairModID );
				writer.Write( m_HairModHue );
				writer.Write( m_BeardModID );
				writer.Write( m_BeardModHue );
			}

			writer.Write( SavagePaintExpiration );

			writer.Write( (int) NpcGuild );
			writer.Write( NpcGuildJoinTime );
			writer.Write( NpcGuildGameTime );

			writer.Write( m_PermaFlags, true );

			writer.Write( NextTailorBulkOrder );

			writer.Write( NextSmithBulkOrder );

			writer.WriteDeltaTime( m_LastJusticeLoss );
			writer.Write( m_JusticeProtectors, true );

			writer.WriteDeltaTime( m_LastSacrificeGain );
			writer.WriteDeltaTime( m_LastSacrificeLoss );
			writer.Write( m_AvailableResurrects );

			writer.Write( (int) m_Flags );

			writer.Write( m_LongTermElapse );
			writer.Write( m_ShortTermElapse );
			writer.Write( GameTime );
		}

		public static void CheckAtrophies( Mobile m )
		{
			SacrificeVirtue.CheckAtrophy( m );
			JusticeVirtue.CheckAtrophy( m );
			CompassionVirtue.CheckAtrophy( m );
			ValorVirtue.CheckAtrophy( m );

			if( m is PlayerMobile )
				ChampionTitleInfo.CheckAtrophy( (PlayerMobile)m );
		}

        public void CheckKillDecay()
        {
            if (!AlwaysMurderer)
            {
                if (m_ShortTermElapse < this.GameTime)
                {
                    m_ShortTermElapse += TimeSpan.FromHours(24);
                    if (ShortTermMurders > 0)
                        --ShortTermMurders;
                }

                if (m_LongTermElapse < this.GameTime)
                {
                    m_LongTermElapse += TimeSpan.FromHours(24);
                    if (Kills > 0)
                        --Kills;
                }
            }
        }

		public void ResetKillTime()
		{
			m_ShortTermElapse = GameTime + TimeSpan.FromHours( 6 );
			m_LongTermElapse = GameTime + TimeSpan.FromHours( 6 );
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime SessionStart
		{
			get{ return m_SessionStart; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan GameTime
		{
			get
			{
			    if ( NetState != null )
					return m_GameTime + (DateTime.Now - m_SessionStart);
			    return m_GameTime;
			}
            set
            {
                if ( NetState != null)
                    m_GameTime = value;
            }
		}

		public override bool CanSee( Mobile m )
		{
            if (m is CharacterStatue)
                ((CharacterStatue)m).OnRequestedAnimation(this);

			if ( m is PlayerMobile && ((PlayerMobile)m).m_VisList.Contains( this ) )
				return true;

			return base.CanSee( m );
		}

		public override bool CanSee( Item item )
		{
			if ( m_DesignContext != null && m_DesignContext.Foundation.IsHiddenToCustomizer( item ) )
				return false;

			return base.CanSee( item );
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			Faction faction = Faction.Find( this );

			if ( faction != null )
				faction.RemoveMember( this );

			BaseHouse.HandleDeletion( this );

            DisguiseTimers.RemoveTimer(this);
		}

		public override bool NewGuildDisplay { get { return Guilds.Guild.NewGuildSystem; } }

        public override bool ShowContextMenu { get { return false; } }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( Map == Faction.Facet )
			{
				PlayerState pl = PlayerState.Find( this );

				if ( pl != null )
				{
					Faction faction = pl.Faction;

					if ( faction.Commander == this )
						list.Add( 1042733, faction.Definition.PropName ); // Commanding Lord of the ~1_FACTION_NAME~
					else if ( pl.Sheriff != null )
						list.Add( 1042734, "{0}\t{1}", pl.Sheriff.Definition.FriendlyName, faction.Definition.PropName ); // The Sheriff of  ~1_CITY~, ~2_FACTION_NAME~
					else if ( pl.Finance != null )
						list.Add( 1042735, "{0}\t{1}", pl.Finance.Definition.FriendlyName, faction.Definition.PropName ); // The Finance Minister of ~1_CITY~, ~2_FACTION_NAME~
					else if ( pl.MerchantTitle != MerchantTitle.None )
						list.Add( 1060776, "{0}\t{1}", MerchantTitles.GetInfo( pl.MerchantTitle ).Title, faction.Definition.PropName ); // ~1_val~, ~2_val~
					else
						list.Add( 1060776, "{0}\t{1}", pl.Rank.Title, faction.Definition.PropName ); // ~1_val~, ~2_val~
				}
			}

            if (Core.ML)
            {
                for (int i = AllFollowers.Count - 1; i >= 0; i--)
                {
                    BaseCreature c = AllFollowers[i] as BaseCreature;

                    if (c != null && c.ControlOrder == OrderType.Guard)
                    {
                        list.Add(501129); // guarded
                        break;
                    }
                }
            }
		}

		public override void OnSingleClick( Mobile from )
		{
            #region Faction
            if ( Map == Faction.Facet )
			{
				PlayerState pl = PlayerState.Find( this );

				if ( pl != null )
				{
					string text;
					bool ascii = false;

					Faction faction = pl.Faction;

					if ( faction.Commander == this )
						text = String.Concat( Female ? "(Commanding Lady of the " : "(Commanding Lord of the ", faction.Definition.FriendlyName, ")" );
					else if ( pl.Sheriff != null )
						text = String.Concat( "(The Sheriff of ", pl.Sheriff.Definition.FriendlyName, ", ", faction.Definition.FriendlyName, ")" );
					else if ( pl.Finance != null )
						text = String.Concat( "(The Finance Minister of ", pl.Finance.Definition.FriendlyName, ", ", faction.Definition.FriendlyName, ")" );
					else
					{
						ascii = true;

						if ( pl.MerchantTitle != MerchantTitle.None )
							text = String.Concat( "(", MerchantTitles.GetInfo( pl.MerchantTitle ).Title.String, ", ", faction.Definition.FriendlyName, ")" );
						else
							text = String.Concat( "(", pl.Rank.Title.String, ", ", faction.Definition.FriendlyName, ")" );
					}

					int hue = ( Faction.Find( from ) == faction ? 98 : 38 );

					PrivateOverheadMessage( MessageType.Label, hue, ascii, text, from.NetState );
				}
            }

            #endregion

            #region Mobile.cs OnSingleclick

            if (Deleted)
                return;

            string abbreviation ="";

            if (GuildClickMessage)
            {
                BaseGuild guild = Guild;

                if (guild != null && DisplayGuildTitle && GuildTitle != "")
                    if (guild.Abbreviation != null)
                        abbreviation = String.Format("[{0}]", Guild.Abbreviation);
                else
                    abbreviation = string.Empty;
            }

            int nameHue = 99;

            if (from == this)
            {
                if (NameHue != -1)
                    nameHue = NameHue;
                else if (AccessLevel > AccessLevel.Player)
                    nameHue = 11;
                else if (AlwaysMurderer)
                    nameHue = Notoriety.GetHue(Notoriety.Murderer);
                else if (from.Kills >= NotorietyHandlers.KILLS_FOR_MURDER)
                    nameHue = Notoriety.GetHue(Notoriety.Murderer);
                else if (from.Karma <= NotorietyHandlers.PLAYER_KARMA_RED)
                    nameHue = Notoriety.GetHue(Notoriety.Murderer);
                else if (from.Criminal)
                    nameHue = Notoriety.GetHue(Notoriety.Criminal);
                else if (from.Karma <= NotorietyHandlers.PLAYER_KARMA_GREY)
                    nameHue = Notoriety.GetHue(Notoriety.CanBeAttacked);
            }
            else
            {
                if (NameHue != -1)
                    nameHue = NameHue;
                else if (AccessLevel > AccessLevel.Player)
                    nameHue = 11;
                else
                    nameHue = Notoriety.GetHue(Notoriety.Compute(from, this));
            }

            string name = Name;

            if (name == null)
                name = String.Empty;

            string prefix = "";

            if ((Player || Body.IsHuman) && Fame >= 10000)
                prefix = (Female ? "Lady" : "Lord");

            string suffix = "";

            if (ClickTitle && !string.IsNullOrEmpty(Title))
                suffix = Title;

            suffix = ApplyNameSuffix(suffix);

            suffix = string.Format("{0}{1}", abbreviation, suffix);

            string playerName;

            if (prefix.Length > 0 && suffix.Length > 0)
                playerName = String.Concat(prefix, " ", name, " ", suffix);
            else if (prefix.Length > 0)
                playerName = String.Concat(prefix, " ", name);
            else if (suffix.Length > 0)
                playerName = String.Concat(name, " ", suffix);
            else
                playerName = name;

            //Taran: Add criminal to nametag when clicking self
            if (from == this && from.Criminal)
                playerName += " (criminal)";

            //Iza - Display GM Title // Maka --> 2 lines of code > a new namespace +class
            if (AccessLevel >= AccessLevel.Counselor && from.NetState != null)
                from.NetState.Send(new AsciiMessage(Serial, Body, MessageType.Label, 1157, 3, Name, AccessLevel == AccessLevel.GameMaster ? "Game Master" : AccessLevel.ToString()));

            PrivateOverheadMessage(MessageType.Label, nameHue, AsciiClickMessage, playerName, from.NetState);
		    #endregion         
		}

		protected override bool OnMove( Direction d )
		{
            if (m_PokerGame != null) //Start Edit For Poker
            {
                if (!HasGump(typeof(PokerLeaveGump)))
                {
                    SendGump(new PokerLeaveGump(this, m_PokerGame));
                    return false;
                }

            } //End Edit For Poker

            if (Hidden && AccessLevel == AccessLevel.Player)
            {
                CustomRegion cR = Region as CustomRegion;
                if (cR != null && cR.Controller.IsRestrictedSkill(47))
                {
                    SendAsciiMessage("Stealthing isn't allowed in this region");
                    RevealingAction();
                }
                else if (AllowedStealthSteps-- <= 0 || (d & Direction.Running) != 0)
                    RevealingAction();
            }

			return true;
		}

	    public bool BedrollLogout { get; set; }

	    [CommandProperty( AccessLevel.GameMaster )]
		public override bool Paralyzed
		{
			get
			{
				return base.Paralyzed;
			}
			set
			{
				base.Paralyzed = value;

				if( value )
					AddBuff( new BuffInfo( BuffIcon.Paralyze, 1075827 ) );	//Paralyze/You are frozen and can not move
				else
					RemoveBuff( BuffIcon.Paralyze );
			}
		}

		#region Ethics
		private Player m_EthicPlayer;

		[CommandProperty( AccessLevel.GameMaster )]
		public Player EthicPlayer
		{
			get { return m_EthicPlayer; }
			set { m_EthicPlayer = value; }
		}
		#endregion

		#region Factions

	    public PlayerState FactionPlayerState { get; set; }

	    #endregion

		#region Quests
		private QuestSystem m_Quest;
		private List<QuestRestartInfo> m_DoneQuests;
		private SolenFriendship m_SolenFriendship;

		public QuestSystem Quest
		{
			get{ return m_Quest; }
			set{ m_Quest = value; }
		}

		public List<QuestRestartInfo> DoneQuests
		{
			get{ return m_DoneQuests; }
			set{ m_DoneQuests = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SolenFriendship SolenFriendship
		{
			get{ return m_SolenFriendship; }
			set{ m_SolenFriendship = value; }
		}
		#endregion

		#region MyRunUO Invalidation
		private bool m_ChangedMyRunUO;

		public bool ChangedMyRunUO
		{
			get{ return m_ChangedMyRunUO; }
			set{ m_ChangedMyRunUO = value; }
		}

		public void InvalidateMyRunUO()
		{
			if ( !Deleted && !m_ChangedMyRunUO )
			{
				m_ChangedMyRunUO = true;
				MyRunUO.QueueMobileUpdate( this );
			}
		}

		public override void OnKillsChange( int oldValue )
		{
			if ( Young && Kills > oldValue )
			{
				Account acc = Account as Account;

				if ( acc != null )
					acc.RemoveYoungStatus( 0 );
			}

			InvalidateMyRunUO();
		}

		public override void OnGenderChanged( bool oldFemale )
		{
			InvalidateMyRunUO();
		}

		public override void OnGuildChange( BaseGuild oldGuild )
		{
			InvalidateMyRunUO();
		}

		public override void OnGuildTitleChange( string oldTitle )
		{
			InvalidateMyRunUO();
		}

		public override void OnKarmaChange( int oldValue )
		{
			InvalidateMyRunUO();
		}

		public override void OnFameChange( int oldValue )
		{
			InvalidateMyRunUO();
		}

		public override void OnSkillChange( SkillName skill, double oldBase )
		{
			if ( Young && SkillsTotal >= 10000 )
			{
				Account acc = Account as Account;

				if ( acc != null )
					acc.RemoveYoungStatus( 1019036 ); // You have successfully obtained a respectable skill level, and have outgrown your status as a young player!
			}

			InvalidateMyRunUO();
		}

		public override void OnAccessLevelChanged( AccessLevel oldLevel )
		{
            if (AccessLevel == AccessLevel.Player)
                IgnoreMobiles = false;
            else
                IgnoreMobiles = true;

			InvalidateMyRunUO();
		}

		public override void OnRawStatChange( StatType stat, int oldValue )
		{
			InvalidateMyRunUO();
		}

		public override void OnDelete()
		{
			if ( m_ReceivedHonorContext != null )
				m_ReceivedHonorContext.Cancel();
			if ( m_SentHonorContext != null )
				m_SentHonorContext.Cancel();

			InvalidateMyRunUO();
		}
		#endregion

		#region Fastwalk Prevention
		private static bool FastwalkPrevention = true; // Is fastwalk prevention enabled?
		private static readonly TimeSpan FastwalkThreshold = TimeSpan.FromSeconds( 0.4 ); // Fastwalk prevention will become active after 0.4 seconds

		private DateTime m_NextMovementTime;

		public virtual bool UsesFastwalkPrevention{ get{ return ( AccessLevel < AccessLevel.Counselor ); } }

		public override TimeSpan ComputeMovementSpeed( Direction dir, bool checkTurning )
		{
			if ( checkTurning && (dir & Direction.Mask) != (Direction & Direction.Mask) )
                return Mobile.RunMount;	// We are NOT actually moving (just a direction change)

            TransformContext context = TransformationSpellHelper.GetContext(this);

            if (context != null && context.Type == typeof(ReaperFormSpell))
                return WalkFoot;

			bool running = ( (dir & Direction.Running) != 0 );

			bool onHorse = ( Mount != null );

            AnimalFormContext animalContext = AnimalForm.GetContext(this);

            if (onHorse || (animalContext != null && animalContext.SpeedBoost))
                return (running ? RunMount : WalkMount);

			return ( running ? RunFoot : WalkFoot );
		}

		public static bool MovementThrottle_Callback( NetState ns )
		{
			PlayerMobile pm = ns.Mobile as PlayerMobile;

			if ( pm == null || !pm.UsesFastwalkPrevention )
				return true;

			if ( pm.m_NextMovementTime == DateTime.MinValue )
			{
				// has not yet moved
				pm.m_NextMovementTime = DateTime.Now;
				return true;
			}

			TimeSpan ts = pm.m_NextMovementTime - DateTime.Now;

			if ( ts < TimeSpan.Zero )
			{
				// been a while since we've last moved
				pm.m_NextMovementTime = DateTime.Now;
				return true;
			}

			return ( ts < FastwalkThreshold );
		}
		#endregion

		#region Enemy of One
		private Type m_EnemyOfOneType;

	    public Type EnemyOfOneType
		{
			get{ return m_EnemyOfOneType; }
			set
			{
				Type oldType = m_EnemyOfOneType;
				Type newType = value;

				if ( oldType == newType )
					return;

				m_EnemyOfOneType = value;

				DeltaEnemies( oldType, newType );
			}
		}

	    public bool WaitingForEnemy { get; set; }

	    private void DeltaEnemies( Type oldType, Type newType )
		{
			foreach ( Mobile m in GetMobilesInRange( 18 ) )
			{
				Type t = m.GetType();

                if (t == oldType || t == newType)
                {
                    NetState ns = this.NetState;

                    if (ns != null)
                    {
						if ( ns.StygianAbyss ) {
                            ns.Send(new MobileMoving(m, Notoriety.Compute(this, m)));
                        } else {
                            ns.Send(new MobileMovingOld(m, Notoriety.Compute(this, m)));
                        }
                    }
                }
			}
		}
		#endregion

		#region Hair and beard mods
		private int m_HairModID = -1, m_HairModHue;
		private int m_BeardModID = -1, m_BeardModHue;

		public void SetHairMods( int hairID, int beardID )
		{
			if ( hairID == -1 )
				InternalRestoreHair( true, ref m_HairModID, ref m_HairModHue );
			else if ( hairID != -2 )
				InternalChangeHair( true, hairID, ref m_HairModID, ref m_HairModHue );

			if ( beardID == -1 )
				InternalRestoreHair( false, ref m_BeardModID, ref m_BeardModHue );
			else if ( beardID != -2 )
				InternalChangeHair( false, beardID, ref m_BeardModID, ref m_BeardModHue );
		}

		private void CreateHair( bool hair, int id, int hue )
		{
			if( hair )
			{
				//TODO Verification?
				HairItemID = id;
				HairHue = hue;
			}
			else
			{
				FacialHairItemID = id;
				FacialHairHue = hue;
			}
		}

		private void InternalRestoreHair( bool hair, ref int id, ref int hue )
		{
			if ( id == -1 )
				return;

			if ( hair )
				HairItemID = 0;
			else
				FacialHairItemID = 0;

			//if( id != 0 )
			CreateHair( hair, id, hue );

			id = -1;
			hue = 0;
		}

		private void InternalChangeHair( bool hair, int id, ref int storeID, ref int storeHue )
		{
			if ( storeID == -1 )
			{
				storeID = hair ? HairItemID : FacialHairItemID;
				storeHue = hair ? HairHue : FacialHairHue;
			}
			CreateHair( hair, id, 0 );
		}
		#endregion

		#region Virtues
		private DateTime m_LastSacrificeGain;
		private DateTime m_LastSacrificeLoss;
		private int m_AvailableResurrects;

		public DateTime LastSacrificeGain{ get{ return m_LastSacrificeGain; } set{ m_LastSacrificeGain = value; } }
		public DateTime LastSacrificeLoss{ get{ return m_LastSacrificeLoss; } set{ m_LastSacrificeLoss = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AvailableResurrects{ get{ return m_AvailableResurrects; } set{ m_AvailableResurrects = value; } }

		//private DateTime m_NextJustAward;
		private DateTime m_LastJusticeLoss;
		private List<Mobile> m_JusticeProtectors;

		public DateTime LastJusticeLoss{ get{ return m_LastJusticeLoss; } set{ m_LastJusticeLoss = value; } }
		public List<Mobile> JusticeProtectors { get { return m_JusticeProtectors; } set { m_JusticeProtectors = value; } }

		private DateTime m_LastCompassionLoss;
		private DateTime m_NextCompassionDay;
		private int m_CompassionGains;

		public DateTime LastCompassionLoss{ get{ return m_LastCompassionLoss; } set{ m_LastCompassionLoss = value; } }
		public DateTime NextCompassionDay{ get{ return m_NextCompassionDay; } set{ m_NextCompassionDay = value; } }
		public int CompassionGains{ get{ return m_CompassionGains; } set{ m_CompassionGains = value; } }

		private DateTime m_LastValorLoss;

		public DateTime LastValorLoss { get { return m_LastValorLoss; } set { m_LastValorLoss = value; } }

		private DateTime m_LastHonorLoss;
	    private HonorContext m_ReceivedHonorContext;
		private HonorContext m_SentHonorContext;
        public DateTime m_hontime;

		public DateTime LastHonorLoss{ get{ return m_LastHonorLoss; } set{ m_LastHonorLoss = value; } }
	    public DateTime LastHonorUse { get; set; }

	    public bool HonorActive { get; set; }

	    public HonorContext ReceivedHonorContext{ get{ return m_ReceivedHonorContext; } set{ m_ReceivedHonorContext = value; } }
		public HonorContext SentHonorContext{ get{ return m_SentHonorContext; } set{ m_SentHonorContext = value; } }
		#endregion

		#region Young system
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Young
		{
			get{ return GetFlag( PlayerFlag.Young ); }
			set{ SetFlag( PlayerFlag.Young, value ); InvalidateProperties(); }
		}

		public override string ApplyNameSuffix( string suffix )
		{
			if ( Young )
			{
				if ( suffix.Length == 0 )
					suffix = "[Young]";
				else
					suffix = String.Concat( suffix, " [Young]" );
			}

			#region Ethics
			if ( m_EthicPlayer != null )
			{
				if ( suffix.Length == 0 )
					suffix = m_EthicPlayer.Ethic.Definition.Adjunct.String;
				else
					suffix = String.Concat( suffix, " ", m_EthicPlayer.Ethic.Definition.Adjunct.String );
			}
			#endregion

            if (Core.ML && Map == Faction.Facet)
            {
                Faction faction = Faction.Find(this);

                if (faction != null)
                {
                    string adjunct = String.Format("[{0}]", faction.Definition.Abbreviation);
                    if (suffix.Length == 0)
                        suffix = adjunct;
                    else
                        suffix = String.Concat(suffix, " ", adjunct);
                }
            }

			return base.ApplyNameSuffix( suffix );
		}

        public override bool ClickTitle
        {
            get { return false; }
        }


		public override TimeSpan GetLogoutDelay()
		{
            if ((Region is GuardedRegion) || Young || BedrollLogout || TestCenter.Enabled)
            {
                if (Region is CustomRegion)
                    return base.GetLogoutDelay();
                if (LastAttackTime + TimeSpan.FromMinutes(1.0) >= DateTime.Now)
                    return TimeSpan.FromMinutes(2.0);
                return TimeSpan.Zero;
            }

		    return base.GetLogoutDelay();
		}

		private DateTime m_LastYoungMessage = DateTime.MinValue;

		public bool CheckYoungProtection( Mobile from )
		{
			if ( !Young )
				return false;

            if (IsInEvent)
                return false;

		    CustomRegion cR = from.Region as CustomRegion;
            if (cR != null && cR.Controller.IgnoreYoungProtection)
                return false;

			if ( Region.IsPartOf( typeof( DungeonRegion )) )
				return false;

			if( from is BaseCreature && ((BaseCreature)from).IgnoreYoungProtection )
				return false;

			if ( Quest != null && Quest.IgnoreYoungProtection( from ) )
				return false;

			if ( DateTime.Now - m_LastYoungMessage > TimeSpan.FromMinutes( 1.0 ) )
			{
				m_LastYoungMessage = DateTime.Now;
				SendLocalizedMessage( 1019067 ); // A monster looks at you menacingly but does not attack.  You would be under attack now if not for your status as a new citizen of Britannia.
			}

			return true;
		}

		private DateTime m_LastYoungHeal = DateTime.MinValue;

		public bool CheckYoungHealTime()
		{
			if ( DateTime.Now - m_LastYoungHeal > TimeSpan.FromMinutes( 5.0 ) )
			{
				m_LastYoungHeal = DateTime.Now;
				return true;
			}

			return false;
		}

        private static readonly Point3D[] m_FeluccaDeathDestinations = new Point3D[]
			{
				new Point3D( 1481, 1612, 20 ),
				new Point3D( 2708, 2153,  0 ),
				new Point3D( 2249, 1230,  0 ),
				new Point3D( 5197, 3994, 37 ),
				new Point3D( 1412, 3793,  0 ),
				new Point3D( 3688, 2232, 20 ),
				new Point3D( 2578,  604,  0 ),
				new Point3D( 4397, 1089,  0 ),
				new Point3D( 5741, 3218, -2 ),
				new Point3D( 2996, 3441, 15 ),
				new Point3D(  624, 2225,  0 ),
				new Point3D( 1916, 2814,  0 ),
				new Point3D( 2929,  854,  0 ),
				new Point3D(  545,  967,  0 ),
				new Point3D( 3665, 2587,  0 ),
                new Point3D( 6247, 111,  0 ) //Mercenary Camp
			};
        /*
		private static readonly Point3D[] m_TrammelDeathDestinations = new Point3D[]
			{
				new Point3D( 1481, 1612, 20 ),
				new Point3D( 2708, 2153,  0 ),
				new Point3D( 2249, 1230,  0 ),
				new Point3D( 5197, 3994, 37 ),
				new Point3D( 1412, 3793,  0 ),
				new Point3D( 3688, 2232, 20 ),
				new Point3D( 2578,  604,  0 ),
				new Point3D( 4397, 1089,  0 ),
				new Point3D( 5741, 3218, -2 ),
				new Point3D( 2996, 3441, 15 ),
				new Point3D(  624, 2225,  0 ),
				new Point3D( 1916, 2814,  0 ),
				new Point3D( 2929,  854,  0 ),
				new Point3D(  545,  967,  0 ),
				new Point3D( 3665, 2587,  0 )
			};

		private static readonly Point3D[] m_IlshenarDeathDestinations = new Point3D[]
			{
				new Point3D( 1216,  468, -13 ),
				new Point3D(  723, 1367, -60 ),
				new Point3D(  745,  725, -28 ),
				new Point3D(  281, 1017,   0 ),
				new Point3D(  986, 1011, -32 ),
				new Point3D( 1175, 1287, -30 ),
				new Point3D( 1533, 1341,  -3 ),
				new Point3D(  529,  217, -44 ),
				new Point3D( 1722,  219,  96 )
			};

		private static readonly Point3D[] m_MalasDeathDestinations = new Point3D[]
			{
				new Point3D( 2079, 1376, -70 ),
				new Point3D(  944,  519, -71 )
			};

		private static readonly Point3D[] m_TokunoDeathDestinations = new Point3D[]
			{
				new Point3D( 1166,  801, 27 ),
				new Point3D(  782, 1228, 25 ),
				new Point3D(  268,  624, 15 )
			};
        */
		public bool YoungDeathTeleport()
		{
			if ( Region.IsPartOf( typeof( Jail ) ) || Region.IsPartOf( "Samurai start location" ) || Region.IsPartOf( "Ninja start location" ) || Region.IsPartOf( "Ninja cave" ) || IsInEvent )
				return false;

			Point3D loc;
			Map map;

			DungeonRegion dungeon = (DungeonRegion) Region.GetRegion( typeof( DungeonRegion ) );
			if ( dungeon != null && dungeon.EntranceLocation != Point3D.Zero )
			{
				loc = dungeon.EntranceLocation;
				map = dungeon.EntranceMap;
			}
			else
			{
				loc = Location;
				map = Map;
			}

			Point3D[] list;

            if (map == Map.Felucca)
                list = m_FeluccaDeathDestinations;
			else if ( map == Map.Trammel )
                list = m_FeluccaDeathDestinations;
			else if ( map == Map.Ilshenar )
                list = m_FeluccaDeathDestinations;
			else if ( map == Map.Malas )
                list = m_FeluccaDeathDestinations;
			else if ( map == Map.Tokuno )
                list = m_FeluccaDeathDestinations;
			else
				return false;

			Point3D dest = Point3D.Zero;
			int sqDistance = int.MaxValue;

			for ( int i = 0; i < list.Length; i++ )
			{
				Point3D curDest = list[i];

				int width = loc.X - curDest.X;
				int height = loc.Y - curDest.Y;
				int curSqDistance = width * width + height * height;

				if ( curSqDistance < sqDistance )
				{
					dest = curDest;
					sqDistance = curSqDistance;
				}
			}

			MoveToWorld( dest, Map.Felucca );
			return true;
		}

		private void SendYoungDeathNotice()
		{
            if (!IsInEvent)
			    SendGump( new YoungDeathNotice() );
		}

        private void SendYoungTeleportGump()
        {
            if (!IsInEvent)
                SendGump(new YoungDeathTeleport());
        }
		#endregion

		#region Speech log
		private SpeechLog m_SpeechLog;

		public SpeechLog SpeechLog{ get{ return m_SpeechLog; } }

		public override void OnSpeech( SpeechEventArgs e )
		{
			if ( SpeechLog.Enabled && NetState != null )
			{
				if ( m_SpeechLog == null )
					m_SpeechLog = new SpeechLog();

				m_SpeechLog.Add( e.Mobile, e.Speech );
			}
		}
		#endregion

		#region Champion Titles
		[CommandProperty( AccessLevel.GameMaster )]
		public bool DisplayChampionTitle
		{
			get { return GetFlag( PlayerFlag.DisplayChampionTitle ); }
			set { SetFlag( PlayerFlag.DisplayChampionTitle, value ); }
		}

		private ChampionTitleInfo m_ChampionTitles;

		[CommandProperty( AccessLevel.GameMaster )]
		public ChampionTitleInfo ChampionTitles { get { return m_ChampionTitles; }  }

        
		private void ToggleChampionTitleDisplay()
		{
			if( !CheckAlive() )
				return;

			if( DisplayChampionTitle )
				SendLocalizedMessage( 1062419, "", 0x23 ); // You have chosen to hide your monster kill title.
			else
				SendLocalizedMessage( 1062418, "", 0x23 ); // You have chosen to display your monster kill title.

			DisplayChampionTitle = !DisplayChampionTitle;
		}
        

		[PropertyObject]
		public class ChampionTitleInfo
		{
			public static TimeSpan LossDelay = TimeSpan.FromDays( 1.0 );
			public const int LossAmount = 90;

			private class TitleInfo
			{
				private int m_Value;
				private DateTime m_LastDecay;

				public int Value { get { return m_Value; } set { m_Value = value; } }
				public DateTime LastDecay { get { return m_LastDecay; } set { m_LastDecay = value; } }

				public TitleInfo()
				{
				}

				public TitleInfo( GenericReader reader )
				{
					int version = reader.ReadEncodedInt();

					switch( version )
					{
						case 0:
						{
							m_Value = reader.ReadEncodedInt();
							m_LastDecay = reader.ReadDateTime();
							break;
						}
					}
				}

				public static void Serialize( GenericWriter writer, TitleInfo info )
				{
					writer.WriteEncodedInt( 0 ); // version

					writer.WriteEncodedInt( info.m_Value );
					writer.Write( info.m_LastDecay );
				}

			}
			private TitleInfo[] m_Values;

			private int m_Harrower;	//Harrower titles do NOT decay


			public int GetValue( ChampionSpawnType type )
			{
				return GetValue( (int)type );
			}

			public void SetValue( ChampionSpawnType type, int value )
			{
				SetValue( (int)type, value );
			}

			public void Award( ChampionSpawnType type, int value )
			{
				Award( (int)type, value );
			}

			public int GetValue( int index )
			{
				if( m_Values == null || index < 0 || index >= m_Values.Length )
					return 0;

				if( m_Values[index] == null )
					m_Values[index] = new TitleInfo();

				return m_Values[index].Value;
			}

			public DateTime GetLastDecay( int index )
			{
				if( m_Values == null || index < 0 || index >= m_Values.Length )
					return DateTime.MinValue;

				if( m_Values[index] == null )
					m_Values[index] = new TitleInfo();

				return m_Values[index].LastDecay;
			}

			public void SetValue( int index, int value )
			{
				if( m_Values == null )
					m_Values = new TitleInfo[ChampionSpawnInfo.Table.Length];

				if( value < 0 )
					value = 0;

				if( index < 0 || index >= m_Values.Length )
					return;

				if( m_Values[index] == null )
					m_Values[index] = new TitleInfo();

				m_Values[index].Value = value;
			}

			public void Award( int index, int value )
			{
				if( m_Values == null )
					m_Values = new TitleInfo[ChampionSpawnInfo.Table.Length];

				if( index < 0 || index >= m_Values.Length || value <= 0 )
					return;

				if( m_Values[index] == null )
					m_Values[index] = new TitleInfo();

				m_Values[index].Value += value;
			}

			public void Atrophy( int index, int value )
			{
				if( m_Values == null )
					m_Values = new TitleInfo[ChampionSpawnInfo.Table.Length];

				if( index < 0 || index >= m_Values.Length || value <= 0 )
					return;

				if( m_Values[index] == null )
					m_Values[index] = new TitleInfo();

				int before = m_Values[index].Value;

				if( (m_Values[index].Value - value) < 0 )
					m_Values[index].Value = 0;
				else
					m_Values[index].Value -= value;

				if( before != m_Values[index].Value )
					m_Values[index].LastDecay = DateTime.Now;
			}

			public override string ToString()
			{
				return "...";
			}

            [CommandProperty(AccessLevel.GameMaster)]
            public int Pestilence { get { return GetValue(ChampionSpawnType.Pestilence); } set { SetValue(ChampionSpawnType.Pestilence, value); } }

			[CommandProperty( AccessLevel.GameMaster )]
			public int Abyss { get { return GetValue( ChampionSpawnType.Abyss ); } set { SetValue( ChampionSpawnType.Abyss, value ); } }

			[CommandProperty( AccessLevel.GameMaster )]
			public int Arachnid { get { return GetValue( ChampionSpawnType.Arachnid ); } set { SetValue( ChampionSpawnType.Arachnid, value ); } }

			[CommandProperty( AccessLevel.GameMaster )]
			public int ColdBlood { get { return GetValue( ChampionSpawnType.ColdBlood ); } set { SetValue( ChampionSpawnType.ColdBlood, value ); } }

			[CommandProperty( AccessLevel.GameMaster )]
			public int ForestLord { get { return GetValue( ChampionSpawnType.ForestLord ); } set { SetValue( ChampionSpawnType.ForestLord, value ); } }

			[CommandProperty( AccessLevel.GameMaster )]
			public int SleepingDragon { get { return GetValue( ChampionSpawnType.SleepingDragon ); } set { SetValue( ChampionSpawnType.SleepingDragon, value ); } }

			[CommandProperty( AccessLevel.GameMaster )]
			public int UnholyTerror { get { return GetValue( ChampionSpawnType.UnholyTerror ); } set { SetValue( ChampionSpawnType.UnholyTerror, value ); } }

			[CommandProperty( AccessLevel.GameMaster )]
			public int VerminHorde { get { return GetValue( ChampionSpawnType.VerminHorde ); } set { SetValue( ChampionSpawnType.VerminHorde, value ); } }
			
			[CommandProperty( AccessLevel.GameMaster )]
			public int Harrower { get { return m_Harrower; } set { m_Harrower = value; } }

			public ChampionTitleInfo()
			{
			}

			public ChampionTitleInfo( GenericReader reader )
			{
				int version = reader.ReadEncodedInt();

				switch( version )
				{
					case 0:
					{
						m_Harrower = reader.ReadEncodedInt();

						int length = reader.ReadEncodedInt();
						m_Values = new TitleInfo[length];

						for( int i = 0; i < length; i++ )
						{
							m_Values[i] = new TitleInfo( reader );
						}

						if( m_Values.Length != ChampionSpawnInfo.Table.Length )
						{
							TitleInfo[] oldValues = m_Values;
							m_Values = new TitleInfo[ChampionSpawnInfo.Table.Length];

							for( int i = 0; i < m_Values.Length && i < oldValues.Length; i++ )
							{
								m_Values[i] = oldValues[i];
							}
						}
						break;
					}
				}
			}

			public static void Serialize( GenericWriter writer, ChampionTitleInfo titles )
			{
				writer.WriteEncodedInt( 0 ); // version

				writer.WriteEncodedInt( titles.m_Harrower );

				int length = titles.m_Values.Length;
				writer.WriteEncodedInt( length );

				for( int i = 0; i < length; i++ )
				{
					if( titles.m_Values[i] == null )
						titles.m_Values[i] = new TitleInfo();

					TitleInfo.Serialize( writer, titles.m_Values[i] );
				}
			}

			public static void CheckAtrophy( PlayerMobile pm )
			{
				ChampionTitleInfo t = pm.m_ChampionTitles;
				if( t == null )
					return;

				if( t.m_Values == null )
					t.m_Values = new TitleInfo[ChampionSpawnInfo.Table.Length];

				for( int i = 0; i < t.m_Values.Length; i++ )
				{
					if( (t.GetLastDecay( i ) + LossDelay) < DateTime.Now )
					{
						t.Atrophy( i, LossAmount );
					}
				}
			}

			public static void AwardHarrowerTitle( PlayerMobile pm )	//Called when killing a harrower.  Will give a minimum of 1 point.
			{
				ChampionTitleInfo t = pm.m_ChampionTitles;
				if( t == null )
					return;

				if( t.m_Values == null )
					t.m_Values = new TitleInfo[ChampionSpawnInfo.Table.Length];

				int count = 1;

				for( int i = 0; i < t.m_Values.Length; i++ )
				{
					if( t.m_Values[i].Value > 900 )
						count++;
				}

				t.m_Harrower = Math.Max( count, t.m_Harrower );	//Harrower titles never decay.
			}
		}
		#endregion

		#region Recipes

		private Dictionary<int, bool> m_AcquiredRecipes;
		
		public virtual bool HasRecipe( Recipe r )
		{
			if( r == null ) 
				return false;

			return HasRecipe( r.ID );
		}

		public virtual bool HasRecipe( int recipeID )
		{
			if( m_AcquiredRecipes != null && m_AcquiredRecipes.ContainsKey( recipeID ) )
				return m_AcquiredRecipes[recipeID];

			return false;
		}

		public virtual void AcquireRecipe( Recipe r )
		{
			if( r != null )
				AcquireRecipe( r.ID );
		}

		public virtual void AcquireRecipe( int recipeID )
		{
			if( m_AcquiredRecipes == null )
				m_AcquiredRecipes = new Dictionary<int, bool>();

			m_AcquiredRecipes[recipeID] = true;
		}

		public virtual void ResetRecipes()
		{
			m_AcquiredRecipes = null;
		}
	
		[CommandProperty( AccessLevel.GameMaster )]
		public int KnownRecipes
		{
			get 
			{
				if( m_AcquiredRecipes == null )
					return 0;

				return m_AcquiredRecipes.Count;
			}
		}
	

		#endregion

		#region Buff Icons

		public void ResendBuffs()
		{
			if( !BuffInfo.Enabled || m_BuffTable == null )
				return;

			NetState state = NetState;

            if (state != null && state.BuffIcon)
            {
				foreach( BuffInfo info in m_BuffTable.Values )
				{
					state.Send( new AddBuffPacket( this, info ) );
				}
			}
		}

		private Dictionary<BuffIcon, BuffInfo> m_BuffTable;

		public void AddBuff( BuffInfo b )
		{
			if( !BuffInfo.Enabled || b == null )
				return;

			RemoveBuff( b );	//Check & subsequently remove the old one.

			if( m_BuffTable == null )
				m_BuffTable = new Dictionary<BuffIcon, BuffInfo>();

			m_BuffTable.Add( b.ID, b );

			NetState state = NetState;

            if (state != null && state.BuffIcon)
            {
				state.Send( new AddBuffPacket( this, b ) );
			}
		}

		public void RemoveBuff( BuffInfo b )
		{
			if( b == null )
				return;

			RemoveBuff( b.ID );
		}

		public void RemoveBuff( BuffIcon b )
		{
			if( m_BuffTable == null || !m_BuffTable.ContainsKey( b ) )
				return;

			BuffInfo info = m_BuffTable[b];

			if( info.Timer != null && info.Timer.Running )
				info.Timer.Stop();

			m_BuffTable.Remove( b );

			NetState state = NetState;

            if (state != null && state.BuffIcon)
            {
				state.Send( new RemoveBuffPacket( this, b ) );
			}

			if( m_BuffTable.Count <= 0 )
				m_BuffTable = null;
		}
		#endregion

        public void AutoStablePets()
        {
            if (Core.SE && AllFollowers.Count > 0)
            {
                for (int i = m_AllFollowers.Count - 1; i >= 0; --i)
                {
                    BaseCreature pet = AllFollowers[i] as BaseCreature;

                    if (pet == null || pet.ControlMaster == null)
                        continue;

                    if (pet.Summoned)
                    {
                        if (pet.Map != Map)
                        {
                            pet.PlaySound(pet.GetAngerSound());
                            Timer.DelayCall(TimeSpan.Zero, new TimerCallback(pet.Delete));
                        }
                        continue;
                    }

                    if (pet is IMount && ((IMount)pet).Rider != null)
                        continue;

                    if ((pet is PackLlama || pet is PackHorse || pet is Beetle || pet is HordeMinionFamiliar) && (pet.Backpack != null && pet.Backpack.Items.Count > 0))
                        continue;

                    if (pet is BaseEscortable)
                        continue;

                    pet.ControlTarget = null;
                    pet.ControlOrder = OrderType.Stay;
                    pet.Internalize();

                    pet.SetControlMaster(null);
                    pet.SummonMaster = null;

                    pet.IsStabled = true;

                    pet.Loyalty = BaseCreature.MaxLoyalty; // Wonderfully happy

                    Stabled.Add(pet);
                    m_AutoStabled.Add(pet);
                }
            }
        }

        public void ClaimAutoStabledPets()
        {
            if (!Core.SE || m_AutoStabled.Count <= 0)
                return;

            if (!Alive)
            {
                SendLocalizedMessage(1076251); // Your pet was unable to join you while you are a ghost.  Please re-login once you have ressurected to claim your pets.				
                return;
            }

            for (int i = m_AutoStabled.Count - 1; i >= 0; --i)
            {
                BaseCreature pet = m_AutoStabled[i] as BaseCreature;

                if (pet == null || pet.Deleted)
                {
                    pet.IsStabled = false;

                    if (Stabled.Contains(pet))
                        Stabled.Remove(pet);

                    continue;
                }

                if ((Followers + pet.ControlSlots) <= FollowersMax)
                {
                    pet.SetControlMaster(this);

                    if (pet.Summoned)
                        pet.SummonMaster = this;

                    pet.ControlTarget = this;
                    pet.ControlOrder = OrderType.Follow;

                    pet.MoveToWorld(Location, Map);

                    pet.IsStabled = false;

                    pet.Loyalty = BaseCreature.MaxLoyalty; // Wonderfully Happy

                    if (Stabled.Contains(pet))
                        Stabled.Remove(pet);
                }
                else
                {
                    SendLocalizedMessage(1049612, pet.Name); // ~1_NAME~ remained in the stables because you have too many followers.
                }
            }

            m_AutoStabled.Clear();
        }

        public override void Kill()
        {
            //Stop the protection timer, if we have one.
            Spells.Second.ProtectionSpell.StopTimer(this);
            //Stop the reflect timer, if we have one.
            MagicReflectSpell.StopTimer(this);

            EndAction(typeof(BasePotion));

            #region base.Kill() (used for the auto cut "lastcorpse" fix)
            if (!CanBeDamaged())
                return;
            if (!Alive || IsDeadBondedPet)
                return;
            if (Deleted)
                return;
            if (!Region.OnBeforeDeath(this))
                return;
            if (!OnBeforeDeath())
                return;

            BankBox box = FindBankNoCreate();

            if (box != null && box.Opened)
                box.Close();

            if (NetState != null)
                NetState.CancelAllTrades();

            if (Spell != null)
                Spell.OnCasterKilled();

            if (Target != null)
                Target.Cancel(this, TargetCancelType.Canceled);

            DisruptiveAction();

            Warmode = false;

            DropHolding();

            Hits = 0;
            Stam = 0;
            Mana = 0;

            Poison = null;
            Combatant = null;

            if (Paralyzed)
                Paralyzed = false;

            if (Frozen)
                Frozen = false;

            List<Item> content = new List<Item>();
            List<Item> equip = new List<Item>();
            List<Item> moveToPack = new List<Item>();

            List<Item> itemsCopy = new List<Item>(Items);

            Container pack = Backpack;

            for (int i = 0; i < itemsCopy.Count; ++i)
            {
                Item item = itemsCopy[i];

                if (item == pack)
                    continue;

                DeathMoveResult res = GetParentMoveResultFor(item);

                switch (res)
                {
                    case DeathMoveResult.MoveToCorpse:
                        {
                            content.Add(item);
                            equip.Add(item);
                            break;
                        }
                    case DeathMoveResult.MoveToBackpack:
                        {
                            moveToPack.Add(item);
                            break;
                        }
                }
            }

            if (pack != null)
            {
                List<Item> packCopy = new List<Item>(pack.Items);

                for (int i = 0; i < packCopy.Count; ++i)
                {
                    Item item = packCopy[i];

                    DeathMoveResult res = GetInventoryMoveResultFor(item);

                    if (res == DeathMoveResult.MoveToCorpse)
                        content.Add(item);
                    else
                        moveToPack.Add(item);
                }

                for (int i = 0; i < moveToPack.Count; ++i)
                {
                    Item item = moveToPack[i];

                    if (RetainPackLocsOnDeath && item.Parent == pack)
                        continue;

                    pack.DropItem(item);
                }
            }

            HairInfo hair = null;
            if (HairItemID != 0)
                hair = new HairInfo(HairItemID, HairHue);

            FacialHairInfo facialhair = null;
            if (FacialHairItemID != 0)
                facialhair = new FacialHairInfo(FacialHairItemID, FacialHairHue);



            #region auto cut "serial" fix
            int maxCorpses = Utility.Random(1, 4);

            maxCorpses += Utility.Random(2);

            for (int i = 0; i < maxCorpses; i++)
            {
                Point3D loc = Location;
                loc.X += Utility.RandomMinMax(-1, 1);
                loc.Y += Utility.RandomMinMax(-1, 1);

                FakeCorpse fc = new FakeCorpse(this);
                fc.MoveToWorld(loc, Map.Felucca);

                fc.Stackable = true;
                fc.Amount = 590;
                fc.Amount = Body.BodyID-2; // protocol defines that for itemid 0x2006, amount=body
                fc.Stackable = false;
            }
            #endregion

            Container c = (CreateCorpseHandler == null ? null : CreateCorpseHandler(this, hair, facialhair, content, equip));

            if (Map != null)
            {
                Packet animPacket = null;
                Packet remPacket = null;

                IPooledEnumerable eable = Map.GetClientsInRange(Location);

                foreach (NetState state in eable)
                {
                    if (state != NetState && (!state.Mobile.HasFilter || InLOS(state.Mobile)))
                    {
                        //Fix be here
                        if (animPacket == null)
                            animPacket = Packet.Acquire(new DeathAnimation(this, null));

                        state.Send(animPacket);

                        if (!state.Mobile.CanSee(this))
                        {
                            if (remPacket == null)
                                remPacket = RemovePacket;

                            state.Send(remPacket);
                        }
                    }
                }

                Packet.Release(animPacket);

                eable.Free();
            }

            Region.OnDeath(this);
            OnDeath(c);
            #endregion
        }

        #region Maka

        public void WeaponTimerCheck()
        {
            //Auto-attack back after being interrupted is delayed
            if (Weapon != null && Weapon is BaseWeapon)
            {
                if (CurrentSwingTimer != null)
                {
                    CurrentSwingTimer.Stop();
                    CurrentSwingTimer = null;
                }

                BaseWeapon bw = Weapon as BaseWeapon;
                NextCombatTime = DateTime.Now + TimeSpan.FromSeconds(bw.GetDelay(this).TotalSeconds*3);
            }
        }

        public void BandageCheck()
        {
            if (BandageContext.GetContext(this) != null)
                BandageContext.GetContext(this).Slip();
        }

        public void SpellCheck()
        {
            if ( Spell != null)
                ((Spell) Spell).DoFizzle();
        }

        public static bool OnAttackRequest(NetState state)
        {
            PlayerMobile pm = state.Mobile as PlayerMobile;

            if (pm == null)
                return true;

            if (pm.Spell != null && pm.Spell.IsCasting)
                state.Mobile.Spell.OnCasterKilled();

            //Abort action
            pm.AbortCurrentPlayerAction();

            //The bandage of death
            pm.BandageCheck();

            if (pm.Warmode && (pm.NextCombatTime > (DateTime.Now + ((BaseWeapon) pm.Weapon).GetDelay(pm)) || (DateTime.Now - ((BaseWeapon) pm.Weapon).GetDelay(pm)) > pm.NextCombatTime))
                pm.NextCombatTime = DateTime.Now + ((BaseWeapon) pm.Weapon).GetDelay(pm);

            return true;
        }

        public override int GetHurtSound()
        {
            if (Body.IsMale)
                return Utility.RandomList(340, 341, 342, 343, 344, 345);
            if (Body.IsFemale)
                return Utility.RandomList(331, 332, 333, 334, 335);

            int sound = GetSoundId();

            if (sound > 0)
                return sound + 3;

            return base.GetHurtSound();
        }

        public override int GetAttackSound()
        {
            int sound = GetSoundId();

            if (sound > 0)
                return sound + 2;
            
            return base.GetAttackSound();
        }

        public int GetSoundId()
        {
            if (Body.IsMale || Body.IsFemale)
                return 0;

            int sound = 0;
            switch (BodyValue)
            {
                case 0xD0: //Chicken
                    sound = 110;
                    break;
                case 0xD9: //Dog
                    sound = 133;
                    break;
                case 0xE1: //Wolf
                    sound = 229;
                    break;
                case 0xD6: //Panther
                    sound = 1122;
                    break;
                case 0x1D: //Gorilla
                    sound = 158;
                    break;
                case 0xD3: //Black bear
                case 0xD4: //Grizzly bear
                case 0xD5: //Polar bear
                    sound = 163;
                    break;
                case 0x33: //Slime
                    sound = 456;
                    break;
                case 0x11: //Orc
                    sound = 1114;
                    break;
                case 0x24: //Lizard man
                    sound = 417;
                    break;
                case 0x04: //Gargoyle
                    sound = 372;
                    break;
                case 0x01: //Ogre
                    sound = 427;
                    break;
                case 0x36: //Troll
                    sound = 461;
                    break;
                case 0x02: //Ettin
                    sound = 367;
                    break;
                case 0x15: //Giant serpent
                    sound = 219;
                    break;
                case 0x09: //Daemon
                    sound = 357;
                    break;
                case 0x3B: //Dragon
                case 0xC: //Dragon
                    sound = 362;
                    break;
                case 0xCC: //Horse
                    sound = 169;
                    break;
            }
            return sound;
        }

        //More hurt anims?
        public int GetHurtAnim()
        {
            //Damage anims:
            if (Body.IsHuman)
            {
                if (Mounted)
                    return 29;
                return 20;
            }
            return 0;
        }

	    public override void OnPoisoned(Mobile from, Poison poison, Poison oldPoison)
        {
            if (poison != null)
            {
                string toMsg = string.Empty, fromMsg = string.Empty;
                if (poison.Level == 0)
                {
                    toMsg = "*You feel sickly*";
                    fromMsg = string.Format("*You see {0} looks sickly*", Name);
                }
                else if (poison.Level == 1)
                {
                    switch (Utility.Random(2))
                    {
                        case 0:
                            toMsg = "*You feel sickly*";
                            fromMsg = string.Format("*You see {0} looks sickly*", Name);
                            break;
                        case 1:
                            toMsg = "*You feel very ill*";
                            fromMsg = string.Format("*You see {0} looks very ill*", Name);
                            break;
                    }
                }
                else if (poison.Level == 2)
                {
                    toMsg = "*You feel very ill*";
                    fromMsg = string.Format("*You see {0} looks very ill*", Name);
                }
                else if (poison.Level == 3)
                {
                    switch (Utility.Random(2))
                    {
                        case 0:
                            toMsg = "*You feel very ill*";
                            fromMsg = string.Format("*You see {0} looks very ill*", Name);
                            break;
                        case 1:
                            toMsg = "*You feel extremely sick*";
                            fromMsg = string.Format("*You see {0} looks extremely sick*", Name);
                            break;
                    }
                }
                else
                {
                    toMsg = "*You feel very ill*";
                    fromMsg = string.Format("*You see {0} looks extremely sick*", Name);
                }

                LocalOverheadMessage(MessageType.Regular, 0x22, true, toMsg);
                NonlocalOverheadMessage(MessageType.Regular, 0x22, true, fromMsg);
            }
        }

        public override void Lift(Item item, int amount, out bool rejected, out LRReason reject)
        {
            rejected = true;
            reject = LRReason.Inspecific;

            if (item == null)
                return;

            Mobile from = this;
            NetState state = NetState;

            if (from.AccessLevel >= AccessLevel.GameMaster || DateTime.Now >= from.NextActionTime)
            {
                if (from.CheckAlive())
                {
                    from.DisruptiveAction();

                    if (from.Holding != null)
                    {
                        reject = LRReason.AreHolding;
                    }
                    else if (from.AccessLevel < AccessLevel.GameMaster && !from.InRange(item.GetWorldLocation(), 3))
                    {
                        reject = LRReason.OutOfRange;
                    }
                    else if (!from.CanSee(item) || !from.InLOS(item))
                    {
                        reject = LRReason.OutOfSight;
                    }
                    else if (!item.VerifyMove(from))
                    {
                        reject = LRReason.CannotLift;
                    }
                    else if (!item.IsAccessibleTo(from))
                    {
                        reject = LRReason.CannotLift;
                    }
                    else if (from.AccessLevel == AccessLevel.Player && (from.Frozen || from.Paralyzed))
                    {
                        reject = LRReason.CannotLift;
                    }
                    else if (!item.CheckLift(from, item, ref reject))
                    {
                    }
                    else
                    {
                        object root = item.RootParent;

                        bool canLoot;
                        if (root is Corpse)
                        {
                            Corpse corpse = (Corpse)root;
                            CustomRegion cs = Region.Find(corpse.Location, corpse.Map) as CustomRegion;

                            if (cs != null)
                            {
                                if (AccessLevel >= AccessLevel.GameMaster || (cs.Controller.CanLootOwnCorpse && cs.Controller.CanLootPlayerCorpse))
                                    canLoot = true;
                                else if (corpse.Owner == this)
                                    canLoot = cs.Controller.CanLootOwnCorpse;
                                else if (corpse.Owner is PlayerMobile)
                                    canLoot = cs.Controller.CanLootPlayerCorpse;
                                else
                                    canLoot = cs.Controller.CanLootNPCCorpse;
                            }
                            else
                                canLoot = true;
                        }
                        else
                            canLoot = true;

                        if (!canLoot)
                        {
                            SendAsciiMessage("You can't loot here.");
                            reject = LRReason.Inspecific;
                        }
                        else if (root != null && root is Mobile && !((Mobile) root).CheckNonlocalLift(from, item))
                        {
                            reject = LRReason.TryToSteal;
                        }
                        else if (!from.OnDragLift(item) || !item.OnDragLift(from))
                        {
                            reject = LRReason.Inspecific;
                        }
                        else if (!from.CheckAlive())
                        {
                            reject = LRReason.Inspecific;
                        }
                        else if (item.EventItem && !IsInEvent && AccessLevel < AccessLevel.GameMaster)
                        {
                            SendAsciiMessage("You can't use event items!");
                            reject = LRReason.Inspecific;
                        }
                        else if (!item.EventItem && IsInEvent && AccessLevel < AccessLevel.GameMaster)
                        {
                            SendAsciiMessage("You can only use event items!");
                            reject = LRReason.Inspecific;
                        }
                        else
                        {
                            item.SetLastMoved();

                            if (item.Spawner != null)
                            {
                                item.Spawner.Remove(item);
                                item.Spawner = null;
                            }

                            if (amount == 0)
                                amount = 1;

                            if (amount > item.Amount)
                                amount = item.Amount;

                            int oldAmount = item.Amount;
                            //item.Amount = amount; //Set in LiftItemDupe

                            if (amount < oldAmount)
                                LiftItemDupe(item, amount);
                            //item.Dupe( oldAmount - amount );

                            Map map = from.Map;

                            if (DragEffects && map != null && (root == null || root is Item))
                            {
                                IPooledEnumerable eable = map.GetClientsInRange(from.Location);
                                Packet p = null;

                                foreach (NetState ns in eable)
                                {
                                    if (ns.Mobile != from && ns.Mobile.CanSee(from) && (!ns.Mobile.HasFilter || InLOS(ns.Mobile)))
                                    {
                                        if (p == null)
                                        {
                                            IEntity src;

                                            if (root == null)
                                                src = new Entity(Serial.Zero, item.Location, map);
                                            else
                                                src = new Entity(((Item) root).Serial, ((Item) root).Location, map);

                                            p = Packet.Acquire(new DragEffect(src, from, item.ItemID, item.Hue, amount));
                                        }

                                        ns.Send(p);
                                    }
                                }

                                Packet.Release(p);

                                eable.Free();
                            }

                            Point3D fixLoc = item.Location;
                            Map fixMap = item.Map;
                            bool shouldFix = (item.Parent == null);

                            item.RecordBounce();
                            item.OnItemLifted(from, item);
                            item.Internalize();

                            from.Holding = item;

                            from.NextActionTime = DateTime.Now + TimeSpan.FromSeconds(0.4);

                            if (fixMap != null && shouldFix)
                                fixMap.FixColumn(fixLoc.X, fixLoc.Y);

                            reject = LRReason.Inspecific;
                            rejected = false;
                        }
                    }
                }
                else
                {
                    reject = LRReason.Inspecific;
                }
            }
            else
            {
                SendActionMessage();
                reject = LRReason.Inspecific;
            }

            if (rejected && state != null)
            {
                state.Send(new LiftRej(reject));

                if (item.Parent is Item)
                {
                    if (state.ContainerGridLines)
                        state.Send(new ContainerContentUpdate6017(item));
                    else
                        state.Send(new ContainerContentUpdate(item));
                }
                else if (item.Parent is Mobile)
                    state.Send(new EquipUpdate(item));
                else
                    item.SendInfoTo(state);

                if (ObjectPropertyList.Enabled && item.Parent != null)
                    state.Send(item.OPLPacket);
            }
        }

	    public bool Dye(Mobile from, DyeTub sender)
        {
            if (this != from)
                return false;

            return true;
        }
	    #endregion
    }
}
