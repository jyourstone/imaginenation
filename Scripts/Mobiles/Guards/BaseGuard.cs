using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public abstract class BaseGuard : BaseCreature
    {
        public enum GuardType
        {
            Standard,
            Regional,
            Colored
        }

        private static readonly SortedDictionary<string, CraftResource> m_RegionOreList = new SortedDictionary<string, CraftResource>();
        private CraftResource m_GuardTheme = CraftResource.None;
        private GuardType m_GuardType = GuardType.Colored;
        private bool m_Disappears = true;

        #region Fields

        public abstract Mobile Focus { get; set; }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.Owner)]
        public bool Disappears
        {
            get { return m_Disappears; }
            set { m_Disappears = value; }
        }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.Owner)]
        public CraftResource GuardTheme
        {
            get { return m_GuardTheme; }
            set
            {
                CraftResource oldValue = m_GuardTheme;
                m_GuardTheme = value;

                if (oldValue != m_GuardTheme)
                    ApplyGuardType();
            }
        }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.Owner)]
        public GuardType Type
        {
            get { return m_GuardType; }
            set
            {
                if (value == GuardType.Standard)
                    m_GuardTheme = CraftResource.None;

                m_GuardType = value;
            }
        }
        #endregion

        public static void Initialize()
        {
            m_RegionOreList.Add("Britain", CraftResource.Verite);
            m_RegionOreList.Add("Minoc", CraftResource.BloodRock);
            m_RegionOreList.Add("Trinsic", CraftResource.Gold);
            m_RegionOreList.Add("Moonglow", CraftResource.BlackRock);
            m_RegionOreList.Add("Serpent's Hold", CraftResource.Silver);
            m_RegionOreList.Add("Magincia", CraftResource.Valorite);
            m_RegionOreList.Add("Vespern", CraftResource.Mytheril);
        }

        public BaseGuard(Serial serial)  : base(serial)
        {
        }

        public BaseGuard(Mobile target)  : base(AIType.AI_GuardAI, FightMode.Aggressor, 10, 1, 0.15, 2)
        {
            if (target != null)
            {
                Location = target.Location;
                Map = target.Map;

                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);
            }

            GuardTheme = GetTownTheme();
        }

        public static void Spawn(Mobile caller, Mobile target)
        {
            Spawn(caller, target, 1, false);
        }

        public static void Spawn(Mobile caller, Mobile target, int amount, bool onlyAdditional)
        {
            if (target == null || target.Deleted)
                return;

            foreach (Mobile m in target.GetMobilesInRange(15))
            {
                if (m is BaseGuard)
                {
                    BaseGuard g = (BaseGuard)m;

                    if (g.Focus == null) // idling
                    {
                        g.Focus = target;

                        --amount;
                    }
                    else if (g.Focus == target && !onlyAdditional)
                    {
                        --amount;
                    }
                }
            }

            while (amount-- > 0)
                caller.Region.MakeGuard(target);
        }

        //Attack the guard and die!
        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (Focus == null)
                Focus = from;

            if (from is PlayerMobile)
                base.OnDamage(amount, from, willKill);
            else
                base.OnDamage(amount, from, true);
        }

        public void ApplyGuardType()
        {
            foreach (Layer i in Enum.GetValues(typeof(Layer)))
            {
                Item item = FindItemOnLayer(i);

                if (item != null)
                {
                    if (item is BaseClothing)
                        ((BaseClothing)item).Resource = m_GuardTheme;
                    else if (item is BaseArmor)
                        ((BaseArmor)item).Resource = m_GuardTheme;
                    else if (item is BaseWeapon)
                        ((BaseWeapon)item).Resource = m_GuardTheme;
                }
            }

            Title = string.Empty;


            //Regional guards are somewhat obsolete, cant think of anything to use them for 
            //now besides for the fact that the have a different name in "color less" regions
            if (m_GuardType == GuardType.Colored && m_GuardTheme != CraftResource.None)
                Name = string.Format("{0} Guard", CraftResources.GetName(m_GuardTheme));
            else
            {
                Region r = Region.Find(Location, Map);

                if (m_GuardTheme == CraftResource.None || m_GuardType == GuardType.Standard || r == null || string.IsNullOrEmpty(r.Name))
                {
                    Name = Female ? NameList.RandomName("female") : Name = NameList.RandomName("male");
                    Title = "the guard";
                }
                else
                    Name = string.Format("{0} Guard", r.Name);
            }
        }

        public CraftResource GetTownTheme()
        {
            return GetTownTheme(Location, Map);
        }

        public CraftResource GetTownTheme(Point3D location, Map map)
        {
            Region r = Region.Find(location, map);

            if (r == null || string.IsNullOrEmpty(r.Name))
                return CraftResource.None;

            //If our region name isn't in the list
            if (!m_RegionOreList.ContainsKey(r.Name))
                return CraftResource.None;

            return m_RegionOreList[r.Name];
        }

        public void OnFocusChanged(Mobile oldFocus, Mobile newFocus)
        {
        }

        public override bool OnBeforeDeath()
        {
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);

            PlaySound(0x1FE);

            Delete();

            return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(2); // version

            //Maka
            writer.Write((int)m_GuardTheme);
            writer.Write((int)m_GuardType);
            writer.Write(m_Disappears);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            //Maka
            m_GuardTheme = (CraftResource)reader.ReadInt();
            m_GuardType = (GuardType)reader.ReadInt();
            m_Disappears = reader.ReadBool();
        }
    }
}