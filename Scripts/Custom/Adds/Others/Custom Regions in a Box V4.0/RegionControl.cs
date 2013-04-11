using System;
using System.Collections;
using System.Collections.Generic;
using Server.Gumps;
using Server.Regions;
using Server.Spells;

namespace Server.Items 
{
    public enum RegionFlag
    {
        None                =   0x00000000,
        AllowBenefitPlayer  =   0x00000001,
        AllowHarmPlayer     =   0x00000002,
        AllowHousing        =   0x00000004,
        AllowSpawn          =   0x00000008,

        CanBeDamaged        =   0x00000010,
        CanHeal             =   0x00000020,
        CanRessurect        =   0x00000040,
        CanUseStuckMenu     =   0x00000080,
        ItemDecay           =   0x00000100,

        ShowEnterMessage    =   0x00000200,
        ShowExitMessage     =   0x00000400,

        AllowBenefitNPC     =   0x00000800,
        AllowHarmNPC        =   0x00001000,

        CanMountEthereal    =   0x000002000,
        // ToDo: Change to "CanEnter"
        CanEnter            =   0x000004000,

        CanLootPlayerCorpse =   0x000008000,
        CanLootNPCCorpse    =   0x000010000,
        // ToDo: Change to "CanLootOwnCorpse"
        CanLootOwnCorpse    =   0x000020000,

        CanUsePotOthers     =   0x000040000,

        IsGuarded           =   0x000080000,

        // Obsolete, needed for old versions for DeSer.
        NoPlayerCorpses     =   0x000100000,
		NoItemDrop          =   0x000200000,
        //

        EmptyNPCCorpse      =   0x000400000,
        EmptyPlayerCorpse   =   0x000800000,
        DeleteNPCCorpse     =   0x001000000,
        DeletePlayerCorpse  =   0x002000000,
        ResNPCOnDeath       =   0x004000000,
        ResPlayerOnDeath    =   0x008000000,
        MoveNPCOnDeath      =   0x010000000,
        MovePlayerOnDeath   =   0x020000000,
        
        NoPlayerItemDrop    =   0x040000000,
        NoNPCItemDrop       =   0x00000001C
    }
    
	public class RegionControl : Item
	{
        private static List<RegionControl> m_AllControls = new List<RegionControl>();

        public static List<RegionControl> AllControls
        {
            get { return m_AllControls; }
        }

        #region Adds

	    private bool m_IsGuarded;
	    private bool m_MoveNPCOnDeath;
	    private bool m_MovePlayerOnDeath;
	    private Point3D m_GoLocation;
        private bool m_LogoutMove;
        private Point3D m_LogoutLoc;
        private Map m_LogoutMap;

        #endregion

        #region Region Flags

        
        private RegionFlag m_Flags;
        
        public RegionFlag Flags
        {
            get { return m_Flags; }
            set { m_Flags = value; }
        }
        
        public bool GetFlag(RegionFlag flag)
        {
            return ((m_Flags & flag) != 0);
        }

        public void SetFlag(RegionFlag flag, bool value)
        {
            if (value)
                m_Flags |= flag;
            else
            {     
                m_Flags &= ~flag;
            }
        }
        
	    [CommandProperty(AccessLevel.GameMaster)]
	    public bool AllowBenefitPlayer { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowHarmPlayer { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowHousing { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowSpawn { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanBeDamaged { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanMountEthereal { get; set; }

        // ToDo: Change to "CanEnter"
        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanEnter { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanHeal { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanRessurect { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanUseStuckMenu { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ItemDecay { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowBenefitNPC { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowHarmNPC { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ShowEnterMessage { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ShowExitMessage { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanLootPlayerCorpse { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanLootNPCCorpse { get; set; }

        // ToDo: Change to "CanLootOwnCorpse"
        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanLootOwnCorpse { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanUsePotHeal { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowTrade { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowPvP { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanUnshrink { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsGuarded
        {
            get { return m_IsGuarded; }
            set
            {
                m_IsGuarded = value;
                if (m_Region != null)
                    m_Region.Disabled = !value;

                Timer.DelayCall(TimeSpan.FromSeconds(2.0), new TimerCallback(UpdateRegion));
            }
        }

        // OBSOLETE, needed for old Deser
        public bool NoPlayerCorpses { get; set; }

        public bool NoItemDrop { get; set; }
        // END OBSOLETE

        [CommandProperty(AccessLevel.GameMaster)]
        public bool EmptyNPCCorpse { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool EmptyPlayerCorpse { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool DeleteNPCCorpse { get; set; }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public bool DeletePlayerCorpse { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ResNPCOnDeath { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ResPlayerOnDeath { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool MoveNPCOnDeath
        {
            get { return m_MoveNPCOnDeath; }
            set
            {
                if (MoveNPCToMap == null || MoveNPCToMap == Map.Internal || MoveNPCToLoc == Point3D.Zero)
                    m_MoveNPCOnDeath = false;
                else
                    m_MoveNPCOnDeath = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool MovePlayerOnDeath
        {
            get { return m_MovePlayerOnDeath; }
            set
            {
                if (MovePlayerToMap == null || MovePlayerToMap == Map.Internal || MovePlayerToLoc == Point3D.Zero)
                    m_MovePlayerOnDeath = false;
                else
                    m_MovePlayerOnDeath = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool NoPlayerItemDrop { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool NoNPCItemDrop { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D GoLocation
        {
            get { return m_GoLocation; }
            set
            {
                m_GoLocation = value;
                UpdateRegion();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool BroadcastArriveMsg { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool NoFameLoss { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IgnoreYoungProtection { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool FizzlePvP { get; set; }

        // Loki edit: Added for Loki's PvP changes
        [CommandProperty(AccessLevel.GameMaster)]
        public bool LokiPvP { get; set; }
        
        # endregion

        #region Adds

	    [CommandProperty(AccessLevel.GameMaster)]
	    public bool CanUsePotExplosion { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanUsePotOthers { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanUsePotMana { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanCutCorpse { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanUsePotStam { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanUsePotShrink { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool HasAttackPenalty { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanPlaceVendors { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowSpecialAttacks { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool LogoutMove
        {
            get { return m_LogoutMove; }
            set
            {
                if (m_LogoutLoc == Point3D.Zero || LogoutMap == null || LogoutMap == Map.Internal)
                    m_LogoutMove = false;
                else
                    m_LogoutMove = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map LogoutMap
        {
            get { return m_LogoutMap; }
            set
            {
                if (value != Map.Internal)
                    m_LogoutMap = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D LogoutLoc { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowSkillGain { get; set; }
        
        #endregion

        #region Region Restrictions

        private BitArray m_RestrictedSpells;
        private BitArray m_RestrictedSkills;

        public BitArray RestrictedSpells
        {
            get { return m_RestrictedSpells; }
        }

        public BitArray RestrictedSkills
        {
            get { return m_RestrictedSkills; }
        }

        # endregion


        # region Region Related Objects

        private CustomRegion m_Region;
        private Rectangle3D[] m_RegionArea;

        public CustomRegion Region
        {
            get { return m_Region; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Rectangle3D[] RegionArea
        {
            get { return m_RegionArea; }
            set { m_RegionArea = value; }
        }

        # endregion


        # region Control Properties

        private bool m_Active = true;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Active
        {
            get { return m_Active; }
            set
            {
                if (m_Active != value)
                {
                    m_Active = value;
                    UpdateRegion();
                }
            }

        }

        # endregion


        # region Region Properties

        private string m_RegionName;
        private int m_RegionPriority;
        private MusicName m_Music;
        private TimeSpan m_PlayerLogoutDelay;
        private int m_LightLevel;

        private Map m_MoveNPCToMap;
        private Point3D m_MoveNPCToLoc;
        private Map m_MovePlayerToMap;
        private Point3D m_MovePlayerToLoc;

        [CommandProperty(AccessLevel.GameMaster)]
        public string RegionName
        {
            get { return m_RegionName; }
            set 
            {
                if (Map != null)// && !RegionNameTaken(value))
                    m_RegionName = value;
                //else if (Map != null)
                //    Console.WriteLine("RegionName not changed for {0}, {1} already has a Region with the name of {2}", this, Map, value);
                else// if(Map == null)
                    Console.WriteLine("RegionName not changed for {0} to {1}, its Map value was null", this, value);

                UpdateRegion();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RegionPriority
        {
            get { return m_RegionPriority; }
            set 
            { 
                m_RegionPriority = value;
                UpdateRegion();           
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public MusicName Music
        {
            get { return m_Music; }
            set
            {
                m_Music = value;
                UpdateRegion();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
		public TimeSpan PlayerLogoutDelay
		{
			get{ return m_PlayerLogoutDelay; }
			set
            { 
                m_PlayerLogoutDelay = value;
                UpdateRegion();
            }
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public int LightLevel
        {
            get { return m_LightLevel; }
            set 
            { 
                m_LightLevel = value;
                UpdateRegion();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map MoveNPCToMap
        {
            get { return m_MoveNPCToMap; }
            set
            {
                if (value != Map.Internal)
                    m_MoveNPCToMap = value;
                else
                    MoveNPCOnDeath = false;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D MoveNPCToLoc
        {
            get { return m_MoveNPCToLoc; }
            set
            {
                if (value != Point3D.Zero)
                    m_MoveNPCToLoc = value;
                else
                    MoveNPCOnDeath = false;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map MovePlayerToMap
        {
            get { return m_MovePlayerToMap; }
            set
            {
                if (value != Map.Internal)
                    m_MovePlayerToMap = value;
                else
                    MovePlayerOnDeath = false;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D MovePlayerToLoc
        {
            get { return m_MovePlayerToLoc; }
            set
            {
                if (value != Point3D.Zero)
                    m_MovePlayerToLoc = value;
                else
                    MovePlayerOnDeath = false;
            }
        }

        # endregion


        [Constructable]
		public RegionControl() : base ( 5609 )
		{
			Visible = false;
			Movable = false;
			Name = "Region Controller";

            if (m_AllControls == null)
                m_AllControls = new List<RegionControl>();
            m_AllControls.Add(this);

            m_RegionName = FindNewName("Custom Region");
            m_RegionPriority = Server.Region.DefaultPriority;

            m_RestrictedSpells = new BitArray(SpellRegistry.Types.Length);
            m_RestrictedSkills = new BitArray(SkillInfo.Table.Length);

            SetDefaultValues();
		}

        [Constructable]
        public RegionControl(Rectangle2D rect): base(5609)
        {
            Visible = false;
            Movable = false;
            Name = "Region Controller";

            if (m_AllControls == null)
                m_AllControls = new List<RegionControl>();
            m_AllControls.Add(this);

            m_RegionName = FindNewName("Custom Region");
            m_RegionPriority = Server.Region.DefaultPriority;

            m_RestrictedSpells = new BitArray(SpellRegistry.Types.Length);
            m_RestrictedSkills = new BitArray(SkillInfo.Table.Length);

            Rectangle3D newrect = Server.Region.ConvertTo3D(rect);
            DoChooseArea(null, Map, newrect.Start, newrect.End, this);

            UpdateRegion();
        }

        [Constructable]
        public RegionControl(Rectangle3D rect): base(5609)
        {
            Visible = false;
            Movable = false;
            Name = "Region Controller";

            if (m_AllControls == null)
                m_AllControls = new List<RegionControl>();
            m_AllControls.Add(this);

            m_RegionName = FindNewName("Custom Region");
            m_RegionPriority = Server.Region.DefaultPriority;

            m_RestrictedSpells = new BitArray(SpellRegistry.Types.Length);
            m_RestrictedSkills = new BitArray(SkillInfo.Table.Length);

            DoChooseArea(null, Map, rect.Start, rect.End, this);

            UpdateRegion();
        }

        [Constructable]
        public RegionControl(Rectangle2D[] rects): base(5609)
        {
            Visible = false;
            Movable = false;
            Name = "Region Controller";

            if (m_AllControls == null)
                m_AllControls = new List<RegionControl>();
            m_AllControls.Add(this);

            m_RegionName = FindNewName("Custom Region");
            m_RegionPriority = Server.Region.DefaultPriority;

            m_RestrictedSpells = new BitArray(SpellRegistry.Types.Length);
            m_RestrictedSkills = new BitArray(SkillInfo.Table.Length);

            foreach (Rectangle2D rect2d in rects)
            {
                Rectangle3D newrect = Server.Region.ConvertTo3D(rect2d);
                DoChooseArea(null, Map, newrect.Start, newrect.End, this);
            }

            UpdateRegion();
        }

        [Constructable]
        public RegionControl(Rectangle3D[] rects): base(5609)
        {
            Visible = false;
            Movable = false;
            Name = "Region Controller";

            if (m_AllControls == null)
                m_AllControls = new List<RegionControl>();
            m_AllControls.Add(this);

            m_RegionName = FindNewName("Custom Region");
            m_RegionPriority = Server.Region.DefaultPriority;

            m_RestrictedSpells = new BitArray(SpellRegistry.Types.Length);
            m_RestrictedSkills = new BitArray(SkillInfo.Table.Length);

            foreach (Rectangle3D rect3d in rects)
            {
                DoChooseArea(null, Map, rect3d.Start, rect3d.End, this);
            }          

            UpdateRegion();
        }

        public virtual void SetDefaultValues()
        {
            AllowPvP = true;
            AllowTrade = true;
            CanUsePotHeal = true;
            CanUsePotMana = true;
            CanUsePotStam = true;
            CanUsePotShrink = true;
            CanCutCorpse = true;
            HasAttackPenalty = true;
            CanPlaceVendors = false;
            CanUnshrink = true;
            AllowSpecialAttacks = true;
            m_LogoutMove = false;
            AllowSkillGain = true;
            BroadcastArriveMsg = false;
            NoFameLoss = false;
            CanBeDamaged = true;
            AllowSpawn = true;
            CanUsePotExplosion = true;
            IgnoreYoungProtection = false;
            FizzlePvP = false;
            LokiPvP = false; //Loki edit
            m_PlayerLogoutDelay = TimeSpan.FromMinutes(2.0);

            AllowBenefitPlayer = true;
            AllowHarmPlayer = true;
            AllowHousing = true;
            AllowSpawn = true;
            CanBeDamaged = true;
            CanHeal = true;
            CanRessurect = true;
            CanUseStuckMenu = true;
            ItemDecay = true;
            ShowEnterMessage = false;
            ShowExitMessage = false;
            AllowBenefitNPC = true;
            AllowHarmNPC = true;
            CanMountEthereal = true;
            CanEnter = true;
            CanLootPlayerCorpse = true;
            CanLootNPCCorpse = true;
            CanLootOwnCorpse = true;
            CanUsePotOthers = true;
            IsGuarded = false;
            EmptyNPCCorpse = false;
            EmptyPlayerCorpse = false;
            DeleteNPCCorpse = false;
            DeletePlayerCorpse = false;
            ResNPCOnDeath = false;
            ResPlayerOnDeath = false;
            MoveNPCOnDeath = false;
            MovePlayerOnDeath = false;
            NoPlayerItemDrop = false;
            NoNPCItemDrop = false;

            /*
            SetFlag(RegionFlag.AllowBenefitPlayer, true);
            SetFlag(RegionFlag.AllowHarmPlayer, true);
            SetFlag(RegionFlag.AllowHousing, true);
            SetFlag(RegionFlag.AllowSpawn, true);
            SetFlag(RegionFlag.CanBeDamaged, true);
            SetFlag(RegionFlag.CanHeal, true);
            SetFlag(RegionFlag.CanRessurect, true);
            SetFlag(RegionFlag.CanUseStuckMenu, true);
            SetFlag(RegionFlag.ItemDecay, true);
            SetFlag(RegionFlag.ShowEnterMessage, false);
            SetFlag(RegionFlag.ShowExitMessage, false);
            SetFlag(RegionFlag.AllowBenefitNPC, true);
            SetFlag(RegionFlag.AllowHarmNPC, true);
            SetFlag(RegionFlag.CanMountEthereal, true);
            SetFlag(RegionFlag.CanEnter, true);
            SetFlag(RegionFlag.CanLootPlayerCorpse, true);
            SetFlag(RegionFlag.CanLootNPCCorpse, true);
            SetFlag(RegionFlag.CanLootOwnCorpse, true);
            SetFlag(RegionFlag.CanUsePotOthers, true);
            SetFlag(RegionFlag.IsGuarded, false);
            SetFlag(RegionFlag.EmptyNPCCorpse, false);
            SetFlag(RegionFlag.EmptyPlayerCorpse, false);
            SetFlag(RegionFlag.DeleteNPCCorpse, false);
            SetFlag(RegionFlag.DeletePlayerCorpse, false);
            SetFlag(RegionFlag.ResNPCOnDeath, false);
            SetFlag(RegionFlag.ResPlayerOnDeath, false);
            SetFlag(RegionFlag.MoveNPCOnDeath, false);
            SetFlag(RegionFlag.MovePlayerOnDeath, false);
            SetFlag(RegionFlag.NoPlayerItemDrop, false);
            SetFlag(RegionFlag.NoNPCItemDrop, false); 
            */
        }

		public RegionControl( Serial serial ) : base( serial )
		{
        }


        #region Control Special Voids

        public bool RegionNameTaken(string testName)
        {

            if (m_AllControls != null)
            {
                foreach (RegionControl control in m_AllControls)
                {
                    if (control.RegionName == testName && control != this)
                        return true;
                }
            }

            return false;
        }

        public string FindNewName(string oldName)
        {
            int i = 1;

            string newName = oldName;
            while( RegionNameTaken(newName) )
            {
                newName = oldName;
                newName += String.Format(" {0}", i);
                i++;
            }

            return newName;
        }

        public void UpdateRegion()
        {
            if (m_Region != null)
                m_Region.Unregister();

            if (Map != null && Active)
            {
                if (this != null && RegionArea != null && RegionArea.Length > 0)
                {
                    m_Region = new CustomRegion(this);
                    m_Region.GoLocation = m_GoLocation;
                    m_Region.Register();
                }
                else
                    m_Region = null;
            }
            else
                m_Region = null;
        }

        public void RemoveArea(int index, Mobile from)
        {
            try
            {
                List<Rectangle3D> rects = new List<Rectangle3D>();
                foreach (Rectangle3D rect in m_RegionArea)
                    rects.Add(rect);

                rects.RemoveAt(index);
                m_RegionArea = rects.ToArray();

                UpdateRegion();
                from.SendMessage("Area Removed!");
            }
            catch
            {
                from.SendMessage("Removing of Area Failed!");
            }
        }
        public static int GetRegistryNumber(ISpell s)
        {
            Type[] t = SpellRegistry.Types;

            for (int i = 0; i < t.Length; i++)
            {
                if (s.GetType() == t[i])
                    return i;
            }

            return -1;
        }


        public bool IsRestrictedSpell(ISpell s)
        {

            if (m_RestrictedSpells.Length != SpellRegistry.Types.Length)
            {

                m_RestrictedSpells = new BitArray(SpellRegistry.Types.Length);

                for (int i = 0; i < m_RestrictedSpells.Length; i++)
                    m_RestrictedSpells[i] = false;

            }

            int regNum = GetRegistryNumber(s);


            if (regNum < 0)	//Happens with unregistered Spells
                return false;

            return m_RestrictedSpells[regNum];
        }

        public bool IsRestrictedSkill(int skill)
        {
            if (m_RestrictedSkills.Length != SkillInfo.Table.Length)
            {

                m_RestrictedSkills = new BitArray(SkillInfo.Table.Length);

                for (int i = 0; i < m_RestrictedSkills.Length; i++)
                    m_RestrictedSkills[i] = false;

            }

            if (skill < 0)
                return false;

            if (skill > 48)
                return false;

            return m_RestrictedSkills[skill];
        }

        public void ChooseArea(Mobile m)
        {
            BoundingBoxPicker.Begin(m, CustomRegion_Callback, this);
        }

        public void CustomRegion_Callback(Mobile from, Map map, Point3D start, Point3D end, object state)
        {
            DoChooseArea(from, map, start, end, state);
        }

        public void DoChooseArea(Mobile from, Map map, Point3D start, Point3D end, object control)
        {
            if (this != null)
            {
                List<Rectangle3D> areas = new List<Rectangle3D>();
                
                if (m_RegionArea != null)
                {
                    foreach (Rectangle3D rect in m_RegionArea)
                        areas.Add(rect);
                }

                if (start.Z == end.Z || start.Z < end.Z)
                {
                    if (start.Z != Server.Region.MinZ)
                        --start.Z;
                    if (end.Z != Server.Region.MaxZ)
                        ++end.Z;
                }
                else
                {
                    if (start.Z != Server.Region.MaxZ)
                        ++start.Z;
                    if (end.Z != Server.Region.MinZ)
                    --end.Z;
                }

                Rectangle3D newrect = new Rectangle3D(start, end);
                areas.Add(newrect);

                m_RegionArea = areas.ToArray();

                UpdateRegion();
            }
        }

        # endregion


        #region Control Overrides

        public override void OnDoubleClick(Mobile m)
        {
            if (m.AccessLevel >= AccessLevel.GameMaster)
            {
                if (m_RestrictedSpells.Length != SpellRegistry.Types.Length)
                {
                    m_RestrictedSpells = new BitArray(SpellRegistry.Types.Length);

                    for (int i = 0; i < m_RestrictedSpells.Length; i++)
                        m_RestrictedSpells[i] = false;

                    m.SendMessage("Resetting all restricted Spells due to Spell change");
                }

                if (m_RestrictedSkills.Length != SkillInfo.Table.Length)
                {

                    m_RestrictedSkills = new BitArray(SkillInfo.Table.Length);

                    for (int i = 0; i < m_RestrictedSkills.Length; i++)
                        m_RestrictedSkills[i] = false;

                    m.SendMessage("Resetting all restricted Skills due to Skill change");

                }

                m.CloseGump(typeof(RegionControlGump));
                m.SendGump(new RegionControlGump(this));
                m.SendMessage("Don't forget to props this object for more options!");
                m.CloseGump(typeof(RemoveAreaGump));
                m.SendGump(new RemoveAreaGump(this));
            }
        }

		public override void OnMapChange()
		{
			UpdateRegion();
			base.OnMapChange();
		}

        public override void OnDelete()
        {
            if (m_Region != null)
                m_Region.Unregister();

            if (m_AllControls != null)
                m_AllControls.Remove(this);

            base.OnDelete();
        }

        # endregion


        #region Ser/Deser Helpers

        public static void WriteBitArray(GenericWriter writer, BitArray ba)
        {
            writer.Write(ba.Length);

            for (int i = 0; i < ba.Length; i++)
            {
                writer.Write(ba[i]);
            }
            return;
        }

        public static BitArray ReadBitArray(GenericReader reader)
        {
            int size = reader.ReadInt();

            BitArray newBA = new BitArray(size);

            for (int i = 0; i < size; i++)
            {
                newBA[i] = reader.ReadBool();
            }

            return newBA;
        }


        public static void WriteRect3DArray(GenericWriter writer, Rectangle3D[] ary)
        {
            if (ary == null)
            {
                writer.Write(0);
                return;
            }

            writer.Write(ary.Length);

            for (int i = 0; i < ary.Length; i++)
            {
                Rectangle3D rect = (ary[i]);
                writer.Write(rect.Start);
                writer.Write(rect.End);
            }
            return;
        }

        public static List<Rectangle2D> ReadRect2DArray(GenericReader reader)
        {
            int size = reader.ReadInt();
            List<Rectangle2D> newAry = new List<Rectangle2D>();

            for (int i = 0; i < size; i++)
            {
                newAry.Add(reader.ReadRect2D());
            }

            return newAry;
        }

        public static Rectangle3D[] ReadRect3DArray(GenericReader reader)
        {
            int size = reader.ReadInt();
            List<Rectangle3D> newAry = new List<Rectangle3D>();

            for (int i = 0; i < size; i++)
            {
                Point3D start = reader.ReadPoint3D();
                Point3D end = reader.ReadPoint3D();
                newAry.Add(new Rectangle3D(start,end));
            }

            return newAry.ToArray();
        }

        # endregion


        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 20 ); // version

            //20 - Loki edit

            writer.Write(LokiPvP);

            //19
            writer.Write(AllowBenefitPlayer);
            writer.Write(AllowHarmPlayer);
            writer.Write(AllowHousing);
            writer.Write(AllowSpawn);
            writer.Write(CanBeDamaged);
            writer.Write(CanHeal);
            writer.Write(CanRessurect);
            writer.Write(CanUseStuckMenu);
            writer.Write(ItemDecay);
            writer.Write(ShowEnterMessage);
            writer.Write(ShowExitMessage);
            writer.Write(AllowBenefitNPC);
            writer.Write(AllowHarmNPC);
            writer.Write(CanMountEthereal);
            writer.Write(CanEnter);
            writer.Write(CanLootPlayerCorpse);
            writer.Write(CanLootNPCCorpse);
            writer.Write(CanLootOwnCorpse);
            writer.Write(CanUsePotOthers);
            writer.Write(m_IsGuarded);
            writer.Write(EmptyNPCCorpse);
            writer.Write(EmptyPlayerCorpse);
            writer.Write(DeleteNPCCorpse);
            writer.Write(DeletePlayerCorpse);
            writer.Write(ResNPCOnDeath);
            writer.Write(ResPlayerOnDeath);
            writer.Write(m_MoveNPCOnDeath);
            writer.Write(m_MovePlayerOnDeath);
            writer.Write(NoPlayerItemDrop);
            writer.Write(NoNPCItemDrop);
            
            //18
            writer.Write(FizzlePvP);

            //17
            writer.Write(CanUsePotExplosion);
            writer.Write(IgnoreYoungProtection);
            
            //16
            writer.Write(m_LogoutMap);

            //15
            writer.Write(NoFameLoss);

            //14
            writer.Write(BroadcastArriveMsg);

            //13
            writer.Write(m_GoLocation);

            //12
            writer.Write(AllowSkillGain);

            //11
            writer.Write(m_LogoutMove);
            writer.Write(m_LogoutLoc);

            //10
            writer.Write(AllowSpecialAttacks);

            //9
            writer.Write(CanUnshrink);

            //8
            writer.Write(CanPlaceVendors);

            //7
            writer.Write(AllowPvP);

            //6
            writer.Write(AllowTrade);

            //5
            writer.Write( CanUsePotHeal );
            writer.Write( CanUsePotMana );
            writer.Write( CanUsePotStam );
            writer.Write( CanUsePotShrink);
            writer.Write(CanCutCorpse);
            writer.Write(HasAttackPenalty);

            WriteRect3DArray(writer, m_RegionArea);
            
            writer.Write((int)m_Flags);

            WriteBitArray(writer, m_RestrictedSpells);
            WriteBitArray(writer, m_RestrictedSkills);

            writer.Write(m_Active);

            writer.Write(m_RegionName);
            writer.Write(m_RegionPriority);
            writer.Write((int)m_Music);
            writer.Write(m_PlayerLogoutDelay);
            writer.Write(m_LightLevel);

            writer.Write(m_MoveNPCToMap);
            writer.Write(m_MoveNPCToLoc);
            writer.Write(m_MovePlayerToMap);
            writer.Write(m_MovePlayerToLoc); 
		}

		public override void Deserialize( GenericReader reader )
		{
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 20: //Loki edit
                    {
                        LokiPvP = reader.ReadBool();
                        goto case 19;
                    }
                case 19:
                    {
                        AllowBenefitPlayer = reader.ReadBool();
                        AllowHarmPlayer = reader.ReadBool();
                        AllowHousing = reader.ReadBool();
                        AllowSpawn = reader.ReadBool();
                        CanBeDamaged = reader.ReadBool();
                        CanHeal = reader.ReadBool();
                        CanRessurect = reader.ReadBool();
                        CanUseStuckMenu = reader.ReadBool();
                        ItemDecay = reader.ReadBool();
                        ShowEnterMessage = reader.ReadBool();
                        ShowExitMessage = reader.ReadBool();
                        AllowBenefitNPC = reader.ReadBool();
                        AllowHarmNPC = reader.ReadBool();
                        CanMountEthereal = reader.ReadBool();
                        CanEnter = reader.ReadBool();
                        CanLootPlayerCorpse = reader.ReadBool();
                        CanLootNPCCorpse = reader.ReadBool();
                        CanLootOwnCorpse = reader.ReadBool();
                        CanUsePotOthers = reader.ReadBool();
                        m_IsGuarded = reader.ReadBool();
                        EmptyNPCCorpse = reader.ReadBool();
                        EmptyPlayerCorpse = reader.ReadBool();
                        DeleteNPCCorpse = reader.ReadBool();
                        DeletePlayerCorpse = reader.ReadBool();
                        ResNPCOnDeath = reader.ReadBool();
                        ResPlayerOnDeath = reader.ReadBool();
                        m_MoveNPCOnDeath = reader.ReadBool();
                        m_MovePlayerOnDeath = reader.ReadBool();
                        NoPlayerItemDrop = reader.ReadBool();
                        NoNPCItemDrop = reader.ReadBool();
                        goto case 18;
                    }
                case 18:
                    {
                        FizzlePvP = reader.ReadBool();
                        goto case 17;
                    }
                case 17:
                    {
                        CanUsePotExplosion = reader.ReadBool();
                        IgnoreYoungProtection = reader.ReadBool();
                        goto case 16;
                    }
                case 16:
                    {
                        m_LogoutMap = reader.ReadMap();
                        goto case 15;
                    }
                case 15:
                    {
                        NoFameLoss = reader.ReadBool();
                        goto case 14;
                    }
                case 14:
                    {
                        BroadcastArriveMsg = reader.ReadBool();
                        goto case 13;
                    }
                case 13:
                    {
                        m_GoLocation = reader.ReadPoint3D();

                        goto case 12;
                    }
                case 12:
                    {
                        AllowSkillGain = reader.ReadBool();
                        goto case 11;
                    }
                case 11:
                    {
                        m_LogoutMove = reader.ReadBool();
                        m_LogoutLoc = reader.ReadPoint3D();
                        goto case 10;
                    }
                case 10:
                    {
                        AllowSpecialAttacks = reader.ReadBool();
                        goto case 9;
                    }
                case 9:
                    {
                        CanUnshrink = reader.ReadBool();
                        goto case 8;
                    }
                case 8:
                    {
                        CanPlaceVendors = reader.ReadBool();
                        goto case 7;
                    }
                case 7:
                    {
                        AllowPvP = reader.ReadBool();
                        goto case 6;
                    }
                case 6:
                    {
                        AllowTrade = reader.ReadBool();
                        goto case 5;
                    }
                case 5:
                    {
                        CanUsePotHeal = reader.ReadBool();
                        CanUsePotMana = reader.ReadBool();
                        CanUsePotStam = reader.ReadBool();
                        CanUsePotShrink = reader.ReadBool();
                        CanCutCorpse = reader.ReadBool();
                        HasAttackPenalty = reader.ReadBool();
                        goto case 4;
                    }
                // New RunUO 2.0 Version (case 4)
                case 4:
                {
                    m_RegionArea = ReadRect3DArray(reader);
                    
                    m_Flags = (RegionFlag)reader.ReadInt();

                    m_RestrictedSpells = ReadBitArray(reader);
                    m_RestrictedSkills = ReadBitArray(reader);

                    m_Active = reader.ReadBool();

                    m_RegionName = reader.ReadString();
                    m_RegionPriority = reader.ReadInt();
                    m_Music = (MusicName)reader.ReadInt();
                    m_PlayerLogoutDelay = reader.ReadTimeSpan();
                    m_LightLevel = reader.ReadInt();

                    m_MoveNPCToMap = reader.ReadMap();
                    m_MoveNPCToLoc = reader.ReadPoint3D();
                    m_MovePlayerToMap = reader.ReadMap();
                    m_MovePlayerToLoc = reader.ReadPoint3D();

                    break;
                }

                // Old RunUO 1.0 Version (cases 3-0)
                case 3:
                {
                    m_LightLevel = reader.ReadInt();
                    goto case 2;
                }
                case 2:
                {
                    m_Music = (MusicName)reader.ReadInt();
                    goto case 1;
                }
                case 1:
                {
                    List<Rectangle2D> rects2d = ReadRect2DArray(reader);
                    foreach (Rectangle2D rect in rects2d)
                    {
                        Rectangle3D newrect = Server.Region.ConvertTo3D(rect);
                        DoChooseArea(null, Map, newrect.Start, newrect.End, this);
                    }

                    m_RegionPriority = reader.ReadInt();
                    m_PlayerLogoutDelay = reader.ReadTimeSpan();

                    m_RestrictedSpells = ReadBitArray(reader);
                    m_RestrictedSkills = ReadBitArray(reader);

                    m_Flags = (RegionFlag)reader.ReadInt();

                    m_RegionName = reader.ReadString();
                    break;
                }
                case 0:
                {
                    List<Rectangle2D> rects2d = ReadRect2DArray(reader);
                    foreach (Rectangle2D rect in rects2d)
                    {
                        Rectangle3D newrect = Server.Region.ConvertTo3D(rect);
                        DoChooseArea(null, Map, newrect.Start, newrect.End, this);
                    }

                    m_RestrictedSpells = ReadBitArray(reader);
                    m_RestrictedSkills = ReadBitArray(reader);

                    m_Flags = (RegionFlag)reader.ReadInt();

                    m_RegionName = reader.ReadString();
                    break;
                }
            }

            m_AllControls.Add(this);

            //if(RegionNameTaken(m_RegionName))
            //    m_RegionName = FindNewName(m_RegionName);

            if (version < 19)
            {
                AllowBenefitPlayer = GetFlag(RegionFlag.AllowBenefitPlayer);
                AllowHarmPlayer = GetFlag(RegionFlag.AllowHarmPlayer);
                AllowHousing = GetFlag(RegionFlag.AllowHousing);
                AllowSpawn = GetFlag(RegionFlag.AllowSpawn);
                CanBeDamaged = GetFlag(RegionFlag.CanBeDamaged);
                CanHeal = GetFlag(RegionFlag.CanHeal);
                CanRessurect = GetFlag(RegionFlag.CanRessurect);
                CanUseStuckMenu = GetFlag(RegionFlag.CanUseStuckMenu);
                ItemDecay = GetFlag(RegionFlag.ItemDecay);
                ShowEnterMessage = GetFlag(RegionFlag.ShowEnterMessage);
                ShowExitMessage = GetFlag(RegionFlag.ShowExitMessage);
                AllowBenefitNPC = GetFlag(RegionFlag.AllowBenefitNPC);
                AllowHarmNPC = GetFlag(RegionFlag.AllowHarmNPC);
                CanMountEthereal = GetFlag(RegionFlag.CanMountEthereal);
                CanEnter = GetFlag(RegionFlag.CanEnter);
                CanLootPlayerCorpse = GetFlag(RegionFlag.CanLootPlayerCorpse);
                CanLootNPCCorpse = GetFlag(RegionFlag.CanLootNPCCorpse);
                CanLootOwnCorpse = GetFlag(RegionFlag.CanLootOwnCorpse);
                CanUsePotOthers = GetFlag(RegionFlag.CanUsePotOthers);
                IsGuarded = GetFlag(RegionFlag.IsGuarded);
                EmptyNPCCorpse = GetFlag(RegionFlag.EmptyNPCCorpse);
                EmptyPlayerCorpse = GetFlag(RegionFlag.EmptyPlayerCorpse);
                DeleteNPCCorpse = GetFlag(RegionFlag.DeleteNPCCorpse);
                DeletePlayerCorpse = GetFlag(RegionFlag.DeletePlayerCorpse);
                ResNPCOnDeath = GetFlag(RegionFlag.ResNPCOnDeath);
                ResPlayerOnDeath = GetFlag(RegionFlag.ResPlayerOnDeath);
                MoveNPCOnDeath = GetFlag(RegionFlag.MoveNPCOnDeath);
                MovePlayerOnDeath = GetFlag(RegionFlag.MovePlayerOnDeath);
                NoPlayerItemDrop = GetFlag(RegionFlag.NoPlayerItemDrop);
                NoNPCItemDrop = GetFlag(RegionFlag.NoNPCItemDrop);
            }

            UpdateRegion();
		}
	}
}
