using System.Collections.Generic;
using Server.ContextMenus;
using Server.Engines.Harvest;
using Server.Targets;

namespace Server.Items
{
    public abstract class BaseAxe : BaseMeleeWeapon, IUsesRemaining
    {
        public override int DefHitSound { get { return Utility.RandomList(0x236, 0x237); } }
        public override int DefMissSound { get { return Utility.RandomList(0x238, 0x239, 0x23A); } }

        public override SkillName DefSkill { get { return SkillName.Swords; } }
        public override WeaponType DefType { get { return WeaponType.Axe; } }
        //public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Slash2H; } }

        public override int GetSwingAnim(Mobile from)
        {
            if (from.Mounted)
                return 29;
            else
                return Utility.RandomList(12, 13, 14);
        }

        public virtual HarvestSystem HarvestSystem { get { return Lumberjacking.System; } }

        private int m_UsesRemaining;
        private bool m_ShowUsesRemaining;

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
            get { return m_UsesRemaining; }
            set { m_UsesRemaining = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ShowUsesRemaining
        {
            get { return m_ShowUsesRemaining; }
            set { m_ShowUsesRemaining = value; InvalidateProperties(); }
        }

        public virtual int GetUsesScalar()
        {
            if (Quality == WeaponQuality.Exceptional)
                return 200;

            return 100;
        }

        public override void UnscaleDurability()
        {
            base.UnscaleDurability();

            int scale = GetUsesScalar();

            m_UsesRemaining = ((m_UsesRemaining * 100) + (scale - 1)) / scale;
            InvalidateProperties();
        }

        public override void ScaleDurability()
        {
            base.ScaleDurability();

            int scale = GetUsesScalar();

            m_UsesRemaining = ((m_UsesRemaining * scale) + 99) / 100;
            InvalidateProperties();
        }

        public BaseAxe(int itemID)
            : base(itemID)
        {
            m_UsesRemaining = 150;
        }

        public BaseAxe(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Sphere.CanUse(from, this))
                return;

            base.OnDoubleClick(from);

            from.SendLocalizedMessage(1010018); // What do you want to use this item on?

            if (HarvestSystem == null)
                return;
            else
                from.Target = new BladedItemTarget(this);

            if (IsChildOf(from.Backpack) || Parent == from)
                HarvestSystem.BeginHarvesting(from, this);
            else
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (HarvestSystem != null)
                BaseHarvestTool.AddContextMenuEntries(from, this, list, HarvestSystem);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(2); // version

            writer.Write(m_ShowUsesRemaining);

            writer.Write(m_UsesRemaining);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        m_ShowUsesRemaining = reader.ReadBool();
                        goto case 1;
                    }
                case 1:
                    {
                        m_UsesRemaining = reader.ReadInt();
                        goto case 0;
                    }
                case 0:
                    {
                        if (m_UsesRemaining < 1)
                            m_UsesRemaining = 150;

                        break;
                    }
            }
        }

        public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
        {
            base.OnHit(attacker, defender, damageBonus);
            /* Loki: No more poison for axes
            CustomRegion cR = attacker.Region as CustomRegion; //Loki edit: Added for check below

            if (!Core.AOS && Poison != null && PoisonCharges > 0 && (cR == null || !(cR.Controller.LokiPvP && Layer == Layer.TwoHanded))) //Loki edit: Added limitation for LokiPvP
            {
                --PoisonCharges;

                if (Utility.RandomDouble() >= 0.5) // 50% chance to poison
                    defender.ApplyPoison(attacker, Poison);
            }
            */
            //if ( !Core.AOS && (attacker.Player || attacker.Body.IsHuman) && Layer == Layer.TwoHanded && (attacker.Skills[SkillName.Anatomy].Value / 400.0) >= Utility.RandomDouble() )
            //{
            //    StatMod mod = defender.GetStatMod( "Concussion" );

            //    if ( mod == null )
            //    {
            //        defender.SendMessage( "You receive a concussion blow!" );
            //        defender.AddStatMod( new StatMod( StatType.Int, "Concussion", -(defender.RawInt / 2), TimeSpan.FromSeconds( 30.0 ) ) );

            //        attacker.SendMessage( "You deliver a concussion blow!" );
            //        attacker.PlaySound( 0x308 );
            //    }
            //}
        }
    }
}